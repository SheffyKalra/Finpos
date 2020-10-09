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
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for AddProductHistory.xaml
    /// </summary>
    public partial class AddProductHistory : Page
    {
        #region Properties
        ProductController controller = new ProductController();
        CategoryController categoryController = new CategoryController();
        private string msg = string.Empty;
        private string header = (string)Application.Current.Resources["addProduct_Headerforpopup"];
        private int _noOfErrorsOnScreen = 0;
        private bool productNameExists = false;
        public ListViewItem item = null;
        private List<CategoryModel> _categories;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private int? categoryId = null;
        private Timer _timer;
        private DateTime _lastBarCodeCharReadTime;
        private IList<ProductModel> _products;
        private OpeningStockController OpeningStockController = new OpeningStockController();
        private ObservableCollection<OpeningStockModel> OpeningStocks;
        private ObservableCollection<ChildProductItemsVm> ChildProductItems;
        BrushConverter color = new BrushConverter();
        public OpeningStockModel _selectedStock;
        public int rowIndex = 0;
        private IList<TaxModel> _taxdetails;
        private string _taxValue;
        TaxController taxController = new TaxController();
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        #endregion
        #region Constructor
        public AddProductHistory()
        {
            InitializeComponent();
            ChangeHeightWidth();
            ClearFields();
            DefaultFields();
            ResponseVm responseTax = taxController.GetTax();
            if (responseTax.FaultData == null)
            {
                TaxModel objTaxModel = new TaxModel(0, "Select", 0, "", "", "", "");
                _taxdetails = responseTax.Response.Cast<TaxModel>().ToList();
                _taxdetails.Insert(0, objTaxModel);
                this.cmbTax.ItemsSource = _taxdetails;
                this.cmbTax.DisplayMemberPath = "TaxDetail";
                this.cmbTax.SelectedValuePath = "TaxCode";
                this.cmbTax.SelectedIndex = 0;
            }
            item_Type.ItemsSource = Enum.GetValues(typeof(CommonEnum.ItemTypes)).Cast<CommonEnum.ItemTypes>();
            item_Type.SelectedItem = (CommonEnum.ItemTypes)1;
            _categories = categoryController.GetCategoriesByCompanyId().Where(x => x.IsActive == true).ToList();

            btn_remove.Background = System.Windows.Media.Brushes.Gray;
            btn_removeChildItem.Background = System.Windows.Media.Brushes.Gray;
            ResponseVm responce = controller.GetProductsByCompanyAndBranch();
            detail_header.Visibility = Visibility.Hidden;
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

            if (OpeningStocks == null)
            {
                additems();
            }
            lvProductDetailwithFrreProduct.ItemsSource = OpeningStocks;
            lvProductDetails.ItemsSource = OpeningStocks;
            lvChildProductDetails.ItemsSource = OpeningStocks;
        }
        #endregion
        #region Common Method 
        public void ChangeHeightWidth()
        {
            this.AddProductPage.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.AddProductPage.Width = HeightWidth.width;

        }
        public void additems()
        {
            OpeningStocks = new ObservableCollection<OpeningStockModel>();
            OpeningStocks.Add(new OpeningStockModel() { ProductName = "" });
        }
        private void SuccessRetrun()
        {
            Common.Notification((string)Application.Current.Resources["product_saveSuccessMsg"], header, false);
            ClearFields();
            DefaultFields();
            OpeningStocks = null;
            additems();
            lvProductDetails.ItemsSource = OpeningStocks;
            lvProductDetailwithFrreProduct.ItemsSource = OpeningStocks;
            lvChildProductDetails.ItemsSource = OpeningStocks;
            AddProductHistory obj = new AddProductHistory();
            NavigationService.Navigate(obj);
        }
        private void ClearFields()
        {
            serviceCompanyLogo.Source = null;
            servicebarcodeLogo.Source = null;
            CompanyDemoLogo.Visibility = Visibility.Visible;
            serviceCompanyDemoLogo.Visibility = Visibility.Visible;
            is_texinclusive.IsChecked = false;
            CompanyLogo.Source = null;
            barcodeLogo.Source = null;
            retail_price.Text = string.Empty;
            trade_price.Text = string.Empty;
            wholeseller_price.Text = string.Empty;
            reseller_price.Text = string.Empty;
            weight_.Text = string.Empty;
            barcode_.Text = string.Empty;
            minimum_level.Text = string.Empty;
            reorder_level.Text = string.Empty;
            description_.Text = string.Empty;
            barcode_.Text = string.Empty;
            bulk_Code.Text = string.Empty;
            txtservicebarcode.Text = string.Empty;
        }
        private void DefaultFields()
        {
            category_code.Text = string.Empty;
            shortname_.Text = string.Empty;
            cmbTax.SelectedIndex = 0;
            product_name.Text = string.Empty;
        }
        private void addProductName(string text)
        {
            lbAutoCat.Items.Add(text);
        }
        #endregion
        #region CRUD Opertion
        private void Timer_Tick(object sender, EventArgs e)
        {
            const int timeout = 1500;
            if ((CommonFunctions.ParseDateToFinclave(DateTime.Now.ToShortDateString()) - _lastBarCodeCharReadTime).Milliseconds < timeout)
                return;

            _timer.Stop();
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
        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            if (lbAutoCat.Visibility == Visibility.Visible)
                lbAutoCat.Visibility = Visibility.Collapsed;
            else
                ReusableCodeTemp("arrow_Click");
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            ReusableCodeTemp("TextBox_KeyUp");
        }

        private void category_code_LostFocus(object sender, RoutedEventArgs e)
        {
            bool isExsist = categoryController.GetCategoriesByCompanyId().Any(x => x.CategoryName.ToLower() == category_code.Text.ToLower());
            if (!isExsist)
                category_code.Text = string.Empty;
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
            dlg.DefaultExt = (string)Application.Current.Resources["company_imagePngType"];
            dlg.Filter = (string)Application.Current.Resources["company_CompareImageType"];

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string path2 = SaveImageFile(dlg.FileName);
                CompanyLogo.Source = new BitmapImage(new Uri(path2));
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
            if (fileName != (string)Application.Current.Resources["add_Company_Image_Name"])
            {
                if (!File.Exists(path2))
                    File.Copy(filePath, path2);
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
                var content = barcode_.Text;
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.CODE_128
                };
                var bitmap = writer.Write(content);
                bitmap.SetResolution(100, 150);
                barcodeLogo.Source = Common.ToBitmapImage(bitmap);

                var response = controller.GetProductsByCompanyAndBranch().Response.Cast<ProductModel>().ToList().Any(x => x.BarCode.ToLower() == barcode_.Text.ToLower() && !string.IsNullOrEmpty(x.BarCode));
                if (response)
                {
                    Common.ErrorMessage((string)Application.Current.Resources["barcode_exeption"], header);
                    barcode_.Text = string.Empty;
                }
            }
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
                            addProductName(obj.CategoryName);
                            found = true;
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
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            productPopUp.IsOpen = false;
            productBulkPopUp.IsOpen = false;
            this.IsEnabled = true;
        }

        private void product_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textBoxName = (System.Windows.Controls.TextBox)sender;
            string filterText = textBoxName.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(dgProducts.ItemsSource);

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
        public void stock_AddItem(ProductModel itemToAdd)
        {

            if (OpeningStocks.Any(x => x.ProductCode == itemToAdd.Id) && _selectedStock.ProductCode != itemToAdd.Id)
            {
                Common.ShowConfirmationPopup((string)Application.Current.Resources["purchase_already"], header,true);
                if (Common._isChecked)
                    AddItemSource(itemToAdd);
            }
            else
                AddItemSource(itemToAdd);
        }
        private void AddItemSource(ProductModel openingModel)
        {
            OpeningStockModel _openingStock = new OpeningStockModel();
            _openingStock.ProductName = openingModel.ItemName;
            _openingStock.productCode = Convert.ToInt64(openingModel.Id);
            _openingStock.CurrentStock = OpeningStockController.GetCurrentStockByProductCode(Convert.ToInt64(openingModel.Id));
            _openingStock.RetailPrice = openingModel.RetailPrice;
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
            btn_removeChildItem.IsEnabled = false;
            btn_removeChildItem.Background = System.Windows.Media.Brushes.Gray;
            bool stockExists = OpeningStockController.CheckProductOpningStock(Convert.ToInt64(productItem.Id));
            if (!stockExists)
                stock_AddItem(productItem);
            else
                Common.ErrorMessage((string)Application.Current.Resources["error_message_false_product_selection_Opening_Stock"], header);
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                    e.CancelCommand();
            }
            else
                e.CancelCommand();
        }
        private Boolean IsTextAllowed(String text)
        {
            return Array.TrueForAll<Char>(text.ToCharArray(),
                delegate (Char c) { return Char.IsDigit(c) || Char.IsControl(c); });
        }
        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            if (productPopUp.IsOpen == true)
                return;
            if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Package))
                OpeningStocks.Remove((OpeningStockModel)lvProductDetailwithFrreProduct.SelectedItem);
            else
                 OpeningStocks.Remove((OpeningStockModel)lvProductDetails.SelectedItem);
                 btn_remove.IsEnabled = false;
                 btn_remove.Background = System.Windows.Media.Brushes.Gray;
        }
        private void lvOpeningStock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            item = (sender as ListViewItem);
            btn_remove.IsEnabled = true;
            btn_remove.Background = (System.Windows.Media.Brush)color.ConvertFrom("#eb5151");
            btn_removeChildItem.IsEnabled = true;
            btn_removeChildItem.Background = (System.Windows.Media.Brush)color.ConvertFrom("#eb5151");
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
        private void txtProductBulk_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            productBulkPopUp.IsOpen = true;
            this.IsEnabled = false;
            btn_remove.IsEnabled = false;
            btn_remove.Background = System.Windows.Media.Brushes.Gray;
        }
        private void is_texinclusive_Unchecked(object sender, RoutedEventArgs e)
        {
            Handle(sender as CheckBox);
        }
        void Handle(CheckBox checkBox)
        {
            if (checkBox.IsChecked.Value)
            {
                cmbTax.IsEnabled = false;
                _taxValue = string.Empty;
                cmbTax.SelectedIndex = 0;
            }
            else
            {
                cmbTax.IsEnabled = true;
                _taxValue = string.Empty;
            }
        }
        private void item_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Package))
            {
                ToggleTabControl(Visibility.Visible, Visibility.Hidden, Visibility.Visible, Visibility.Collapsed, Visibility.Hidden, Visibility.Hidden);
                ToggleProductList(Visibility.Visible,Visibility.Collapsed);

            }
            else if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Kit))
            {
                ToggleProductList(Visibility.Collapsed, Visibility.Visible);
                ToggleTabControl(Visibility.Visible, Visibility.Hidden, Visibility.Visible, Visibility.Collapsed, Visibility.Hidden, Visibility.Hidden);

            }
            else if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Service))
            {
                ToggleTabControl(Visibility.Hidden, Visibility.Visible, Visibility.Hidden, Visibility.Collapsed, Visibility.Hidden, Visibility.Hidden);
            }
            else if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.RePack))
            {
                ToggleTabControl(Visibility.Hidden, Visibility.Hidden, Visibility.Visible, Visibility.Collapsed, Visibility.Visible, Visibility.Visible);
            }
            else if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Bulk))
            {
                ToggleTabControl(Visibility.Hidden, Visibility.Hidden, Visibility.Visible, Visibility.Collapsed, Visibility.Hidden, Visibility.Hidden);
            }
            else
            {
                ToggleTabControl(Visibility.Hidden, Visibility.Hidden, Visibility.Visible, Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
            }
        }

        private void ToggleTabControl(Visibility detailHeaderVisible, Visibility serviceChargeVisible,Visibility detailGridVisible,Visibility childItemsVisible,Visibility bulkCodeVisible,Visibility bulk_CodeVisible)
        {
            detail_header.Visibility = detailHeaderVisible;
            service_Charges.Visibility = serviceChargeVisible;
            detail_Grid.Visibility = detailGridVisible;
            bulkCode.Visibility = bulkCodeVisible;
            bulk_Code.Visibility = bulk_CodeVisible;
            tabControl.SelectedIndex = 0;
            childItems.Visibility = childItemsVisible;
            ClearFields();
        }

        private void ToggleProductList(Visibility visilvProductDetailwithFrreProduct, Visibility visilvProductDetails)
        {
            lvProductDetailwithFrreProduct.Visibility = visilvProductDetailwithFrreProduct;
            lvProductDetails.Visibility = visilvProductDetails;
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
                        Common.ErrorMessage((string)Application.Current.Resources["error_message_Tax"], header);
                    if (serviceCompanyLogo.Source != null)
                        SaveImageFile(serviceCompanyLogo.Source.ToString());

                    model = new ProductModel(0, product_name.Text, null,
                        string.IsNullOrEmpty(service_Charge.Text) ? nullval : Convert.ToDecimal(service_Charge.Text),
                        string.IsNullOrEmpty(trade_price.Text) ? nullval : Convert.ToDecimal(trade_price.Text),
                        string.IsNullOrEmpty(wholeseller_price.Text) ? nullval : Convert.ToDecimal(wholeseller_price.Text),
                        string.IsNullOrEmpty(reseller_price.Text) ? nullval : Convert.ToDecimal(reseller_price.Text),
                        Convert.ToInt32(item_Type.SelectedItem), string.IsNullOrEmpty(weight_.Text) ? nullval : Convert.ToDecimal(weight_.Text),
                        barcode_.Text, string.IsNullOrEmpty(_taxValue) ? nullval : Convert.ToDecimal(_taxValue),
                        string.IsNullOrEmpty(minimum_level.Text) ? integernull : Convert.ToInt32(minimum_level.Text),
                        string.IsNullOrEmpty(reorder_level.Text) ? integernull : Convert.ToInt32(reorder_level.Text),
                        null, is_texinclusive.IsChecked.Value, shortname_.Text, serviceDescription_.Text,
                        UserModelVm.BranchId, string.Empty, string.Empty, null,
                        (serviceCompanyLogo.Source != null) ? System.IO.Path.GetFileName(serviceCompanyLogo.Source.ToString()) : "", UserModelVm.CompanyId, null);
                    controller.SaveUpdateProduct(model);
                    SuccessRetrun();
                }
                else
                {
                    if (string.IsNullOrEmpty(product_name.Text) || string.IsNullOrEmpty(retail_price.Text) || string.IsNullOrEmpty(trade_price.Text) || string.IsNullOrEmpty(itemType.Text) || item_Type.SelectedIndex == -1)
                        Common.ErrorMessage((string)Application.Current.Resources["error_message_Tax"], header);
                    else if (is_texinclusive.IsChecked.Value == false && string.IsNullOrEmpty(_taxValue))
                        Common.ErrorMessage((string)Application.Current.Resources["taxPercentage_ErrorMsg"], header);
                    else if (Convert.ToDecimal(retail_price.Text) < Convert.ToDecimal(trade_price.Text))
                        Common.ErrorMessage((string)Application.Current.Resources["taxPercentage_ErrorInRetailPriceMsg"], header);
                    else
                    {
                        Stocks = lvProductDetails.Items.Cast<OpeningStockModel>().Select(x => x).ToList();
                        bool anyDuplicate = Stocks.GroupBy(x => x.Quantity).Any(g => g.Count() > 1);
                        bool IsStocks = Stocks.Where(x => x.ProductCode > 0 && x.Quantity > 0).Any();
                        bool IsChildProduct = Stocks.Where(x => x.RetailPrice > 0 && x.Quantity > 0).Any();
                        if ((Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Kit) || Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Package)) && !IsStocks)
                            Common.ErrorMessage((string)Application.Current.Resources["error_message_addProduct"], header);
                        else if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Bulk) && string.IsNullOrEmpty(weight_.Text))
                            Common.ErrorMessage((string)Application.Current.Resources["error_message_addWeight"], header);
                        else if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.RePack) && (string.IsNullOrEmpty(weight_.Text) || string.IsNullOrEmpty(bulk_Code.Text)))
                            Common.ErrorMessage((string)Application.Current.Resources["error_message_addWeightAndBulkCode"], header);
                        else if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Standard) && anyDuplicate)
                            Common.ErrorMessage((string)Application.Current.Resources["error_message_addChildItems"], header);
                        else
                        {
                            List<ProductItemContentModel> productItemContentModel = new List<ProductItemContentModel>();
                            List<SubProductItemModel> subProductItemModel = new List<SubProductItemModel>();

                            if (!string.IsNullOrEmpty(category_code.Text))
                                categoryId = _categories.FirstOrDefault(x => x.CategoryName.ToLower() == category_code.Text.ToLower()).Id;
                            if (CompanyLogo.Source != null)
                                SaveImageFile(CompanyLogo.Source.ToString());
                            model = new ProductModel(0, product_name.Text, categoryId, string.IsNullOrEmpty(retail_price.Text) ? nullval : Convert.ToDecimal(retail_price.Text),
                                string.IsNullOrEmpty(trade_price.Text) ? nullval : Convert.ToDecimal(trade_price.Text), string.IsNullOrEmpty(wholeseller_price.Text) ? nullval : Convert.ToDecimal(wholeseller_price.Text), string.IsNullOrEmpty(reseller_price.Text) ? nullval : Convert.ToDecimal(reseller_price.Text),
                                Convert.ToInt32(item_Type.SelectedItem), string.IsNullOrEmpty(weight_.Text) ? nullval : Convert.ToDecimal(weight_.Text), barcode_.Text, string.IsNullOrEmpty(_taxValue) ? nullval : Convert.ToDecimal(_taxValue), string.IsNullOrEmpty(minimum_level.Text) ? integernull : Convert.ToInt32(minimum_level.Text), string.IsNullOrEmpty(reorder_level.Text) ? integernull : Convert.ToInt32(reorder_level.Text), null, is_texinclusive.IsChecked.Value, shortname_.Text, description_.Text, UserModelVm.BranchId, string.Empty, string.Empty, null,
                                (CompanyLogo.Source != null) ? System.IO.Path.GetFileName(CompanyLogo.Source.ToString()) : "", UserModelVm.CompanyId, string.IsNullOrEmpty(bulk_Code.Text) ? integernull : _products.FirstOrDefault(x => x.ItemName.ToLower() == bulk_Code.Text.ToLower() && x.ItemType == Convert.ToInt64(CommonEnum.ItemTypes.Bulk)).Id);

                            int parentProductId = controller.SaveUpdateProduct(model);
                            if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Kit) || Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Package))
                            {

                                productItemContentModel.AddRange(Stocks.Where(z => z.ProductCode > 0 && z.Quantity > 0).Select(x =>
                                {
                                    return new ProductItemContentModel(0, Convert.ToInt32(x.ProductCode), x.Quantity, parentProductId, UserModelVm.UserId, null, "", "", x.isFreeProduct);
                                }).ToList());
                                controller.SaveProductItemContent(productItemContentModel);
                            }
                            if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Standard) && IsChildProduct)
                            {
                                subProductItemModel.AddRange(Stocks.Select(x =>
                                {
                                    return new SubProductItemModel(0, parentProductId, x.Quantity, x.RetailPrice.Value, UserModelVm.UserId, null, "", "");
                                }).ToList());
                                controller.SaveSubProductItems(subProductItemModel);
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

    

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            DefaultFields();
            NavigateToBackPage();
        }
        public void NavigateToBackPage()
        {
            inventory form = new inventory();
            NavigationService.Navigate(form);
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var CurrentRowData = (OpeningStockModel)((FrameworkElement)e.OriginalSource).DataContext;
            int index = OpeningStocks.IndexOf(OpeningStocks.Where(X => X.ProductCode == CurrentRowData.ProductCode && X.Quantity == CurrentRowData.Quantity).FirstOrDefault());
            OpeningStockModel _openingStock = new OpeningStockModel();
            _openingStock.ProductName = CurrentRowData.ProductName;
            _openingStock.ProductCode = Convert.ToInt64(CurrentRowData.ProductCode);
            _openingStock.CurrentStock = OpeningStockController.GetCurrentStockByProductCode(Convert.ToInt64(CurrentRowData.ProductCode));
            _openingStock.RetailPrice = CurrentRowData.RetailPrice;
            _openingStock.Quantity = CurrentRowData.Quantity;
            _openingStock.TotalPrice = CurrentRowData.Quantity * CurrentRowData.RetailPrice;
            OpeningStocks[index] = _openingStock;
            if (Convert.ToString(item_Type.SelectedItem) == Convert.ToString(CommonEnum.ItemTypes.Package))
                lvProductDetailwithFrreProduct.ItemsSource = OpeningStocks;
            else
                lvProductDetails.ItemsSource = OpeningStocks;
        }

        private void is_texinclusive_Checked(object sender, RoutedEventArgs e)
        {
            Handle(sender as CheckBox);
        }
        private void dgProductsBulk_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProductModel productItem = (ProductModel)dgProductsBulk.SelectedItem;
            productBulkPopUp.IsOpen = false;
            this.IsEnabled = true;
            btn_remove.IsEnabled = false;
            bulk_Code.Text = Convert.ToString(productItem.ItemName);
        }

        private void txtservicebarcode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtservicebarcode.Text != "")
            {
                var content = txtservicebarcode.Text;
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.CODE_128
                };
                var bitmap = writer.Write(content);
                bitmap.SetResolution(100, 150);
                servicebarcodeLogo.Source = Common.ToBitmapImage(bitmap);
                var response = controller.GetProductsByCompanyAndBranch().Response.Cast<ProductModel>().ToList().Any(x => x.BarCode.ToLower() == txtservicebarcode.Text.ToLower() && !string.IsNullOrEmpty(x.BarCode));
                if (response)
                {
                    Common.ErrorMessage((string)Application.Current.Resources["barcode_exeption"], header);
                    txtservicebarcode.Text = string.Empty;
                }
            }
        }

        private void serviceUploadLogo_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = (string)Application.Current.Resources["company_imagePngType"];
            dlg.Filter = (string)Application.Current.Resources["company_CompareImageType"];
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string path2 = SaveImageFile(dlg.FileName);
                serviceCompanyLogo.Source = new BitmapImage(new Uri(path2));
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

        private void btn_removeChildItem_Click(object sender, RoutedEventArgs e)
        {
            OpeningStockModel productContent = (OpeningStockModel)lvChildProductDetails.SelectedItem;
            OpeningStocks.Remove(productContent);
            lvProductDetails.ItemsSource = OpeningStocks;
            lvChildProductDetails.ItemsSource = OpeningStocks;
            btn_removeChildItem.IsEnabled = false;
            btn_removeChildItem.Background = System.Windows.Media.Brushes.Gray;
        }

        private void cmbTax_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _taxValue = cmbTax.SelectedIndex > 0 ? Convert.ToString(_taxdetails.FirstOrDefault(item => item.TaxCode == Convert.ToInt32(this.cmbTax.SelectedValue)).Rate) : null;
        }

        private void product_name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(product_name.Text))
            {
                var productId = controller.GetProductIdByName(product_name.Text);
                if (productId != null)
                {
                    product_name.Text = string.Empty;
                    productNameExists = true;
                    Common.ErrorMessage((string)Application.Current.Resources["product_exists"], header);
                }
                else
                    productNameExists = false;
            }
        }

        private void check_IsnumericValue(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var isNumericValue = Common.isNumeric(textBox.Text, System.Globalization.NumberStyles.AllowDecimalPoint);
            if (!isNumericValue)
                textBox.Text = string.Empty;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lbAutoCat.Visibility = Visibility.Collapsed;
        }

        private void lvUsers_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);

        }

        private void lvUsers_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }
        #endregion

        private void category_code_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void product_name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
