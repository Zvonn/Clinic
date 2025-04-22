using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Clinic; // Пространство имен с db_clinicEntities и методом GetContext

namespace Clinic.Pages
{
    public partial class AppointmentsPage : Page
    {
        private db_clinicEntities _context;

        public AppointmentsPage()
        {
            InitializeComponent();
            // Получаем контекст через метод GetContext
            _context = Clinic.db_clinicEntities.GetContext();
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            AppointmentsDataGrid.ItemsSource = _context.appointments.ToList();
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {


            if (AppointmentsDataGrid.SelectedItem is appointments selectedAppointment)
            {
                // Предполагаем, что EditAppointmentWindow принимает объект appointments для редактирования
                Windows.EditAppointmentWindow editWindow = new Windows.EditAppointmentWindow(selectedAppointment);
                if (editWindow.ShowDialog() == true)
                {
                    LoadAppointments();
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddAppointmentPage(null));
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (AppointmentsDataGrid.SelectedItem is appointments selectedAppointment)
            {
                if (MessageBox.Show("Удалить выбранную запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var appointmentToDelete = _context.appointments.Find(selectedAppointment.appointment_id);
                    if (appointmentToDelete != null)
                    {
                        _context.appointments.Remove(appointmentToDelete);
                        _context.SaveChanges();
                        LoadAppointments();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void AppointIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Visibility == Visibility.Visible)
            {
                LoadAppointments();
            }
        }
    }
}
