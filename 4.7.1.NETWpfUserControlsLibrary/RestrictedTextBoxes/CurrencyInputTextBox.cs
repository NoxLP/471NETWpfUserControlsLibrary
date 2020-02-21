using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NET471WpfUserControlsLibrary.RestrictedTextBoxes
{
    public class CurrencyInputTextBox : aInputRestrictedTextBox
    {
        public CurrencyInputTextBox()
        {
            //_DefaultText = DefaultText; //0.ToString("C2", CultureInfo.CurrentCulture);
            Text = _DefaultText;
            HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;
            VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
        }

        private bool _DirectInputChangedByScript;

        #region dependency properties
        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }
        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof(string), typeof(CurrencyInputTextBox), new PropertyMetadata("0", DefaultTextChangedStatic));
        private static void DefaultTextChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CurrencyInputTextBox).DefaultTextChanged((string)e.NewValue);
        }
        private void DefaultTextChanged(string value)
        {
            _DefaultText = value;
        }

        public decimal DecimalValue
        {
            get { return (decimal)GetValue(DecimalValueProperty); }
            set { SetValue(DecimalValueProperty, value); }
        }
        public static readonly DependencyProperty DecimalValueProperty =
            DependencyProperty.Register("DecimalValue", typeof(decimal), typeof(CurrencyInputTextBox), new PropertyMetadata(0M, DecimalValueChanged));
        private static void DecimalValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CurrencyInputTextBox).DirectDecimalInput((decimal)e.NewValue);
        }
        private void DirectDecimalInput(decimal value)
        {
            if (_DirectInputChangedByScript)
            {
                _DirectInputChangedByScript = false;
                return;
            }

            Text = value.ToString();
        }
        #endregion

        protected override void aInputRestrictedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (GetIfUserChangedEvent())
                return;
            if (GetIfTextIsNullOrEmptyAndSetTextToDefault(ref e, text => string.IsNullOrEmpty(text)))// || text.Equals(_DefaultText)))
                return;

            decimal test;

            if (decimal.TryParse(Text, NumberStyles.Any, CultureInfo.InvariantCulture, out test))
            {
                _FormattedString = test.ToString("C2", CultureInfo.CurrentCulture);
                _CleanString = test.ToString(CultureInfo.InvariantCulture);
                _DirectInputChangedByScript = true;
                DecimalValue = test;
            }
            else
            {
                IncorrectInput(ref e);
            }
        }
    }
}
