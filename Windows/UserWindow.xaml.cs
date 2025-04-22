using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Clinic.Windows
{
    public partial class UserWindow : NavigationWindow
    {
        public UserWindow()
        {
            InitializeComponent();
            if (cbTables.SelectedItem is ComboBoxItem selected && selected.Tag != null)
            {
                MainFrame.Navigate(new Uri(selected.Tag.ToString(), UriKind.Relative));
            }
        }

        private void BtnNavigate_Click(object sender, RoutedEventArgs e)
        {
            if (cbTables.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null)
            {
                string pageUri = selectedItem.Tag.ToString();
                MainFrame.Navigate(new Uri(pageUri, UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Выберите таблицу для просмотра", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
