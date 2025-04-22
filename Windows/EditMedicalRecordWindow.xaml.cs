using System;
using System.Linq;
using System.Windows;
using Clinic;

namespace Clinic.Windows
{
    public partial class EditMedicalRecordWindow : Window
    {
        private medical_records _record;
        private db_clinicEntities _context = db_clinicEntities.GetContext();

        public EditMedicalRecordWindow(medical_records record)
        {
            InitializeComponent();
            _record = record;

            cmbPatient.ItemsSource = _context.patients.ToList();
            cmbDoctor.ItemsSource = _context.doctors.ToList();
            cmbPatient.SelectedValue = _record.patient_id;
            cmbDoctor.SelectedValue = _record.doctor_id;
            dpVisitDate.SelectedDate = _record.visit_date;
            txtDiagnosis.Text = _record.diagnosis;
            txtTreatment.Text = _record.treatment;
            txtNotes.Text = _record.notes;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPatient.SelectedValue == null ||
                cmbDoctor.SelectedValue == null ||
                dpVisitDate.SelectedDate == null ||
                string.IsNullOrWhiteSpace(txtDiagnosis.Text) ||
                string.IsNullOrWhiteSpace(txtTreatment.Text))
            {
                MessageBox.Show("Заполните все обязательные поля.",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _record.patient_id = (int)cmbPatient.SelectedValue;
            _record.doctor_id = (int)cmbDoctor.SelectedValue;
            _record.visit_date = dpVisitDate.SelectedDate.Value;
            _record.diagnosis = txtDiagnosis.Text;
            _record.treatment = txtTreatment.Text;
            _record.notes = txtNotes.Text;

            _context.SaveChanges();
            MessageBox.Show("Запись обновлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
