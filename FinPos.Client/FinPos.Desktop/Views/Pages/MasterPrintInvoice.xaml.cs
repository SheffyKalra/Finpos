using FinPos.Client.CommonFunction;
using FinPos.Client.Controllers;
using FinPos.DomainContracts.DataContracts;
using OnBarcode.Barcode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZXing;

namespace FinPos.Client.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MasterPrintInvoice.xaml
    /// </summary>
    public partial class MasterPrintInvoice : Page
    {
        ProductController productController = new ProductController();
        private IList<ProductModel> _products;
        public MasterPrintInvoice(MasterLabelSettingModel row, List<ProductLabelSettingModel> productlabel)
        {
            InitializeComponent();
            List<PrintMasterSettingModel> models = new List<PrintMasterSettingModel>();
            ResponseVm responce = productController.GetProductsByCompanyAndBranch();
            _products = responce.Response.Cast<ProductModel>().ToList();

            //for (var i = 0; i <= Convert.ToInt32(row.StartRow); i++)
            //{

            //if (i == Convert.ToInt32(row.StartRow))
            //{
            //for (var k = 0; k <= Convert.ToInt32(row.StartColumn); k++)
            //{
            //if (k == Convert.ToInt32(row.StartColumn))
            //{
            foreach (var data in productlabel)
            {
                var product = _products?.FirstOrDefault(x => x.Id == data.ProductCode);
                for (var j = 1; j <= data.Quantity; j++)
                {
                    PrintMasterSettingModel model = new PrintMasterSettingModel();
                    if (row.PrintBarCode)
                    {
                        model.Barcode = product.BarCode;
                    }
                    else
                        model.Barcode = string.Empty;
                    if (row.PrintItemCode)
                        model.ProductCode = Convert.ToString(product.Id);
                    else
                        model.ProductCode = string.Empty;
                    if (row.PrintItemDetail)
                        model.ProductName = product.ItemName;
                    else
                        model.ProductName = string.Empty;
                    if (row.PrintItemPrice)
                        model.Price = Convert.ToString(product.RetailPrice);
                    else
                    {
                        model.Price = string.Empty;
                    }
                    var content = model.Barcode;
                    var writer = new BarcodeWriter
                    {
                        Format = BarcodeFormat.CODE_128
                    };
                    var bitmap = writer.Write(content);
                    bitmap.SetResolution(100, 150);
                    //Linear barcode = new Linear();
                    //barcode.Type = BarcodeType.CODE128;
                    //barcode.Data = model.Barcode;
                    //barcode.drawBarcodeAsBytes();
                    //Utility.CommonMethods.CommonFunctions.ByteToImage(barcode.drawBarcodeAsBytes());
                    model.ImageData = Common.ToBitmapImage(bitmap);
                    model.Barcode = string.Empty;
                    models.Add(model);

                }


            }
            TvBox.ItemsSource = models;
        }

        //else
        //    models.Add(new PrintMasterSettingModel());

        //}
        //}
        // }
        //[System.Runtime.InteropServices.DllImport("gdi32.dll")]
        //public static extern bool DeleteObject(IntPtr hObject);

        //private BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        //{
        //    IntPtr hBitmap = bitmap.GetHbitmap();
        //    BitmapImage retval;

        //    try
        //    {
        //        retval = (BitmapImage)Imaging.CreateBitmapSourceFromHBitmap(
        //                     hBitmap,
        //                     IntPtr.Zero,
        //                     Int32Rect.Empty,
        //                     BitmapSizeOptions.FromEmptyOptions());
        //    }
        //    finally
        //    {
        //        DeleteObject(hBitmap);
        //    }

        //    return retval;
        //}
        //private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        //{
        //    // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

        //    using (MemoryStream outStream = new MemoryStream())
        //    {
        //        BitmapEncoder enc = new BmpBitmapEncoder();
        //        enc.Frames.Add(BitmapFrame.Create(bitmapImage));
        //        enc.Save(outStream);
        //        System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

        //        return new Bitmap(bitmap);
        //    }
        //}

        private BitmapImage LoadImage(string filename)
        {
            return new BitmapImage(new Uri("pack://application:,,,/" + filename));
        }

    }
}
