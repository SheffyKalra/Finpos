using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonMethods;
using FinPos.Utility.Constants;
using NLog;
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
using System.Windows.Navigation;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ViewBranch.xaml
    /// </summary>
    public partial class ViewBranch : Page
    {
        CompanyController controller = new CompanyController();
        int companyId;
        IList<BranchModel> branches;
        private GridViewColumnHeader listViewSortCol = null;
        private Sort listViewSortAdorner = null;
        BrushConverter color = new BrushConverter();
        public string _compamnyName = string.Empty;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string msg = string.Empty;
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        public ViewBranch(dynamic companyId, string companyName)
        {
            InitializeComponent();
            ChangeHeightWidth();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            _compamnyName = companyName;
            this.companyId = companyId;
            ResponseVm responce = controller.GetCompanyBranches(this.companyId);//.OrderByDescending(x => x.CreatedDate).ToList();
            if (responce.FaultData == null)
            {
                branches = responce.Response.Cast<BranchModel>().ToList();
                lvBranch.ItemsSource = branches;
                btn_addBranch.IsEnabled = true;
                btn_back.IsEnabled = true;
                lblBranchHeading.Content = "Branches (" + _compamnyName + ")";
                btn_editBranch.IsEnabled = false;
                btn_editBranch.Background = Brushes.Gray;
                btn_clear.IsEnabled = false;
                btn_clear.Background = Brushes.Gray;

            }
            else
            {
                ConfirmationPopup form = new ConfirmationPopup(responce.FaultData.Detail.ErrorDetails, "Branch", false);
                form.ShowDialog();
                // Common.ErrorNotification(responce.FaultData.Detail.ErrorDetails, "Branch", false);
            }
        }
        public void ChangeHeightWidth()
        {
            this.ViewBranchPage.Height = HeightWidth.Height - 65;
            this.ViewBranchPage.Width = HeightWidth.width;

        }
        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = Convert.ToString(column.Tag);
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvBranch.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new Sort(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvBranch.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));


        }
        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            lvBranch.SelectedItem = null;

            btn_addBranch.IsEnabled = true;
            //btn_delete.IsEnabled = false;
            btn_editBranch.IsEnabled = false;
            btn_clear.IsEnabled = false;

            btn_editBranch.Background = (Brush)color.ConvertFrom("#0091EA");
            btn_clear.Background = (Brush)color.ConvertFrom("#eb5151");
            btn_back.Background = (Brush)color.ConvertFrom("#0091EA");
            btn_IsActive.Visibility = Visibility.Collapsed;
        }

        private void btn_viewBranch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_addBranch_Click(object sender, RoutedEventArgs e)
        {
            if (companyId != null || companyId != 0)
            {
                AddBranch addBranch = new AddBranch(this.companyId, _compamnyName);
                NavigationService.Navigate(addBranch);
            }

        }

        private void edit_Branch_Click(object sender, RoutedEventArgs e)
        {
            var row = lvBranch.SelectedItem;
            EditBranch EditPage = new EditBranch(row, _compamnyName);
            NavigationService.Navigate(EditPage);

        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            Company _company = new Company();
            NavigationService.Navigate(_company);
        }

        private void lvBranch_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            dynamic row = e.Source;
            dynamic branch = row.DataContext;
            if (item != null || item.IsSelected)
            {
                btn_editBranch.IsEnabled = true;
                btn_clear.IsEnabled = true;

                btn_editBranch.Background = (Brush)color.ConvertFrom(CommonConstants._greenColorCode);
                btn_clear.Background = (Brush)color.ConvertFrom(CommonConstants._redColorCode);
                btn_back.Background = (Brush)color.ConvertFrom(CommonConstants._greenColorCode);
                IsActiveBlock.Text = branch.IsActive ? "In Active" : "Active";
                btn_IsActive.Visibility = Visibility.Visible;
                btn_IsActive.Background = (Brush)color.ConvertFrom(CommonConstants._greenColorCode);
            }

        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            var text = txt_search.Text.ToLower();
            IList<BranchModel> model = branches;
            lvBranch.ItemsSource = model.Where(x => Convert.ToString(x.Id).Contains(text) || x.Name.ToLower().Contains(text)).ToList();
            CollectionViewSource.GetDefaultView(lvBranch.ItemsSource).Refresh();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error(e.ExceptionObject);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void btn_IsActive_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvBranch.SelectedItem;
            BranchModel branch = new BranchModel(row.Id, row.CompanyId, row.Name, row.Description, row.Address, row.IsDefault, row.IsActive == true ? false : true, row.CreatedDate, row.UpdatedDate, row.ModifiedBy, row.CreatedBy);
            controller.SaveUpdateBranch(branch);
            Window yourParentWindow = Window.GetWindow(this);
            if (yourParentWindow.GetType().Name == "Main")
            {
                var page = yourParentWindow as Main;
                page.BindBranchCMBFiltered(companyId.ToString(), row.Name);
                //   (Main)yourParentWindow.
            }
            ResponseVm responce = controller.GetCompanyBranches(row.CompanyId);
            List<BranchModel> _branches = responce.Response.Cast<BranchModel>().ToList();
            branches = _branches;
            lvBranch.ItemsSource = _branches;
            // msg = "Branch status has been updated Successfully.";
            // ConfirmationPopup form1 = new ConfirmationPopup(msg, "Branch", false);
            //  form1.ShowDialog();
            Common.Notification((string)Application.Current.Resources["branch_UpdateMsg"], "Branch", false);
            btn_editBranch.IsEnabled = false;
            btn_editBranch.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            btn_IsActive.Visibility = Visibility.Collapsed;
        }

        #region Search Box
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
            lvBranch.ItemsSource = branches.Where(x => Convert.ToString(x.Id.Value).Contains(txt_search.Text) || x.Address.ToLower().Contains(txt_search.Text) || x.Description.ToLower().Contains(txt_search.Text)).ToList();
            CollectionViewSource.GetDefaultView(lvBranch.ItemsSource).Refresh();
        }
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
            lvBranch.ItemsSource = branches.Where(x => Convert.ToString(x.Id.Value).Contains(txt_search.Text) || x.Address.ToLower().Contains(txt_search.Text) || x.Description.ToLower().Contains(txt_search.Text)).ToList();
            CollectionViewSource.GetDefaultView(lvBranch.ItemsSource).Refresh();
            //SetTextOnSearch();
        }
        #endregion

        private void lvBranch_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lvBranch_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }
    }
}
