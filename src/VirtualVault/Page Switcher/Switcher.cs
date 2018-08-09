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
using System.Windows.Controls;
#endregion

namespace VirtualVault.Page_Switcher
{
    public static class Switcher
    {
        #region Global Variables
        public static wdw_pageSwitcher pageSwitcher;
        #endregion

        #region Methods

        #region ChangeWindowTitle
        /// <summary>
        /// Change le nom de la fenêtre de l'application.
        /// </summary>
        /// <param name="_userControlTitle">Nom d'une interface à afficher</param>
        public static void ChangeWindowTitle(string _userControlTitle)
        {
            string userControlTitle = _userControlTitle;
            Switcher.pageSwitcher.Title = userControlTitle;
        }
        #endregion

        #region Switch
        /// <summary>
        /// Change la page de la fenêtre.
        /// </summary>
        /// <param name="_newPage">Nouvelle page à insérer</param>
        public static void Switch(UserControl _newPage)
        {
            UserControl newPage = _newPage;
            pageSwitcher.Navigate(newPage);
        }
        #endregion

        #endregion

    }
}
