using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NET471WpfUserControlsLibrary.RestrictedTextBoxes
{
    public class LettersInputTextBox : TextBox
    {
        public LettersInputTextBox()
        {
            Text = "";
            this.PreviewTextInput += LettersInputTextBox_PreviewTextInput;
        }
        
        private void LettersInputTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Text) && !char.IsLetter(e.Text[0]))
                e.Handled = true;
        }
    }
}
