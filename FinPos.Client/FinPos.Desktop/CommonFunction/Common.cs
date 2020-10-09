using FinPos.Client.Views;
using OnBarcode.Barcode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using ToastNotifications;

namespace FinPos.Client.CommonFunction
{
    public static class Common
    {
        public static Window _container { get; set; }
        public static Window _mdiContainer { get; set; }
        public static double PercentMaxValue = 100;
        public static double _containerWidth { get; set; }

        public static double _containerHeight { get; set; }

        public static bool _isChecked { get; set; }
        public enum type
        {
            OpeningStock = 0,
            StockManagement = 1
        }
        public enum barcodeHeight
        {
            [Description("XSmall-3")]
            XSmall = 3,
            [Description("Small-5")]
            Small = 5,
            [Description("Medium-7")]
            Medium = 7,
            [Description("Large-8")]
            Large = 8,
            [Description("XLarge-9")]
            XLarge = 9
        }

        public enum sheetSizes
        {
            [Description("A1-594 x 841")]
            A1 = 1,
            [Description("A2-420 x 594")]
            A2 = 2,
            [Description("A3-297 x 420")]
            A3 = 3,
            [Description("A4-210 x 297")]
            A4 = 4,
            [Description("A5-148 x 210")]
            A5 = 5
        }
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
        public static DependencyObject GetAncestorByType(DependencyObject element, Type type)
        {
            if (element == null) return null;

            if (element.GetType() == type) return element;

            return GetAncestorByType(System.Windows.Media.VisualTreeHelper.GetParent(element), type);

        }
        public static void ShowConfirmationPopup(string message, string callingPage, bool isBtnShow)
        {
            ConfirmationPopup confirmform = new ConfirmationPopup(message, callingPage, isBtnShow);
            confirmform.ShowDialog();
        }
        public static void ShowNotification(string title, string msg, bool istrue)
        {
            int duration;
            //  int.TryParse(duration, out duration);
            //if (duration <= 0)
            //{
            //    duration = -1;
            //}

            var animationMethod = FormAnimator.AnimationMethod.Slide;
            foreach (FormAnimator.AnimationMethod method in Enum.GetValues(typeof(FormAnimator.AnimationMethod)))
            {
                //if (string.Equals(method.ToString(), comboBoxAnimation.SelectedItem))
                //{
                //    animationMethod = method;
                //    break;
                //}
            }

            var animationDirection = FormAnimator.AnimationDirection.Up;
            foreach (FormAnimator.AnimationDirection direction in Enum.GetValues(typeof(FormAnimator.AnimationDirection)))
            {
                //if (string.Equals(direction.ToString(), comboBoxAnimationDirection.SelectedItem))
                //{
                //    animationDirection = direction;
                //    break;
                //}
            }

            var toastNotification = new Notification("Title", "Hello", 2, animationMethod, animationDirection);
            toastNotification.Show();
        }
        public static void Notification(string msg, string title, bool isTrue)
        {
            int duration = 4;
            //  int.TryParse(duration, out duration);
            //if (duration <= 0)
            //{
            //    duration = -1;
            //}

            var animationMethod = FormAnimator.AnimationMethod.Slide;
            //foreach (FormAnimator.AnimationMethod method in Enum.GetValues(typeof(FormAnimator.AnimationMethod)))
            //{
            //    //if (string.Equals(method.ToString(), comboBoxAnimation.SelectedItem))
            //    //{
            //    //    animationMethod = method;
            //    //    break;
            //    //}
            //}

            var animationDirection = FormAnimator.AnimationDirection.Up;
            //foreach (FormAnimator.AnimationDirection direction in Enum.GetValues(typeof(FormAnimator.AnimationDirection)))
            //{
            //    //if (string.Equals(direction.ToString(), comboBoxAnimationDirection.SelectedItem))
            //    //{
            //    //    animationDirection = direction;
            //    //    break;
            //    //}
            //}

            var toastNotification = new Notification(title, msg, duration, animationMethod, animationDirection);
            toastNotification.Show();


        }
        public static bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
        {
            Double result;
            return Double.TryParse(val, NumberStyle,
                System.Globalization.CultureInfo.CurrentCulture, out result);
        }
        public static decimal RoundOff(decimal value)
        {
            return Math.Round(value, 2);
        }
        private static byte[] GenerateBacode(string _data)
        {
            Linear barcode = new Linear();
            barcode.Type = BarcodeType.CODE128;
            barcode.Data = _data;
            return barcode.drawBarcodeAsBytes();
        }

        public static void ErrorMessage(string errorMessage, string callingPage)
        {
            ConfirmationPopup form = new ConfirmationPopup(errorMessage, callingPage, false);
            form.ShowDialog();
        }
        public static void ErrorNotification(string msg, string title, bool isTrue)
        {
            int duration = 4;
            //  int.TryParse(duration, out duration);
            //if (duration <= 0)
            //{
            //    duration = -1;
            //}

            var animationMethod = FormAnimator.AnimationMethod.Slide;
            //foreach (FormAnimator.AnimationMethod method in Enum.GetValues(typeof(FormAnimator.AnimationMethod)))
            //{
            //    //if (string.Equals(method.ToString(), comboBoxAnimation.SelectedItem))
            //    //{
            //    //    animationMethod = method;
            //    //    break;
            //    //}
            //}

            var animationDirection = FormAnimator.AnimationDirection.Up;
            //foreach (FormAnimator.AnimationDirection direction in Enum.GetValues(typeof(FormAnimator.AnimationDirection)))
            //{
            //    //if (string.Equals(direction.ToString(), comboBoxAnimationDirection.SelectedItem))
            //    //{
            //    //    animationDirection = direction;
            //    //    break;
            //    //}
            //}

            var toastNotification = new Notification(title, msg, duration, animationMethod, animationDirection);
            toastNotification.Show();
        }
    }

}
