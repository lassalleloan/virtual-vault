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
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using VirtualVault.Database;
using VirtualVault.Page_Switcher;
using VirtualVault.Settings;
using VirtualVault.User_Controls.Home_Interface;
using VirtualVault.User_Controls.Password_Generator;
#endregion

namespace VirtualVault.User_Controls.Authentication_Card
{
    /// <summary>
    /// Logique d'interaction pour usc_authenCard.xaml
    /// </summary>
    public partial class usc_authenCard : UserControl
    {

        #region Constructors

        #region usc_authenCard
        /// <summary>
        /// Initialise une nouvelle instance de la classe usc_authenCard.
        /// </summary>
        public usc_authenCard(List<string> _authenCardData = null)
        {
            InitializeComponent();

            // Modifie la fenêtre principale et l'interface courante.
            Switcher.ChangeWindowTitle(Data_AuthenCard.Default.AuthenCardTitle);
            cbo_category.ItemsSource = VaultDatabase.GetCategoryNameList();

            AuthenCard.ACardData = _authenCardData;

            if (AuthenCard.ACardData == null)
            {
                AuthenCard.UpdateDateAndCtgControls(lbl_crtDateValue, lbl_chgDateValue, cbo_category);
                return;
            }
            else
            {
                // Assigne à des champs de saisie, les valeurs récupérées pour la modification de la fiche d'authentification.
                txt_name.Text = AuthenCard.ACardData[0];
                txt_shortcut.Text = AuthenCard.ACardData[1];
                txt_username.Text = AuthenCard.ACardData[2];
                txt_password.Text = AuthenCard.ACardData[3];
                txt_scrNote.Text = AuthenCard.ACardData[4];
                chk_bmk.IsChecked = Convert.ToBoolean(AuthenCard.ACardData[5]);

                // Assigne à des étiquette, seulement la date.
                lbl_chgDateValue.Content = AuthenCard.ACardData[6].Substring(0, 10);
                lbl_crtDateValue.Content = AuthenCard.ACardData[7].Substring(0, 10);

                // Assigne à une liste déroulante, un élément sélectionné par défaut.
                cbo_category.SelectedIndex = Convert.ToInt32(AuthenCard.ACardData[8]) - 1;
                return;
            }
        }
        #endregion

        #endregion

        #region Events

        #region cmd_cancel_Click
        /// <summary>
        /// Action lors clic sur le bouton "cmd_cancel". 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_cancel_Click(object sender, RoutedEventArgs e)
        {
            // Affiche l'interface d'accueil.
            usc_home usc_homeAuthenCard = new usc_home();
            Switcher.Switch(usc_homeAuthenCard);
            return;
        }
        #endregion

        #region cmd_pwdGen_Click
        /// <summary>
        /// Action lors du clic sur le bouton "cmd_pwdGen".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_pwdGen_Click(object sender, RoutedEventArgs e)
        {
            // Ouverture de la fenêtre de génération de mots de passe aléatoire.
            wdw_pwdGen wdw_pwdGenHome = new wdw_pwdGen();
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
            string cpxFactor = lbl_cpxFactor.Content.ToString();
            string selectedCategory = cbo_category.SelectedItem.ToString();
            bool bookmarkSatut = Convert.ToBoolean(chk_bmk.IsChecked);

            lbl_message.Content = string.Empty;

            // Assigne à des propriétés, des entrées utilisateurs.
            AuthenCard.SaveIputs(txt_name.Text, txt_shortcut.Text, txt_username.Text, txt_password.Text, cpxFactor, txt_scrNote.Text, selectedCategory, bookmarkSatut);

            // Vérifie les données de la fiche d'authentification.
            if (AuthenCard.IsAuthenCardDataEmpty())
            {
                lbl_message.Content = Data_AuthenCard.Default.IsACardDataNull;
                return;
            }

            // Assigne à une liste des données à enregistrer.
            List<string> aCardData = AuthenCard.GetAuthenCardData();

            // Vérifie le contenu d'une propriété pour la modification.
            if (AuthenCard.ACardData != null)
            {
                // Assignement de données pour la modification d'une fiche d'authentification.
                aCardData.Add(AuthenCard.ACardData[9]);
                VaultDatabase.UpdateAuthenCardData(aCardData);

                // Affiche l'interface d'accueil.
                usc_home usc_homeAuthenCard = new usc_home();
                Switcher.Switch(usc_homeAuthenCard);
                return;
            }

            // Vérifie l'existance du nom de la fiche d'authentification.
            if (AuthenCard.IsAuthenCardExist())
            {
                lbl_message.Content = Data_AuthenCard.Default.IsAuthenCardExist;
                return;
            }

            // Vérifie le chiffrement des données de la fiche d'authentification.
            if (AuthenCard.IsCipheredACardDataEmpty(aCardData))
            {
                System.Windows.Forms.MessageBox.Show(Data_AES.Default.EncryptData);
                return;
            }

            // Vérifie l'enregistrement de la fiche d'authentification chiffrée.
            if (AuthenCard.IsSaveAuthenCard(aCardData))
            {
                // Affiche l'interface d'accueil.
                usc_home usc_homeNote = new usc_home();
                Switcher.Switch(usc_homeNote);
                return;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.SaveAuthenCard);
                return;
            } 
        }
        #endregion

        #region txt_password_TextChanged
        /// <summary>
        /// Action lors du changement du contenu de la textBox "txt_password".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_password_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Assigne à deux étiquettes, une couleur.
            AuthenCard.GetLabelColor(txt_password.Text, lbl_cpxFactor, lbl_cpxCmt);
        }
        #endregion

        #region usc_authenCard_Loaded
        /// <summary>
        /// Action lors du chargement de l'interface.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void usc_authenCard_Loaded(object sender, RoutedEventArgs e)
        {
            // Assigne le focus au champs de saisie du nom d'utilisateur.
            txt_name.Focus();
        }
        #endregion

        #endregion

    }
}
