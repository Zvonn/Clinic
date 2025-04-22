using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Clinic;

namespace Clinic.Pages
{
    public partial class AddDoctorPage : Page
    {
        private doctors _doctor = new doctors();
        private db_clinicEntities _context = db_clinicEntities.GetContext();

        public AddDoctorPage()
        {
            InitializeComponent();
            cmbSpecialization.ItemsSource = _context.speciallizations.ToList();
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(txtFirstName.Text)) errors.AppendLine("Введите имя!");
            if (string.IsNullOrWhiteSpace(txtLastName.Text)) errors.AppendLine("Введите фамилию!");
            if (cmbSpecialization.SelectedValue == null) errors.AppendLine("Выберите специализацию!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _doctor.first_name = txtFirstName.Text;
            _doctor.last_name = txtLastName.Text;
            _doctor.contact_info = txtContactInfo.Text;
            _doctor.speciallization_id = (int)cmbSpecialization.SelectedValue;

            _context.doctors.Add(_doctor);
            try
            {
                _context.SaveChanges();
                MessageBox.Show("Врач добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
