using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonMethods;
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
    /// Interaction logic for AddWastage.xaml
    /// </summary>
    public partial class AddWastage : Page
    {
        #region Properties
        private ProductController controller = new ProductController();
        private int _noOfErrorsOnScreen = 0;
        private string msg = string.Empty;
        private string header = "AddWastage";
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private OpeningStockController OpeningStockController = new OpeningStockController();
        private int productCode;
        #endregion

        #region Constructor
        public AddWastage()
        {
            InitializeComponent();
            ChangeHeightWidth();
            ClearFields();
            txtDate.Text = CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString());
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }
        #endregion

        #region Common Methods
        public void ChangeHeightWidth()
        {
            this.AddWastagePage.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.AddWastagePage.Width = HeightWidth.width;

        }
        private void NavigateToParent()
        {
            Wastage _wastage = new Wastage();
            NavigationService.Navigate(_wastage);
        }
        private void ClearFields()
        {
            txtName.Text = "";
            txtBatchNo.Text = "";
            txtQuantity.Text = "";
            txt_Reason.Text = "";
            txtDate.Text = "";
        }
        /// <summary>
        /// this method is created for setting values in Product searchable textbox
        /// this method is called at two diffrent places one at arrow click which will populate all products if query serach will be empty
        /// </summary>
        /// <param name="sender"></param>
        private void ReusableCodeTemp(string callingMethod)
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
                        addProductName(obj.ItemName, obj.Id.Value);
                        found = true;
                    }
                    border.Visibility = Visibility.Visible;
                    resultStack.Visibility = Visibility.Visible;
                }
                else
                {
                    border.Visibility = Visibility.Collapsed;
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
            }
            if (!found)
            {
                resultStack.Children.Add(new TextBlock() { Text = "No results found." });
            }
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
            resultStack.Children.Add(block);
        }
        #endregion
      
        #region Events
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (productCode != 0)
            {
                if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtQuantity.Text) || string.IsNullOrEmpty(txtDate.Text) || string.IsNullOrEmpty(txt_Reason.Text))
                    Common.ErrorMessage((string)Application.Current.Resources["wastage_popupquantityerrormsgRequiredFields"], header);
                else if (Convert.ToInt64(txtQuantity.Text) < 1)
                    Common.ErrorMessage((string)Application.Current.Resources["wastage_popupquantityerrormsgForZeroQuantity"], header);
                else if (Convert.ToInt64(txtQuantity.Text) > OpeningStockController.GetCurrentStockByProductCode(Convert.ToInt64(productCode)))
                {
                    Common.ErrorMessage((string)Application.Current.Resources["wastage_popupquantityerrormsg"], header);
                    txtQuantity.Text = string.Empty;
                }
                else
                {
                    try
                    {
                        #region Save
                        ProductController controller = new ProductController();
                        WastageModel model = new WastageModel(0, productCode, txtName.Text, Convert.ToInt32(txtQuantity.Text), txtDate.Text, txt_Reason.Text, txtBatchNo.Text, UserModelVm.BranchId, UserModelVm.CompanyId);
                        controller.SaveUpdateWastage(model);
                        Common.Notification((string)Application.Current.Resources["wastage_SavedSuccessMsg"], header, false);
                        ClearFields();
                        NavigateToParent();
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Common.ErrorMessage(ex.Message, header);
                    }
                }
            }
            else
            {
                Common.ErrorMessage((string)Application.Current.Resources["errorInvalidProductSelection"], header);
            }
        }
        private void txtEmail_Error(object sender, System.Windows.Controls.ValidationErrorEventArgs e)
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

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            NavigateToParent();
        }
        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            ReusableCodeTemp("txtName_KeyUp");
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

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var border = (resultStack.Parent as ScrollViewer).Parent as Border;
            resultStack.Children.Add(new TextBlock() { Text = "No results found." });
            resultStack.Visibility = Visibility.Collapsed;
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
        #endregion
    }
}
