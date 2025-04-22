using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Clinic;

namespace Clinic.Pages
{
    public partial class AddUserPage : Page
    {
        private db_clinicEntities _context = db_clinicEntities.GetContext();

        public AddUserPage()
        {
            InitializeComponent();
            cmbRole.ItemsSource = _context.Roles.ToList();
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(pwdPassword.Password) ||
                cmbRole.SelectedValue == null)
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = new Users
            {
                username = txtUsername.Text,
                password_hash = ComputeSha256Hash(pwdPassword.Password),
                role_id = (int)cmbRole.SelectedValue
            };

            _context.Users.Add(user);
            try
            {
                _context.SaveChanges();
                MessageBox.Show("Пользователь добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private static string ComputeSha256Hash(string raw)
        {
            var sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
            var sb = new StringBuilder();
            foreach (var b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
