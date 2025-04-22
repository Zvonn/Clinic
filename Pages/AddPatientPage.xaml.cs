using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Clinic; // db_clinicEntities и модели

namespace Clinic.Pages
{
    public partial class AddPatientPage : Page
    {
        private db_clinicEntities _context = db_clinicEntities.GetContext();

        public AddPatientPage()
        {
            InitializeComponent();
            // Подготовка полей
            cmbGender.ItemsSource = new[] { "М", "Ж" };
            dpDOB.SelectedDate = DateTime.Today;
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(txtFirstName.Text)) errors.AppendLine("Введите имя!");
            if (string.IsNullOrWhiteSpace(txtLastName.Text)) errors.AppendLine("Введите фамилию!");
            if (dpDOB.SelectedDate == null) errors.AppendLine("Выберите дату рождения!");
            if (cmbGender.SelectedItem == null) errors.AppendLine("Выберите пол!");
            if (string.IsNullOrWhiteSpace(txtContact.Text)) errors.AppendLine("Введите контактную информацию!");
            if (string.IsNullOrWhiteSpace(txtAddress.Text)) errors.AppendLine("Введите адрес!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var patient = new patients
            {
                first_name = txtFirstName.Text,
                last_name = txtLastName.Text,
                dob = dpDOB.SelectedDate.Value,
                gender = cmbGender.SelectedItem.ToString(),
                contact_info = txtContact.Text,
                address = txtAddress.Text
            };

            _context.patients.Add(patient);
            try
            {
                _context.SaveChanges();
                MessageBox.Show("Пациент добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
