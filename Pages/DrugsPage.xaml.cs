using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Clinic; // db_clinicEntities

namespace Clinic.Pages
{
    public partial class DrugsPage : Page
    {
        private db_clinicEntities _context;

        public DrugsPage()
        {
            InitializeComponent();
            _context = db_clinicEntities.GetContext();
            LoadDrugs();
        }

        private void LoadDrugs()
        {
            DrugsDataGrid.ItemsSource = _context.drugs.ToList();
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddDrugPage());
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (DrugsDataGrid.SelectedItem is drugs drug)
            {
                var win = new Windows.EditDrugWindow(drug);
                if (win.ShowDialog() == true)
                    LoadDrugs();
            }
            else
            {
                MessageBox.Show("Выберите лекарство для редактирования.",
                                "Информация",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (DrugsDataGrid.SelectedItem is drugs drug
             && MessageBox.Show("Удалить выбранное лекарство?",
                                "Подтверждение",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                _context.drugs.Remove(_context.drugs.Find(drug.drug_id));
                _context.SaveChanges();
                LoadDrugs();
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
                LoadDrugs();
        }
    }
}
