using System;
using System.Windows;
using Clinic; // db_clinicEntities and models

namespace Clinic.Windows
{
    public partial class EditDrugWindow : Window
    {
        private drugs _drug;
        private db_clinicEntities _context = db_clinicEntities.GetContext();

        public EditDrugWindow(drugs selectedDrug)
        {
            InitializeComponent();
            _drug = selectedDrug;

            txtDrugName.Text = _drug.drug_name;
            txtDescription.Text = _drug.description;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDrugName.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _drug.drug_name = txtDrugName.Text;
            _drug.description = txtDescription.Text;

            _context.SaveChanges();
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
