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

namespace Painter
{
    class Utility
    {
        public static Rectangle FindPolygonBoundary(Polygon p)
        {
            var xMax = double.MinValue;
            var yMax = double.MinValue;
            var xMin = double.MaxValue;
            var yMin = double.MaxValue;
            foreach (var pt in p.Points)
            {
                var x = pt.X;
                var y = pt.Y;
                xMax = Math.Max(xMax, x);
                yMax = Math.Max(yMax, y);
                xMin = Math.Min(xMin, x);
                yMin = Math.Min(yMin, y);
            }

            var width = xMax - xMin;
            var height = yMax - yMin;
            var left = xMin;
            var top = yMin;

            var r = new Rectangle
            {
                Width = width,
                Height = height,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeDashArray = new DoubleCollection(new double[] {4, 2})
            };

            Canvas.SetLeft(r, left);
            Canvas.SetTop(r, top);

            return r;
        }

        public static Rectangle[] CreateCornerRectangles(Rectangle r)
        {
            const int d = 5;
            var rectangles = new Rectangle[4];
            for (var i = 0; i < 4; i++)
            {
                var e = new Rectangle
                {
                    Width = d,
                    Height = d,
                    Fill = new SolidColorBrush(Colors.Red)
                };
                rectangles[i] = e;
            }
            Canvas.SetLeft(rectangles[0], Canvas.GetLeft(r));
            Canvas.SetLeft(rectangles[1], Canvas.GetLeft(r) + r.Width - d);
            Canvas.SetLeft(rectangles[2], Canvas.GetLeft(r));
            Canvas.SetLeft(rectangles[3], Canvas.GetLeft(r) + r.Width - d);

            Canvas.SetTop(rectangles[0], Canvas.GetTop(r));
            Canvas.SetTop(rectangles[1], Canvas.GetTop(r));
            Canvas.SetTop(rectangles[2], Canvas.GetTop(r) + r.Height - d);
            Canvas.SetTop(rectangles[3], Canvas.GetTop(r) + r.Height - d);

            return rectangles;
        }

        public static void RemoveSelection(Canvas c, Rectangle r, Rectangle[] handles)
        {
            c.Children.Remove(r);
            if (handles == null) return;
            foreach (var handle in handles)
            {
                c.Children.Remove(handle);
            }
        }

        public static void Translate(double dx, double dy, params Shape[] shapes)
        {
            foreach (var sh in shapes)
            {
                Canvas.SetLeft(sh, Canvas.GetLeft(sh) + dx);
                Canvas.SetTop(sh, Canvas.GetTop(sh) + dy);
            }
        }
    }
}
