using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningApi.Helpers
{
    public static class ErrorContent
    {
        public static class UserContext
        {
            public static class FirstName
            {
                public const string EmptyCode = "ERR_FN_EMPTY";
                public const string EmptyMessage = "Firstname cannot be empty";

                public const string MinLengthCode = "ERR_FN_LNG";
                public const string MinLengthMessage = "Firstname must have minimum of 2 letters";
            }

            public static class LastName
            {
                public const string EmptyCode = "Lastname cannot be empty";
                public const string EmptyMessage = "Lastname must have minimum of 2 letters";

                public const string MinLengthCode = "ERR_LN_EMPTY";
                public const string MinLengthMessage = "ERR_LN_LNG";
            }

            public static class UserName
            {
                public const string EmptyCode = "ERR_UN_EMPTY";
                public const string EmptyMessage = "Usertname cannot be empty";

                public const string MinLengthCode = "ERR_UN_LNG";
                public const string MinLengthMessage = "Username must have minimum of 2 letters";

                public const string ExistCode = "ERR_UN_EXST";
                public const string ExistMessage = "Username already exist";
            }

            public static class Email
            {
                public const string EmptyCode = "ERR_EMAIL_EMPTY";
                public const string EmptyMessage = "Emailname cannot be empty";

                public const string ValidAddressCode = "ERR_EMAIL_ADR";
                public const string ValidAddressMessage = "Email is not valid";
            }

            public static class Sex
            {
                public const string MustCode = "ERR_SEX";
                public const string MustMessage = "Sex value is not valid";
            }

            public static class Password
            {
                public const string EmptyCode = "ERR_PSD_EMPTY";
                public const string EmptyMessage = "Password cannot be empty";

                public const string LengthCode = "ERR_PSD_LNG";
                public const string LengthMessage = "Password must be between 6 and 12 characters";
            }

            public static class ConfirmPassword
            {
                public const string EqualPasswordCode = "ERR_PSD_CFRM_EQUL";
                public const string EqualPasswordMessage = "Confirm Password must be Equal to Password";
            }

            public static class Code
            {
                public const string EmptyTokenCode = "ERR_TKN_EMPTY";
                public const string EmptyTokenMessage = "Token Id cannot be empty";
            }

            public static class UserId
            {
                public const string EmptyUserIdCode = "ERR_UID_EMPTY";
                public const string EmptyUserIdMessage = "User Id cannot be empty";
            }

            public static class User
            {
                public const string UserNotFoundCode = "ERR_USER_NF";
                public const string UserNotFoundMessage = "user not found";
            }

            public static class EmailNotConfirmed
            {
                public const string EmailNotConfirmedCode = "ERR_EMAIL_NC";
                public const string EmailNotConfirmedMessage = "not allowed to login. Confirm email";
            }


            public static class DateOfBirth
            {
                public const string EmptyCode = "ERR_DOB_EMPTY";
                public const string EmptyMessage = "Date of Birth cannot be empty";

                public const string RegexCode = "ERR_DOB_RGX";
                public const string RegexMessage = "Date of Birth is not valid: the format dd-mm-yyyy";
            }


            public static class Sponsor
            {
                public const string EmptyCode = "ERR_SPN_NF";
                public const string EmptyMessage = "Sponsor not found";
            }

        }
        public static class EmailSenderContext
        {
            public static class Sender
            {
                public const string SendCode = "ERR_Snd_Email";
                public const string SendMessage = "An error occured while sending a mail";
            }
        }

        
    }
}
