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
using System.Data.SqlClient;
using VirtualVault.Settings;
#endregion

namespace VirtualVault.Database
{
    public partial class VaultDatabase
    {

        // Vault

        #region Global Variables
        private static string _vaultID = string.Empty;
        private static string _vaultName = string.Empty;
        #endregion

        #region Properties

        #region VaultID
        /// <summary>
        /// Contient l'identifiant du coffre-fort virtuel utilisé.
        /// </summary>
        private static string VaultID
        {
            get
            {
                return _vaultID;
            }
            set
            {
                _vaultID = value;
            }
        }
        #endregion

        #region VaultName
        /// <summary>
        /// Contient le nom du coffre-fort virtuel utilisé.
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

        #region GetVaultID
        /// <summary>
        /// Obtient l'identifiant d'un coffre-fort virtuel.
        /// </summary>
        /// <param name="_username">Nom d'utilisateur</param>
        /// <param name="_password">Mot de passe</param>
        /// <returns>Boolean : True -> Identifiant d'un coffre-fort virtuel obtenu. False -> Aucune correspondance.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        private static bool GetVaultID(string _username, string _password)
        {
            try
            {
                string vaultID = string.Empty;

                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string querySelect = "SELECT [idVault] FROM [tbl_vault] WHERE [vaultUsername]=@username AND [vaultPassword]=@password;";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        string username = _username;
                        string password = _password;

                        // Assigne à des propriétés, des hachages d'identifiants de connexion.
                        SetUsnAndPwdHash(username, password);

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@username", UsernameHashed);
                        commandSQL.Parameters.AddWithValue("@password", PasswordHashed);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne et lit tant qu'il existe des lignes.
                            if (dataReader.HasRows && dataReader.Read())
                            {
                                // Obtient le numéro d'identifiant du coffre-fort virtuel.
                                VaultID = dataReader["idVault"].ToString();
                                return true;
                            }

                            return false;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetVaultID);
                return false;
            }
        }
        #endregion

        #region GetVaultName
        /// <summary>
        /// Obtient le nom d'un coffre-fort virtuel.
        /// </summary>
        /// <remarks>
        /// Obtient le nom d'un coffre-fort virtuel correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>String : Réussite -> Nom d'un coffre-fort virtuel. Echec -> Chaîne de caractères vide.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static string GetVaultName()
        {
            try
            {
                if (VaultName != string.Empty)
                {
                    return VaultName;
                }

                string vaultName = string.Empty;

                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string querySelect = "SELECT [vaultName] FROM [tbl_vault] WHERE [idVault]=@idVault;";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne et lit tant qu'il existe des lignes.
                            if (dataReader.HasRows && dataReader.Read())
                            {
                                // Obtient le nom du coffre-fort virtuel.
                                vaultName = dataReader["vaultName"].ToString();
                            }

                            return vaultName;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetVaultName);
                return string.Empty;
            }
        }
        #endregion

        #region UpdVaultName
        /// <summary>
        /// Met à jour le nom d'un coffre-virtuel.
        /// </summary>
        /// <param name="_newVaultName">Nouveau nom du coffre-fort virtuel</param>
        /// <remarks>
        /// Met à jour le nom d'un coffre-virtuel correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>Boolean : True -> Commande SQL éxecutée. False -> Commande SQL non éxecutée.</returns>
        /// <exception cref="SqlConnection, SqlCommand">
        /// Exception levée par l'objet SqlConnection ou SqlCommand.
        /// </exception>
        public static bool UpdVaultName(string _newVaultName)
        {
            try
            {
                bool success = false;
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string queryUpdate = "UPDATE [tbl_vault] SET [vaultName]=@newVaultName WHERE [idVault]=@idVault;";

                    using (SqlCommand commandSQL = new SqlCommand(queryUpdate, connectionSQL))
                    {
                        string newVaultName = _newVaultName;

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@newVaultName", newVaultName);
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        // Exécute la commande SQL.
                        success = commandSQL.ExecuteNonQuery() == 1;
                    }
                }

                return success;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.UpdVaultName);
                return false;
            }
        }
        #endregion

        #endregion

    }
}
