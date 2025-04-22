using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data.Entity;
using Clinic; // db_clinicEntities

namespace Clinic.Pages
{
    public partial class DoctorsPage : Page
    {
        private db_clinicEntities _context;

        public DoctorsPage()
        {
            InitializeComponent();
            _context = db_clinicEntities.GetContext();
            LoadDoctors();
        }

        private void LoadDoctors()
        {
            DoctorsDataGrid.ItemsSource =
                _context.doctors
                        .Include(d => d.speciallizations)
                        .ToList();
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddDoctorPage());
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (DoctorsDataGrid.SelectedItem is doctors doc)
            {
                var win = new Windows.EditDoctorWindow(doc);
                if (win.ShowDialog() == true)
                    LoadDoctors();
            }
            else
            {
                MessageBox.Show("Выберите врача для редактирования.",
                                "Информация",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (DoctorsDataGrid.SelectedItem is doctors doc
             && MessageBox.Show("Удалить выбранного врача?",
                                "Подтверждение",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                _context.doctors.Remove(_context.doctors.Find(doc.doctor_id));
                _context.SaveChanges();
                LoadDoctors();
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible) LoadDoctors();
        }
    }
}
