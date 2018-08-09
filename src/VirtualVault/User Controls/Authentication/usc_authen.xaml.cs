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
using System.Windows.Input;
using VirtualVault.Database;
using VirtualVault.Page_Switcher;
using VirtualVault.Settings;
using VirtualVault.User_Controls.Home_Interface;
#endregion

namespace VirtualVault.User_Controls.Authentication
{
    /// <summary>
    /// Logique d'interaction pour usc_authen.xaml
    /// </summary>
    public partial class usc_authen : UserControl
    {

        #region Global Variables
        private PasswordBox _pwd_password_2 = new PasswordBox();
        #endregion

        #region Constructors

        #region usc_authen
        /// <summary>
        /// Initialise une nouvelle instance de la classe usc_authen.
        /// </summary>
        public usc_authen()
        {
            InitializeComponent();
            
            Switcher.ChangeWindowTitle(Data_Authen.Default.AuthenTitle);

            // Vérifie l'existance d'un coffre-fort virtuel.
            if (Authen.IsFirstLaunch())
            {
                // Ajoute des contrôles à la grille.
                Label lbl_password_2 = Authen.CreateAuthenLabelPassword_2();
                _pwd_password_2 = Authen.CreateAuthenPwdBoxPassword_2();
                Authen.MoveAuthenButtonConnection(cmd_connection);

                grd_authenLayout.Children.Add(lbl_password_2);
                grd_authenLayout.Children.Add(_pwd_password_2);
            }

            return;
        }
        #endregion

        #endregion

        #region Events

        #region cmd_connection_Click
        /// <summary>
        /// Action Lors du clic sur le bouton "cmd_connection".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_connection_Click(object sender, RoutedEventArgs e)
        {
            lbl_message.Content = string.Empty;

            // Assigne à des propriétés, des entrées utilisateurs.
            Authen.SaveIputs(txt_username.Text, pwd_password.Password, _pwd_password_2.Password);

            // Vérifie la saisie des identifiants de connexion.
            if (Authen.IsUsnAndPwdEmpty())
            {
                lbl_message.Content = Data_Authen.Default.IsUsnAndPwdEmpty;
                return;
            }

            if (Authen.IsFirstRun)
            {
                // Vérifie la longueur du mot de passe.
                if (Authen.IsPwdLengthInfSeven())
                {
                    lbl_message.Content = Data_Authen.Default.IsPwdLengthInfSeven;
                    return;
                }

                // Vérifie la correspondance des deux mots de passe.
                if (Authen.ArePasswordNotEqual())
                {
                    lbl_message.Content = Data_Authen.Default.ArePasswordNotEqual;
                    return;
                }

                // Vérifie l'enregistrement des identifiants de connexion.
                if (Authen.IsUsnAndPwdSave())
                {
                    // Assigne à des propriétés, des entrées utilisateurs.
                    VaultDatabase.SetInputs(txt_username.Text, pwd_password.Password);

                    // Ajoute les catégories par défaut.
                    Authen.AddDefaultCategories();

                    // Affiche l'interface d'accueil.
                    usc_authen usc_authenAuthen = new usc_authen();
                    Switcher.Switch(usc_authenAuthen);
                    return;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.SaveUsnAndPwd);
                    return;
                }
            }
            else
            {
                // Vérifie la correspondance des identifiants de connexion.
                if (Authen.IsUsnAndPwdCheck())
                {
                    // Assigne à des propriétés, des entrées utilisateurs.
                    VaultDatabase.SetInputs(txt_username.Text, pwd_password.Password);

                    // Affiche l'interface d'accueil.
                    usc_home usc_homeAuthen = new usc_home();
                    Switcher.Switch(usc_homeAuthen);
                    return;
                }
                else
                {
                    lbl_message.Content = Data_Authen.Default.IsUsnAndPwdCheck;
                    return;
                }
            }

        }
        #endregion

        #region usc_authen_KeyUp
        /// <summary>
        /// Action lors de la remontée d'une touche du clavier.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void usc_authen_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            bool isKeyEnterPess = (e.Key == Key.Enter);

            // Vérifie la remontée de la touche Enter.
            if (isKeyEnterPess)
            {
                cmd_connection_Click(sender, e);
            }

            return;
        }
        #endregion

        #region usc_authen_Loaded
        /// <summary>
        /// Action lors du chargement de l'interface.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void usc_authen_Loaded(object sender, RoutedEventArgs e)
        {
            // Assigne le focus au champs de saisie du nom d'utilisateur.
            txt_username.Focus();
        }
        #endregion

        #endregion

    }
}
