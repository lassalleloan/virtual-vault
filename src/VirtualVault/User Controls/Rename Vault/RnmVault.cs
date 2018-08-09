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

namespace VirtualVault.User_Controls.Rename_Vault
{
    public static class RnmVault
    {

        #region Global Variables
        private static string _vaultName = string.Empty;
        #endregion

        #region Properties

        #region VaultName
        /// <summary>
        /// Contient le nom du coffre-fort virtuel.
        /// </summary>
        private static string VaultName
        {
            get
            {
                return _vaultName;
            }
            set
            {
                _vaultName = value;
            }
        }
        #endregion

        #endregion

        #region Methods

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

        #region IsVltNameLength_Error
        /// <summary>
        /// Vérifie la longueur du nom de coffre-fort virtuel.
        /// </summary>
        /// <remarks>
        /// Vérifie si le nom du coffe-fort virtuel est null ou non compris entre 1 et 30 caractères.
        /// </remarks>
        /// <returns>Boolean : True -> Longueur incorrecte . False -> Longueur correcte.</returns>
        public static bool IsVltNameLength_Error()
        {
            const int VLTNAME_LENGTH_MAX = 30;
            bool isVltNameLength_Error = ((VaultName == null) || (!IsBetween(0, VaultName.Length, VLTNAME_LENGTH_MAX)));

            // Vérifie si le nom du coffe-fort virtuel est null ou non compris entre 1 et 30 caractères.
            if (isVltNameLength_Error)
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
        /// <param name="_vaultName">Nom du coffre-fort virtuel.</param>
        public static void SaveInputs(string _vaultName)
        {
            VaultName = _vaultName;
        }
        #endregion

        #endregion

    }
}
