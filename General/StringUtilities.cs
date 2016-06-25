using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;

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
    }
}
