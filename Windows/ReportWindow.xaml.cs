// Windows/ReportWindow.xaml.cs
using System;
using System.Windows;
using Microsoft.Win32; // For WPF SaveFileDialog
using System.IO;

namespace Clinic.Windows
{
    public partial class ReportWindow : Window
    {
        public ReportWindow()
        {
            InitializeComponent();
        }

        private void BtnDoctorReport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel файлы (*.xlsx)|*.xlsx",
                FileName = $"Отчет_по_посещаемости_врачей_{DateTime.Now:yyyyMMdd}.xlsx",
                Title = "Сохранить отчет по посещаемости врачей"
            };

            if (saveFileDialog.ShowDialog() == true) // In WPF, ShowDialog returns bool? (nullable bool)
            {
                Reports.ReportGenerator.GenerateDoctorAppointmentsReport(saveFileDialog.FileName);
            }
        }

        private void BtnDiagnosisReport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word файлы (*.docx)|*.docx",
                FileName = $"Отчет_по_диагнозам_{DateTime.Now:yyyyMMdd}.docx",
                Title = "Сохранить отчет по диагнозам пациентов"
            };

            if (saveFileDialog.ShowDialog() == true) // In WPF, ShowDialog returns bool? (nullable bool)
            {
                Reports.ReportGenerator.GeneratePatientDiagnosisReport(saveFileDialog.FileName);
            }
        }
        private void BtnDiagnosisStatsReport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel файлы (*.xlsx)|*.xlsx",
                FileName = $"Статистика_по_диагнозам_{DateTime.Now:yyyyMMdd}.xlsx",
                Title = "Сохранить отчет по статистике диагнозов"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                Reports.ReportGenerator.GenerateDiagnosisStatisticsReport(saveFileDialog.FileName);
            }
        }
    }
}
