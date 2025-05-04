using System;
using System.Linq;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace Clinic.Reports
{
    public class ReportGenerator
    {
        // Отчет по посещаемости врачей (Excel)
        public static void GenerateDoctorAppointmentsReport(string filePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                // Создаем экземпляр Excel
                excelApp = new Excel.Application();
                workbook = excelApp.Workbooks.Add();
                worksheet = workbook.Sheets[1];

                // Настраиваем заголовки
                worksheet.Cells[1, 1] = "ID врача";
                worksheet.Cells[1, 2] = "ФИО врача";
                worksheet.Cells[1, 3] = "Специализация";
                worksheet.Cells[1, 4] = "Количество приемов";
                worksheet.Cells[1, 5] = "Период";

                // Форматирование заголовков
                Excel.Range headerRange = worksheet.Range["A1:E1"];
                headerRange.Font.Bold = true;
                headerRange.Interior.Color = Excel.XlRgbColor.rgbLightGray;

                // Получаем данные
                var context = db_clinicEntities.GetContext();
                var now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1); // Начало текущего месяца
                var endDate = startDate.AddMonths(1).AddDays(-1); // Конец текущего месяца

                var doctorStats = context.doctors
                    .Select(d => new
                    {
                        d.doctor_id,
                        FullName = d.last_name + " " + d.first_name,
                        Specialization = d.speciallizations.specialization_name,
                        AppointmentCount = d.appointments.Count(a =>
                            a.appointment_date >= startDate &&
                            a.appointment_date <= endDate)
                    })
                    .OrderByDescending(d => d.AppointmentCount)
                    .ToList();

                // Заполняем данными
                int row = 2;
                foreach (var stat in doctorStats)
                {
                    worksheet.Cells[row, 1] = stat.doctor_id;
                    worksheet.Cells[row, 2] = stat.FullName;
                    worksheet.Cells[row, 3] = stat.Specialization;
                    worksheet.Cells[row, 4] = stat.AppointmentCount;
                    worksheet.Cells[row, 5] = $"{startDate.ToString("dd.MM.yyyy")} - {endDate.ToString("dd.MM.yyyy")}";
                    row++;
                }

                // Автоподбор ширины столбцов
                worksheet.Columns.AutoFit();

                // Сохраняем файл
                workbook.SaveAs(filePath);
                workbook.Close();

                MessageBox.Show($"Отчет успешно сохранен по пути: {filePath}",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчета: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Освобождаем ресурсы
                if (worksheet != null)
                    Marshal.ReleaseComObject(worksheet);

                if (workbook != null)
                {
                    Marshal.ReleaseComObject(workbook);
                }

                if (excelApp != null)
                {
                    excelApp.Quit();
                    Marshal.ReleaseComObject(excelApp);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        // Отчет по диагнозам пациентов (Word)
        public static void GeneratePatientDiagnosisReport(string filePath)
        {
            Word.Application wordApp = null;
            Word.Document document = null;

            try
            {
                // Создаем экземпляр Word
                wordApp = new Word.Application();
                document = wordApp.Documents.Add();

                // Добавляем заголовок
                Word.Paragraph titlePara = document.Paragraphs.Add();
                titlePara.Range.Text = "Отчет по диагнозам пациентов";
                titlePara.Range.Font.Bold = 1;
                titlePara.Range.Font.Size = 16;
                titlePara.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                titlePara.Range.InsertParagraphAfter();

                // Добавляем дату отчета
                Word.Paragraph datePara = document.Paragraphs.Add();
                datePara.Range.Text = $"Дата создания: {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}";
                datePara.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                datePara.Range.InsertParagraphAfter();

                // Получаем данные
                var context = db_clinicEntities.GetContext();
                var records = context.medical_records
                    .OrderByDescending(r => r.visit_date)
                    .Take(100) // Берем последние 100 записей для отчета
                    .Select(r => new
                    {
                        PatientName = r.patients.last_name + " " + r.patients.first_name,
                        r.patients.gender,
                        Age = DateTime.Now.Year - r.patients.dob.Year,
                        r.diagnosis,
                        r.treatment,
                        DoctorName = r.doctors.last_name + " " + r.doctors.first_name,
                        r.visit_date
                    })
                    .ToList();

                // Добавляем информацию о каждой записи
                foreach (var record in records)
                {
                    // Имя пациента и базовая информация
                    Word.Paragraph patientPara = document.Paragraphs.Add();
                    patientPara.Range.Text = $"Пациент: {record.PatientName}";
                    patientPara.Range.Font.Bold = 1;
                    patientPara.Range.InsertParagraphAfter();

                    // Информация о пациенте
                    Word.Paragraph infoPara = document.Paragraphs.Add();
                    infoPara.Range.Text = $"Пол: {record.gender}, Возраст: {record.Age} лет";
                    infoPara.Range.InsertParagraphAfter();

                    Word.Paragraph visitPara = document.Paragraphs.Add();
                    visitPara.Range.Text = $"Дата посещения: {record.visit_date.ToString("dd.MM.yyyy")}";
                    visitPara.Range.InsertParagraphAfter();

                    Word.Paragraph doctorPara = document.Paragraphs.Add();
                    doctorPara.Range.Text = $"Врач: {record.DoctorName}";
                    doctorPara.Range.InsertParagraphAfter();

                    // Диагноз и лечение
                    Word.Paragraph diagnosisPara = document.Paragraphs.Add();
                    diagnosisPara.Range.Text = "Диагноз: " + record.diagnosis;
                    diagnosisPara.Range.InsertParagraphAfter();

                    Word.Paragraph treatmentPara = document.Paragraphs.Add();
                    treatmentPara.Range.Text = "Лечение: " + record.treatment;
                    treatmentPara.Range.InsertParagraphAfter();

                    // Добавляем разделительную линию
                    Word.Paragraph separatorPara = document.Paragraphs.Add();
                    separatorPara.Range.InsertParagraphAfter();

                    // Добавляем горизонтальную линию
                    object beginRange = separatorPara.Range.Start;
                    object endRange = separatorPara.Range.End;
                    Word.Range lineRange = document.Range(ref beginRange, ref endRange);

                    lineRange.Borders[Word.WdBorderType.wdBorderBottom].LineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    lineRange.Borders[Word.WdBorderType.wdBorderBottom].LineWidth = Word.WdLineWidth.wdLineWidth050pt;

                    document.Paragraphs.Add().Range.InsertParagraphAfter(); // Пустая строка после разделителя
                }

                // Сохраняем документ
                document.SaveAs2(filePath);
                document.Close();

                MessageBox.Show($"Отчет успешно сохранен по пути: {filePath}",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчета: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Освобождаем ресурсы
                if (document != null)
                {
                    Marshal.ReleaseComObject(document);
                }

                if (wordApp != null)
                {
                    wordApp.Quit();
                    Marshal.ReleaseComObject(wordApp);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        public static void GenerateDiagnosisStatisticsReport(string filePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            Excel.Chart chart = null;

            try
            {
                // Создаем экземпляр Excel
                excelApp = new Excel.Application();
                workbook = excelApp.Workbooks.Add();
                worksheet = workbook.Sheets[1];
                worksheet.Name = "Статистика диагнозов";

                // Настраиваем заголовки
                worksheet.Cells[1, 1] = "Диагноз";
                worksheet.Cells[1, 2] = "Количество случаев";
                worksheet.Cells[1, 3] = "Процент";

                // Форматирование заголовков
                Excel.Range headerRange = worksheet.Range["A1:C1"];
                headerRange.Font.Bold = true;
                headerRange.Interior.Color = Excel.XlRgbColor.rgbLightGray;

                // Получаем данные
                var context = db_clinicEntities.GetContext();

                var diagnosisStats = context.medical_records
                    .GroupBy(r => r.diagnosis)
                    .Select(g => new
                    {
                        Diagnosis = g.Key,
                        Count = g.Count()
                    })
                    .OrderByDescending(d => d.Count)
                    .Take(10) // Топ-10 диагнозов
                    .ToList();

                // Рассчитываем общее количество для процентов
                int totalCases = diagnosisStats.Sum(d => d.Count);

                // Заполняем данными
                int row = 2;
                foreach (var stat in diagnosisStats)
                {
                    double percentage = (double)stat.Count / totalCases * 100;

                    worksheet.Cells[row, 1] = stat.Diagnosis;
                    worksheet.Cells[row, 2] = stat.Count;
                    worksheet.Cells[row, 3] = $"{percentage:F2}%";
                    row++;
                }

                // Автоподбор ширины столбцов
                worksheet.Columns.AutoFit();

                // Создаем диаграмму
                Excel.ChartObjects chartObjects = (Excel.ChartObjects)worksheet.ChartObjects();
                Excel.ChartObject chartObject = chartObjects.Add(300, 10, 400, 300);
                chart = chartObject.Chart;

                // Настраиваем диаграмму
                chart.SetSourceData(worksheet.Range[$"A1:B{row - 1}"]);
                chart.ChartType = Excel.XlChartType.xlPie;
                chart.HasTitle = true;
                chart.ChartTitle.Text = "Распределение диагнозов";
                chart.HasLegend = true;
                chart.Legend.Position = Excel.XlLegendPosition.xlLegendPositionRight;

                // Сохраняем файл
                workbook.SaveAs(filePath);
                workbook.Close();

                MessageBox.Show($"Отчет по статистике диагнозов успешно сохранен по пути: {filePath}",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчета: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Освобождаем ресурсы
                if (chart != null)
                    Marshal.ReleaseComObject(chart);

                if (worksheet != null)
                    Marshal.ReleaseComObject(worksheet);

                if (workbook != null)
                    Marshal.ReleaseComObject(workbook);

                if (excelApp != null)
                {
                    excelApp.Quit();
                    Marshal.ReleaseComObject(excelApp);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}