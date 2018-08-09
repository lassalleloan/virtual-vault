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
        // AuthenCard_1

        #region Methods

        #region CheckAuthenCard
        /// <summary>
        /// Vérifie l'existance du nom d'une fiche d'authentification.
        /// </summary>
        /// <param name="_authenCardName">Nom d'une fiche d'authentification</param>
        /// <remarks>
        /// Vérifie l'existance du nom d'une fiche d'authentification correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>Boolean : True -> Nom déjà existant. False -> Nom inexistant.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static bool CheckAuthenCard(string _authenCardName)
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string querySelect = "SELECT [authenCardName] FROM [tbl_authenCard] WHERE [idVault]=@idVault AND [authenCardName]=@authenCardName;";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        string authenCardName = _authenCardName;

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);
                        commandSQL.Parameters.AddWithValue("@authenCardName", authenCardName);

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
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.CheckAuthenCard);
                return false;
            }
        }
        #endregion

        #region DeleteAuthenCard
        /// <summary>
        /// Supprime une fiche d'authentification.
        /// </summary>
        /// <param name="_authenCardID">Identifiant de fiche d'authentificaition</param>
        /// <returns>Boolean : True -> Commande SQL exécutée. False -> Commande SQL non exécutée.</returns>
        /// <exception cref="SqlConnection, SqlCommand">
        /// Exception levée par l'objet SqlConnection ou SqlCommand.
        /// </exception>
        public static bool DeleteAuthenCard(string _authenCardID)
        {
            try
            {
                bool success = false;

                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string queryDelete = "DELETE FROM [tbl_authenCard] WHERE [idVault]=@idVault AND [idAuthenCard]=@idAuthenCard;";

                    using (SqlCommand commandSQL = new SqlCommand(queryDelete, connectionSQL))
                    {
                        const int NUMBER_LINE_ASSIGNS = 1;
                        string authenCardID = _authenCardID;

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idAuthenCard", authenCardID);
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        // Exécute la commande SQL.
                        success = (commandSQL.ExecuteNonQuery() == NUMBER_LINE_ASSIGNS);
                    }
                }

                return success;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.DeleteAuthenCard);
                return false;
            }
        }
        #endregion
        
        #region GetACardBookmark
        /// <summary>
        /// Obtient le statut du favoris des fiches d'authentification.
        /// </summary>
        /// <param name="_authenCardID">Identifiant de fichd'authentification</param>
        /// <param name="_categoryName">Nom de catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient une liste des statuts du favoris des fiches d'authentification correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste des statuts du favoris des fiches d'authentification. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static bool GetACardBookmark(string _authenCardID = "", string _categoryName = "", string _bookmark = "")
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string authenCardID = _authenCardID;
                    string categoryName = _categoryName;
                    string bookmark = _bookmark;
                    string querySelect_ByCondition = string.Empty;

                    // Vérifie l'assignement d'un identifiant de fiche d'authentification.
                    if (authenCardID != string.Empty)
                    {
                        querySelect_ByCondition = " AND [idAuthenCard]=" + authenCardID;
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
                        querySelect_ByCondition = " AND [authenCardBookmark]=" + bookmark;
                    }

                    string querySelect = "SELECT [authenCardBookmark] FROM [tbl_authenCard] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

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
                                    scrNoteBookmark = Convert.ToBoolean(dataReader["authenCardBookmark"]);
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

        #region GetACardCategory
        /// <summary>
        /// Obtient une liste des catégories des fiches d'authentification.
        /// </summary>
        /// <param name="_authenCardID">Identifiant de fichd'authentification</param>
        /// <param name="_categoryName">Nom de catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient une liste des catégories des fiches d'authentification correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste des catégories des fiches d'authentification. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<string> GetACardCategory(string _authenCardID = "", string _categoryName = "", string _bookmark = "")
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string authenCardID = _authenCardID;
                    string categoryName = _categoryName;
                    string bookmark = _bookmark;
                    string querySelect_ByCondition = string.Empty;

                    // Vérifie l'assignement d'un identifiant de fiche d'authentification.
                    if (authenCardID != string.Empty)
                    {
                        querySelect_ByCondition = " AND [idAuthenCard]=" + authenCardID;
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
                        querySelect_ByCondition = " AND [authenCardBookmark]=" + bookmark;
                    }

                    string querySelect = "SELECT [idCategory] FROM [tbl_authenCard] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

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
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetACardCategory);
                return null;
            }
        }
        #endregion

        #region GetACardChgDate
        /// <summary>
        /// Obtient une liste des dates de modification des fiches d'authentification.
        /// </summary>
        /// <param name="_authenCardID">Identifiant de fiche d'authentification</param>
        /// <param name="_categoryName">Nom de catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient une liste des dates de modification des fiches d'authentification correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste des dates de modification des fiches d'authentification. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<string> GetACardChgDate(string _authenCardID = "", string _categoryName = "", string _bookmark = "")
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string authenCardID = _authenCardID;
                    string categoryName = _categoryName;
                    string bookmark = _bookmark;
                    string querySelect_ByCondition = string.Empty;

                    // Vérifie l'assignement d'un identifiant de fiche d'authentification.
                    if (authenCardID != string.Empty)
                    {
                        querySelect_ByCondition = " AND [idAuthenCard]=" + authenCardID;
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
                        querySelect_ByCondition = " AND [authenCardBookmark]=" + bookmark;
                    }

                    string querySelect = "SELECT [authenCardChangeDate] FROM [tbl_authenCard] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

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
                                    scrNoteBookmark.Add(dataReader["authenCardChangeDate"].ToString());
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
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetACardChgDate);
                return null;
            }
        }
        #endregion

        #region GetACardCpxFactorList
        /// <summary>
        /// Obtient une liste des facteurs de complexité des fiches d'authentification.
        /// </summary>
        /// <param name="_authenCardID">Identifiant de fichd'authentification</param>
        /// <param name="_categoryName">Nom de catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient une liste des facteurs de complexité des fiches d'authentification correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste des facteurs de complexité des fiches d'authentification. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<string> GetACardCpxFactorList(string _authenCardID = "", string _categoryName = "", string _bookmark = "")
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string authenCardID = _authenCardID;
                    string categoryName = _categoryName;
                    string bookmark = _bookmark;
                    string querySelect_ByCondition = string.Empty;

                    // Vérifie l'assignement d'un identifiant de fiche d'authentification.
                    if (authenCardID != string.Empty)
                    {
                        querySelect_ByCondition = " AND [idAuthenCard]=" + authenCardID;
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
                        querySelect_ByCondition = " AND [authenCardBookmark]=" + bookmark;
                    }

                    string querySelect = "SELECT [authenCardComplexity] FROM [tbl_authenCard] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne.
                            if (dataReader.HasRows)
                            {
                                List<string> aCardsCpxList = new List<string>();

                                // Lit tant qu'il existe des lignes.
                                while (dataReader.Read())
                                {
                                    // Obtient le facteur de complexité des fiches d'authentification.
                                    aCardsCpxList.Add(dataReader["authenCardComplexity"].ToString());
                                }

                                return aCardsCpxList;
                            }

                            return null;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetACardCpxFactorList);
                return null;
            }
        }
        #endregion

        #region GetACardCrtDate
        /// <summary>
        /// Obtient une liste des dates de créations des fiches d'authentification.
        /// </summary>
        /// <param name="_authenCardID">Identifiant de fichd'authentification</param>
        /// <param name="_categoryName">Nom de catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient une liste des dates de créations des fiches d'authentification correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste des dates de créations des fiches d'authentification. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<string> GetACardCrtDate(string _authenCardID = "", string _categoryName = "", string _bookmark = "")
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string authenCardID = _authenCardID;
                    string categoryName = _categoryName;
                    string bookmark = _bookmark;
                    string querySelect_ByCondition = string.Empty;

                    // Vérifie l'assignement d'un identifiant de fiche d'authentification.
                    if (authenCardID != string.Empty)
                    {
                        querySelect_ByCondition = " AND [idAuthenCard]=" + authenCardID;
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
                        querySelect_ByCondition = " AND [authenCardBookmark]=" + bookmark;
                    }

                    string querySelect = "SELECT [authenCardCreationDate] FROM [tbl_authenCard] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

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
                                    scrNoteBookmark.Add(dataReader["authenCardCreationDate"].ToString());
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
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetACardCrtDate);
                return null;
            }
        }
        #endregion

        #region GetACardIDList
        /// <summary>
        /// Obtient une liste des identifiants des fiches d'authentification.
        /// </summary>
        /// <param name="_authenCardID">Identifiant de fichd'authentification</param>
        /// <param name="_categoryName">Nom de catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient une liste des identifiants des fiches d'authentification correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste des noms des fiches d'authentification. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<string> GetACardIDList(string _categoryName = "", string _bookmark = "")
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

                        querySelect_ByCondition = " AND [idCategory]=" + categoryID;
                    }

                    // Vérifie l'assignement d'un favoris.
                    if (bookmark != string.Empty)
                    {
                        querySelect_ByCondition = " AND [authenCardBookmark]=" + bookmark;
                    }

                    string querySelect = "SELECT [idAuthenCard] FROM [tbl_authenCard] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne.
                            if (dataReader.HasRows)
                            {
                                List<string> aCardIDList = new List<string>();

                                // Lit tant qu'il existe des lignes.
                                while (dataReader.Read())
                                {
                                    // Obtient les identifiants des fiches d'authentification.
                                    aCardIDList.Add(dataReader["idAuthenCard"].ToString());
                                }

                                return aCardIDList;
                            }

                            return null;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetACardIDList);
                return null;
            }
        }
        #endregion

        #endregion

    }
}
