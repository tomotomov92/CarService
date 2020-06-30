using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    public static class Constants
    {
        public const string SessionKeyUserId = "_Id";
        public const string SessionKeyUserName = "_UserName";
        public const string SessionKeyUserRole = "_UserRole";

        public const string DefaultPassword = "Qwerty1!";
        public const string EmailNotExist = "Wrong email address or password!";
        public const string WrongPassword = "Wrong email address or password!";
        public const string UserAlreadyExists = "You are already registered. If you have forgotten your password use the link from the Sign In page!";
        public const string RegistrationPasswordsDoNotMatch = "The two passwords do not match!";
        public const string ErrorDuringTheRegistrationTryAgain = "There was an issue with your registration. Please try again!";
    }
}
