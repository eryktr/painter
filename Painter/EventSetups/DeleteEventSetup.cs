using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Painter.EventSetups
{
    class DeleteEventSetup : IEventSetup
    {
        public Canvas Canvas { get; set; }
        private DeleteEventSetup() { }

        public DeleteEventSetup(Canvas c)
        {
            MainWindow.ModeChanged += OnModeChanged;
            Canvas = c;
        }
        public void SetupEvents()
        {
            foreach (Shape sh in Canvas.Children)
            {
                sh.MouseLeftButtonDown += Delete;
            }
        }

        public void OnModeChanged(object sender, RoutedEventArgs e)
        {
            foreach (Shape sh in Canvas.Children)
            {
                sh.MouseLeftButtonDown -= Delete;
            }
        }

        public void Delete(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.CurrentMode != Mode.Delete) return;
            var sh = (Shape) sender;
            Canvas.Children.Remove(sh);
        }
    }
}
