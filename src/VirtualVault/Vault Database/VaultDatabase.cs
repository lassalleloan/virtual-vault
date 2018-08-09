#region Application Description
/**********************************************************************\
*                            VirtualVault                             *
*                                                                     *
* Auteur : L. Lassalle                                                *
* Date de création : 02/02/2015                                       *
* Date de révision : 24/03/2015                                       *
* Description :                                                       *
* Application permettant le stockage sécurisé et l'organisation       *
* des identifiants de connexion et de notes personnelles sécrisées    *
* de l'utilisateur.                                                   *
\**********************************************************************/
#endregion

#region References

#endregion

namespace VirtualVault.Database
{
    public partial class VaultDatabase
    {
        // Properties

        #region Global Variables
        private static string _password = string.Empty;
        private static byte[] _passwordHashed = null;
        private static string _username = string.Empty;
        private static byte[] _usernameHashed = null;
        #endregion

        #region Properties

        #region PasswordHash
        /// <summary>
        /// Contient un mot de passe haché.
        /// </summary>
        private static byte[] PasswordHashed
        {
            get
            {
                return _passwordHashed;
            }
            set
            {
                _passwordHashed = value;
            }
        }
        #endregion

        #region UsernameHash
        /// <summary>
        /// Contient un nom d'utilisateur haché.
        /// </summary>
        private static byte[] UsernameHashed
        {
            get
            {
                return _usernameHashed;
            }
            set
            {
                _usernameHashed = value;
            }
        }
        #endregion

        #region UserPassword
        /// <summary>
        /// Contient un mot de passe utilisateur.
        /// </summary>
        public static string UserPassword
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }
        #endregion

        #region UserUsername
        /// <summary>
        /// Contient un nom d'utilisateur.
        /// </summary>
        public static string UserUsername
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }
        #endregion

        #endregion

        #region Methods

        #region SaveInputs
        /// <summary>
        /// Assigne à des propriétés, des entrées utilisateurs.
        /// </summary>
        /// <param name="_usernameInput">Nom d'utilisateur</param>
        /// <param name="_passwordInput">Mot de passe</param>
        public static void SetInputs(string _usernameInput, string _passwordInput)
        {
            UserUsername = _usernameInput;
            UserPassword = _passwordInput;

            GetVaultID(UserUsername, UserPassword);
        }
        #endregion

        #endregion

    }
}
