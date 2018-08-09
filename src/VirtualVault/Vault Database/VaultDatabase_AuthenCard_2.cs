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
        // AuthenCard_2

        #region Methods

        #region GetACardNameList
        /// <summary>
        /// Obtient une liste des noms des fiches d'authentification.
        /// </summary>
        /// <param name="_authenCardID">Identifiant de fichd'authentification</param>
        /// <param name="_categoryName">Nom de catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient une liste des noms des fiches d'authentification correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste des noms des fiches d'authentification. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<string> GetACardNameList(string _authenCardID = "", string _categoryName = "", string _bookmark = "")
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

                    string querySelect = "SELECT [authenCardName] FROM [tbl_authenCard] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne.
                            if (dataReader.HasRows)
                            {
                                List<string> aCardsNameList = new List<string>();

                                // Lit tant qu'il existe des lignes.
                                while (dataReader.Read())
                                {
                                    // Obtient le nom des fiches d'authentification.
                                    aCardsNameList.Add(dataReader["authenCardName"].ToString());
                                }

                                return aCardsNameList;
                            }

                            return null;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetACardNameList);
                return null;
            }
        }
        #endregion

        #region GetACardNumber
        /// <summary>
        /// Obtient le nombre de fiche d'authentification.
        /// </summary>
        /// <param name="_categoryName">Nom de catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient le nombre de fiche d'authentification correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>Integer : Réussite -> Nombre de fiche d'authentification. Echec -> Valeur -1.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static int GetACardNumber(string _categoryName = "", string _bookmark = "")
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

                    string querySelect = "SELECT COUNT([idAuthenCard]) AS nbrAuthenCard FROM [tbl_authenCard] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            int aCardNumber = -1;

                            // Vérifie si la commande SQL retourne au moins une ligne et lit tant qu'il existe des lignes.
                            if (dataReader.HasRows && dataReader.Read())
                            {
                                // Obtient le nombre de fiche d'authentification.
                                aCardNumber = Convert.ToInt32(dataReader["nbrAuthenCard"].ToString());
                            }

                            return aCardNumber;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetACardNumber);
                return -1;
            }
        }
        #endregion

        #region GetACardShcList
        /// <summary>
        /// Obtient une liste des raccourcis des fiches d'authentification.
        /// </summary>
        /// <param name="_authenCardID">Identifiant de fichd'authentification</param>
        /// <param name="_categoryName">Nom de catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient une liste des raccourcis des fiches d'authentification correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste des raccourcis des fiches d'authentification. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<string> GetACardShcList(string _authenCardID = "", string _categoryName = "", string _bookmark = "")
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

                    string querySelect = "SELECT [authenCardShortcut] FROM [tbl_authenCard] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne.
                            if (dataReader.HasRows)
                            {
                                List<string> aCardsShortcut = new List<string>();

                                // Lit tant qu'il existe des lignes.
                                while (dataReader.Read())
                                {
                                    // Obtient le raccourci des fiches d'authentification.
                                    aCardsShortcut.Add(dataReader["authenCardShortcut"].ToString());
                                }

                                return aCardsShortcut;
                            }

                            return null;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetACardShcList);
                return null;
            }
        }
        #endregion

        #region GetACardUsnList
        /// <summary>
        /// Obtient une liste des noms d'utilisateurs des fiches d'authentification.
        /// </summary>
        /// <param name="_authenCardID">Identifiant de fichd'authentification</param>
        /// <param name="_categoryName">Nom de catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient une liste des noms d'utilisateurs des fiches d'authentification correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste des noms d'utilisateurs des fiches d'authentification. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<byte[]> GetACardUsnList(string _authenCardID = "", string _categoryName = "", string _bookmark = "")
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

                    string querySelect = "SELECT [authenCardUsername] FROM [tbl_authenCard] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne.
                            if (dataReader.HasRows)
                            {
                                List<byte[]> authenCardUsnList = new List<byte[]>();

                                // Lit tant qu'il existe des lignes.
                                while (dataReader.Read())
                                {
                                    // Obtient le nom d'utilisateur des fiches d'authentification.
                                    authenCardUsnList.Add((byte[])dataReader["authenCardUsername"]);
                                }

                                return authenCardUsnList;
                            }

                            return null;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetACardUsnList);
                return null;
            }
        }
        #endregion

        #region GetACardPwdList
        /// <summary>
        /// Obtient une liste des mots de passe des fiches d'authentification.
        /// </summary>
        /// <param name="_authenCardID">Identifiant de fichd'authentification</param>
        /// <param name="_categoryName">Nom de catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient une liste des mots de passe des fiches d'authentification correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste des mots de passe des fiches d'authentification. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<byte[]> GetACardPwdList(string _authenCardID = "", string _categoryName = "", string _bookmark = "")
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

                    // Vérifie l'assignement d'un favoris.s
                    if (bookmark != string.Empty)
                    {
                        querySelect_ByCondition = " AND [authenCardBookmark]=" + bookmark;
                    }

                    string querySelect = "SELECT [authenCardPassword] FROM [tbl_authenCard] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne.
                            if (dataReader.HasRows)
                            {
                                List<byte[]> authenCardPwdList = new List<byte[]>();

                                // Lit tant qu'il existe des lignes.
                                while (dataReader.Read())
                                {
                                    // Obtient le nom d'utilisateur des fiches d'authentification.
                                    authenCardPwdList.Add((byte[])dataReader["authenCardPassword"]);
                                }

                                return authenCardPwdList;
                            }

                            return null;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetACardUsnList);
                return null;
            }
        }
        #endregion

        #region GetACardScrNoteList
        /// <summary>
        /// Obtient une liste des noms d'utilisateurs des fiches d'authentification.
        /// </summary>
        /// <param name="_authenCardID">Identifiant de fichd'authentification</param>
        /// <param name="_categoryName">Nom de catégorie</param>
        /// <param name="_bookmark">Statut du favoris</param>
        /// <remarks>
        /// Obtient une liste des noms d'utilisateurs des fiches d'authentification correspondant au même coffre-fort virtuel.
        /// </remarks>
        /// <returns>List : Réussite -> Liste des noms d'utilisateurs des fiches d'authentification. Echec -> Valeur null.</returns>
        /// <exception cref="SqlConnection, SqlCommand, SqlDataReader">
        /// Exception levée par l'objet SqlConnection, SqlCommand ou SqlDataReader.
        /// </exception>
        public static List<byte[]> GetACardScrNoteList(string _authenCardID = "", string _categoryName = "", string _bookmark = "")
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

                    // Vérifie l'assignement d'un favoris.s
                    if (bookmark != string.Empty)
                    {
                        querySelect_ByCondition = " AND [authenCardBookmark]=" + bookmark;
                    }

                    string querySelect = "SELECT [authenCardScrNote] FROM [tbl_authenCard] WHERE [idVault]=@idVault" + querySelect_ByCondition + ";";

                    using (SqlCommand commandSQL = new SqlCommand(querySelect, connectionSQL))
                    {
                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);

                        using (SqlDataReader dataReader = commandSQL.ExecuteReader())
                        {
                            // Vérifie si la commande SQL retourne au moins une ligne.
                            if (dataReader.HasRows)
                            {
                                List<byte[]> authenCardUsnList = new List<byte[]>();

                                // Lit tant qu'il existe des lignes.
                                while (dataReader.Read())
                                {
                                    // Obtient le nom d'utilisateur des fiches d'authentification.
                                    authenCardUsnList.Add((byte[])dataReader["authenCardScrNote"]);
                                }

                                return authenCardUsnList;
                            }

                            return null;
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.GetACardScrNoteList);
                return null;
            }
        }
        #endregion

        #region SaveAuthenCard
        /// <summary>
        /// Enregistre une fiche d'authentification.
        /// </summary>
        /// <param name="_name">Nom d'une fiche d'authentification</param>
        /// <param name="_shortcut">Raccourci d'une fiche d'authentification</param>
        /// <param name="_username">nom d'utilisateur d'une fiche d'authentification</param>
        /// <param name="_password">Mot de passe d'une fiche d'authentification</param>
        /// <param name="_cpxFactor">Facteur de complexité d'une fiche d'authentification</param>
        /// <param name="_scrNote">Note séécurisée d'une fiche d'authentification</param>
        /// <param name="_bookmark">Favoris d'une fiche d'authentification</param>
        /// <param name="_creationDate">Date de création d'une fiche d'authentification</param>
        /// <param name="_changeDate">Date de modification d'une fiche d'authentification</param>
        /// <param name="_categoryName">Nom de la catégorie d'une fiche d'authentification</param>
        /// <returns>Boolean : True -> Commande SQL exécutée. False -> Commande SQL non exécutée.</returns>
        /// <exception cref="SqlConnection, SqlCommand">
        /// Exception levée par l'objet SqlConnection ou SqlCommand.
        /// </exception>
        public static bool SaveAuthenCard(string _name, string _shortcut, string _username, string _password, string _cpxFactor,
                                        string _scrNote, string _bookmark, string _creationDate, string _changeDate, string _categoryName)
        {
            try
            {
                bool success = false;

                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string queryInsert_1 = "INSERT INTO [tbl_authenCard]([authenCardName], [authenCardShortcut], [authenCardUsername],  [authenCardPassword], [authenCardComplexity],";
                    string queryInsert_2 = " [authenCardScrNote], [authenCardBookmark], [authenCardCreationDate], [authenCardChangeDate], [idVault], [idCategory])";
                    string queryInsert_3 = " VALUES (@authenCardName, @authenCardShortcut, @authenCardUsername, @authenCardPassword, @authenCardComplexity,";
                    string queryInsert_4 = " @authenCardScrNote, @authenCardBookmark, @authenCardCreationDate, @authenCardChangeDate, @idVault, @idCategory);";
                    string queryInsert_Final = queryInsert_1 + queryInsert_2 + queryInsert_3 + queryInsert_4;

                    using (SqlCommand commandSQL = new SqlCommand(queryInsert_Final, connectionSQL))
                    {
                        const int NUMBER_LINE_ASSIGNS = 1;
                        List<string> authenCardData = new List<string>();

                        // Assigne des données d'une fiche d'authentification à enregistrer.
                        authenCardData.Add(_name);
                        authenCardData.Add(_shortcut);
                        authenCardData.Add(_username);
                        authenCardData.Add(_password);
                        authenCardData.Add(_cpxFactor);
                        authenCardData.Add(_scrNote);
                        authenCardData.Add(_bookmark);
                        authenCardData.Add(_creationDate);
                        authenCardData.Add(GetCategoryID(_categoryName));

                        byte[] cipheredACardUsn = Convert.FromBase64String(authenCardData[2]);
                        byte[] cipheredACardPwd = Convert.FromBase64String(authenCardData[3]);
                        byte[] cipheredAcardScrNote = Convert.FromBase64String(authenCardData[5]);

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@authenCardName", authenCardData[0]);
                        commandSQL.Parameters.AddWithValue("@authenCardShortcut", authenCardData[1]);
                        commandSQL.Parameters.AddWithValue("@authenCardUsername", cipheredACardUsn);
                        commandSQL.Parameters.AddWithValue("@authenCardPassword", cipheredACardPwd);
                        commandSQL.Parameters.AddWithValue("@authenCardComplexity", authenCardData[4]);
                        commandSQL.Parameters.AddWithValue("@authenCardScrNote", cipheredAcardScrNote);
                        commandSQL.Parameters.AddWithValue("@authenCardBookmark", authenCardData[6]);
                        commandSQL.Parameters.AddWithValue("@authenCardCreationDate", authenCardData[7]);
                        commandSQL.Parameters.AddWithValue("@authenCardChangeDate", authenCardData[7]);
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);
                        commandSQL.Parameters.AddWithValue("@idCategory", authenCardData[8]);

                        // Exécute la commande SQL.
                        success = (commandSQL.ExecuteNonQuery() == NUMBER_LINE_ASSIGNS);
                    }
                }

                return success;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.SaveAuthenCard);
                return false;
            }
        }
        #endregion

        #region UpdateACardCategory
        /// <summary>
        /// Met à jour une catégorie de fiche d'authentification pour sa suppression.
        /// </summary>
        /// <param name="_categoryID">Identifiant de catégorie</param>
        private static void UpdateACardCategory(string _categoryID)
        {
            try
            {
                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string queryUpdate = "UPDATE [tbl_authenCard] SET [idCategory]=1 WHERE [idVault]=@idVault AND [idCategory]=@idCategory;";

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
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.UpdateACardCategory);
                return;
            }
        }
        #endregion

        #region UpdateAuthenCardData
        /// <summary>
        /// Met à jour les données d'une fiche d'authentification.
        /// </summary>
        /// <param name="_authenCardData">Liste de données d'une fiche d'authentification</param>
        /// <returns>Boolean : True -> Données mise à jour. False -> Valeur null.</returns>
        public static bool UpdateAuthenCardData(List<string> _authenCardData)
        {
            try
            {
                bool success = false;

                using (SqlConnection connectionSQL = new SqlConnection(Data_VaultDatabase.Default.VaultDatabaseConnectionString))
                {
                    connectionSQL.Open();

                    string queryUpdate_1 = "UPDATE [tbl_authenCard] SET [authenCardName]=@authenCardName, [authenCardShortcut]=@authenCardShortcut, [authenCardUsername]=@authenCardUsername,";
                    string queryUpdate_2 = " [authenCardPassword]=@authenCardPassword, [authenCardComplexity]=@authenCardComplexity, [authenCardScrNote]=@authenCardScrNote, ";
                    string queryUpdate_3 = "[authenCardBookmark]=@authenCardBookmark, [authenCardChangeDate]=@authenCardChangeDate, [idCategory]=@idCategory";
                    string queryUpdate_4 = " WHERE [idVault]=@idVault AND [idAuthenCard]=@idAuthenCard;";
                    string queryUpdate_Final = queryUpdate_1 + queryUpdate_2 + queryUpdate_3 + queryUpdate_4;

                    using (SqlCommand commandSQL = new SqlCommand(queryUpdate_Final, connectionSQL))
                    {
                        const int NUMBER_LINE_ASSIGNS = 1;
                        List<string> authenCardData = _authenCardData;
                        string categoryID = VaultDatabase.GetCategoryID(authenCardData[8]);

                        byte[] cipheredACardUsername = Convert.FromBase64String(authenCardData[2]);
                        byte[] cipheredACardPassword = Convert.FromBase64String(authenCardData[3]);
                        byte[] cipheredACardScrNote = Convert.FromBase64String(authenCardData[5]);

                        // Ajoute une valeur à l'endroit spécifié dans la commande SQL.
                        commandSQL.Parameters.AddWithValue("@authenCardName", authenCardData[0]);
                        commandSQL.Parameters.AddWithValue("@authenCardShortcut", authenCardData[1]);
                        commandSQL.Parameters.AddWithValue("@authenCardUsername", cipheredACardUsername);
                        commandSQL.Parameters.AddWithValue("@authenCardPassword", cipheredACardPassword);
                        commandSQL.Parameters.AddWithValue("@authenCardComplexity", authenCardData[4]);
                        commandSQL.Parameters.AddWithValue("@authenCardScrNote", cipheredACardScrNote);
                        commandSQL.Parameters.AddWithValue("@authenCardBookmark", authenCardData[6]);
                        commandSQL.Parameters.AddWithValue("@authenCardChangeDate", authenCardData[7]);
                        commandSQL.Parameters.AddWithValue("@idVault", VaultID);
                        commandSQL.Parameters.AddWithValue("@idCategory", categoryID);
                        commandSQL.Parameters.AddWithValue("@idAuthenCard", authenCardData[9]);

                        // Exécute la commande SQL.
                        success = (commandSQL.ExecuteNonQuery() == NUMBER_LINE_ASSIGNS);
                    }
                }

                return success;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(Data_VaultDatabase.Default.UpdateAuthenCardData);
                return false;
            }
        }
        #endregion

        #endregion

    }
}

