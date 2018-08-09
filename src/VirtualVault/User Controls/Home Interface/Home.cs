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
using System.Text;
using System.Windows;
using System.Windows.Controls;
using VirtualVault.Cryptography;
using VirtualVault.Database;
using VirtualVault.Settings;
#endregion

namespace VirtualVault.User_Controls.Main
{
    public partial class Home
    {
        
        #region Methods

        #region GetAllItemNbr
        /// <summary>
        /// Obtient le nombre total d'éléments.
        /// </summary>
        /// <returns>Int : Réussite -> Nombre total d'éléments. Echec -> Valeur -2.</returns>
        public static int GetAllItemNbr(string _categoryName = "", string _bookmark = "")
        {
            string categoryName = _categoryName;
            string bookmark = _bookmark;

            // Assigne à un nombre le nombre de fiche d'authentification.
            int authenCardNbr = VaultDatabase.GetACardNumber(categoryName, bookmark);

            // Assigne à un nombre le nombre de note sécurisée.
            int scrNoteNbr = VaultDatabase.GetScrNoteNumber(categoryName, bookmark);

            // Assigne à un nombre le total des deux nombres.
            int allItemNbr = authenCardNbr + scrNoteNbr;

            return allItemNbr;
        }
        #endregion

        #region AddAnyDataControl
        /// <summary>
        /// Ajoute un contrôle si aucun élément n'existe.
        /// </summary>
        /// <param name="_stackPanel">Tableau de contrôles</param>
        public static Label AddAnyDataControl()
        {
            // Assigne à une étiquette des propriétés graphique.
            Label lbl_anyItem = LabelBaseBuild(Data_Home.Default.lbl_anyItem_Content, 260, 40);
            lbl_anyItem.Margin = new Thickness(340, 241, 0, 0);
            lbl_anyItem.FontSize = 20;
            lbl_anyItem.FontWeight = FontWeights.Bold;

            return lbl_anyItem;

        }
        #endregion

        #region LabelBaseBuild
        /// <summary>
        /// Assigne à une étiquette des propriétés graphiques basiques.
        /// </summary>
        /// <param name="_labelContent">Nom de l'étiquette</param>
        /// <param name="_labelWidth">Largeur de l'étiquette</param>
        /// <param name="_labelHeight">Hauteur de l'étiquette</param>
        /// <returns>Label : Réussite -> Etiquette affectée. Echec -> Valeur null.</returns>
        private static Label LabelBaseBuild(string _labelContent, int _labelWidth, int _labelHeight)
        {
            string labelContent = _labelContent;
            int labelWidth = _labelWidth;
            int labelHeight = _labelHeight;
            Label lbl_dataBase = new Label();

            // Assigne à une étiquette des propriétés graphiques basiques.
            lbl_dataBase.Content = labelContent;
            lbl_dataBase.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            lbl_dataBase.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            lbl_dataBase.Width = labelWidth;
            lbl_dataBase.Height = labelHeight;

            return lbl_dataBase;
        }
        #endregion

        #region LabelDataSection
        /// <summary>
        /// Assigne à une étiquette de section de données, des propriétés graphiques.
        /// </summary>
        /// <param name="_labelContent">Contenu de l'étiquette</param>
        /// <returns>Label : Réussite -> Etiquette affectée. Echec -> Valeur null.</returns>
        private static Label LabelDataSection(string _labelContent)
        {
            string labelContent = _labelContent;

            // Assigne à une étiquette, des propriétés graphiques.
            Label lbl_dataSection = LabelBaseBuild(labelContent, 300, 36);
            lbl_dataSection.FontSize = 16;
            lbl_dataSection.FontWeight = FontWeights.Bold;

            return lbl_dataSection;
        }
        #endregion

        #region PlainData
        /// <summary>
        /// Déchiffre une liste de données.
        /// </summary>
        /// <param name="_cipheredData">Liste de données</param>
        /// <returns>List : Réussite -> Liste des données déchiffrées. Echec -> Valeur null.</returns>
        private static List<string> PlainData(List<byte[]> _cipheredData)
        {
            List<byte[]> cipheredData = _cipheredData;
            List<string> plainData = new List<string>();
            byte[] passwordByte = Encoding.UTF8.GetBytes(VaultDatabase.UserPassword);

            // Déchiffre une liste de données.
            foreach (byte[] element in _cipheredData)
            {
                byte[] decryptedData = AESAlgorithm.DecryptData(passwordByte, element);

                plainData.Add(Encoding.UTF8.GetString(decryptedData).Replace("\0", string.Empty));
            }

            return plainData;
        }
        #endregion

        #region StackPanelBaseBuild
        /// <summary>
        /// Assigne à un tableau de contrôles, des propriétés graphiques basiques.
        /// </summary>
        /// <param name="_stackPanelWidth">Longueur du tableau de contrôles</param>
        /// <param name="_stackPanelHeight">Hauteur du tableau de contrôles</param>
        /// <param name="_stackPanelMarginTop">Marge haute du tableau de contrôles</param>
        /// <returns>Label : Réussite -> Tableau de contrôles affecté. Echec -> Valeur null.</returns>
        private static StackPanel StackPanelBaseBuild(int _stackPanelWidth, int _stackPanelHeight, int _stackPanelMarginTop)
        {
            int stackPanelWidth = _stackPanelWidth;
            int stackPanelHeight = _stackPanelHeight;
            int stackPanelMarginTop = _stackPanelMarginTop;
            StackPanel stp_scrNoteData = new StackPanel();

            // Assigne à un tableau de contrôles, des propriétés graphiques basiques.
            stp_scrNoteData.Width = stackPanelWidth;
            stp_scrNoteData.Height = stackPanelHeight;
            stp_scrNoteData.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            stp_scrNoteData.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            stp_scrNoteData.Margin = new Thickness(0, stackPanelMarginTop, 0, 0);

            return stp_scrNoteData;
        }
        #endregion

        #endregion

    }
}
