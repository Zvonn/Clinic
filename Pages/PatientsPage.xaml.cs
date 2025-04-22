using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Clinic.Windows;

namespace Clinic.Pages
{
    public partial class PatientsPage : Page
    {
        private db_clinicEntities _context = db_clinicEntities.GetContext();

        public PatientsPage()
        {
            InitializeComponent();
            LoadPatients();
        }

        private void LoadPatients()
        {
            PatientsDataGrid.ItemsSource = _context.patients.ToList();
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPatientPage());
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (PatientsDataGrid.SelectedItem is patients selectedPatient)
            {
                var editWindow = new EditPatientWindow(selectedPatient);
                if (editWindow.ShowDialog() == true)
                {
                    LoadPatients();
                }
            }
            else
            {
                MessageBox.Show("Выберите пациента для редактирования.");
            }
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (PatientsDataGrid.SelectedItem is patients selectedPatient)
            {
                if (MessageBox.Show("Удалить пациента?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _context.patients.Remove(selectedPatient);
                    _context.SaveChanges();
                    LoadPatients();
                }
            }
            else
            {
                MessageBox.Show("Выберите пациента для удаления.");
            }
        }
    }
}
