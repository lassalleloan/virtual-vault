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
using System.Text;
using VirtualVault.Cryptography;
using VirtualVault.Settings;
#endregion

namespace VirtualVault.Database
{
    public partial class VaultDatabase
    {

        // Authen

        #region Methods

        #region CheckUsnAndPwd
        /// <summary>
        /// Vérifie la correspondance des identifiants de connexion.
        /// </summary>
        /// <param name="_username">Nom d'utilisateur</param>
        /// <param name="_password">Mot de passe</param>
        /// <remarks>
        /// Vérifie la correspondance des identifiants de connexion correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>Boolean : True -> Identifiants corrects. False -> Identifiants incorrect.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static bool CheckUsnAndPwd(string _username, string _password)
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string querySelect = "SELECT [vaultUsername], [vaultPassword] FROM [tbl_vault] WHERE [idVault]=@idVault;";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        string username = _username;
                        string password = _password;
                        
                        if(GetVaultID(username, password))
                        {
                            // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                            commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                            using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                            {
                                // Vérifie si la commande SQL retourne au moins une ligne.
                                return dataReader.HasRows;
                            }
                        }

                        return false;
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.CheckUsnAndPwd);
                return false;
            }
        }
        #endregion

        #region IsFirstLaunch
        /// <summary>
        /// Vérifie l'existance d'un coffre-fort virtuel.
        /// </summary>
        /// <returns>Boolean : True -> Coffre-fort déjà existant. False -> Coffre-fort inexistant.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static bool IsFirstLaunch()
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string querySelect = "SELECT TOP 1 [idVault] FROM [tbl_vault];";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL ne retourne pas de ligne.
                            return (!dataReader.HasRows);
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.IsFirstLaunch);
                return false;
            }
        }
        #endregion

        #region SaveUsnAndPwd
        /// <summary>
        /// Enregistre des identifiants de connexion.
        /// </summary>
        /// <param name="_username">Nom d'utilisateur</param>
        /// <param name="_password">Mot de passe</param>
        /// <returns>Boolean : True -> Commande SQL exécutée. False -> Commande SQL non exécutée.</returns>
        /// <exception cref="SqlConnection, SqlCommand">
        /// Exception levée par l'objet SqlConnection ou SqlCommand.
        /// </exception>
        public static bool SaveUsnAndPwd(string _username, string _password)
        {
            try
            {
                bool success = false;

                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string queryInsert = "INSERT INTO [tbl_vault]([vaultUsername], [vaultPassword]) VALUES (@username, @password);";

                    using (SqlCommand commandSQL = new SqlCommand(queryInsert, connectionSQL))
                    {
                        const int NUMBER_LINE_ASSIGNS = 1;
                        string username = _username;
                        string password = _password;

                        // Assigne à des propriétés, des hachages d'identifiants de connexion.
                        SetUsnAndPwdHash(username, password);

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@username", UsernameHashed);
                        commandSQL.Parameters.AddWithValue("@password", PasswordHashed);

                        // Exécute la commande SQL.
                        success = (commandSQL.ExecuteNonQuery() == NUMBER_LINE_ASSIGNS);
                    }
                }

                return success;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.SaveUsnAndPwd);
                return false;
            }
        }
        #endregion

        #region SetUsnAndPwdHash
        /// <summary>
        /// Assigne à des propriétés, des hachages d'identifiants de connexion.
        /// </summary>
        /// <param name="_username">Nom d'utilisateur</param>
        /// <param name="_password">Mot de passe</param>
        private static void SetUsnAndPwdHash(string _username, string _password)
        {
            string username = _username;
            string password = _password;

            // Obtient le hachage des identifiants de connexion.
            byte[] hashedUsername = HashAlgorithm.GetHash(Encoding.ASCII.GetBytes(username));
            byte[] hashedPassword = HashAlgorithm.GetHash(Encoding.ASCII.GetBytes(password));

            // Assigne à des propriétés, les valeurs obtenues.
            UsernameHashed = hashedUsername;
            PasswordHashed = hashedPassword;
        }
        #endregion

        #region UpdateUsnAndPwd
        /// <summary>
        /// Met à jour les hachages des identifiants de connexion.
        /// </summary>
        /// <param name="_username">Nom d'utilisateur</param>
        /// <param name="_password">Mot de passe</param>
        /// <remarks>
        /// Met à jour les hachages des identifiants de connexion correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>Boolean : True -> Commande SQL exécutée. False -> Command SQL non exécutée.</returns>
        /// <exception cref="SqlConnection, SqlCommand">
        /// Exception levée par l'objet SqlConnection ou SqlCommand.
        /// </exception>
        public static bool UpdateUsnAndPwd(string _username, string _password)
        {
            try
            {
                bool success = false;

                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string queryUpdate = "UPDATE [tbl_vault] SET [vaultUsername]=@newUsername, [vaultPassword]=@newPassword WHERE [idVault]=@idVault;";

                    using (SqlCommand commandSQL = new SqlCommand(queryUpdate, connectionSQL))
                    {
                        const int NUMBER_LINE_ASSIGNS = 1;
                        string username = _username;
                        string password = _password;

                        // Assigne à des propriétés, des hachages d'identifiants de connexion.
                        SetUsnAndPwdHash(username, password);

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@newUsername", UsernameHashed);
                        commandSQL.Parameters.AddWithValue("@newPassword", PasswordHashed);
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        // Exécute la commande SQL.
                        success = (commandSQL.ExecuteNonQuery() == NUMBER_LINE_ASSIGNS);
                    }
                }

                return success;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.UpdateUsnAndPwd);
                return false;
            }
        }
        #endregion

        #endregion

    }
}
