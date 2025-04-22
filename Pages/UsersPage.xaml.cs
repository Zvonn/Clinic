using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data.Entity;
using Clinic; // db_clinicEntities

namespace Clinic.Pages
{
    public partial class UsersPage : Page
    {
        private db_clinicEntities _context;

        public UsersPage()
        {
            InitializeComponent();
            _context = db_clinicEntities.GetContext();
            LoadUsers();
        }

        private void LoadUsers()
        {
            UsersDataGrid.ItemsSource =
                _context.Users
                        .Include(u => u.Roles)
                        .ToList();
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddUserPage());
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is Users u)
            {
                var win = new Windows.EditUserWindow(u);
                if (win.ShowDialog() == true)
                    LoadUsers();
            }
            else
            {
                MessageBox.Show("Выберите пользователя для редактирования.",
                                "Информация",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is Users u
             && MessageBox.Show("Удалить пользователя?",
                                "Подтверждение",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                _context.Users.Remove(_context.Users.Find(u.user_id));
                _context.SaveChanges();
                LoadUsers();
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
                LoadUsers();
        }
    }
}
