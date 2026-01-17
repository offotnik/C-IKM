using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LibraryWPFApp
{
    /// <summary>
    /// Окно управления читателями: просмотр, добавление, редактирование и удаление записей.
    /// Поддерживает ввод дат, телефонов, email и адреса.
    /// </summary>
    public partial class ReadersWindow : Window
    {
        private List<Reader> _readers;
        private Reader _selectedReader;

        /// <summary>
        /// Инициализирует окно управления читателями и загружает данные.
        /// </summary>
        public ReadersWindow()
        {
            InitializeComponent();
            LoadData();
        }

        /// <summary>
        /// Загружает список читателей из базы данных.
        /// </summary>
        private async void LoadData()
        {
            _readers = await LibraryDbService.GetAllReadersAsync();
            ReadersGrid.ItemsSource = _readers;
        }

        /// <summary>
        /// Обрабатывает выбор читателя в таблице и заполняет форму.
        /// </summary>
        private void ReadersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedReader = ReadersGrid.SelectedItem as Reader;
            if (_selectedReader != null)
            {
                LastNameBox.Text = _selectedReader.LastName ?? "";
                FirstNameBox.Text = _selectedReader.FirstName ?? "";
                MiddleNameBox.Text = _selectedReader.MiddleName ?? "";
                BirthDatePicker.SelectedDate = _selectedReader.BirthDate;
                PhoneBox.Text = _selectedReader.Phone ?? "";
                EmailBox.Text = _selectedReader.Email ?? "";
                AddressBox.Text = _selectedReader.Address ?? "";
                GetRentCheck.IsChecked = _selectedReader.GetRent;
                MustReturnPicker.SelectedDate = _selectedReader.MustReturn;
            }
        }

        /// <summary>
        /// Добавляет нового читателя в базу данных.
        /// Требует заполнения фамилии и имени.
        /// </summary>
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var lastName = LastNameBox.Text.Trim();
            var firstName = FirstNameBox.Text.Trim();
            if (string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(firstName))
            {
                MessageBox.Show("Фамилия и имя обязательны!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var reader = new Reader
            {
                LastName = lastName,
                FirstName = firstName,
                MiddleName = string.IsNullOrWhiteSpace(MiddleNameBox.Text) ? null : MiddleNameBox.Text.Trim(),
                BirthDate = BirthDatePicker.SelectedDate,
                Phone = PhoneBox.Text.Trim(),
                Email = EmailBox.Text.Trim(),
                Address = AddressBox.Text.Trim(),
                GetRent = GetRentCheck.IsChecked == true,
                MustReturn = MustReturnPicker.SelectedDate
            };

            await LibraryDbService.CreateReaderAsync(reader);
            LoadData();
            ClearFields();
        }

        /// <summary>
        /// Сохраняет изменения выбранного читателя.
        /// </summary>
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedReader == null)
            {
                MessageBox.Show("Выберите читателя для сохранения.", "Информация", MessageBoxButton.OK);
                return;
            }

            _selectedReader.LastName = LastNameBox.Text.Trim();
            _selectedReader.FirstName = FirstNameBox.Text.Trim();
            _selectedReader.MiddleName = string.IsNullOrWhiteSpace(MiddleNameBox.Text) ? null : MiddleNameBox.Text.Trim();
            _selectedReader.BirthDate = BirthDatePicker.SelectedDate;
            _selectedReader.Phone = PhoneBox.Text.Trim();
            _selectedReader.Email = EmailBox.Text.Trim();
            _selectedReader.Address = AddressBox.Text.Trim();
            _selectedReader.GetRent = GetRentCheck.IsChecked == true;
            _selectedReader.MustReturn = MustReturnPicker.SelectedDate;

            await LibraryDbService.UpdateReaderAsync(_selectedReader);
            LoadData();
        }

        /// <summary>
        /// Удаляет выбранного читателя после подтверждения.
        /// </summary>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedReader == null)
            {
                MessageBox.Show("Выберите читателя для удаления.", "Информация", MessageBoxButton.OK);
                return;
            }

            if (MessageBox.Show($"Удалить читателя {_selectedReader.LastName} {_selectedReader.FirstName}?",
                "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await LibraryDbService.DeleteReaderAsync(_selectedReader.UserId);
                LoadData();
                ClearFields();
                _selectedReader = null;
            }
        }

        /// <summary>
        /// Обновляет список читателей.
        /// </summary>
        private void RefreshButton_Click(object sender, RoutedEventArgs e) => LoadData();

        /// <summary>
        /// Очищает все поля формы.
        /// </summary>
        private void ClearFields()
        {
            LastNameBox.Clear(); FirstNameBox.Clear(); MiddleNameBox.Clear();
            BirthDatePicker.SelectedDate = null;
            PhoneBox.Clear(); EmailBox.Clear(); AddressBox.Clear();
            GetRentCheck.IsChecked = false;
            MustReturnPicker.SelectedDate = null;
            _selectedReader = null;
            ReadersGrid.SelectedIndex = -1;
        }
    }
}