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

namespace VirtualVault.User_Controls.Category
{
    /// <summary>
    /// Logique d'interaction pour usc_category.xaml
    /// </summary>
    public partial class usc_category : UserControl
    {

        #region Global Variables
        private static string _oldCategoryName = string.Empty;
        #endregion

        #region Properties

        #region OldCategoryName
        // Contient l'ancien nom d'une catégorie.
        private static string OldCategoryName
        {
            get
            {
                return _oldCategoryName;
            }
            set
            {
                _oldCategoryName = value;
            }
        }
        #endregion

        #endregion

        #region Constructors
        
        #region usc_category
        /// <summary>
        /// Initialise une nouvelle instance de la classe usc_category.
        /// </summary>
        public usc_category(string _categoryName = "", bool _isCategoryUpdate = false)
        {
            InitializeComponent();

            // Change le nom de la fenêtre de l'appication.
            Switcher.ChangeWindowTitle(Data_Category.Default.CategoryTitle);

            // Assigne à des propriétés, les statuts des actions sur un nom de catégorie.
            txt_name.Text = _categoryName;
            OldCategoryName = _categoryName;
            Category.IsCategoryUpdate = _isCategoryUpdate;
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
            usc_home usc_homeCategory = new usc_home();
            Switcher.Switch(usc_homeCategory);
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
            Category.SaveIputs(txt_name.Text);

            // Vérifie la longueur du nom de la catégorie.
            if (Category.IsCtgNameLength_Error())
            {
                lbl_message.Content = Data_Category.Default.IsCtgNameLength_Error;
                return;
            }

            // Vérifie l'existance du nom de catégorie.
            if (Category.IsCategoryExist())
            {
                lbl_message.Content = Data_Category.Default.IsCategoryExist;
                return;
            }

            // Vérifie le statut de l'action de modifier un nom de catégorie.
            if (Category.IsCategoryUpdate)
            {
                // Vérifie la mise à jour du nom de catégorie.
                if (VaultDatabase.UpdateCategory(OldCategoryName, txt_name.Text))
                {
                    // Affiche l'interface d'accueil.
                    usc_home usc_homeCategory = new usc_home();
                    Switcher.Switch(usc_homeCategory);
                    return;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.UpdateCategory);
                    return;
                }
            }

            // Vérifie l'enregistrement du nom de catégorie.
            if (VaultDatabase.SaveCategory(txt_name.Text))
            {
                // Affiche l'interface d'accueil.
                usc_home usc_homeCategory = new usc_home();
                Switcher.Switch(usc_homeCategory);
                return;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.SaveCategory);
                return;
            }
        }
        #endregion

        #region usc_category_KeyUp
        /// <summary>
        /// Action lors de la remontée d'une touche du clavier.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void usc_category_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            bool isKeyEnterPess = (e.Key == Key.Enter);

            // Vérifie la remontée de la touche Enter.
            if (isKeyEnterPess)
            {
                cmd_save_Click(sender, e);
            }
        }
        #endregion

        #region usc_category_Loaded
        /// <summary>
        /// Action lors du chargement de l'interface.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void usc_category_Loaded(object sender, RoutedEventArgs e)
        {
            // Assigne le focus au champs de saisie du nom d'utilisateur.
            txt_name.Focus();
        }
        #endregion
        
        #endregion

    }
}
