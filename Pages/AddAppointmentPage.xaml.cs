using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Clinic;  // пространство имён с db_clinicEntities и моделями

namespace Clinic.Pages
{
    public partial class AddAppointmentPage : Page
    {
        // Если null, значит режим "добавления", иначе - редактирование существующей записи
        private appointments _appointment;
        private db_clinicEntities _context = db_clinicEntities.GetContext();

        // Конструктор с параметром: если передан объект, то редактируем его
        public AddAppointmentPage(appointments selectedAppointment)
        {
            InitializeComponent();

            if (selectedAppointment != null)
                _appointment = selectedAppointment;
            else
                _appointment = new appointments();

            
            DataContext = _appointment;

            
            cmbStatus.ItemsSource = _context.statuses.ToList();
            cmbStatus.DisplayMemberPath = "status_name";
            cmbStatus.SelectedValuePath = "status_id";

            
            if (_appointment.status_id != 0)
            {
                cmbStatus.SelectedValue = _appointment.status_id;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            
            if (!int.TryParse(txtPatientId.Text, out int patientId))
                errors.AppendLine("Введите корректный номер пациента!");

            if (!int.TryParse(txtDoctorId.Text, out int doctorId))
                errors.AppendLine("Введите корректный номер врача!");

            if (dpAppointmentDate.SelectedDate == null)
                errors.AppendLine("Выберите дату приёма!");

            if (string.IsNullOrWhiteSpace(txtReason.Text))
                errors.AppendLine("Введите причину приёма!");

            if (cmbStatus.SelectedValue == null)
                errors.AppendLine("Выберите статус!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

           
            _appointment.patient_id = patientId;
            _appointment.doctor_id = doctorId;
            _appointment.appointment_date = dpAppointmentDate.SelectedDate.Value;
            _appointment.reason = txtReason.Text;
            _appointment.status_id = (int)cmbStatus.SelectedValue;

            
            if (_appointment.appointment_id == 0)
            {
                _context.appointments.Add(_appointment);
            }

            try
            {
                _context.SaveChanges();
                MessageBox.Show("Данные успешно сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
