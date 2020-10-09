using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonEnums;
using FinPos.Utility.CommonMethods;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using ZXing;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for EditProductHistory.xaml
    /// </summary>
    public partial class EditProductHistory : Page
    {
        ProductController controller = new ProductController();
        CategoryController categoryController = new CategoryController();
        PurchaseController purchaseController = new PurchaseController();
        private List<StockModelVM> _purchases;
        private string msg = string.Empty;
        private string header = "Product";
        private int _noOfErrorsOnScreen = 0;
        private byte[] binaryImage;
        private List<CategoryModel> _categories;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private int? categoryId = null;
        private IList<ProductModel> _products;
        private OpeningStockController OpeningStockController = new OpeningStockController();
        private ObservableCollection<OpeningStockModel> OpeningStocks;
        BrushConverter color = new BrushConverter();
        public OpeningStockModel _selectedStock;
        public int rowIndex = 0;
        public int RowId;
        public string barCodepresent;
        public ListViewItem item = null;
        List<ProductItemContentModel> productItemContents = null;
        List<SubProductItemModel> subProductItem = null;
        List<ProductItemContentModel> _deletestocks = new List<ProductItemContentModel>();
        List<SubProductItemModel> _deleteSubProductItemModel = new List<SubProductItemModel>();
        private IList<TaxModel> _taxdetails;
        private string _taxValue, fileName;
        private bool productNameExists = false;
        TaxController taxController = new TaxController();
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        public EditProductHistory(dynamic row)
        {
            InitializeComponent();
            // histroy_calender.d
            bool isExist = controller.IsProductExistInRepack(row.Id);
            lblProduct.Content = "Edit (" + row.ItemName + ")";
            if (isExist)
            {
                ClearFields();
                DefaultFields();
                //  inventory form = new inventory();
                //  NavigationService.Navigate(form);
                Common.ErrorMessage((string)Application.Current.Resources["product_alreadyInUsedMsg"], header);
            }
            ChangeHeightWidth();
            ResponseVm responseTax = taxController.GetTax();
            if (responseTax.FaultData == null)
            {
                TaxModel objTaxModel = new TaxModel(0, "Select", 0, "", "", "", "");
                _taxdetails = responseTax.Response.Cast<TaxModel>().ToList();
                _taxdetails.Insert(0, objTaxModel);
                this.cmbTax.ItemsSource = _taxdetails;
                this.cmbTax.DisplayMemberPath = "TaxDetail";
                this.cmbTax.SelectedValuePath = "TaxCode";
                // this.cmbTax.SelectedIndex = 0;
            }
            _taxValue = Convert.ToString(row.TaxPercentage);
            if (row.TaxPercentage != null && Convert.ToDecimal(row.TaxPercentage) != Convert.ToDecimal(0) && _taxdetails.Any(item => Convert.ToDouble(item.Rate) == Convert.ToDouble(_taxValue)))
            {
                _taxValue = Convert.ToString(row.TaxPercentage);
                cmbTax.Text = Convert.ToString(_taxdetails.FirstOrDefault(item => item.Rate == Convert.ToDouble(row.TaxPercentage)).TaxDetail);
            }
            else
            {
                _taxValue = Convert.ToString(row.TaxPercentage);
                cmbTax.SelectedIndex = 0;
            }
            minimum_level.Text = Convert.ToString(row.MinimumLevel);
            reorder_level.Text = Convert.ToString(row.ReOrderLevel);
            ResponseVm responceProduct = controller.GetProductsByCompanyAndBranch();
            productItemContents = controller.GetProductItemContentById(row.Id);
            subProductItem = controller.GetSubProductItemById(row.Id);
            if (responceProduct.FaultData == null)
            {
                lvPurchase.Visibility = Visibility.Hidden;
                _categories = categoryController.GetCategoriesByCompanyId().Where(x => x.IsActive == true).ToList();
                product_name.Text = row.ItemName;
                category_code.Text = row.CategoryName;
                wholeseller_price.Text = Convert.ToString(row.WholeSellerPrice);
                reseller_price.Text = Convert.ToString(row.ResellerPrice);
                item_Type.ItemsSource = Enum.GetValues(typeof(CommonEnum.ItemTypes)).Cast<CommonEnum.ItemTypes>();
                CommonEnum.ItemTypes itemType = (CommonEnum.ItemTypes)row.ItemType;
                item_Type.SelectedItem = itemType;
                item_Type.IsEnabled = false;
                weight_.Text = Convert.ToString(row.Weight);
                barcode_.Text = row.BarCode;
                barCodepresent = row.BarCode;
                if (barcode_.Text != "")
                {
                    service_Barcode.Text = row.BarCode;
                    ChangeServiceBarcodeImage();
                }
                // taxPercentage_.Text = Convert.ToString(row.TaxPercentage);
                minimum_level.Text = Convert.ToString(row.MinimumLevel);
                reorder_level.Text = Convert.ToString(row.ReOrderLevel);
                ResponseVm responce = controller.GetProductsByCompanyAndBranch();
                productItemContents = controller.GetProductItemContentById(row.Id);
                subProductItem = controller.GetSubProductItemById(row.Id);
                if (responce.FaultData == null)
                {
                    _products = responce.Response.Cast<ProductModel>().ToList();
                    if (_products != null && _products.Count > 0)
                    {
                        dgProducts.ItemsSource = _products;
                        dgProductsBulk.ItemsSource = _products.Where(x => x.ItemType == Convert.ToInt32(CommonEnum.ItemTypes.Bulk));
                        dgProductsBulk.Visibility = Visibility.Visible;
                        dgProducts.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        dgProducts.Visibility = Visibility.Collapsed;
                        brd_exp.Visibility = Visibility.Visible;
                        brd_expBulk.Visibility = Visibility.Visible;
                    }
                }
                if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Package))
                {
                    BindCompanyLogo(row);
                    ItemTypeEnabledDisabled();
                    //detail_header.Visibility = Visibility.Visible;
                    //service_Charges.Visibility = Visibility.Hidden;
                    //detail_Grid.Visibility = Visibility.Visible;
                    //bulkCode.Visibility = Visibility.Hidden;
                    //bulk_Code.Visibility = Visibility.Hidden;
                    //childItems.IsEnabled = false;
                    //childItems.Visibility = Visibility.Collapsed;
                    lvProductDetailwithFrreProduct.Visibility = Visibility.Visible;
                    lvProductDetails.Visibility = Visibility.Collapsed;
                    List<OpeningStockModel> purchaseStockData = productItemContents.Select(x =>
                    {
                        ProductModel product = GetProductName(x.ChildProductId);
                        return new OpeningStockModel() { ProductCode = x.ChildProductId, ProductContentId = x.Id, ProductName = product.ItemName, Quantity = x.Quantity, RetailPrice = product.RetailPrice, TotalPrice = x.Quantity * product.RetailPrice, isFreeProduct = x.IsFreeProduct };
                    }).ToList();
                    OpeningStocks = new ObservableCollection<OpeningStockModel>(purchaseStockData);
                }
                else if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Kit))
                {
                    BindCompanyLogo(row);
                    //detail_header.Visibility = Visibility.Visible;
                    //service_Charges.Visibility = Visibility.Hidden;
                    //detail_Grid.Visibility = Visibility.Visible;
                    //bulkCode.Visibility = Visibility.Hidden;
                    //bulk_Code.Visibility = Visibility.Hidden;
                    //childItems.IsEnabled = false;
                    //childItems.Visibility = Visibility.Collapsed;
                    ItemTypeEnabledDisabled();
                    lvProductDetailwithFrreProduct.Visibility = Visibility.Collapsed;
                    lvProductDetails.Visibility = Visibility.Visible;
                    List<OpeningStockModel> purchaseStockData = productItemContents.Select(x =>
                    {
                        ProductModel product = GetProductName(x.ChildProductId);
                        return new OpeningStockModel() { ProductCode = x.ChildProductId, ProductContentId = x.Id, ProductName = product.ItemName, Quantity = x.Quantity, RetailPrice = product.RetailPrice, TotalPrice = x.Quantity * product.RetailPrice };
                    }).ToList();
                    OpeningStocks = new ObservableCollection<OpeningStockModel>(purchaseStockData);
                }
                else if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Service))
                {
                    BindServiceCompanyLogo(row);
                    detail_header.Visibility = Visibility.Hidden;
                    service_Charges.Visibility = Visibility.Visible;
                    detail_Grid.Visibility = Visibility.Hidden;
                    tabControl.SelectedIndex = 0;
                    service_Charge.Text = Convert.ToString(row.RetailPrice);
                    serviceDescription_.Text = row.Description;
                    service_Barcode.Text = row.BarCode;
                    childItems.Visibility = Visibility.Collapsed;
                    childItems.IsEnabled = false;
                    if (service_Barcode.Text != string.Empty)
                        ChangeServiceBarcodeImage();
                }
                else if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.RePack))
                {
                    BindCompanyLogo(row);
                    detail_header.Visibility = Visibility.Hidden;
                    service_Charges.Visibility = Visibility.Hidden;
                    detail_Grid.Visibility = Visibility.Visible;
                    bulkCode.Visibility = Visibility.Visible;
                    bulk_Code.Visibility = Visibility.Visible;
                    tabControl.SelectedIndex = 0;
                    bulk_Code.Text = _products.FirstOrDefault(x => x.Id == row.BulkCode && x.ItemType == Convert.ToInt32(CommonEnum.ItemTypes.Bulk)).ItemName;
                    childItems.Visibility = Visibility.Collapsed;
                    childItems.IsEnabled = false;
                }
                else if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Bulk))
                {
                    BindCompanyLogo(row);
                    detail_header.Visibility = Visibility.Hidden;
                    service_Charges.Visibility = Visibility.Hidden;
                    tabControl.SelectedIndex = 0;
                    detail_Grid.Visibility = Visibility.Visible;
                    bulkCode.Visibility = Visibility.Hidden;
                    bulk_Code.Visibility = Visibility.Hidden;
                    childItems.IsEnabled = false;
                    childItems.Visibility = Visibility.Collapsed;

                }
                else
                {
                    BindCompanyLogo(row);
                    detail_header.Visibility = Visibility.Hidden;
                    service_Charges.Visibility = Visibility.Hidden;
                    tabControl.SelectedIndex = 0;
                    detail_Grid.Visibility = Visibility.Visible;
                    bulkCode.Visibility = Visibility.Hidden;
                    bulk_Code.Visibility = Visibility.Hidden;
                    childItems.IsEnabled = true;

                    List<OpeningStockModel> subProductItems = subProductItem.Select(x =>
                    {
                        // ProductModel product = GetProductName(x.ChildProductId);
                        return new OpeningStockModel() { ProductCode = x.Id, ProductContentId = x.Id, Quantity = x.Quantity, RetailPrice = x.Retail };
                    }).ToList();
                    OpeningStocks = new ObservableCollection<OpeningStockModel>(subProductItems);
                    //if (OpeningStocks.Count > 0)
                    //    lvChildProductDetails.ItemsSource = OpeningStocks;
                    //else
                    //    additems();
                }

                if (row.ImageText != "")
                {
                    fileName = row.ImageText;
                    //byte[] value = Convert.FromBase64String(row.ImageText);
                    //binaryImage = row.ItemImage;
                    //CompanyLogo.Source = CommonFunctions.ByteToImage(value);
                    //CompanyDemoLogo.Visibility = Visibility.Hidden;
                    //CompanyLogo.Visibility = Visibility.Visible;
                }
                // item_image.Text = row.ItemImage;
                shortname_.Text = row.ShortName;
                description_.Text = row.Description;
                is_texinclusive.IsChecked = row.IsTaxInclusive;
                RowId = row.Id;
                retail_price.Text = Convert.ToString(row.RetailPrice);
                trade_price.Text = Convert.ToString(row.TradePrice);
                if (OpeningStocks == null || OpeningStocks.Count == 0)
                {
                    additems();
                }
                lvChildProductDetails.Items.Clear();
                lvProductDetailwithFrreProduct.ItemsSource = OpeningStocks;
                lvProductDetails.ItemsSource = OpeningStocks;
                lvChildProductDetails.ItemsSource = OpeningStocks;
            }
        }

        private void ItemTypeEnabledDisabled()
        {
            detail_header.Visibility = Visibility.Visible;
            service_Charges.Visibility = Visibility.Hidden;
            detail_Grid.Visibility = Visibility.Visible;
            bulkCode.Visibility = Visibility.Hidden;
            bulk_Code.Visibility = Visibility.Hidden;
            childItems.IsEnabled = false;
            tabControl.SelectedIndex = 0;
            childItems.Visibility = Visibility.Collapsed;
        }
        private void BindCompanyLogo(dynamic row)
        {
            if (row.ItemImage != null)
            {
                binaryImage = row.ItemImage;
                CompanyLogo.Source = CommonFunctions.ByteToImage(row.ItemImage);
                CompanyDemoLogo.Visibility = Visibility.Hidden;
                CompanyLogo.Visibility = Visibility.Visible;
            }
        }
        private void BindServiceCompanyLogo(dynamic row)
        {
            if (row.ItemImage != null)
            {
                binaryImage = row.ItemImage;
                serviceCompanyLogo.Source = CommonFunctions.ByteToImage(row.ItemImage);
                serviceCompanyDemoLogo.Visibility = Visibility.Hidden;
                serviceCompanyLogo.Visibility = Visibility.Visible;
            }
        }

        private ProductModel GetProductName(int productCode)
        {
            return _products.FirstOrDefault(x => x.Id.Value == productCode);
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
            //if (Regex.IsMatch(taxPercentage_.Text, @"\.\d\d"))
            //{
            //    e.Handled = true;
            //}
        }

        private void txtEmail_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
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
            bool isExsist = categoryController.GetCategoriesByCompanyId().Any(x => x.CategoryName.ToLower() == category_code.Text.ToLower());
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

                string path2 = SaveImageFile(dlg.FileName);
                CompanyLogo.Source = new BitmapImage(new Uri(path2));
                //CompanyLogo.Source = b;
                CompanyDemoLogo.Visibility = Visibility.Hidden;
                CompanyLogo.Visibility = Visibility.Visible;
            }
        }
        private static string SaveImageFile(string filePath)
        {
            string FinposBasePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName;
            string path = FinposBasePath + @"\FinPosImageDocument";
            string fileName = System.IO.Path.GetFileName(filePath);
            // Create directory temp if it doesn't exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string path2 = System.IO.Path.Combine(path, fileName);
            if (fileName != (string)Application.Current.Resources["add_Company_Image_Name"] && fileName != "System.Windows.Media.Imaging.BitmapImage")
            {
                if (!File.Exists(path2))
                {
                    File.Copy(filePath, path2);
                }
            }
            return path2;
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
            if (barcode_.Text != "")
            {
                ChangeBarcodeImage();
                var response = controller.GetProductsByCompanyAndBranch().Response.Cast<ProductModel>().ToList().Any(x => x.BarCode.ToLower() == barcode_.Text.ToLower() && x.BarCode.ToLower() != barcode_.Text.ToLower() && !string.IsNullOrEmpty(x.BarCode));
                if (response)
                {
                    //var isExsist = responseAny(x => x.BarCode.ToLower() == barcode_.Text.ToLower());
                    //if (isExsist)
                    //{
                    ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["barcode_exeption"], header, false);
                    form.ShowDialog();
                    //  Common.ErrorNotification((string)Application.Current.Resources["barcode_exeption"], header, false);
                    barcode_.Text = string.Empty;
                }
                //}
            }
        }

        private void ChangeBarcodeImage()
        {
            var content = barcode_.Text;
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128
            };
            var bitmap = writer.Write(content);
            bitmap.SetResolution(100, 150);
            barcodeLogo.Source = Common.ToBitmapImage(bitmap);

        }
        private void ChangeServiceBarcodeImage()
        {
            var content = service_Barcode.Text;
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128
            };
            var bitmap = writer.Write(content);
            bitmap.SetResolution(100, 150);
            servicebarcodeLogo.Source = Common.ToBitmapImage(bitmap);
            barcodeLogo.Source = Common.ToBitmapImage(bitmap);
        }

        private void ReusableCodeTemp(string callingMethod)
        {
            try
            {
                bool found = false;
                var data = categoryController.GetCategoriesByCompanyId().Where(x => x.IsActive == true).ToList();
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
            productBulkPopUp.IsOpen = false;
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
        }
        private void AddItemSource(ProductModel openingModel)
        {
            OpeningStockModel _openingStock = new OpeningStockModel();
            _openingStock.ProductName = openingModel.ItemName;
            _openingStock.productCode = Convert.ToInt64(openingModel.Id);
            _openingStock.RetailPrice = openingModel.RetailPrice;
            _openingStock.ProductContentId = _selectedStock.ProductContentId;
            OpeningStocks[rowIndex] = _openingStock;
            if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Package))
                lvProductDetailwithFrreProduct.ItemsSource = OpeningStocks;
            else
                lvProductDetails.ItemsSource = OpeningStocks;
        }
        private void dgProducts_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProductModel productItem = (ProductModel)dgProducts.SelectedItem;
            productPopUp.IsOpen = false;
            this.IsEnabled = true;
            btn_remove.IsEnabled = false;
            btn_remove.Background = System.Windows.Media.Brushes.Gray;
            stock_AddItem(productItem);
        }
        private void lvOpeningStock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            item = (sender as ListViewItem);
            btn_remove.IsEnabled = true;
            btn_removeChildItem.IsEnabled = true;
            btn_remove.Background = (System.Windows.Media.Brush)color.ConvertFrom("#eb5151");
            btn_removeChildItem.Background = (System.Windows.Media.Brush)color.ConvertFrom("#eb5151");
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
            OpeningStockModel productContent = (OpeningStockModel)lvProductDetails.SelectedItem;
            if (productContent.ProductContentId != null)
                _deletestocks.Add(new ProductItemContentModel(productContent.ProductContentId, Convert.ToInt32(productContent.ProductCode), productContent.Quantity, RowId, UserModelVm.UserId, null, productContent.CreatedDate, string.Empty, false));
            _deleteSubProductItemModel.Add(new SubProductItemModel(productContent.ProductContentId, RowId, productContent.Quantity, Convert.ToDecimal(productContent.RetailPrice), UserModelVm.UserId, null, productContent.CreatedDate, string.Empty));


            OpeningStocks.Remove(productContent);
            if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Package))
                lvProductDetailwithFrreProduct.ItemsSource = OpeningStocks;

            else
                lvProductDetails.ItemsSource = OpeningStocks;
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
            ListViewItem lvi = GetAncestorByType(
            e.OriginalSource as DependencyObject, typeof(ListViewItem)) as ListViewItem;
            if (lvi != null)
            {
                if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Package))
                {
                    lvProductDetailwithFrreProduct.SelectedIndex =
                   lvProductDetailwithFrreProduct.ItemContainerGenerator.IndexFromContainer(lvi);
                    rowIndex = lvProductDetailwithFrreProduct.SelectedIndex;
                    _selectedStock = (OpeningStockModel)lvProductDetailwithFrreProduct.SelectedItem;
                }
                else
                {
                    lvProductDetails.SelectedIndex =
                                       lvProductDetails.ItemContainerGenerator.IndexFromContainer(lvi);
                    rowIndex = lvProductDetails.SelectedIndex;
                    _selectedStock = (OpeningStockModel)lvProductDetails.SelectedItem;
                }
            }
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }

        private void is_texinclusive_Unchecked(object sender, RoutedEventArgs e)
        {
            Handle(sender as CheckBox);
        }
        void Handle(CheckBox checkBox)
        {


            if (checkBox.IsChecked.Value)
            {
                cmbTax.IsEnabled = false;//   taxPercentage_.Visibility = Visibility.Hidden;
                _taxValue = string.Empty;
                cmbTax.SelectedIndex = 0;
                //  taxPercentage.Visibility = Visibility;
                // taxPercentage_.Text = string.Empty;

            }
            else
            {
                cmbTax.IsEnabled = true;
                //_taxValue = string.Empty;
                _taxValue = string.Empty;
            }
        }

        private void item_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Convert.ToString(item_Type.SelectedItem) != Convert.ToString(CommonEnum.ItemTypes.Standard))
            {
                detail_header.Visibility = Visibility.Visible;
            }
            else
            {
                detail_header.Visibility = Visibility.Hidden;
                tabControl.SelectedIndex = 0;
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            ProductModel model;
            List<OpeningStockModel> Stocks;
            decimal? nullval = null;
            int? integernull = null;
            if (!productNameExists)
            {
                if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Service))
                {
                    if (string.IsNullOrEmpty(product_name.Text))
                    {
                        ShowError((string)Application.Current.Resources["error_message_Tax"]);
                    }
                    if (serviceCompanyLogo.Source != null && System.IO.Path.GetFileName(serviceCompanyLogo.Source.ToString()) != "System.Windows.Media.Imaging.BitmapImage")
                    {
                        SaveImageFile(serviceCompanyLogo.Source.ToString());
                        fileName = System.IO.Path.GetFileName(serviceCompanyLogo.Source.ToString());
                    }
                    model = new ProductModel(RowId, product_name.Text, null, string.IsNullOrEmpty(service_Charge.Text) ? nullval : Convert.ToDecimal(service_Charge.Text), string.IsNullOrEmpty(trade_price.Text) ? nullval : Convert.ToDecimal(trade_price.Text), string.IsNullOrEmpty(wholeseller_price.Text) ? nullval : Convert.ToDecimal(wholeseller_price.Text), string.IsNullOrEmpty(reseller_price.Text) ? nullval : Convert.ToDecimal(reseller_price.Text), Convert.ToInt32(item_Type.SelectedItem), string.IsNullOrEmpty(weight_.Text) ? nullval : Convert.ToDecimal(weight_.Text), barcode_.Text, string.IsNullOrEmpty(_taxValue) ? nullval : Convert.ToDecimal(_taxValue), string.IsNullOrEmpty(minimum_level.Text) ? integernull : Convert.ToInt32(minimum_level.Text), string.IsNullOrEmpty(reorder_level.Text) ? integernull : Convert.ToInt32(reorder_level.Text), null, is_texinclusive.IsChecked.Value, shortname_.Text, serviceDescription_.Text, UserModelVm.BranchId, string.Empty, string.Empty, null, (fileName != null) ? fileName : "", UserModelVm.CompanyId, null);
                    controller.SaveUpdateProduct(model);
                    SuccessRetrun();
                }
                else
                {
                    if (string.IsNullOrEmpty(product_name.Text) || string.IsNullOrEmpty(retail_price.Text) || string.IsNullOrEmpty(trade_price.Text) || string.IsNullOrEmpty(itemType.Text) || item_Type.SelectedIndex == -1)
                    {
                        ShowError((string)Application.Current.Resources["error_message_Tax"]);
                    }
                    else if (is_texinclusive.IsChecked.Value == false && string.IsNullOrEmpty(_taxValue))
                    {
                        ShowError((string)Application.Current.Resources["taxPercentage_ErrorMsg"]);
                    }
                    else if (Convert.ToDecimal(retail_price.Text) < Convert.ToDecimal(trade_price.Text))
                    {
                        ShowError((string)Application.Current.Resources["taxPercentage_ErrorInRetailPriceMsg"]);
                    }
                    else
                    {
                        Stocks = lvProductDetails.Items.Cast<OpeningStockModel>().Select(x => x).ToList();
                        bool IsStocks = Stocks.Where(x => x.ProductCode > 0 && x.Quantity > 0).Any();
                        bool IsChildProduct = Stocks.Where(x => x.RetailPrice > 0 && x.Quantity > 0).Any();
                        bool anyDuplicate = Stocks.GroupBy(x => x.Quantity).Any(g => g.Count() > 1);
                        if ((Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Kit) || Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Package)) && !IsStocks)
                        {
                            ShowError((string)Application.Current.Resources["error_message_addProduct"]);
                        }
                        else if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Bulk) && string.IsNullOrEmpty(weight_.Text))
                        {
                            ShowError((string)Application.Current.Resources["error_message_addWeight"]);
                        }
                        else if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.RePack) && (string.IsNullOrEmpty(weight_.Text) || string.IsNullOrEmpty(bulk_Code.Text)))
                        {
                            ShowError((string)Application.Current.Resources["error_message_addWeightAndBulkCode"]);
                        }
                        else if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Standard) && anyDuplicate)
                        {
                            ShowError((string)Application.Current.Resources["error_message_addChildItems"]);
                        }
                        else
                        {
                            List<ProductItemContentModel> productItemContentModel = new List<ProductItemContentModel>();
                            List<SubProductItemModel> subProductItemModel = new List<SubProductItemModel>();
                            if (!string.IsNullOrEmpty(category_code.Text))
                            {
                                categoryId = _categories.FirstOrDefault(x => x.CategoryName.ToLower() == category_code.Text.ToLower()).Id;
                            }
                            //binaryImage = Utility.CommonMethods.CommonFunctions.ImageToByteArray((BitmapImage)CompanyLogo.Source);
                            //string data = Convert.ToBase64String(binaryImage);
                            if (CompanyLogo.Source != null && System.IO.Path.GetFileName(CompanyLogo.Source.ToString()) != "System.Windows.Media.Imaging.BitmapImage")
                            {
                                SaveImageFile(CompanyLogo.Source.ToString());
                                fileName = System.IO.Path.GetFileName(CompanyLogo.Source.ToString());
                            }


                            model = new ProductModel(RowId, product_name.Text, categoryId, string.IsNullOrEmpty(retail_price.Text) ? nullval : Convert.ToDecimal(retail_price.Text), string.IsNullOrEmpty(trade_price.Text) ? nullval : Convert.ToDecimal(trade_price.Text), string.IsNullOrEmpty(wholeseller_price.Text) ? nullval : Convert.ToDecimal(wholeseller_price.Text), string.IsNullOrEmpty(reseller_price.Text) ? nullval : Convert.ToDecimal(reseller_price.Text), Convert.ToInt32(item_Type.SelectedItem), string.IsNullOrEmpty(weight_.Text) ? nullval : Convert.ToDecimal(weight_.Text), barcode_.Text, string.IsNullOrEmpty(_taxValue) ? nullval : Convert.ToDecimal(_taxValue), string.IsNullOrEmpty(minimum_level.Text) ? integernull : Convert.ToInt32(minimum_level.Text), string.IsNullOrEmpty(reorder_level.Text) ? integernull : Convert.ToInt32(reorder_level.Text), null, is_texinclusive.IsChecked.Value, shortname_.Text, description_.Text, UserModelVm.BranchId, string.Empty, string.Empty, null,
                                (fileName != null) ? fileName : "", UserModelVm.CompanyId, string.IsNullOrEmpty(bulk_Code.Text) ? integernull : _products.FirstOrDefault(x => x.ItemName.ToLower() == bulk_Code.Text.ToLower() && x.ItemType == Convert.ToInt32(CommonEnum.ItemTypes.Bulk)).Id);

                            int parentProductId = controller.SaveUpdateProduct(model);
                            if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Kit) || Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Package))
                            {

                                productItemContentModel.AddRange(Stocks.Where(z => z.ProductCode > 0 && z.Quantity > 0).Select(x =>
                                {
                                    return new ProductItemContentModel(x.ProductContentId == null ? 0 : x.ProductContentId, Convert.ToInt32(x.ProductCode), x.Quantity, parentProductId, UserModelVm.UserId, null, "", "", x.isFreeProduct);
                                }).ToList());
                                controller.SaveUpdateProductItemContent(productItemContentModel, _deletestocks);
                            }
                            if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Standard) && IsChildProduct)
                            {
                                subProductItemModel.AddRange(Stocks.Select(x =>
                                {
                                    return new SubProductItemModel(x.ProductContentId == null ? 0 : x.ProductContentId, parentProductId, x.Quantity, x.RetailPrice.Value, UserModelVm.UserId, null, "", "");
                                }).ToList());
                                controller.SaveUpdateSubProductItems(subProductItemModel, _deleteSubProductItemModel);
                            }
                            SuccessRetrun();
                        }
                    }
                }
            }
            else
            {
                productNameExists = true;
                Common.ErrorMessage((string)Application.Current.Resources["product_exists"], header);
            }
        }
        private void ShowError(string msg)
        {
            ConfirmationPopup form = new ConfirmationPopup(msg, header, false);
            form.ShowDialog();
        }
        private void SuccessRetrun()
        {
            Common.Notification((string)Application.Current.Resources["product_updateSuccessMsg"], header, false);
            ClearFields();
            DefaultFields();
            inventory form1 = new inventory();
            NavigationService.Navigate(form1);
        }
        private void ClearFields()
        {
            serviceCompanyLogo.Source = null;
            servicebarcodeLogo.Source = null;
            CompanyDemoLogo.Visibility = Visibility.Visible;
            serviceCompanyDemoLogo.Visibility = Visibility.Visible;
            is_texinclusive.IsChecked = false;
            barcodeLogo.Source = null;
            retail_price.Text = "";
            trade_price.Text = "";
            wholeseller_price.Text = "";
            reseller_price.Text = "";
            weight_.Text = "";
            barcode_.Text = "";
            minimum_level.Text = "";
            reorder_level.Text = "";
            // itemImage.Text = "";
            description_.Text = "";
            barcode_.Text = "";
            bulk_Code.Text = "";
            service_Barcode.Text = "";
        }
        private void addProductName(string text)
        {
            lbAutoCat.Items.Add(text);
        }

        private void product_name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void is_texinclusive_Checked(object sender, RoutedEventArgs e)
        {
            Handle(sender as CheckBox);
        }

        //private void taxPercentage__LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (taxPercentage_.Text == ".")
        //    {
        //        taxPercentage_.Text = string.Empty;
        //    }
        //    else if (!string.IsNullOrEmpty(taxPercentage_.Text) && Convert.ToDecimal(taxPercentage_.Text) > 100)
        //    {
        //        ShowError("Tax percentage exceed");
        //        taxPercentage_.Text = "";
        //    }
        //}

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var CurrentRowData = (OpeningStockModel)((System.Windows.FrameworkElement)e.OriginalSource).DataContext;
            int index = OpeningStocks.IndexOf(OpeningStocks.Where(X => X.ProductCode == CurrentRowData.ProductCode && X.Quantity == CurrentRowData.Quantity).FirstOrDefault());
            OpeningStockModel _openingStock = new OpeningStockModel();
            _openingStock.ProductName = CurrentRowData.ProductName;
            _openingStock.ProductCode = Convert.ToInt64(CurrentRowData.ProductCode);
            _openingStock.CurrentStock = OpeningStockController.GetCurrentStockByProductCode(Convert.ToInt64(CurrentRowData.ProductCode));
            _openingStock.RetailPrice = CurrentRowData.RetailPrice;
            _openingStock.Quantity = CurrentRowData.Quantity;
            _openingStock.ProductContentId = CurrentRowData.ProductContentId;
            _openingStock.TotalPrice = CurrentRowData.Quantity * CurrentRowData.RetailPrice;
            OpeningStocks[index] = _openingStock;
            if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Package))
                lvProductDetailwithFrreProduct.ItemsSource = OpeningStocks;
            else
                lvProductDetails.ItemsSource = OpeningStocks;
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            DefaultFields();
            inventory form = new inventory();
            NavigationService.Navigate(form);
        }
        private void DefaultFields()
        {
            category_code.Text = "";
            shortname_.Text = "";
            cmbTax.SelectedIndex = 0;//axPercentage_.Text = "";
            product_name.Text = "";
        }

        private void txtProductBulk_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            productBulkPopUp.IsOpen = true;
            this.IsEnabled = false;
            btn_remove.IsEnabled = false;
            btn_remove.Background = System.Windows.Media.Brushes.Gray;
        }

        private void productBulk_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textBoxName = (System.Windows.Controls.TextBox)sender;
            string filterText = textBoxName.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(dgProductsBulk.ItemsSource);

            if (cv == null)
                return;
            if (!string.IsNullOrEmpty(filterText))
            {

                cv.Filter = o =>
                {
                    /* change to get data row value */
                    ProductModel p = o as ProductModel;
                    return (p.ItemName.ToUpper().StartsWith(filterText.ToUpper()) || p.BarCode.ToUpper().StartsWith(filterText.ToUpper()) || Convert.ToString(p.Id).ToUpper().StartsWith(filterText.ToUpper()));
                    /* end change to get data row value */
                };

            }
            else
                cv.Filter = null;
        }

        private void dgProductsBulk_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProductModel productItem = (ProductModel)dgProductsBulk.SelectedItem;
            productBulkPopUp.IsOpen = false;
            this.IsEnabled = true;
            btn_remove.IsEnabled = false;
            // btn_remove.Background = System.Windows.Media.Brushes.Gray;
            bulk_Code.Text = Convert.ToString(productItem.ItemName);
        }

        private void purchase_Click(object sender, RoutedEventArgs e)
        {
            histroy_calender.IsEnabled = true;
            GetHistoryOfProduct(RowId, CommonFunctions.ParseDateToFinclave(DateTime.Now.ToShortDateString()).Year);
        }

        private void service_Barcode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (service_Barcode.Text != "")
            {
                var content = service_Barcode.Text;
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.CODE_128
                };
                var bitmap = writer.Write(content);
                bitmap.SetResolution(100, 150);
                servicebarcodeLogo.Source = Common.ToBitmapImage(bitmap);

                var response = controller.GetProductsByCompanyAndBranch().Response.Cast<ProductModel>().ToList().Any(x => x.BarCode.ToLower() == service_Barcode.Text.ToLower() && x.BarCode.ToLower() != service_Barcode.Text.ToLower() && !string.IsNullOrEmpty(x.BarCode));
                if (response)
                {
                    //var isExsist = responseAny(x => x.BarCode.ToLower() == barcode_.Text.ToLower());
                    //if (isExsist)
                    //{
                    ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["barcode_exeption"], header, false);
                    form.ShowDialog();
                    Common.ErrorNotification((string)Application.Current.Resources["barcode_exeption"], header, false);
                    service_Barcode.Text = string.Empty;
                }
                //}
            }
        }

        private void serviceUploadLogo_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string path2 = SaveImageFile(dlg.FileName);
                serviceCompanyLogo.Source = new BitmapImage(new Uri(path2));
                //CompanyLogo.Source = b;
                serviceCompanyDemoLogo.Visibility = Visibility.Hidden;
                serviceCompanyLogo.Visibility = Visibility.Visible;
            }
        }

        private void serviceRemoveLogo_Click(object sender, RoutedEventArgs e)
        {
            serviceCompanyDemoLogo.Visibility = Visibility.Visible;
            serviceCompanyLogo.Source = CompanyDemoLogo.Source;
            serviceCompanyLogo.Visibility = Visibility.Hidden;
        }
        public void ChangeHeightWidth()
        {
            this.EditPage.Height = HeightWidth.Height - 65;
            this.EditPage.Width = HeightWidth.width;
        }
        private void btn_removeChildItem_Click(object sender, RoutedEventArgs e)
        {
            OpeningStockModel productContent = (OpeningStockModel)lvChildProductDetails.SelectedItem;
            if (productContent.ProductContentId != null)
                _deleteSubProductItemModel.Add(new SubProductItemModel(productContent.ProductContentId, RowId, productContent.Quantity, Convert.ToDecimal(productContent.RetailPrice), UserModelVm.UserId, null, productContent.CreatedDate, string.Empty));


            OpeningStocks.Remove(productContent);
            lvProductDetails.ItemsSource = OpeningStocks;
            lvChildProductDetails.ItemsSource = OpeningStocks;
        }

        private void lvChildProductDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tabControl_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void category_code_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void cmbTax_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTax.SelectedIndex > 0)
            {
                _taxValue = Convert.ToString(_taxdetails.FirstOrDefault(item => item.TaxCode == Convert.ToInt32(this.cmbTax.SelectedValue)).Rate);
            }
            else
            {
                _taxValue = null;
            }
        }

        private void product_name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (product_name.Text != string.Empty)
            {
                var productId = controller.GetProductIdByName(product_name.Text);
                if (productId != null && RowId != productId)
                {
                    product_name.Text = string.Empty;
                    productNameExists = true;
                    Common.ErrorMessage((string)Application.Current.Resources["product_exists"], header);
                }
                else
                {
                    productNameExists = false;
                }
            }
        }

        private void histroy_calender_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            GetHistoryOfProduct(RowId, CommonFunctions.ParseDateToFinclave(histroy_calender.Text).Year);
        }
        private void GetHistoryOfProduct(int productId, int year)
        {
            _purchases = purchaseController.GetPurchaseById(productId, year);
            lvPurchase.ItemsSource = _purchases;
            lvPurchase.Visibility = Visibility.Visible;
        }

        private void check_IsnumericValue(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var isNumericValue = Common.isNumeric(textBox.Text, System.Globalization.NumberStyles.AllowDecimalPoint);
            if (!isNumericValue)
                textBox.Text = string.Empty;
        }

        private void GridViewColumnHeader_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void retail_price_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {

        }

        private void txtserviceLostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void serviceDescription__TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void lvUsers_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);

        }

        private void lvUsers_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lvPurchase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void reorder_level_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void GridViewColumnHeader_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}

