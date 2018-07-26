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
    class CircleEventSetup : IEventSetup
    {
        public Canvas Canvas { get; set; }
        private Ellipse _lastCircle;
        private double _initX, _initY;
        private bool _drawCircle;

        private CircleEventSetup() {}

        public CircleEventSetup(Canvas c)
        {
            Canvas = c;
        }

        public void SetupEvents()
        {
            Canvas.MouseLeftButtonDown += Draw;
        }

        public void Draw(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(Canvas);
            _initX = pos.X;
            _initY = pos.Y;
            Canvas.MouseMove += DrawCircle;
        }

        public void DrawCircle(object sender, MouseEventArgs args)
        {
            if (MainWindow.CurrentMode == Mode.Circle)
            {
                if (args.LeftButton == MouseButtonState.Pressed)
                {
                    Canvas.Children.Remove(_lastCircle);
                    var newPos = args.GetPosition(Canvas);
                    double newX = newPos.X;
                    double newY = newPos.Y;
                    double radius = Math.Sqrt((newX - _initX) * (newX - _initX) + (newY - _initY) * (newY - _initY));
                    Ellipse c = new Ellipse
                    {
                        Height = radius,
                        Width = radius,
                        Fill = new SolidColorBrush(MainWindow.CurrentColor),
                        Stroke = new SolidColorBrush(MainWindow.CurrentColor)
                    };
                    var top = newY > _initY ? _initY : newY;
                    var left = newX > _initX ? _initX : newX;
                    Canvas.SetTop(c, top);
                    Canvas.SetLeft(c, left);
                    Canvas.Children.Add(c);
                    _lastCircle = c;
                }
                else
                {
                    _lastCircle = null;
                    Canvas.MouseMove -= DrawCircle;
                }
            }
        }

    }
}
