using Laboratory_Management_System.Data;
using Microsoft.VisualBasic.ApplicationServices;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Laboratory_Management_System.Auth
{
    internal class Auth
    {
        private readonly Connection _connection;

        public Auth()
        {
            _connection = new Connection();
        }
        public String getLabname(String email)
        {
            using (MySqlConnection conn = Connection.dataSource())
            {
                string labname = string.Empty;
                try
                {
                    _connection.connOpen(conn);
                    string query = "SELECT labname FROM users WHERE email = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            labname = reader["labname"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while retrieving the lab name: " + ex.Message);
                }
                finally
                {
                    _connection.connClose(conn);
                }

                return labname;
            }
        }


        public bool LoginUser(string email, string password)
        {
            bool isAuthenticated = false;

            using (MySqlConnection conn = Connection.dataSource())
            {
                try
                {
                    _connection.connOpen(conn);

                    string query = "SELECT COUNT(*) FROM users WHERE email = @Email AND password = @Password";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    object result = cmd.ExecuteScalar();

                    if (Convert.ToInt32(result) > 0)
                    {
                        Global.Global.labName = getLabname(email);
                
                        isAuthenticated = true;
                        
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
                finally
                {
                    _connection.connClose(conn);
                }
            }

            return isAuthenticated;
        }

        public void SignUpUser(string labname, string email, string password)
        {
            SignUpForm signUpForm = new SignUpForm();
            using (MySqlConnection conn = Connection.dataSource())
            {
                try
                {
                    _connection.connOpen(conn);

                    // Check if the email already exists
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE email = @Email";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Email", email);

                    object result = checkCmd.ExecuteScalar();

                    if (Convert.ToInt32(result) > 0)
                    {
                        Console.WriteLine("Email already exists. Please use a different email.");
                        signUpForm.errorLabel.Text = "Email already exists";
                        return;
                    }

                    // Insert new user into the database
                    string insertQuery = "INSERT INTO users (labname, email, password) VALUES (@Labname, @Email, @Password)";
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@Labname", labname);
                    insertCmd.Parameters.AddWithValue("@Email", email);
                    insertCmd.Parameters.AddWithValue("@Password", password);

                    insertCmd.ExecuteNonQuery();

                    Console.WriteLine("User registered successfully!");
                    signUpForm.errorLabel.Text = "";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred during registration: " + ex.Message);
                    signUpForm.errorLabel.Text = "An error occurred during registration: " + ex.Message;
                }
                finally
                {
                    _connection.connClose(conn);
                }
            }
        }
    }
}

