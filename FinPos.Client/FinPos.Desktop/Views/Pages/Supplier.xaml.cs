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
using System.Windows.Media;


namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Supplier.xaml
    /// </summary>
    public partial class Supplier : Page
    {
        private GridViewColumnHeader listViewSortCol = null;
        private Sort listViewSortAdorner = null;
        private SupplierController controller = new SupplierController();
        private IList<SupplierModel> _supplier;
        private string msg = string.Empty;
        private string header = "Supplier";
        BrushConverter color = new BrushConverter();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        public Supplier()
        {
            InitializeComponent();
            ChangeHeightWidth();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            _supplier = controller.GetSuppliersByCompanyAndBrach(UserModelVm.CompanyId,UserModelVm.BranchId).OrderBy(x => x.Id).ToList<SupplierModel>();
            lvSuppliers.ItemsSource = _supplier;
            btn_delete.IsEnabled = false;
            btn_delete.Background = Brushes.Gray;
            btn_clearSupplier.IsEnabled = false;
            btn_clearSupplier.Background = Brushes.Gray;
            edit_Supplier.IsEnabled = false;
            edit_Supplier.Background = Brushes.Gray;
        }
        public void ChangeHeightWidth()
        {
            this.SupplierPage.Height = HeightWidth.Height - 65;
            this.SupplierPage.Width = HeightWidth.width;

        }
        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = Convert.ToString(column.Tag);
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvSuppliers.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new Sort(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvSuppliers.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));


        }
        private void lvSupplier_PreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            var item = (sender as ListViewItem);

            if (item != null || item.IsSelected)
            {
                btn_delete.IsEnabled = true;
                btn_delete.Background = (Brush)color.ConvertFrom(CommonConstants._redColorCode);
                btn_clearSupplier.IsEnabled = true;
                btn_clearSupplier.Background = (Brush)color.ConvertFrom(CommonConstants._redColorCode);
                edit_Supplier.IsEnabled = true;
                edit_Supplier.Background = (Brush)color.ConvertFrom(CommonConstants._greenColorCode);
            }

        }
        private void btn_addSupplier_Click(object sender, RoutedEventArgs e)
        {
            AddSupplier _addSupplier = new AddSupplier();
            NavigationService.Navigate(_addSupplier);

        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvSuppliers.SelectedItem;
            ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["supplier_ConformationMsg"], header, true);
            form.ShowDialog();
            if (Common._isChecked)
            {
                controller.DeleteSupplier(row.Id);
                _supplier = controller.GetSuppliersByCompanyAndBrach(UserModelVm.CompanyId, UserModelVm.BranchId).OrderBy(x => x.Id).ToList<SupplierModel>();
                lvSuppliers.ItemsSource = _supplier;
                //  msg = "Supplier deleted successfully";
                // ConfirmationPopup form1 = new ConfirmationPopup(msg, header, false);
                //  form1.ShowDialog();
                Common.Notification((string)Application.Current.Resources["supplier_DeletedMsg"], header, false);
                DisableButtons();
            }
        }


        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            var text = supplier_search.Text.ToLower();

            lvSuppliers.ItemsSource = _supplier.Where(x => Convert.ToString(x.Id.Value).Contains(text) || x.SupplierName.ToLower().Contains(text)).ToList();
            CollectionViewSource.GetDefaultView(lvSuppliers.ItemsSource).Refresh();
        }

        private void btn_clearSupplier_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
        }

        private void DisableButtons()
        {
            lvSuppliers.SelectedItem = null;
            btn_delete.IsEnabled = false;
            btn_delete.Background = Brushes.Gray;
            btn_clearSupplier.IsEnabled = false;
            btn_clearSupplier.Background = Brushes.Gray;
            edit_Supplier.IsEnabled = false;
            edit_Supplier.Background = Brushes.Gray;
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error(e.ExceptionObject);
        }

        private void edit_Supplier_Click(object sender, RoutedEventArgs e)
        {
            var row = lvSuppliers.SelectedItem;
            EditSupplier editSupplier = new EditSupplier(row);
            NavigationService.Navigate(editSupplier);
        }

        private void supplier_search_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            lvSuppliers.ItemsSource = _supplier.Where(x => Convert.ToString(x.Id.Value).Contains(supplier_search.Text) || x.SupplierName.ToLower().Contains(supplier_search.Text.ToLower())).ToList();
            CollectionViewSource.GetDefaultView(lvSuppliers.ItemsSource).Refresh();
            btn_clearSupplier.IsEnabled = false;
            btn_clearSupplier.Background = Brushes.Gray;
            btn_delete.IsEnabled = false;
            btn_delete.Background = Brushes.Gray;
            edit_Supplier.IsEnabled = false;
            edit_Supplier.Background = Brushes.Gray;
        }
        private void SetTextOnSearch()
        {
            if (string.IsNullOrEmpty(supplier_search.Text))
            {
                supplier_search.Text = "Search";
            }
        }
        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            supplier_search.Text = string.Empty;
            lvSuppliers.ItemsSource = _supplier.Where(x => Convert.ToString(x.Id.Value).Contains(string.Empty) || x.SupplierName.ToLower().Contains(string.Empty)).ToList();
            CollectionViewSource.GetDefaultView(lvSuppliers.ItemsSource).Refresh();
            SetTextOnSearch();
        }
        private void supplier_search_GotFocus(object sender, RoutedEventArgs e)
        {
            supplier_search.Text = string.Empty;
        }

        private void supplier_search_LostFocus(object sender, RoutedEventArgs e)
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
