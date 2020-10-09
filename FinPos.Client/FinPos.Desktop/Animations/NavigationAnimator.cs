using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media;

namespace FinPos.Client.Animations
{
    public class NavigationAnimator
    {
        private static NavigationService navigationService;
        private static FrameworkElement srcElement, targetElement;
        private static FinposTransition transition;

        public event EventHandler<TransitionEventArgs> TransitionAnimationCompleted = delegate { };

        public void Navigate(Frame frame, FrameworkElement nextElement)
        {
            navigationService = frame.NavigationService;
            if (navigationService == null) { return; }

            srcElement = navigationService.Content as FrameworkElement;
            targetElement = nextElement;

            if (srcElement != null)
            {
                navigationService.Navigating += NavigationAnimator_Navigating;
            }

            if (transition == null)
            {
                var mask1 = new Rectangle()
                {
                    Fill = new SolidColorBrush(Color.FromArgb(77, 8, 17, 48))
                };

                var mask2 = new Rectangle()
                {
                    Fill = new SolidColorBrush(Color.FromArgb(77, 8, 17, 48))
                };

                transition = new FinposTransition(mask1, mask2);
            }

            navigationService.Navigate(nextElement);
        }
        private void NavigationAnimator_Navigating(object sender, NavigatingCancelEventArgs e)
		{
			e.Cancel = true;

			var mockItem1 = new Rectangle()
			{
				Fill = new VisualBrush(srcElement)
			};
			navigationService.Navigating -= NavigationAnimator_Navigating;
			navigationService.Navigate(targetElement);
			targetElement.Loaded += delegate
			{
				if (transition.IsRunning)
				{
					transition.StopTransition();
				}
                var container = (sender as FrameworkElement).Parent as Grid;
                transition.Begin(mockItem1, container, targetElement);
            };
		}

    }
}
