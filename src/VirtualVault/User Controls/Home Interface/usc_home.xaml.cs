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
using VirtualVault.User_Controls.Authentication;
using VirtualVault.User_Controls.Authentication_Card;
using VirtualVault.User_Controls.Category;
using VirtualVault.User_Controls.Change_Password;
using VirtualVault.User_Controls.Main;
using VirtualVault.User_Controls.Password_Generator;
using VirtualVault.User_Controls.Rename_Vault;
using VirtualVault.User_Controls.Secure_Note;
#endregion

namespace VirtualVault.User_Controls.Home_Interface
{
    /// <summary>
    /// Logique d'interaction pour usc_home.xaml
    /// </summary>
    public partial class usc_home : UserControl
    {

        #region Constructors

        #region usc_home
        /// <summary>
        /// Initialise une nouvelle instance de la classe usc_home.
        /// </summary>
        public usc_home(string _categoryName = "Tous les éléments", string _bookmark = "")
        {
            InitializeComponent();

            Switcher.ChangeWindowTitle(Data_Home.Default.HomeTitle);

            string categoryName = _categoryName;
            string bookmark = _bookmark;
            int allItermNbr = Home.GetAllItemNbr(categoryName, bookmark);

            if (bookmark != string.Empty)
            {
                lbl_bmk.FontWeight = FontWeights.Bold;
            }

            lbl_vltName.Content = VaultDatabase.GetVaultName();
            lbl_itemNbr.Content = allItermNbr + Data_Home.Default.ItemLegendNbr;

            // Ajoute des contrôles pour visualiser des catégries.
            Home.AddCategoryControls(stp_category, categoryName);

            if (allItermNbr == 0)
            {
                // Ajoute un contrôle lors du premier lancement de l'application.
                Label lbl_anyDataControl = Home.AddAnyDataControl();
                stp_allData.Children.Add(lbl_anyDataControl);
            }
            else
            {
                // Ajoute des contrôles pour visualiser des données.
                Home.AddACardsControls(stp_allData, categoryName, bookmark);
                Home.AddScrNoteDataControls(stp_allData, categoryName, bookmark);
            }
        }
        #endregion

        #endregion

        #region Events

        #region cmd_addACard_Click
        /// <summary>
        /// Action lors clic sur le bouton "cmd_addACard". 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_addACard_Click(object sender, RoutedEventArgs e)
        {
            usc_authenCard usc_authenCardHome = new usc_authenCard();
            Switcher.Switch(usc_authenCardHome);
        }
        #endregion

        #region cmd_addCategory_Click
        /// <summary>
        /// Action lors clic sur le bouton "cmd_addCtg". 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_addCategory_Click(object sender, RoutedEventArgs e)
        {
            usc_category usc_categoryHome = new usc_category();
            Switcher.Switch(usc_categoryHome);
        }
        #endregion

        #region cmd_addScrNote_Click
        /// <summary>
        /// Action lors clic sur le bouton "cmd_addNote". 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_addScrNote_Click(object sender, RoutedEventArgs e)
        {
            usc_scrNote usc_scrNoteHome = new usc_scrNote();
            Switcher.Switch(usc_scrNoteHome);
        }
        #endregion

        #region lbl_bmk_MouseLeftButtonUp
        /// <summary>
        /// Action lors du clic sur le label "lbl_updPwd".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_bmk_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string bookmarkByte = 1.ToString();

            // Affiche l'interface d'accueil aves les éléments favoris.
            usc_home usc_bookmarkHome = new usc_home(_bookmark: bookmarkByte);
            Switcher.Switch(usc_bookmarkHome);
            return;
        }
        #endregion

        #region lbl_chgPwd_MouseLeftButtonUp
        /// <summary>
        /// Action lors du clic sur le label "lbl_updPwd".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_chgPwd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            usc_chgPwd usc_chgPwdHome = new usc_chgPwd();
            Switcher.Switch(usc_chgPwdHome);
        }
        #endregion

        #region lbl_deconnection_MouseLeftButtonUp
        /// <summary>
        /// Action lors du clic sur le label "lbl_deconnection".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_deconnection_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            usc_authen usc_authenHome = new usc_authen();
            Switcher.Switch(usc_authenHome);
        }
        #endregion

        #region lbl_pwdGen_MouseLeftButtonUp
        /// <summary>
        /// Action lors du clic sur le label "lbl_pwdGen".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_pwdGen_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            wdw_pwdGen wdw_pwdGenHome = new wdw_pwdGen();
        }
        #endregion

        #region lbl_vltName_MouseRightButtonUp
        /// <summary>
        /// Action lors du clic droit sur le label "lbl_vltName".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_vltName_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (lbl_vltName.ContextMenu != null)
            {
                lbl_vltName.ContextMenu.PlacementTarget = lbl_vltName;
                lbl_vltName.ContextMenu.IsOpen = true;
            }
        }
        #endregion

        #region MenuItem_RnmVault_Click
        /// <summary>
        /// Action lors du clic sur le menuItem du contextMenu du label "lbl_vltName".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_RnmVault_Click(object sender, RoutedEventArgs e)
        {
            usc_rnmVault usc_rnmVaultHome = new usc_rnmVault(lbl_vltName.Content.ToString());
            Switcher.Switch(usc_rnmVaultHome);
        }
        #endregion

        #endregion
    }
}
