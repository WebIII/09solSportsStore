using System;
using System.Text.RegularExpressions;

namespace SportsStore.Models.Domain {
    public class City {
        #region Fields
        private string _postalCode;
        private string _name;
        #endregion

        #region Properties
        public string Postalcode {
            get => _postalCode;
            set {
                Regex postcodeRegex = new Regex(@"^\d{4}$");
                if (value == null || !postcodeRegex.IsMatch(value))
                    throw new ArgumentException("Postal code must contain exactly 4 digits");
                _postalCode = value;
            }
        }

        public string Name {
            get => _name;
            set {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
                    throw new ArgumentException("City must have a name, and the name should not exceed 100 characters");
                _name = value;
            }
        }
        #endregion

        #region Contstructors
        public City(string postalcode, string name) {
            Postalcode = postalcode;
            Name = name;
        }
        #endregion
    }
}