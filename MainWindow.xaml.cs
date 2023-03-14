using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        #region Seting
        MyTransform transform = new MyTransform();
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            transform = new MyTransform(FigureThickness.Value);
        }
        private void newLine_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (transform.defultMode)
            {
                case 0:
                    /*if (StopPuncture)
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
                    }*/
                    //line = null;
                    break;
                case 1:
                    //rect = null;
                    break;
            }


        }
        #region SwitchColor
        private void Black_Click(object sender, RoutedEventArgs e)
        {
            transform.color = (short)MyTransform.Colors.black;
            transform.Recolor();
        }

        private void Grey_Click(object sender, RoutedEventArgs e)
        {
            transform.color = (short)MyTransform.Colors.gray;
            transform.Recolor();
        }

        private void Red_Click(object sender, RoutedEventArgs e)
        {
            transform.color = (short)MyTransform.Colors.red;
            transform.Recolor();
        }

        private void Yellow_Click(object sender, RoutedEventArgs e)
        {
            transform.color = (short)MyTransform.Colors.yellow;
            transform.Recolor();
        }

        private void Blue_Click(object sender, RoutedEventArgs e)
        {
            transform.color = (short)MyTransform.Colors.blue;
            transform.Recolor();
        }

        private void Green_Click(object sender, RoutedEventArgs e)
        {
            transform.color = (short)MyTransform.Colors.green;
            transform.Recolor();
        }
        #endregion

        #region DrawSpaceMouse
        private void DrawSpace_MouseMove(object sender, MouseEventArgs e)
        {
            Point pos = Mouse.GetPosition(DrawSpace);
            switch (transform.defultMode)
            {
                case 0:
                    if (e.LeftButton == MouseButtonState.Released)
                        return;
                    transform.DrawLine(pos);
                    break;
                case 1:
                    if (e.LeftButton == MouseButtonState.Released)
                        return;
                    transform.DrawRectangle(pos);
                    break;
                case 2:
                    if (e.LeftButton == MouseButtonState.Released)
                        return;
                    transform.DrawEllipse(pos);
                    break;
                case 3:
                    if (e.LeftButton == MouseButtonState.Pressed)
                        transform.TranslateTransform(pos);
                    else if (transform.TakenShapes.Count == 1)
                    {
                        try
                        {
                        DrawSpace.Children.Add(transform.Resize(pos, transform.TakenShapes.Keys.First()));
                        }
                        catch
                        {

                        }
                    }
                    else
                        return;
                    break;
                case 4:
                    if (e.LeftButton == MouseButtonState.Released)
                        return;
                    transform.DrawTriangle(pos);
                    break;
            }
        }
        private void DrawSpace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (transform.defultMode)
            {
                case 3:

                    break;
            }
        }
        private void DrawSpace_MouseDown(object sender, MouseButtonEventArgs e)
        {
            transform.posInBlock = Mouse.GetPosition(DrawSpace);
            var myobject = transform.Drawing();
            switch (transform.defultMode)
            {
                case 0:
                case 1:
                case 2:
                case 4:
                    DrawSpace.Children.Add(myobject);
                    break;
                case 3:
                    if (e.LeftButton == MouseButtonState.Pressed)
                        transform.Select(e);
                    break;
            }
        }
        #endregion
        #region KeyDown
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    transform.DeleteFromTaken(ref DrawSpace);
                    break;
            }
        }
        #endregion

        private void RotateButton(object sender, RoutedEventArgs e)
        {
            /*foreach (Shape shape in TakenShapes.Keys)
            {
                var index = DrawSpace.Children.IndexOf(shape);
                //DrawSpace.Children.RemoveAt(index);
                //
                //var children = DrawSpace.Children[DrawSpace.Children.IndexOf(shape)];
                //
                var rotateTransform = shape.RenderTransform as RotateTransform;
                //var rotateTransform = children.RenderTransform as RotateTransform;
                var transform = new RotateTransform(45 + (rotateTransform?.Angle ?? 0));
                var angel = transform.Angle * Math.PI / 180;
                if (shape.Name == "Line")
                {
                    Line temp = shape as Line;
                    Point center = new Point((temp.X1 + temp.X2) / 2, (temp.Y1 + temp.Y2) / 2);
                    double x, y;
                    x = temp.X1;
                    y = temp.Y1;
                    Rot(ref x, ref y, angel, center);
                    temp.X1 = x;
                    temp.Y1 = y;
                    //
                    x = temp.X2;
                    y = temp.Y2;
                    Rot(ref x, ref y, angel, center);
                    temp.X2 = x;
                    temp.Y2 = y;
                    
                    transform.CenterX = centerX / 2;
                transform.CenterY = centerY / 2;
                shape.RenderTransform = transform;
                    //DrawSpace.Children.Add(temp);
                }

            }*/
            /* foreach(Shape shape in TakenShapes.Keys)
             {

             }*/
        }
        private void Rot(ref double x, ref double y, double angel, Point center)
        {
            double[] n = new double[2] { x, y };
            double[,] m = { { Math.Cos(angel), -Math.Sin(angel) }, { Math.Sin(angel), Math.Cos(angel) } };
            x = (n[0] * m[0, 0]) + (n[1] * m[1, 0]);
            y = (n[0] * m[0, 1]) + (n[1] * m[1, 1]);
        }
        private void ToGroup(object sender, RoutedEventArgs e)
        {
            //    double maxX = 0;
            //    double minX = 10000;
            //    double maxY = 0;
            //    double minY = 10000;
            //    Dictionary<Line, Brush> temp = new Dictionary<Line, Brush>(TakenLines);
            //    foreach (Line line in TakenLines.Keys)
            //    {
            //        #region rectangle
            //        if (line.X1 > maxX || line.X2 > maxX)
            //        {
            //            if (line.X1 > line.X2)
            //                maxX = line.X1;
            //            else
            //                maxX = line.X2;
            //        }
            //        if (line.X1 < minX || line.X2 < minX)
            //        {
            //            if (line.X1 < line.X2)
            //                minX = line.X1;
            //            else
            //                minX = line.X2;
            //        }
            //        if (line.Y1 > maxY || line.Y2 > maxY)
            //        {
            //            if (line.Y1 > line.Y2)
            //                maxY = line.Y1;
            //            else
            //                maxY = line.Y2;
            //        }
            //        if (line.Y1 < minY || line.Y2 < minY)
            //        {
            //            if (line.Y1 < line.Y2)
            //                minY = line.Y1;
            //            else
            //                minY = line.Y2;
            //        }
            //        #endregion
            //        //var children = DrawSpace.Children[DrawSpace.Children.IndexOf(line)];

            //        temp.Remove(line);

            //    }
            //    Rectangle rect = new Rectangle();
            //    rect.Stroke = System.Windows.Media.Brushes.Aqua;
            //    rect.Width = maxX - minX;
            //    rect.Height = (maxY - 1) - (minY - 1);
            //    DrawSpace.Children.Add(rect);
            //    Canvas.SetTop(rect, ((maxY+minY)/2)-rect.Height/2);
            //    Canvas.SetLeft(rect, ((maxX+minX)/2)-rect.Width/2);
        }
        #region SwitchMode
        private void LineButton_Click(object sender, RoutedEventArgs e)
        {
            transform.defultMode = (short)MyTransform.Mode.line;
            transform.TakenShapeClear();
        }

        private void RectButton_Click(object sender, RoutedEventArgs e)
        {
            transform.defultMode = (short)MyTransform.Mode.rect;
            transform.TakenShapeClear();
        }
        private void DragButton_Click(object sender, RoutedEventArgs e)
        {
            transform.defultMode = (short)MyTransform.Mode.edit;
        }

        private void FigureThickness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            transform.strongBrush = FigureThickness.Value;
        }

        private void EllipseButton_Click(object sender, RoutedEventArgs e)
        {
            transform.defultMode = (short)MyTransform.Mode.elips;
        }

        private void TriangleButton_Click(object sender, RoutedEventArgs e)
        {
            transform.defultMode = (short)MyTransform.Mode.triangle;
        }
        #endregion

        private void Fill(object sender, RoutedEventArgs e)
        {
            transform.Fill();
        }
    }
}
