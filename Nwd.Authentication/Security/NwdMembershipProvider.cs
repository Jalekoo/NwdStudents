using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;
using Nwd.Authentication.Model;

namespace Nwd.Authentication.Security
{
    public class NwdMembershipProvider : MembershipProvider
    {
        bool _enablePasswordReset;
        bool _enablePasswordRetrieval;
        int _maxInvalidPasswordAttempts;
        private MachineKeySection _machineKey;
        int _minRequiredNonAlphanumericCharacters;
        int _minRequiredPasswordLength;
        int _passwordAttemptWindow;
        MembershipPasswordFormat _passwordFormat;
        string _passwordStrengthRegularExpression;
        bool _requiresQuestionAndAnswer;
        bool _requiresUniqueEmail;
        string _applicationName = "NwdAuth";

        public override void Initialize( string name, NameValueCollection config )
        {
            base.Initialize( name, config );

            _maxInvalidPasswordAttempts = Convert.ToInt32( ProviderUtils.GetConfigValue( config, "maxInvalidPasswordAttempts", "5" ) );
            _passwordAttemptWindow = Convert.ToInt32( ProviderUtils.GetConfigValue( config, "passwordAttemptWindow", "10" ) );
            _minRequiredNonAlphanumericCharacters = Convert.ToInt32( ProviderUtils.GetConfigValue( config, "minRequiredNonAlphanumericCharacters", "1" ) );
            _minRequiredPasswordLength = Convert.ToInt32( ProviderUtils.GetConfigValue( config, "minRequiredPasswordLength", "7" ) );
            _passwordStrengthRegularExpression = Convert.ToString( ProviderUtils.GetConfigValue( config, "passwordStrengthRegularExpression", string.Empty ) );
            _enablePasswordReset = Convert.ToBoolean( ProviderUtils.GetConfigValue( config, "enablePasswordReset", "true" ) );
            _enablePasswordRetrieval = Convert.ToBoolean( ProviderUtils.GetConfigValue( config, "enablePasswordRetrieval", "false" ) );
            _requiresQuestionAndAnswer = Convert.ToBoolean( ProviderUtils.GetConfigValue( config, "requiresQuestionAndAnswer", "false" ) );
            _requiresUniqueEmail = Convert.ToBoolean( ProviderUtils.GetConfigValue( config, "requiresUniqueEmail", "false" ) );

            switch( config["passwordFormat"] ?? "Hashed" )
            {

                case "Hashed":
                    _passwordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    _passwordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    _passwordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException( "Password format is invalid" );
            }

            if( !string.IsNullOrEmpty( _passwordStrengthRegularExpression ) )
            {
                _passwordStrengthRegularExpression = _passwordStrengthRegularExpression.Trim();
                if( !string.IsNullOrEmpty( _passwordStrengthRegularExpression ) )
                {
                    try
                    {
                        new Regex( _passwordStrengthRegularExpression );
                    }
                    catch( ArgumentException ex )
                    {
                        throw new ProviderException( ex.Message, ex );
                    }
                }

                if( _minRequiredPasswordLength < _minRequiredNonAlphanumericCharacters )
                {
                    throw new ProviderException( "Min required password length is invalid." );
                }
            }

            Configuration configuration = WebConfigurationManager.OpenWebConfiguration( HostingEnvironment.ApplicationVirtualPath );
            _machineKey = (MachineKeySection)configuration.GetSection( "system.web/machineKey" );
        }

        public override string ApplicationName { get { return _applicationName; } set { _applicationName = value; } }

        public override bool ChangePassword( string username, string oldPassword, string newPassword )
        {
            using( var c = new NwdAuthContext() )
            {
                User u = GetUser( username, c );

                if( u == null ) return false;
                if( u.Password != EncodePassword( oldPassword ) ) return false;

                u.Password = EncodePassword( newPassword );
                c.Users.Attach( u );
                c.Entry( u ).Property( m => m.Password ).IsModified = true;
                c.SaveChanges();

                return true;
            }
        }

        public override bool ChangePasswordQuestionAndAnswer( string username, string password, string newPasswordQuestion, string newPasswordAnswer )
        {
            throw new NotSupportedException();
        }

        public override MembershipUser CreateUser( string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status )
        {
            if( username == null || username.Length > 0 ) { status = MembershipCreateStatus.InvalidEmail; return null; }
            if( password == null || password.Length > 0 ) { status = MembershipCreateStatus.InvalidPassword; return null; }

            using( var c = new NwdAuthContext() )
            {
                if( GetUser( username, c ) != null ) { status = MembershipCreateStatus.DuplicateUserName; return null; }
                User u = new User { Username = username, Password = password, Email = email };
                c.Users.Add( u );
                c.SaveChanges();
                status = MembershipCreateStatus.Success;
                return GetUser( username, false );
            }
        }

        public override bool DeleteUser( string username, bool deleteAllRelatedData )
        {
            using( var c = new NwdAuthContext() )
            {
                MembershipUser u = GetUser( username, false );
                if( u == null ) return false;

                return true;
            }
        }

        public override bool EnablePasswordReset
        {
            get { return _enablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return _enablePasswordRetrieval; }
        }

        public override MembershipUserCollection FindUsersByEmail( string emailToMatch, int pageIndex, int pageSize, out int totalRecords )
        {
            using( var c = new NwdAuthContext() )
            {
                IQueryable<User> u = c.Users.Where( m => m.Email == emailToMatch );
                totalRecords = u.Count();
                //return u.Skip( pageIndex * pageSize ).Take( pageSize );
                throw new NotImplementedException();
            }
        }

        public override MembershipUserCollection FindUsersByName( string usernameToMatch, int pageIndex, int pageSize, out int totalRecords )
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection GetAllUsers( int pageIndex, int pageSize, out int totalRecords )
        {
            MembershipUserCollection muc = new MembershipUserCollection();
            using( var c = new NwdAuthContext() )
            {
                IQueryable<User> listUser = c.Users;
                totalRecords = listUser.Count();
                foreach( User m in listUser.Skip( pageIndex * pageSize ).Take( pageSize ) )
                    muc.Add( GetMembershipUserFromUser( m ) );

                return muc;
            }
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotSupportedException();
        }

        public override string GetPassword( string username, string answer )
        {
            using (var c = new NwdAuthContext())
            {
                return c.Users.Where(m => m.Username == username).Select( m => m.Password).FirstOrDefault();
            }
        }

        public override MembershipUser GetUser( string username, bool isOnlineUser )
        {
            using( var c = new NwdAuthContext() )
            {
                User u = GetUser( username, c );
                if( u == null ) return null;
                return GetMembershipUserFromUser( u );
            }
        }

        public override MembershipUser GetUser( object providerUserKey, bool isOnlineUser )
        {
            using( var c = new NwdAuthContext() )
            {
                User u = c.Users.Where( m => m.Id == (int)providerUserKey ).FirstOrDefault();
                if( u == null ) return null;
                return GetMembershipUserFromUser( u );
            }
        }

        public User GetUser( string username, NwdAuthContext context )
        {
            return context.Users.Where( m => m.Username == username ).FirstOrDefault();
        }

        public override string GetUserNameByEmail( string email )
        {
            throw new NotSupportedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return _maxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return _minRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { return _passwordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return _passwordFormat; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return _passwordStrengthRegularExpression; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return _requiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return _requiresUniqueEmail; }
        }

        public override string ResetPassword( string username, string answer )
        {
            throw new NotSupportedException();
        }

        public override bool UnlockUser( string userName )
        {
            throw new NotSupportedException();
        }

        public override void UpdateUser( MembershipUser user )
        {
            using( var c = new NwdAuthContext() )
            {
                User u = c.Users.Where( m => m.Username == user.UserName ).FirstOrDefault();
                if( u == null ) return;

                c.Users.Attach( u );

                if( user.Email != u.Email )
                {
                    u.Email = user.Email;
                    c.Entry( u ).Property( m => m.Email ).IsModified = true;
                }

                if( user.ProviderName != u.Name )
                {
                    u.Name = user.ProviderName;
                    c.Entry( u ).Property( m => m.Email ).IsModified = true;
                }

                c.SaveChanges();
            }
        }

        public override bool ValidateUser( string username, string password )
        {
            using( var c = new NwdAuthContext() )
            {
                var user = GetUser( username, false );
                if( user == null ) return false;

                if( CheckPassword( password, user.GetPassword() ) ) return true;

                return false;
            }
        }

        private bool CheckPassword( string password, string dbpassword )
        {
            string pass1 = password;
            string pass2 = dbpassword;

            switch( PasswordFormat )
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword( dbpassword );
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword( password );
                    break;
                default:
                    break;
            }

            return pass1 == pass2;
        }

        public string EncodePassword( string password )
        {
            string encodedPassword = password;

            switch( PasswordFormat )
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword = Convert.ToBase64String( EncryptPassword( Encoding.Unicode.GetBytes( password ) ) );
                    break;
                case MembershipPasswordFormat.Hashed:
                    HMACSHA1 hash = new HMACSHA1 { Key = HexToByte( _machineKey.ValidationKey ) };
                    encodedPassword = Convert.ToBase64String( hash.ComputeHash( Encoding.Unicode.GetBytes( password ) ) );
                    break;
                default:
                    throw new ProviderException( "Unsupported password format" );
            }

            return encodedPassword;
        }

        private string UnEncodePassword( string encodedPassword )
        {
            string password = encodedPassword;

            switch( PasswordFormat )
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password = Encoding.Unicode.GetString( DecryptPassword( Convert.FromBase64String( password ) ) );
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException( "Unable to hash password" );
                default:
                    throw new ProviderException( "Unsupported password format" );
            }

            return password;
        }

        private static byte[] HexToByte( string hexString )
        {
            return UTF8Encoding.UTF8.GetBytes( hexString );
        }

        private MembershipUser GetMembershipUserFromUser( User u )
        {
            return new MembershipUser( u.Name, u.Username, u.Id, u.Email, u.PasswordQuestion, u.Comment, u.IsApproved, u.IsLockedOut, u.CreationDate, u.LastLoginDate, u.LastActivityDate, u.LastPasswordChangedDate, u.LastLockoutDate );
        }
    }
}
