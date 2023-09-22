using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using static System.Windows.Forms.LinkLabel;

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
            transform = new MyTransform(FigureThickness.Value, this);
        }
        #region SwitchColor
        private void Black_Click(object sender, RoutedEventArgs e)
        {
            transform.color = (short)MyTransform.Colors.black;
            transform.Recolor(false);
            ColorPicker.Fill = Black.Background;
        }

        private void Grey_Click(object sender, RoutedEventArgs e)
        {
            transform.color = (short)MyTransform.Colors.gray;
            transform.Recolor(false);
            ColorPicker.Fill = Grey.Background;
        }

        private void Red_Click(object sender, RoutedEventArgs e)
        {
            transform.color = (short)MyTransform.Colors.red;
            transform.Recolor(false);
            ColorPicker.Fill = Red.Background;
        }

        private void Yellow_Click(object sender, RoutedEventArgs e)
        {
            transform.color = (short)MyTransform.Colors.yellow;
            transform.Recolor(false);
            ColorPicker.Fill = Yellow.Background;
        }

        private void Blue_Click(object sender, RoutedEventArgs e)
        {
            transform.color = (short)MyTransform.Colors.blue;
            transform.Recolor(false);
            ColorPicker.Fill = Blue.Background;
        }

        private void Green_Click(object sender, RoutedEventArgs e)
        {
            transform.color = (short)MyTransform.Colors.green;
            transform.Recolor(false);
            ColorPicker.Fill = Green.Background;
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
                    break;
                case 4:
                    if (e.LeftButton == MouseButtonState.Released)
                        return;
                    transform.DrawTriangle(pos);
                    break;
            }
        }
        private void VisibleElementSize(Shape shape = null)
        {
            if (shape != null)
                switch (shape.Name)
                {
                    case "Line":
                        length.Text = Convert.ToString((shape as Polyline).Width);
                        width.Text = "---";
                        break;
                    case "Rectangle":
                        length.Text = Convert.ToString((shape as Rectangle).Height);
                        width.Text = Convert.ToString((shape as Rectangle).Width);
                        break;
                    case "Ellipse":
                        length.Text = Convert.ToString((shape as Ellipse).Height);
                        width.Text = Convert.ToString((shape as Ellipse).Width);
                        break;
                    case "Triangle":
                        length.Text = Convert.ToString((shape as Polygon).Height);
                        width.Text = Convert.ToString((shape as Polygon).Width);
                        break;
                }
            else
            {
                length.Text = "---";
                width.Text = "---";
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
                    try
                    {
                        if (transform.TakenShapes.Count == 1)
                            VisibleElementSize(transform.TakenShapes.First().Key);
                        else
                            VisibleElementSize();
                    }
                    catch
                    {

                    }
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
            try
            {
                transform.Rotate();
            }
            catch { }
        }
        private void ToGroup(object sender, RoutedEventArgs e)
        {
            try
            {
                if (transform.TakenShapes.Count > 1 && transform.ShapeGroupContains(transform.TakenShapes.First().Key) == -1)
                {
                    transform.ShapeGroup.Add(new System.Collections.Generic.Dictionary<Shape, Brush>(transform.TakenShapes));
                }
                else if (transform.ShapeGroupContains(transform.TakenShapes.First().Key) >= 0)
                {
                    transform.ShapeGroup[transform.ShapeGroupContains(transform.TakenShapes.First().Key)].Clear();
                }
            }
            catch { }
            /*
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
            */
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
            transform.BrushEdit();
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
            transform.Recolor(true);
        }

        private void length_TextChanged(object sender, TextChangedEventArgs e)
        {
            //edit lenght
            try
            {
                switch (transform.TakenShapes.First().Key.Name)
                {
                    case "Line":
                        (transform.TakenShapes.First().Key as Polyline).Width = Convert.ToDouble(length.Text);
                        break;
                    case "Rectangle":
                        (transform.TakenShapes.First().Key as Rectangle).Height = Convert.ToDouble(length.Text);
                        break;
                    case "Ellipse":
                        (transform.TakenShapes.First().Key as Ellipse).Height = Convert.ToDouble(length.Text);
                        break;
                    case "Triangle":
                        (transform.TakenShapes.First().Key as Polygon).Height = Convert.ToDouble(length.Text);
                        break;
                    default:
                        width.Text = "---";
                        length.Text = "---";
                        break;
                }
            }
            catch { }
        }

        private void width_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                switch (transform.TakenShapes.First().Key.Name)
                {
                    case "Rectangle":
                        (transform.TakenShapes.First().Key as Rectangle).Width = Convert.ToDouble(width.Text);
                        break;
                    case "Ellipse":
                        (transform.TakenShapes.First().Key as Ellipse).Width = Convert.ToDouble(width.Text);
                        break;
                    case "Triangle":
                        (transform.TakenShapes.First().Key as Polygon).Width = Convert.ToDouble(width.Text);
                        break;
                }
            }
            catch { }
        }
    }
}
