using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Painter.EventSetups;
using Xceed.Wpf.Toolkit;

namespace Painter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Color _currentColor = Colors.Black;
        private static Label _colorLabel;

        public static Color CurrentColor
        {
            get => _currentColor;
            set 
            {
                _currentColor = value;
                OnColorChanged(_colorLabel);
            }
        } 
        public IEventSetup EventSetup { get; set; }
        private static Mode currentMode = Mode.None;
        public delegate void ModeChangedEventHandler(object sender, RoutedEventArgs args);
        public static event ModeChangedEventHandler ModeChanged;
        public static Mode CurrentMode
        {
            get => currentMode;
            set
            {
                currentMode = value;
                OnModeChanged();
            }
        }

        public static readonly List<Rectangle> Rectangles = new List<Rectangle>();
        public static readonly List<Ellipse> Circles = new List<Ellipse>();
        public static readonly List<Polygon> Polygons = new List<Polygon>();

        public MainWindow()
        {
            InitializeComponent();
            ColorLabel.Background = new SolidColorBrush(CurrentColor);
            MainCanvas.ClipToBounds = true;
            _colorLabel = ColorLabel;
        }



        private void CheckButton(object sender, RoutedEventArgs e)
        {
            UncheckButtons(this, e);
            var b = (Button) sender;
            b.BorderBrush = new SolidColorBrush(Colors.Blue);
            b.BorderThickness = new Thickness(2);

        }

        private void UncheckButtons(object sender, RoutedEventArgs e)
        {
            var buttons = from Button b in SideButtons.Children select b;
            foreach (var b in buttons)
            {
                b.BorderBrush = null;
                b.BorderThickness = new Thickness(0);
            }
        }

        private void ChangeColorButton_OnClick(object sender, RoutedEventArgs e)
        {
            currentMode = Mode.ChangeColor;
            CheckButton(sender, e);
            UpdateEventSetup();
        }

        private void RectangleButton_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentMode = Mode.Rectangle;
            CheckButton(sender, e);
            UpdateEventSetup();
        }
        
        private void CircleButton_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentMode = Mode.Circle;
            CheckButton(sender, e);
            UpdateEventSetup();
        }

        private void PolygonButton_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentMode = Mode.Polygon;
            CheckButton(sender, e);
            UpdateEventSetup();
        }

        private void MoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentMode = Mode.Moving;
            CheckButton(sender, e);
            UpdateEventSetup();
        }

        private void SetObjectColorButton_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentMode = Mode.SetColor;
            CheckButton(sender, e);
            UpdateEventSetup();

        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentMode = Mode.Delete;
            CheckButton(sender, e);
            UpdateEventSetup();
        }

        private void AssignEvents()
        {
            EventSetup.SetupEvents();
        }

        private void UpdateEventSetup()
        {
            switch (CurrentMode)
            {
                case Mode.Rectangle:
                    EventSetup = new RectangleEventSetup(MainCanvas);
                    break;

                case Mode.Circle:
                    EventSetup = new CircleEventSetup(MainCanvas);
                    break;

                case Mode.Polygon:
                    EventSetup = new PolygonEventSetup(MainCanvas);
                    break;

                case Mode.Moving:
                    EventSetup = new MoveEventSetup(MainCanvas);
                    break;

                case Mode.SetColor:
                    EventSetup = new SetColorEventSetup(MainCanvas);
                    break;

                case Mode.ChangeColor:
                    EventSetup = new ChangeColorEventSetup(MainCanvas);
                    break;

                case Mode.Delete:
                    EventSetup = new DeleteEventSetup(MainCanvas);
                    break;
            }
            AssignEvents();

         }

        public static void OnColorChanged(Label l)
        {
            l.Background = new SolidColorBrush(CurrentColor);
        }

        protected static void OnModeChanged()
        {
            ModeChanged?.Invoke(null, new RoutedEventArgs());
        }
    }
}
