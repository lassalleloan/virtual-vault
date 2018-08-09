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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;
using VirtualVault.Settings;
#endregion

namespace VirtualVault.User_Controls.Password_Generator
{
    public static class PwdGen
    {

        #region Global Variables
        private static bool _isCheckLowerCase = false;
        private static bool _isCheckNumber = false;
        private static bool _isCheckSymbol = false;
        private static bool _isCheckUpperCase = false;
        private static int _passwordLength = 0;
        #endregion

        #region Properties

        #region IsCheckLowerCase
        /// <summary>
        /// Contient le statut d'une case à cocher.
        /// </summary>
        private static bool IsCheckLowerCase
        {
            get
            {
                return _isCheckLowerCase;
            }
            set
            {
                _isCheckLowerCase = value;
            }
        }
        #endregion

        #region IsCheckNumber
        /// <summary>
        /// Contient le statut d'une case à cocher.
        /// </summary>
        private static bool IsCheckNumber
        {
            get
            {
                return _isCheckNumber;
            }
            set
            {
                _isCheckNumber = value;
            }
        }
        #endregion

        #region IsCheckSymbol
        /// <summary>
        /// Contient le statut d'une case à cocher.
        /// </summary>
        private static bool IsCheckSymbol
        {
            get
            {
                return _isCheckSymbol;
            }
            set
            {
                _isCheckSymbol = value;
            }
        }
        #endregion

        #region IsCheckUpperCase
        /// <summary>
        /// Contient le statut d'une case à cocher.
        /// </summary>
        private static bool IsCheckUpperCase
        {
            get
            {
                return _isCheckUpperCase;
            }
            set
            {
                _isCheckUpperCase = value;
            }
        }
        #endregion

        #region PasswordLength
        /// <summary>
        /// Contient la longueur du mot de passe à générer.
        /// </summary>
        private static int PasswordLength
        {
            get
            {
                return _passwordLength;
            }
            set
            {
                _passwordLength = value;
            }
        }
        #endregion

        #endregion

        #region Methods

        #region ChangeColorByValue
        /// <summary>
        /// Change la couleur d'une bar de progression par rapport à sa valeur.
        /// </summary>
        /// <param name="_progressBar">Bar de progression</param>
        /// <param name="_label">Etiquette</param>
        public static void ChangeColorByValue(ProgressBar _progressBar, Label _label)
        {
            // Affecte une couleur à la bar de progression en fonction de sa valeur.
            GetProgressBarColor(0, 40, _progressBar, Data_PwdGen.Default.cpxCmtVeryBad, Colors.OrangeRed, _label);
            GetProgressBarColor(40, 60, _progressBar, Data_PwdGen.Default.cpxCmtBad, Colors.DarkOrange, _label);
            GetProgressBarColor(60, 80, _progressBar, Data_PwdGen.Default.cpxCmtGood, Colors.GreenYellow, _label);
            GetProgressBarColor(80, 100, _progressBar, Data_PwdGen.Default.cpxCmtFantastic, Colors.DarkGreen, _label);
        }
        #endregion

        #region IsBetween
        /// <summary>
        /// Vérifie si une valeur se situe entre deux valeurs extrêmes.
        /// </summary>
        /// <param name="_minimum">Valeur minimale</param>
        /// <param name="_compareValue">Valeur à comparer</param>
        /// <param name="_maximum">Valeur maximale</param>
        /// <returns>Boolean : True -> valeur comprise entre les deux extrêmes. False -> Valeur en dehorsdes extrêmes.</returns>
        private static bool IsBetween(int _minimum, int _compareValue, int _maximum)
        {
            int minimum = _minimum;
            int compareValue = _compareValue;
            int maximum = _maximum;
            bool isCompareValueSupMini = compareValue > minimum;
            bool isCompareValueInfMax = (compareValue <= maximum);

            return (isCompareValueSupMini && isCompareValueInfMax);
        }
        #endregion

        #region IsCheckedAnyCase
        /// <summary>
        /// Vérifie le cochage de cases à cocher.
        /// </summary>
        /// <returns>Boolean : True -> Aucune case cochée. False -> Au moins une case cochée.</returns>
        public static bool IsCheckedAnyCase()
        {
            // Vérifie le cochage de case à cocher.
            if (!IsCheckUpperCase && !IsCheckLowerCase && !IsCheckNumber && !IsCheckSymbol)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region GetMultipleCpxFactor
        /// <summary>
        /// Obtient le multiple du facteur de complexité.
        /// </summary>
        /// <param name="_template">Chaîne de caractères temporaire</param>
        /// <param name="_regexList">Liste des expressions régulière choisi</param>
        /// <param name="_isCheckCheckBox">Statut de la case à cocher</param>
        /// <param name="_chosenCharacters">Caratères choisi</param>
        /// <param name="_regexString">Expression régulière correspondante</param>
        /// <returns>Integer : Réussite -> Valeur 1. Echec -> Valeur 0.</returns>
        private static int GetMultipleCpxFactor(StringBuilder _template, List<string> _regexList, bool _isCheckCheckBox, string _chosenCharacters, string _regexString)
        {
            StringBuilder template = _template;
            List<string> regexList = _regexList;
            bool isCheckCheckBox = _isCheckCheckBox;
            string chosenCharacters = _chosenCharacters;
            string regexString = _regexString;

            // Vérifie le statut de la case à cocher.
            if (isCheckCheckBox)
            {
                // Ajoute les caractères choisis et les expressions régulières.
                template.Append(chosenCharacters);
                regexList.Add(regexString);

                return 1;
            }

            return 0;
        }
        #endregion

        #region GetProgressBarColor
        /// <summary>
        /// Obtient la couleur de la bar de progression en fonction de sa valeur.
        /// </summary>
        /// <param name="_minimumValue">Valeur minimum</param>
        /// <param name="_maximumValue">Valeur maxmimu</param>
        /// <param name="_progressBar">Bar de progression</param>
        /// <param name="_contentLabel">Contenu de l'étiquette</param>
        /// <param name="_progressBarColor">Bar de progression</param>
        /// <param name="_label">Etiquette</param>
        private static void GetProgressBarColor(int _minimumValue, int _maximumValue, ProgressBar _progressBar, string _contentLabel, Color _progressBarColor, Label _label)
        {
            int minimumValue = _minimumValue;
            int progressBarValue = Convert.ToInt32(_progressBar.Value);
            int maximumValue = _maximumValue;
            ProgressBar progressBar = _progressBar;
            string contentLabel = _contentLabel;
            Color progressBarColor = _progressBarColor;
            Label label = _label;

            // Vérifie si une valeur se situe entre deux valeurs extrêmes.
            if (IsBetween(minimumValue, progressBarValue, maximumValue))
            {
                // Assigne à une bar de progression une couleur et à une étiquette un commentaire.
                progressBar.Foreground = new SolidColorBrush(progressBarColor);
                label.Content = contentLabel;
            }
        }
        #endregion

        #region GetRandomPwd
        /// <summary>
        /// Obtient un mot de passe aléatoire.
        /// </summary>
        /// <param name="_progressBar">Bar de progression</param>
        /// <returns>String : Réusite -> Mot de passe aléatoire. Echec -> Valeur null.</returns>
        public static string GetRandomPwd(ProgressBar _progressBar)
        {
            try
            {
                Random random = new Random((int)DateTime.Now.Ticks);

                List<string> regexList = new List<string>();
                char[] password = new char[PasswordLength];
                Regex regexCheckString = null;
                string passwordString = null;
                bool regexMatch = true;

                // Assigne une chaîne de caractères.
                string template = GetStringTemplate(regexList, PasswordLength, _progressBar);

                // Assigne un tableau de caractères ordonnés aléatoirement.
                char[] chars = template.ToArray().OrderBy(s => (random.Next(2) % 2) == 0).ToArray();

                // Génère un mot de passe tant qu'il ne contient pas tous les choix de caractères spécifiés.
                do
                {
                    // Assigne un tableau de caractères choisis aléatoirement.
                    for (int index = 0; index < PasswordLength; index++)
                    {
                        password[index] = chars[random.Next(0, chars.Length)];
                    }

                    passwordString = new string(password);

                    // Vérifie la correspondance entre deux une chaîne de caractère. 
                    foreach (string regex in regexList)
                    {
                        regexCheckString = new Regex(@"[" + regex + "]{1}");

                        // Vérifie le contenu du mot de passe avec les choix de caractères spécifiés.
                        if (regexCheckString.IsMatch(passwordString))
                        {
                            regexMatch = true;
                        }
                        else
                        {
                            regexMatch = false;
                            break;
                        }
                    }

                } while (!regexMatch);

                return passwordString;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_PwdGen.Default.GetRandomPwd);
                return null;
            }
        }
        #endregion

        #region GetStringTemplate
        /// <summary>
        /// Obtient une chaîne de caractères.
        /// </summary>
        /// <param name="_regexList">Liste des expressions régulières</param>
        /// <param name="_passwordLength">Longueur du mot de passe</param>
        /// <param name="_progressBar">Bar de progression</param>
        /// <returns>Chaîne de caractère contenant les caractères à choisir aléatoirement.</returns>
        private static string GetStringTemplate(List<string> _regexList, int _passwordLength, ProgressBar _progressBar)
        {
            const int RAPPORT_CPX_VALUE = 5;
            const int FACTOR_CPX_VALUE = 20;
            int multipleFactorCpx = 0;
            int cpxFactorCharacters = 0;
            int cpxFactorLength = 0;
            int passwordLength = _passwordLength;
            StringBuilder template = new StringBuilder();

            // Assigne le multiple du facteur de complexité.
            multipleFactorCpx += GetMultipleCpxFactor(template, _regexList, IsCheckUpperCase, Data_PwdGen.Default.letters.ToUpper(), Data_PwdGen.Default.regexUpper);
            multipleFactorCpx += GetMultipleCpxFactor(template, _regexList, IsCheckLowerCase, Data_PwdGen.Default.letters.ToLower(), Data_PwdGen.Default.regexLower);
            multipleFactorCpx += GetMultipleCpxFactor(template, _regexList, IsCheckNumber, Data_PwdGen.Default.numbers, Data_PwdGen.Default.regexNumbers);
            multipleFactorCpx += GetMultipleCpxFactor(template, _regexList, IsCheckSymbol, Data_PwdGen.Default.symbols, Data_PwdGen.Default.regexSymbols);

            // Assigne à la valeur de la bar de progression un facteur de complexité.
            cpxFactorCharacters = (passwordLength / RAPPORT_CPX_VALUE);
            cpxFactorLength = (FACTOR_CPX_VALUE * multipleFactorCpx);
            _progressBar.Value = cpxFactorCharacters + cpxFactorLength;

            return template.ToString();
        }
        #endregion

        #region SaveIputs
        /// <summary>
        /// Assigne à des propriétés, des entrées utilisateurs.
        /// </summary>
        /// <param name="_isCheckUpperCase">Satut d'une case à cocher</param>
        /// <param name="_isCheckLowerCase">Satut d'une case à cocher</param>
        /// <param name="_isCheckNumber">Satut d'une case à cocher</param>
        /// <param name="_isCheckSymbol">Satut d'une case à cocher</param>
        public static void SaveInputs(bool _isCheckUpperCase, bool _isCheckLowerCase, bool _isCheckNumber, bool _isCheckSymbol, int _passwordLength)
        {
            IsCheckUpperCase = _isCheckUpperCase;
            IsCheckLowerCase = _isCheckLowerCase;
            IsCheckNumber = _isCheckNumber;
            IsCheckSymbol = _isCheckSymbol;
            PasswordLength = _passwordLength;
        }
        #endregion

        #endregion

    }
}
