using Laboratory_Management_System.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratory_Management_System.Patient
{
    internal class PatientManager
    {
        private readonly Connection _connection;

        public PatientManager()
        {
            _connection = new Connection();
        }

        public int AddPatient(string name, int age, string gender, string telephone, string address)
        {
            int newId = -1;

            using (MySqlConnection conn = Connection.dataSource())
            {
                try
                {
                    _connection.connOpen(conn);

                    string insertQuery = "INSERT INTO patients (name, age, gender, telephone, address) VALUES (@Name, @Age, @Gender, @Telephone, @Address)";
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@Name", name);
                    insertCmd.Parameters.AddWithValue("@Age", age);
                    insertCmd.Parameters.AddWithValue("@Gender", gender);
                    insertCmd.Parameters.AddWithValue("@Telephone", telephone);
                    insertCmd.Parameters.AddWithValue("@Address", address);

                    insertCmd.ExecuteNonQuery();
                    newId = (int)insertCmd.LastInsertedId;

                    Console.WriteLine("Patient added successfully with ID: " + newId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while adding the patient: " + ex.Message);
                }
                finally
                {
                    _connection.connClose(conn);
                }
            }

            return newId;
        }

        public Model.Patient GetPatientDetails(int id)
        {
            Model.Patient patient = null;

            using (MySqlConnection conn = Connection.dataSource())
            {
                try
                {
                    _connection.connOpen(conn);

                    string query = "SELECT id, name, age, gender, telephone, address FROM patients WHERE id = @Id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        patient = new Model.Patient
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Age = reader.GetInt32("age"),
                            Gender = reader.GetString("gender"),
                            Telephone = reader.GetString("telephone"),
                            Address = reader.GetString("address")
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while retrieving the patient details: " + ex.Message);
                }
                finally
                {
                    _connection.connClose(conn);
                }
            }

            return patient;
        }

        private int GetNextTestId()
        {
            int nextTestId = 1;

            using (MySqlConnection conn = Connection.dataSource())
            {
                try
                {
                    _connection.connOpen(conn);

                    string query = "SELECT MAX(test_id) FROM test_results";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        nextTestId = Convert.ToInt32(result) + 1;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while retrieving the next test ID: " + ex.Message);
                }
                finally
                {
                    _connection.connClose(conn);
                }
            }

            return nextTestId;
        }

        public int SaveTestResults(int patientId, DataGridView dataGridView)
        {
            int testId = GetNextTestId();

            using (MySqlConnection conn = Connection.dataSource())
            {
                try
                {
                    _connection.connOpen(conn);

                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string testName = row.Cells["testNameColumn"].Value?.ToString();
                        string value = row.Cells["valueColumn"].Value?.ToString();
                        string range = row.Cells["rangeColumn"].Value?.ToString();
                        string result = row.Cells["resultColumn"].Value?.ToString();

                        if (!string.IsNullOrEmpty(testName) && !string.IsNullOrEmpty(value) &&
                            !string.IsNullOrEmpty(range) && !string.IsNullOrEmpty(result))
                        {
                            string insertQuery = "INSERT INTO test_results (patient_id, test_id, test_name, value, rang, result) VALUES (@PatientId, @TestId, @TestName, @Value, @Range, @Result)";
                            MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                            insertCmd.Parameters.AddWithValue("@PatientId", patientId);
                            insertCmd.Parameters.AddWithValue("@TestId", testId);
                            insertCmd.Parameters.AddWithValue("@TestName", testName);
                            insertCmd.Parameters.AddWithValue("@Value", value);
                            insertCmd.Parameters.AddWithValue("@Range", range);
                            insertCmd.Parameters.AddWithValue("@Result", result);

                            insertCmd.ExecuteNonQuery();
                        }
                    }

                    Console.WriteLine("Test results saved successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while saving the test results: " + ex.Message);
                }
                finally
                {
                    _connection.connClose(conn);
                }
            }
            return testId;
        }
    }
}
