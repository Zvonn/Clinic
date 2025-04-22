using System;
using System.Linq;
using System.Windows;
using Clinic;

namespace Clinic.Windows
{
    public partial class EditAppointmentWindow : Window
    {
        private db_clinicEntities _context = db_clinicEntities.GetContext();
        private appointments _appointment;

        public EditAppointmentWindow(appointments appointment)
        {
            InitializeComponent();
            _appointment = appointment;
            
            txtPatientId.Text = _appointment.patient_id.ToString();
            txtDoctorId.Text = _appointment.doctor_id.ToString();
            dpAppointmentDate.SelectedDate = _appointment.appointment_date;
            txtReason.Text = _appointment.reason;
            cmbStatus.SelectedValue = _appointment.status_id;
            cmbStatus.ItemsSource = _context.statuses.ToList();
            cmbStatus.DisplayMemberPath = "status_name";
            cmbStatus.SelectedValuePath = "status_id";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtPatientId.Text, out int patientId) ||
                !int.TryParse(txtDoctorId.Text, out int doctorId) ||
                dpAppointmentDate.SelectedDate == null)
            {
                MessageBox.Show("Проверьте ввод данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            _appointment.patient_id = patientId;
            _appointment.doctor_id = doctorId;
            _appointment.appointment_date = dpAppointmentDate.SelectedDate.Value;
            _appointment.reason = txtReason.Text;
            _appointment.status_id = (int)cmbStatus.SelectedValue;
            _context.SaveChanges();

            MessageBox.Show("Запись обновлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
