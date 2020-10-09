using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Management;
using System.Printing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MasterLabelSetting.xaml
    /// </summary>
    public partial class MasterLabelSetting : Page
    {
        ProductController productController = new ProductController();
        private IList<ProductModel> _products;
        private ObservableCollection<ProductLabelSettingModel> purchaseStocks;
        public ListViewItem item = null;
        public int rowIndex = 0;
        private int count = 0;
        public ProductLabelSettingModel _selectedStock;
        public MasterLabelSettingModel _masterLabel;
        public List<ProductLabelSettingModel> _productLabelSetting;
        BrushConverter color = new BrushConverter();
        List<Printers> objPrinters = new List<Printers>();
        private static IntPtr windowHandle;
        public MasterLabelSetting()
        {
            InitializeComponent();
            btn_remove.IsEnabled = false;
            btn_remove.Background = Brushes.Gray;
            ResponseVm responce = productController.GetProductsByCompanyAndBranch();
            _masterLabel = productController.GetMasaterLabelSetting();
            BindPrinters();

            GridLengthConverter gridLengthConverter = new GridLengthConverter();
            if (responce.FaultData == null)
            {
                _products = responce.Response.Cast<ProductModel>().ToList();
                dgPurchases.ItemsSource = _products;
                dgPurchases.Visibility = Visibility.Visible;
            }
            if (_masterLabel != null)
            {
                List<ProductLabelSettingModel> productLabel = productController.GetProductLabelSettings(_masterLabel.Id.Value);
                _productLabelSetting = productLabel?.Select(x => new ProductLabelSettingModel(x.Id, x.MasterLabelCode, x.ProductCode, x.Quantity, _products.Any(j => j.Id == x.ProductCode) == true ? _products.Where(z => z.Id == x.ProductCode).FirstOrDefault().ItemName : string.Empty)).ToList();
                purchaseStocks = new ObservableCollection<ProductLabelSettingModel>(_productLabelSetting);
                print_item_code.IsChecked = _masterLabel.PrintItemCode ? true : false;
                print_item_price.IsChecked = _masterLabel.PrintItemPrice ? true : false;
                chk_print_barcode.IsChecked = _masterLabel.PrintBarCode ? true : false;
                chk_item_detail.IsChecked = _masterLabel.PrintItemDetail ? true : false;
                if (_masterLabel.BarCodeHeight != "")
                {
                    //bar_code_height.SelectedItem = (Common.barcodeHeight)Enum.Parse(typeof(Common.barcodeHeight), _masterLabel.BarCodeHeight); //;Enum.GetValues(typeof(CommonFunction.Common.barcodeHeight));
                }

            }
            if (purchaseStocks == null)
            {
                additems();
            }
            lstPurchase.ItemsSource = purchaseStocks;
        }

        #region Plug & Play Printer Code
        private void BindPrinters()
        {
            Printers objPrintre = new Printers();
            objPrinters = new List<Printers>();
            objPrintre.Id = 0;
            objPrintre.Name = "Select";
            objPrinters.Add(objPrintre);
            ValidatePrinters(objPrinters);
            objPrinters = objPrinters.Where(item => item.Name != "fax").ToList<Printers>();
            cmbPrinters.ItemsSource = objPrinters;
            cmbPrinters.SelectedValue = "Id";
            cmbPrinters.DisplayMemberPath = "Name";
            cmbPrinters.SelectedIndex = 0;
        }



        private List<Printers> ValidatePrinters(List<Printers> objList)
        {
            Thread thread = new Thread(() =>
            {
                Printers objPrintre = new Printers();
                string printerName;
                ManagementObjectSearcher theSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
                foreach (ManagementObject mo in theSearcher.Get())
                {
                    printerName = mo["Name"].ToString().ToLower();

                    if (!mo["WorkOffline"].ToString().ToLower().Equals("true"))
                    {
                        objPrintre = new Printers();
                        objPrintre.Id = count++;
                        objPrintre.Name = printerName;
                        objList.Add(objPrintre);
                    }
                }
            });
            thread.Start();
            thread.Join();
            return objList;
        }


        public void Usb_DeviceAdded()
        {
            CommonFunction.Common.Notification((string)Application.Current.Resources["Device_Added"], "", false);
            BindPrinters();
        }

        public void Usb_DeviceRemoved()
        {
            CommonFunction.Common.Notification((string)Application.Current.Resources["Device_Removed"], "", false);
            BindPrinters();
            //throw new NotImplementedException();
        }
        #endregion

        public void additems()
        {
            purchaseStocks = new ObservableCollection<ProductLabelSettingModel>();
            purchaseStocks.Add(new ProductLabelSettingModel() { ProductName = "" });
        }
        private void dgPurchase_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProductModel productItem = (ProductModel)dgPurchases.SelectedItem;
            productPopUp.IsOpen = false;
            this.IsEnabled = true;
            stock_AddItem(productItem);
        }
        public void stock_AddItem(ProductModel itemToAdd)
        {
            if (purchaseStocks.Any(x => x.ProductCode == itemToAdd.Id) && _selectedStock.ProductCode != itemToAdd.Id)
            {
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["purchase_already"], "Master Label", true);
                form.ShowDialog();
                if (Common._isChecked)
                {
                    AddpurchaseItemSource(itemToAdd);
                }
            }
            else
            {
                AddpurchaseItemSource(itemToAdd);
            }
        }
        private void AddpurchaseItemSource(ProductModel purchaseModel)
        {
            ProductLabelSettingModel _purchaseStock = new ProductLabelSettingModel();
            _purchaseStock.ProductName = purchaseModel.ItemName;
            _purchaseStock.ProductCode = purchaseModel.Id.Value;
            purchaseStocks[rowIndex] = _purchaseStock;
            lstPurchase.ItemsSource = purchaseStocks;
        }
        private void btn_AddRow_Click(object sender, RoutedEventArgs e)
        {
            purchaseStocks.Add(new ProductLabelSettingModel() { ProductName = "" });
        }
        private void PreviewTextInputHandler(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }
        private void TextBox_KeyUp_1(object sender, KeyEventArgs e)
        {
            var textbox = sender as TextBox;
            if (e.Key == Key.Space)
            {
                textbox.Text = Regex.Replace(textbox.Text, @"\s+", "");
                e.Handled = true;
            }
        }

        private void print_item_price_Checked(object sender, RoutedEventArgs e)
        {
            lb_print_price.Visibility = Visibility.Visible;
        }

        private void print_item_price_Unchecked(object sender, RoutedEventArgs e)
        {
            lb_print_price.Visibility = Visibility.Collapsed;
        }

        private void print_item_code_Checked(object sender, RoutedEventArgs e)
        {
            // lb_print_Ic.Content = ItemId;
            lb_print_Id.Visibility = Visibility.Visible;
        }

        private void print_item_code_Unchecked(object sender, RoutedEventArgs e)
        {
            lb_print_Id.Visibility = Visibility.Collapsed;
        }

        private void chk_item_detail_Checked(object sender, RoutedEventArgs e)
        {
            lb_print_Ic.Visibility = Visibility.Visible;
        }

        private void chk_item_detail_Unchecked(object sender, RoutedEventArgs e)
        {
            lb_print_Ic.Visibility = Visibility.Collapsed;
        }

        private void chk_print_barcode_Checked(object sender, RoutedEventArgs e)
        {
            lb_print_bc.Visibility = Visibility.Visible;
        }

        private void chk_print_barcode_Unchecked(object sender, RoutedEventArgs e)
        {
            lb_print_bc.Visibility = Visibility.Collapsed;
        }

        //private void dgPurchase_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    item = (sender as ListViewItem);
        //  //  btn_remove.IsEnabled = true;
        //   // btn_remove.Background = (Brush)color.ConvertFrom("#eb5151");
        //}

        //public static FixedDocument GetFixedDocument(FrameworkElement toPrint, PrintDialog printDialog)
        //{
        //    PrintCapabilities capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
        //    Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
        //    Size visibleSize = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
        //    FixedDocument fixedDoc = new FixedDocument();
        //    //If the toPrint visual is not displayed on screen we neeed to measure and arrange it  
        //    toPrint.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        //    toPrint.Arrange(new Rect(new Point(0, 0), toPrint.DesiredSize));
        //    //  
        //    Size size = toPrint.DesiredSize;
        //    //Will assume for simplicity the control fits horizontally on the page  
        //    double yOffset = 0;
        //    while (yOffset < size.Height)
        //    {
        //        VisualBrush vb = new VisualBrush(toPrint);
        //        vb.Stretch = Stretch.None;
        //        vb.AlignmentX = AlignmentX.Left;
        //        vb.AlignmentY = AlignmentY.Top;
        //        vb.ViewboxUnits = BrushMappingMode.Absolute;
        //        vb.TileMode = TileMode.None;
        //        vb.Viewbox = new Rect(0, yOffset, visibleSize.Width, visibleSize.Height);
        //        PageContent pageContent = new PageContent();
        //        FixedPage page = new FixedPage();
        //        ((IAddChild)pageContent).AddChild(page);
        //        fixedDoc.Pages.Add(pageContent);
        //        page.Width = pageSize.Width;
        //        page.Height = pageSize.Height;
        //        Canvas canvas = new Canvas();
        //        FixedPage.SetLeft(canvas, capabilities.PageImageableArea.OriginWidth);
        //        FixedPage.SetTop(canvas, capabilities.PageImageableArea.OriginHeight);
        //        canvas.Width = visibleSize.Width;
        //        canvas.Height = visibleSize.Height;
        //        canvas.Background = vb;
        //        page.Children.Add(canvas);
        //        yOffset += visibleSize.Height;
        //    }
        //    return fixedDoc;
        //}
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPrinters.SelectedIndex != 0)
            {
                MasterLabelSettingModel model = new MasterLabelSettingModel(0, 0, print_item_code.IsChecked.Value, chk_item_detail.IsChecked.Value, "0", print_item_price.IsChecked.Value, chk_print_barcode.IsChecked.Value, "", "", string.Empty, "", "");
                //MasterLabelSettingModel model = new MasterLabelSettingModel(0, 0, print_item_code.IsChecked.Value, chk_item_detail.IsChecked.Value, "0", print_item_price.IsChecked.Value, chk_print_barcode.IsChecked.Value, bar_code_height.Text, label_sheet_dd.SelectedValue.ToString(), string.Empty, nud_start_row.Value.ToString(), nud_start_column.Value.ToString());
                //  int masterLabelId = controller.SaveUpdateMasterLabel(model);
                List<ProductLabelSettingModel> newStocks = new List<ProductLabelSettingModel>();
                List<ProductLabelSettingModel> Stocks = lstPurchase.Items.Cast<ProductLabelSettingModel>().Select(x => x).ToList();
                if (Stocks.Any(x => x.ProductCode > 0 && x.Quantity > 0))
                {
                    // stocks = lstPurchase.ItemsSource.Cast<StockModel>().ToList();
                    newStocks.AddRange(Stocks.Where(z => z.ProductCode > 0 && z.Quantity > 0).Select(x =>
                    {
                        return new ProductLabelSettingModel(0, 0, Convert.ToInt32(x.ProductCode), x.Quantity, x.ProductName);
                        // newStocks.Add(stock);
                    }).ToList());
                }
                MasterPrintInvoice form = new MasterPrintInvoice(model, newStocks);

                PrintDialog printDlg = new PrintDialog();
                //PageMediaSizeName pazesize = (PageMediaSizeName)Enum.Parse(typeof(PageMediaSizeName), CommonConstants._pazeSize + model.LabelSheet);
                //printDlg.PrintTicket.PageMediaSize = new PageMediaSize(pazesize);
                PageSettings obj = GetPrinterPageInfo(this.cmbPrinters.Text);


                #region CommentedCode
                ///  System.Windows.Forms.PrintPreviewDialog printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();

                //  if (printDlg.ShowDialog() == true)

                //  {
                //PrintCapabilities capabilities = printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);
                //Size pageSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);
                //Size visibleSize = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
                // FixedDocument fixedDoc = new FixedDocument();
                ////If the toPrint visual is not displayed on screen we neeed to measure and arrange it   
                //form.TvBox.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                //form.TvBox.Arrange(new Rect(new Point(0, 0), form.TvBox.DesiredSize));
                ////   
                //Size size = form.TvBox.DesiredSize;
                ////Will assume for simplicity the control fits horizontally on the page   
                //double yOffset = 0;
                //while (yOffset < size.Height)
                //{
                //    VisualBrush vb = new VisualBrush(form.TvBox);
                //    vb.Stretch = Stretch.None;
                //    vb.AlignmentX = AlignmentX.Left;
                //    vb.AlignmentY = AlignmentY.Top;
                //    vb.ViewboxUnits = BrushMappingMode.Absolute;
                //    vb.TileMode = TileMode.None;
                //    vb.Viewbox = new Rect(0, yOffset, visibleSize.Width, visibleSize.Height);

                //    PageContent pageContent = new PageContent();
                //    FixedPage page = new FixedPage();
                //    ((IAddChild)pageContent).AddChild(page);
                //    fixedDoc.Pages.Add(pageContent);
                //    form.TvBox.Width = printDlg.PrintableAreaWidth; //pageSize.Width;
                //    form.TvBox.Height = pageSize.Height;
                //    Canvas canvas = new Canvas();
                //    FixedPage.SetLeft(canvas, capabilities.PageImageableArea.OriginWidth);
                //    FixedPage.SetTop(canvas, capabilities.PageImageableArea.OriginHeight);
                //    canvas.Width = visibleSize.Width;
                //    canvas.Height = visibleSize.Height;
                //    canvas.Background = vb;
                //    canvas.Margin = new Thickness(2, 2, 2, 2);

                //    page.Children.Add(canvas);
                //    yOffset += visibleSize.Height;
                //}
                //;
                //  return fixedDoc;
                // form.TvBox.MaxWidth = pageSize.Width;
                #endregion
                #region EarlierCode
                //// get selected printer capabilities
                //System.Printing.PrintCapabilities capabilities = printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);
                ////get scale of the print wrt to screen of WPF visual

                //double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / this.ActualWidth, capabilities.PageImageableArea.ExtentHeight /
                //              this.ActualHeight);
                ////  //Transform the Visual to scale
                //form.TvBox.LayoutTransform = new ScaleTransform(scale, scale);
                //form.TvBox.Margin = new Thickness(5, 5, 5, 5);
                //form.Height = 1150;
                ////  //get the size of the printer page
                //Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
                ////  //update the layout of the visual to the printer page size.
                //form.TvBox.Height = capabilities.PageImageableArea.ExtentHeight;
                //form.TvBox.Width = capabilities.PageImageableArea.ExtentWidth;
                //form.TvBox.Measure(sz);
                //////  form.Width = 850;
                //form.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

                ////  now print the visual to printer to fit on the one page.
                //for (var i = 0; i <= Convert.ToInt32(model.StartRow); i++)
                //{
                //    if (i == Convert.ToInt32(model.StartRow))
                //    {
                //        printDlg.PrintVisual(form.TvBox, "First Fit to Page WPF Print");
                //    }
                //}
                // printDlg.PrintDocument(fixedDoc.DocumentPaginator, "My Document");


                #endregion

                //PrintDialog pd = new PrintDialog();
                //pd.PrintQueue = new PrintQueue(new PrintServer(), cmbPrinters.Text);
                //if (DialogResult.OK == pd.ShowDialog(this))
                //{
                //    // Send a printer-specific to the printer.
                //    RawPrinterHelper.SendStringToPrinter(cmbPrinters.Text, "Check");
                //}
                //GenerateThermalLabelWorker(form);
                form.TvBox.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                form.TvBox.Arrange(new Rect(new Point(0, 0), form.TvBox.DesiredSize));
                PrintDialog dialog = new PrintDialog();
                StackPanel myPanel = new StackPanel();
                myPanel.RenderSize = form.TvBox.DesiredSize;

                foreach (PrintMasterSettingModel item in form.TvBox.Items)
                {

                    TextBlock myBlock = new TextBlock();
                    myBlock.Text = "Item :" + item.ProductName;
                    myBlock.TextAlignment = System.Windows.TextAlignment.Center;
                    myPanel.Children.Add(myBlock);
                    Image myImage = new Image();
                    myImage.Width = item.ImageData.Width;
                    myImage.Height = item.ImageData.Height;
                    myImage.Stretch = Stretch.None;
                    myImage.Source = item.ImageData;
                    myPanel.Children.Add(myImage);
                    TextBlock myBlock1 = new TextBlock();
                    myBlock1.Text = "Price :" + item.Price;
                    myBlock1.TextAlignment = System.Windows.TextAlignment.Center;
                    myPanel.Children.Add(myBlock1);
                }
                myPanel.Arrange(new Rect(0, 20, form.TvBox.DesiredSize.Width,
                      form.TvBox.DesiredSize.Height));
                dialog.PrintQueue = new PrintQueue(new PrintServer(), cmbPrinters.Text);
                //dialog.PrintDocument()
                //  dialog.ShowDialog();

                //System.Windows.Forms.PrintPreviewDialog printPrvDlg = new System.Windows.Forms.PrintPreviewDialog();
                //printPrvDlg.Document = myPanel;
                //printPrvDlg.Width = 1200;
                //printPrvDlg.Height = 800;
                //printPrvDlg.ShowDialog();
                dialog.PrintVisual(myPanel, "A Great Image.");

                //PrintCapabilities capabilities = printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);
                //Size pageSize = new Size(obj.Bounds.Width, obj.Bounds.Height);
                //Size visibleSize = new Size(obj.Bounds.Width, obj.Bounds.Height);
                //FixedDocument fixedDoc = new FixedDocument();

                ////If the toPrint visual is not displayed on screen we neeed to measure and arrange it  
                //form.TvBox.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                //form.TvBox.Arrange(new Rect(new Point(0, 0), form.TvBox.DesiredSize));
                ////  
                //Size size = form.TvBox.DesiredSize;
                ////Will assume for simplicity the control fits horizontally on the page  
                //double yOffset = 0;
                //while (yOffset < size.Height)
                //{
                //    VisualBrush vb = new VisualBrush(form.TvBox);
                //    vb.Stretch = Stretch.None;
                //    vb.AlignmentX = AlignmentX.Left;
                //    vb.AlignmentY = AlignmentY.Top;
                //    vb.ViewboxUnits = BrushMappingMode.Absolute;
                //    vb.TileMode = TileMode.None;
                //    vb.Viewbox = new Rect(0, yOffset, visibleSize.Width, visibleSize.Height);
                //    PageContent pageContent = new PageContent();
                //    FixedPage page = new FixedPage();
                //    ((IAddChild)pageContent).AddChild(page);
                //    fixedDoc.Pages.Add(pageContent);
                //    page.Width = pageSize.Width;
                //    page.Height = pageSize.Height;
                //    Canvas canvas = new Canvas();
                //    FixedPage.SetLeft(canvas, capabilities.PageImageableArea.OriginWidth);
                //    FixedPage.SetTop(canvas, capabilities.PageImageableArea.OriginHeight);
                //    canvas.Width = visibleSize.Width;
                //    canvas.Height = visibleSize.Height;
                //    canvas.Background = vb;
                //    page.Children.Add(canvas);
                //    yOffset += visibleSize.Height;
                //}
                ////for (var i = 0; i <= Convert.ToInt32(model.StartRow); i++)
                ////{
                ////    if (i == Convert.ToInt32(model.StartRow))
                ////    {
                ////        printDlg.PrintVisual(form.TvBox, "First Fit to Page WPF Print");
                ////    }
                ////}
                ////printDlg.ShowDialog();
                //printDlg.PrintQueue = new PrintQueue(new PrintServer(), cmbPrinters.Text);
                ////PrintDocument pd=printDlg.
                //printDlg.PrintDocument(fixedDoc.DocumentPaginator, "First Fit to Page WPF Print");

            }
            else
            {
                ConfirmationPopup obj = new ConfirmationPopup((string)Application.Current.Resources["select_printer"], (string)Application.Current.Resources["masterLabel_Header_Popup"], false);
                obj.ShowDialog();
                //CommonFunction.Common.((string)Application.Current.Resources["select_printer"], "", false);
            }
        }
        public static PageSettings GetPrinterPageInfo(String printerName)
        {
            System.Drawing.Printing.PrinterSettings settings;

            // If printer name is not set, look for default printer
            if (String.IsNullOrEmpty(printerName))
            {
                foreach (var printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    settings = new System.Drawing.Printing.PrinterSettings();

                    settings.PrinterName = printer.ToString();

                    if (settings.IsDefaultPrinter)
                        return settings.DefaultPageSettings;
                }

                return null; // <- No default printer  
            }

            // printer by its name 
            settings = new System.Drawing.Printing.PrinterSettings();

            settings.PrinterName = printerName;

            return settings.DefaultPageSettings;
        }
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {

            inventory product = new inventory();
            NavigationService.Navigate(product);
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            productController.DeleteMasterLabelSetting();
            productController.DeleteProductLabelSetting();
            MasterLabelSettingModel model = new MasterLabelSettingModel(0, 0, print_item_code.IsChecked.Value, chk_item_detail.IsChecked.Value, "0", print_item_price.IsChecked.Value, chk_print_barcode.IsChecked.Value, "", "", string.Empty, "", "");
            //MasterLabelSettingModel model = new MasterLabelSettingModel(0, 0, print_item_code.IsChecked.Value, chk_item_detail.IsChecked.Value, "0", print_item_price.IsChecked.Value, chk_print_barcode.IsChecked.Value, bar_code_height.Text, label_sheet_dd.SelectedValue.ToString(), string.Empty, nud_start_row.Value.ToString(), nud_start_column.Value.ToString());
            int masterLabelId = productController.SaveUpdateMasterLabel(model);
            List<ProductLabelSettingModel> newStocks = new List<ProductLabelSettingModel>();
            List<ProductLabelSettingModel> Stocks = lstPurchase.Items.Cast<ProductLabelSettingModel>().Select(x => x).ToList();
            if (Stocks.Any(x => x.ProductCode > 0 && x.Quantity > 0))
            {
                // stocks = lstPurchase.ItemsSource.Cast<StockModel>().ToList();
                newStocks.AddRange(Stocks.Where(z => z.ProductCode > 0 && z.Quantity > 0).Select(x =>
                {
                    return new ProductLabelSettingModel(0, masterLabelId, Convert.ToInt32(x.ProductCode), x.Quantity, x.ProductName);
                    // newStocks.Add(stock);
                }).ToList());
            }
            productController.SaveProductLabel(newStocks);
            //inventory product = new inventory();
            //  NavigationService.Navigate(product);
        }

        private void print_unit_measure_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            productPopUp.IsOpen = false;
            this.IsEnabled = true;
        }
        private void txtProduct_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            productPopUp.IsOpen = true;
            this.IsEnabled = false;
            System.Windows.Controls.ListViewItem lvi = GetAncestorByType(
    e.OriginalSource as DependencyObject, typeof(System.Windows.Controls.ListViewItem)) as System.Windows.Controls.ListViewItem;
            if (lvi != null)
            {
                lstPurchase.SelectedIndex =
                    lstPurchase.ItemContainerGenerator.IndexFromContainer(lvi);
                rowIndex = lstPurchase.SelectedIndex;
                _selectedStock = (ProductLabelSettingModel)lstPurchase.SelectedItem;
            }
        }
        public static DependencyObject GetAncestorByType(DependencyObject element, Type type)
        {
            if (element == null) return null;
            if (element.GetType() == type) return element;
            return GetAncestorByType(VisualTreeHelper.GetParent(element), type);
        }

        private void product_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textBoxName = (System.Windows.Controls.TextBox)sender;
            string filterText = textBoxName.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(dgPurchases.ItemsSource);

            if (cv == null)
                return;
            if (!string.IsNullOrEmpty(filterText))
            {

                cv.Filter = o =>
                {
                    /* change to get data row value */
                    ProductModel p = o as ProductModel;
                    return (p.ItemName.ToUpper().Contains(filterText.ToUpper()) || p.BarCode.ToUpper().Contains(filterText.ToUpper()) || Convert.ToString(p.Id).ToUpper().Contains(filterText.ToUpper()));
                    /* end change to get data row value */
                };

            }
            else
                cv.Filter = null;
        }
        private void lstPurchase_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            item = (sender as ListViewItem);
            btn_remove.IsEnabled = true;
            btn_remove.Background = (Brush)color.ConvertFrom("#eb5151");
        }

        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            purchaseStocks.Remove((ProductLabelSettingModel)lstPurchase.SelectedItem);
        }

        private void lstPurchase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #region Search Box
        private void txt_search_GotFocus(object sender, RoutedEventArgs e)
        {
            txt_search.Text = string.Empty;
        }
        private void txt_search_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt_search.Text))
            {
                txt_search.Text = "Search";
            }
        }
        private void txt_search_KeyUp(object sender, KeyEventArgs e)
        {
            dgPurchases.ItemsSource = _products.Where(p => p.ItemName.ToUpper().Contains(txt_search.Text.ToUpper()) || p.BarCode.ToUpper().Contains(txt_search.Text.ToUpper()) || Convert.ToString(p.Id).ToUpper().Contains(txt_search.Text.ToUpper())).ToList();
            CollectionViewSource.GetDefaultView(dgPurchases.ItemsSource).Refresh();
            //SetTextOnSearch();
        }
        private void SetTextOnSearch()
        {
            if (string.IsNullOrEmpty(txt_search.Text))
            {
                txt_search.Text = "Search";
            }
        }
        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            RefreshList();
            SetTextOnSearch();
        }

        private void RefreshList()
        {
            txt_search.Text = string.Empty;
            dgPurchases.ItemsSource = _products.Where(p => p.ItemName.ToUpper().Contains(txt_search.Text.ToUpper()) || p.BarCode.ToUpper().Contains(txt_search.Text.ToUpper()) || Convert.ToString(p.Id).ToUpper().Contains(txt_search.Text.ToUpper())).ToList();
            CollectionViewSource.GetDefaultView(dgPurchases.ItemsSource).Refresh();
        }
        #endregion


    }

}
