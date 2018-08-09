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
using System.Security.Cryptography;
using VirtualVault.Settings;
#endregion

namespace VirtualVault.Cryptography
{
    public static class HashAlgorithm
    {
        #region Global Variables
        private const int _SALT_BYTE_SIZE = 256;
        #endregion

        #region Methods

        #region CheckHash
        /// <summary>
        /// Vérifie la correspondance entre deux hachages.
        /// </summary>
        /// <param name="_referenceHash">Hachage de référence</param>
        /// <param name="_hashCompare">Hachage à comparer</param>
        /// <returns>Boolean : True -> Hachages identiques. False -> Hachages différents.</returns>
        /// <exception cref="referenceHash, hashCompare">
        /// Exception levée par l'objet referenceHash ou hashToCompare.
        /// </exception>
        public static bool CheckHash(byte[] _referenceHash, byte[] _hashCompare)
        {
            byte[] referenceHash = _referenceHash;
            byte[] hashCompare = _hashCompare;

            // Vérifie si l'un des deux hachages est null, inférieur ou égale à 0 octets.
            if (IsDataLength_Error(referenceHash) || IsDataLength_Error(hashCompare))
            {
                return false;
            }

            try
            {
                uint difference = (uint)referenceHash.Length ^ (uint)hashCompare.Length;

                // Applique un XOR entre chaque octets des deux hachages.
                for (int index = 0; (index < referenceHash.Length) && (index < hashCompare.Length); index++)
                {
                    difference |= (uint)(referenceHash[index] ^ hashCompare[index]);
                }

                bool isNotDifference = (difference == 0);
                return isNotDifference;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_Hash.Default.CheckHash);
                return false;
            }
        }
        #endregion

        #region GetHash
        /// <summary>
        /// Obtient le hachage d'une donnée.
        /// </summary>
        /// <param name="_data">Données à hacher</param>
        /// <remarks>
        /// Hache la donnée concaténée avec le salage.
        /// </remarks>
        /// <returns>Byte : Réussite -> Donnée hachée. Echec -> Valeur null.</returns>
        /// <exception cref="SHA512">
        /// Exception levée par l'objet SHA512.
        /// </exception>
        public static byte[] GetHash(byte[] _data)
        {
            byte[] data = _data;

            // Vérifie si la donnée est null, inférieure ou égale à 0 octets.
            if (IsDataLength_Error(data))
            {
                return null;
            }

            try
            {
                byte[] saltedData = new byte[_SALT_BYTE_SIZE + data.Length];
                byte[] hashedData = null;

                // Récupére le salage.
                byte[] hashSalt = Convert.FromBase64String(Data_Hash.Default.Salt);
                bool isSaltNull = (Convert.ToBase64String(hashSalt) == string.Empty);

                // Vérifie le salage.
                if (isSaltNull)
                {
                    hashSalt = GetRandomSalt();
                }

                // Concatène la donnée avec le salage.
                System.Buffer.BlockCopy(hashSalt, 0, saltedData, 0, _SALT_BYTE_SIZE);
                System.Buffer.BlockCopy(data, 0, saltedData, _SALT_BYTE_SIZE, data.Length);

                using (SHA512 sha512Hash = new SHA512Managed())
                {
                    // Hache le résultat de la concaténation.
                    hashedData = sha512Hash.ComputeHash(saltedData);

                    sha512Hash.Clear();
                    sha512Hash.Dispose();
                }

                return hashedData;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_Hash.Default.GetHash);
                return null;
            }

        }
        #endregion

        #region GetRandomSalt
        /// <summary>
        /// Obtient un salage aléatoire.
        /// </summary>
        /// <remarks>
        /// Obtient le salage et l'enregistre sous la variable Salt dans le fichier Data_Hash.settings.
        /// </remarks>
        /// <returns>Byte : Réussite -> Salage aléatoire. Echec -> Valeur null.</returns>
        /// <exception cref="RNGCryptoServiceProvider">
        /// Exception levée par l'objet RNGCryptoServiceProvider.
        /// </exception>
        private static byte[] GetRandomSalt()
        {
            try
            {
                byte[] randomSalt = null;

                using (RNGCryptoServiceProvider rngcsp = new RNGCryptoServiceProvider())
                {
                    // Obtient un salage aléatoire.
                    randomSalt = new byte[_SALT_BYTE_SIZE];
                    rngcsp.GetBytes(randomSalt);

                    // Enregistre le salage sous la variable Salt dans le fichier Data_Hash.settings.
                    Data_Hash.Default.Salt = Convert.ToBase64String(randomSalt);
                    Data_Hash.Default.Save();
                }

                return randomSalt;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_Hash.Default.GetRandomSalt);
                return null;
            }

        }
        #endregion

        #region IsDataLength_Error
        /// <summary>
        /// Vérifie la longueur d'une donnée.
        /// </summary>
        /// <param name="_data">Données</param>
        /// <remarks>
        /// La donnée à traiter ne doit pas être null, inférieure ou égale à 0 octets.
        /// </remarks>
        /// <returns>Boolean : True -> Longueur incorrecte. False -> Longueur correcte.</returns>
        private static bool IsDataLength_Error(byte[] _data)
        {
            byte[] data = _data;
            bool isDataLength_Error = ((data == null) || (data.Length <= 0));

            // Vérifie si la donnée est null, inférieure ou égale à 0 octets.
            if (isDataLength_Error)
            {
                System.Windows.Forms.MessageBox.Show(Data_Hash.Default.IsDataLength_Error);
                return true;
            }

            return false;
        }
        #endregion

        #endregion

    }
}
