using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LibraryWPFApp
{
    /// <summary>
    /// Окно управления блокировками читателей: просмотр, добавление, редактирование и удаление.
    /// Позволяет указать причину блокировки и сумму оплаты.
    /// </summary>
    public partial class BlocksWindow : Window
    {
        private List<Block> _blocks;
        private List<Reader> _readers;
        private Block _selectedBlock;

        /// <summary>
        /// Инициализирует окно и загружает данные.
        /// </summary>
        public BlocksWindow()
        {
            InitializeComponent();
            LoadData();
        }

        /// <summary>
        /// Загружает блокировки и читателей из базы данных.
        /// </summary>
        private async void LoadData()
        {
            _blocks = await LibraryDbService.GetAllBlocksAsync();
            _readers = await LibraryDbService.GetAllReadersAsync();
            BlocksGrid.ItemsSource = _blocks;
            ReaderCombo.ItemsSource = _readers;
        }

        /// <summary>
        /// Обрабатывает выбор блокировки и заполняет форму.
        /// </summary>
        private void BlocksGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedBlock = BlocksGrid.SelectedItem as Block;
            if (_selectedBlock != null)
            {
                ReaderCombo.SelectedValue = _selectedBlock.UserId;
                BlockedCheck.IsChecked = _selectedBlock.IsBlocked;
                ReasonBox.Text = _selectedBlock.BlockReason ?? "";
                PaidBox.Text = _selectedBlock.PaidAmount.ToString();
            }
        }

        /// <summary>
        /// Добавляет новую блокировку.
        /// Требует выбора читателя.
        /// </summary>
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var reader = ReaderCombo.SelectedItem as Reader;
            if (reader == null)
            {
                MessageBox.Show("Выберите читателя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int paid = 0;
            if (!string.IsNullOrWhiteSpace(PaidBox.Text) && !int.TryParse(PaidBox.Text, out paid))
            {
                MessageBox.Show("Поле 'Оплачено' должно быть числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var block = new Block
            {
                UserId = reader.UserId,
                IsBlocked = BlockedCheck.IsChecked == true,
                BlockReason = string.IsNullOrWhiteSpace(ReasonBox.Text) ? null : ReasonBox.Text.Trim(),
                PaidAmount = paid
            };

            await LibraryDbService.CreateBlockAsync(block);
            LoadData();
            ClearFields();
        }

        /// <summary>
        /// Сохраняет изменения блокировки.
        /// Проверяет корректность суммы оплаты.
        /// </summary>
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedBlock == null)
            {
                MessageBox.Show("Выберите блокировку для сохранения.", "Информация", MessageBoxButton.OK);
                return;
            }

            var reader = ReaderCombo.SelectedItem as Reader;
            if (reader == null)
            {
                MessageBox.Show("Выберите читателя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int paid = 0;
            if (!string.IsNullOrWhiteSpace(PaidBox.Text) && !int.TryParse(PaidBox.Text, out paid))
            {
                MessageBox.Show("Поле 'Оплачено' должно быть числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _selectedBlock.UserId = reader.UserId;
            _selectedBlock.IsBlocked = BlockedCheck.IsChecked == true;
            _selectedBlock.BlockReason = string.IsNullOrWhiteSpace(ReasonBox.Text) ? null : ReasonBox.Text.Trim();
            _selectedBlock.PaidAmount = paid;

            await LibraryDbService.UpdateBlockAsync(_selectedBlock);
            LoadData();
        }

        /// <summary>
        /// Удаляет блокировку после подтверждения.
        /// </summary>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedBlock == null)
            {
                MessageBox.Show("Выберите блокировку для удаления.", "Информация", MessageBoxButton.OK);
                return;
            }

            if (MessageBox.Show("Удалить эту блокировку?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await LibraryDbService.DeleteBlockAsync(_selectedBlock.BlockId);
                LoadData();
                ClearFields();
                _selectedBlock = null;
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
            BlockedCheck.IsChecked = false;
            ReasonBox.Clear();
            PaidBox.Clear();
            _selectedBlock = null;
            BlocksGrid.SelectedIndex = -1;
        }
    }
}