#region Application Description
/**********************************************************************\
*                            VirtualVault                             *
*                                                                     *
* Auteur : L. Lassalle                                                *
* Date de création : 02/02/2015                                       *
* Date de révision : 24/03/2015                                       *
* Description :                                                       *
* Application permettant le stockage sécurisé et l'organisation       *
* des identifiants de connexion et de notes personnelles sécurisées   *
* de l'utilisateur.                                                   *
\**********************************************************************/
#endregion

#region References
using System.Windows;
using System.Windows.Controls;
using VirtualVault.Database;
using VirtualVault.Settings;
#endregion

namespace VirtualVault.User_Controls.Authentication
{
    public static class Authen
    {

        #region Global Variables
        private static bool _isFirstRun = false;
        private static string _passwordInput_1 = string.Empty;
        private static string _passwordInput_2 = string.Empty;
        private static string _usernameInput = string.Empty;
        #endregion

        #region Properties

        #region IsFirstRun
        /// <summary>
        /// Contient le statut du premier lancement.
        /// </summary>
        public static bool IsFirstRun
        {
            get
            {
                return _isFirstRun;
            }
            set
            {
                _isFirstRun = value;
            }
        }
        #endregion

        #region PasswordInput_1
        /// <summary>
        /// Contient le mot de passe.
        /// </summary>
        private static string PasswordInput_1
        {
            get
            {
                return _passwordInput_1;
            }
            set
            {
                _passwordInput_1 = value;
            }
        }
        #endregion

        #region PasswordInput_2
        /// <summary>
        /// Contient le mot de passe de confirmation.
        /// </summary>
        private static string PasswordInput_2
        {
            get
            {
                return _passwordInput_2;
            }
            set
            {
                _passwordInput_2 = value;
            }
        }
        #endregion

        #region UsernameInput
        /// <summary>
        /// Contient le nom d'utilisateur.
        /// </summary>
        private static string UsernameInput
        {
            get
            {
                return _usernameInput;
            }
            set
            {
                _usernameInput = value;
            }
        }
        #endregion

        #endregion

        #region Methods

        #region AddDefaultCategories
        /// <summary>
        /// Ajoute des catégories par défaut.
        /// </summary>
        /// <remarks>
        /// Ajoute des catégories par défaut après avoir vérifié leurs existance.
        /// </remarks>
        public static void AddDefaultCategories()
        {
            // Vérifie et enregistre un nom de catégorie.
            IsNotCategoryExist(Data_Authen.Default.AllItem);
            IsNotCategoryExist(Data_Authen.Default.Personnal);
            IsNotCategoryExist(Data_Authen.Default.Professional);
            IsNotCategoryExist(Data_Authen.Default.Mailbox);
            IsNotCategoryExist(Data_Authen.Default.SocialNetwork);
            IsNotCategoryExist(Data_Authen.Default.Bank);
        }
        #endregion

        #region ArePasswordNotEqual
        /// <summary>
        /// Vérifie la correspondance entre deux mots de passe.
        /// </summary>
        /// <returns>Boolean : True -> Mots de passe différents. False -> Mots de passe égaux.</returns>
        public static bool ArePasswordNotEqual()
        {
            bool arePasswordNotEqual = (!PasswordInput_1.Equals(PasswordInput_2));

            // Vérifie la correspondance entre deux mots de passe.
            if (arePasswordNotEqual)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region CreateAuthenLabelPassword_2
        /// <summary>
        /// Crée une étiquette pour le mot de passe de confirmation.
        /// </summary>
        /// <returns>Label : Réussite -> Etiquette du mot de passe de confirmation. Echec -> Valeur null.</returns>
        public static Label CreateAuthenLabelPassword_2()
        {
            Label lbl_password2 = new Label();

            // Assigne à une étiquette des propriétés graphique.
            lbl_password2.Content = Data_Authen.Default.AddAuthenLabelPassword_2;
            lbl_password2.Margin = new Thickness(329, 326, 0, 0);
            lbl_password2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            lbl_password2.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            lbl_password2.Width = 173;
            lbl_password2.Height = 26;

            return lbl_password2;
        }
        #endregion

        #region CreateAuthenPwdBoxPassword_2
        /// <summary>
        /// Crée un champs de saisie pour le mot de passe de confirmation.
        /// /// </summary>
        /// <returns>TextBox : Réussite -> Champs de saisie du mot de passe de confirmation. Echec -> Valeur null.</returns>
        public static PasswordBox CreateAuthenPwdBoxPassword_2()
        {
            PasswordBox txt_password2 = new PasswordBox();

            // Assigne à un champs de saisie des propriétés graphiques.
            txt_password2.Margin = new Thickness(507, 326, 0, 0);
            txt_password2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            txt_password2.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            txt_password2.Width = 220;
            txt_password2.Height = 26;
            txt_password2.TabIndex = 3;

            return txt_password2;
        }
        #endregion

        #region FirstLaunch
        /// <summary>
        /// Vérifie l'existance d'un coffre-fort virtuel.
        /// </summary>
        /// <returns>Boolean : True -> Coffre-fort déjà existant. False -> Coffre-fort inexistant.</returns>
        public static bool IsFirstLaunch()
        {
            IsFirstRun = VaultDatabase.IsFirstLaunch();

            // Vérifie l'existance d'un coffre-fort virtuel.
            if (IsFirstRun)
            {
                // Vide les valeurs de salage.
                Data_Hash.Default.Reset();
                return true;
            }

            return false;
        }
        #endregion

        #region IsNotCategoryExist
        /// <summary>
        /// Vérifie l'existance d'une catégorie et l'enregistre.
        /// </summary>
        /// <param name="_categoryName">Nom d'une catégorie</param>
        private static void IsNotCategoryExist(string _categoryName)
        {
            string categoryName = _categoryName;
            bool isNotCategoryExist = (!VaultDatabase.CheckCategory(categoryName));

            // Vérifie l'existance du nom de catégorie.
            if (isNotCategoryExist)
            {
                // Enregistre un nom de catégorie.
                VaultDatabase.SaveCategory(categoryName);
            }
        }
        #endregion
        
        #region IsPwdLengthInfSeven
        /// <summary>
        /// Vérifie la longueur du mot de passe.
        /// </summary>
        /// <remarks>
        /// Le mot de passe ne doit pas être null, inférieur ou égale à 7 caractères.
        /// </remarks>
        /// <returns>Boolean : True -> Longueur incorrect. False -> Longueur correcte.</returns>
        public static bool IsPwdLengthInfSeven()
        {
            const int PWD_LENGHT_AVOID = 7;
            bool isPwdLengthInfSeven = (PasswordInput_1.Length <= PWD_LENGHT_AVOID);

            // Vérifie si le mot de passe est null, inférieur ou égale à 7 caractères.
            if (isPwdLengthInfSeven)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region IsUsnAndPwdCheck
        /// <summary>
        /// Vérifie la correspondance des identifiants de connexion.
        /// </summary>
        /// <returns>Boolean : True -> Identifiants corrects. False -> Identifiants incorrect ou erreur.</returns>
        public static bool IsUsnAndPwdCheck()
        {
            bool hashAreEqual = false;

            hashAreEqual = VaultDatabase.CheckUsnAndPwd(UsernameInput, PasswordInput_1);

            return hashAreEqual;
        }
        #endregion

        #region IsUsnAndPwdEmpty
        /// <summary>
        /// Vérifie la longueur des identifiants de connexion.
        /// </summary>
        /// <remarks>
        /// Les identifiants de connexion ne doivent pas être null, inférieur ou égale à 0 caractères.
        /// </remarks>
        /// <returns>Boolean : True -> Longueur incorrect. False -> Longueur correct.</returns>
        public static bool IsUsnAndPwdEmpty()
        {
            bool isUsernameError = ((UsernameInput == null) || (UsernameInput.Length <= 0));
            bool isPasswordError = ((PasswordInput_1 == null) || (PasswordInput_1.Length <= 0));

            // Vérifie si identifiants de connexion sont null, inférieur ou égale à 0 caractères.
            if (isUsernameError || isPasswordError)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region IsUsnAndPwdSave
        /// <summary>
        /// Enregistre les identifiants de connexion.
        /// </summary>
        /// <returns>Boolean : True -> Identifiants de connexion enregistrés. False -> Identifiants de connexion non enregistrés.</returns>
        public static bool IsUsnAndPwdSave()
        {
            bool success = false;

            success = VaultDatabase.SaveUsnAndPwd(UsernameInput, PasswordInput_1);

            return success;
        }
        #endregion

        #region MoveAuthenButtonConnection
        /// <summary>
        /// Déplace un bouton de connexion.
        /// </summary>
        /// <param name="_cmd_connection">Bouton</param>
        public static void MoveAuthenButtonConnection(Button _cmd_connection)
        {
            // Assigne à un bouton des propriétés graphiques.
            _cmd_connection.Margin = new Thickness(732, 326, 0, 0);
            _cmd_connection.TabIndex = 4;
        }
        #endregion

        #region SaveIputs
        /// <summary>
        /// Assigne à des propriétés, des entrées utilisateurs.
        /// </summary>
        /// <param name="_usernameInput">Nom d'utilisateur</param>
        /// <param name="_passwordInput">Mot de passe</param>
        public static void SaveIputs(string _usernameInput, string _passwordInput_1, string _passwordInput_2)
        {
            UsernameInput = _usernameInput;
            PasswordInput_1 = _passwordInput_1;
            PasswordInput_2 = _passwordInput_2;
        }
        #endregion

        #endregion

    }
}
