using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FinPos.Client.Animations
{
    public static class ExtentionMethods
    {
        public static void NavigateToPage(this Frame FinposPageContainer, Page page)
        {
            var navigation = new NavigationAnimator();
            navigation.Navigate(FinposPageContainer, page);
        }
    }
}
