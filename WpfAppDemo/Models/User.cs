using SqlSugar;
using System.ComponentModel;
using System;

namespace WpfAppDemo.Models
{
    [SugarTable("Users")]
    public class User : IDataErrorInfo
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";

        [SugarColumn(IsIgnore = true)]
        public string Error => null;

        [SugarColumn(IsIgnore = true)]
        public string this[string columnName]
        {
            get
            {
                string result = null;
                switch (columnName)
                {
                    case nameof(Username):
                        if (string.IsNullOrWhiteSpace(Username))
                            result = "Username is required.";
                        else if (Username.Length < 3)
                            result = "Username must be at least 3 characters.";
                        break;
                    case nameof(Email):
                        if (!string.IsNullOrWhiteSpace(Email) && !Email.Contains("@"))
                            result = "Invalid email format.";
                        break;
                    case nameof(Password):
                        if (string.IsNullOrWhiteSpace(Password))
                            result = "Password is required.";
                        break;
                }
                return result;
            }
        }
    }
}
