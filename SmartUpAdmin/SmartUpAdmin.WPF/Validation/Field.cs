using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace SmartUpAdmin.Core.NewFolder
{
    public class Field
    {
        private TextBox TextField;
        private string PreviousText;
        private int MinLength;
        private int MaxLength;
        private Regex BannedCharacters;
        private List<string> ErrorMessages = new List<string>();
        private Dictionary<Func<bool>, string> ErrorChecks = new Dictionary<Func<bool>, string>();

        public Field(TextBox textField, int minLength, int maxLength, Regex bannedCharacters)
        {
            TextField = textField;
            MinLength = minLength;
            MaxLength = maxLength;
            BannedCharacters = bannedCharacters;
            AddPreviewMouseDown();
        }

        public bool Validate()
        {
            bool isValid = true;
            PreviousText = TextField.Text;
            if (string.IsNullOrWhiteSpace(TextField.Text) || string.IsNullOrEmpty(TextField.Text))
            {
                isValid = AddErrorMessage("Het veld mag niet leeg zijn.");
            }
            else
            {
                isValid &= ValidateMinLength();
                isValid &= ValidateMaxLength();
                isValid &= ValidateBannedCharacters();
                isValid &= ValidateErrorChecks();
            }
            return isValid;
        }

        public bool ValidateMinLength()
        {
            bool isValid = true;
            if (PreviousText.Length < MinLength)
            {
                isValid = AddErrorMessage($"Het veld moet minstens {MinLength} karakter(s) bevatten.");
            }
            return isValid;
        }

        public bool ValidateMaxLength()
        {
            bool isValid = true;
            if (PreviousText.Length > MaxLength)
            {
                isValid = AddErrorMessage($"Het veld mag maximaal {MaxLength} karakter(s) bevatten.");
            }
            return isValid;
        }

        public bool ValidateBannedCharacters()
        {
            bool isValid = true;
            if (BannedCharacters != null && BannedCharacters.IsMatch(PreviousText))
            {
                isValid = AddErrorMessage("Het veld bevat ongeldige karakters.");
            }
            return isValid;
        }

        public bool ValidateErrorChecks()
        {
            bool isValid = true;
            foreach (KeyValuePair<Func<bool>, string> errorCheck in ErrorChecks)
            {
                if (errorCheck.Key.Invoke())
                {
                    ErrorMessages.Add(errorCheck.Value);
                    TextField.BorderBrush = System.Windows.Media.Brushes.Red;
                    TextField.Text = errorCheck.Value;
                    isValid = false;
                }
            }
            return isValid;
        }

        public void AddErrorCheck(Func<bool> errorCheck, string errorMessage)
        {
            ErrorChecks.Add(errorCheck, errorMessage);
        }

        private bool AddErrorMessage(string errorMessage)
        {
            ErrorMessages.Add(errorMessage);
            TextField.BorderBrush = System.Windows.Media.Brushes.Red;
            TextField.Text = errorMessage;
            return false;
        }

        private void AddPreviewMouseDown()
        {
            TextField.PreviewMouseDown += (sender, e) =>
            {
                if (ErrorMessages.Contains(TextField.Text))
                {
                    TextField.Text = "";
                    TextField.BorderBrush = System.Windows.Media.Brushes.Black;
                }
            };
        }

        public string GetText()
        {
            return TextField.Text;
        }

    }
}
