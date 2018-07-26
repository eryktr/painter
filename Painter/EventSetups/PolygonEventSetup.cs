using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Painter.EventSetups
{
    class PolygonEventSetup : IEventSetup
    {
        public Canvas Canvas;
        private Polygon _lastPolygon;
        private readonly List<Point> _points;
        private readonly Polyline _polyline;
        private PolygonEventSetup() { }

        public PolygonEventSetup(Canvas c)
        {
            _points = new List<Point>();
            _polyline = new Polyline();
            _polyline.Stroke = new SolidColorBrush(MainWindow.CurrentColor);
            _polyline.StrokeThickness = 2;
            Canvas = c;
            Canvas.Children.Add(_polyline);
        }
        public void SetupEvents()
        {
            Canvas.MouseLeftButtonDown += Draw;
            Canvas.MouseRightButtonDown += Finalize;
        }

        public void Draw(object sender, MouseButtonEventArgs args)
        {
            if (MainWindow.CurrentMode != Mode.Polygon) return;
            _points.Add(args.GetPosition(Canvas));
            _polyline.Points = new PointCollection(_points);
            
        }

        public void Finalize(object sender, MouseButtonEventArgs args)
        {
            if (MainWindow.CurrentMode != Mode.Polygon) return;
            var p = new Polygon
            {
                Points = new PointCollection(_points),
                Fill = new SolidColorBrush(MainWindow.CurrentColor),
            };
            Canvas.Children.Add(p);
            _points.RemoveRange(0, _points.Count);






        }
    }
}
