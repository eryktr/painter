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
        private readonly IEnumerable<Shape> Shapes;

        private MoveEventSetup() { }

        public MoveEventSetup(Canvas c)
        {
            Canvas = c;
            Shapes =  (from Shape s in Canvas.Children select s);
        }

        //The layer of abstraction needs to be lowered and class-specific implementations used.
        public void SetupEvents()
        {
            foreach(var s in Shapes)
            {
                s.MouseLeftButtonDown += Mark;
            }
        }

        public void Mark(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.CurrentMode != Mode.Moving) return;
            var sh = (Shape)sender;
            var r = new Rectangle
            {
                Width =  sh.ActualWidth,
                Height = sh.ActualHeight,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeDashArray = new DoubleCollection(new double[] {4, 2})
            };
            Canvas.SetLeft(r,  Canvas.GetLeft(sh));
            Canvas.SetTop(r, Canvas.GetTop(sh));
            Canvas.Children.Add(r);
        } 
    }
}
