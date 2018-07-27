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
    class MoveEventSetup : IEventSetup
    {
        public Canvas Canvas { get; set; }
        private Shape _currentlySelected;
        private Rectangle _currentRectangle;
        private Rectangle[] _currentHandles;
        private double _initX, _initY;

        public Shape CurrentlySelected
        {
            get => _currentlySelected;
            set
            {
                if (_currentlySelected != null) _currentlySelected.MouseLeftButtonDown -= Move;
                _currentlySelected = value;
                if (_currentlySelected != null) _currentlySelected.MouseLeftButtonDown += Move;
            }
        }

        private MoveEventSetup()
        {
        }

        public MoveEventSetup(Canvas c)
        {
            Canvas = c;
            MainWindow.ModeChanged += OnModeChanged;
        }

        public void SetupEvents()
        {
            SubscribeEvents();
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

                CurrentlySelected = sh;
                _currentRectangle = r;
                _currentHandles = rects;

                MouseButtonEventHandler Unmark = (o, args) =>
                {
                    Utility.RemoveSelection(Canvas, _currentRectangle, _currentHandles);
                    CurrentlySelected = null;
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

        public void OnModeChanged(object sender, RoutedEventArgs e)
        {
            Utility.RemoveSelection(Canvas, _currentRectangle, _currentHandles);
            UnsubscribeEvents();
            CurrentlySelected = null;
        }

        public void SubscribeEvents()
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

        public void UnsubscribeEvents()
        {
            foreach (var c in MainWindow.Circles)
            {
                c.MouseLeftButtonDown -= Mark;
            }

            foreach (var r in MainWindow.Rectangles)
            {
                r.MouseLeftButtonDown -= Mark;
            }

            foreach (var p in MainWindow.Polygons)
            {
                p.MouseLeftButtonDown -= MarkPolygon;
            }
        }

        public void Move(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.CurrentMode != Mode.Moving) return;
            var sh = (Shape) sender;
            var p = e.GetPosition(Canvas);
            _initX = p.X;
            _initY = p.Y;
            MouseEventHandler translate = (o, args) =>
            {
                if (args.LeftButton != MouseButtonState.Pressed) return;
                var p2 = args.GetPosition(Canvas);
                var newX = p2.X;
                var newY = p2.Y;
                var dx = newX - _initX;
                var dy = newY - _initY;
                Utility.Translate(dx, dy, _currentHandles[0], _currentHandles[1], _currentHandles[2],
                    _currentHandles[3], _currentRectangle, _currentlySelected);
                _initX = newX;
                _initY = newY;
            };
            sh.MouseMove += translate;

            sh.PreviewMouseLeftButtonUp += (o, args) =>
            {
                MessageBox.Show(":");
                sh.MouseMove -= translate;
            };
        }
    }
}