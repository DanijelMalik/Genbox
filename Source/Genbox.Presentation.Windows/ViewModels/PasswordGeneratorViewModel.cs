using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generator4Developers.Presentation.Windows.Properties;
using Generator4Developers.Presentation.Windows.Services;

namespace Generator4Developers.Presentation.Windows.ViewModels
{
    [GeneratorService]
    public class PasswordGeneratorViewModel : ServiceViewModel
    {
        public PasswordGeneratorViewModel()
        {
            InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            PasswordLength = 15;
            IsUpperCaseLetters = true;
            IsLowerCaseLetters = true;
            IsNumbers = true;
            IsSpecialCharacters = true;
            IsExcludeSimilarCharacters = true;
            SpecialCharacters = "~!@#$%^&*";
        }

        private string _specialCharacters;
        private bool _isUpperCaseLetters;
        private bool _isLowerCaseLetters;
        private bool _isNumbers;
        private bool _isSpecialCharacters;
        private int _passwordLength;
        private double _passwordStrength;
        private bool _isExcludeSimilarCharacters;

        public override string DisplayName
        {
            get { return Resources.ServiceName_PasswordGenerator; }
        }

        public override string Generate()
        {
            if (!IsUpperCaseLetters && !IsLowerCaseLetters && !IsNumbers && !IsSpecialCharacters)
            {
                throw new InvalidOperationException("Impossible to generate a password!");
            }

            PasswordStrength = CalculatePasswordStrength();
            var characters = new List<char>();
            var builder = new StringBuilder();
            var random = new Random();
            var upperCaseLetters = Enumerable.Range((byte)'A', (byte)'Z' - (byte)'A').Select(x => (char)x).ToList();
            var lowerCaseLetters = Enumerable.Range((byte)'a', (byte)'z' - (byte)'a').Select(x => (char)x).ToList();
            var numbers = Enumerable.Range((byte)'0', (byte)'9' - (byte)'0').Select(x => (char)x).ToList();
            var specialCharacters = SpecialCharacters.ToCharArray();

            if (IsUpperCaseLetters)
            {
                characters.AddRange(upperCaseLetters);
            }

            if (IsLowerCaseLetters)
            {
                characters.AddRange(lowerCaseLetters);
            }

            if (IsNumbers)
            {
                characters.AddRange(numbers);
            }

            if (IsSpecialCharacters)
            {
                characters.AddRange(specialCharacters);
            }

            if (!characters.Any())
            {
                return string.Empty;
            }

            for (var i = 0; i < PasswordLength; i++)
            {
                if (characters.Count == 0)
                {
                    throw new InvalidOperationException("Impossible to generate a password!");
                }

                var nextIndex = (random.Next(1, characters.Count) * 13) % characters.Count;
                var nextChar = characters[nextIndex];
                builder.Append(nextChar);

                if (IsExcludeSimilarCharacters)
                {
                    characters.RemoveAt(nextIndex);
                }
            }

            var tempPassword = builder.ToString();

            if (IsUpperCaseLetters)
            {
                tempPassword = UpdatePassword(tempPassword, upperCaseLetters);
            }

            if (IsLowerCaseLetters)
            {
                tempPassword = UpdatePassword(tempPassword, lowerCaseLetters);
            }

            if (IsNumbers)
            {
                tempPassword = UpdatePassword(tempPassword, numbers);
            }

            if (IsSpecialCharacters)
            {
                tempPassword = UpdatePassword(tempPassword, specialCharacters);
            }

            return tempPassword;
        }

        private static string UpdatePassword(string password, ICollection<char> characters)
        {
            var rnd = new Random();
            var builder = new StringBuilder(password);

            if (!password.Any(characters.Contains))
            {
                var index = rnd.Next(0, password.Length);
                var character = characters.ElementAt(rnd.Next(0, characters.Count));

                builder[index] = character;
                return builder.ToString();
            }

            return password;
        }

        private double CalculatePasswordStrength()
        {
            const double upperCaseFactor = 0.15d;
            const double lowerCaseFactor = 0.15d;
            const double numbersFactor = 0.2d;
            const double specialCharactersFactor = 0.25d;
            var factor = 0d;

            if (IsUpperCaseLetters)
            {
                factor += upperCaseFactor;
            }

            if (IsLowerCaseLetters)
            {
                factor += lowerCaseFactor;
            }

            if (IsNumbers)
            {
                factor += numbersFactor;
            }

            if (IsSpecialCharacters)
            {
                factor += specialCharactersFactor;
            }

            return PasswordLength * 0.1d * factor;
        }

        public int PasswordLength
        {
            get { return _passwordLength; }
            set
            {
                _passwordLength = value;
                OnPropertyChanged();
            }
        }

        public double PasswordStrength
        {
            get { return _passwordStrength; }
            private set
            {
                _passwordStrength = value;
                OnPropertyChanged();
                OnPropertyChanged("PasswordStrengthText");
            }
        }

        public string PasswordStrengthText
        {
            get { return GetPasswordStrengthText(); }
        }

        public string SpecialCharacters
        {
            get { return _specialCharacters; }
            set
            {
                _specialCharacters = value;
                OnPropertyChanged();
            }
        }

        public bool IsUpperCaseLetters
        {
            get { return _isUpperCaseLetters; }
            set
            {
                _isUpperCaseLetters = value;
                OnPropertyChanged();
            }
        }

        public bool IsLowerCaseLetters
        {
            get { return _isLowerCaseLetters; }
            set
            {
                _isLowerCaseLetters = value;
                OnPropertyChanged();
            }
        }

        public bool IsNumbers
        {
            get { return _isNumbers; }
            set
            {
                _isNumbers = value;
                OnPropertyChanged();
            }
        }

        public bool IsSpecialCharacters
        {
            get { return _isSpecialCharacters; }
            set
            {
                _isSpecialCharacters = value;
                OnPropertyChanged();
            }
        }

        public bool IsExcludeSimilarCharacters
        {
            get { return _isExcludeSimilarCharacters; }
            set
            {
                _isExcludeSimilarCharacters = value;
                OnPropertyChanged();
            }
        }

        private string GetPasswordStrengthText()
        {
            var values = new[] { "Poor", "Low", "Medium", "High", "Very High" };
            var index = (int)Math.Min(values.Length - 1, PasswordStrength / 0.2);

            return String.Format("{0} ({1:P0})", values[index], Math.Min(PasswordStrength, 1));
        }

        public override string Generate(int mode, string pattern)
        {
            return Generate();
        }
    }
}