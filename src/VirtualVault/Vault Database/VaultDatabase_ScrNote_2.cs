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

        // ScrNote_2

        #region Methods

        #region GetScrNoteContentList
        /// <summary>
        /// Obtient une liste du contenu des notes sécurisées.
        /// </summary>
        /// <remarks>
        /// Obtient une liste du contenu des notes sécurisées correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste contenu des notes sécurisées. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<byte[]> GetScrNoteContentList(string _scrNoteID = "", string _categoryName = "", string _bookmark = "")
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
                        querySelect_ByCondition = " AND [idScrNote]=" + scrNoteID + "";
                    }

                    // Vérifie l'assignement d'un nom de catégorie.
                    if (categoryName != string.Empty && categoryName != Data_Authen.Default.AllItem)
                    {
                        string categoryID = GetCategoryID(categoryName);

                        querySelect_ByCondition = " AND [idCategory]=" + categoryID + "";
                    }

                    // Vérifie l'assignement d'un favoris.
                    if (bookmark != string.Empty)
                    {
                        querySelect_ByCondition = " AND [scrNoteBookmark]=" + bookmark;
                    }

                    string querySelect = "SELECT [scrNoteContent] FROM [tbl_scrNote] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne.
                            if (dataReader.HasRows)
                            {
                                List<byte[]> scrNoteContentList = new List<byte[]>();

                                // Lit tant qu'il existe des lignes.
                                while (dataReader.Read())
                                {
                                    // Obtient le contenu des notes sécurisées.
                                    scrNoteContentList.Add((byte[])dataReader["scrNoteContent"]);
                                }

                                return scrNoteContentList;
                            }

                            return null;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetScrNoteContentList);
                return null;
            }
        }
        #endregion

        #region GetScrNoteIDList
        /// <summary>
        /// Obtient une liste des identifiants des notes sécurisées.
        /// </summary>
        /// <param name="_scrNoteID">Identifiant de fichd'authentification</param>
        /// <param name="_categoryName">Nom de catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient une liste des identifiants des notes sécurisées correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> liste des identifiants des notes sécurisées. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<string> GetScrNoteIDList(string _scrNoteID = "", string _categoryName = "", string _bookmark = "")
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

                    string querySelect = "SELECT [idScrNote] FROM [tbl_scrNote] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne.
                            if (dataReader.HasRows)
                            {
                                List<string> scrNoteIDList = new List<string>();

                                // Lit tant qu'il existe des lignes.
                                while (dataReader.Read())
                                {
                                    // Obtient les identifiants des notes sécurisées.
                                    scrNoteIDList.Add(dataReader["idScrNote"].ToString());
                                }

                                return scrNoteIDList;
                            }

                            return null;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetScrNoteIDList);
                return null;
            }
        }
        #endregion

        #region GetScrNoteNameList
        /// <summary>
        /// Obtient une liste des noms des notes sécurisées.
        /// </summary>
        /// <remarks>
        /// Obtient une liste des noms des notes sécurisées correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste des noms des notes sécurisées. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<string> GetScrNoteNameList(string _scrNoteID = "", string _categoryName = "", string _bookmark = "")
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

                    string querySelect = "SELECT [scrNoteName] FROM [tbl_scrNote] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne.
                            if (dataReader.HasRows)
                            {
                                List<string> scrNoteNameList = new List<string>();

                                // Lit tant qu'il existe des lignes.
                                while (dataReader.Read())
                                {
                                    // Obtient le nom des fiches d'authentification.
                                    scrNoteNameList.Add(dataReader["scrNoteName"].ToString());
                                }

                                return scrNoteNameList;
                            }

                            return null;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetScrNoteNameList);
                return null;
            }
        }
        #endregion

        #region GetScrNoteNumber

        /// <summary>
        /// Obtient le nombre de note sécurisées.
        /// </summary> 
        /// <param name="_categoryName">Nom d'une catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient le nombre de note sécurisées correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>Integer : Réussite -> Nombre de note sécurisées. Echec -> Valeur -1.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static int GetScrNoteNumber(string _categoryName = "", string _bookmark = "")
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string categoryName = _categoryName;
                    string bookmark = _bookmark;
                    string querySelect_ByCondition = string.Empty;

                    // Vérifie l'assignement d'un nom de catégorie.
                    if (categoryName != string.Empty && categoryName != Data_Authen.Default.AllItem)
                    {
                        string categoryID = GetCategoryID(categoryName);

                        querySelect_ByCondition = " AND idCategory=" + categoryID;
                    }

                    // Vérifie l'assignement d'un favoris.
                    if (bookmark != string.Empty)
                    {
                        querySelect_ByCondition = " AND scrNoteBookmark=" + bookmark;
                    }

                    string querySelect_Final = "SELECT COUNT([idScrNote]) AS scrNoteNbr FROM [tbl_scrNote] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect_Final, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            int scrNoteNumber = -1;

                            // Lit tant qu'il existe des lignes.
                            if (dataReader.Read())
                            {
                                // Obtient le nombre de note sécurisées.
                                scrNoteNumber = Convert.ToInt32(dataReader["scrNoteNbr"].ToString());
                            }

                            return scrNoteNumber;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetScrNoteNumber);
                return -1;
            }
        }
        #endregion

        #region SaveScrNote
        /// <summary>
        /// Enregistre une note sécurisée.
        /// </summary>
        /// <param name="_name">Nom</param>
        /// <param name="_content">Contenu</param>
        /// <param name="_categoryName">Numéro d'identifiant de la catégoie</param>
        /// <param name="_bookmark">Favoris</param>
        /// <param name="_creationDate">Date de création</param>
        /// <param name="_changeDate">Date de modification</param>
        /// <param name="_categoryName">Nom de la catégorie</param>
        /// <returns>Boolean : True -> Commande SQL exécutée. False -> Commande SQL non exécutée.</returns>
        /// <exception cref="SqlConnection, SqlCommand">
        /// Exception levée par l'objet SqlConnection ou SqlCommand.
        /// </exception>
        public static bool SaveScrNote(string _name, string _content, string _bookmark, string _creationDate, string _changeDate, string _categoryName)
        {
            try
            {
                bool success = false;

                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string queryInsert_1 = "INSERT INTO [tbl_scrNote]([scrNoteName], [scrNoteContent], [scrNoteBookmark], [scrNoteCreationDate], [scrNoteChangeDate], [idVault], [idCategory])";
                    string queryInsert_2 = " VALUES (@scrNoteName, @scrNoteContent, @scrNoteBookmark, @scrNoteCreationDate, @scrNoteChangeDate, @idVault, @idCategory);";
                    string queryInsert_Final = queryInsert_1 + queryInsert_2;

                    using (SqlCommand commandSQL = new SqlCommand(queryInsert_Final, connectionSQL))
                    {
                        const int NUMBER_LINE_ASSIGNS = 1;
                        List<string> scrNoteData = new List<string>();

                        // Assigne des données d'une note sécurisées à enregistrer.
                        scrNoteData.Add(_name);
                        scrNoteData.Add(_content);
                        scrNoteData.Add(_bookmark);
                        scrNoteData.Add(_creationDate);
                        scrNoteData.Add(_changeDate);
                        scrNoteData.Add(GetCategoryID(_categoryName));

                        byte[] cipheredScrNoteContent = Convert.FromBase64String(scrNoteData[1]);

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@scrNoteName", scrNoteData[0]);
                        commandSQL.Parameters.AddWithValue("@scrNoteContent", cipheredScrNoteContent);
                        commandSQL.Parameters.AddWithValue("@scrNoteBookmark", scrNoteData[2]);
                        commandSQL.Parameters.AddWithValue("@scrNoteCreationDate", scrNoteData[3]);
                        commandSQL.Parameters.AddWithValue("@scrNoteChangeDate", scrNoteData[4]);
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);
                        commandSQL.Parameters.AddWithValue("@idCategory", scrNoteData[5]);

                        // Exécute la commande SQL.
                        success = (commandSQL.ExecuteNonQuery() == NUMBER_LINE_ASSIGNS);
                    }
                }

                return success;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.SaveScrNote);
                return false;
            }
        }
        #endregion

        #region UpdateScrNoteCategory
        /// <summary>
        /// Met à jour une catégorie de notes sécurisées pour sa suppression.
        /// </summary>
        /// <param name="_categoryID">Identifiant de catégorie</param>
        private static void UpdateScrNoteCategory(string _categoryID)
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string queryUpdate = "UPDATE [tbl_scrNote] SET idCategory=1 WHERE [idVault]=@idVault AND [idCategory]=@idCategory;";

                    using (SqlCommand commandSQL = new SqlCommand(queryUpdate, connectionSQL))
                    {
                        string categoryID = _categoryID;

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idCategory", categoryID);
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        // Exécute la commande SQL.
                        commandSQL.ExecuteNonQuery();
                    }
                }

                return;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.UpdateScrNoteCategory);
                return;
            }
        }
        #endregion

        #region UpdateScrNoteData
        /// <summary>
        /// Met à jour les données d'une note sécurisée.
        /// </summary>
        /// <param name="_authenCardData">Liste de données d'une fiche d'authentification</param>
        /// <returns>Boolean : True -> Données mise à jour. False -> Valeur null.</returns>
        public static bool UpdateScrNoteData(List<string> _scrNoteData)
        {
            try
            {
                bool success = false;

                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string queryUpdate_1 = "UPDATE [tbl_scrNote] SET [scrNoteName]=@scrNoteName, [scrNoteContent]=@scrNoteContent,";
                    string queryupdate_2 = " [scrNoteBookmark]=@scrNoteBookmark, [scrNoteChangeDate]=@scrNoteChangeDate, [idCategory]=@idCategory";
                    string queryUpdate_3 = "  WHERE [idVault]=@idVault AND [idScrNote]=@idScrNote;";
                    string queryUpdate_Final = queryUpdate_1 + queryupdate_2 + queryUpdate_3;

                    using (SqlCommand commandSQL = new SqlCommand(queryUpdate_Final, connectionSQL))
                    {
                        const int NUMBER_LINE_ASSIGNS = 1;
                        List<string> scrNoteData = _scrNoteData;
                        string categoryID = VaultDatabase.GetCategoryID(scrNoteData[4]);

                        byte[] cipheredScrNoteContent = Convert.FromBase64String(scrNoteData[1]);

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@scrNoteName", scrNoteData[0]);
                        commandSQL.Parameters.AddWithValue("@scrNoteContent", cipheredScrNoteContent);
                        commandSQL.Parameters.AddWithValue("@scrNoteBookmark", scrNoteData[2]);
                        commandSQL.Parameters.AddWithValue("@scrNoteChangeDate", scrNoteData[3]);
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);
                        commandSQL.Parameters.AddWithValue("@idCategory", categoryID);
                        commandSQL.Parameters.AddWithValue("@idScrNote", scrNoteData[5]);

                        // Exécute la commande SQL.
                        success = (commandSQL.ExecuteNonQuery() == NUMBER_LINE_ASSIGNS);
                    }
                }

                return success;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.UpdateScrNoteData);
                return false;
            }
        }
        #endregion

        #endregion

    }
}
