using FinPos.Client.Controllers;
using FinPos.Domain.DataContracts;
using FinPos.Utility.CommonMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using NLog;
using FinPos.Utility.Constants;
using FinPos.DomainContracts.DataContracts;
using FinPos.Client.CommonFunction;
using FinPos.Client.Controls;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Company.xaml
    /// </summary>

    public partial class Company : Page
    {
        private GridViewColumnHeader listViewSortCol = null;
        private Sort listViewSortAdorner = null;
        private IList<CompanyModel> _companies;
        private CompanyController controller = new CompanyController();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //  Main mainWind = new Main();
        private string msg = string.Empty;
        BrushConverter color = new BrushConverter();
        public string header = (string)Application.Current.Resources["company_Title"];
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        public Company()
        {
            InitializeComponent();
            ChangeHeightWidth();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            _companies = controller.GetCompanies().OrderByDescending(x => x.CreatedDate).ToList<CompanyModel>();

            //_companies = controller.GetCompanies(UserModelVm.CompanyId).ToList<CompanyModel>();
            lvUsers.ItemsSource = _companies;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvUsers.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            btn_addCompany.IsEnabled = true;
            edit_Company.IsEnabled = false;
            edit_Company.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            btn_viewBranch.IsEnabled = false;
            btn_viewBranch.Background = Brushes.Gray;

        }
        public void ChangeHeightWidth()
        {
            this.CompanyPage.Height = HeightWidth.Height - 65;
            this.CompanyPage.Width = HeightWidth.width;

        }
        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = Convert.ToString(column.Tag);
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvUsers.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new Sort(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvUsers.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));


        }
        public T GetChild<T>(DependencyObject obj) where T : DependencyObject
        {
            DependencyObject child = null;
            for (Int32 i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child.GetType() == typeof(T))
                {
                    break;
                }
                else if (child != null)
                {
                    child = GetChild<T>(child);
                    if (child != null && child.GetType() == typeof(T))
                    {
                        break;
                    }
                }
            }
            return child as T;
        }
        private void lvUsers_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);
            dynamic row = e.Source;
            dynamic company = row.DataContext;
            if (item != null || item.IsSelected)
            {
                edit_Company.IsEnabled = true;
                btn_clear.IsEnabled = true;
                btn_viewBranch.IsEnabled = true;
                edit_Company.Background = (Brush)color.ConvertFrom(CommonConstants._greenColorCode);
                btn_clear.Background = (Brush)color.ConvertFrom(CommonConstants._redColorCode);
                btn_viewBranch.Background = (Brush)color.ConvertFrom(CommonConstants._greenColorCode);
                tbCompanyIsActive.Text = company.IsActive ? "In Active" : "Active";
                btn_active.Visibility = Visibility.Visible;
                btn_active.Background = (Brush)color.ConvertFrom(CommonConstants._greenColorCode);
            }
        }

        private void btn_addCompany_Click(object sender, RoutedEventArgs e)
        {
            AddCompany addCompany = new AddCompany();
            NavigationService.Navigate(addCompany);
        }
        private void edit_Company_Click(object sender, RoutedEventArgs e)
        {
            var row = lvUsers.SelectedItem;
            EditCompany _editCompany = new EditCompany(row);
            NavigationService.Navigate(_editCompany);
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            lvUsers.SelectedItem = null;
            btn_addCompany.IsEnabled = true;
            edit_Company.IsEnabled = false;
            edit_Company.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            btn_viewBranch.IsEnabled = false;
            btn_viewBranch.Background = Brushes.Gray;
            btn_active.Visibility = Visibility.Collapsed;
        }

        private void btn_viewBranch_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvUsers.SelectedItem;
            ViewBranch viewBranch = new ViewBranch(row.Id, row.Name);
            NavigationService.Navigate(viewBranch);
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            var text = cmpny_search.Text.ToLower();
            lvUsers.ItemsSource = _companies.Where(x => Convert.ToString(x.Id.Value).Contains(text) || x.Name.ToLower().Contains(text)).ToList();
            CollectionViewSource.GetDefaultView(lvUsers.ItemsSource).Refresh();
        }

        //private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        //{
        //    GridViewColumnHeader column = (sender as GridViewColumnHeader);
        //    string sortBy = column.Tag.ToString();
        //    if (listViewSortCol != null)
        //    {
        //        AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
        //        lvUsers.Items.SortDescriptions.Clear();
        //    }

        //    ListSortDirection newDir = ListSortDirection.Ascending;
        //    if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
        //        newDir = ListSortDirection.Descending;

        //    listViewSortCol = column;
        //    listViewSortAdorner = new Sort(listViewSortCol, newDir);
        //    AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
        //    lvUsers.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        //}

        private void TextBlock_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        //void GridViewColumnClickEventHandler(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    if (e.OriginalSource is CheckBox)
        //    {
        //        //do nothing
        //    }
        //    else
        //    {
        //        //process your logic
        //    }
        //}
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error(e.ExceptionObject);
        }

        private void btn_active_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvUsers.SelectedItem;
            if (row.IsDefault == true)
            {
                //  msg = "Can't Inactive default company";
                ConfirmationPopup confirmPopUp = new ConfirmationPopup((string)Application.Current.Resources["company_InactiveErrorMsg"], "Company", false);
                confirmPopUp.ShowDialog();
                return;
            }
            CompanyModel company = new CompanyModel(row.Id, row.Name, row.Description, row.PhoneNo, row.Logo, row.IsDefault, row.IsActive == true ? false : true, row.CreatedBy, row.UpdatedDate, row.ModifiedBy, row.CreatedBy);
            controller.SaveUpdateCompany(company);
            Window yourParentWindow = Window.GetWindow(this);
            if (yourParentWindow.GetType().Name == "Main")
            {
                var page = yourParentWindow as Main;
                page.BindCompanyCMBFiltered();
                //   (Main)yourParentWindow.
            }
            List<CompanyModel> companies = controller.GetCompanies().ToList();
            _companies = companies;
            lvUsers.ItemsSource = _companies;
            string status = row.IsActive == true ? "In Active" : "Active";
            msg = "Company " + status + " Successfully.";
            //   ConfirmationPopup form1 = new ConfirmationPopup(msg, "Company", false);
            Common.Notification(msg, header, false);
            // form1.ShowDialog();
            edit_Company.IsEnabled = false;
            edit_Company.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            btn_active.Visibility = Visibility.Collapsed;
            btn_viewBranch.IsEnabled = false;
            btn_viewBranch.Background = Brushes.Gray;
        }

        private void cmpny_search_KeyUp(object sender, KeyEventArgs e)
        {
            var text = cmpny_search.Text.ToLower();
            lvUsers.ItemsSource = _companies.Where(x => Convert.ToString(x.Id.Value).Contains(text) || x.Name.ToLower().Contains(text)).ToList();
            CollectionViewSource.GetDefaultView(lvUsers.ItemsSource).Refresh();
            edit_Company.IsEnabled = false;
            edit_Company.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            btn_active.Visibility = Visibility.Collapsed;
            btn_viewBranch.IsEnabled = false;
            btn_viewBranch.Background = Brushes.Gray;
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        { }
        //    lvUsers.Focus();


        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            cmpny_search.Text = string.Empty;
            lvUsers.ItemsSource = _companies.Where(x => Convert.ToString(x.Id.Value).Contains(cmpny_search.Text) || x.Name.ToLower().Contains(cmpny_search.Text)).ToList();
            CollectionViewSource.GetDefaultView(lvUsers.ItemsSource).Refresh();
            SetTextOnSearch();
        }

        private void SetTextOnSearch()
        {
            if (string.IsNullOrEmpty(cmpny_search.Text))
            {
                cmpny_search.Text = "Search";
            }
        }

        private void cmpny_search_GotFocus(object sender, RoutedEventArgs e)
        {
            cmpny_search.Text = string.Empty;
        }

        private void cmpny_search_LostFocus(object sender, RoutedEventArgs e)
        {
            SetTextOnSearch();
        }

        private void lvUsers_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
            
        }

        private void lvUsers_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }
    }
}

