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
using System.Data.SqlClient;
using VirtualVault.Settings;
#endregion

namespace VirtualVault.Database
{
    public partial class VaultDatabase
    {

        // ScrNote_1

        #region Methods

        #region CheckScrNoteName
        /// <summary>
        /// Vérifie l'existance du nom d'une note sécurisées.
        /// </summary>
        /// <param name="_scrNoteName">Nom d'une note sécurisées</param>
        /// <remarks>
        /// Vérifie l'existance du nom d'une note sécurisées correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>Boolean : True -> Nom déjà existant. False -> Nom inexistant.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static bool CheckScrNoteName(string _scrNoteName)
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string querySelect = "SELECT [scrNoteName] FROM [tbl_scrNote] WHERE [idVault]=@idVault AND [scrNoteName]=@scrNoteName;";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        string scrNoteName = _scrNoteName;

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);
                        commandSQL.Parameters.AddWithValue("@scrNoteName", scrNoteName);

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
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.CheckScrNoteName);
                return false;
            }
        }
        #endregion

        #region DeleteScrNote
        /// <summary>
        /// Supprime une note sécurisée.
        /// </summary>
        /// <param name="_scrNoteID">Identifiant de note sécurisée</param>
        /// <returns>Boolean : True -> Commande SQL exécutée. False -> Commande SQL non exécutée.</returns>
        /// <exception cref="SqlConnection, SqlCommand">
        /// Exception levée par l'objet SqlConnection ou SqlCommand.
        /// </exception>
        public static bool DeleteScrNote(string _scrNoteID)
        {
            try
            {
                bool success = false;

                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string queryInsert = "DELETE FROM [tbl_scrNote] WHERE [idVault]=@idVault AND [idScrNote]=@idScrNote;";

                    using (SqlCommand commandSQL = new SqlCommand(queryInsert, connectionSQL))
                    {
                        const int NUMBER_LINE_ASSIGNS = 1;
                        string scrNoteID = _scrNoteID;

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idScrNote", scrNoteID);
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        // Exécute la commande SQL.
                        success = (commandSQL.ExecuteNonQuery() == NUMBER_LINE_ASSIGNS);
                    }
                }

                return success;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.DeleteScrNote);
                return false;
            }
        }
        #endregion

        #region GetScrNoteBookmark
        /// <summary>
        /// Obtient le statut du favoris d'une note sécurisées.
        /// </summary>
        /// <param name="_scrNoteID">Identifiant de note sécurisée</param>
        /// <param name="_categoryName">Nom d'une catégrie</param>
        /// <param name="_bookmark">Satut favoris</param>
        /// <remarks>
        /// Obtient le statut du favoris d'une note sécurisées correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>Boolean : True -> Satut de favoris de la note sécurisée. False : -> Valeur null</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static bool GetScrNoteBookmark(string _scrNoteID = "", string _categoryName = "", string _bookmark = "")
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string scrNoteID = _scrNoteID;
                    string categoryName = _categoryName;
                    string bookmark = _bookmark;
                    string querySelect_ByCondition = string.Empty;

                    if (scrNoteID != string.Empty)
                    {
                        querySelect_ByCondition = " AND [idScrNote]=" + scrNoteID;
                    }

                    // Vérifie l'assignement d'un nom de catégorie.
                    if (categoryName != string.Empty && categoryName != Data_Authen.Default.AllItem)
                    {
                        string categoryID = GetCategoryID(categoryName);

                        querySelect_ByCondition = " AND [idCategory]=" + categoryID;
                    }

                    // Vérifie l'assignement d'un favoris.
                    if (bookmark != string.Empty)
                    {
                        querySelect_ByCondition = " AND [scrNoteBookmark]=" + bookmark;
                    }

                    string querySelect = "SELECT [scrNoteBookmark] FROM [tbl_scrNote] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne.
                            if (dataReader.HasRows)
                            {
                                bool scrNoteBookmark = false;

                                // Lit tant qu'il existe des lignes.
                                while (dataReader.Read())
                                {
                                    // Obtient le contenu des notes sécurisées.
                                    scrNoteBookmark = Convert.ToBoolean(dataReader["scrNoteBookmark"]);
                                }

                                return scrNoteBookmark;
                            }

                            return false;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetScrNoteBookmark);
                return false;
            }
        }
        #endregion

        #region GetScrNoteCategory
        /// <summary>
        /// Obtient une liste des catégories des notes sécurisées.
        /// </summary>
        /// <param name="_scrNoteID">Identifiant d'une note sécurisée</param>
        /// <param name="_categoryName">Nom de catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient une liste des catégories des notes sécurisées correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste des catégories des notes sécurisées. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<string> GetScrNoteCategory(string _scrNoteID = "", string _categoryName = "", string _bookmark = "")
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string scrNoteID = _scrNoteID;
                    string categoryName = _categoryName;
                    string bookmark = _bookmark;
                    string querySelect_ByCondition = string.Empty;

                    if (scrNoteID != string.Empty)
                    {
                        querySelect_ByCondition = " AND [idScrNote]=" + scrNoteID;
                    }

                    // Vérifie l'assignement d'un nom de catégorie.
                    if (categoryName != string.Empty && categoryName != Data_Authen.Default.AllItem)
                    {
                        string categoryID = GetCategoryID(categoryName);

                        querySelect_ByCondition = " AND [idCategory]=" + categoryID;
                    }

                    // Vérifie l'assignement d'un favoris.
                    if (bookmark != string.Empty)
                    {
                        querySelect_ByCondition = " AND [scrNoteBookmark]=" + bookmark;
                    }

                    string querySelect = "SELECT [idCategory] FROM [tbl_scrNote] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne.
                            if (dataReader.HasRows)
                            {
                                List<string> scrNoteBookmark = new List<string>();

                                // Lit tant qu'il existe des lignes.
                                while (dataReader.Read())
                                {
                                    // Obtient le contenu des notes sécurisées.
                                    scrNoteBookmark.Add(dataReader["idCategory"].ToString());
                                }

                                return scrNoteBookmark;
                            }

                            return null;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetScrNoteCategory);
                return null;
            }
        }
        #endregion

        #region GetScrNoteChgDate
        /// <summary>
        /// Obtient une liste des dates de modification des notes sécurisées.
        /// </summary>
        /// <param name="_scrNoteID">Identifiant d'une note sécurisée</param>
        /// <param name="_categoryName">Nom de catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient une liste des dates de modification des notes sécurisées correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste des dates de modification des notes sécurisées. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<string> GetScrNoteChgDate(string _scrNoteID = "", string _categoryName = "", string _bookmark = "")
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string scrNoteID = _scrNoteID;
                    string categoryName = _categoryName;
                    string bookmark = _bookmark;
                    string querySelect_ByCondition = string.Empty;

                    if (scrNoteID != string.Empty)
                    {
                        querySelect_ByCondition = " AND [idScrNote]=" + scrNoteID;
                    }

                    // Vérifie l'assignement d'un nom de catégorie.
                    if (categoryName != string.Empty && categoryName != Data_Authen.Default.AllItem)
                    {
                        string categoryID = GetCategoryID(categoryName);

                        querySelect_ByCondition = " AND [idCategory]=" + categoryID;
                    }

                    // Vérifie l'assignement d'un favoris.
                    if (bookmark != string.Empty)
                    {
                        querySelect_ByCondition = " AND [scrNoteBookmark]=" + bookmark;
                    }

                    string querySelect = "SELECT [scrNoteChangeDate] FROM [tbl_scrNote] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne.
                            if (dataReader.HasRows)
                            {
                                List<string> scrNoteBookmark = new List<string>();

                                // Lit tant qu'il existe des lignes.
                                while (dataReader.Read())
                                {
                                    // Obtient le contenu des notes sécurisées.
                                    scrNoteBookmark.Add(dataReader["scrNoteChangeDate"].ToString());
                                }

                                return scrNoteBookmark;
                            }

                            return null;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetScrNoteChgDate);
                return null;
            }
        }
        #endregion

        #region GetScrNoteCrtDate
        /// <summary>
        /// Obtient une liste des dates de création des notes sécurisées.
        /// </summary>
        /// <param name="_scrNoteID">Identifiant d'une note sécurisée</param>
        /// <param name="_categoryName">Nom de catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient une liste des dates de création des notes sécurisées correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste des dates de création des notes sécurisées. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<string> GetScrNoteCrtDate(string _scrNoteID = "", string _categoryName = "", string _bookmark = "")
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string scrNoteID = _scrNoteID;
                    string categoryName = _categoryName;
                    string bookmark = _bookmark;
                    string querySelect_ByCondition = string.Empty;

                    if (scrNoteID != string.Empty)
                    {
                        querySelect_ByCondition = " AND [idScrNote]=" + scrNoteID;
                    }

                    // Vérifie l'assignement d'un nom de catégorie.
                    if (categoryName != string.Empty && categoryName != Data_Authen.Default.AllItem)
                    {
                        string categoryID = GetCategoryID(categoryName);

                        querySelect_ByCondition = " AND [idCategory]=" + categoryID;
                    }

                    // Vérifie l'assignement d'un favoris.
                    if (bookmark != string.Empty)
                    {
                        querySelect_ByCondition = " AND [scrNoteBookmark]=" + bookmark;
                    }

                    string querySelect = "SELECT [scrNoteCreationDate] FROM [tbl_scrNote] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne.
                            if (dataReader.HasRows)
                            {
                                List<string> scrNoteBookmark = new List<string>();

                                // Lit tant qu'il existe des lignes.
                                while (dataReader.Read())
                                {
                                    // Obtient le contenu des notes sécurisées.
                                    scrNoteBookmark.Add(dataReader["scrNoteCreationDate"].ToString());
                                }

                                return scrNoteBookmark;
                            }

                            return null;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetScrNoteCrtDate);
                return null;
            }
        }
        #endregion

        #endregion

    }
}
