using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data.Entity;
using Clinic; // db_clinicEntities

namespace Clinic.Pages
{
    public partial class PrescriptionsPage : Page
    {
        private db_clinicEntities _context;

        public PrescriptionsPage()
        {
            InitializeComponent();
            _context = db_clinicEntities.GetContext();
            LoadPrescriptions();
        }

        private void LoadPrescriptions()
        {
            PrescriptionsDataGrid.ItemsSource =
                _context.prescriptions
                        .Include(p => p.drugs)
                        .Include(p => p.medical_records)
                        .ToList();
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPrescriptionPage());
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (PrescriptionsDataGrid.SelectedItem is prescriptions pres)
            {
                var win = new Windows.EditPrescriptionWindow(pres);
                if (win.ShowDialog() == true)
                    LoadPrescriptions();
            }
            else
            {
                MessageBox.Show("Выберите рецепт для редактирования.",
                                "Информация",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (PrescriptionsDataGrid.SelectedItem is prescriptions pres
                && MessageBox.Show("Удалить выбранный рецепт?",
                                   "Подтверждение",
                                   MessageBoxButton.YesNo,
                                   MessageBoxImage.Question)
                   == MessageBoxResult.Yes)
            {
                _context.prescriptions.Remove(_context.prescriptions.Find(pres.prescription_id));
                _context.SaveChanges();
                LoadPrescriptions();
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
                LoadPrescriptions();
        }
    }
}
