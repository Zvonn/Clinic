using System;
using System.Linq;
using System.Windows;
using Clinic;
using Clinic.Windows;

namespace Clinic
{
    public partial class MainWindow : Window
    {
        private db_clinicEntities _context = new db_clinicEntities();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = _context.Users.FirstOrDefault(u => u.username == username);

            if (user == null || user.password_hash != PasswordHelper.ComputeHash(password))
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show($"Добро пожаловать, {user.username}!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            if (user.role_id == 1) // Админ
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
            }
            else
            {
                UserWindow userWindow = new UserWindow();
                userWindow.Show();
            }

            this.Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow regWindow = new RegistrationWindow();
            regWindow.Show();
            this.Close(); // Закрываем текущее окно
        }
    }
}
