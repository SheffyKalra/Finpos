using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FinPos.Server.ServerViews
{
    /// <summary>
    /// Interaction logic for ConfirmationPopup.xaml
    /// </summary>
    public partial class ConfirmationPopup : Window
    {
        public ConfirmationPopup()
        {
            InitializeComponent();
        }
        public ConfirmationPopup(string msg,string header)
        {
            InitializeComponent();
            txtConfirmation.Text = msg;
            lbHeader.Text = header;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
       
    }
}
