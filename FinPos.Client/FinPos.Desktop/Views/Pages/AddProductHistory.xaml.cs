using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonEnums;
using FinPos.Utility.CommonMethods;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AddProductHistory.xaml
    /// </summary>
    public partial class AddProductHistory : Page
    {
        ProductController controller = new ProductController();
        CategoryController categoryController = new CategoryController();
        private string msg = string.Empty;
        private string header = "Product";
        private int _noOfErrorsOnScreen = 0;
        // private byte[] binaryImage;
        public ListViewItem item = null;
        private List<CategoryModel> _categories;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private int? categoryId = null;

        // private Bitmap DisplayImage;
        // private Bitmap CroppedImage;
        ///  private Graphics DisplayGraphics;
        // private System.Drawing.Point StartPoint, EndPoint;
        // private GridViewColumnHeader listViewSortCol = null;
        // private Sort listViewSortAdorner = null;
        private IList<ProductModel> _products;
        private OpeningStockController OpeningStockController = new OpeningStockController();
        private ObservableCollection<OpeningStockModel> OpeningStocks;
        BrushConverter color = new BrushConverter();
        public OpeningStockModel _selectedStock;
        public int rowIndex = 0;
        public AddProductHistory()
        {
            InitializeComponent();
            item_Type.ItemsSource = Enum.GetValues(typeof(CommonEnum.ItemTypes)).Cast<CommonEnum.ItemTypes>();
            item_Type.SelectedItem = (CommonEnum.ItemTypes)1;
            _categories = categoryController.GetCategories().Where(x => x.IsActive == true).ToList();
            btn_remove.Background = System.Windows.Media.Brushes.Gray;
            ResponseVm responce = controller.GetProducts();
            detail_header.Visibility = Visibility.Hidden;
            if (responce.FaultData == null)
            {
                _products = responce.Response.Cast<ProductModel>().ToList();
                if (_products != null && _products.Count > 0)
                {
                    dgProducts.ItemsSource = _products;
                    dgProducts.Visibility = Visibility.Visible;
                }
                else
                {
                    dgProducts.Visibility = Visibility.Collapsed;
                    brd_exp.Visibility = Visibility.Visible;
                }
            }

            if (OpeningStocks == null)
            {
                additems();
            }
            lvProductDetails.ItemsSource = OpeningStocks;
        }

        public void additems()
        {
            OpeningStocks = new ObservableCollection<OpeningStockModel>();
            OpeningStocks.Add(new OpeningStockModel() { ProductName = "" });
        }

        private void txt_cmpPhone_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            ke.Handled = (ke.Key == Key.Space) ? true : false;
        }

        private void txt_cmpPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CommonFunctions.DecimalValueChecker(sender, e);
        }

        private void txtEmail_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }

        private void purchase_Click(object sender, RoutedEventArgs e)
        {

        }

        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            if (lbAutoCat.Visibility == Visibility.Visible)
            {
                lbAutoCat.Visibility = Visibility.Collapsed;
            }
            else
            {
                ReusableCodeTemp("arrow_Click");
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            ReusableCodeTemp("TextBox_KeyUp");
        }

        private void category_code_LostFocus(object sender, RoutedEventArgs e)
        {
            bool isExsist = categoryController.GetCategories().Any(x => x.CategoryName.ToLower() == category_code.Text.ToLower());
            if (!isExsist)
            {
                category_code.Text = "";
            }
            lbAutoCat.Visibility = Visibility.Collapsed;
        }

        private void reorder_level_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void btn_uploadlogo_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {

                string fileName = System.IO.Path.GetFileName(dlg.FileName);
                string path = @"C:\POSDocuments";
                // Create directory temp if it doesn't exist
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string path2 = System.IO.Path.Combine(path, fileName);
                if (!File.Exists(path2))
                {
                    File.Copy(dlg.FileName, path2);
                }
                CompanyLogo.Source = new BitmapImage(new Uri(path2));
                //CompanyLogo.Source = b;
                CompanyDemoLogo.Visibility = Visibility.Hidden;
                CompanyLogo.Visibility = Visibility.Visible;
            }
        }

        private void btn_removelogo_Click(object sender, RoutedEventArgs e)
        {
            CompanyDemoLogo.Visibility = Visibility.Visible;
            CompanyLogo.Source = CompanyDemoLogo.Source;
            CompanyLogo.Visibility = Visibility.Hidden;
        }

        private void lbAutoCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbAutoCat.Visibility = Visibility.Collapsed;
            if (lbAutoCat.SelectedIndex != -1)
            {
                category_code.Text = lbAutoCat.SelectedItem.ToString();
            }
        }

        private void barcodeLogo_lostFocus(object sender, RoutedEventArgs e)
        {
            var response = controller.GetProducts().Response.Cast<ProductModel>().ToList().Any(x => x.BarCode.ToLower() == barcode_.Text.ToLower() && !string.IsNullOrEmpty(x.BarCode));
            if (response)
            {
                // var isExsist= responce.Any(x => x.BarCode.ToLower() == barcode_.Text.ToLower());
                //   if (isExsist)
                //  {
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["barcode_exeption"], header, false);
                form.ShowDialog();
                // Common.ErrorNotification((string)Application.Current.Resources["barcode_exeption"], header, false);
                barcode_.Text = string.Empty;
                // }
            }
        }

        private void ReusableCodeTemp(string callingMethod)
        {
            try
            {
                bool found = false;
                var data = _categories;
                string query = category_code.Text;
                lbAutoCat.Items.Clear();
                if (query.Length == 0)
                {
                    if (callingMethod == "arrow_Click")
                    {
                        foreach (var obj in data)
                        {
                            //if (Convert.ToString(obj.CategoryName).ToLower().StartsWith(query.ToLower()))
                            //{
                            addProductName(obj.CategoryName);
                            found = true;
                            //}
                        }
                        lbAutoCat.IsEnabled = true;
                        lbAutoCat.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lbAutoCat.IsEnabled = false;
                        lbAutoCat.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    foreach (var obj in data)
                    {
                        if (Convert.ToString(obj.CategoryName).ToLower().StartsWith(query.ToLower()))
                        {
                            addProductName(obj.CategoryName);
                            found = true;
                        }
                    }
                    lbAutoCat.IsEnabled = true;
                    lbAutoCat.Visibility = Visibility.Visible;
                }
                if (!found)
                {
                    lbAutoCat.Items.Add(new TextBlock() { Text = (string)Application.Current.Resources["no_result_alert"] });
                    lbAutoCat.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btn_addRow_Click(object sender, RoutedEventArgs e)
        {
            OpeningStocks.Add(new OpeningStockModel() { ProductName = "" });
        }

        public static DependencyObject GetAncestorByType(DependencyObject element, Type type)
        {
            if (element == null) return null;

            if (element.GetType() == type) return element;

            return GetAncestorByType(VisualTreeHelper.GetParent(element), type);

        }
        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            productPopUp.IsOpen = false;
            this.IsEnabled = true;
        }

        private void product_search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        public void stock_AddItem(ProductModel itemToAdd)
        {

            if (OpeningStocks.Any(x => x.ProductCode == itemToAdd.Id) && _selectedStock.ProductCode != itemToAdd.Id)
            {
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["purchase_already"], "OpeningStock", true);
                form.ShowDialog();
                if (Common._isChecked)
                {
                    AddItemSource(itemToAdd);
                }
            }
            else
            {
                AddItemSource(itemToAdd);
            }
            //OpeningStockModel _openingStock = new OpeningStockModel();
            //_openingStock.ProductName = itemToAdd.ItemName;
            //_openingStock.productCode = Convert.ToInt64(itemToAdd.Id);
            //_openingStock.CurrentStock = OpeningStockController.GetCurrentStockByProductCode(Convert.ToInt64(itemToAdd.Id));
            //OpeningStocks[rowIndex] = _openingStock;
            //lvOpeningStock.ItemsSource = OpeningStocks;
        }
        private void AddItemSource(ProductModel openingModel)
        {
            OpeningStockModel _openingStock = new OpeningStockModel();
            _openingStock.ProductName = openingModel.ItemName;
            _openingStock.productCode = Convert.ToInt64(openingModel.Id);
            _openingStock.CurrentStock = OpeningStockController.GetCurrentStockByProductCode(Convert.ToInt64(openingModel.Id));
            _openingStock.RetailPrice = openingModel.RetailPrice;
            OpeningStocks[rowIndex] = _openingStock;
            lvProductDetails.ItemsSource = OpeningStocks;
        }
        private void dgProducts_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProductModel productItem = (ProductModel)dgProducts.SelectedItem;
            productPopUp.IsOpen = false;
            this.IsEnabled = true;
            btn_remove.IsEnabled = false;
            btn_remove.Background = System.Windows.Media.Brushes.Gray;
            bool stockExists = OpeningStockController.CheckProductOpningStock(Convert.ToInt64(productItem.Id));
            if (!stockExists)
            {
                stock_AddItem(productItem);
            }
            else
            {

                ResourceDictionary myResourceDictionary = new ResourceDictionary();
                myResourceDictionary.Source =
                new Uri("/ResourceFiles/En.xaml",
                        UriKind.RelativeOrAbsolute);
                string errorMessage = (string)myResourceDictionary["error_message_false_product_selection_Opening_Stock"];

                //  ConfirmationPopup form = new ConfirmationPopup(errorMessage, header, false);
                //   form.ShowDialog();
                Common.ErrorNotification(errorMessage, header, false);
            }
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
        private Boolean IsTextAllowed(String text)
        {
            return Array.TrueForAll<Char>(text.ToCharArray(),
                delegate (Char c) { return Char.IsDigit(c) || Char.IsControl(c); });
        }
        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            if (productPopUp.IsOpen == true)
            {
                return;
            }
            OpeningStocks.Remove((OpeningStockModel)lvProductDetails.SelectedItem);
        }
        private void lvOpeningStock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            item = (sender as ListViewItem);
            btn_remove.IsEnabled = true;
            btn_remove.Background = (System.Windows.Media.Brush)color.ConvertFrom("#eb5151");
        }
        private void PreviewTextInputHandler(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void txtProduct_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            productPopUp.IsOpen = true;
            this.IsEnabled = false;
            btn_remove.IsEnabled = false;
            btn_remove.Background = System.Windows.Media.Brushes.Gray;
            //  this.ApplicationBar.IsVisible = false;
            //ProductListPopUp productList = new ProductListPopUp();
            //productList.ShowDialog();
            System.Windows.Controls.ListViewItem lvi = GetAncestorByType(
            e.OriginalSource as DependencyObject, typeof(System.Windows.Controls.ListViewItem)) as System.Windows.Controls.ListViewItem;
            if (lvi != null)
            {
                lvProductDetails.SelectedIndex =
                    lvProductDetails.ItemContainerGenerator.IndexFromContainer(lvi);
                rowIndex = lvProductDetails.SelectedIndex;
                _selectedStock = (OpeningStockModel)lvProductDetails.SelectedItem;
            }
        }
        //private void txtProductQuantity_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.IsEnabled = false;
        //    //System.Windows.Controls.ListViewItem lvi = GetAncestorByType(
        //    //e.OriginalSource as DependencyObject, typeof(System.Windows.Controls.ListViewItem)) as System.Windows.Controls.ListViewItem;
        //    //if (lvi != null)
        //    //{
        //       // lvProductDetails.SelectedIndex =
        //       //     lvProductDetails.ItemContainerGenerator.IndexFromContainer(lvi);
        //     //   rowIndex = lvProductDetails.SelectedIndex;
        //        _selectedStock = (OpeningStockModel)lvProductDetails.SelectedItem;
        //   // }
        //}

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }

        private void is_texinclusive_Unchecked(object sender, RoutedEventArgs e)
        {
            Handle(sender as CheckBox);
        }
        void Handle(CheckBox checkBox)
        {
            bool chkd = checkBox.IsChecked.Value;

            if (chkd)
            {
                taxPercentage_.Visibility = Visibility.Hidden;
                taxPercentage.Visibility = Visibility.Hidden;
                taxPercentage_.Text = string.Empty;
            }
            else
            {
                taxPercentage_.Visibility = Visibility.Visible;
                taxPercentage.Visibility = Visibility.Visible;
            }
        }

        private void item_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Kit) || Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Package))
            {
                detail_header.Visibility = Visibility.Visible;
                default_Tab.Visibility = Visibility.Visible;
                service_Charges.Visibility = Visibility.Hidden;
                detail_Grid.Visibility = Visibility.Visible;
            }
            else if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Service))
            {
                detail_header.Visibility = Visibility.Hidden;
                default_Tab.Visibility = Visibility.Visible;
                service_Charges.Visibility = Visibility.Visible;
                detail_Grid.Visibility = Visibility.Hidden;
                tabControl.SelectedIndex = 0;
            }
            else
            {
                detail_header.Visibility = Visibility.Hidden;
                default_Tab.Visibility = Visibility.Visible;
                service_Charges.Visibility = Visibility.Hidden;
                tabControl.SelectedIndex = 0;
                detail_Grid.Visibility = Visibility.Visible;
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            ProductModel model;
            decimal? nullval = null;
            int? integernull = null;
            if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Service))
            {
                if (string.IsNullOrEmpty(product_name.Text))
                {
                    ShowError((string)Application.Current.Resources["error_message_Tax"]);
                }
                model = new ProductModel(0, string.Empty, null, string.IsNullOrEmpty(service_Charge.Text) ? nullval : Convert.ToDecimal(service_Charge.Text), string.IsNullOrEmpty(trade_price.Text) ? nullval : Convert.ToDecimal(trade_price.Text), string.IsNullOrEmpty(wholeseller_price.Text) ? nullval : Convert.ToDecimal(wholeseller_price.Text), string.IsNullOrEmpty(reseller_price.Text) ? nullval : Convert.ToDecimal(reseller_price.Text), Convert.ToInt32(item_Type.SelectedItem), string.IsNullOrEmpty(weight_.Text) ? nullval : Convert.ToDecimal(weight_.Text), barcode_.Text, string.IsNullOrEmpty(taxPercentage_.Text) ? nullval : Convert.ToDecimal(taxPercentage_.Text), string.IsNullOrEmpty(minimum_level.Text) ? integernull : Convert.ToInt32(minimum_level.Text), string.IsNullOrEmpty(reorder_level.Text) ? integernull : Convert.ToInt32(reorder_level.Text), null, is_texinclusive.IsChecked.Value, shortname_.Text, description_.Text, UserModelVm.BranchId, string.Empty, string.Empty, null, CompanyLogo.Source.ToString(), UserModelVm.CompanyId, null);
                controller.SaveUpdateProduct(model);
                SuccessRetrun();
            }
            else
            {
                if (string.IsNullOrEmpty(product_name.Text) || string.IsNullOrEmpty(retail_price.Text) || string.IsNullOrEmpty(trade_price.Text) || string.IsNullOrEmpty(itemType.Text) || item_Type.SelectedIndex == -1)
                {
                   ShowError((string)Application.Current.Resources["error_message_Tax"]);
                }
                else if (is_texinclusive.IsChecked.Value == false && string.IsNullOrEmpty(taxPercentage_.Text))
                {
                    ShowError((string)Application.Current.Resources["taxPercentage_ErrorMsg"]);
                }
                else
                {
                    List<ProductItemContentModel> productItemContentModel = new List<ProductItemContentModel>();

                    if (!string.IsNullOrEmpty(category_code.Text))
                    {
                        categoryId = _categories.FirstOrDefault(x => x.CategoryName.ToLower() == category_code.Text.ToLower()).Id;
                    }
                    //binaryImage = Utility.CommonMethods.CommonFunctions.ImageToByteArray((BitmapImage)CompanyLogo.Source);
                    //string data = Convert.ToBase64String(binaryImage);
                    model = new ProductModel(0, product_name.Text, categoryId, string.IsNullOrEmpty(retail_price.Text) ? nullval : Convert.ToDecimal(retail_price.Text), string.IsNullOrEmpty(trade_price.Text) ? nullval : Convert.ToDecimal(trade_price.Text), string.IsNullOrEmpty(wholeseller_price.Text) ? nullval : Convert.ToDecimal(wholeseller_price.Text), string.IsNullOrEmpty(reseller_price.Text) ? nullval : Convert.ToDecimal(reseller_price.Text), Convert.ToInt32(item_Type.SelectedItem), string.IsNullOrEmpty(weight_.Text) ? nullval : Convert.ToDecimal(weight_.Text), barcode_.Text, string.IsNullOrEmpty(taxPercentage_.Text) ? nullval : Convert.ToDecimal(taxPercentage_.Text), string.IsNullOrEmpty(minimum_level.Text) ? integernull : Convert.ToInt32(minimum_level.Text), string.IsNullOrEmpty(reorder_level.Text) ? integernull : Convert.ToInt32(reorder_level.Text), null, is_texinclusive.IsChecked.Value, shortname_.Text, description_.Text, UserModelVm.BranchId, string.Empty, string.Empty, null, CompanyLogo.Source.ToString(), UserModelVm.CompanyId, null);
                    int parentProductId = controller.SaveUpdateProduct(model);
                    if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Kit) || Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Package))
                    {
                        List<OpeningStockModel> Stocks = lvProductDetails.Items.Cast<OpeningStockModel>().Select(x => x).ToList();
                        productItemContentModel.AddRange(Stocks.Where(z => z.ProductCode > 0 && z.Quantity > 0).Select(x =>
                        {
                            return new ProductItemContentModel(0, Convert.ToInt32(x.ProductCode), x.Quantity, parentProductId, 1, null, "", "");
                        }).ToList());
                        controller.SaveUpdateProductItemContent(productItemContentModel);
                    }
                    SuccessRetrun();
                }
            }
        }

        private void ShowError(string msg)
        {
            ConfirmationPopup form = new ConfirmationPopup(msg, header, false);
            form.ShowDialog();
        }
        private void SuccessRetrun()
        {
            Common.Notification((string)Application.Current.Resources["product_saveSuccessMsg"], header, false);
            ClearFields();
            inventory form1 = new inventory();
            NavigationService.Navigate(form1);
        }
        private void ClearFields()
        {
            product_name.Text = "";
            category_code.Text = "";
            retail_price.Text = "";
            trade_price.Text = "";
            wholeseller_price.Text = "";
            reseller_price.Text = "";
            weight_.Text = "";
            barcode_.Text = "";
            taxPercentage_.Text = "";
            minimum_level.Text = "";
            reorder_level.Text = "";
            //itemImage.Text = "";
            shortname_.Text = "";
            description_.Text = "";
        }
        private void addProductName(string text)
        {
            lbAutoCat.Items.Add(text);
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            inventory form = new inventory();
            NavigationService.Navigate(form);
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    OpeningStockModel customer = (OpeningStockModel)lvProductDetails.SelectedItem;
        //}
    }
}
