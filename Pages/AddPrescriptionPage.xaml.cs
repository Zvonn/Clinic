using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Clinic;

namespace Clinic.Pages
{
    public partial class AddPrescriptionPage : Page
    {
        private db_clinicEntities _context = db_clinicEntities.GetContext();

        public AddPrescriptionPage()
        {
            InitializeComponent();
            cmbRecord.ItemsSource = _context.medical_records.ToList();
            cmbDrug.ItemsSource = _context.drugs.ToList();
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (cmbRecord.SelectedValue == null) errors.AppendLine("Выберите запись!");
            if (cmbDrug.SelectedValue == null) errors.AppendLine("Выберите лекарство!");
            if (string.IsNullOrWhiteSpace(txtDescription.Text)) errors.AppendLine("Введите описание!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var pres = new prescriptions
            {
                record_id = (int)cmbRecord.SelectedValue,
                drug_id = (int)cmbDrug.SelectedValue,
                description = txtDescription.Text
            };

            _context.prescriptions.Add(pres);
            try
            {
                _context.SaveChanges();
                MessageBox.Show("Рецепт добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
