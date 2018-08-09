#region Application Description
/**********************************************************************\
*                            VirtualVault                             *
*                                                                     *
* Auteur : L. Lassalle                                                *
* Date de création : 02/02/2015                                       *
* Date de révision : 24/03/2015                                       *
* Description :                                                       *
* Application permettant le stockage sécurisé et l'organisation       *
* des identifiants de connexion et de notes personnelles sécurisées    *
* de l'utilisateur.                                                   *
\**********************************************************************/
#endregion

#region References
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VirtualVault.Database;
using VirtualVault.Page_Switcher;
using VirtualVault.Settings;
using VirtualVault.User_Controls.Authentication_Card;
using VirtualVault.User_Controls.Home_Interface;
#endregion

namespace VirtualVault.User_Controls.Main
{
    public partial class Home
    {

        #region Global Variables

        #endregion

        #region Properties



        #endregion

        #region Events

        #region cmd_aCardCopyPwd_Click
        /// <summary>
        /// Action lors du clic sur le bouton "cmd_aCardCopyPwd".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void cmd_aCardCopyPwd_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            string authenCardID = clickedButton.Name.Substring(13);
            List<byte[]> cipheredACardPwd = VaultDatabase.GetACardPwdList(authenCardID);
            List<string> plainACardPwd = PlainData(cipheredACardPwd);

            // Enregistre le mot de passe dans le presse-papier. 
            Clipboard.SetData(DataFormats.Text, (Object)plainACardPwd[0]);
            return;
        }
        #endregion

        #region cmd_aCardCopyUsn_Click
        /// <summary>
        /// Action lors du clic sur le bouton "cmd_aCardCopyUsn".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void cmd_aCardCopyUsn_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            string authenCardID = clickedButton.Name.Substring(13);
            List<byte[]> cipheredACardUsn = VaultDatabase.GetACardUsnList(authenCardID);
            List<string> plainACardUsn = PlainData(cipheredACardUsn);

            // Enregistre le nom d'utilisateur dans le presse-papier. 
            Clipboard.SetData(DataFormats.Text, (Object)plainACardUsn[0]);
            return;
        }
        #endregion

        #region lbl_aCardDelete_MouseLeftButtonUp
        /// <summary>
        /// Action lors du clic sur le label "lbl_aCardDelete".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void lbl_aCardDelete_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Label clickedLabel = sender as Label;
            string authenCardID = clickedLabel.Name.Substring(13);

            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(Data_Home.Default.lbl_MenuItemDelete_Click_Message_AuthenCard,
                                                                                                Data_Home.Default.lbl_MenuItemDelete_Click_Title_AuthenCard,
                                                                                                System.Windows.Forms.MessageBoxButtons.YesNo);

            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                // Vérifie l'exécution de la suppression de la fiche d'authentification.
                if (VaultDatabase.DeleteAuthenCard(authenCardID))
                {
                    // Affiche l'interface d'accueil.
                    usc_home usc_homeHome = new usc_home();
                    Switcher.Switch(usc_homeHome);
                    return;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.DeleteAuthenCard);
                    return;
                }
            }
            else
            {
                // Affiche l'interface d'accueil.
                usc_home usc_homeHome = new usc_home();
                Switcher.Switch(usc_homeHome);
                return;
            }
        }
        #endregion

        #region lbl_authenCardShc_MouseLeftButtonUp
        /// <summary>
        /// Action lors du clic sur le label "lbl_authenCardShc".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void lbl_authenCardShc_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Label lbl_authenCardShc = sender as Label;
            string authenCardShcContent = lbl_authenCardShc.Content.ToString();
            bool isNotAuthenCardShcContentEmpty = (authenCardShcContent != " ");

            // Vérifie le contenu de l'étiquette du raccourci Internet.
            if (isNotAuthenCardShcContentEmpty)
            {
                // Ouverture du lien Internet dans le navigateur par défaut.
                Process.Start(lbl_authenCardShc.Content.ToString());
            }
        }
        #endregion

        #region lbl_aCardUpdate_MouseLeftButtonUp
        /// <summary>
        /// Action lors du clic sur le label "lbl_aCardUpdate".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void lbl_aCardUpdate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Assigne les données d'une fiche d'authentification pour la modifier.
            Label clickedLabel = sender as Label;
            string authenCardID = clickedLabel.Name.Substring(13);

            List<string> authenCardName = VaultDatabase.GetACardNameList(authenCardID);
            List<string> authenCardShc = VaultDatabase.GetACardShcList(authenCardID);

            List<byte[]> authenCardCipheredUsn = VaultDatabase.GetACardUsnList(authenCardID);
            List<string> authenCardPlainUsn = PlainData(authenCardCipheredUsn);

            List<byte[]> authenCardCipheredPwd = VaultDatabase.GetACardPwdList(authenCardID);
            List<string> authenCardPlainPwd = PlainData(authenCardCipheredPwd);

            List<byte[]> authenCardCipheredScrNote = VaultDatabase.GetACardScrNoteList(authenCardID);
            List<string> authenCardPlainScrNote = PlainData(authenCardCipheredScrNote);

            bool authenCardBookmark = VaultDatabase.GetACardBookmark(authenCardID);
            List<string> authenCardCrtDate = VaultDatabase.GetACardCrtDate(authenCardID);
            List<string> authenCardChgDate = VaultDatabase.GetACardChgDate(authenCardID);
            List<string> authenCardCategory = VaultDatabase.GetACardCategory(authenCardID);

            List<string> authenCardData = new List<string>();
            authenCardData.Add(authenCardName[0]);
            authenCardData.Add(authenCardShc[0]);
            authenCardData.Add(authenCardPlainUsn[0]);
            authenCardData.Add(authenCardPlainPwd[0]);
            authenCardData.Add(authenCardPlainScrNote[0]);
            authenCardData.Add(authenCardBookmark.ToString());
            authenCardData.Add(authenCardCrtDate[0]);
            authenCardData.Add(authenCardChgDate[0]);
            authenCardData.Add(authenCardCategory[0]);
            authenCardData.Add(authenCardID);

            usc_authenCard usc_authenCardHome = new usc_authenCard(authenCardData);
            Switcher.Switch(usc_authenCardHome);

            return;
        }
        #endregion

        #endregion

        #region Methods

        #region AddACardsControls
        /// <summary>
        /// Ajoute des contrôles pour visualiser des fiches d'authentification.
        /// </summary>
        /// <param name="_stackPanel">Tableau de contrôles</param>
        /// <param name="_category">Nom de catégorie pour le tri</param>
        /// <param name="_bookmark">Satut dees favoris pour le tri</param>
        public static void AddACardsControls(StackPanel _stackPanel, string _category = "", string _bookmark = "")
        {
            string category = _category;
            string bookmark = _bookmark;
            string contentLabel = string.Empty;

            // Assigne à des listes, les données des fiches d'authentification.
            List<string> aCardsIDList = VaultDatabase.GetACardIDList(_categoryName:category, _bookmark:bookmark);
            List<string> aCardsNameList = VaultDatabase.GetACardNameList(_categoryName: category, _bookmark: bookmark);
            List<string> aCardsShcList = VaultDatabase.GetACardShcList(_categoryName: category, _bookmark: bookmark);
            List<byte[]> aCardsCipheredUsnList = VaultDatabase.GetACardUsnList(_categoryName: category, _bookmark: bookmark);
            List<string> aCardsCpxList = VaultDatabase.GetACardCpxFactorList(_categoryName: category, _bookmark: bookmark);

            if ((aCardsNameList != null) || (aCardsShcList != null) || (aCardsCipheredUsnList != null) || (aCardsCpxList != null))
            {
                // Assigne à des listes, les données des fiches d'authentification.
                List<string> aCardPlainUsnList = PlainData(aCardsCipheredUsnList);

                // Assigne à une étiquette, un nom de section des données.
                Label lbl_dataSection = LabelDataSection(Data_Home.Default.lbl_aCardSection_Content);
                _stackPanel.Children.Add(lbl_dataSection);

                for (int index = 0; index < aCardsNameList.Count; index++)
                {
                    // Assigne à un tableau de contrôles, les données de chaque fiche d'authentification.
                    StackPanel stp_AuthenCardData = new StackPanel();
                    stp_AuthenCardData.Width = 910;
                    stp_AuthenCardData.Height = 72;
                    _stackPanel.Children.Add(stp_AuthenCardData);

                    // Assigne à un tableau de contrôles, le nom et les actions d'une fiche d'authentification.
                    StackPanel stp_aCardFirstLine = new StackPanel();
                    stp_aCardFirstLine.Height = 36;
                    stp_aCardFirstLine.Orientation = Orientation.Horizontal;
                    stp_AuthenCardData.Children.Add(stp_aCardFirstLine);

                    // Assigne à une étiquette, le nom d'une fiche d'authentification.
                    Label lbl_authenCardName = LabelBaseBuild(aCardsNameList[index], 270, 26);
                    stp_aCardFirstLine.Children.Add(lbl_authenCardName);

                    // Assigne à une étiquette, le raccourci d'une fiche d'authentification.
                    Label lbl_authenCardShc = LabelBaseBuild(aCardsShcList[index], 170, 26);
                    lbl_authenCardShc.Margin = new Thickness(10, 0, 0, 0);
                    lbl_authenCardShc.Foreground = new SolidColorBrush(Colors.Blue);
                    lbl_authenCardShc.MouseLeftButtonUp += new MouseButtonEventHandler(lbl_authenCardShc_MouseLeftButtonUp);
                    stp_aCardFirstLine.Children.Add(lbl_authenCardShc);

                    // Assigne à une étiquette, le nom d'utilisateur d'une fiche d'authentification.
                    Label lbl_authenCardUsn = LabelBaseBuild(aCardPlainUsnList[index], 190, 26);
                    lbl_authenCardUsn.Margin = new Thickness(10, 0, 0, 0);
                    stp_aCardFirstLine.Children.Add(lbl_authenCardUsn);

                    // Assigne à une étiquette, le facteur de complexité d'une fiche d'authentification.
                    contentLabel = aCardsCpxList[index] + Data_Home.Default.lbl_aCardCpxFactor_Content;
                    Label lbl_aCardCpxFactor = LabelBaseBuild(contentLabel, 90, 26);
                    lbl_aCardCpxFactor.Margin = new Thickness(10, 0, 0, 0);
                    ChangeLabelColor(aCardsCpxList[index], lbl_aCardCpxFactor);
                    stp_aCardFirstLine.Children.Add(lbl_aCardCpxFactor);

                    // Assigne à une étiquette, l'action de modifier une fiche d'authentification.
                    Label lbl_aCardUpdate = LabelBaseBuild(Data_Home.Default.lbl_mainDataUpdate, 60, 26);
                    lbl_aCardUpdate.Margin = new Thickness(10, 0, 0, 0);

                    lbl_aCardUpdate.Name = Data_Home.Default.authenCardID + aCardsIDList[index];
                    lbl_aCardUpdate.MouseLeftButtonUp += new MouseButtonEventHandler(lbl_aCardUpdate_MouseLeftButtonUp);
                    stp_aCardFirstLine.Children.Add(lbl_aCardUpdate);

                    // Assigne à une étiquette, l'action de supprimer une fiche d'authentification.
                    Label lbl_aCardDelete = LabelBaseBuild(Data_Home.Default.lbl_mainDataDelete, 70, 26);
                    lbl_aCardDelete.Margin = new Thickness(10, 0, 0, 0);

                    lbl_aCardDelete.Name = Data_Home.Default.authenCardID + aCardsIDList[index];
                    lbl_aCardDelete.MouseLeftButtonUp += new MouseButtonEventHandler(lbl_aCardDelete_MouseLeftButtonUp);
                    stp_aCardFirstLine.Children.Add(lbl_aCardDelete);

                    // Assigne à un tableau de contrôles, les actions de copies d'une fiche d'authentification.
                    StackPanel stp_aCardSecondLine = new StackPanel();
                    stp_aCardSecondLine.Height = 36;
                    stp_aCardSecondLine.Orientation = Orientation.Horizontal;
                    stp_AuthenCardData.Children.Add(stp_aCardSecondLine);

                    // Assigne à un bouton, l'action de copier le nom d'utilisateur d'une fiche d'authentification.
                    Button cmd_aCardCopyUsn = ButtonACardCopy(Data_Home.Default.cmd_aCardCopyUsn);
                    cmd_aCardCopyUsn.Margin = new Thickness(590, 0, 0, 0);

                    cmd_aCardCopyUsn.Name = Data_Home.Default.authenCardID + aCardsIDList[index];
                    cmd_aCardCopyUsn.Click += new RoutedEventHandler(cmd_aCardCopyUsn_Click);
                    stp_aCardSecondLine.Children.Add(cmd_aCardCopyUsn);

                    // Assigne à un bouton, l'action de copier le mot de passe d'une fiche d'authentification.
                    Button cmd_aCardCopyPwd = ButtonACardCopy(Data_Home.Default.cmd_aCardCopyPwd);
                    cmd_aCardCopyPwd.Margin = new Thickness(10, 0, 0, 0);

                    cmd_aCardCopyPwd.Name = Data_Home.Default.authenCardID + aCardsIDList[index];
                    cmd_aCardCopyPwd.Click += new RoutedEventHandler(cmd_aCardCopyPwd_Click);
                    stp_aCardSecondLine.Children.Add(cmd_aCardCopyPwd);
                }
            }
        }
        #endregion

        #region ButtonACardCopy
        /// <summary>
        /// Assigne à un bouton, des propriétés graphiques.
        /// </summary>
        /// <param name="_buttonContent">Contenu du bouton</param>
        /// <returns>Label : Réussite -> Bouton affecté. Echec -> Valeur null.</returns>
        private static Button ButtonACardCopy(string _buttonContent)
        {
            string buttonContent = _buttonContent;
            Button cmd_dataBase = new Button();

            // Assigne à un bouton, des propriétés graphiques.
            cmd_dataBase.Content = buttonContent;
            cmd_dataBase.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            cmd_dataBase.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            cmd_dataBase.Width = 150;
            cmd_dataBase.Height = 26;

            return cmd_dataBase;
        }
        #endregion

        #region GetLabelColor
        /// <summary>
        /// Obtient une couleurà assigner à deux étiquettes.
        /// </summary>
        /// <param name="_stringValue">Pourcentage de complexité</param>
        /// <param name="_label">Etiquette</param>
        /// <param name="_lbl_cpxCmt">Etiquette</param>
        private static void ChangeLabelColor(string _stringValue, Label _label)
        {
            const int POURCENT_COLOR = 50;
            int value = Convert.ToInt32(_stringValue);
            bool isValueInfFifty = (value < POURCENT_COLOR);

            // Vérifie la valeur du pourcentage d'efficacité.
            if (isValueInfFifty)
            {
                // Assigne à une étiquette, une couleur.
                _label.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                // Assigne à une étiquette, une couleur.
                _label.Foreground = new SolidColorBrush(Colors.Green);
            }
        }
        #endregion

        #endregion

    }
}
