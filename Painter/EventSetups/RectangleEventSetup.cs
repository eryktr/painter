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
    class RectangleEventSetup : IEventSetup
    {
        private double _initX, _initY;
        private Rectangle lastRectangle = null;
        public Canvas Canvas { get; set; }
        

        private RectangleEventSetup() { }
        public RectangleEventSetup(Canvas c)
        {
            Canvas = c;
        }

        public void SetupEvents()
        {
            MainWindow.ModeChanged += OnModeChanged;
            Canvas.MouseLeftButtonDown += Draw;
        }

        private void Draw(object sender, MouseButtonEventArgs args)
        {
            _initX = args.GetPosition(Canvas).X;
            _initY = args.GetPosition(Canvas).Y;
            Canvas.MouseMove += DrawRectangle;
        }

        private void DrawRectangle(object sender, MouseEventArgs args)
        {
            if (MainWindow.CurrentMode == Mode.Rectangle)
            {
                if (args.LeftButton == MouseButtonState.Pressed)
                {
                    Canvas.Children.Remove(lastRectangle);
                    MainWindow.Rectangles.Remove(lastRectangle);
                    var newX = args.GetPosition(Canvas).X;
                    var newY = args.GetPosition(Canvas).Y;
                    var r = new Rectangle
                    {
                        Stroke = new SolidColorBrush(MainWindow.CurrentColor),
                        Fill = new SolidColorBrush(MainWindow.CurrentColor)
                    };
                    Canvas.Children.Add(r);
                    MainWindow.Rectangles.Add(r);
                    r.Width = (newX > _initX ? newX - _initX : _initX - newX);
                    r.Height = (newY > _initY ? newY - _initY : _initY - newY);
                    Canvas.SetLeft(r, Math.Min(_initX, newX));
                    Canvas.SetTop(r, Math.Min(newY, _initY));
                    lastRectangle = r;
                }
                else
                {
                    lastRectangle = null;
                    Canvas.MouseMove -= DrawRectangle;
                }
            }
        }

        public void OnModeChanged(object sender, RoutedEventArgs e)
        {
            Canvas.MouseLeftButtonDown -= Draw;
        }
    }
}
