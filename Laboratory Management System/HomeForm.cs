using Laboratory_Management_System.Patient;
using Laboratory_Management_System.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Laboratory_Management_System
{
    public partial class HomeForm : Form
    {
        String result = "";
        int testId;
        public HomeForm()
        {
            InitializeComponent();
        }

        private void patRegBtn_Click(object sender, EventArgs e)
        {
            pageControl.SelectedTab = patRegPage;
        }

        private void testBtn_Click(object sender, EventArgs e)
        {
            pageControl.SelectedTab = testingPage;
        }

        private void historyBtn_Click(object sender, EventArgs e)
        {
            pageControl.SelectedTab = patHistoryPage;
        }

        

        private void registerBtn_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text.Length > 0 && ageTextBox.Text.Length > 0 && genderTextBox.Text.Length > 0 && TpTextBox.Text.Length > 0 && addressTextBox.Text.Length > 0)
            {
                Patient.PatientManager patientManager = new Patient.PatientManager();
                feedbackLabel.Text = "Patient registerd successfully with ID : "  + patientManager.AddPatient(nameTextBox.Text,int.Parse( ageTextBox.Text),genderTextBox.Text,TpTextBox.Text,addressTextBox.Text).ToString();
                errorLabel.Text = string.Empty;
            }
            else
            {
                errorLabel.Text = "fill the required fields";
            }
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
          
                nameTextBox.Text = string.Empty;
                ageTextBox.Text = string.Empty;
                genderTextBox.Text = string.Empty;
                TpTextBox.Text = string.Empty;
                addressTextBox.Text = string.Empty;
                feedbackLabel.Text = string.Empty;
                errorLabel.Text = string.Empty;
            
        }

        private void checkPatBtn_Click(object sender, EventArgs e)
        {
            Patient.PatientManager patientManager = new Patient.PatientManager();
            Model.Patient patient = patientManager.GetPatientDetails(int.Parse(testPatIdTextBox.Text));

            if (patient != null)
            {
                testNameTextBox.Text = patient.Name;
                testAgeTextBox.Text = patient.Age.ToString();
                testGenderTextBox.Text = patient.Gender;
                testTelephoneTextBox.Text = patient.Telephone;
                testAddressTextbox.Text = patient.Address;
            }
            else
            {
                MessageBox.Show("No patient found with the given ID");
                testNameTextBox.Text = string.Empty;
                testAgeTextBox.Text = string.Empty;
                testGenderTextBox.Text = string.Empty;
                testTelephoneTextBox.Text= string.Empty;
                testAddressTextbox.Text= string.Empty;
                testPatIdTextBox.Text = string.Empty;
                Console.WriteLine("No patient found with the given ID.");
            }
        }

        private void testComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Test.TestManager testManager = new Test.TestManager();
            int index = testComboBox.SelectedIndex;
            testDetailsLabel.Text = testManager.testNames[index] + "  " + testManager.minValues[index] + " - " + testManager.maxValues[index];
        }

        private void checkResultBtn_Click(object sender, EventArgs e)
        {
            Test.TestManager testManager = new Test.TestManager();
            int index = testComboBox.SelectedIndex;
            double value = double.Parse(resultTextBox.Text);
            double minValue = testManager.minValues[index];
            double maxValue = testManager.maxValues[index];

            if(value < minValue)
            {
                resultLabel.Text = "LOW";
                result = "LOW";
                resultLabel.ForeColor = Color.Red;
            }else if(value > maxValue)
            {
                resultLabel.Text = "HIGH";
                result = "HIGH";
                resultLabel.ForeColor= Color.Red;
            }
            else
            {
                resultLabel.Text = "NORMAL";
                result = "NORMAL";
                resultLabel.ForeColor = Color.Green;
            }

            addToReportBtn.Enabled = true;
        }

        private void addToReportBtn_Click(object sender, EventArgs e)
        {
            Test.TestManager testManager = new Test.TestManager();

            int index = testComboBox.SelectedIndex;
            String testName = testManager.testNames[index].ToString();
            String value = double.Parse(resultTextBox.Text).ToString();
            String minValue = testManager.minValues[index].ToString();
            String maxValue = testManager.maxValues[index].ToString();

            resultDataGridView.Rows.Add(testName,value,minValue+" - "+maxValue,result);
            addToReportBtn.Enabled = false;
        }

    

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font font = new Font("Arial", 10);
            Font headerFont = new Font("Arial", 12, FontStyle.Bold);
            Brush brush = Brushes.Black;
            Pen pen = Pens.Black;

            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;

            //Title
            e.Graphics.DrawString(Global.Global.labName, headerFont, brush, x, y);
            y += headerFont.GetHeight(e.Graphics);


            // Print patient details
            e.Graphics.DrawString("Patient Name: "+ testNameTextBox.Text, headerFont, brush, x, y);
            y += headerFont.GetHeight(e.Graphics);
            e.Graphics.DrawString("Patient ID: " + testPatIdTextBox.Text, font, brush, x, y);
            y += font.GetHeight(e.Graphics);
            e.Graphics.DrawString("Gender: " + testGenderTextBox.Text, font, brush, x, y);
            y += font.GetHeight(e.Graphics);
            e.Graphics.DrawString("Telephone: " + testTelephoneTextBox.Text, font, brush, x, y);
            y += font.GetHeight(e.Graphics);
            e.Graphics.DrawString("Address: " + testAddressTextbox.Text, font, brush, x, y);
            y += font.GetHeight(e.Graphics);
            e.Graphics.DrawString("Test ID: " + testId, font, brush, x, y);
            y += font.GetHeight(e.Graphics) * 2; // Extra space before the table

            // Calculate total width
            float totalWidth = 0;
            foreach (DataGridViewColumn column in resultDataGridView.Columns)
            {
                totalWidth += column.Width;
            }

            // Center the table
            float startX = e.MarginBounds.Left + (e.MarginBounds.Width - totalWidth) / 2;

            // Draw column headers
            x = startX;
            foreach (DataGridViewColumn column in resultDataGridView.Columns)
            {
                e.Graphics.DrawString(column.HeaderText, headerFont, brush, x, y);
                e.Graphics.DrawRectangle(pen, x, y, column.Width, headerFont.GetHeight(e.Graphics));
                x += column.Width;
            }

            y += headerFont.GetHeight(e.Graphics);

            // Draw rows
            foreach (DataGridViewRow row in resultDataGridView.Rows)
            {
                if (row.IsNewRow) continue;

                x = startX; // Reset x position for each row
                foreach (DataGridViewCell cell in row.Cells)
                {
                    e.Graphics.DrawString(cell.Value?.ToString(), font, brush, x, y);
                    e.Graphics.DrawRectangle(pen, x, y, cell.OwningColumn.Width, font.GetHeight(e.Graphics));
                    x += cell.OwningColumn.Width;
                }

                y += font.GetHeight(e.Graphics);
            }
        }

        private void PrintDataGridView()
        {
            // Set the event handler for PrintPage
            printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument1_PrintPage);

            // Configure the PrintDialog
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument1;

            // Show the PrintDialog and if the user clicks OK, print the document
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void siticoneRoundedButton2_Click(object sender, EventArgs e)
        {
            PrintDataGridView();
        }

        private void saveTestBtn_Click(object sender, EventArgs e)
        {
            PatientManager patientManager = new PatientManager();
            testId = patientManager.SaveTestResults(int.Parse(testPatIdTextBox.Text.ToString()), resultDataGridView);
        }
    }
    
}
