using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LibraryWPFApp
{
    /// <summary>
    /// Окно управления выдачами книг: просмотр, добавление, редактирование и удаление записей.
    /// Поддерживает выбор читателя, экземпляра, дат выдачи и возврата.
    /// </summary>
    public partial class LoansWindow : Window
    {
        private List<Loan> _loans;
        private List<Reader> _readers;
        private List<BookCopy> _copies;
        private Loan _selectedLoan;

        /// <summary>
        /// Инициализирует окно и загружает данные.
        /// </summary>
        public LoansWindow()
        {
            InitializeComponent();
            LoadData();
        }

        /// <summary>
        /// Загружает выдачи, читателей и экземпляры из базы данных.
        /// </summary>
        private async void LoadData()
        {
            _loans = await LibraryDbService.GetAllLoansAsync();
            _readers = await LibraryDbService.GetAllReadersAsync();
            _copies = await LibraryDbService.GetAllCopiesAsync();

            LoansGrid.ItemsSource = _loans;
            ReaderCombo.ItemsSource = _readers;
            CopyCombo.ItemsSource = _copies;
        }

        /// <summary>
        /// Обрабатывает выбор выдачи и заполняет форму.
        /// </summary>
        private void LoansGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedLoan = LoansGrid.SelectedItem as Loan;
            if (_selectedLoan != null)
            {
                ReaderCombo.SelectedValue = _selectedLoan.UserId;
                CopyCombo.SelectedValue = _selectedLoan.CopyId;
                BearDatePicker.SelectedDate = _selectedLoan.BearDate;
                DueDatePicker.SelectedDate = _selectedLoan.DueDate;
                ReturnDatePicker.SelectedDate = _selectedLoan.ReturnDate;
                ReturnedCheck.IsChecked = _selectedLoan.IsReturned;
            }
        }

        /// <summary>
        /// Добавляет новую запись о выдаче.
        /// Требует выбора читателя и экземпляра.
        /// </summary>
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var reader = ReaderCombo.SelectedItem as Reader;
            var copy = CopyCombo.SelectedItem as BookCopy;
            if (reader == null || copy == null)
            {
                MessageBox.Show("Выберите читателя и экземпляр!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var loan = new Loan
            {
                UserId = reader.UserId,
                CopyId = copy.CopyId,
                BearDate = BearDatePicker.SelectedDate ?? DateTime.Today,
                DueDate = DueDatePicker.SelectedDate,
                ReturnDate = ReturnDatePicker.SelectedDate,
                IsReturned = ReturnedCheck.IsChecked == true
            };

            await LibraryDbService.CreateLoanAsync(loan);
            LoadData();
            ClearFields();
        }

        /// <summary>
        /// Сохраняет изменения выдачи.
        /// </summary>
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedLoan == null)
            {
                MessageBox.Show("Выберите запись для сохранения.", "Информация", MessageBoxButton.OK);
                return;
            }

            var reader = ReaderCombo.SelectedItem as Reader;
            var copy = CopyCombo.SelectedItem as BookCopy;
            if (reader == null || copy == null)
            {
                MessageBox.Show("Выберите читателя и экземпляр!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _selectedLoan.UserId = reader.UserId;
            _selectedLoan.CopyId = copy.CopyId;
            _selectedLoan.BearDate = BearDatePicker.SelectedDate ?? DateTime.Today;
            _selectedLoan.DueDate = DueDatePicker.SelectedDate;
            _selectedLoan.ReturnDate = ReturnDatePicker.SelectedDate;
            _selectedLoan.IsReturned = ReturnedCheck.IsChecked == true;

            await LibraryDbService.UpdateLoanAsync(_selectedLoan);
            LoadData();
        }

        /// <summary>
        /// Удаляет запись о выдаче после подтверждения.
        /// </summary>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedLoan == null)
            {
                MessageBox.Show("Выберите запись для удаления.", "Информация", MessageBoxButton.OK);
                return;
            }

            if (MessageBox.Show("Удалить эту выдачу?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await LibraryDbService.DeleteLoanAsync(_selectedLoan.LoanId);
                LoadData();
                ClearFields();
                _selectedLoan = null;
            }
        }

        /// <summary>
        /// Обновляет данные.
        /// </summary>
        private void RefreshButton_Click(object sender, RoutedEventArgs e) => LoadData();

        /// <summary>
        /// Очищает поля формы.
        /// </summary>
        private void ClearFields()
        {
            ReaderCombo.SelectedIndex = -1;
            CopyCombo.SelectedIndex = -1;
            BearDatePicker.SelectedDate = null;
            DueDatePicker.SelectedDate = null;
            ReturnDatePicker.SelectedDate = null;
            ReturnedCheck.IsChecked = false;
            _selectedLoan = null;
            LoansGrid.SelectedIndex = -1;
        }
    }
}