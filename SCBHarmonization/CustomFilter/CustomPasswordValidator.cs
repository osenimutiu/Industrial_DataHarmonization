using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace SCBHarmonization.CustomFilter
{
    public class CustomPasswordValidator : IIdentityValidator<string>
    {
        public int RequiredLength { get; set; }
        public CustomPasswordValidator(int length)
        {
            RequiredLength = length;
        }
        public Task<IdentityResult> ValidateAsync(string password)
        {
            if (String.IsNullOrEmpty(password) || password.Length < RequiredLength)
            {
                return Task.FromResult(IdentityResult.Failed(
                    $"Password should be at least {RequiredLength} characters"));
            }

            int counter = 0;
            List<string> patterns = new List<string>();
            patterns.Add(@"[a-z]");                                          // lowercase
            patterns.Add(@"[A-Z]");                                          // uppercase
            patterns.Add(@"[0-9]");                                          // digits
                                                                             // don't forget to include white space in special symbols
            patterns.Add(@"[!@#$%^&*\(\)_\+\-\={}<>,\.\|""'~`:;\\?\/\[\] ]"); // special symbols

            // count type of different chars in password
            foreach (string p in patterns)
            {
                if (Regex.IsMatch(password, p))
                {
                    counter++;
                }
            }

            if (counter < 2)
            {
                return Task.FromResult(IdentityResult.Failed(
                    "Please use characters from at least two of these groups: lowercase, uppercase, digits, special symbols"));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
    public class CustomPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string password = (string)value;
            //bool passwordstrong = Code to check password is strong or not (the code is depend on your requirement)
            bool passwordstrong = true;
            if (!passwordstrong)
            {
                return new ValidationResult("Password is not valid!");
            }

            return ValidationResult.Success;
        }
    }

    public class ApplicationHelper
    {
        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}