#region Application Description
/**********************************************************************\
*                            VirtualVault                             *
*                                                                     *
* Auteur : L. Lassalle                                                *
* Date de création : 02/02/2015                                       *
* Date de révision : 24/03/2015                                       *
* Description :                                                       *
* Application permettant le stockage sécurisé et l'organisation       *
* des identifiants de connexion et de notes personnelles sécrisées    *
* de l'utilisateur.                                                   *
\**********************************************************************/
#endregion

#region References
using System;
using System.Data.SqlClient;
using VirtualVault.Settings;

#endregion

namespace VirtualVault.Database
{
    public partial class VaultDatabase
    {

        // ScrNote

        #region Global Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        #region GetScrNoteNumber
        /// <summary>
        /// Obtient le nombre de note sécurisées.
        /// </summary>
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

                    string querySelect_Final = "SELECT COUNT([idScrNote]) AS scrNoteNbr FROM [tbl_scrNote] WHERE idVault=@idVault" + querySelect_ByCondition + ";";

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

                    string queryUpdate = "UPDATE [tbl_scrNote] SET idCategory=1 WHERE idVault=@idVault AND idCategory=@idCategory;";

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

        #endregion

    }
}
