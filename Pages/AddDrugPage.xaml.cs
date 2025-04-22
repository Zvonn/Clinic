using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Clinic; // db_clinicEntities

namespace Clinic.Pages
{
    public partial class AddDrugPage : Page
    {
        private db_clinicEntities _context = db_clinicEntities.GetContext();

        public AddDrugPage()
        {
            InitializeComponent();
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(txtDrugName.Text))
                errors.AppendLine("Введите название лекарства!");
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
                errors.AppendLine("Введите описание!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var drug = new drugs
            {
                drug_name = txtDrugName.Text,
                description = txtDescription.Text
            };

            _context.drugs.Add(drug);
            try
            {
                _context.SaveChanges();
                MessageBox.Show("Лекарство добавлено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
