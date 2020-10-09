﻿using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonEnums;
using FinPos.Utility.CommonMethods;
using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Page
    {
        ProductController controller = new ProductController();
        CategoryController categoryController = new CategoryController();
        private string msg = string.Empty;
        private string header = "Product";
        private int _noOfErrorsOnScreen = 0;
        private byte[] binaryImage;
        private List<CategoryModel> _categories;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private int? categoryId = null;

        private Bitmap DisplayImage;
        private Bitmap CroppedImage;
        private Graphics DisplayGraphics;
        private System.Drawing.Point StartPoint, EndPoint;
        public AddProduct()
        {
            InitializeComponent();
            item_Type.ItemsSource = Enum.GetValues(typeof(CommonEnum.ItemTypes)).Cast<CommonEnum.ItemTypes>();
            item_Type.SelectedItem = (CommonEnum.ItemTypes)1;
            _categories = categoryController.GetCategories().Where(x => x.IsActive == true).ToList();
            ClearFields();

        }
        public AddProduct(string croppedFile)
        {
            var converter = new ImageSourceConverter();
            //CompanyDefaultLogo.Source = (ImageSource)converter.ConvertFromString(croppedFile);
            CompanyLogo.Visibility = Visibility.Visible;
            //CompanyDefaultLogo.Visibility = Visibility.Hidden;
            //CompanyDefaultLogo.Source = CompanyLogo.Source;
            //CompanyLogo.Visibility = Visibility.Visible;     
        }
        private void btn_save_click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(product_name.Text) || string.IsNullOrEmpty(retail_price.Text) || string.IsNullOrEmpty(trade_price.Text) || string.IsNullOrEmpty(itemType.Text) || item_Type.SelectedIndex == -1)
            {
              //  msg = "Please fill the required fields first";
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["error_message_Tax"], header, false);
                form.ShowDialog();
            }
            else if (is_texinclusive.IsChecked.Value == false && string.IsNullOrEmpty(taxPercentage_.Text))
            {
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["taxPercentage_ErrorMsg"], header, false);
                form.ShowDialog();
            }
            else
            {
                decimal? nullval = null;
                int? integernull = null;
                if (!string.IsNullOrEmpty(category_code.Text))
                {
                    categoryId = _categories.FirstOrDefault(x => x.CategoryName.ToLower() == category_code.Text.ToLower()).Id;
                }
                //binaryImage = Utility.CommonMethods.CommonFunctions.ImageToByteArray((BitmapImage)CompanyLogo.Source);
                //string data = Convert.ToBase64String(binaryImage);
                ProductModel model = new ProductModel(0, product_name.Text, categoryId, string.IsNullOrEmpty(retail_price.Text) ? nullval : Convert.ToDecimal(retail_price.Text), string.IsNullOrEmpty(trade_price.Text) ? nullval : Convert.ToDecimal(trade_price.Text), string.IsNullOrEmpty(wholeseller_price.Text) ? nullval : Convert.ToDecimal(wholeseller_price.Text), string.IsNullOrEmpty(reseller_price.Text) ? nullval : Convert.ToDecimal(reseller_price.Text), Convert.ToInt32(item_Type.SelectedItem), string.IsNullOrEmpty(weight_.Text) ? nullval : Convert.ToDecimal(weight_.Text), barcode_.Text, string.IsNullOrEmpty(taxPercentage_.Text) ? nullval : Convert.ToDecimal(taxPercentage_.Text), string.IsNullOrEmpty(minimum_level.Text) ? integernull : Convert.ToInt32(minimum_level.Text), string.IsNullOrEmpty(reorder_level.Text) ? integernull : Convert.ToInt32(reorder_level.Text), null, is_texinclusive.IsChecked.Value, shortname_.Text, description_.Text, UserModelVm.BranchId, string.Empty, string.Empty, null, CompanyLogo.Source.ToString(),UserModelVm.CompanyId,null);
              int id=  controller.SaveUpdateProduct(model);
               // msg = "Product saved successfully";
               // ConfirmationPopup form = new ConfirmationPopup(msg, header, false);
               // form.ShowDialog();
                Common.Notification((string)Application.Current.Resources["product_saveSuccessMsg"], header, false);
                ClearFields();
                inventory form1 = new inventory();
                NavigationService.Navigate(form1);
            }
        }
        private void ClearFields()
        {
            product_name.Text = "";
            category_code.Text = "";
            retail_price.Text = "";
            trade_price.Text = "";
            wholeseller_price.Text = "";
            reseller_price.Text = "";
            weight_.Text = "";
            barcode_.Text = "";
            taxPercentage_.Text = "";
            minimum_level.Text = "";
            reorder_level.Text = "";
            //itemImage.Text = "";
            shortname_.Text = "";
            description_.Text = "";
        }
        private void txtEmail_Error(object sender, System.Windows.Controls.ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;

        }
        private void AddProduct_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
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

        private void btn_back_click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            inventory form = new inventory();
            NavigationService.Navigate(form);
        }
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            ReusableCodeTemp("TextBox_KeyUp");
            //bool found = false;
            //var data = _categories;
            //string query = category_code.Text;

            //if (query.Length == 0)
            //{
            //    lbAutoCat.Items.Clear();
            //    lbAutoCat.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    lbAutoCat.IsEnabled = true;
            //    lbAutoCat.Visibility = Visibility.Visible;
            //}

            //lbAutoCat.Items.Clear();

            //foreach (var obj in data)
            //{
            //    if (Convert.ToString(obj.CategoryName).ToLower().StartsWith(query.ToLower()))
            //    {
            //        addProductName(obj.CategoryName);
            //        found = true;
            //    }
            //}

            //if (!found)
            //{
            //    lbAutoCat.Items.Add(new TextBlock() { Text = (string)Application.Current.Resources["no_result_alert"] });
            //    lbAutoCat.IsEnabled = false;
            //}
        }
        private void addProductName(string text)
        {
            lbAutoCat.Items.Add(text);
        }

        private void btn_removelogo_Click(object sender, RoutedEventArgs e)
        {
            CompanyDemoLogo.Visibility = Visibility.Visible;
            CompanyLogo.Source = CompanyDemoLogo.Source;
            CompanyLogo.Visibility = Visibility.Hidden;
        }

        private void btn_uploadlogo_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {

                string fileName = Path.GetFileName(dlg.FileName);
                string path = @"C:\POSDocuments";
                // Create directory temp if it doesn't exist
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string path2 = Path.Combine(path, fileName);
                if (!File.Exists(path2))
                {
                    File.Copy(dlg.FileName, path2);
                }
                CompanyLogo.Source = new BitmapImage(new Uri(path2));
                //CompanyLogo.Source = b;
                CompanyDemoLogo.Visibility = Visibility.Hidden;
                CompanyLogo.Visibility = Visibility.Visible;
            }
        }

        //private void btn_close_Click(object sender, RoutedEventArgs e)
        //{
        //    imageCropPopUp.IsOpen = false;
        //}
        //private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    var mousePosition = e.GetPosition(sender as UIElement);
        //    Canvas.SetLeft(selectionRectangle, mousePosition.X);
        //    Canvas.SetTop(selectionRectangle, mousePosition.Y);
        //    selectionRectangle.Visibility = System.Windows.Visibility.Visible;

        //}

        //private void Canvas_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        var mousePosition = e.GetPosition(sender as UIElement);
        //        selectionRectangle.Width = 200;
        //        selectionRectangle.Height = 200;
        //    }
        //}
        private void category_code_LostFocus(object sender, RoutedEventArgs e)
        {
            bool isExsist = categoryController.GetCategories().Any(x => x.CategoryName.ToLower() == category_code.Text.ToLower());
            if (!isExsist)
            {
                category_code.Text = "";
            }
            lbAutoCat.Visibility = Visibility.Collapsed;
        }


        private void lbAutoCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbAutoCat.Visibility = Visibility.Collapsed;
            if (lbAutoCat.SelectedIndex != -1)
            {
                category_code.Text = lbAutoCat.SelectedItem.ToString();
            }
        }

        private void reorder_level_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void is_texinclusive_Checked_1(object sender, RoutedEventArgs e)
        {
            //  Handle(sender as CheckBox);

        }

        private void is_texinclusive_Unchecked(object sender, RoutedEventArgs e)
        {
            Handle(sender as CheckBox);

        }
        void Handle(CheckBox checkBox)
        {
            bool chkd = checkBox.IsChecked.Value;

            if (chkd)
            {
                taxPercentage_.Visibility = Visibility.Hidden;
                taxPercentage.Visibility = Visibility.Hidden;
                taxPercentage_.Text = string.Empty;
            }
            else
            {
                taxPercentage_.Visibility = Visibility.Visible;
                taxPercentage.Visibility = Visibility.Visible;
            }
        }

        private void barcodeLogo_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void barcodeLogo_lostFocus(object sender, RoutedEventArgs e)
        {
            var response = controller.GetProducts().Response.Cast<ProductModel>().ToList().Any(x => x.BarCode.ToLower() == barcode_.Text.ToLower() && !string.IsNullOrEmpty(x.BarCode));
            if (response)
            {
                // var isExsist= responce.Any(x => x.BarCode.ToLower() == barcode_.Text.ToLower());
                //   if (isExsist)
                //  {
                ConfirmationPopup form = new ConfirmationPopup((string)Application.Current.Resources["barcode_exeption"], header, false);
                form.ShowDialog();
                // Common.ErrorNotification((string)Application.Current.Resources["barcode_exeption"], header, false);
                barcode_.Text = string.Empty;
                // }
            }
        }
        private void arrow_Click(object sender, RoutedEventArgs e)
        {
            if (lbAutoCat.Visibility == Visibility.Visible)
            {
                lbAutoCat.Visibility = Visibility.Collapsed;
            }
            else
            {
                ReusableCodeTemp("arrow_Click");
            }

        }
        /// <summary>
        /// this method is created for setting values in Product searchable textbox
        /// this method is called at two diffrent places one at arrow click which will populate all products if query serach will be empty
        /// </summary>
        /// <param name=""></param>
        private void ReusableCodeTemp(string callingMethod)
        {
            try
            {
                bool found = false;
                var data = _categories;
                string query = category_code.Text;
                lbAutoCat.Items.Clear();
                if (query.Length == 0)
                {
                    if (callingMethod == "arrow_Click")
                    {
                        foreach (var obj in data)
                        {
                            //if (Convert.ToString(obj.CategoryName).ToLower().StartsWith(query.ToLower()))
                            //{
                            addProductName(obj.CategoryName);
                            found = true;
                            //}
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

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            lbAutoCat.Items.Add(new TextBlock() { Text = (string)Application.Current.Resources["no_result_alert"] });
            lbAutoCat.IsEnabled = false;
            lbAutoCat.Visibility = Visibility.Collapsed;

        }
    }
}