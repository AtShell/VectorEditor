using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Shapes;
using static System.Windows.Forms.LinkLabel;
using System.Windows.Controls;
using static WpfApp1.MainWindow;
using System.Windows.Media;
using System.Xml.Linq;
using System.Windows.Ink;

namespace WpfApp1
{
    internal class MyTransform
    {
        #region Enums
        public enum Mode
        {
            line,
            rect,
            elips,
            edit,
            triangle
        }
        public enum Colors
        {
            black,
            gray,
            red,
            blue,
            yellow,
            green
        }
        #endregion

        #region Variables

        public Point posInBlock;

        private Rectangle rect;
        private Ellipse elipse;
        private Polygon triangle;
        private Polyline pline;
        private MainWindow window;
        public double strongBrush;
        public short defultMode = 0;
        public short color = 0;
        public Dictionary<Shape, Brush> TakenShapes = new Dictionary<Shape, Brush>();
        public List<Dictionary<Shape, Brush>> ShapeGroup = new List<Dictionary<Shape, Brush>>();
        #endregion

        public MyTransform(double strong, MainWindow win)
        {
            strongBrush = strong;
            window = win;
        }
        public MyTransform() { }

        #region Drawing Shapes
        public Shape Drawing()
        {
            switch (defultMode)
            {
                case 0:
                    pline = new Polyline();
                    InitializeColorStroke(pline);
                    pline.Points = new PointCollection
                    {
                        new Point(1,0),
                        new Point(2,1),
                    };
                    Canvas.SetLeft(pline, posInBlock.X);
                    Canvas.SetTop(pline, posInBlock.Y);
                    return pline;
                case 1:
                    rect = new Rectangle();
                    InitializeColorStroke(rect);
                    Canvas.SetLeft(rect, posInBlock.X);
                    Canvas.SetTop(rect, posInBlock.Y);
                    return rect;
                case 2:
                    elipse = new Ellipse();
                    InitializeColorStroke(elipse);
                    Canvas.SetLeft(elipse, posInBlock.X);
                    Canvas.SetTop(elipse, posInBlock.Y);
                    return elipse;
                case 4:
                    triangle = new Polygon();
                    InitializeColorStroke(triangle);
                    triangle.Points = new PointCollection
                    {
                        new Point(1, 0),
                        new Point(0, 1),
                        new Point(2, 1)
                    };
                    Canvas.SetLeft(triangle, posInBlock.X);
                    Canvas.SetTop(triangle, posInBlock.Y);
                    return triangle;
                default: return null;
            }
        }
        public void DrawLine(Point pos)
        {
            var x = Math.Min(pos.X, posInBlock.X);
            var y = Math.Min(pos.Y, posInBlock.Y);
            var w = Math.Max(pos.X, posInBlock.X) - x;
            pline.Name = "Line";
            pline.Width = w;
            pline.Stretch = Stretch.Fill;
            Canvas.SetLeft(pline, x);
            Canvas.SetTop(pline, y);
        }
        public void DrawRectangle(Point pos)
        {
            var x = Math.Min(pos.X, posInBlock.X);
            var y = Math.Min(pos.Y, posInBlock.Y);
            var w = Math.Max(pos.X, posInBlock.X) - x;
            var h = Math.Max(pos.Y, posInBlock.Y) - y;
            rect.Width = w;
            rect.Height = h;
            rect.Name = "Rectangle";
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
        }
        public void DrawEllipse(Point pos)
        {
            var x = Math.Min(pos.X, posInBlock.X);
            var y = Math.Min(pos.Y, posInBlock.Y);
            var w = Math.Max(pos.X, posInBlock.X) - x;
            var h = Math.Max(pos.Y, posInBlock.Y) - y;
            elipse.Width = w;
            elipse.Height = h;
            elipse.Name = "Ellipse";
            Canvas.SetLeft(elipse, x);
            Canvas.SetTop(elipse, y);
        }
        public void DrawTriangle(Point pos)
        {
            var x = Math.Min(pos.X, posInBlock.X);
            var y = Math.Min(pos.Y, posInBlock.Y);
            var w = Math.Max(pos.X, posInBlock.X) - x;
            var h = Math.Max(pos.Y, posInBlock.Y) - y;
            triangle.Name = "Triangle";
            triangle.Width = w;
            triangle.Height = h;
            triangle.Stretch = Stretch.Fill;
            Canvas.SetLeft(triangle, x);
            Canvas.SetTop(triangle, y);
        }
        #endregion

        #region Color
        public void InitializeColorStroke(Shape shape)
        {
            switch (color)
            {
                case 0:
                    shape.Stroke = Brushes.Black;
                    break;
                case 1:
                    shape.Stroke = Brushes.Gray;
                    break;
                case 2:
                    shape.Stroke = Brushes.Red;
                    break;
                case 3:
                    shape.Stroke = Brushes.Blue;
                    break;
                case 4:
                    shape.Stroke = Brushes.Yellow;
                    break;
                case 5:
                    shape.Stroke = Brushes.Green;
                    break;
            }
            shape.StrokeThickness = strongBrush;
        }
        public void InitializeColorFill(Shape shape)
        {
            switch (color)
            {
                case 0:
                    shape.Fill = Brushes.Black;
                    break;
                case 1:
                    shape.Fill = Brushes.Gray;
                    break;
                case 2:
                    shape.Fill = Brushes.Red;
                    break;
                case 3:
                    shape.Fill = Brushes.Blue;
                    break;
                case 4:
                    shape.Fill = Brushes.Yellow;
                    break;
                case 5:
                    shape.Fill = Brushes.Green;
                    break;
            }
        }
        public void Recolor(bool fill)
        {
            foreach (Shape shape in TakenShapes.Keys)
            {
                if (fill)
                    InitializeColorFill(shape);
                else
                    InitializeColorStroke(shape);
            }
            if (fill)
                TakenShapeClear();
            else
                TakenShapes.Clear();
        }
        #endregion


        #region test

        #endregion


        #region TakenList manipulation
        public int ShapeGroupContains(Shape ex)
        {
            int counter = 0;
            foreach (var pair in ShapeGroup)
            {
                if (pair.ContainsKey(ex))
                {
                    return counter;
                }
            }
            return -1; //notContainsKey
        }
        public void Select(MouseButtonEventArgs e)
        {
            try
            {
                var type = (Shape)e.Source;
                int num = ShapeGroupContains(type);
                if (ShapeGroup.Count >= 1 && num != -1)
                {

                    foreach (var shape in ShapeGroup[num])
                    {
                        TakenShapes.Add(shape.Key, Brushes.Aqua);
                    }
                    return;
                }
                //add to empty list
                if (TakenShapes.Count == 0)
                {
                    AddShape(ref type);
                    window.FigureThickness.Value = TakenShapes.First().Key.StrokeThickness;
                    return;
                }
                //add or switch 
                if (TakenShapes.Count == 1)
                {
                    window.FigureThickness.Value = TakenShapes.First().Key.StrokeThickness;

                    if (TakenShapes.ContainsKey(type))
                    {
                        TakenShapeRemove(type);
                        return;
                    }
                    else if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        AddShape(ref type);
                        return;
                    }
                    else
                    {
                        TakenShapeClear();
                        AddShape(ref type);
                        window.FigureThickness.Value = TakenShapes.First().Key.StrokeThickness;
                        return;
                    }
                }
                //add to list or remove from
                if (TakenShapes.Count > 1)
                {
                    if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        if (TakenShapes.ContainsKey(type))
                        {
                            TakenShapeRemove(type);
                            return;
                        }
                        else
                        {
                            AddShape(ref type);
                            return;
                        }

                    }
                    else
                    {
                        if (!TakenShapes.ContainsKey(type))
                        {
                            TakenShapeClear();
                            AddShape(ref type);
                            return;
                        }
                    }
                }
            }
            catch
            {
                TakenShapeClear();
            }
        }
        public void AddShape(ref Shape shape)
        {
            TakenShapes.Add(shape, shape.Stroke);
            shape.Stroke = Brushes.Aqua;
        }
        public void TakenShapeClear()
        {
            try
            {
                foreach (Shape shape in TakenShapes.Keys)
                {
                    int num = ShapeGroupContains(shape);
                    if (num == -1)
                        shape.Stroke = TakenShapes[shape];
                    else
                    {
                        Shape s = ShapeGroup[num].First().Key;
                        shape.Stroke = s.Stroke;
                    }
                }
            }
            catch { }
            TakenShapes.Clear();
        }
        public void TakenShapeRemove(Shape shape)
        {
            shape.Stroke = TakenShapes[shape];
            TakenShapes.Remove(shape);
        }
        public void DeleteFromTaken(ref Canvas DrawSpace)
        {
            foreach (Shape shape in TakenShapes.Keys)
            {
                DrawSpace.Children.Remove(shape);
            }
            TakenShapeClear();
        }
        #endregion

        #region Tranformation Shape
        public void TranslateTransform(Point pos)
        {
            double offset_x = pos.X - posInBlock.X;
            double offset_y = pos.Y - posInBlock.Y;
            double new_x, new_y;
            foreach (Shape shape in TakenShapes.Keys)
            {
                new_x = Canvas.GetLeft(shape) + offset_x;
                new_y = Canvas.GetTop(shape) + offset_y;
                Canvas.SetLeft(shape, new_x);
                Canvas.SetTop(shape, new_y);
                break;
            }
            posInBlock = pos;
        }
        public void BrushEdit()
        {
            foreach (Shape shape in TakenShapes.Keys)
            {
                shape.StrokeThickness = strongBrush;
            }
        }
        public void Rotate()
        {
            foreach (Shape shape in TakenShapes.Keys)
            {
                var rt = shape.RenderTransform as RotateTransform;
                var trans = new RotateTransform(45 + (rt?.Angle ?? 0));
                shape.RenderTransformOrigin = new Point(0.5, 0.5);
                shape.RenderTransform = trans;
            }
        }
        #endregion
    }
}
