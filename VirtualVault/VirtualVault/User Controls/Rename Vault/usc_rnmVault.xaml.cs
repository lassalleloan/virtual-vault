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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VirtualVault.Database;
using VirtualVault.Page_Switcher;
using VirtualVault.Settings;
using VirtualVault.User_Controls.Home_Interface;
#endregion

namespace VirtualVault.User_Controls.Rename_Vault
{
    /// <summary>
    /// Logique d'interaction pour usc_rnmVault.xaml
    /// </summary>
    public partial class usc_rnmVault : UserControl
    {
        #region Constructors

        #region usc_rnmVault
        /// <summary>
        /// Initialise une nouvelle instance de la classe usc_rnmVault.
        /// </summary>
        public usc_rnmVault(string _oldVaultName)
        {
            InitializeComponent();

            // Modifie la fenêtre principale et récupère l'ancien nom du coffre-fort virtuel.
            Switcher.ChangeWindowTitle(Data_RnmVault.Default.RnmVaultTitle);
            txt_name.Text = _oldVaultName;
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
            usc_home nfc_homeRnmVault = new usc_home();
            Switcher.Switch(nfc_homeRnmVault);
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
            RnmVault.SaveInputs(txt_name.Text);

            // Vérifie la longueur du nom du coffre-fort virtuel.
            if (RnmVault.IsVltNameLength_Error())
            {
                lbl_message.Content = Data_RnmVault.Default.IsVltNameLength_Error;
                return;
            }

            // Vérifie la mise à jour du nom du coffre-fort virtuel.
            if (VaultDatabase.UpdVaultName(txt_name.Text))
            {
                // Affiche l'interface d'accueil.
                usc_home usc_homeRename = new usc_home();
                Switcher.Switch(usc_homeRename);
                return;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.UpdVaultName);
                return;
            }
        }
        #endregion

        #region usc_rnmVault_KeyUp
        /// <summary>
        /// Action lors de la remontée d'une touche du clavier.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void usc_rnmVault_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            bool isKeyEnterPess = (e.Key == Key.Enter);

            // Vérifie la remontée de la touche Enter.
            if (isKeyEnterPess)
            {
                cmd_save_Click(sender, e);
            }
        }
        #endregion

        #region usc_rnmVault_Loaded
        /// <summary>
        /// Action lors du chargement de l'interface.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void usc_rnmVault_Loaded(object sender, RoutedEventArgs e)
        {
            // Assigne le focus au champs de saisie du nom d'utilisateur.
            txt_name.Focus();
        }
        #endregion
        
        #endregion

    }
}
