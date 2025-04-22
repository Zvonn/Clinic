using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Clinic;

namespace Clinic.Pages
{
    public partial class AddMedicalRecordPage : Page
    {
        private medical_records _record = new medical_records();
        private db_clinicEntities _context = db_clinicEntities.GetContext();

        public AddMedicalRecordPage()
        {
            InitializeComponent();
            cmbPatient.ItemsSource = _context.patients.ToList();
            cmbDoctor.ItemsSource = _context.doctors.ToList();
            dpVisitDate.SelectedDate = DateTime.Today;
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (cmbPatient.SelectedValue == null) errors.AppendLine("Выберите пациента!");
            if (cmbDoctor.SelectedValue == null) errors.AppendLine("Выберите врача!");
            if (dpVisitDate.SelectedDate == null) errors.AppendLine("Выберите дату приёма!");
            if (string.IsNullOrWhiteSpace(txtDiagnosis.Text)) errors.AppendLine("Введите диагноз!");
            if (string.IsNullOrWhiteSpace(txtTreatment.Text)) errors.AppendLine("Введите лечение!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _record.patient_id = (int)cmbPatient.SelectedValue;
            _record.doctor_id = (int)cmbDoctor.SelectedValue;
            _record.visit_date = dpVisitDate.SelectedDate.Value;
            _record.diagnosis = txtDiagnosis.Text;
            _record.treatment = txtTreatment.Text;
            _record.notes = txtNotes.Text;

            _context.medical_records.Add(_record);
            try
            {
                _context.SaveChanges();
                MessageBox.Show("Запись добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
