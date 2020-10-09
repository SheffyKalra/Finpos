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
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddCategory : Page
    {
        #region Properties
        private string msg = string.Empty;
        private string header = (string)Application.Current.Resources["add_category"]; 
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion
        #region Constructor
        /// <summary>
        public AddCategory()
        {
            InitializeComponent();
            ChangeHeightWidth();
            ClearFileds();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }
        #endregion
        #region Common Methods
        public void ChangeHeightWidth()
        {
            this.AddCategoryPage.Height = HeightWidth.Height - HeightWidth.DefaultHeight;
            this.AddCategoryPage.Width = HeightWidth.width;

        }
        public void ClearFileds()
        {
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
        }
        private void GoToBackPage()
        {
            Category _category = new Category();
            NavigationService.Navigate(_category);

        }
        #endregion
        #region CRUD Operation

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                Common.ErrorMessage((string)Application.Current.Resources["commonFieldsError_Msg"], header);
            }
            else
            {
                try
                {
                    CategoryController controller = new CategoryController();

                    if (controller.GetCategoriesByCompanyId().Any(x => x.CategoryName.ToLower() == txtName.Text.ToLower()))
                    {
                        Common.ErrorMessage((string)Application.Current.Resources["Errorcategory_AlreadyExist"], header);
                    }
                    else
                    {
                        CategoryModel _category = new CategoryModel(0, txtName.Text, txtDescription.Text, UserModelVm.BranchId ?? null, false, CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString()), 1, null, null, category_isActive.IsChecked.Value, string.Empty, UserModelVm.CompanyId);
                        controller.SaveUpdateCategory(_category);
                        ClearFileds();
                        Common.Notification((string)Application.Current.Resources["category_SavedSuccessMsg"], header, false);
                        GoToBackPage();
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
