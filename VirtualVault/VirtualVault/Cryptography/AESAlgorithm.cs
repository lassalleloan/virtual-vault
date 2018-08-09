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
using System.IO;
using System.Security.Cryptography;
using VirtualVault.Settings;
#endregion

namespace VirtualVault.Cryptography
{
    public static class AESAlgorithm
    {

        #region Methods

        #region DecryptData
        /// <summary>
        /// Déchiffre la donnée.
        /// </summary>
        /// <param name="_password">Mot de passe de déchiffrement</param>
        /// <param name="_cipheredData">Donnée à déchiffrer</param>
        /// <remarks>
        /// Déchiffre la donnée par l'utilisation de l'algorithme AES 256 bits.
        /// </remarks>
        /// <returns>Byte : Réussite -> Donnée déchiffrée. Echec -> Valeur null.</returns>
        /// <exception cref="AesManaged, MemoryStream, CryptoStream">
        /// Exeption levée par l'objet AesManaged, MemoryStream ou CryptoStream.
        /// </exception>
        public static byte[] DecryptData(byte[] _password, byte[] _cipheredData)
        {
            byte[] password = _password;
            byte[] cipheredData = _cipheredData;

            // Vérifie si le mot de passe est null, inférieur ou égale à 7 octets.
            if (IsPwdLength_Error(password))
            {
                return null;
            }

            // Vérifie si la donnée est null, inférieure ou égale à 0 octets.
            if (IsDataLength_Error(cipheredData))
            {
                return null;
            }

            try
            {
                byte[] plainData = new byte[cipheredData.Length];

                using (AesManaged aesManaged = new AesManaged())
                {
                    List<byte[]> algorithmInputs = GetKeyAndIV(password);
                    aesManaged.Key = algorithmInputs[0];
                    aesManaged.IV = algorithmInputs[1];

                    // Assigne le système de déchiffrement.
                    ICryptoTransform aesDecryptor = aesManaged.CreateDecryptor(aesManaged.Key, aesManaged.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(cipheredData))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesDecryptor, CryptoStreamMode.Read))
                        {
                            // Déchiffre la donnée.
                            csDecrypt.Read(plainData, 0, plainData.Length);
                        }
                    }
                }

                return plainData;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_AES.Default.DecryptData);
                return null;
            }
        }
        #endregion

        #region EncryptData
        /// <summary>
        /// Chiffre la donnée.
        /// </summary>
        /// <param name="_password">Mot de passe de chiffrement</param>
        /// <param name="_plainData">Donnée à chiffrer</param>
        /// <remarks>
        /// Chiffre la donnée par l'utilisation de l'algorithme AES 256 bits.
        /// </remarks>
        /// <returns>Byte : Réussite -> Donnée chiffrée. Echec -> Valeur null.</returns>
        /// <exception cref="AesManaged, MemoryStream, CryptoStream">
        /// Exeption levée par l'objet AesManaged, MemoryStream ou CryptoStream.
        /// </exception>
        public static byte[] EncryptData(byte[] _password, byte[] _plainData)
        {
            byte[] password = _password;
            byte[] plainData = _plainData;

            // Vérifie si le mot de passe est null, inférieur ou égale à 7 octets.
            if (IsPwdLength_Error(password))
            {
                return null;
            }

            // Vérifie si la donnée est null, inférieure ou égale à 0 octets.
            if (IsDataLength_Error(plainData))
            {
                return null;
            }

            try
            {
                byte[] cipherData = null;

                using (AesManaged aesManaged = new AesManaged())
                {
                    List<byte[]> algorithmInputs = GetKeyAndIV(password);
                    aesManaged.Key = algorithmInputs[0];
                    aesManaged.IV = algorithmInputs[1];

                    // Assigne le système de chiffrement.
                    ICryptoTransform aesEncryptor = aesManaged.CreateEncryptor(aesManaged.Key, aesManaged.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesEncryptor, CryptoStreamMode.Write))
                        {
                            // Chiffre la donnée.
                            csEncrypt.Write(plainData, 0, plainData.Length);
                            csEncrypt.FlushFinalBlock();

                            cipherData = msEncrypt.ToArray();
                        }
                    }
                }

                return cipherData;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_AES.Default.EncryptData);
                return null;
            }

        }
        #endregion

        #region GetKeyAndIV
        /// <summary>
        /// Obtient une clé et un vecteur d'initialisation.
        /// </summary>
        /// <param name="_password">Mot de passe de dérivation</param>
        /// <remarks>
        /// Obtient la clé et le vecteur d'initialisation par la dérivation d'un mot de passe, d'un salage et d'un nombre d'itérations.
        /// </remarks>
        /// <returns>List : Réussite -> Clé et vecteur d'initialisation. Echec -> Valeur null.</returns>
        /// <exception cref="Rfc2898DeriveBytes">
        /// Exception levée par l'objet Rfc2898DeriveBytes.
        /// </exception>
        private static List<byte[]> GetKeyAndIV(byte[] _password)
        {
            byte[] password = _password;

            // Vérifie si le mot de passe est null, inférieur ou égale à 7 octets.
            if (IsPwdLength_Error(password))
            {
                return null;
            }

            try
            {
                const int DERIVE_BYTE_ITERATIONS = 10000;
                const int KEY_SIZE = 32;
                const int IV_SIZE = 16;
                List<byte[]> algorithmInputs = new List<byte[]>();

                // Assigne la valeur du salage.
                byte[] aesSalt = Convert.FromBase64String(Data_AES.Default.Salt);
                bool isSaltNull = (Convert.ToBase64String(aesSalt) == string.Empty);

                // Vérifie la valeur du salage.
                if (isSaltNull)
                {
                    aesSalt = GetRandomSalt();
                }

                using (Rfc2898DeriveBytes rfcDB = new Rfc2898DeriveBytes(password, aesSalt, DERIVE_BYTE_ITERATIONS))
                {
                    // Obtient une clé et un vecteur d'initialisation.
                    byte[] key = rfcDB.GetBytes(KEY_SIZE);
                    byte[] iv = rfcDB.GetBytes(IV_SIZE);

                    algorithmInputs.Add(key);
                    algorithmInputs.Add(iv);
                }

                return algorithmInputs;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_AES.Default.GetKeyAndIV);
                return null;
            }
        }
        #endregion

        #region GetRandomSalt
        /// <summary>
        /// Obtient un salage aléatoire.
        /// </summary>
        /// <remarks>
        /// Obtient le salage et l'enregistre sous la variable Salt dans le fichier Data_AES.settings.
        /// </remarks>
        /// <returns>Byte : Réussite -> Salage aléatoire. Echec -> Valeur null.</returns>
        /// <exception cref="RNGCryptoServiceProvider">
        /// Exception levée par l'objet RNGCryptoServiceProvider.
        /// </exception>
        private static byte[] GetRandomSalt()
        {
            try
            {
                const int SALT_BYTE_SIZE = 256;
                byte[] randomSalt = null;

                using (RNGCryptoServiceProvider rngcsp = new RNGCryptoServiceProvider())
                {
                    // Obtient une valeur du salage aléatoire.
                    randomSalt = new byte[SALT_BYTE_SIZE];
                    rngcsp.GetBytes(randomSalt);

                    // Enregistre la valeur du salage sous la variable Salt dans le fichier Data_AES.settings.
                    Data_AES.Default.Salt = Convert.ToBase64String(randomSalt);
                    Data_AES.Default.Save();
                }

                return randomSalt;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_AES.Default.GetRandomSalt);
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
                System.Windows.Forms.MessageBox.Show(Data_AES.Default.IsDataLength_Error);
                return true;
            }

            return false;
        }
        #endregion

        #region IsPwdLength_Error
        /// <summary>
        /// Vérifie la longeur d'un mot de passe.
        /// </summary>
        /// <param name="_password">Mot de passe</param>
        /// <remarks>
        /// Le mot de passe ne doit pas être null, inférieur ou égale à 7 octets.
        /// </remarks>
        /// <returns>Boolean : True -> Longueur incorrecte. False -> Longueur correcte.</returns>
        private static bool IsPwdLength_Error(byte[] _password)
        {
            const int PWD_LENGHT_AVOID = 7;
            byte[] password = _password;
            bool isPwdLength_Error = ((password == null) || (password.Length <= PWD_LENGHT_AVOID));

            // Vérifie si le mot de passe est null, inférieur ou égale à 7 octets.
            if (isPwdLength_Error)
            {
                System.Windows.Forms.MessageBox.Show(Data_AES.Default.IsPwdLength_Error);
                return true;
            }

            return false;
        }
        #endregion

        #endregion

    }
}
