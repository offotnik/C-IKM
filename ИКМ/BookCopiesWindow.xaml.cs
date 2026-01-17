using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LibraryWPFApp
{
    /// <summary>
    /// Окно управления экземплярами книг: просмотр, добавление, редактирование и удаление.
    /// Позволяет указать доступность экземпляра и выбрать книгу из списка.
    /// </summary>
    public partial class BookCopiesWindow : Window
    {
        private List<BookCopy> _copies;
        private List<Book> _books;
        private BookCopy _selectedCopy;

        /// <summary>
        /// Инициализирует окно и загружает данные.
        /// </summary>
        public BookCopiesWindow()
        {
            InitializeComponent();
            LoadData();
        }

        /// <summary>
        /// Загружает экземпляры и книги из базы данных.
        /// </summary>
        private async void LoadData()
        {
            _copies = await LibraryDbService.GetAllCopiesAsync();
            _books = await LibraryDbService.GetAllBooksAsync();
            CopiesGrid.ItemsSource = _copies;
            BookCombo.ItemsSource = _books;
        }

        /// <summary>
        /// Обрабатывает выбор экземпляра и заполняет форму.
        /// </summary>
        private void CopiesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedCopy = CopiesGrid.SelectedItem as BookCopy;
            if (_selectedCopy != null)
            {
                BookCombo.SelectedValue = _selectedCopy.BookId;
                AvailableCheck.IsChecked = _selectedCopy.IsAvailable;
            }
        }

        /// <summary>
        /// Добавляет новый экземпляр книги.
        /// Требует выбора книги.
        /// </summary>
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var book = BookCombo.SelectedItem as Book;
            if (book == null)
            {
                MessageBox.Show("Выберите книгу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var copy = new BookCopy
            {
                BookId = book.BookId,
                IsAvailable = AvailableCheck.IsChecked == true
            };

            await LibraryDbService.CreateCopyAsync(copy);
            LoadData();
            ClearFields();
        }

        /// <summary>
        /// Сохраняет изменения выбранного экземпляра.
        /// </summary>
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCopy == null)
            {
                MessageBox.Show("Выберите экземпляр для сохранения.", "Информация", MessageBoxButton.OK);
                return;
            }

            var book = BookCombo.SelectedItem as Book;
            if (book == null)
            {
                MessageBox.Show("Выберите книгу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _selectedCopy.BookId = book.BookId;
            _selectedCopy.IsAvailable = AvailableCheck.IsChecked == true;

            await LibraryDbService.UpdateCopyAsync(_selectedCopy);
            LoadData();
        }

        /// <summary>
        /// Удаляет выбранный экземпляр после подтверждения.
        /// </summary>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCopy == null)
            {
                MessageBox.Show("Выберите экземпляр для удаления.", "Информация", MessageBoxButton.OK);
                return;
            }

            if (MessageBox.Show("Удалить этот экземпляр?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await LibraryDbService.DeleteCopyAsync(_selectedCopy.CopyId);
                LoadData();
                ClearFields();
                _selectedCopy = null;
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
            BookCombo.SelectedIndex = -1;
            AvailableCheck.IsChecked = false;
            _selectedCopy = null;
            CopiesGrid.SelectedIndex = -1;
        }
    }
}