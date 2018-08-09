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
using VirtualVault.User_Controls.Category;
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

        #region Methods

        #region AddCategoryControls
        /// <summary>s
        /// Ajoute des contrôles pour visualiser des catégries.
        /// </summary>
        /// <param name="_stackPanel">Tableau de contrôles</param>
        public static void AddCategoryControls(StackPanel _stackPanel, string _categoryNameSelected)
        {
            string categoryNameSelected = _categoryNameSelected;

            // Assigne à une liste les noms ds catégories.
            List<string> categoriesName = VaultDatabase.GetCategoryNameList();

            // Vérifie le contenu de la liste des noms de catégories.
            if (categoriesName != null)
            {
                foreach (string element in categoriesName)
                {
                    // Assigne à une étiquette, des propriétés graphiques basiques.
                    Label lbl_categoryName = LabelBaseBuild(element, 300, 26);

                    // Vérifie et applique une propriété graphique à la catégorie sélectionné
                    if ((categoryNameSelected != string.Empty) && (element == categoryNameSelected))
                    {
                        lbl_categoryName.FontWeight = FontWeights.Bold;
                    }

                    _stackPanel.Children.Add(lbl_categoryName);

                    lbl_categoryName.MouseLeftButtonUp += new MouseButtonEventHandler(lbl_ctgName_MouseLeftButtonUp);

                    if (element != categoriesName[0])
                    {
                        lbl_categoryName.MouseRightButtonUp += new MouseButtonEventHandler(lbl_ctgName_MouseRightButtonUp);

                        // Assigne à une étiquette, un menu contextuel.
                        ContextMenu lbl_categoryNameContextMenu = new ContextMenu();
                        lbl_categoryName.ContextMenu = lbl_categoryNameContextMenu;

                        // Assigne à un menu contextuel, un menu d'élément.
                        MenuItem lbl_MenuItemRename = new MenuItem();
                        lbl_MenuItemRename.Header = Data_Home.Default.AddCategoryControls_Rename + lbl_categoryName.Content.ToString();
                        lbl_MenuItemRename.Click += new RoutedEventHandler(lbl_MenuItemRename_Click);
                        lbl_categoryNameContextMenu.Items.Add(lbl_MenuItemRename);

                        // Assigne à un menu contextuel, un menu d'élément.
                        MenuItem lbl_MenuItemDelete = new MenuItem();
                        lbl_MenuItemDelete.Header = Data_Home.Default.AddCategoryControls_Delete + lbl_categoryName.Content.ToString();
                        lbl_MenuItemDelete.Click += new RoutedEventHandler(lbl_MenuItemDelete_Click);
                        lbl_categoryNameContextMenu.Items.Add(lbl_MenuItemDelete);
                    }
                }
            }
        }
        #endregion

        #region lbl_MenuItemDelete_Click
        /// <summary>
        /// Action lors du clic sur le menuItem supprimer du contextMenu du label "lbl_stpCategory".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void lbl_MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedMenuItem = sender as MenuItem;
            string categoryName = clickedMenuItem.Header.ToString().Substring(10);

            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(Data_Home.Default.lbl_MenuItemDelete_Click_Message_Category,
                                                                                                Data_Home.Default.lbl_MenuItemDelete_Click_Title_Category,
                                                                                                System.Windows.Forms.MessageBoxButtons.YesNo);

            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                // Vérifie l'exécution de la suppression du de la catégorie.
                if (VaultDatabase.DeleteCategory(categoryName))
                {
                    // Affiche l'interface d'accueil.
                    usc_home usc_homeHome = new usc_home();
                    Switcher.Switch(usc_homeHome);
                    return;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.DeleteCategory);
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

        #region lbl_MenuItemRename_Click
        /// <summary>
        /// Action lors du clic sur le menuItem renommer du contextMenu du label "lbl_stpCategory".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void lbl_MenuItemRename_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedMenuItem = sender as MenuItem;
            string categoryName = clickedMenuItem.Header.ToString().Substring(9);

            // Affiche l'interface pour renommer une catégorie.
            usc_category usc_categoryMain = new usc_category(categoryName, true);
            Switcher.Switch(usc_categoryMain);
            return;
        }
        #endregion

        #region lbl_ctgName_MouseRightButtonUp
        /// <summary>
        /// Action lors du clic droit sur le label "lbl_stpCategory".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void lbl_ctgName_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Label lbl_clickedLabel = sender as Label;

            // Vérifie le contenu du menu contextuel du label "lbl_category".
            if (lbl_clickedLabel.ContextMenu != null)
            {
                lbl_clickedLabel.ContextMenu.PlacementTarget = lbl_clickedLabel;
                lbl_clickedLabel.ContextMenu.IsOpen = true;
            }
        }
        #endregion

        #region lbl_ctgName_MouseLeftButtonUp
        /// <summary>
        /// Action lors du clic sur le label "lbl_category".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void lbl_ctgName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Label lbl_clickedLabel = sender as Label;
            string clickedLabelNamed = lbl_clickedLabel.Content.ToString();

            // Affiche l'interface d'accueil.
            usc_home usc_categoryItemHome = new usc_home(_categoryName: clickedLabelNamed);
            Switcher.Switch(usc_categoryItemHome);
            return;
        }
        #endregion

        #endregion

    }
}
