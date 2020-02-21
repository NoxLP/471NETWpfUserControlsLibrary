using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace NET471WpfUserControlsLibrary.RestrictedTextBoxes
{
    /// <summary>
    /// Lógica de interacción para DateTimeInputTextBox2.xaml
    /// </summary>
    public partial class DateTimeInputTextBox : UserControl, INotifyPropertyChanged
    {
        public enum DatePart { None, Day, Month, Year }

        public DateTimeInputTextBox()
        {
            this.SizeChanged += DateTimeInputTextBox2_SizeChanged;
            this.LostFocus += DateTimeInputTextBox2_LostFocus;
            InitializeComponent();
            _PartsTBs[DatePart.Day] = DayTB;
            _PartsTBs[DatePart.Month] = MonthTB;
            _PartsTBs[DatePart.Year] = YearTB;
            DayTB.Tag = DatePart.Day;
            MonthTB.Tag = DatePart.Month;
            YearTB.Tag = DatePart.Year;

            DayTB.PreviewTextInput += TBs_PreviewTextInput;
            DayTB.TextChanged += TBs_TextChanged;
            DayTB.KeyDown += TBs_KeyDown;
            //DayTB.LostFocus += TBs_LostFocus;
            MonthTB.PreviewTextInput += TBs_PreviewTextInput;
            MonthTB.TextChanged += TBs_TextChanged;
            MonthTB.KeyDown += TBs_KeyDown;
            //MonthTB.LostFocus += TBs_LostFocus;
            YearTB.PreviewTextInput += TBs_PreviewTextInput;
            YearTB.TextChanged += TBs_TextChanged;
            YearTB.KeyDown += TBs_KeyDown;
            //YearTB.LostFocus += TBs_LostFocus;

            SetAll(DateTimeStringFormat);
        }
                

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        private Dictionary<DatePart, TextBox> _PartsTBs = new Dictionary<DatePart, TextBox>();
        private Dictionary<DatePart, int> _PartsLength = new Dictionary<DatePart, int>();
        private Dictionary<int, DatePart> _PartsOrder = new Dictionary<int, DatePart>();
        private int _TotalDigits;
        private string _Separator;
        private bool _ChangeDate;
        

        public string Separator
        {
            get { return _Separator; }
            private set
            {
                if (value != _Separator)
                {
                    _Separator = value;
                    OnPropertyChanged(nameof(Separator));
                }
            }
        }

        public string DateTimeStringFormat
        {
            get { return (string)GetValue(DateTimeStringFormatProperty); }
            set { SetValue(DateTimeStringFormatProperty, value); }
        }
        public static readonly DependencyProperty DateTimeStringFormatProperty =
            DependencyProperty.Register("DateTimeStringFormat", typeof(string), typeof(DateTimeInputTextBox),
                new PropertyMetadata("dd/MM/yyyy", new PropertyChangedCallback(StringFormat_PropertyChanged)));
        private static void StringFormat_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as DateTimeInputTextBox).SetAll((string)e.NewValue);
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(DateTimeInputTextBox), new PropertyMetadata(""));
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(DateTimeInputTextBox), 
                new PropertyMetadata(DateTime.Now));
        private static void Date_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var dtTB = obj as DateTimeInputTextBox;
            if (!dtTB._ChangeDate)
            {
                dtTB._ChangeDate = true;
                return;
            }
            dtTB.SetCurrentText((DateTime)e.NewValue);
        }

        private void SetPartsWidth()
        {
            //double ratio = Width / _TotalDigits;
            foreach (var kvp in _PartsLength)
            {
                _PartsTBs[kvp.Key].Width = kvp.Value * 11;
            }
        }        
        private string GetSeparator(string dateFormat)
        {
            if (dateFormat.Contains(" "))
                return " ";
            if (dateFormat.Contains("."))
                return ".";
            return "/";
        }
        private void SetPartsCountAndIndex(string newValue)
        {

            Separator = GetSeparator(newValue);
            int d = newValue.Count(x => x == 'd');
            int D = newValue.Count(x => x == 'D');
            int m = newValue.Count(x => x == 'm');
            int M = newValue.Count(x => x == 'M');
            int y = newValue.Count(x => x == 'y');
            int Y = newValue.Count(x => x == 'Y');

            if (d != 0 && D != 0)
                throw new ArgumentException("Can't handle both 'd' and 'D' formats together for date. Use only one of them.");
            if (m != 0 && M != 0)
                throw new ArgumentException("Can't handle both 'm' and 'M' formats together for date. Use only one of them.");
            if (y != 0 && Y != 0)
                throw new ArgumentException("Can't handle both 'y' and 'Y' formats together for date. Use only one of them.");

            _PartsLength.Remove(DatePart.Day);
            _PartsLength.Remove(DatePart.Month);
            _PartsLength.Remove(DatePart.Year);
            _TotalDigits = 0;
            int dayIndex = -1;
            int MonthIndex = -1;
            int YearIndex = -1;
            if (d > 0)
            {
                _PartsLength.Add(DatePart.Day, d);
                dayIndex = newValue.IndexOf("d");
                _TotalDigits += d + 1;
            }
            else if (D > 0)
            {
                _PartsLength.Add(DatePart.Day, D);
                dayIndex = newValue.IndexOf("D");
                _TotalDigits += D + 1;
            }
            if (m > 0)
            {
                _PartsLength.Add(DatePart.Month, m);
                MonthIndex = newValue.IndexOf("m");
                _TotalDigits += m + 1;
            }
            else if (M > 0)
            {
                _PartsLength.Add(DatePart.Month, M);
                MonthIndex = newValue.IndexOf("M");
                _TotalDigits += M + 1;
            }
            if (y != 0)
            {
                _PartsLength.Add(DatePart.Year, y);
                YearIndex = newValue.IndexOf("y");
                _TotalDigits += y;
            }
            else if (Y > 0)
            {
                _PartsLength.Add(DatePart.Year, Y);
                YearIndex = newValue.IndexOf("Y");
                _TotalDigits += Y;
            }

            List<Tuple<int, DatePart>> parts = new List<Tuple<int, DatePart>>();
            foreach (var kvp in _PartsLength)
            {
                parts.Add(new Tuple<int, DatePart>(
                    kvp.Key == DatePart.Day ? dayIndex :
                        (kvp.Key == DatePart.Month ? MonthIndex : YearIndex),
                    kvp.Key));
            }
            parts.Sort((x, x2) => x.Item1.CompareTo(x2.Item1));

            _PartsOrder.Clear();
            for (int i = 0; i < 3; i++)
            {
                _PartsOrder.Add(i, parts[i].Item2);
            }
        }
        private void SetAll(string newValue)
        {
            SetPartsCountAndIndex((string)newValue);

            int partsHidden = 0;
            int partsVisible = 0;
            foreach (var part in Enum.GetValues(typeof(DatePart)))
            {
                if ((DatePart)part == DatePart.None)
                    continue;

                if (!_PartsLength.ContainsKey((DatePart)part))
                {
                    _PartsTBs[(DatePart)part].Visibility = Visibility.Hidden;
                    partsHidden++;
                    if (partsHidden == 0)
                        Separator1.Visibility = Visibility.Hidden;
                    else
                        Separator2.Visibility = Visibility.Hidden;
                }
                else
                {
                    _PartsTBs[(DatePart)part].Visibility = Visibility.Visible;
                    partsVisible++;
                    if (partsVisible == 0)
                        Separator1.Visibility = Visibility.Visible;
                    else
                        Separator2.Visibility = Visibility.Visible;
                }
            }

            SetPartsWidth();

            foreach (var kvp2 in _PartsOrder)
            {
                switch (kvp2.Key)
                {
                    case 0:
                        Grid.SetColumn(_PartsTBs[kvp2.Value], 0);
                        break;
                    case 1:
                        Grid.SetColumn(_PartsTBs[kvp2.Value], 2);
                        break;
                    case 2:
                        Grid.SetColumn(_PartsTBs[kvp2.Value], 4);
                        break;
                }
            }
        }

        private string GetNowPart(DatePart part)
        {
            switch(part)
            {
                case DatePart.Day:
                    return DateTime.Now.Day.ToString().PadLeft(2, '0');
                case DatePart.Month:
                    return DateTime.Now.Month.ToString().PadLeft(2, '0');
                default:
                    return DateTime.Now.Year.ToString();
            }
        }
        private string GetCurrentText()
        {
            string text = "";
            DatePart current;
            string currentPartText;
            for (int i = 0; i < _PartsOrder.Count; i++)
            {
                current = _PartsOrder[i];
                
                currentPartText = _PartsTBs[current].Text;
                if (string.IsNullOrEmpty(currentPartText))
                    currentPartText = GetNowPart(current);

                text += currentPartText;
                if (_PartsOrder.Count - 1 != i)
                    text += Separator;
            }

            return text;
        }
        private void SetCurrentText(DateTime date)
        {
            //_CleanDate = date;
            _ChangeDate = false;
            Date = date;
            Text = date.ToString(DateTimeStringFormat);

            DayTB.Text = date.Day.ToString().PadLeft(2, '0');
            MonthTB.Text = date.Month.ToString().PadLeft(2, '0');
            YearTB.Text = date.Year.ToString();
        }

        private void DateTimeInputTextBox2_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetPartsWidth();
        }
        private void DateTimeInputTextBox2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_PartsTBs.Values.Contains(Keyboard.FocusedElement))
                return;
            var text = GetCurrentText();
            DateTime test;
            if (DateTime.TryParseExact(text, DateTimeStringFormat, CultureInfo.InvariantCulture, DateTimeStyles.AllowInnerWhite, out test))
                SetCurrentText(test);
            else
                SetCurrentText(Date);
        }
        private void TBs_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(!char.IsNumber(e.Text.ToCharArray()[0]))
                e.Handled = true;
        }
        private void TBs_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                var text = GetCurrentText();
                DateTime test;
                if (DateTime.TryParseExact(text, DateTimeStringFormat, CultureInfo.CurrentCulture, DateTimeStyles.AllowInnerWhite, out test))
                    SetCurrentText(test);
                else
                    SetCurrentText(Date);

                FocusManager.SetFocusedElement(Application.Current.MainWindow, Application.Current.MainWindow as IInputElement);
            }
        }
        private DatePart GetNextPartToWrite(DatePart current)
        {
            var currentIndex = _PartsOrder
                .Where(x => x.Value == current)
                .Select(x => x.Key)
                .SingleOrDefault();
            currentIndex++;
            if (currentIndex > _PartsOrder.Count - 1)
                return _PartsOrder[0];

            return _PartsOrder[currentIndex];
        }
        private void TBs_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null)
                return;
            DatePart part = (DatePart)tb.Tag;
            if (tb.Text.Length == _PartsLength[part])
            {
                var next = GetNextPartToWrite(part);
                if(next == _PartsOrder[0])
                    FocusManager.SetFocusedElement(Application.Current.MainWindow, Application.Current.MainWindow as IInputElement);
                else
                    _PartsTBs[next].Focus();
            }
        }
    }
}
