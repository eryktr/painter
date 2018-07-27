using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Painter.EventSetups
{
    class MoveEventSetup : IEventSetup
    {
        public Canvas Canvas { get; set; }
        private Shape _currentlySelected;
        private Rectangle _currentRectangle;
        private Rectangle[] _currentHandles;

        private MoveEventSetup()
        {
        }

        public MoveEventSetup(Canvas c)
        {
            Canvas = c;
        }

        //The layer of abstraction needs to be lowered and class-specific implementations used.
        public void SetupEvents()
        {
            foreach (var c in MainWindow.Circles)
            {
                c.MouseLeftButtonDown += Mark;
            }

            foreach (var r in MainWindow.Rectangles)
            {
                r.MouseLeftButtonDown += Mark;
            }

            foreach (var p in MainWindow.Polygons)
            {
                p.MouseLeftButtonDown += MarkPolygon;
            }
        }

        public void Mark(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.CurrentMode != Mode.Moving) return;
            {
                Utility.RemoveSelection(Canvas, _currentRectangle, _currentHandles);

                var sh = (Shape) sender;
                var r = new Rectangle
                {
                    Width = sh.ActualWidth,
                    Height = sh.ActualHeight,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeDashArray = new DoubleCollection(new double[] {4, 2})
                };
                Canvas.SetLeft(r, Canvas.GetLeft(sh));
                Canvas.SetTop(r, Canvas.GetTop(sh));
                Canvas.Children.Add(r);

                var rects = Utility.CreateCornerRectangles(r);
                foreach (var rec in rects)
                {
                    Canvas.Children.Add(rec);
                }

                _currentlySelected = sh;
                _currentRectangle = r;
                _currentHandles = rects;

                MouseButtonEventHandler Unmark = (o, args) =>
                {
                    Utility.RemoveSelection(Canvas, _currentRectangle, _currentHandles);
                    _currentlySelected = null;
                };

                Canvas.MouseRightButtonDown += Unmark;
            }
        }

        public void MarkPolygon(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.CurrentMode != Mode.Moving) return;
            var pol = (Polygon) sender;
            var r = Utility.FindPolygonBoundary(pol);
            Canvas.Children.Add(r);
        }
    }
}