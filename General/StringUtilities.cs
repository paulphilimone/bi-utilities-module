using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;

namespace mz.betainteractive.utilities.module.General {
    public class StringUtilities {
        public static string UnformatCurrency(string text) {
            string unformatted = ""+text;

            string symbol = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol;
            unformatted = unformatted.Replace(symbol, "").Trim();

            return unformatted;
        }

        public static double? GetUnformattedCurrency(string text) {
            string unformatted = UnformatCurrency(text);
            double value = 0;

            bool result = Double.TryParse(unformatted, out value);

            if (result == false) {
                return null;
            }

            return value;
        }

        public static string UnformatCurrency(string text, CultureInfo culture) {
            string unformatted = "" + text;

            string symbol = culture.NumberFormat.CurrencySymbol;
            unformatted = unformatted.Replace(symbol, "").Trim();

            return unformatted;
        }

        public static double? GetUnformattedCurrency(string text, CultureInfo culture) {
            string unformatted = UnformatCurrency(text, culture);
            double value = 0;

            bool result = Double.TryParse(unformatted, out value);

            if (result == false) {
                return null;
            }

            return value;
        }

        public static bool ValidateIPAddress(string ip) {
            string regxIp = "^[0-9]+\\.[0-9]+\\.[0-9]+\\.[0-9]+$";
            return Validate(regxIp, ip);
        }

        public static bool ValidateInteger(string number) {            
            string regxNum = "^[0-9]+$";
            return Validate(regxNum, number);
        }

        public static bool ValidateEmail(string email) {
            string regxEmail = "^.+@.+\\..+$";
            return Validate(regxEmail, email);
        }
        // +258 84 1234567 \\+[0-9]+
        // +974 33 411753
        // +21 470079
        public static bool Validate(string regularExpression, string text) {
            try {
                return Regex.IsMatch(text, regularExpression);
            } catch (Exception ex) {
                LogErrors.AddErrorLog(ex, "Validate Regular Expression ["+regularExpression+" <-> "+text+"]");
                return false;
            }
        }

        public static string Capitalize(string text) {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text);
        }

        public static string RemoveAccents(string input) {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            input = Regex.Replace(input, @"[^\p{L}]", string.Empty);

            var normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized) {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark) {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string GetEnumDescription(Enum value) {
            var field = value.GetType().GetField(value.ToString());
            var attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attr != null ? attr.Description : value.ToString();
        }

        public static string ShortenName(string name) {
            if (string.IsNullOrWhiteSpace(name))
                return name;

            // Words that add no meaning
            string[] ignoreWords = { "de", "da", "do", "das", "dos", "para", "por", "a", "ao", "à" };

            var parts = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(w => !ignoreWords.Contains(w.ToLower()))
                .ToList();

            if (parts.Count == 0)
                return name;

            // Strategy:
            // - Shorten each word to 4–6 characters
            // - For long words use the first syllable-like section
            List<string> shortened = new List<string>();

            foreach (string word in parts) {
                string w = word;

                if (w.Length > 6) {
                    // "Transporte" → "Transp"
                    // "Alimentação" → "Alim"
                    // "Extraordinárias" → "Extra"
                    w = w.Substring(0, 4);

                    // If 5th char is a consonant, keep it
                    if (word.Length > 5 && !IsVowel(word[4])) {
                        w += word[4];
                    }
                } else if (w.Length > 4) {
                    // Medium words "Subsidio" → "Subs"
                    w = w.Substring(0, 4);
                }

                shortened.Add(w);
            }

            // Join with dot for readability: Subs.Transp
            return string.Join(".", shortened);
        }

        private static bool IsVowel(char c) {
            return "aeiouáéíóúâêôàãõ".Contains(char.ToLower(c));
        }

        public static bool IsDecimalNumber(string text) {
            return decimal.TryParse(text, out decimal value);
        }
    }
}
