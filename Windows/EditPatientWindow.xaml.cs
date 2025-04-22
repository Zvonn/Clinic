using System;
using System.Windows;
using Clinic;

namespace Clinic.Windows
{
    public partial class EditPatientWindow : Window
    {
        private db_clinicEntities _context = db_clinicEntities.GetContext();
        private patients _patient;

        public EditPatientWindow(patients patient)
        {
            InitializeComponent();
            _patient = patient;

            txtFirstName.Text = _patient.first_name;
            txtLastName.Text = _patient.last_name;
            dpDOB.SelectedDate = _patient.dob;
            txtGender.Text = _patient.gender;
            txtContact.Text = _patient.contact_info;
            txtAddress.Text = _patient.address;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _patient.first_name = txtFirstName.Text;
            _patient.last_name = txtLastName.Text;
            _patient.dob = dpDOB.SelectedDate ?? DateTime.Now;
            _patient.gender = txtGender.Text;
            _patient.contact_info = txtContact.Text;
            _patient.address = txtAddress.Text;

            _context.SaveChanges();
            MessageBox.Show("Пациент обновлён.");
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
