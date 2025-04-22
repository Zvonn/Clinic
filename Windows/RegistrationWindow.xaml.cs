using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Clinic;

namespace Clinic.Windows
{
    public partial class RegistrationWindow : Window
    {
        private db_clinicEntities _context = new db_clinicEntities();

        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();
            string role = ((ComboBoxItem)cmbRole.SelectedItem)?.Content.ToString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || role == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_context.Users.Any(u => u.username == username))
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Users newUser = new Users
            {
                username = username,
                password_hash = PasswordHelper.ComputeHash(password), // Хешируем пароль
                role_id = role == "Админ" ? 1 : 2 // ID ролей в таблице
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close(); // Закрываем окно регистрации
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close(); // Возвращаемся на авторизацию
        }
    }
}
