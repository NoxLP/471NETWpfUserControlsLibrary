using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    /// Lógica de interacción para ColorPickerPalette.xaml
    /// </summary>
    public partial class ColorPickerPalette : UserControl
    {
        public ColorPickerPalette()
        {
            InitializeComponent();
            
            Standard = new List<Brush>()
            {
                Brushes.Transparent,
                Brushes.White,
                Brushes.Gray,
                Brushes.Black,
                Brushes.Red,
                Brushes.Green,
                Brushes.Blue,
                Brushes.Cyan,
                Brushes.Yellow,
                Brushes.Orange,
                Brushes.Violet
            };
            var more = typeof(Brushes).GetProperties()
                .Select(x => x.GetValue(null) as Brush);
            more = more.OrderBy(x =>
                {
                    System.Windows.Media.Color winColor = ((SolidColorBrush)x).Color;
                    System.Drawing.Color drawColor = System.Drawing.Color.FromArgb(winColor.R, winColor.G, winColor.B);
    
                    return drawColor.GetHue() + drawColor.GetSaturation();
                });
            MoreLB.ItemsSource = more;
            Loaded += ColorPickerPalette_Loaded;
        }
        private void ColorPickerPalette_Loaded(object sender, RoutedEventArgs e)
        {
            if (SelectedBrush != null)
                return;
            if (DefaultSelectedBrush != null)
            {
                SelectedBrush = DefaultSelectedBrush;
                DefaultBorder.Visibility = Visibility.Visible;
                DefaultList.Visibility = Visibility.Visible;
                DefaultList.ItemsSource = new Brush[1] { DefaultSelectedBrush };
            }
            else
            {
                DefaultBorder.Visibility = Visibility.Collapsed;
                DefaultList.Visibility = Visibility.Collapsed;
            }
        }

        private bool _ScriptUnChecked;

        public List<Brush> Standard
        {
            get { return (List<Brush>)GetValue(StandardProperty); }
            set { SetValue(StandardProperty, value); }
        }
        public static readonly DependencyProperty StandardProperty =
            DependencyProperty.Register("Standard", typeof(List<Brush>), typeof(ColorPickerPalette), new PropertyMetadata(null));
        public ObservableCollection<Brush> SpecialBrushes
        {
            get { return (ObservableCollection<Brush>)GetValue(SpecialBrushesProperty); }
            set { SetValue(SpecialBrushesProperty, value); }
        }
        public static readonly DependencyProperty SpecialBrushesProperty =
            DependencyProperty.Register(
                "SpecialBrushes", typeof(ObservableCollection<Brush>), typeof(ColorPickerPalette), new PropertyMetadata(new ObservableCollection<Brush>(), SpecialChanged));
        private static void SpecialChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPickerPalette)d).InstanceSpecialChanged(e);
        }
        private void InstanceSpecialChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var coll = (INotifyCollectionChanged)e.NewValue;
                coll.CollectionChanged += SpecialBrushesCollectionChanged;
            }
            if (e.OldValue != null)
            {
                var coll = (INotifyCollectionChanged)e.NewValue;
                coll.CollectionChanged -= SpecialBrushesCollectionChanged;
            }
        }
        private void SpecialBrushesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(SpecialBrushes.Count > 0 && SpecialBorder.Visibility != Visibility.Visible)
            {
                SpecialBorder.Visibility = Visibility.Visible;
                SpecialList.Visibility = Visibility.Visible;
            }
            else if(SpecialBrushes.Count == 0 && SpecialBorder.Visibility != Visibility.Collapsed)
            {
                SpecialBorder.Visibility = Visibility.Collapsed;
                SpecialList.Visibility = Visibility.Collapsed;
            }
        }
        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }
        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(ColorPickerPalette), new PropertyMetadata(null));
        public Thickness InternalPadding
        {
            get { return (Thickness)GetValue(InternalPaddingProperty); }
            set { SetValue(InternalPaddingProperty, value); }
        }
        public static readonly DependencyProperty InternalPaddingProperty =
            DependencyProperty.Register("InternalPadding", typeof(Thickness), typeof(ColorPickerPalette), new PropertyMetadata(new Thickness(2)));
        public Brush DefaultSelectedBrush
        {
            get { return (Brush)GetValue(DefaultSelectedBrushProperty); }
            set { SetValue(DefaultSelectedBrushProperty, value); }
        }
        public static readonly DependencyProperty DefaultSelectedBrushProperty =
            DependencyProperty.Register("DefaultSelectedBrush", typeof(Brush), typeof(ColorPickerPalette), new PropertyMetadata(null));

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = true;
        }
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if(_ScriptUnChecked)
            {
                _ScriptUnChecked = false;
                return;
            }

            if (popup.IsOpen)
                popup.IsOpen = false;
        }
        private void Popup_Closed(object sender, EventArgs e)
        {
            if (TB.IsChecked.HasValue && TB.IsChecked.Value)
            {
                _ScriptUnChecked = true;
                TB.IsChecked = false;
            }
        }
    }
}
