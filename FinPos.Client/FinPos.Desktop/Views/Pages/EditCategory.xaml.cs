using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonMethods;
using NLog;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for EditCategory.xaml
    /// </summary>
    public partial class EditCategory : Page
    {
        #region Properties
        private string msg = string.Empty;
        public dynamic seletecRow;
        public int RowId;
        private string header = "Edit category";
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructor
        public EditCategory(dynamic row)
        {
            InitializeComponent();
            ChangeHeightWidth();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            SetText(row);
        }
        #endregion

        #region Common Methods
        private void SetText(dynamic row)
        {
            seletecRow = row;
            txtName.Text = row.CategoryName;
            txtDescription.Text = row.Description;
            category_isActive.IsChecked = row.IsActive;
            lblEditCatagory.Content = "Edit (" + row.CategoryName + ")";
            RowId = row.Id;
        }

        public void ChangeHeightWidth()
        {
            this.EditCategoryPage.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.EditCategoryPage.Width = HeightWidth.width;
        }
        private void SaveCategory(CategoryController controller)
        {
            CategoryModel _category = new CategoryModel(RowId, txtName.Text, txtDescription.Text, UserModelVm.BranchId, false, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), 1, null, null, category_isActive.IsChecked.Value, string.Empty, UserModelVm.CompanyId);
            controller.SaveUpdateCategory(_category);
            ClearFields();
            Common.Notification((string)Application.Current.Resources["category_UpdatedSuccessMsg"], header, false);
            GoToBackPage();
        }
        private void GoToBackPage()
        {
            Category category = new Category();
            NavigationService.Navigate(category);
        }
        private void ClearFields()
        {
            txtName.Text = "";
            txtDescription.Text = "";
        }
        #endregion
       
        #region Events
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtDescription.Text))
            {
                Common.ErrorMessage((string)Application.Current.Resources["error_message_Tax"], header);
            }
            else
            {
                try
                {
                    CategoryController controller = new CategoryController();
                    if (controller.GetCategoriesByCompanyId().Any(x => x.CategoryName.ToLower() == txtName.Text.ToLower() && x.Id != RowId))
                    {
                        Common.ErrorMessage((string)Application.Current.Resources["Errorcategory_AlreadyExist"], header);
                    }
                    else
                    {
                        SaveCategory(controller);
                    }
                }
                catch (Exception ex)
                {
                    Common.ErrorMessage(ex.Message, header);
                }
            }
        }
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            GoToBackPage();
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error(e.ExceptionObject);
        }
        #endregion

    }
}
