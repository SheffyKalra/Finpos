using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FinPos.Client.Views.UserControls;
using FinPos.Client.Controllers;
using System.Windows.Navigation;
using FinPos.DomainContracts.DataContracts;
using FinPos.Domain.DataContracts;
using FinPos.Client.Animations;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Interop;
using FinPos.Client.Controls;
using System.Windows.Threading;
using FinPos.Client.Views.Pages;
using FinPos.Utility.CommonMethods;
//using FinPos.Client.Model;

namespace FinPos.Client.Views
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private static IntPtr windowHandle;
        public IList<CompanyModel> _companies;
        public IList<CompanyBranchVm> _companieWithBranch;
        private int _companyId;
        private BranchModel _branch;
        private string _companyName;
        UserModelVm _userModel;
        public CompanyController controller = new CompanyController();
        private int PrevTabIndex { get; set; }

        public Main()
        {
            InitializeComponent();
            finposTabControl.SelectedIndex = 5;
            PrevTabIndex = 5;

            FormLayoutGrid.ColumnDefinitions[0].Width = new System.Windows.GridLength(System.Windows.SystemParameters.PrimaryScreenWidth);
            // lblDate.Content = DateTime.Now.Date;
        }
        public Main(UserModel user)
        {

            _companieWithBranch = new List<CompanyBranchVm>();
            InitializeComponent();
            //  lblDate.Content = DateTime.Now.Date;
            _companies = controller.GetCompanies().OrderByDescending(x => x.CreatedDate).ToList();
            CompanyModel companyModel = _companies.FirstOrDefault(x => x.IsActive == true);
            foreach (var company in _companies)//int i = 0; i < _companies.Count(); i++)
            {
                List<BranchModel> branchs = controller.GetCompanyBranches(company.Id.Value).Response.Cast<BranchModel>().ToList();
                _companieWithBranch.Add(new CompanyBranchVm(company, branchs));
                if (company.IsDefault)
                {
                    _branch = branchs.FirstOrDefault(x => x.IsDefault == true);
                }
            }

            _userModel = new UserModelVm(user.Id.Value, user.FirstName + " " + user.LastName, user.Initials, user.Email, companyModel.Id.Value, companyModel.Name, _branch?.Id, _branch?.Name);
            BindCompanyCMBFiltered();
            txtInitials.Text = user.Initials;
            txtFullName.Text = user.FirstName + " " + user.LastName;
            txtDate.Text = CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString());
            finposTabControl.SelectedIndex = 5;
            PrevTabIndex = 5;

            //txtCompanyHeader.Text = companyModel.Name;
        }

        //public void BindCompanyCMB()
        //{

        //    this.cmbCompanyName.ItemsSource = _companies;
        //    this.cmbCompanyName.DisplayMemberPath = "Name";
        //    this.cmbCompanyName.SelectedValue = "Id";
        //    foreach (var items in _companies)
        //    {
        //        if (items.IsDefault)
        //        {
        //            this.cmbCompanyName.Text = items.Name;
        //        }
        //    }
        //}
        public void BindCompanyCMBFiltered()
        {
            try
            {


                this.cmbCompanyName.ItemsSource = null;
                CompanyModel obj = new CompanyModel(0, (string)Application.Current.Resources["Combo_Select"]);
                IList<CompanyModel> companies = controller.GetActiveCompanies().ToList();
                companies = companies.Where(item => item.IsActive == true).ToList();
                companies.Insert(0, obj);
                this.cmbCompanyName.ItemsSource = companies;
                this.cmbCompanyName.DisplayMemberPath = "Name";
                this.cmbCompanyName.SelectedValue = "Id";
                foreach (var items in companies)
                {
                    if (items.IsDefault)
                    {
                        this.cmbCompanyName.Text = items.Name;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_supplier_click(object sender, RoutedEventArgs e)
        {
            btn_supplier.Focusable = true;
            btn_supplier.Focus();
            Supplier Page = new Supplier();
            this.FinposContainer.Navigate(Page);
            e.Handled = true;
        }
        #region ClickEvents
        private void manageCompanyClick(object sender, RoutedEventArgs e)
        {
            btn_company.Focusable = true;
            btn_company.Focus();
            Company targetPage = new Company();
            this.FinposContainer.Navigate(targetPage);
            e.Handled = true;
        }

        private void btn_branch_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }
        private void logout_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void Payroll_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void Accounts_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void Inventory_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }
        private void btn_wastage_Click(object sender, RoutedEventArgs e)
        {
            //  this.finposTabControl.SelectedIndex = 11;

            /// Wastage targetPage = new Wastage();
            //EditProductHistory targetPage = new EditProductHistory(null);

            Wastage targetPage = new Wastage();

            this.FinposContainer.Navigate(targetPage);
            e.Handled = true;
            //this.FinposContainer.NavigateToPage(targetPage);
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void btn_product_Click(object sender, RoutedEventArgs e)
        {
            btn_product.Focusable = true;
            btn_product.Focus();
            inventory targetPage = new inventory();

            this.FinposContainer.Navigate(targetPage);
            e.Handled = true;
        }
        private void btn_category_Click(object sender, RoutedEventArgs e)
        {
            btn_category.Focusable = true;
            btn_category.Focus();
            Category targetPage = new Category();
            this.FinposContainer.Navigate(targetPage);
            e.Handled = true;
        }


        #endregion

        #region MenuSelectionChange
        private void finposTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e != null)
            {
                TabControl tabControl = sender as TabControl;
                var _tab = e.OriginalSource as TabControl;

                if (tabControl.IsMouseOver == false && this.finposTabControl.SelectedIndex != 5)
                {
                    this.finposTabControl.SelectedIndex = PrevTabIndex;
                    return;
                }
                else if (tabControl.IsMouseOver)
                {
                    PrevTabIndex = tabControl.SelectedIndex;
                }
                else
                {
                    btn_company.Focusable = true;
                    btn_company.Focus();
                    return;
                }
                if (_tab != null)
                {
                    switch (_tab.SelectedIndex)
                    {
                        case 0:
                            btn_customer.Focusable = true;
                            btn_customer.Focus();
                            break;
                        case 1:
                            btn_supplier.Focusable = true;
                            btn_supplier.Focus();
                            btn_supplier.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                            break;
                        case 2:
                            btn_category.Focusable = true;
                            btn_category.Focus();
                            btn_category.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                            break;
                        case 3:
                            btn_expense.Focusable = true;
                            btn_expense.Focus();
                            break;
                        case 4:
                            btn_manageSallary.Focusable = true;
                            btn_manageSallary.Focus();
                            break;
                        case 5:
                            btn_company.Focusable = true;
                            btn_company.Focus();
                            btn_company.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                            break;
                    }
                }
            }
            else
            {

                var contentpage = FinposContainer.Content as Page;
                var value = contentpage.GetType();
                switch (value.Name)
                {
                    case "Purchase":
                        //  this.FinposContainer.Navigate(contentpage);
                        btnEvent(btn_purchase);
                        //btn_purchase.Focusable = true;
                        //btn_purchase.Focus();
                        //btn_purchase.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                        break;
                    case "Supplier":
                        // this.FinposContainer.Navigate(contentpage);
                        btnEvent(btn_supplier);
                        break;
                    case "DirectPurchase":
                        btnEvent(btn_directPurchase);
                        break;
                    case "Category":
                        btnEvent(btn_category);
                        break;
                    case "OpeningStock":
                        btnEvent(btn_opening_stock);
                        break;
                    case "Wastage":
                        btnEvent(btn_wastage);
                        break;
                    case "StockAdjustment":
                        btnEvent(btn_stockAdjustment);
                        break;
                    case "inventory":
                        btnEvent(btn_product);
                        break;
                    case "MasterLabelSetting":
                        //btnEvent(btn_MasterLabel);
                        break;
                    case "Tax":
                        btnEvent(btn_Tax);
                        break;
                    case "AddProductHistory":
                        AddProductHistory objAddProduct = new AddProductHistory();
                        this.FinposContainer.Navigate(objAddProduct);
                        break;
                    case "AddWastage":
                        AddWastage objAddWastage = new AddWastage();
                        this.FinposContainer.Navigate(objAddWastage);
                        break;
                    case "AddPurchase":
                        AddPurchase objAddPurchase = new AddPurchase();
                        this.FinposContainer.Navigate(objAddPurchase);
                        break;
                    case "AddDirectPurchase":
                        AddDirectPurchase objAddDirectPurchase = new AddDirectPurchase();
                        this.FinposContainer.Navigate(objAddDirectPurchase);
                        break;
                    case "Repack":
                        Repack objRepack = new Repack();
                        this.FinposContainer.Navigate(objRepack);
                        break;
                    case "CoupanManagment":
                        btnEvent(btnCoupons);
                        break;
                        //case "Tax":
                        //    btnEvent(btn_Tax);
                        //    break;
                        //case "Tax":
                        //    btnEvent(btn_Tax);
                        // break;
                }
            }

            //else
            //{
            //    finposTabControl.SelectedIndex = this.finposTabControl.SelectedIndex;
            //}
            //case 10:
            //    btn_product.Focusable = true;
            //    btn_product.Focus();
            //    btn_product.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            //    break;
            //case 11:
            //    btn_wastage.Focusable = true;
            //    btn_wastage.Focus();
            //    btn_wastage.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            //    break;

            #endregion

        }
        public void ChangeIndex(int index)
        {
            this.finposTabControl.SelectedIndex = index;
        }
        private void btnEvent(Button btnName)
        {
            btnName.Focusable = true;
            btnName.Focus();
            btnName.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
        //private void btn_opening_stock_Click(object sender, RoutedEventArgs e)
        //{

        //    StockTransferOut os = new StockTransferOut();

        //    this.FinposContainer.Navigate(os);
        //}


        private void btn_opening_stock_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            OpeningStock os = new OpeningStock();
            this.FinposContainer.Navigate(os);
        }

        private void ddlTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var type = e.NewValue.GetType();
            if (Convert.ToString(type.Name) != "CompanyBranchVm")
            {
                var branch = ((BranchModel)e.NewValue);
                // ddlCompany.Text = branch.Name;
                var company = _companies.FirstOrDefault(x => x.Id.Value == branch.CompanyId);
                UserModelVm.CompanyId = company.Id.Value;
                UserModelVm.CompanyName = company.Name;
                UserModelVm.BranchId = branch.Id.Value;
                UserModelVm.BranchName = branch.Name;
            }
        }

        private void btn_purchase_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Purchase Page = new Purchase();
            this.FinposContainer.Navigate(Page);
        }

        private void btn_stockAdjustment_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            StockAdjustment stockAdjustment = new StockAdjustment();
            this.FinposContainer.Navigate(stockAdjustment);
        }

        private void btn_directPurchase_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            DirectPurchase Page = new DirectPurchase();
            this.FinposContainer.Navigate(Page);
        }
        private void btn_MasterLabel_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            MasterLabelSetting page = new MasterLabelSetting();
            this.FinposContainer.Navigate(page);
        }
        /// <summary>
        /// this method is created for setting values in Product searchable textbox
        /// this method is called at two diffrent places one at arrow click which will populate all products if query serach will be empty
        /// </summary>
        /// <param name="sender"></param>
        //private void ReusableCodeTemp(string callingMethod)
        //{
        //    try
        //    {
        //        bool found = false;
        //        var border = (lbAutoCat.Parent as ScrollViewer).Parent as Border;
        //        lbAutoCat.Items.Clear();
        //        string query = (txtCompany as TextBox).Text;

        //        if (query.Length == 0)
        //        {
        //            if (callingMethod == "Companyarrow_Click")
        //            {
        //                foreach (var obj in _companies)
        //                {
        //                    //if (obj.ItemName.ToLower().StartsWith(query.ToLower()))
        //                    //{
        //                    addProductName(obj.Name, obj.Id.Value);
        //                    found = true;
        //                    //}
        //                }
        //                //resultStack.Children.Clear();
        //                border.Visibility = Visibility.Visible;
        //                lbAutoCat.Visibility = Visibility.Visible;
        //            }
        //            else
        //            {

        //                border.Visibility = Visibility.Collapsed;
        //            }
        //        }
        //        else
        //        {
        //            foreach (var obj in _companies)
        //            {
        //                if (obj.Name.ToLower().StartsWith(query.ToLower()))
        //                {
        //                    addProductName(obj.Name, obj.Id.Value);
        //                    found = true;
        //                }
        //            }
        //            border.Visibility = Visibility.Visible;
        //            lbAutoCat.Visibility = Visibility.Visible;
        //        }
        //        if (!found)
        //        {
        //            lbAutoCat.Items.Add(new TextBlock() { Text = "No results found." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        //private void addProductName(string text, int code)
        //{
        //    try
        //    {
        //        var border = (lbAutoCat.Parent as ScrollViewer).Parent as Border;
        //        TextBlock block = new TextBlock();
        //        block.Text = text;
        //        block.Margin = new Thickness(2, 3, 2, 3);
        //        block.Cursor = Cursors.Hand;
        //        block.MouseLeftButtonUp += (sender, e) =>
        //        {

        //            txtCompany.Text = (sender as TextBlock).Text;
        //            _companyId = code;
        //            border.Visibility = Visibility.Collapsed;
        //        };

        //        block.MouseEnter += (sender, e) =>
        //        {
        //            BrushConverter bc = new BrushConverter();
        //            TextBlock b = sender as TextBlock;
        //            b.Background = (Brush)bc.ConvertFrom("#fff");
        //        };

        //        block.MouseLeave += (sender, e) =>
        //        {
        //            TextBlock b = sender as TextBlock;
        //            b.Background = Brushes.Transparent;
        //        };

        //        // Add to the panel 
        //        lbAutoCat.Items.Add(block);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        private void txtCompany_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            e.Handled = true;
        }

        //private void Companyarrow_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var border = (lbAutoCat.Parent as ScrollViewer).Parent as Border;
        //        if (border.Visibility == Visibility.Visible)
        //        {
        //            border.Visibility = Visibility.Collapsed;
        //        }
        //        else
        //        {
        //            ReusableCodeTemp("Companyarrow_Click");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        private void lbAutoCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbCompanyName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCompanyName.SelectedIndex > 0)
            {
                int value = finposTabControl.SelectedIndex;
                var val = (CompanyModel)cmbCompanyName.SelectedValue;
                UserModelVm.CompanyId = Convert.ToInt32(val.Id);
                UserModelVm.CompanyName = val.Name;
                UserModelVm.BranchId = null;
                UserModelVm.BranchName = null;
                BindBranchCMBFiltered(val.Id.ToString(), "0");
                //this.FinposContainer.Navigate(content);
                finposTabControl.SelectedIndex = value;
                NavigateToPreviousPage();
            }

            e.Handled = true;

        }

        private void NavigateToPreviousPage()
        {
            var content = FinposContainer.Content as Page;
            if (content != null)
                finposTabControl_SelectionChanged(content, null);
        }

        public void BindBranchCMBFiltered(string companyId, string branch)
        {
            List<BranchModel> branchs = controller.GetCompanyActiveBranches(Convert.ToInt32(companyId)).Response.Cast<BranchModel>().ToList();
            BranchModel objSelect = new BranchModel(0, 0, (string)Application.Current.Resources["Combo_Select"], (string)Application.Current.Resources["Combo_Select"], "", false, true, "", "", "", "");
            branchs.Insert(0, objSelect);
            if (branchs.Count > 0)
            {
                this.cmbBranchName.ItemsSource = branchs;
                this.cmbBranchName.DisplayMemberPath = "Name";
                this.cmbBranchName.SelectedValue = "Id";
                if (branch == "0")
                {
                    this.cmbBranchName.SelectedIndex = 0;
                }
                else
                {
                    this.cmbBranchName.Text = branch;
                }
            }
            else
            {
                this.cmbBranchName.ItemsSource = null;
            }

        }
        //public void BindBranchCMB(string companyId, string branchId)
        //{
        //    BranchModel objSelect = new BranchModel("Select", 0, "Select");
        //    this.cmbBranchName.ItemsSource = null;
        //    List<BranchModel> branchs = controller.GetCompanyBranches(Convert.ToInt32(companyId)).Response.Cast<BranchModel>().ToList();
        //    branchs.Insert(0, objSelect);
        //    if (branchs.Count > 0)
        //    {
        //        this.cmbBranchName.ItemsSource = branchs;
        //        this.cmbBranchName.DisplayMemberPath = "Name";
        //        this.cmbBranchName.SelectedValue = "Id";
        //        this.cmbBranchName.SelectedValue = branchId;
        //    }
        //    else
        //    {
        //        this.cmbBranchName.ItemsSource = null;
        //    }
        //}

        private void cmbBranchName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBranchName.SelectedIndex > 0)
            {
                var val = (BranchModel)cmbBranchName.SelectedValue;
                if (val != null)
                {
                    var content = FinposContainer.Content as Page;
                    int value = finposTabControl.SelectedIndex;
                    UserModelVm.BranchId = val.Id;
                    UserModelVm.BranchName = val.Name;
                    finposTabControl.SelectedIndex = value;
                    //this.FinposContainer.Navigate(content);
                    finposTabControl.SelectedIndex = value;
                    if (content != null)
                        finposTabControl_SelectionChanged(content, null);
                }
            }
            else
            {
                var content = FinposContainer.Content as Page;
                int value = finposTabControl.SelectedIndex;
                UserModelVm.BranchId = null;
                UserModelVm.BranchName = null;
                finposTabControl.SelectedIndex = value;
                //this.FinposContainer.Navigate(content);
                finposTabControl.SelectedIndex = value;
                if (content != null)
                    finposTabControl_SelectionChanged(content, null);
            }
            e.Handled = true;

        }

        private void btn_Tax_Click(object sender, RoutedEventArgs e)
        {
            Tax Page = new Tax();
            this.FinposContainer.Navigate(Page);
        }

        #region Plug & Play Printer Code
        protected override void OnSourceInitialized(EventArgs e)
        {

            base.OnSourceInitialized(e);
            // Adds the windows message processing hook and registers USB device add/removal notification.
            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            if (source != null)
            {

                windowHandle = source.Handle;
                source.AddHook(HwndHandler);
                UsbNotification.RegisterUsbDeviceNotification(windowHandle);
            }
        }

        /// <summary>
        /// Method that receives window messages.
        /// </summary>
        private IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == UsbNotification.WmDevicechange)
            {
                switch ((int)wparam)
                {
                    case UsbNotification.DbtDeviceremovecomplete:
                        var contentpage = FinposContainer.Content as Page;
                        var value = contentpage.GetType();
                        if (value.Name == "MasterLabelSetting")
                        {
                            var content = FinposContainer.Content as MasterLabelSetting;
                            content.Usb_DeviceAdded();
                        }
                        break;
                    case UsbNotification.DbtDevicearrival:
                        var content1 = FinposContainer.Content as Page;
                        var value1 = content1.GetType();
                        if (value1.Name == "MasterLabelSetting")
                        {
                            var content = FinposContainer.Content as MasterLabelSetting;
                            content.Usb_DeviceRemoved();
                        }
                        break;
                }
            }
            handled = false;
            return IntPtr.Zero;
        }
        #endregion

        private void btn_repack_Click(object sender, RoutedEventArgs e)
        {
            Repack Page = new Repack();
            this.FinposContainer.Navigate(Page);
            e.Handled = true;
        }
        private void btnCoupons_Click(object sender, RoutedEventArgs e)
        {
            CoupanManagment Page = new CoupanManagment();
            this.FinposContainer.Navigate(Page);
            e.Handled = true;
        }

        private void paymentTosupplier_Click(object sender, RoutedEventArgs e)
        {
            //PaymentToSupplier page = new PaymentToSupplier();
            //this.FinposContainer.Navigate(page);
            e.Handled = true;
        }

        private void FinposContainer_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var height = ((System.Windows.FrameworkElement)sender).ActualHeight;
            var width = ((System.Windows.FrameworkElement)sender).ActualWidth;
            var leftpnlht = leftpanel.ActualHeight;
            var leftpnlwith = leftpanel.ActualWidth;
            HeightWidth.Height = height - leftpnlht;
            HeightWidth.width = width - leftpnlwith;
            var contentpage = FinposContainer.Content as Page;
            if (contentpage != null)
            {
                var value = contentpage.GetType().Name;
                switch (value)
                {
                    case "Tax":
                        var page = contentpage as Tax;
                        page.ChangeHeightWidth();
                        break;
                    case "AddTax":
                        var addTax = contentpage as AddTax;
                        addTax.ChangeHeightWidth();
                        break;
                    case "EditTax":
                        var editTax = contentpage as EditTax;
                        editTax.ChangeHeightWidth();
                        break;
                    case "AddBranch":
                        var addBranch = contentpage as AddBranch;
                        addBranch.ChangeHeightWidth();
                        break;
                    case "EditBranch":
                        var editBranch = contentpage as EditBranch;
                        editBranch.ChangeHeightWidth();
                        break;
                    case "ViewBranch":
                        var viewBranch = contentpage as ViewBranch;
                        viewBranch.ChangeHeightWidth();
                        break;
                    case "Company":
                        var company = contentpage as Company;
                        company.ChangeHeightWidth();
                        break;
                    case "AddCompany":
                        var addCompany = contentpage as AddCompany;
                        addCompany.ChangeHeightWidth();
                        break;
                    case "EditCompany":
                        var editCompany = contentpage as EditCompany;
                        editCompany.ChangeHeightWidth();
                        break;
                    case "Category":
                        var category = contentpage as Category;
                        category.ChangeHeightWidth();
                        break;
                    case "AddCategory":
                        var addCategory = contentpage as AddCategory;
                        addCategory.ChangeHeightWidth();
                        break;
                    case "EditCategory":
                        var editCategory = contentpage as EditCategory;
                        editCategory.ChangeHeightWidth();
                        break;
                    case "inventory":
                        var inventory = contentpage as inventory;
                        inventory.ChangeHeightWidth();
                        break;
                    case "AddProductHistory":
                        var addProduct = contentpage as AddProductHistory;
                        addProduct.ChangeHeightWidth();
                        break;
                    case "EditProductHistory":
                        var editProduct = contentpage as EditProductHistory;
                        editProduct.ChangeHeightWidth();
                        break;
                    case "OpeningStock":
                        var openingStock = contentpage as OpeningStock;
                        openingStock.ChangeHeightWidth();
                        break;
                    case "StockAdjustment":
                        var stockAdjustment = contentpage as StockAdjustment;
                        stockAdjustment.ChangeHeightWidth();
                        break;
                    case "Wastage":
                        var wastage = contentpage as Wastage;
                        wastage.ChangeHeightWidth();
                        break;
                    case "AddWastage":
                        var addWastage = contentpage as AddWastage;
                        addWastage.ChangeHeightWidth();
                        break;
                    case "EditWastage":
                        var editWastage = contentpage as EditWastage;
                        editWastage.ChangeHeightWidth();
                        break;
                    case "Supplier":
                        var supplier = contentpage as Supplier;
                        supplier.ChangeHeightWidth();
                        break;
                    case "AddSupplier":
                        var addSupplier = contentpage as AddSupplier;
                        addSupplier.ChangeHeightWidth();
                        break;
                    case "EditSupplier":
                        var editSupplier = contentpage as EditSupplier;
                        editSupplier.ChangeHeightWidth();
                        break;
                    case "Purchase":
                        var purchase = contentpage as Purchase;
                        purchase.ChangeHeightWidth();
                        break;
                    case "AddPurchase":
                        var addPurchase = contentpage as AddPurchase;
                        addPurchase.ChangeHeightWidth();
                        break;
                    case "EditPurchase":
                        var editPurchase = contentpage as EditPurchase;
                        editPurchase.ChangeHeightWidth();
                        break;
                    case "DirectPurchase":
                        var directPurchase = contentpage as DirectPurchase;
                        directPurchase.ChangeHeightWidth();
                        break;
                    case "AddDirectPurchase":
                        var addDirectPurchase = contentpage as AddDirectPurchase;
                        addDirectPurchase.ChangeHeightWidth();
                        break;
                    case "EditDirectPurchase":
                        var editDirectPurchase = contentpage as EditDirectPurchase;
                        editDirectPurchase.ChangeHeightWidth();
                        break;
                    case "PaymentToSupplier":
                        var paymentToSupplier = contentpage as PaymentToSupplier;
                        paymentToSupplier.ChangeHeightWidth();
                        break;
                    case "AddPaymentToSupplier":
                        var addPaymentToSupplier = contentpage as AddPaymentToSupplier;
                        addPaymentToSupplier.ChangeHeightWidth();
                        break;
                    case "EditPaymentToSupplier":
                        var editPaymentToSupplier = contentpage as EditPaymentToSupplier;
                        editPaymentToSupplier.ChangeHeightWidth();
                        break;
                    case "AddCoupon":
                        var addCoupon = contentpage as AddCoupon;
                        addCoupon.ChangeHeightWidth();
                        break;
                    case "EditCoupon":
                        var EditCoupon = contentpage as EditCoupon;
                        EditCoupon.ChangeHeightWidth();
                        break;
                    case "Repack":
                        var repack = contentpage as Repack;
                        repack.ChangeHeightWidth();
                        break;
                    case "CoupanManagment":
                        var CoupanManagment = contentpage as CoupanManagment;
                        CoupanManagment.ChangeHeightWidth();
                        break;
                    

                }
            }
        }
    }
}
