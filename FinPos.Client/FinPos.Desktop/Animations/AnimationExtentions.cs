using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FinPos.Client.Animations
{
    internal static class AnimationExtentions
    {
        internal static AnimationContext Animate(this FrameworkElement target)
        {
            var result = new AnimationContext();
            result.Targets.Add(target);
            return result;
        }

        internal static AnimationContext Scale(this AnimationContext target, params double[] args)
        {
            var values = args.ToList();

            if (args.Length % 2 != 0)
            {
                throw new InvalidOperationException("Params should come in a time-value pair");
            }

            foreach (var element in target.Targets)
            {
                var scaleX = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTarget(scaleX, element);
                Storyboard.SetTargetProperty(scaleX, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));

                var scaleY = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTarget(scaleY, element);
                Storyboard.SetTargetProperty(scaleY, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));

                for (int i = 0; i < values.Count; i += 2)
                {
                    scaleX.KeyFrames.Add(new SplineDoubleKeyFrame()
                    {
                        KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(values[i])),
                        Value = values[i + 1]
                    });

                    scaleY.KeyFrames.Add(new SplineDoubleKeyFrame()
                    {
                        KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(values[i])),
                        Value = values[i + 1]
                    });
                }

                target.Instance.Children.Add(scaleX);
                target.Instance.Children.Add(scaleY);
            }

            target.StartIndex = target.EndIndex;
            target.EndIndex += 2 * target.Targets.Count;

            return target;
        }

        internal static AnimationContext MoveX(this AnimationContext target, params double[] args)
        {
            return target.SingleProperty("(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)", args);
        }

        internal static AnimationContext Opacity(this AnimationContext target, params double[] args)
        {
            return target.SingleProperty("(UIElement.Opacity)", args);
        }

        internal static AnimationContext Splines(this AnimationContext target, Spline spline)
        {
            return target.Splines(1, spline.Point1.X, spline.Point1.Y, spline.Point2.X, spline.Point2.Y);
        }

        internal static AnimationContext Splines(this AnimationContext target, int index, Spline spline)
        {
            return target.Splines(index, spline.Point1.X, spline.Point1.Y, spline.Point2.X, spline.Point2.Y);
        }

        internal static AnimationContext Splines(this AnimationContext target, int index, double x1, double y1, double x2, double y2)
        {
            for (var num = target.StartIndex; num < target.EndIndex; num++)
            {
                ((target.Instance.Children[num] as DoubleAnimationUsingKeyFrames).KeyFrames[index] as SplineDoubleKeyFrame).KeySpline = new KeySpline()
                {
                    ControlPoint1 = new Point(x1, y1),
                    ControlPoint2 = new Point(x2, y2)
                };
            }
            return target;
        }

        internal static AnimationContext Origin(this AnimationContext target, double x1, double x2)
        {
            foreach (var element in target.Targets)
            {
                element.RenderTransformOrigin = new Point(x1, x2);
            }
            return target;
        }

        internal static AnimationContext Animate(this AnimationContext target, params FrameworkElement[] newTargets)
        {
            target.Targets.Clear();
            foreach (var newElement in newTargets)
            {
                target.Targets.Add(newElement);
            }
            return target;
        }

        internal static AnimationContext EnsureDefaultTransforms(this AnimationContext target)
        {
            foreach (var element in target.Targets)
            {
                TransformGroup group = element.RenderTransform as TransformGroup;
                group = new TransformGroup();
                group.Children.Add(new ScaleTransform());
                group.Children.Add(new SkewTransform());
                group.Children.Add(new RotateTransform());
                group.Children.Add(new TranslateTransform());

                element.RenderTransform = group;
            }
            return target;
        }

        internal static AnimationContext With(this AnimationContext target, params FrameworkElement[] newElements)
        {
            foreach (var elements in newElements)
            {
                target.Targets.Add(elements);
            }
            return target;
        }

        internal static AnimationContext Without(this AnimationContext target, params FrameworkElement[] newElements)
        {
            foreach (var elements in newElements)
            {
                target.Targets.Remove(elements);
            }
            return target;
        }

        private static AnimationContext SingleProperty(this AnimationContext target, string propertyPath, params double[] args)
        {
            var values = args.ToList();

            if (args.Length % 2 != 0)
            {
                throw new InvalidOperationException("Params should come in a time-value pair");
            }

            foreach (var element in target.Targets)
            {
                var moveX = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTarget(moveX, element);
                Storyboard.SetTargetProperty(moveX, new PropertyPath(propertyPath));

                for (int i = 0; i < values.Count; i += 2)
                {
                    moveX.KeyFrames.Add(new SplineDoubleKeyFrame()
                    {
                        KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(values[i])),
                        Value = values[i + 1]
                    });
                }

                target.Instance.Children.Add(moveX);
            }

            target.StartIndex = target.EndIndex;
            target.EndIndex += target.Targets.Count;

            return target;
        }

        internal class AnimationContext
        {
            public AnimationContext()
            {
                this.Instance = new Storyboard()
                {
                    FillBehavior = FillBehavior.HoldEnd
                };

                this.Targets = new List<FrameworkElement>(4);
            }

            public int StartIndex
            {
                get;
                set;
            }

            public int EndIndex
            {
                get;
                set;
            }

            internal ICollection<FrameworkElement> Targets
            {
                get;
                private set;
            }

            internal Storyboard Instance
            {
                get;
                set;
            }
        }
    }
}
