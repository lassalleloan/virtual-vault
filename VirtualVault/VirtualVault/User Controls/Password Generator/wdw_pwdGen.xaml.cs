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
using System.Windows;
using System.Windows.Input;
using VirtualVault.Settings;
#endregion

namespace VirtualVault.User_Controls.Password_Generator
{
    /// <summary>
    /// Logique d'interaction pour wdw_pwdGen.xaml
    /// </summary>
    public partial class wdw_pwdGen : Window
    {

        #region Global Variables
        private static wdw_pwdGen _instance = null;
        #endregion

        #region Constructors

        #region wdw_pwdGen
        /// <summary>
        /// Initialise une nouvelle instance unique de la classe wdw_pwdGen.
        /// </summary>
        public wdw_pwdGen()
        {
            // Vérifie l'existance d'une unique instance.
            if (_instance == null)
            {
                InitializeComponent();

                // Affiche la fenêtre et assigne l'instance à cette fenêtre.
                wdw_pwdGenPwdGen.Title = Data_PwdGen.Default.PwdGenTitle;
                wdw_pwdGenPwdGen.Show();
                _instance = wdw_pwdGenPwdGen;
            }
            else
            {
                // Assigne l'instance au premier plan.
                _instance.Focus();
            }

            return;
        }
        #endregion

        #endregion

        #region Events

        #region cmd_copyPwd_Click
        /// <summary>
        /// Action lors du clic sur le bouton "cmd_copyPwd".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_copyPwd_Click(object sender, RoutedEventArgs e)
        {
            // Enregistre des données dans le presse-papier. 
            Clipboard.SetData(DataFormats.Text, (Object)txt_password.Text);
            return;
        }
        #endregion

        #region cmd_generate_Click
        /// <summary>
        /// Action lors du clic sur le bouton "cmd_generate".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_generate_Click(object sender, RoutedEventArgs e)
        {
            bool chk_uprCaseChecked = Convert.ToBoolean(chk_uprCase.IsChecked);
            bool chk_lwrCaseChecked = Convert.ToBoolean(chk_lwrCase.IsChecked);
            bool chk_numberChecked = Convert.ToBoolean(chk_number.IsChecked);
            bool chk_symbolChecked = Convert.ToBoolean(chk_symbol.IsChecked);
            int passwordLength = Convert.ToInt32(sld_pwdLength.Value);

            lbl_message.Content = string.Empty;

            // Assigne à des propriétés, des entrées utilisateurs.
            PwdGen.SaveInputs(chk_uprCaseChecked, chk_lwrCaseChecked, chk_numberChecked, chk_symbolChecked, passwordLength);

            // Vérifie le cochage de cases à cocher.
            if (PwdGen.IsCheckedAnyCase())
            {
                lbl_message.Content = Data_PwdGen.Default.IsCheckedAnyCase;
                return;
            }
            else
            {
                // Assigne le mot de passe généré et change la valeur de la bar de progression.
                txt_password.Text = PwdGen.GetRandomPwd(pgr_cpx);
                PwdGen.ChangeColorByValue(pgr_cpx, lbl_cpxCmt);
            }
        }
        #endregion

        #region wdw_pwdGen_Closing
        /// <summary>
        /// Action lors de la fermeture de la window "wdw_pwdGen".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wdw_pwdGen_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Met à null la valeur de l'instance.
            _instance = null;
        }
        #endregion

        #region wdw_pwdGen_KeyUp
        /// <summary>
        /// Action lors de la remontée d'une touche du clavier.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wdw_pwdGen_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            bool isKeyEnterPess = (e.Key == Key.Enter);

            // Vérifie la remontée de la touche Enter.
            if (isKeyEnterPess)
            {
                cmd_generate_Click(sender, e);
                return;
            }
        }
        #endregion

        #region wdw_pwdGen_Loaded
        /// <summary>
        /// Action lors du chargement de l'interface.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wdw_pwdGen_Loaded(object sender, RoutedEventArgs e)
        {
            // Assigne le focus au champs de saisie du nom d'utilisateur.
            cmd_generate.Focus();
        }
        #endregion

        #endregion

    }
}
