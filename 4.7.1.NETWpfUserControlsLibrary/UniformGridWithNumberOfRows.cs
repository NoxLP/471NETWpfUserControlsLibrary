using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NET471WpfUserControlsLibrary
{
    public class UniformGridWithNumberOfRows : Panel
    {
        //private double _TotalChildrenWidth;
        private Size _LastFinalSize;
        private int _ChildrensPerRow;
        private Size _SizePerChildren;

        #region dependency properties
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register("Padding", typeof(Thickness), typeof(UniformGridWithNumberOfRows), 
                new PropertyMetadata(new Thickness(0,0,0,0), OnSizePropertyChanged));

        public int NumberOfRows
        {
            get { return (int)GetValue(NumberOfRowsProperty); }
            set { SetValue(NumberOfRowsProperty, value); }
        }
        public static readonly DependencyProperty NumberOfRowsProperty =
            DependencyProperty.Register("NumberOfRows", typeof(int), typeof(UniformGridWithNumberOfRows), 
                new PropertyMetadata(1, OnSizePropertyChanged));
        private static void OnSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ug = d as UniformGridWithNumberOfRows;
            ug.ArrangeOverride(ug._LastFinalSize);
        }
        #endregion

        private double Clamp(double min, double max, double value)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            Size size = new Size(0, 0);
            //_TotalChildrenWidth = 0d;
            int count = Children.Count;
            _ChildrensPerRow = count / NumberOfRows;
            
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
                //_TotalChildrenWidth += child.DesiredSize.Width;
                size.Width = Math.Max(size.Width, child.DesiredSize.Width);
                size.Height = Math.Max(size.Height, child.DesiredSize.Height);
            }

            size.Width = Clamp(
                MinWidth, 
                MaxWidth,
                double.IsPositiveInfinity(availableSize.Width) ? size.Width : availableSize.Width);
            size.Height = Clamp(
                MinWidth,
                MaxWidth,
                double.IsPositiveInfinity(availableSize.Height) ? size.Height : availableSize.Height);

            _SizePerChildren = new Size(
                (size.Width + (Padding.Left * count) + (Padding.Right * count)) / _ChildrensPerRow,
                (size.Height + (Padding.Top * count) + (Padding.Bottom * count)) / NumberOfRows);

            foreach (UIElement child in Children)
            {
                child.Measure(size);
            }

            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _LastFinalSize = finalSize;
            int count = Children.Count;
            
            int currentRow;
            UIElement child;
            for (int i = 0; i < count; i++)
            {
                child = Children[i];
                currentRow = (_ChildrensPerRow % (i + 1));
                double width = _SizePerChildren.Width * i;
                double height = _SizePerChildren.Height * currentRow;
                Point topLeft = new Point();
                if(i == 0)
                    topLeft.X = Padding.Left + width;
                else if(i < count)
                    topLeft.X = Padding.Left + Padding.Right + width;
                else
                    topLeft.X = Padding.Right + width;

                if (currentRow == 0)
                    topLeft.Y = Padding.Top + height;
                else if (currentRow < NumberOfRows)
                    topLeft.Y = Padding.Top + Padding.Bottom + height;
                else
                    topLeft.Y = Padding.Bottom + height;

                child.Arrange(new Rect(topLeft, _SizePerChildren));
            }

            return finalSize;
        }
    }
}
