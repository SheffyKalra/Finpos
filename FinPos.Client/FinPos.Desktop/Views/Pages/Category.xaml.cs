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
using System.Windows.Navigation;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Category.xaml
    /// </summary>
    public partial class Category : Page
    {
        #region Properties
        private GridViewColumnHeader listViewSortCol = null;
        private Sort listViewSortAdorner = null;
        private IList<CategoryModel> _categories;
        private CategoryController controller = new CategoryController();
        private ProductController productController = new ProductController();
        GridLengthConverter gridLengthConverter = new GridLengthConverter();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string header = "Category";
        BrushConverter color = new BrushConverter();
        private string msg = string.Empty;
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        #endregion

        #region Constructor
        public Category()
        {
            InitializeComponent();
            ChangeHeightWidth();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            ButtonSettings();
            BindCategories();
        }
        #endregion

        #region Common Methods
        private void BindCategories()
        {
            _categories = controller.GetCategoriesByCompanyId();
            if (_categories.Count > 0)
            {
                lvCategories.ItemsSource = _categories;
            }
        }

        public void ChangeHeightWidth()
        {
            this.CategoryPage.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.CategoryPage.Width = HeightWidth.width;
        }
        private void ButtonSettings()
        {
            btn_editCategory.IsEnabled = false;
            btn_editCategory.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            btn_Delete.IsEnabled = false;
            btn_Delete.Background = Brushes.Gray;
        }
        private void SetTextOnSearch()
        {
            if (string.IsNullOrEmpty(category_search.Text))
            {
                category_search.Text = "Search";
            }
        }
        #endregion

        #region Events
        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = Convert.ToString(column.Tag);
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvCategories.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new Sort(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvCategories.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
        //private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        //{
        //    dynamic row = lvCategories.SelectedItem;
        //    Common.ShowConfirmationPopup((string)Application.Current.Resources["conformation_CategoryMsg"], header,true);
        //    if (Common._isChecked)
        //    {
        //        var result = controller.DeleteCategory(row.WastageId);
        //        BindCategories();
        //        Common.Notification((string)Application.Current.Resources["delete_CategorySuccess"], header, false);
        //    }
        //}
        private void addCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategory _addCategory = new AddCategory();
            NavigationService.Navigate(_addCategory);
        }
        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            lvCategories.SelectedItem = null;
            addCategory.Visibility = Visibility.Visible;
            btn_editCategory.IsEnabled = false;
            btn_editCategory.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            btn_Delete.IsEnabled = false;
            btn_Delete.Background = Brushes.Gray;
            btnIsactive.Visibility = Visibility.Collapsed;
        }
        private void btn_editCategory_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvCategories.SelectedItem;
            EditCategory editCategory = new EditCategory(row);
            NavigationService.Navigate(editCategory);
        }
        private void btn_search_click(object sender, RoutedEventArgs e)
        {
            var text = category_search.Text.ToLower();
            lvCategories.ItemsSource = _categories.Where(x => Convert.ToString(x.Id.Value).Contains(text) || x.CategoryName.ToLower().Contains(text)).ToList();
            CollectionViewSource.GetDefaultView(lvCategories.ItemsSource).Refresh();
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error(e.ExceptionObject);
        }
        private void delete_category_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvCategories.SelectedItem;
            Common.ShowConfirmationPopup((string)Application.Current.Resources["conformation_CategoryMsg"], header,true);
            if (Common._isChecked)
            {
                var result = controller.DeleteCategory(row.Id);
                if (result == true)
                {
                    BindCategories();
                    Common.Notification((string)Application.Current.Resources["delete_CategorySuccess"], header, false);
                }
                else
                    Common.ErrorMessage((string)Application.Current.Resources["error_CategoryMsg"], header);

                ButtonSettings();
                btnIsactive.Visibility = Visibility.Collapsed;
            }
        }
        private void btn_isActive_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvCategories.SelectedItem;
            CategoryModel category = new CategoryModel(row.Id, row.CategoryName, row.Description, row.BranchCode, row.IsDeleted, row.CreatedDate, row.Createdby, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), row.ModifiedBy, row.IsActive == true ? false : true, string.Empty, row.CompanyCode);
            controller.SaveUpdateCategory(category);
            BindCategories();
            Common.Notification((string)Application.Current.Resources["update_CategoryMsg"], header, false);
            ButtonSettings();
            btnIsactive.Visibility = Visibility.Collapsed;
        }
        private void category_search_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var text = category_search.Text.ToLower();
            lvCategories.ItemsSource = _categories.Where(x => Convert.ToString(x.Id.Value).Contains(text) || x.CategoryName.ToLower().Contains(text)).ToList();
            CollectionViewSource.GetDefaultView(lvCategories.ItemsSource).Refresh();
            btn_editCategory.IsEnabled = false;
            btn_editCategory.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
            btn_Delete.IsEnabled = false;
            btn_Delete.Background = Brushes.Gray;
            btnIsactive.Visibility = Visibility.Collapsed;
        }


        private void category_search_GotFocus(object sender, RoutedEventArgs e)
        {
            category_search.Text = string.Empty;
        }

        private void category_search_LostFocus(object sender, RoutedEventArgs e)
        {
            SetTextOnSearch();
        }

        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            category_search.Text = string.Empty;
            lvCategories.ItemsSource = _categories.Where(x => Convert.ToString(x.Id.Value).Contains(category_search.Text) || x.CategoryName.ToLower().Contains(category_search.Text)).ToList();
            CollectionViewSource.GetDefaultView(lvCategories.ItemsSource).Refresh();
            lvCategories.Focus();
            SetTextOnSearch();
        }

        private void lvUsers_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lvUsers_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }
        #endregion 

        #region PreviewRow
        private void lvCategory_PreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            var item = (sender as ListViewItem);
            dynamic row = e.Source;
            dynamic category = row.DataContext;
            if (item != null || item.IsSelected)
            {
                btn_editCategory.IsEnabled = true;
                btn_editCategory.Background = (Brush)color.ConvertFrom(CommonConstants._greenColorCode);
                btn_clear.IsEnabled = true;
                btn_clear.Background = (Brush)color.ConvertFrom(CommonConstants._redColorCode);
                btn_Delete.IsEnabled = true;
                btn_Delete.Background = (Brush)color.ConvertFrom(CommonConstants._redColorCode);
                btnIsactive.Visibility = Visibility.Visible;
                btnIsactive.Background = (Brush)color.ConvertFrom(CommonConstants._greenColorCode);
                isActiveBlock.Text = category.IsActive ? "In Active" : "Active";
            }
        }
        #endregion

    }
}
