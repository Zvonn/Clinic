using System.Linq;
using System.Windows;
using Clinic;

namespace Clinic.Windows
{
    public partial class EditPrescriptionWindow : Window
    {
        private prescriptions _prescription;
        private db_clinicEntities _context = db_clinicEntities.GetContext();

        public EditPrescriptionWindow(prescriptions pres)
        {
            InitializeComponent();
            _prescription = pres;

            cmbRecord.ItemsSource = _context.medical_records.ToList();
            cmbDrug.ItemsSource = _context.drugs.ToList();

            cmbRecord.SelectedValue = _prescription.record_id;
            cmbDrug.SelectedValue = _prescription.drug_id;
            txtDescription.Text = _prescription.description;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRecord.SelectedValue == null ||
                cmbDrug.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _prescription.record_id = (int)cmbRecord.SelectedValue;
            _prescription.drug_id = (int)cmbDrug.SelectedValue;
            _prescription.description = txtDescription.Text;

            _context.SaveChanges();
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
