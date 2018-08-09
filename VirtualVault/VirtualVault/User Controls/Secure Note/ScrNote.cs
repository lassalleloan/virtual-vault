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

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using VirtualVault.Cryptography;
using VirtualVault.Database;
using VirtualVault.Settings;
namespace VirtualVault.User_Controls.Secure_Note
{
    public static class ScrNote
    {

        #region Global Variables
        private static bool _scrNoteBmk = false;
        private static string _scrNoteCategory = string.Empty;
        private static string _scrNoteContent = string.Empty;
        private static string _scrNoteName = string.Empty;
        private static List<string> _scrNoteData = null;
        #endregion

        #region Properties

        #region ScrNoteBmk
        /// <summary>
        /// Contient le statut favoris de la note sécurisée.
        /// </summary>
        private static bool ScrNoteBmk
        {
            get
            {
                return _scrNoteBmk;
            }
            set
            {
                _scrNoteBmk = value;
            }
        }
        #endregion

        #region ScrNoteCategory
        /// <summary>
        /// Contient la catégorie de la note sécurisée.
        /// </summary>
        private static string ScrNoteCategory
        {
            get
            {
                return _scrNoteCategory;
            }
            set
            {
                _scrNoteCategory = value;
            }
        }
        #endregion

        #region ScrNoteContent
        /// <summary>
        /// Contient le contenu de la note sécurisée.
        /// </summary>
        private static string ScrNoteContent
        {
            get
            {
                return _scrNoteContent;
            }
            set
            {
                _scrNoteContent = value;
            }
        }
        #endregion

        #region ScrNoteName
        /// <summary>
        /// Contient le nom de la note sécurisée.
        /// </summary>
        private static string ScrNoteName
        {
            get
            {
                return _scrNoteName;
            }
            set
            {
                _scrNoteName = value;
            }
        }
        #endregion

        #region ScrNoteData
        /// <summary>
        /// Contient les données de la note sécurisée.
        /// </summary>
        public static List<string> ScrNoteData
        {
            get
            {
                return _scrNoteData;
            }
            set
            {
                _scrNoteData = value;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region GetCipheredData
        /// <summary>
        /// Obtient les données chiffrées.
        /// </summary>
        /// <param name="_data">Données</param>
        /// <returns>Byte[] : Données chiffrées.</returns>
        private static byte[] GetCipheredData(string _data)
        {
            string data = _data;
            byte[] passwordByte = Encoding.UTF8.GetBytes(VaultDatabase.UserPassword);
            byte[] dataByte = Encoding.UTF8.GetBytes(data);

            // Chiffre le contenu.
            byte[] cipheredData = AESAlgorithm.EncryptData(passwordByte, dataByte);

            return cipheredData;
        }
        #endregion

        #region GetScrNoteData
        /// <summary>
        /// Obtient les données saisies.
        /// </summary>
        /// <returns>List : Liste des données de la note sécurisée.</returns>
        public static List<string> GetScrNoteData()
        {
            List<string> scrNoteData = new List<string>();

            // Assigne à un tableau d'octets, les données chiffré d'une note sécurisée.
            byte[] cipheredScrNoteContent = GetCipheredData(ScrNoteContent);

            // Ajoute les données de la note sécurisée.
            scrNoteData.Add(ScrNoteName);
            scrNoteData.Add(Convert.ToBase64String(cipheredScrNoteContent));
            scrNoteData.Add(Convert.ToByte(ScrNoteBmk).ToString());
            scrNoteData.Add(String.Format(Data_ScrNote.Default.DateTimeFormat, DateTime.Now.Date));
            scrNoteData.Add(ScrNoteCategory);

            return scrNoteData;
        }
        #endregion

        #region IsCipheredScrNoteDataEmpty
        /// <summary>
        /// Verifie les données chiffrées de la notes sécurisées.
        /// </summary>
        /// <param name="_scrNoteData">Liste des données chiffrées</param>
        /// <returns>Boolean : True -> Données chiffrées vide. False -> Données chiffrées non vide.</returns>
        public static bool IsCipheredScrNoteDataEmpty(List<string> _scrNoteData)
        {
            List<string> scrNoteData = _scrNoteData;
            bool isCipheredScrNoteDataEmpty = ((scrNoteData[1] == string.Empty) || (scrNoteData[1].Length <= 0));

            // Vérifie les données chiffré de la notes sécurisées.
            if (isCipheredScrNoteDataEmpty)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region IsSaveScrNote
        /// <summary>
        /// Vérifie l'enregistrement des données.
        /// </summary>
        /// <param name="_scrNoteData">Liste des données à enregistrer.</param>
        /// <returns>Boolean : True -> Enregistrement effectué. False -> Enregistrement non effectué.</returns>
        public static bool IsSaveScrNote(List<string> _scrNoteData)
        {
            List<string> scrNoteData = _scrNoteData;
            bool isSaveScrNote = VaultDatabase.SaveScrNote(scrNoteData[0], scrNoteData[1], scrNoteData[2], scrNoteData[3], scrNoteData[3], scrNoteData[4]);

            // Vérifie l'enregistrement des données d'une note sécurisées.
            if (isSaveScrNote)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region IsScrNoteExist
        /// <summary>
        /// Vérifie l'existance d'un nom d'une note sécurisée.
        /// </summary>
        /// <returns>Boolean : True -> Nom déjà existant. False -> Nom inexistant.</returns>
        public static bool IsScrNoteExist()
        {
            bool isScrNoteAlreadyExist = false;

            isScrNoteAlreadyExist = VaultDatabase.CheckScrNoteName(ScrNoteName);

            return isScrNoteAlreadyExist;
        }
        #endregion

        #region IsScrNoteDataNull
        /// <summary>
        /// Vérifie les données d'une note sécurisées.
        /// </summary>
        /// <returns>Boolean : True -> Données vide. False -> Données saisies.</returns>
        public static bool IsScrNoteDataEmpty()
        {
            bool isScrNoteNameLength_Error = ((ScrNoteName == null) || (ScrNoteName.Length <= 0));
            bool isScrNoteContentLength_Error = ((ScrNoteContent == null) || (ScrNoteContent.Length <= 0));

            // Vérifie les données de la note sécurisées.
            if (isScrNoteNameLength_Error || isScrNoteContentLength_Error)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region SaveInputs
        /// <summary>
        /// Assigne à des propriétés, des entrées utilisateurs.
        /// </summary>
        /// <param name="_scrNoteName">Nom de la note sécurisée</param>
        /// <param name="_scrNoteContent">Contenu de la note sécurisée</param>
        /// <param name="_scrNoteCategory">Catégorie de la note sécurisée</param>
        /// <param name="_scrNoteBmk">Statut favoris de la note sécurisée</param>
        public static void SaveInputs(string _scrNoteName, string _scrNoteContent, string _scrNoteCategory, bool _scrNoteBmk)
        {
            ScrNoteName = _scrNoteName;
            ScrNoteContent = _scrNoteContent;
            ScrNoteCategory = _scrNoteCategory;
            ScrNoteBmk = _scrNoteBmk;
        }
        #endregion

        #region UpdateDateAndCtgControls
        /// <summary>
        /// Met à jour des contrôles de l'interface de notes sécurisées.
        /// </summary>
        /// <param name="_lbl_crtDate">Etiquette</param>
        /// <param name="_lbl_chgDate">Etiquette</param>
        /// <param name="_cbo_category">Liste déroulante</param>
        public static void UpdateDateAndCtgControls(Label _lbl_crtDate, Label _lbl_chgDate, ComboBox _cbo_category)
        {
            // Assigne les contrôles des dates courantes et des noms de catégories.
            _lbl_crtDate.Content = DateTime.Now.Date.ToShortDateString();
            _lbl_chgDate.Content = DateTime.Now.Date.ToShortDateString();
            _cbo_category.ItemsSource = VaultDatabase.GetCategoryNameList();
            _cbo_category.SelectedIndex = 0;
        }
        #endregion

        #endregion

    }
}
