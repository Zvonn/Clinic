using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Clinic;

namespace Clinic.Windows
{
    public partial class EditUserWindow : Window
    {
        private Users _user;
        private db_clinicEntities _context = db_clinicEntities.GetContext();

        public EditUserWindow(Users user)
        {
            InitializeComponent();
            _user = user;

            txtUsername.Text = _user.username;
            cmbRole.ItemsSource = _context.Roles.ToList();
            cmbRole.SelectedValue = _user.role_id;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                cmbRole.SelectedValue == null)
            {
                MessageBox.Show("Логин и роль обязательны.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _user.username = txtUsername.Text;
            if (!string.IsNullOrWhiteSpace(pwdPassword.Password))
                _user.password_hash = ComputeSha256Hash(pwdPassword.Password);
            _user.role_id = (int)cmbRole.SelectedValue;

            _context.SaveChanges();
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
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
