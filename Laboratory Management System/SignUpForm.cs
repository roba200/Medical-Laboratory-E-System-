using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratory_Management_System
{
    public partial class SignUpForm : Form
    {
        public SignUpForm()
        {
            InitializeComponent();
        }

        private void signUpButton_Click(object sender, EventArgs e)
        {
            Auth.Auth auth = new Auth.Auth();

            // Sign up a new user
            if (labNameTextBox.Text == "" || emailTextBox.Text == "" || passwordTextBox.Text == "" || confirmPasswordTextBox.Text == "")
            {
                errorLabel.Text = "Fields can't be empty";
            }
            else
            {
                if (passwordTextBox.Text == confirmPasswordTextBox.Text)
                {
                    auth.SignUpUser(labNameTextBox.Text, emailTextBox.Text, passwordTextBox.Text);
                    errorLabel.Text = "";
                    HomeForm homeForm = new HomeForm();
                    homeForm.Show();
                    this.Hide();
                }
                else
                {
                    errorLabel.Text = "Passwords do not match";
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void logInHereBtn_Click(object sender, EventArgs e)
        {
            LogIn logIn = new LogIn();
            logIn.Show();
            this.Hide();
        }
    }
}
