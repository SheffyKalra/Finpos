using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.Constants;
using NLog;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for EditWastage.xaml
    /// </summary>
    public partial class EditWastage : Page
    {
        private ProductController controller = new ProductController();
        public int RowId;
        public dynamic seletecRow;
        public bool IsImageUpload = false;
        public bool IsImageExist = false;
        private int _noOfErrorsOnScreen = 0;
        private string msg = string.Empty;
        private string header = "Wastage";
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private OpeningStockController OpeningStockController = new OpeningStockController();
        private int productCode;
        public EditWastage(dynamic row)
        {
            InitializeComponent();
            ChangeHeightWidth();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            seletecRow = row;
            txtName.Text = row.ProductName;
            productCode = row.ItemCode;
            txtBatchNo.Text = row.BatchNo;
            txtQuantity.Text = Convert.ToString(row.Quantity);
            txt_Reason.Text = row.Reason;
            txtDate.Text = Convert.ToString(row.Date);
            RowId = row.WastageId;

        }
        public void ChangeHeightWidth()
        {
            this.EditWastagePage.Height = HeightWidth.Height - 65;
            this.EditWastagePage.Width = HeightWidth.width;

        }
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            ReusableCodeTemp("TextBox_KeyUp");
        }
        private void addProductName(string text, int code)
        {
            var border = (resultStack.Parent as ScrollViewer).Parent as Border;
            TextBlock block = new TextBlock();
            block.Text = text;
            block.Margin = new Thickness(2, 3, 2, 3);
            block.Cursor = Cursors.Hand;
            block.MouseLeftButtonUp += (sender, e) =>
            {

                txtName.Text = (sender as TextBlock).Text;
                productCode = code;
                border.Visibility = Visibility.Collapsed;
            };

            block.MouseEnter += (sender, e) =>
            {
                BrushConverter bc = new BrushConverter();
                TextBlock b = sender as TextBlock;
                b.Background = (Brush)bc.ConvertFrom("#fff");
            };

            block.MouseLeave += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.Transparent;
            };

            // Add to the panel 
            resultStack.Children.Add(block);
        }

        //private void addProductCode(string text, int code)
        //{
        //    var border = (resultStackForCode.Parent as ScrollViewer).Parent as Border;
        //    TextBlock block = new TextBlock();

        //    block.Text = Convert.ToString(code);
        //    block.Margin = new Thickness(2, 3, 2, 3);
        //    block.Cursor = Cursors.Hand;

        //    block.MouseLeftButtonUp += (sender, e) =>
        //    {

        //        txt_ProductCode.Text = (sender as TextBlock).Text;
        //        txtName.Text = text;
        //        border.Visibility = Visibility.Collapsed;

        //    };

        //    block.MouseEnter += (sender, e) =>
        //    {
        //        TextBlock b = sender as TextBlock;
        //        b.Background = Brushes.PeachPuff;
        //    };

        //    block.MouseLeave += (sender, e) =>
        //    {
        //        BrushConverter bc = new BrushConverter();
        //        TextBlock b = sender as TextBlock;
        //        b.Background = (Brush)bc.ConvertFrom(CommonConstants._purpleColorCode);
        //    };

        //               resultStackForCode.Children.Add(block);
        //}
        //private void txt_ProductCode_KeyUp(object sender, KeyEventArgs e)
        //{
        //    bool found = false;
        //    var border = (resultStackForCode.Parent as ScrollViewer).Parent as Border;
        //    ResponseVm responce = controller.GetProducts();
        //    var data = responce.Response.Cast<ProductModel>().ToList();
        //    string query = (sender as TextBox).Text;

        //    if (query.Length == 0)
        //    {
        //        resultStackForCode.Children.Clear();
        //        border.Visibility = System.Windows.Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        border.Visibility = System.Windows.Visibility.Visible;
        //    }
        //    resultStackForCode.Children.Clear();

        //    foreach (var obj in data)
        //    {
        //        if (Convert.ToString(obj.ItemCode).ToLower().StartsWith(query.ToLower()))
        //        {
        //            addProductCode(obj.ItemName, obj.ItemCode);
        //            found = true;
        //        }
        //    }

        //    if (!found)
        //    {
        //        resultStackForCode.Children.Add(new TextBlock() { Text = "No results found." });
        //    }
        //}

        private void txtEmail_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }

        private void AddCustomer_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (productCode != 0)
            {
                int currentStock = OpeningStockController.GetCurrentStockByProductCode(productCode);
                if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtQuantity.Text) || string.IsNullOrEmpty(txtDate.Text))
                {
                    //  msg = "Fill all required fields";
                    ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["error_message_Tax"], header, false);
                    form.ShowDialog();
                    // Common.ErrorNotification((string)Application.Current.Resources["error_message_Tax"], header, false);
                }
                else if (Convert.ToInt64(txtQuantity.Text) < 1)
                {
                    ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["wastage_popupquantityerrormsgForZeroQuantity"], header, false);
                    form.ShowDialog();
                    //  Common.ErrorNotification((string)Application.Current.Resources["wastage_popupquantityerrormsg"], header, false);

                    //txtQuantity.Text = string.Empty;
                }
                else if (Convert.ToInt64(txtQuantity.Text) > currentStock)
                {
                    ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["wastage_popupquantityerrormsg"], header, false);
                    form.ShowDialog();
                    //  Common.ErrorNotification((string)Application.Current.Resources["wastage_popupquantityerrormsg"], header, false);
                }
                else
                {
                    try
                    {
                        WastageModel wastageModel = new WastageModel(RowId, productCode, txtName.Text, Convert.ToInt32(txtQuantity.Text), txtDate.Text, txt_Reason.Text, txtBatchNo.Text, UserModelVm.BranchId, UserModelVm.CompanyId);
                        controller.SaveUpdateWastage(wastageModel);
                        Common.Notification((string)Application.Current.Resources["wastage_UpdatedSuccessMsg"], header, false);
                        ClearFields();
                        GoToBackPage();

                    }
                    catch (Exception ex)
                    {
                        ConfirmationPopup form = new ConfirmationPopup(ex.Message, header, false);
                        form.ShowDialog();
                        // Common.ErrorNotification(ex.Message, header, false);
                    }
                }
            }
            else
            {
                Common.ErrorMessage((string)Application.Current.Resources["errorInvalidProductSelection"], header);
            }
        }

        private void ClearFields()
        {
            txtName.Text = "";
            txtBatchNo.Text = "";
            txtQuantity.Text = "";
            txt_Reason.Text = "";
            txtDate.Text = "";
        }
        private void GoToBackPage()
        {
            Wastage _wastage = new Wastage();
            NavigationService.Navigate(_wastage);

        }
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            GoToBackPage();
        }

        private void txtQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error(e.ExceptionObject);
        }

        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            var border = (resultStack.Parent as ScrollViewer).Parent as Border;
            if (border.Visibility == Visibility.Visible)
            {
                border.Visibility = Visibility.Collapsed;
            }
            else
            {
                ReusableCodeTemp("arrow_Click");
            }
        }
        private void ReusableCodeTemp(string callingMethod)
        {
            try
            {
                bool found = false;
                var border = (resultStack.Parent as ScrollViewer).Parent as Border;
                ResponseVm responce = controller.GetProductsByCompanyAndBranch();
                var data = responce.Response.Cast<ProductModel>().ToList();
                resultStack.Children.Clear();
                string query = (txtName as TextBox).Text;

                if (query.Length == 0)
                {
                    if (callingMethod == "arrow_Click")
                    {
                        foreach (var obj in data)
                        {
                            //if (obj.ItemName.ToLower().StartsWith(query.ToLower()))
                            //{
                            addProductName(obj.ItemName, obj.Id.Value);
                            found = true;
                            //}
                        }
                        //resultStack.Children.Clear();
                        border.Visibility = Visibility.Visible;
                        resultStack.Visibility = Visibility.Visible;
                        resultStack.IsEnabled = true;
                    }
                    else
                    {
                        border.Visibility = Visibility.Collapsed;
                        resultStack.IsEnabled = false;
                    }
                }
                else
                {
                    foreach (var obj in data)
                    {
                        if (obj.ItemName.ToLower().StartsWith(query.ToLower()))
                        {
                            addProductName(obj.ItemName, obj.Id.Value);
                            found = true;
                        }
                    }
                    border.Visibility = Visibility.Visible;
                    resultStack.Visibility = Visibility.Visible;
                    resultStack.IsEnabled = true;
                }
                if (!found)
                {
                    resultStack.IsEnabled = false;
                    resultStack.Children.Add(new TextBlock() { Text = "No results found." });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var border = (resultStack.Parent as ScrollViewer).Parent as Border;
            resultStack.Children.Add(new TextBlock() { Text = (string)Application.Current.Resources["no_result_alert"] });
            resultStack.IsEnabled = false;
            border.Visibility = Visibility.Collapsed;


        }

        private void txtQuantity_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            ke.Handled = (ke.Key == Key.Space) ? true : false;
        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            Grid_MouseUp(null, null);
            if (txtName.Text != string.Empty)
            {
                var productId = controller.GetProductIdByName(txtName.Text);
                if (productId != null)
                    productCode = Convert.ToInt32(productId);
                else
                    productCode = 0;
            }
        }

        private void txtQuantity_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var isNumericValue = Common.isNumeric(textBox.Text, System.Globalization.NumberStyles.Integer);
            if (!isNumericValue)
                textBox.Text = string.Empty;
        }
    }
}
