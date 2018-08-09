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
using VirtualVault.Page_Switcher;
using VirtualVault.Settings;
using VirtualVault.User_Controls.Authentication;
using VirtualVault.User_Controls.Home_Interface;
#endregion

namespace VirtualVault.User_Controls.Change_Password
{
    /// <summary>
    /// Logique d'interaction pour usc_chgPwd.xaml
    /// </summary>
    public partial class usc_chgPwd : UserControl
    {

        #region Constructors

        #region usc_chgPwd
        /// <summary>
        /// Initialise une nouvelle instance de la classe usc_chgPwd.
        /// </summary>
        public usc_chgPwd()
        {
            InitializeComponent();

            // Change le nom de la fenêtre de l'appication.
            Switcher.ChangeWindowTitle(Data_ChgPwd.Default.ChgPwdTitle);
            return;
        }
        #endregion

        #endregion

        #region Events

        #region cmd_cancel_Click
        /// <summary>
        /// Action lors du clic sur le bouton "cmd_cancel".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_cancel_Click(object sender, RoutedEventArgs e)
        {
            // Affiche l'interface d'accueil.
            usc_home usc_homeChgPwd = new usc_home();
            Switcher.Switch(usc_homeChgPwd);
            return;
        }
        #endregion

        #region cmd_save_Click
        /// <summary>
        /// Action lors du clic sur le bouton "cmd_save".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_save_Click(object sender, RoutedEventArgs e)
        {
            // Assigne à des propriétés, des entrées utilisateurs.
            ChgPwd.SaveIputs(pwd_oldPwd.Password, pwd_newPwd_1.Password, pwd_newPwd_2.Password);

            // Vérifie la longueur du mot de passe.
            if (ChgPwd.IsPwdLengthInfSeven())
            {
                lbl_message.Content = Data_ChgPwd.Default.IsPwdLengthInfSeven;
                return;
            }

            // Vérifie la correspondance des deux mots de passe.
            if (ChgPwd.ArePasswordNotEqual())
            {
                lbl_message.Content = Data_ChgPwd.Default.ArePasswordNotEqual;
                return;
            }

            // Vérifie la correspondance des identifiants de connexion.
            if (ChgPwd.IsUsnAndPwdCheck())
            {
                // Vérifie la mise à jour des identifiants de connexion.
                if (ChgPwd.IsUsnAndPwdUpdate())
                {
                    // Affiche l'interface d'authentification.
                    usc_authen usc_authenUpdPwd = new usc_authen();
                    Switcher.Switch(usc_authenUpdPwd);
                    return;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.UpdateUsnAndPwd);
                    return;
                }
            }
            else
            {
                lbl_message.Content = Data_ChgPwd.Default.IsUsnAndPwdCheck;
                return;
            }
        }
        #endregion

        #region usc_chgPwd_KeyUp
        /// <summary>
        /// Action lors de la remontée d'une touche du clavier.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void usc_chgPwd_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            bool isKeyEnterPess = (e.Key == Key.Enter);

            // Vérifie la remontée de la touche Enter.
            if (isKeyEnterPess)
            {
                cmd_save_Click(sender, e);
            }
        }
        #endregion

        #region usc_chgPwd_Loaded
        /// <summary>
        /// Action lors du chargement de l'interface.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void usc_chgPwd_Loaded(object sender, RoutedEventArgs e)
        {
            // Assigne le focus au champs de saisie du nom d'utilisateur.
            pwd_oldPwd.Focus();
        }
        #endregion


        #endregion

    }
}
