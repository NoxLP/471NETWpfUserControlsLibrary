using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NET471WpfUserControlsLibrary.RestrictedTextBoxes
{
    public class IntegralInputTextBox : aInputRestrictedTextBox
    {
        public IntegralInputTextBox()
        {
            //_DefaultText = DefaultText; //0.ToString();
            Text = _DefaultText;
            this.PreviewTextInput += IntegralInputTextBox_PreviewTextInput;
            HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;
            VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
        }
        
        protected string _PreviewedText = "";
        private bool _DirectInputChangedByScript;

        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }
        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof(string), typeof(IntegralInputTextBox), new PropertyMetadata("0", DefaultTextChangedStatic));
        private static void DefaultTextChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IntegralInputTextBox).DefaultTextChanged((string)e.NewValue);
        }
        private void DefaultTextChanged(string value)
        {
            _DefaultText = value;
        }

        public int IntegralValue
        {
            get { return (int)GetValue(IntegralValueProperty); }
            set { SetValue(IntegralValueProperty, value); }
        }
        public static readonly DependencyProperty IntegralValueProperty =
            DependencyProperty.Register("IntegralValue", typeof(int), typeof(IntegralInputTextBox), 
                new PropertyMetadata(0, new PropertyChangedCallback(IntegralValueChanged)));
        private static void IntegralValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IntegralInputTextBox).DirectIntegralInput((int)e.NewValue);
        }
        private void DirectIntegralInput(int value)
        {
            if(_DirectInputChangedByScript)
            {
                _DirectInputChangedByScript = false;
                return;
            }
            Text = value.ToString();
        }

        private void IntegralInputTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            _PreviewedText = e.Text;
        }
        protected override void aInputRestrictedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (GetIfUserChangedEvent())
                return;
            if (GetIfTextIsNullOrEmptyAndSetTextToDefault(ref e, text => string.IsNullOrEmpty(text)))// || text == "0"))
                return;

            int test;

            if (int.TryParse(Text, NumberStyles.Any, CultureInfo.InvariantCulture, out test))
            {
                _FormattedString = test.ToString("N0");
                _CleanString = test.ToString();
                _DirectInputChangedByScript = true;
                IntegralValue = test;
            }
            else
            {
                IncorrectInput(ref e);
            }
        }
        protected override void IncorrectInput(ref TextChangedEventArgs e)
        {
            _DoChangedEvent = false;
            char prev = _PreviewedText.ToCharArray()[0];
            Text = char.IsNumber(prev) ? _CleanString + prev : _CleanString;
            SelectionStart = Text.Length;
            e.Handled = true;
        }
    }
}
