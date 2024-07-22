using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Laboratory_Management_System.Auth;

namespace Laboratory_Management_System
{
    
    public partial class LogIn : Form
    {
        
        public LogIn()
        {
            InitializeComponent();
            
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void logInbtn_Click(object sender, EventArgs e)
        {

            Auth.Auth auth = new Auth.Auth();
            if (emailTextBox.Text == "" || passwordTextBox.Text == "")
            {
                errorLabel.Text = "Fields Can't be Empty";
            }
            else
            {
                bool isAuthenticated = auth.LoginUser(emailTextBox.Text, passwordTextBox.Text);
               

                if (isAuthenticated)
                {
                    Console.WriteLine("Login successful!");
                    errorLabel.Text = "";
                    HomeForm homeForm = new HomeForm();
                    homeForm.Show();
                    this.Hide();
                }
                else
                {
                    Console.WriteLine("Login failed!");
                    errorLabel.Text = "Incorrect Email or Password";
                }
            }
        }

        private void signUpHereBtn_Click(object sender, EventArgs e)
        {
            SignUpForm signUpForm = new SignUpForm();
            signUpForm.Show();
            this.Hide();
        }
    }
}
