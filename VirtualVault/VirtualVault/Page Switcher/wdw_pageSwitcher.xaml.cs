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
using VirtualVault.User_Controls.Authentication;
#endregion

namespace VirtualVault.Page_Switcher
{
    /// <summary>
    /// Logique d'interaction pour wdw_pageSwitcher.xaml
    /// </summary>
    public partial class wdw_pageSwitcher : Window
    {
        #region Constructors
        /// <summary>
        /// Initialise une nouvelle instance de la classe wdw_pageSwitcher.
        /// </summary>
        public wdw_pageSwitcher()
        {
            InitializeComponent();

            Switcher.pageSwitcher = this;

            // Affiche l'interface d'authentification.
            usc_authen authenPageSwitcher = new usc_authen();
            Switcher.Switch(authenPageSwitcher);
        }
        #endregion

        #region Events

        #region wdw_pageSwitcher_Closing
        /// <summary>
        /// Action lors de la fermeture de la fenêtre principale "wdw_nfc_switcher".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wdw_pageSwitcher_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Arrêt de l'application complète.
            Application.Current.Shutdown();
            return;
        }
        #endregion

        #endregion

        #region Methods

        #region Navigate
        /// <summary>
        /// Navigue entre les différentes interfaces.
        /// </summary>
        /// <param name="_nextPage">Page suivante.</param>
        public void Navigate(UserControl _nextPage)
        {
            UserControl nextPage = _nextPage;

            // Assigne le contenu de la fenêtre de l'application.
            this.Content = nextPage;
        }
        #endregion

        #endregion

    }
}
