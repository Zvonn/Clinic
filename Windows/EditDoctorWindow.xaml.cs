using System;
using System.Linq;
using System.Windows;
using Clinic;

namespace Clinic.Windows
{
    public partial class EditDoctorWindow : Window
    {
        private doctors _doctor;
        private db_clinicEntities _context = db_clinicEntities.GetContext();

        public EditDoctorWindow(doctors selectedDoctor)
        {
            InitializeComponent();
            _doctor = selectedDoctor;

            // Заполняем поля
            txtFirstName.Text = _doctor.first_name;
            txtLastName.Text = _doctor.last_name;
            txtContactInfo.Text = _doctor.contact_info;

            // Специализации
            cmbSpecialization.ItemsSource = _context.speciallizations.ToList();
            cmbSpecialization.SelectedValue = _doctor.speciallization_id;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                cmbSpecialization.SelectedValue == null)
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _doctor.first_name = txtFirstName.Text;
            _doctor.last_name = txtLastName.Text;
            _doctor.contact_info = txtContactInfo.Text;
            _doctor.speciallization_id = (int)cmbSpecialization.SelectedValue;

            _context.SaveChanges();
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
