using System.Windows;

namespace FinPos.Client.Animations
{
    internal class Spline
    {
        public Spline(double x1, double y1, double x2, double y2)
        {
            this.Point1 = new Point(x1, y1);
            this.Point2 = new Point(x2, y2);
        }

        internal Point Point1
        {
            get;
            set;
        }

        internal Point Point2
        {
            get;
            set;
        }
    }
}
