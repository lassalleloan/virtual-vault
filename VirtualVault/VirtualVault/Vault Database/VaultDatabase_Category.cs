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
using System.Collections.Generic;
using System.Data.SqlClient;
using VirtualVault.Settings;
#endregion

namespace VirtualVault.Database
{
    public partial class VaultDatabase
    {

        // Category

        #region Global Variables
        private static string _categoryID = string.Empty;
        #endregion

        #region Properties

        #region CategoryID
        /// <summary>
        /// Contient l'identifiant d'une catégorie.
        /// </summary>
        private static string CategoryID
        {
            get
            {
                return _categoryID;
            }
            set
            {
                _categoryID = value;
            }
        }
        #endregion

        #endregion

        #region Methods

        #region CheckCategory
        /// <summary>
        /// Vérifie l'existance d'une catégorie.
        /// </summary>
        /// <param name="_categoryName">Nom d'une catégorie</param>
        /// <remarks>
        /// Vérifie l'existance d'une catégorie correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>Boolean : True -> Nom déjà existant. False -> Nom inexistant.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static bool CheckCategory(string _categoryName)
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string querySelect = "SELECT [categoryName] FROM [tbl_category] WHERE [idVault]=@idVault AND [categoryName]=@categoryName;";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        string categoryName = _categoryName;

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);
                        commandSQL.Parameters.AddWithValue("@categoryName", categoryName);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne.
                            return dataReader.HasRows;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.CheckCategory);
                return false;
            }
        }
        #endregion

        #region DeleteCategory
        /// <summary>
        /// Supprime un nom de catégorie.
        /// </summary>
        /// <param name="_categoryName">Nom d'une catégorie</param>
        /// <returns>Boolean : True -> Commande SQL exécutée. False -> Commande SQL non exécutée.</returns>
        /// <exception cref="SqlConnection, SqlCommand">
        /// Exception levée par l'objet SqlConnection ou SqlCommand.
        /// </exception>
        public static bool DeleteCategory(string _categoryName)
        {
            try
            {
                bool success = false;

                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string categoryName = _categoryName;
                    string categoryID = GetCategoryID(categoryName);

                    // Met à jour la catégorie des fiches d'authentification et des notes sécurisées.
                    UpdateACardCategory(categoryID);
                    UpdateScrNoteCategory(categoryID);

                    string queryDelete = "DELETE FROM [tbl_category] WHERE idVault=@idVault AND [idCategory]=@idCategory;";

                    using (SqlCommand commandSQL = new SqlCommand(queryDelete, connectionSQL))
                    {
                        const int NUMBER_LINE_ASSIGNS = 1;

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idCategory", categoryID);
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        // Exécute la commande SQL.
                        success = (commandSQL.ExecuteNonQuery() == NUMBER_LINE_ASSIGNS);
                    }
                }

                return success;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.DeleteCategory);
                return false;
            }
        }
        #endregion

        #region GetCategoryID
        /// <summary>
        /// Obtient le numéro d'identifiant d'une catégorie.
        /// </summary>
        /// <param name="_categoryName">Nom d'une catégorie</param>
        /// <returns>String : Réussite -> Numéro d'identifiant de la catégorie. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        private static string GetCategoryID(string _categoryName)
        {
            try
            {
                if (CategoryID != string.Empty)
                {
                    return CategoryID;
                }

                string categoryID = string.Empty;

                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string querySelect = "SELECT [idCategory] FROM [tbl_category] WHERE [categoryName]=@categoryName;";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        string categoryName = _categoryName;

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@categoryName", categoryName);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne et lit tant qu'il existe des lignes.
                            if (dataReader.HasRows && dataReader.Read())
                            {
                                // Obtient le numéro d'identifiant de la catégorie.
                                categoryID = dataReader["idCategory"].ToString();
                            }
                        }
                    }
                }

                return categoryID;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetCategoryID);
                return null;
            }
        }
        #endregion

        #region GetCategoryNameList
        /// <summary>
        /// Obtient une liste des noms des catégories.
        /// </summary>
        /// <remarks>
        /// Obtient une liste des noms des catégories correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste des noms des catgéories. False -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<string> GetCategoryNameList()
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string querySelect = "SELECT [categoryName] FROM [tbl_category] WHERE [idVault]=@idVault;";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne.
                            if (dataReader.HasRows)
                            {
                                List<string> ctgNameList = new List<string>();

                                // Lit tant qu'il existe des lignes.
                                while (dataReader.Read())
                                {
                                    // Obtient le nom des catégories.
                                    ctgNameList.Add(dataReader["categoryName"].ToString());
                                }

                                return ctgNameList;
                            }

                            return null;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetCategorieNameList);
                return null;
            }
        }
        #endregion

        #region SaveCategory
        /// <summary>
        /// Enregistre une catégorie.
        /// </summary>
        /// <param name="_categoryName">Nom d'une catégorie</param>
        /// <returns>Boolean : True -> Commande SQL exécutée. False -> Commande SQL non exécutée.</returns>
        /// <exception cref="SqlConnection, SqlCommand">
        /// Exception levée par l'objet SqlConnection ou SqlCommand.
        /// </exception>
        public static bool SaveCategory(string _categoryName)
        {
            try
            {
                bool success = false;

                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string queryInsert = "INSERT INTO [tbl_category]([categoryName], [idVault]) VALUES (@ctgName, @idVault);";

                    using (SqlCommand commandSQL = new SqlCommand(queryInsert, connectionSQL))
                    {
                        const int NUMBER_LINE_ASSIGNS = 1;
                        string categoryName = _categoryName;

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@ctgName", categoryName);
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        // Exécute la commande SQL.
                        success = (commandSQL.ExecuteNonQuery() == NUMBER_LINE_ASSIGNS);
                    }
                }

                return success;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.SaveCategory);
                return false;
            }
        }
        #endregion

        #region UpdateCategory
        /// <summary>
        /// Met à jour un nom de catégorie.
        /// </summary>
        /// <param name="_oldCategoryName">Ancien nom d'une catégorie</param>
        /// <param name="_newCategoryName">Nouveau nom d'une catégorie</param>
        /// <returns>Boolean : True -> Commande SQL exécutée. False -> Commande SQL non exécutée.</returns>
        /// <exception cref="SqlConnection, SqlCommand">
        /// Exception levée par l'objet SqlConnection ou SqlCommand.
        /// </exception>
        public static bool UpdateCategory(string _oldCategoryName, string _newCategoryName)
        {
            try
            {
                bool success = false;

                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string queryUpdate = "UPDATE [tbl_category] SET [categoryName]=@newCtgName WHERE [idVault]=@idVault AND [categoryName]=@oldCtgName;";

                    using (SqlCommand commandSQL = new SqlCommand(queryUpdate, connectionSQL))
                    {
                        const int NUMBER_LINE_ASSIGNS = 1;
                        string oldCategoryName = _oldCategoryName;
                        string newCategoryName = _newCategoryName;

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@oldCtgName", oldCategoryName);
                        commandSQL.Parameters.AddWithValue("@newCtgName", newCategoryName);
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        // Exécute la commande SQL.
                        success = (commandSQL.ExecuteNonQuery() == NUMBER_LINE_ASSIGNS);
                    }
                }

                return success;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.UpdateCategory);
                return false;
            }
        }
        #endregion

        #endregion

    }
}
