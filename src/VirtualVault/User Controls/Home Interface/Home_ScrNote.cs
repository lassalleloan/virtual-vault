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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VirtualVault.Database;
using VirtualVault.Page_Switcher;
using VirtualVault.Settings;
using VirtualVault.User_Controls.Home_Interface;
using VirtualVault.User_Controls.Secure_Note;
#endregion

namespace VirtualVault.User_Controls.Main
{
    public partial class Home
    {
        #region Events

        #region lbl_scrNoteUpdate_MouseLeftButtonUp
        /// <summary>
        /// Action lors du clic sur le label "lbl_scrNoteUpdate".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void lbl_scrNoteUpdate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Label clickedLabel = sender as Label;
            string scrNoteID = clickedLabel.Name.Substring(10);

            List<string> scrNoteName = VaultDatabase.GetScrNoteNameList(scrNoteID);
            List<byte[]> scrNoteCipheredContent = VaultDatabase.GetScrNoteContentList(scrNoteID);
            List<string> scrNotePlainContent = PlainData(scrNoteCipheredContent);
            bool scrNoteBookmark = VaultDatabase.GetScrNoteBookmark(scrNoteID);
            List<string> scrNoteCrtDate = VaultDatabase.GetScrNoteCrtDate(scrNoteID);
            List<string> scrNoteChgDate = VaultDatabase.GetScrNoteChgDate(scrNoteID);
            List<string> scrNoteCategory = VaultDatabase.GetScrNoteCategory(scrNoteID);

            List<string> scrNoteData = new List<string>();
            scrNoteData.Add(scrNoteName[0]);
            scrNoteData.Add(scrNotePlainContent[0]);
            scrNoteData.Add(scrNoteBookmark.ToString());
            scrNoteData.Add(scrNoteCrtDate[0]);
            scrNoteData.Add(scrNoteChgDate[0]);
            scrNoteData.Add(scrNoteCategory[0]);
            scrNoteData.Add(scrNoteID);

            usc_scrNote usc_noteHome = new usc_scrNote(scrNoteData);
            Switcher.Switch(usc_noteHome);

            return;
        }
        #endregion

        #region lbl_scrNoteDelete_MouseLeftButtonUp
        /// <summary>
        /// Action lors du clic sur le label "lbl_scrNoteDelete".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void lbl_scrNoteDelete_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Label clickedLabel = sender as Label;
            string scrNoteID = clickedLabel.Name.Substring(10);

            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(Data_Home.Default.lbl_MenuItemDelete_Click_Message_ScrNote,
                                                                                                Data_Home.Default.lbl_MenuItemDelete_Click_Title_ScrNote,
                                                                                                System.Windows.Forms.MessageBoxButtons.YesNo);

            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                // Vérifie l'exécution de la suppression du de la note sécurisée.
                if (VaultDatabase.DeleteScrNote(scrNoteID))
                {
                    // Affiche l'interface d'accueil.
                    usc_home usc_homeHome = new usc_home();
                    Switcher.Switch(usc_homeHome);
                    return;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.DeleteScrNote);
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

        #endregion

        #region Methods

        #region AddScrNoteDataControls
        /// <summary>
        /// Ajoute des contrôles pour visualiser des notes sécurisées.
        /// </summary>
        /// <param name="_stackPanel">Tableau de contrôles</param>
        /// <param name="_category">Nom de catégorie pour le tri</param>
        /// <param name="_bookmark">Satut dees favoris pour le tri</param>
        public static void AddScrNoteDataControls(StackPanel _stackPanel, string _category = "", string _bookmark = "")
        {
            string category = _category;
            string bookmark = _bookmark;

            // Assigne à des listes, les données des notes sécurisées.
            List<string> scrNoteID = VaultDatabase.GetScrNoteIDList(_categoryName: category, _bookmark: bookmark);
            List<string> scrNoteNameList = VaultDatabase.GetScrNoteNameList(_categoryName: category, _bookmark: bookmark);
            List<byte[]> scrNoteCipheredContenList = VaultDatabase.GetScrNoteContentList(_categoryName: category, _bookmark: bookmark);

            if ((scrNoteNameList != null) || (scrNoteCipheredContenList != null))
            {
                // Assigne à des listes, les données des notes sécurisées.
                List<string> scrNotePlainContentList = PlainData(scrNoteCipheredContenList);

                // Assigne à une étiquette, un nom de section des données.
                Label lbl_dataSection = LabelDataSection(Data_Home.Default.lbl_scrNoteSection_Content);
                _stackPanel.Children.Add(lbl_dataSection);

                for (int index = 0; index < scrNoteNameList.Count; index++)
                {
                    // Assigne à un tableau de contrôles, les données de chaque note sécurisée.
                    StackPanel stp_scrNoteData = StackPanelBaseBuild(910, 136, 20);
                    _stackPanel.Children.Add(stp_scrNoteData);

                    // Assigne à un tableau de contrôles, le nom et les actions d'une note sécurisée.
                    StackPanel stp_scrNoteFirstLine = StackPanelBaseBuild(910, 26, 0);
                    stp_scrNoteFirstLine.Orientation = Orientation.Horizontal;
                    stp_scrNoteData.Children.Add(stp_scrNoteFirstLine);

                    // Assigne à une étiquette, le nom d'une note sécurisée.
                    Label lbl_scrNoteName = LabelBaseBuild(scrNoteNameList[index], 280, 26);
                    stp_scrNoteFirstLine.Children.Add(lbl_scrNoteName);

                    // Assigne à une étiquette, l'action de modifier une note sécurisée.
                    Label lbl_scrNoteUpdate = LabelScrNoteActions(Data_Home.Default.lbl_mainDataUpdate);
                    lbl_scrNoteUpdate.Margin = new Thickness(440, 0, 0, 0);

                    lbl_scrNoteUpdate.Name = Data_Home.Default.scrNoteID + scrNoteID[index];
                    lbl_scrNoteUpdate.MouseLeftButtonUp += new MouseButtonEventHandler(lbl_scrNoteUpdate_MouseLeftButtonUp);
                    stp_scrNoteFirstLine.Children.Add(lbl_scrNoteUpdate);

                    // Assigne à une étiquette, l'action de supprimer une note sécurisée.
                    Label lbl_scrNoteDelete = LabelScrNoteActions(Data_Home.Default.lbl_mainDataDelete);
                    lbl_scrNoteDelete.Margin = new Thickness(10, 0, 0, 0);

                    lbl_scrNoteDelete.Name = Data_Home.Default.scrNoteID + scrNoteID[index];
                    lbl_scrNoteDelete.MouseLeftButtonUp += new MouseButtonEventHandler(lbl_scrNoteDelete_MouseLeftButtonUp);
                    stp_scrNoteFirstLine.Children.Add(lbl_scrNoteDelete);

                    // Assigne à un tableau de contrôles, le contenu d'une note sécurisée.
                    StackPanel stp_scrNoteSecondLine = StackPanelBaseBuild(910, 100, 10);
                    stp_scrNoteData.Children.Add(stp_scrNoteSecondLine);

                    // Assigne à un champs de saisie, le contenu d'ne note sécurisée.
                    TextBox txt_scrNoteContent = TextBoxScrNoteContent(scrNotePlainContentList[index]);
                    txt_scrNoteContent.Margin = new Thickness(260, 0, 0, 0);
                    stp_scrNoteSecondLine.Children.Add(txt_scrNoteContent);
                }
            }
        }
        #endregion

        #region LabelScrNoteActions
        /// <summary>
        /// Assigne à une étiquette d'actions d'une note sécurisée, des propriétés graphiques.
        /// </summary>
        /// <param name="_labelContent">Contenu de l'étiquette</param>
        /// <returns>Label : Réussite -> Etiquette affectée. Echec -> Valeur null.</returns>
        private static Label LabelScrNoteActions(string _labelContent)
        {
            string labelContent = _labelContent;

            // Assigne à une étiquette, des propriétés graphiques.
            Label lbl_stpElementData = LabelBaseBuild(labelContent, 80, 26);
            lbl_stpElementData.HorizontalContentAlignment = HorizontalAlignment.Right;

            return lbl_stpElementData;
        }
        #endregion

        #region TextBoxScrNoteContent
        /// <summary>
        /// Assigne à un champs de saisie de contenu d'une note sécurisée, des propriétés graphiques.
        /// </summary>
        /// <param name="_textBoxContent">Champs de saisie</param>
        /// <returns>TextBox : Réussite -> Champs de saisie affecté. Echec -> Valeur null.</returns>
        private static TextBox TextBoxScrNoteContent(string _textBoxContent)
        {
            string textBoxContent = _textBoxContent;
            TextBox txt_scrNoteContent = new TextBox();

            // Assigne à un champs de saisie d'une note sécurisée, des propriétés graphiques.
            txt_scrNoteContent.Text = textBoxContent;
            txt_scrNoteContent.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            txt_scrNoteContent.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            txt_scrNoteContent.Width = 620;
            txt_scrNoteContent.Height = 100;
            txt_scrNoteContent.IsReadOnly = true;
            txt_scrNoteContent.BorderThickness = new Thickness(0);
            txt_scrNoteContent.TextWrapping = TextWrapping.Wrap;
            txt_scrNoteContent.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            return txt_scrNoteContent;
        }
        #endregion

        #endregion

    }
}
