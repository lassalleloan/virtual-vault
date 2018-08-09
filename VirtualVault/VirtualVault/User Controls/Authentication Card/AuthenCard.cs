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
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;
using VirtualVault.Cryptography;
using VirtualVault.Database;
using VirtualVault.Settings;
#endregion

namespace VirtualVault.User_Controls.Authentication_Card
{
    public static class AuthenCard
    {

        #region Global Variables
        private static bool _aCardBmk = false;
        private static string _aCardCategory = string.Empty;
        private static string _aCardCpxFactor = string.Empty;
        private static List<string> _aCardData = null;
        private static string _aCardName = string.Empty;
        private static string _aCardPassword = string.Empty;
        private static string _aCardScrNote = string.Empty;
        private static string _aCardShc = string.Empty;
        private static string _aCardUsername = string.Empty;
        #endregion

        #region Properties

        #region ACardBmk
        /// <summary>
        /// Contient le statut favoris de la fiche d'authentification.
        /// </summary>
        private static bool ACardBmk
        {
            get
            {
                return _aCardBmk;
            }
            set
            {
                _aCardBmk = value;
            }
        }
        #endregion

        #region ACardCategory
        /// <summary>
        /// Contient la catégorie de la fiche d'authentification.
        /// </summary>
        private static string ACardCategory
        {
            get
            {
                return _aCardCategory;
            }
            set
            {
                _aCardCategory = value;
            }
        }
        #endregion

        #region ACardCpxFactor
        /// <summary>
        /// Contient le facteur de complexité de la fiche d'authentification.
        /// </summary>
        private static string ACardCpxFactor
        {
            get
            {
                return _aCardCpxFactor;
            }
            set
            {
                _aCardCpxFactor = value;
            }
        }
        #endregion

        #region ACardData
        public static List<string> ACardData
        {
            get
            {
                return _aCardData;
            }
            set
            {
                _aCardData = value;
            }
        }
        #endregion

        #region ACardName
        /// <summary>
        /// Contient le nom de la fiche d'authentification.
        /// </summary>
        private static string ACardName
        {
            get
            {
                return _aCardName;
            }
            set
            {
                _aCardName = value;
            }
        }
        #endregion

        #region ACardPassword
        /// <summary>
        /// Contient le mot de passe de la fiche d'authentification.
        /// </summary>
        private static string ACardPassword
        {
            get
            {
                return _aCardPassword;
            }
            set
            {
                _aCardPassword = value;
            }
        }
        #endregion

        #region ACardScrNote
        /// <summary>
        /// Contient la note sécurisée de la fiche d'authentification.
        /// </summary>
        private static string ACardScrNote
        {
            get
            {
                return _aCardScrNote;
            }
            set
            {
                if (value == string.Empty)
                {
                    value = " ";
                }
                _aCardScrNote = value;
            }
        }
        #endregion

        #region ACardShc
        /// <summary>
        /// Contient le raccourci de la fiche d'authentification.
        /// </summary>
        private static string ACardShc
        {
            get
            {
                return _aCardShc;
            }
            set
            {
                if (value == string.Empty)
                {
                    value = " ";
                }
                _aCardShc = value;
            }
        }
        #endregion

        #region ACardUsername
        /// <summary>
        /// Contient le nom d'utilisateur de la fiche d'authentification.
        /// </summary>
        private static string ACardUsername
        {
            get
            {
                return _aCardUsername;
            }
            set
            {
                _aCardUsername = value;
            }
        }
        #endregion

        #endregion

        #region Methods

        #region GetAuthenCardData
        /// <summary>
        /// Obtient une liste des données saisies.
        /// </summary>
        /// <returns>List : Liste des données de la fiche d'authentification.</returns>
        public static List<string> GetAuthenCardData()
        {
            List<string> aCardData = new List<string>();

            // Assigne à un tableau d'octets, les données chiffrées d'une fiche d'authentification.
            byte[] cipheredACardUsername = GetCipheredData(ACardUsername);
            byte[] cipheredACardPassword = GetCipheredData(ACardPassword);
            byte[] cipheredACardScrnote = GetCipheredData(ACardScrNote);

            // Ajoute les données de la fiche d'authentification.
            aCardData.Add(ACardName);
            aCardData.Add(ACardShc);
            aCardData.Add(Convert.ToBase64String(cipheredACardUsername));
            aCardData.Add(Convert.ToBase64String(cipheredACardPassword));
            aCardData.Add(ACardCpxFactor);
            aCardData.Add(Convert.ToBase64String(cipheredACardScrnote));
            aCardData.Add(Convert.ToByte(ACardBmk).ToString());
            aCardData.Add(String.Format(Data_AuthenCard.Default.DateTimeFormat, DateTime.Now.Date));
            aCardData.Add(ACardCategory);

            return aCardData;
        }
        #endregion

        #region GetCipheredData
        /// <summary>
        /// Obtient des données chiffrées.
        /// </summary>
        /// <param name="_data">Données à chiffrer</param>
        /// <returns>Byte[] : Donnée chiffrées.</returns>
        private static byte[] GetCipheredData(string _data)
        {
            string data = _data;
            byte[] passwordByte = Encoding.UTF8.GetBytes(VaultDatabase.UserPassword);
            byte[] dataByte = Encoding.UTF8.GetBytes(data);

            // Chiffre les données.
            byte[] cipheredData = AESAlgorithm.EncryptData(passwordByte, dataByte);

            return cipheredData;
        }
        #endregion

        #region GetCpxPourcent
        /// <summary>
        /// Obtient le poucentage de complexité du mot de passe.
        /// </summary>
        /// <param name="_password">Mot de passe</param>
        /// <param name="_cpxFactor">Etiquette du pourcentage</param>
        /// <returns>Integer : Réussite -> Pourcentage de complexité. Echec -> Valeur 0.</returns>
        private static int GetCpxPourcent(string _password)
        {
            const int RAPPORT_CPX_VALUE = 5;
            const int FACTOR_CPX_VALUE = 20;

            string textBoxText = _password;
            int multipleFactorCpx = 0;
            int cpxFactorLength = 0;
            int cpxFactorCharacters = 0;

            List<string> regexList = new List<string>();
            int cpxFactorValue = 0;
            Regex regexCheckString = null;

            // Ajoute les expressions régulières à utiliser.
            regexList.Add(Data_PwdGen.Default.regexUpper);
            regexList.Add(Data_PwdGen.Default.regexLower);
            regexList.Add(Data_PwdGen.Default.regexNumbers);
            regexList.Add(Data_PwdGen.Default.regexSymbols);

            // Vérifie la correspondance entre deux une chaîne de caractère. 
            foreach (string regex in regexList)
            {
                regexCheckString = new Regex(@"[" + regex + "]{1}");

                if (regexCheckString.IsMatch(textBoxText))
                {
                    multipleFactorCpx++;
                }
            }

            // Calcul du pourcentage de complexité.
            cpxFactorCharacters = (textBoxText.Length / RAPPORT_CPX_VALUE);
            cpxFactorLength = (FACTOR_CPX_VALUE * multipleFactorCpx);
            cpxFactorValue = cpxFactorCharacters + cpxFactorLength;

            return cpxFactorValue;
        }
        #endregion

        #region GetLabelColor
        /// <summary>
        /// Obtient une couleurà assigner à deux étiquettes.
        /// </summary>
        /// <param name="_textBoxContent">Pourcentage de complexité</param>
        /// <param name="_lbl_cpxFactor">Etiquette</param>
        /// <param name="_lbl_cpxCmt">Etiquette</param>
        public static void GetLabelColor(string _textBoxContent, Label _lbl_cpxFactor, Label _lbl_cpxCmt)
        {
            const int POURCENT_COLOR = 50;
            int cpxFactorValue = GetCpxPourcent(_textBoxContent);
            bool isCpxCmtValueInfPctg = (cpxFactorValue < POURCENT_COLOR);

            // Vérifie la valeur du pourcentage d'efficacité.
            if (isCpxCmtValueInfPctg)
            {
                // Assigne à une étiquette, une couleur.
                _lbl_cpxFactor.Foreground = new SolidColorBrush(Colors.Red);
                _lbl_cpxCmt.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                // Assigne à une étiquette, une couleur.
                _lbl_cpxFactor.Foreground = new SolidColorBrush(Colors.Green);
                _lbl_cpxCmt.Foreground = new SolidColorBrush(Colors.Green);
            }

            // assigne au contenun d'une étiquette, la valeur de la variable.
            _lbl_cpxFactor.Content = cpxFactorValue;
        }
        #endregion

        #region IsAuthenCardExist
        /// <summary>
        /// Vérifie l'existance du nom d'une fiche d'authentification
        /// </summary>
        /// <returns>Boolean : True -> Nom déjà existant. False -> Nom inexistant.</returns>
        public static bool IsAuthenCardExist()
        {
            bool isACardAlreadyExist = false;

            isACardAlreadyExist = VaultDatabase.CheckAuthenCard(ACardName);

            return isACardAlreadyExist;
        }
        #endregion

        #region IsAuthenCardDataEmpty
        /// <summary>
        /// Vérifie les données de la fiche d'authentification.
        /// </summary>
        /// <returns>Boolean : True -> Données vide. False -> Données saisies.</returns>
        public static bool IsAuthenCardDataEmpty()
        {
            bool isACardNameLength_Error = ((ACardName == null) || (ACardName.Length <= 0));
            bool isACardUsnLength_Error = ((ACardUsername == null) || (ACardUsername.Length <= 0));
            bool isACardPwdLength_Error = ((ACardPassword == null) || (ACardPassword.Length <= 0));

            // Vérifie les données de la fiche d'authentification.
            if (isACardNameLength_Error || isACardUsnLength_Error || isACardPwdLength_Error)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region IsCipheredACardDataEmpty
        /// <summary>
        /// Verifie la longueur des données chiffrées d'une fiche d'authentification.
        /// </summary>
        /// <param name="_aCardData">Liste des données chiffrées</param>
        /// <returns>Boolean : True -> Données chiffrées vide. False -> Données chiffrées non vide.</returns>
        public static bool IsCipheredACardDataEmpty(List<string> _aCardData)
        {
            List<string> aCardData = _aCardData;
            bool isCipheredACardUsnEmpty = ((aCardData[2] == string.Empty) || (aCardData[2].Length <= 0));
            bool isCipheredACardPwdEmpty = ((aCardData[3] == string.Empty) || (aCardData[3].Length <= 0));
            bool isCipheredACardScrNoteEmpty = ((aCardData[4] == string.Empty) || (aCardData[4].Length <= 0));

            // Verifie les données chiffrées de la fiche d'authentification.
            if (isCipheredACardUsnEmpty || isCipheredACardPwdEmpty || isCipheredACardScrNoteEmpty)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region IsSaveAuthenCard
        /// <summary>
        /// Vérifie l'enregistrement des données d'une fiche d'authentification.
        /// </summary>
        /// <param name="_aCardData">Liste des données à enregistrer.</param>
        /// <returns>Boolean : True -> Enregistrement effectué. False -> Enregistrement non effectué.</returns>
        public static bool IsSaveAuthenCard(List<string> _aCardData)
        {
            List<string> aCardData = _aCardData;
            bool isSaveAthenCardData = VaultDatabase.SaveAuthenCard(aCardData[0], aCardData[1], aCardData[2], aCardData[3], aCardData[4],
                                                                    aCardData[5], aCardData[6], aCardData[7], aCardData[7], aCardData[8]);

            // Vérifie l'enregistrement des données d'une fiche d'authentification.
            if (isSaveAthenCardData)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region SaveIputs
        /// <summary>
        /// Assigne à des propriétés, des entrées utilisateurs.
        /// </summary>
        /// <param name="_aCardName">Nom de la fiche d'authentification</param>
        /// <param name="_aCardShc">Raccourci de la fiche d'authentification</param>
        /// <param name="_aCardUsername">Nom d'utilisateur de la fiche d'authentification</param>
        /// <param name="_aCardPassword">Mot de passe de la fiche d'authentification</param>
        /// <param name="_aCardScrNote">Note de la fiche d'authentification</param>
        public static void SaveIputs(string _aCardName, string _aCardShc, string _aCardUsername, string _aCardPassword,
                                    string _aCardCpxFactor, string _aCardScrNote, string _aCardCategory, bool _aCardBmk)
        {
            ACardName = _aCardName;
            ACardShc = _aCardShc;
            ACardUsername = _aCardUsername;
            ACardPassword = _aCardPassword;
            ACardCpxFactor = _aCardCpxFactor;
            ACardScrNote = _aCardScrNote;
            ACardCategory = _aCardCategory;
            ACardBmk = _aCardBmk;
        }
        #endregion

        #region UpdateDateAndCtgControls
        /// <summary>
        /// Met à jour des contrôles de l'interface de fiche d'authentification.
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
