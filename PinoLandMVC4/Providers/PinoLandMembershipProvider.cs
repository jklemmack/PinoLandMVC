using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;

using MG = Fuqua.CompetativeAnalysis.MarketGame;

namespace PinoLandMVC4.Providers
{
    public class PinoLandMembershipProvider : MembershipProvider
    {
        private static string APPLICATION_NAME = "PinoLand";
        private static int MIN_PASSWORD_LENGTH = 8;
        //private static string PROVIDER_NAME = "PinoLandMembershipProvider";

        private string connectionString = null;

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            string connectionStringName = config["connectionStringName"];
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            base.Initialize(name, config);
        }

        public override string ApplicationName
        {
            get
            {
                return APPLICATION_NAME;
            }
            set
            {
                throw new ArgumentException("ApplicationName is read only.");
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            using (MG.GameDataObjectContext context = new MG.GameDataObjectContext(connectionString))
            {
                MG.IM_User user = context.IM_User.FirstOrDefault(u => String.Compare(u.UserName, username, true) == 0);
                if (user != null)
                {
                    status = MembershipCreateStatus.DuplicateUserName;
                    return null;
                }
                user = new MG.IM_User() { UserName = username, Email = email, Active = true, Password = password };
                context.IM_User.AddObject(user);
                context.SaveChanges();
                status = MembershipCreateStatus.Success;

                return new MembershipUser(this.Name, username, user.UserId, email, null, null, true, false
                    , DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow, DateTime.MinValue, DateTime.MinValue);
            }

        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return true; }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { return MIN_PASSWORD_LENGTH; }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            using (MG.GameDataObjectContext context = new MG.GameDataObjectContext(connectionString))
            {
                MG.IM_User user = context.IM_User.FirstOrDefault(
                    u => String.Compare(u.UserName, username, true) == 0
                    && string.Compare(u.Password, password, false) == 0);

                if (user != null)
                    return true;
                return false;
            }
        }
    }
}