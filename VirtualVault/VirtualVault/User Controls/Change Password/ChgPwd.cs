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
using VirtualVault.Database;
#endregion

namespace VirtualVault.User_Controls.Change_Password
{
    public static class ChgPwd
    {

        #region Global Variables
        private static string _passwordInput_1 = string.Empty;
        private static string _passwordInput_2 = string.Empty;
        private static string _passwordInput_3 = string.Empty;
        #endregion

        #region Properties

        #region NewPwdInput_1
        /// <summary>
        /// Contient le nouveau mot de passe de confirmation.
        /// </summary>
        private static string NewPwdInput_1
        {
            get
            {
                return _passwordInput_2;
            }
            set
            {
                _passwordInput_2 = value;
            }
        }
        #endregion

        #region NewPwdInput_2
        /// <summary>
        /// Contient le mot de passe de confirmation.
        /// </summary>
        private static string NewPwdInput_2
        {
            get
            {
                return _passwordInput_3;
            }
            set
            {
                _passwordInput_3 = value;
            }
        }
        #endregion

        #region OldPwdInput
        /// <summary>
        /// Contient l'ancien mot de passe.
        /// </summary>
        private static string OldPwdInput
        {
            get
            {
                return _passwordInput_1;
            }
            set
            {
                _passwordInput_1 = value;
            }
        }
        #endregion

        #endregion

        #region Methods

        #region ArePasswordNotEqual
        /// <summary>
        /// Vérifie la correspondance entre deux mots de passe.
        /// </summary>
        /// <returns>Boolean : True -> Mots de passe différents. False -> Mots de passe égaux.</returns>
        public static bool ArePasswordNotEqual()
        {
            bool arePasswordNotEqual = (!NewPwdInput_1.Equals(NewPwdInput_2));

            // Vérifie la correspondance entre deux mots de passe.
            if (arePasswordNotEqual)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region IsPwdLengthInfSeven
        /// <summary>
        /// Vérifie la longueur du mot de passe.
        /// </summary>
        /// <remarks>
        /// Le mot de passe ne doit pas être null, inférieur ou égale à 7 caractères.
        /// </remarks>
        /// <returns>Boolean : True -> Longueur incorrect. False -> Longueur correcte.</returns>
        public static bool IsPwdLengthInfSeven()
        {
            const int PWD_LENGHT_AVOID = 7;
            bool isPwdLengthInfSeven = (NewPwdInput_1.Length <= PWD_LENGHT_AVOID);

            // Vérifie si le mot de passe est null, inférieur ou égale à 7 caractères.
            if (isPwdLengthInfSeven)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region IsUsnAndPwdCheck
        /// <summary>
        /// Vérifie la correspondance des identifiants de connexion.
        /// </summary>
        /// <returns>Boolean : True -> Identifiants corrects. False -> Identifiants incorrect ou erreur.</returns>
        public static bool IsUsnAndPwdCheck()
        {
            bool hashAreEqual = false;

            hashAreEqual = VaultDatabase.CheckUsnAndPwd(VaultDatabase.UserUsername, OldPwdInput);

            return hashAreEqual;
        }
        #endregion

        #region IsUsnAndPwdUpdate
        /// <summary>
        /// Vérifie la mise à jour des hachages des identifiants de connexion.
        /// </summary>
        /// <returns>Boolean : True -> Mise à jour effectuée. False -> Mise à jour non effectuée.</returns>
        public static bool IsUsnAndPwdUpdate()
        {
            bool successHashed = false;

            successHashed = VaultDatabase.UpdateUsnAndPwd(VaultDatabase.UserUsername, NewPwdInput_1);

            return successHashed;
        }
        #endregion

        #region SaveIputs
        /// <summary>
        /// Assigne à des propriétés, des entrées utilisateurs.
        /// </summary>
        /// <param name="_passwordInput_1">Ancien mot de passe</param>
        /// <param name="_passwordInput_2">Nouveau mot de passe</param>
        /// <param name="_passwordInput_3">Nouveau mot de passe de confirmation</param>
        public static void SaveIputs(string _passwordInput_1, string _passwordInput_2, string _passwordInput_3)
        {
            OldPwdInput = _passwordInput_1;
            NewPwdInput_1 = _passwordInput_2;
            NewPwdInput_2 = _passwordInput_3;
        }
        #endregion

        #endregion

    }
}
