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
#endregion

namespace VirtualVault.User_Controls.Secure_Note
{
    /// <summary>
    /// Logique d'interaction pour usc_scrNote.xaml
    /// </summary>
    public partial class usc_scrNote : UserControl
    {
        #region Constructors
        
        #region usc_scrNote
        /// <summary>
        /// Initialise une nouvelle instance de la classe usc_scrNote.
        /// </summary>
        public usc_scrNote(List<string> _scrNoteData = null)
        {
            InitializeComponent();

            // Modifie la fenêtre principale et l'interface courante.
            Switcher.ChangeWindowTitle(Data_ScrNote.Default.ScrNoteTitle);
            cbo_category.ItemsSource = VaultDatabase.GetCategoryNameList();

            ScrNote.ScrNoteData = _scrNoteData;

            if (ScrNote.ScrNoteData == null)
            {
                ScrNote.UpdateDateAndCtgControls(lbl_crtDateValue, lbl_chgDateValue, cbo_category);
                return;
            }
            else
            {
                txt_name.Text = ScrNote.ScrNoteData[0];
                txt_content.Text = ScrNote.ScrNoteData[1];
                chk_bmk.IsChecked = Convert.ToBoolean(ScrNote.ScrNoteData[2]);
                lbl_chgDateValue.Content = ScrNote.ScrNoteData[3].Substring(0, 10);
                lbl_crtDateValue.Content = ScrNote.ScrNoteData[4].Substring(0, 10);
                cbo_category.SelectedIndex = Convert.ToInt32(ScrNote.ScrNoteData[5]) - 1;
            }
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
            usc_home usc_homeScrNote = new usc_home();
            Switcher.Switch(usc_homeScrNote);
            return;
        }
        #endregion
        
        #region cmd_save_Click
        /// <summary>
        /// Action Lors du clic sur le bouton "cmd_connection".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_save_Click(object sender, RoutedEventArgs e)
        {
            lbl_message.Content = string.Empty;

            // Assigne à des propriétés, des entrées utilisateurs.
            ScrNote.SaveInputs(txt_name.Text, txt_content.Text, cbo_category.SelectedItem.ToString(), Convert.ToBoolean(chk_bmk.IsChecked));

            // Vérifie les données de la note sécurisée.
            if (ScrNote.IsScrNoteDataEmpty())
            {
                lbl_message.Content = Data_ScrNote.Default.IsScrNoteDataEmpty;
                return;
            }

            // Assigne à une liste, des données à enregistrer.
            List<string> scrNoteData = ScrNote.GetScrNoteData();

            if (ScrNote.ScrNoteData != null)
            {
                // Assignement de données pour la modification d'une fiche d'authentification.
                scrNoteData.Add(ScrNote.ScrNoteData[6]);
                VaultDatabase.UpdateScrNoteData(scrNoteData);

                // Affiche l'interface d'accueil.
                usc_home usc_homeNote = new usc_home();
                Switcher.Switch(usc_homeNote);
                return;
            }

            // Vérifie l'existance du nom de la note sécurisée.
            if (ScrNote.IsScrNoteExist())
            {
                lbl_message.Content = Data_ScrNote.Default.IsScrNoteAlreadyExist;
                return;
            }

            // Vérifie le chiffrement des données de la note sécurisée.
            if (ScrNote.IsCipheredScrNoteDataEmpty(scrNoteData))
            {
                System.Windows.Forms.MessageBox.Show(Data_AES.Default.EncryptData);
                return;
            }

            // Vérifie l'enregistrement de la note sécurisée chiffrée.
            if (ScrNote.IsSaveScrNote(scrNoteData))
            {
                // Affiche l'interface d'accueil.
                usc_home usc_homeNote = new usc_home();
                Switcher.Switch(usc_homeNote);
                return;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.SaveScrNote);
                return;
            }
        }
        #endregion

        #region usc_scrNote_Loaded
        /// <summary>
        /// Action lors du chargement de l'interface.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void usc_scrNote_Loaded(object sender, RoutedEventArgs e)
        {
            // Assigne le focus au champs de saisie du nom d'utilisateur.
            txt_name.Focus();
        }
        #endregion
        
        #endregion

    }
}
