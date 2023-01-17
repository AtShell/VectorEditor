using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double rotateAngle;
        Point p;
        Line newLine = new Line();
        short defultColor = 0;
        Line editLine = null;
        int mark = 0;
        Ellipse taker = null;
        bool StopPuncture = false;
        bool IsEditMod = false;
        bool IsLineMod = true;
        Dictionary<Line, Brush> TakenLines = new Dictionary<Line, Brush>();
        //private string selectedMode = "Line";
        enum colors
        {
            black,
            gray,
            red,
            blue,
            yellow,
            green
        }
        public MainWindow()
        {
            InitializeComponent();
        }
        private void DrawSpace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (StopPuncture)
            {
                StopPuncture = false;
            }
            else
            {
                if (IsEditMod)
                {

                    DrawSpace.Children.Remove(taker);
                    IsEditMod = false;
                    IsLineMod = true;
                    Cursor = Cursors.Arrow;

                }
                else if (IsLineMod)
                {

                    TakenLinesClear();

                    if (newLine.X1 == 0)
                    {
                        newLine.X1 = p.X;
                        newLine.Y1 = p.Y;
                        Cursor = Cursors.Pen;
                    }
                    else
                    {
                        newLine.X2 = p.X;
                        newLine.Y2 = p.Y;
                        DrawLine(newLine);
                        newLine.MouseLeftButtonUp += new MouseButtonEventHandler(newLine_MouseLeftButtonUp);
                        newLine = new Line();
                        Cursor = Cursors.Arrow;

                    }

                }
            }
        }
        private void newLine_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (StopPuncture)
            {
                StopPuncture = false;
            }
            {
                if (IsLineMod)
                {
                    StopPuncture = true;
                    Line lineT = (Line)sender;
                    p = Mouse.GetPosition(DrawSpace);
                    if (Math.Abs(p.X - lineT.X1) < 10 && Math.Abs(p.Y - lineT.Y1) < 10)
                    {
                        IsLineMod = false;
                        IsEditMod = true;
                        taker = CreatElips(p);
                        editLine = lineT;
                        mark = 1;
                        Cursor = Cursors.Hand;
                    }
                    else if (Math.Abs(p.X - lineT.X2) < 10 && Math.Abs(p.Y - lineT.Y2) < 10)
                    {
                        IsLineMod = false;
                        IsEditMod = true;
                        taker = CreatElips(p);
                        editLine = lineT;
                        mark = 2;
                        Cursor = Cursors.Hand;
                    }
                    else
                    {
                        if (Keyboard.IsKeyDown(Key.LeftCtrl))
                        {
                            try
                            {
                                TakenLines.Add(lineT, lineT.Stroke);
                                lineT.Stroke = Brushes.Aqua;
                            }
                            catch (ArgumentException)
                            {
                                lineT.Stroke = TakenLines[lineT];
                                TakenLines.Remove(lineT);
                            }

                        }
                        else
                        {
                            TakenLinesClear();
                            TakenLines.Add(lineT, lineT.Stroke);
                            lineT.Stroke = Brushes.Aqua;
                        }
                    }
                }
            }


        }
        #region colors
        private void Black_Click(object sender, RoutedEventArgs e)
        {
            defultColor = (short)colors.black;
        }

        private void Grey_Click(object sender, RoutedEventArgs e)
        {
            defultColor = (short)colors.gray;
        }

        private void Red_Click(object sender, RoutedEventArgs e)
        {
            defultColor = (short)colors.red;
        }

        private void Yellow_Click(object sender, RoutedEventArgs e)
        {
            defultColor = (short)colors.yellow;
        }

        private void Blue_Click(object sender, RoutedEventArgs e)
        {
            defultColor = (short)colors.blue;
        }

        private void Green_Click(object sender, RoutedEventArgs e)
        {
            defultColor = (short)colors.green;
        }
        #endregion
        private void DrawSpace_MouseMove(object sender, MouseEventArgs e)
        {
            Point oldp = p;
            p = Mouse.GetPosition(DrawSpace);

            if (IsEditMod)
            {
                if (mark == 1)
                {
                    DrawSpace.Children.Remove(taker);
                    editLine.X1 = p.X;
                    editLine.Y1 = p.Y;
                    taker = CreatElips(p);
                }
                if (mark == 2)
                {
                    DrawSpace.Children.Remove(taker);
                    editLine.X2 = p.X;
                    editLine.Y2 = p.Y;
                    taker = CreatElips(p);
                }
            }

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                StopPuncture = true;

                foreach (Line line in TakenLines.Keys)
                {
                    line.X1 += p.X - oldp.X;
                    line.X2 += p.X - oldp.X;
                    line.Y1 += p.Y - oldp.Y;
                    line.Y2 += p.Y - oldp.Y;
                }
            }
        }
        private void DrawLine(Line newLine)
        {
            InitializeLine(newLine);
            DrawSpace.Children.Add(newLine);
        }
        private void InitializeLine(Line newLine)
        {
            switch (defultColor)
            {
                case 0:
                    newLine.Stroke = Brushes.Black;
                    break;
                case 1:
                    newLine.Stroke = Brushes.Gray;
                    break;
                case 2:
                    newLine.Stroke = Brushes.Red;
                    break;
                case 3:
                    newLine.Stroke = Brushes.Blue;
                    break;
                case 4:
                    newLine.Stroke = Brushes.Yellow;
                    break;
                case 5:
                    newLine.Stroke = Brushes.Green;
                    break;
            }


            newLine.StrokeThickness = FigureThickness.Value;
        }
        private Ellipse CreatElips(Point p)
        {
            Ellipse el = new Ellipse();
            el.Width = 10;
            el.Height = 10;
            el.Stroke = Brushes.Red;
            el.StrokeThickness = 2;
            Canvas.SetTop(el, p.Y - 5);
            Canvas.SetLeft(el, p.X - 5);
            DrawSpace.Children.Add(el);

            return el;
        }
        private void TakenLinesClear()
        {
            foreach (Line line in TakenLines.Keys)
            {
                line.Stroke = TakenLines[line];
            }
            TakenLines.Clear();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && IsEditMod)
            {
                IsEditMod = false;
                IsLineMod = true;
                DrawSpace.Children.Remove(editLine);
                DrawSpace.Children.Remove(taker);
                Cursor = Cursors.Arrow;
            }
            else if (e.Key == Key.Delete)
            {
                Dictionary<Line, Brush> temp = new Dictionary<Line, Brush>(TakenLines);
                foreach (Line line in TakenLines.Keys)
                {
                    DrawSpace.Children.Remove(line);
                    temp.Remove(line);
                }
                //TakenLines = temp;
            }

        }

        private void Rotate(object sender, RoutedEventArgs e)
        {
            Dictionary<Line, Brush> temp = new Dictionary<Line, Brush>(TakenLines);
            foreach (Line line in TakenLines.Keys)
            {
                var children = DrawSpace.Children[DrawSpace.Children.IndexOf(line)];
                var rotateTransform1 = children.RenderTransform as RotateTransform;
                var transform = new RotateTransform(45 + (rotateTransform1?.Angle ?? 0));
                transform.CenterX = (line.X1 + line.X2) / 2;
                transform.CenterY = (line.Y1 + line.Y2) / 2;
                children.RenderTransform = transform;
                temp.Remove(line);
                
            }
        }
    }
}
