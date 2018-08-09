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

namespace VirtualVault.User_Controls.Category
{
    public static class Category
    {

        #region Global Variables
        private static string _categoryName = string.Empty;
        private static bool _isCategoryUpdate = false;
        #endregion

        #region Properties

        #region CategoryName
        /// <summary>
        /// Contient le nom d'une catégorie.
        /// </summary>
        private static string CategoryName
        {
            get
            {
                return _categoryName;
            }
            set
            {
                _categoryName = value;
            }
        }
        #endregion

        #region IsCategoryUpdate
        // Contient le statut de l'action de modifier un nom de catégorie.
        public static bool IsCategoryUpdate
        {
            get
            {
                return _isCategoryUpdate;
            }
            set
            {
                _isCategoryUpdate = value;
            }
        }
        #endregion

        #endregion

        #region Methods

        #region IsBetween
        /// <summary>
        /// Vérifie si une valeur se situe entre deux valeurs extrêmes.
        /// </summary>
        /// <param name=
        /// _minimum">Valeur minimale"</param>
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

        #region IsCategoryAlreadyExist
        /// <summary>
        /// Vérifie l'existance d'un nom de catégorie.
        /// </summary>
        /// <param name="_categoryName">Nom de la catégorie</param>
        /// <returns>Boolean : True -> Nom déjà existant. False -> Nom inexistant.</returns>
        public static bool IsCategoryExist()
        {
            bool isCategoryAlreadyExist = false;

            isCategoryAlreadyExist = VaultDatabase.CheckCategory(CategoryName);

            return isCategoryAlreadyExist;
        }
        #endregion

        #region IsCtgNameLength_Error
        /// <summary>
        /// Vérifie la longueur du nom de la catégorie.
        /// </summary>
        /// <remarks>
        /// Le nom de catégorie ne doit pas être null et non comprise entre 1 et 30 caractères.
        /// </remarks>
        /// <returns>Boolean : True -> Longueur correcte. False -> Longueur incorrecte.</returns>
        public static bool IsCtgNameLength_Error()
        {
            const int CTGNAME_LENGTH_MAX = 30;
            bool isCtgNameLength_Error = ((CategoryName == null) || (!IsBetween(0, CategoryName.Length, CTGNAME_LENGTH_MAX)));

            // Vérifie si le nom de catégorie est null ou compris entre 1 et 30 caractères.
            if (isCtgNameLength_Error)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region SaveIputs
        /// <summary>
        /// Assigne à des propriétés, des entrées utilisateurs.
        /// </summary>
        /// <param name="_categoryName">Nom d'une catégorie</param>
        public static void SaveIputs(string _categoryName)
        {
            CategoryName = _categoryName;
        }
        #endregion

        #endregion

    }
}
