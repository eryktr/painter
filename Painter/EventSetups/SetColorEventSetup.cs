using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Painter.EventSetups
{
    class SetColorEventSetup : IEventSetup
    {
        public Canvas Canvas { get; set; }
        private SetColorEventSetup() { }

        public SetColorEventSetup(Canvas c)
        {
            Canvas = c;
        }
        public void SetupEvents()
        {
            MessageBox.Show("I work");
            
        }
    }
}
