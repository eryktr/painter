using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace Painter.EventSetups
{
    class ChangeColorEventSetup : IEventSetup
    {
        public Canvas Canvas;
        private ChangeColorEventSetup() { }
        private ColorPicker _colorPicker;

        public ChangeColorEventSetup(Canvas c)
        {
            MainWindow.ModeChanged += OnModeChanged;
            Canvas = c;
        }

        public void SetupEvents()
        {
            _colorPicker = new ColorPicker
            {
                SelectedColor = MainWindow.CurrentColor,
                IsOpen = true,
            };
            Canvas.Children.Add(_colorPicker);
            Canvas.SetLeft(_colorPicker, 200);
            _colorPicker.SelectedColorChanged += (o, args) =>
            {
                if (_colorPicker.SelectedColor != null) MainWindow.CurrentColor = _colorPicker.SelectedColor.Value;
                
            };

            Canvas.MouseLeave += (o, e) =>
            {
                if (MainWindow.CurrentMode != Mode.ChangeColor) return;
                Canvas.Children.Remove(_colorPicker);
            };
        }

        public void OnModeChanged(object sender, RoutedEventArgs e)
        {
            Canvas.Children.Remove(_colorPicker);
        }
    }
}
