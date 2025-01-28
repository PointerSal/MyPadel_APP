using Microsoft.Maui.ApplicationModel.Communication;
using MyPadelApp.Resources.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyPadelApp.Helpers
{
    public class FieldValidations
    {
        public static (bool, string) IsEmailValid(string email)
        {
            try
            {
                var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (string.IsNullOrWhiteSpace(email))
                    return (true, AppResources.EmailRequired);

                if (!Regex.IsMatch(email, emailRegex))
                    return (true, AppResources.EmailInvalid);
                else
                    return (false, string.Empty);
            }
            catch
            {
                return (true, AppResources.EmailRequired);
            }
        }
        public static (bool, string) IsFieldNotEmpty(string Text,string ErrorMessage)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Text))
                    return (true, ErrorMessage);
                else
                    return (false, string.Empty);
            }
            catch
            {
                return (true, ErrorMessage);
            }
        }

        public static (bool, string) IsPasswordValid(string password)
        {
            try
            {
                var strongPasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$";

                if (string.IsNullOrWhiteSpace(password))
                    return (true, AppResources.PasswordRequired);

                if (!Regex.IsMatch(password, strongPasswordRegex))
                    return (true, AppResources.PasswordStrengthRequired);

                return (false, string.Empty);
            }
            catch
            {
                return (true, AppResources.PasswordRequired);
            }
        }
        public static (bool, string) ArePasswordsMatching(string password, string confirmPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(confirmPassword))
                    return (true, AppResources.ConfirmPasswordRequired);

                if (!password.Equals(confirmPassword))
                    return (true, AppResources.PasswordsDoNotMatch);

                return (false, string.Empty);
            }
            catch
            {
                return (true, AppResources.ConfirmPasswordRequired);
            }
        }

        public static (bool, string) ValidateCheckBoxes(bool isTermsAccepted, bool isPrivacyAccepted, bool isMarketingAccepted)
        {
            if (!isTermsAccepted || !isPrivacyAccepted || !isMarketingAccepted)
                return (true, AppResources.AcceptConditions);

            return (false, string.Empty);
        }
        public static (bool, string) IsItalianPhoneNumberValid(string phoneNumber)
        {
            try
            {
                phoneNumber = phoneNumber.Replace(" ", "").Replace("-", "");

                var phoneRegex = @"^(\+39|0039)?[0-9]{2,4}[0-9]{6,8}$|^(\+39|0039)?3[0-9]{8,9}$";

                if (string.IsNullOrWhiteSpace(phoneNumber))
                    return (true, AppResources.PhoneNumberRequired);

                if (!Regex.IsMatch(phoneNumber, phoneRegex))
                    return (true, AppResources.InvalidNumber);

                return (false, string.Empty); 
            }
            catch (Exception)
            {
            }
            return (true, AppResources.InvalidNumber);
        }
    }
}
