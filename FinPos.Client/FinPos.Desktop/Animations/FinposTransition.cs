using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FinPos.Client.Animations
{
    public class FinposTransition
    {
        private Storyboard animation = new Storyboard();
        private FrameworkElement item1;
        private FrameworkElement item2;
        private FrameworkElement mask1;
        private FrameworkElement mask2;
        private bool isRunning;

        public event EventHandler<TransitionEventArgs> TransitionAnimationCompleted = delegate { };

        public FinposTransition(FrameworkElement mask1, FrameworkElement mask2)
        {
            this.mask1 = mask1;
            this.mask2 = mask2;
        }

        public double PageWidth
        {
            get;
            set;
        }

        public Grid Container
        {
            get;
            set;
        }

        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        public void Begin(FrameworkElement mockItem1, Grid container, FrameworkElement newPage)
        {
            this.Container = container;
            this.PageWidth = container.ActualWidth;
            this.item1 = mockItem1;
            this.item2 = newPage;

            this.Container.Children.Add(this.item1);

            var width = PageWidth;

            var scaleFactor = 0.8;
            var halfSrink = (1.0 - scaleFactor) / 2;

            this.Container.Children.Add(this.mask1);
            this.Container.Children.Add(this.mask2);

            var scaleDownSpline = new Spline(0.504000008106232, 0.256000012159348, 0.458999991416931, 1);
            var moveSpline = new Spline(0.247999995946884, 0, 1, 1);
            var scaleUpSpline = new Spline(0.321000009775162, 0, 0.45100000500679, 1);
            var spacing = 15.0;

            this.animation = this.item1
                .Animate()
                .With(this.mask1)
                    .EnsureDefaultTransforms()
                    .Origin(0, 0.5)
                    .Scale(0, 1, 0.2, 1, 0.7, scaleFactor)
                        .Splines(2, scaleDownSpline)
                    .MoveX(0.7, 0, 1.365, (-width * (scaleFactor - halfSrink)) - spacing, 1.9, (-width * scaleFactor) - spacing)
                        .Splines(1, moveSpline)
                        .Splines(2, scaleUpSpline)
                .Without(this.item1)
                    .Opacity(0, 0, 0.2, 0, 0.7, 1)
                        .Splines(1, scaleDownSpline)
                .Animate(this.mask2)
                    .Opacity(1.35, 1, 1.9, 0)
                        .Splines(scaleUpSpline)
                .With(this.item2)
                    .EnsureDefaultTransforms()
                    .Origin(0.5, 0.5)
                    .Scale(0, scaleFactor, 1.365, scaleFactor, 1.9, 1)
                        .Splines(1, moveSpline)
                        .Splines(2, scaleUpSpline)
                    .MoveX(0.0, ((scaleFactor + halfSrink) * width) + spacing, 0.2, ((scaleFactor + halfSrink) * width) + spacing, 0.7, ((scaleFactor - halfSrink) * width) + spacing, 1.365, 0)
                        .Splines(2, scaleDownSpline)
                        .Splines(3, moveSpline)
                .Instance;

            this.animation.SpeedRatio = 755.0 / width * 1.5;

            this.isRunning = true;

            this.animation.Completed += (sender, e) =>
            {
                this.TransitionAnimationCompleted(this, new TransitionEventArgs(item2));

                Container.Children.Remove(mask1);
                Container.Children.Remove(mask2);
                RemoveVisualBrush();
                Container.Children.Remove(item1);
                item1 = null;
                item2 = null;
            };

            this.animation.Begin();
        }


        private void RemoveVisualBrush()
        {
            var shape = item1 as Shape;
            if (shape != null)
            {
                var visBrush = shape.Fill as VisualBrush;
                if (visBrush != null)
                {
                    visBrush.Visual = null;
                }
            }
        }


        public void StopTransition()
        {
            // Move to initial state:
            this.animation.Stop();

            this.Container.Children.Remove(this.item1);

            this.Container.Children.Remove(this.mask1);
            this.Container.Children.Remove(this.mask2);
            this.item1 = null;
            this.item2 = null;
            this.isRunning = false;
        }
    }

    public class TransitionEventArgs : EventArgs
    {
        public FrameworkElement TargetExamplePage
        {
            get;
            set;
        }

        public TransitionEventArgs(FrameworkElement targetExample)
        {
            this.TargetExamplePage = targetExample;
        }
    }
}
