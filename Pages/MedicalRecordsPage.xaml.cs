using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;
using Clinic;
using Clinic.Windows;

namespace Clinic.Pages
{
    public partial class MedicalRecordsPage : Page
    {
        private db_clinicEntities _context;

        public MedicalRecordsPage()
        {
            InitializeComponent();
            _context = db_clinicEntities.GetContext();

            
            cmbFilterDoctor.ItemsSource = _context.doctors.ToList();
            cmbFilterPatient.ItemsSource = _context.patients.ToList();
            cmbSort.SelectedIndex = 0;  

            LoadAll();
        }

        private void LoadAll()
        {
            RecordsDataGrid.ItemsSource = _context.medical_records
                                              .Include(r => r.doctors)
                                              .Include(r => r.patients)
                                              .ToList();
        }

        private void ApplyFilters()
        {
            var q = _context.medical_records
                            .Include(r => r.doctors)
                            .Include(r => r.patients)
                            .AsQueryable();

           
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string s = txtSearch.Text.ToLower();
                q = q.Where(r => r.diagnosis.ToLower().Contains(s)
                              || r.notes.ToLower().Contains(s));
            }

            
            if (cmbFilterDoctor.SelectedValue is int docId)
                q = q.Where(r => r.doctor_id == docId);

            
            if (cmbFilterPatient.SelectedValue is int patId)
                q = q.Where(r => r.patient_id == patId);

           
            if (dpFrom.SelectedDate.HasValue)
            {
                var from = dpFrom.SelectedDate.Value.Date;
                q = q.Where(r => DbFunctions.TruncateTime(r.visit_date) >= from);
            }
            if (dpTo.SelectedDate.HasValue)
            {
                var to = dpTo.SelectedDate.Value.Date;
                q = q.Where(r => DbFunctions.TruncateTime(r.visit_date) <= to);
            }

            // Сортировка
            if (cmbSort.SelectedIndex == 0)
                q = q.OrderBy(r => r.visit_date);
            else
                q = q.OrderByDescending(r => r.visit_date);

            RecordsDataGrid.ItemsSource = q.ToList();
        }

        private void ButtonApplyFilters_OnClick(object sender, RoutedEventArgs e) => ApplyFilters();

        private void ButtonClearFilters_OnClick(object sender, RoutedEventArgs e)
        {
           
            txtSearch.Clear();
            cmbFilterDoctor.SelectedIndex = -1;
            cmbFilterPatient.SelectedIndex = -1;
            dpFrom.SelectedDate = null;
            dpTo.SelectedDate = null;
            cmbSort.SelectedIndex = 0;
            LoadAll();
        }

      

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddMedicalRecordPage());
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (RecordsDataGrid.SelectedItem is medical_records rec)
            {
                var win = new EditMedicalRecordWindow(rec);
                if (win.ShowDialog() == true)
                    LoadAll();
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (RecordsDataGrid.SelectedItem is medical_records rec &&
                MessageBox.Show("Удалить запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _context.medical_records.Remove(_context.medical_records.Find(rec.record_id));
                _context.SaveChanges();
                LoadAll();
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible) LoadAll();
        }
    }
}
