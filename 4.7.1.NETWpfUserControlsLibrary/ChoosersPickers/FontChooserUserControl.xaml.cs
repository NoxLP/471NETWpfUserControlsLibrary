using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace NET471WpfUserControlsLibrary.ChoosersPickers
{
    /// <summary>
    /// Lógica de interacción para FontChooserUserControl.xaml
    /// </summary>
    public partial class FontChooserUserControl : UserControl, INotifyPropertyChanged
    {
        public class HorizontalAlignmentCBItem
        {
            public string Source { get; set; }
            public TextAlignment HAlign { get; set; }
        }
        public class VerticalAlignmentCBItem
        {
            public string Source { get; set; }
            public VerticalAlignment VAlign { get; set; }
        }
        public FontChooserUserControl()
        {
            InitializeComponent();

            _DisplayFontSizes = new List<double>(_DefaultFontSizes);
            OnPropertyChanged(nameof(FontSizes));
        }

        #region fields
        private double[] _DefaultFontSizes = new double[] { 5, 6, 7, 8, 9, 10, 11, 12, 14, 18, 24, 36 };
        private HorizontalAlignmentCBItem[] _HorizontalAlignmentsCBItems = new HorizontalAlignmentCBItem[]
        {
            new HorizontalAlignmentCBItem { Source = "pack://application:,,,/4.7.1.NETWpfUserControlsLibrary;component/Resources/TextAlignmentHLeft.png", HAlign = TextAlignment.Left},
            new HorizontalAlignmentCBItem { Source = "pack://application:,,,/4.7.1.NETWpfUserControlsLibrary;component/Resources/TextAlignmentHCenter.png", HAlign = TextAlignment.Center},
            new HorizontalAlignmentCBItem { Source = "pack://application:,,,/4.7.1.NETWpfUserControlsLibrary;component/Resources/TextAlignmentHRight.png", HAlign = TextAlignment.Right},
            new HorizontalAlignmentCBItem { Source = "pack://application:,,,/4.7.1.NETWpfUserControlsLibrary;component/Resources/TextAlignmentHJustify.png", HAlign = TextAlignment.Justify}
        };
        private VerticalAlignmentCBItem[] _VerticalAlignmentCBItems = new VerticalAlignmentCBItem[]
        {
            new VerticalAlignmentCBItem { Source = "pack://application:,,,/4.7.1.NETWpfUserControlsLibrary;component/Resources/TextAlignmentVTop.png", VAlign = VerticalAlignment.Top},
            new VerticalAlignmentCBItem { Source = "pack://application:,,,/4.7.1.NETWpfUserControlsLibrary;component/Resources/TextAlignmentVCenter.png", VAlign = VerticalAlignment.Center},
            new VerticalAlignmentCBItem { Source = "pack://application:,,,/4.7.1.NETWpfUserControlsLibrary;component/Resources/TextAlignmentVBottom.png", VAlign = VerticalAlignment.Bottom}
        };
        private List<double> _DisplayFontSizes;
        private bool _IsBold;
        private bool _IsItalic;
        private bool _IsUnderlined;
        private bool _IsStrikeThrough;
        #endregion

        #region properties
        public ICollectionView FontSizes
        {
            get
            {
                var coll = CollectionViewSource.GetDefaultView(_DisplayFontSizes);
                return coll;
            }
        }
        public ICollectionView HorizontalAlignments
        {
            get
            {
                var coll = CollectionViewSource.GetDefaultView(_HorizontalAlignmentsCBItems);
                return coll;
            }
        }
        public ICollectionView VerticalAlignments
        {
            get
            {
                var coll = CollectionViewSource.GetDefaultView(_VerticalAlignmentCBItems);
                return coll;
            }
        }
        public bool IsBold
        {
            get { return _IsBold; }
            set
            {
                if (_IsBold != value)
                {
                    _IsBold = value;
                    if (value)
                        ChoosedFontWeight = FontWeights.Bold;
                    else
                        ChoosedFontWeight = FontWeights.Normal;
                    OnPropertyChanged(nameof(IsBold));
                }
            }
        }
        public bool IsItalic
        {
            get { return _IsItalic; }
            set
            {
                if (_IsItalic != value)
                {
                    _IsItalic = value;
                    if (value)
                        ChoosedFontStyle = FontStyles.Italic;
                    else
                        ChoosedFontStyle = FontStyles.Normal;
                    OnPropertyChanged(nameof(IsItalic));
                }
            }
        }
        public bool IsUnderlined
        {
            get { return _IsUnderlined; }
            set
            {
                if (_IsUnderlined != value)
                {
                    _IsUnderlined = value;
                    if (value)
                    {
                        IsStrikeThrough = false;
                        ChoosedTextDecoration = TextDecorations.Underline;
                    }
                    else
                        ChoosedTextDecoration = null;
                    OnPropertyChanged(nameof(IsUnderlined));
                }
            }
        }
        public bool IsStrikeThrough
        {
            get { return _IsStrikeThrough; }
            set
            {
                if (_IsStrikeThrough != value)
                {
                    _IsStrikeThrough = value;
                    if (value)
                    {
                        IsUnderlined = false;
                        ChoosedTextDecoration = TextDecorations.Strikethrough;
                    }
                    else
                        ChoosedTextDecoration = null;
                    OnPropertyChanged(nameof(IsStrikeThrough));
                }
            }
        }
        #endregion

        #region dependency properties
        public FontFamily ChoosedFontFamily
        {
            get { return (FontFamily)GetValue(ChoosedFontFamilyProperty); }
            set { SetValue(ChoosedFontFamilyProperty, value); }
        }
        public static readonly DependencyProperty ChoosedFontFamilyProperty =
            DependencyProperty.Register("ChoosedFontFamily", typeof(FontFamily), typeof(FontChooserUserControl), new PropertyMetadata(new FontFamily("Arial")));
        public double ChoosedFontSize
        {
            get { return (double)GetValue(ChoosedFontSizeProperty); }
            set { SetValue(ChoosedFontSizeProperty, value); }
        }
        public static readonly DependencyProperty ChoosedFontSizeProperty =
            DependencyProperty.Register("ChoosedFontSize", typeof(double), typeof(FontChooserUserControl), new PropertyMetadata(10d));
        public FontWeight ChoosedFontWeight
        {
            get { return (FontWeight)GetValue(ChoosedFontWeightProperty); }
            set { SetValue(ChoosedFontWeightProperty, value); }
        }
        public static readonly DependencyProperty ChoosedFontWeightProperty =
            DependencyProperty.Register("ChoosedFontWeight", typeof(FontWeight), typeof(FontChooserUserControl), new PropertyMetadata(FontWeights.Normal));
        public FontStretch ChoosedFontStretch
        {
            get { return (FontStretch)GetValue(ChoosedFontStretchProperty); }
            set { SetValue(ChoosedFontStretchProperty, value); }
        }
        public static readonly DependencyProperty ChoosedFontStretchProperty =
            DependencyProperty.Register("ChoosedFontStretch", typeof(FontStretch), typeof(FontChooserUserControl), new PropertyMetadata(FontStretches.Normal));
        public FontStyle ChoosedFontStyle
        {
            get { return (FontStyle)GetValue(ChoosedFontStyleProperty); }
            set { SetValue(ChoosedFontStyleProperty, value); }
        }
        public static readonly DependencyProperty ChoosedFontStyleProperty =
            DependencyProperty.Register("ChoosedFontStyle", typeof(FontStyle), typeof(FontChooserUserControl), new PropertyMetadata(FontStyles.Normal));
        public string ChoosedFontSizeText
        {
            get { return (string)GetValue(ChoosedFontSizeTextProperty); }
            set { SetValue(ChoosedFontSizeTextProperty, value); }
        }
        public static readonly DependencyProperty ChoosedFontSizeTextProperty =
            DependencyProperty.Register("ChoosedFontSizeText", typeof(string), typeof(FontChooserUserControl), new PropertyMetadata(""));
        public TextDecorationCollection ChoosedTextDecoration
        {
            get { return (TextDecorationCollection)GetValue(ChoosedTextDecorationProperty); }
            set { SetValue(ChoosedTextDecorationProperty, value); }
        }
        public static readonly DependencyProperty ChoosedTextDecorationProperty =
            DependencyProperty.Register("ChoosedTextDecoration", typeof(TextDecorationCollection), typeof(FontChooserUserControl), new PropertyMetadata(null));
        public Brush ChoosedFontBrush
        {
            get { return (Brush)GetValue(ChoosedFontBrushProperty); }
            set { SetValue(ChoosedFontBrushProperty, value); }
        }
        public static readonly DependencyProperty ChoosedFontBrushProperty =
            DependencyProperty.Register("ChoosedFontBrush", typeof(Brush), typeof(FontChooserUserControl), new PropertyMetadata(Brushes.Black));
        public TextAlignment ChoosedHorizontalAlignment
        {
            get { return (TextAlignment)GetValue(ChoosedHorizontalAlignmentProperty); }
            set { SetValue(ChoosedHorizontalAlignmentProperty, value); }
        }
        public static readonly DependencyProperty ChoosedHorizontalAlignmentProperty =
            DependencyProperty.Register("ChoosedHorizontalAlignment", typeof(TextAlignment), typeof(FontChooserUserControl), new PropertyMetadata(TextAlignment.Left));
        public VerticalAlignment ChoosedVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(ChoosedVerticalAlignmentProperty); }
            set { SetValue(ChoosedVerticalAlignmentProperty, value); }
        }
        public static readonly DependencyProperty ChoosedVerticalAlignmentProperty =
            DependencyProperty.Register("ChoosedVerticalAlignment", typeof(VerticalAlignment), typeof(FontChooserUserControl), new PropertyMetadata(VerticalAlignment.Top));
        #endregion

        private void AddTextToFontSizesListIfDoesntContains()
        {
            double size;
            if (double.TryParse(ChoosedFontSizeText, out size) && !_DisplayFontSizes.Contains(size))
            {
                ChoosedFontSize = size;
                _DisplayFontSizes.Add(size);
            }
        }
        private void SizeChooser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                AddTextToFontSizesListIfDoesntContains();
            }
        }
        private void SizeChooser_LostFocus(object sender, RoutedEventArgs e)
        {
            AddTextToFontSizesListIfDoesntContains();
        }

        #region property changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        #endregion
    }
}
