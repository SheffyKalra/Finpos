using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.Client.Controls;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonMethods;
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
    /// Interaction logic for Wastage.xaml
    /// </summary>
    public partial class Wastage : Page
    {
        private GridViewColumnHeader listViewSortCol = null;
        private Sort listViewSortAdorner = null;
        private ProductController controller = new ProductController();
        private IList<DomainContracts.DataContracts.WastageModel> _wastage;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string msg = string.Empty;
        private string header = "Wastage";
        BrushConverter color = new BrushConverter();
        CommonFunction.Validations objValidation = new CommonFunction.Validations();
        public Wastage()
        {
            InitializeComponent();
            ChangeHeightWidth();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            ResponseVm responce = controller.GetWastage();
            startDate_Search.Text = CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString());
            endDate_search.Text = CommonFunctions.ParseDateToFinclaveString(DateTime.Now.ToShortDateString());
            if (responce.FaultData == null)
            {
                if (UserModelVm.BranchId != null)
                {
                    _wastage = responce.Response.Cast<WastageModel>().ToList().Where(x => x.CompanyCode == UserModelVm.CompanyId && x.BranchCode == UserModelVm.BranchId).OrderByDescending(x => x.Date).Where(x => CommonFunctions.ParseDateToFinclave(x.Date) >= CommonFunctions.ParseDateToFinclave(Settings.FinalYearStartDate) && CommonFunctions.ParseDateToFinclave(x.Date) <= CommonFunctions.ParseDateToFinclave(Settings.FinalYearEndDate)).ToList<WastageModel>();
                }
                else
                {
                    _wastage = responce.Response.Cast<WastageModel>().ToList().Where(x => x.CompanyCode == UserModelVm.CompanyId).OrderByDescending(x => x.Date).Where(x => CommonFunctions.ParseDateToFinclave(x.Date) >= CommonFunctions.ParseDateToFinclave(Settings.FinalYearStartDate) && CommonFunctions.ParseDateToFinclave(x.Date) <= CommonFunctions.ParseDateToFinclave(Settings.FinalYearEndDate)).ToList<WastageModel>();
                }

                //_wastage = responce.Response.Cast<DomainContracts.DataContracts.WastageModel>().OrderByDescending(x => x.Date).Where(x=>Convert.ToDateTime(x.Date)>=Convert.ToDateTime(Settings.FinalYearStartDate) && Convert.ToDateTime(x.Date)<=Convert.ToDateTime(Settings.FinalYearEndDate)).ToList();
                lvWastage.ItemsSource = _wastage;
                //if (lvWastage.Items.Count == 0)
                //     lvWastage.Items.Add("no record found");
                btn_addWastage.IsEnabled = true;
                edit_Wastage.IsEnabled = false;
                edit_Wastage.Background = Brushes.Gray;
                btn_Delete.IsEnabled = false;
                btn_Delete.Background = Brushes.Gray;
                btn_clear.IsEnabled = false;
                btn_clear.Background = Brushes.Gray;
            }
            else
            {
               ConfirmationPopup form = new ConfirmationPopup(responce.FaultData.Detail.ErrorDetails, header, false);
               form.ShowDialog();
              //  Common.ErrorNotification(responce.FaultData.Detail.ErrorDetails, header, false);
            }

        }
        public void ChangeHeightWidth()
        {
            this.WastagePage.Height = HeightWidth.Height - 65;
            this.WastagePage.Width = HeightWidth.width;

        }
        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = Convert.ToString(column.Tag);
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvWastage.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new Sort(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvWastage.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));


        }
       
        private void lvUsers_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);

            if (item != null || item.IsSelected)
            {
                // btn_addCompany.IsEnabled = false;
                //btn_delete.IsEnabled = true;
                edit_Wastage.IsEnabled = true;
                btn_Delete.IsEnabled = true;
                btn_clear.IsEnabled = true;
                // edit_Company.Background = Brushes.ForestGreen;
                edit_Wastage.Background = (Brush)color.ConvertFrom("#0091EA");
                // btn_clear.Background = Brushes.Red;
                btn_Delete.Background = (Brush)color.ConvertFrom("#eb5151");
                btn_clear.Background = (Brush)color.ConvertFrom("#eb5151");

                //   btn_viewBranch.Background = Brushes.ForestGreen;
                //Do your stuff
            }
        }
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(startDate_Search.Text) || string.IsNullOrEmpty(endDate_search.Text))
            {
               ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["date_ErrorMsg"], header, false);
                form.ShowDialog();
              //  Common.ErrorNotification((string)Application.Current.Resources["date_ErrorMsg"], header, false);
                lvWastage.ItemsSource = _wastage;
                CollectionViewSource.GetDefaultView(lvWastage.ItemsSource).Refresh();
            }
            else if (CommonFunctions.ParseDateToFinclave(startDate_Search.Text).Date > CommonFunctions.ParseDateToFinclave(endDate_search.Text).Date)
            {
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["date_ErrorMsgForGreater"], header, false);
               form.ShowDialog();
              //  Common.ErrorNotification((string)Application.Current.Resources["date_ErrorMsgForGreater"], header, false);
            }
            else
            {
                lvWastage.ItemsSource = _wastage.Where(x => CommonFunctions.ParseDateToFinclave(x.Date).Date >= CommonFunctions.ParseDateToFinclave(startDate_Search.Text).Date && CommonFunctions.ParseDateToFinclave(x.Date).Date <= CommonFunctions.ParseDateToFinclave(endDate_search.Text).Date);
                //  lvWastage.ItemsSource = _wastage;
                //}
                //else
                //{
                //    lvUsers.ItemsSource = _companies.Where(x => x.Name.ToLower().Contains(text)).ToList();

                //}
                CollectionViewSource.GetDefaultView(lvWastage.ItemsSource).Refresh();


                //GridLengthConverter gridLengthConverter = new GridLengthConverter();
                //// SerachColumn.Width = (GridLength)gridLengthConverter.ConvertFrom((CommonFunction.Common._containerWidth - 210));  //(90 for Header + 20 for Footer = 110)         
                ////lvProducts.MinWidth = (CommonFunction.Common._containerWidth - 210);
                ////lvProducts.MaxWidth = (CommonFunction.Common._containerWidth - 210);
                //SerachColumn.Width = (GridLength)gridLengthConverter.ConvertFrom((CommonFunction.Common._containerWidth - 210));
                //lvWastage.MinWidth = (CommonFunction.Common._containerWidth - 210);
                //lvWastage.MaxWidth = (CommonFunction.Common._containerWidth - 210);
                //lvWastage.Height = CommonFunction.Common._containerHeight - 275;
                //ActionPanelBorder.Width = (CommonFunction.Common._containerWidth - 210);
                //ResponceVm responce = controller.GetWastageWithDateFilter(Convert.ToDateTime(startDate_Search.Text), Convert.ToDateTime(endDate_search.Text));
                //if (responce.FaultData == null)
                //{
                //    _wastage = responce.Responce.Cast<WastageModel>().ToList();
                //      lvWastage.ItemsSource = _wastage;
                //        //if (lvWastage.Items.Count == 0)
                //        //    lvWastage.Items.Add("no record found");
                //        btn_addWastage.IsEnabled = true;
                //        edit_Wastage.IsEnabled = false;
                //        edit_Wastage.Background = Brushes.Gray;
                //        btn_Delete.IsEnabled = false;
                //        btn_Delete.Background = Brushes.Gray;
                //    }

                //else
                //{
                //    ConfirmationPopup form = new ConfirmationPopup(responce.FaultData.Detail.ErrorDetails, header, false);
                //    form.ShowDialog();
                //}

            }
        }

        private void add_Wastage(object sender, RoutedEventArgs e)
        {
            AddWastage _addWastage = new AddWastage();
            NavigationService.Navigate(_addWastage);

        }

        private void edit_Wastage_Click(object sender, RoutedEventArgs e)
        {
            var row = lvWastage.SelectedItem;
            EditWastage _editWastage = new EditWastage(row);
            NavigationService.Navigate(_editWastage);
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            dynamic row = lvWastage.SelectedItem;
           // msg = "Are you sure ? Do you want to delete Wastage ?";
            ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["wastage_DeleteConformationMsg"], header, true);
            form.ShowDialog();

            if (Common._isChecked)
            {
                var result = controller.DeleteWastage(row.WastageId);
                //if (result == true)
                //{
                ResponseVm responce = controller.GetWastage();//.ToList();
                if (responce.FaultData == null)
                {
                    _wastage = responce.Response.Cast<DomainContracts.DataContracts.WastageModel>().ToList();
                    lvWastage.ItemsSource = _wastage;
                   // msg = "Wastage has been deleted Successfully.";
                  //  ConfirmationPopup form1 = new ConfirmationPopup(msg, header, false);
                  //  form1.ShowDialog();
                    Common.Notification((string)Application.Current.Resources["wastage_DeleteSuccessMsg"], header, false);
                    DisableButton();
                }
                else
                {
                    ConfirmationPopup form1 = new ConfirmationPopup(responce.FaultData.Detail.ErrorDetails, header, false);
                    form1.ShowDialog();
                }
            }

        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            lvWastage.SelectedItem = null;
            btn_addWastage.IsEnabled = true;
            //btn_delete.IsEnabled = false;
            edit_Wastage.IsEnabled = false;
            edit_Wastage.Background = Brushes.Gray;
            btn_Delete.IsEnabled = false;
            btn_Delete.Background = Brushes.Gray;
            btn_clear.IsEnabled = false;
            btn_clear.Background = Brushes.Gray;
        }
        private void DisableButton()
        {
            edit_Wastage.IsEnabled = false;
            btn_Delete.IsEnabled = false;
            btn_clear.IsEnabled = false;
            // edit_Company.Background = Brushes.ForestGreen;
            edit_Wastage.Background = Brushes.Gray;
            // btn_clear.Background = Brushes.Red;
            btn_Delete.Background = Brushes.Gray;
            btn_clear.Background = Brushes.Gray;
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error(e.ExceptionObject);
        }

        private void startDate_Search_KeyUp(object sender, KeyEventArgs e)
        {
            var dd = e.Source;
            //if (!e.ctrlKey && !e.metaKey && (e.keyCode == 32 || e.keyCode > 46))
            //    doFormat(e.target)
        }

        private void lvWastage_Loaded(object sender, RoutedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }

        private void lvWastage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            objValidation.UpdateColumnsWidth(sender as ListView);
        }
    }
}
