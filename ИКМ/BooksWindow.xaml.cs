using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LibraryWPFApp
{
    /// <summary>
    /// Окно управления книгами: просмотр, добавление, редактирование и удаление записей.
    /// Включает выпадающий список для выбора автора.
    /// </summary>
    public partial class BooksWindow : Window
    {
        private List<Book> _books;
        private List<Author> _authors;
        private Book _selectedBook;

        /// <summary>
        /// Инициализирует окно управления книгами и загружает данные из базы данных.
        /// </summary>
        public BooksWindow()
        {
            InitializeComponent();
            LoadData();
        }

        /// <summary>
        /// Загружает список книг и авторов из базы данных.
        /// Обновляет DataGrid и ComboBox выбора автора.
        /// </summary>
        private async void LoadData()
        {
            _books = await LibraryDbService.GetAllBooksAsync();
            _authors = await LibraryDbService.GetAllAuthorsAsync();
            BooksGrid.ItemsSource = _books;
            AuthorCombo.ItemsSource = _authors;
        }

        /// <summary>
        /// Обрабатывает изменение выбранной книги в таблице.
        /// Заполняет поля формы данными выбранной книги.
        /// </summary>
        private void BooksGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedBook = BooksGrid.SelectedItem as Book;
            if (_selectedBook != null)
            {
                TitleBox.Text = _selectedBook.Title ?? "";
                YearBox.Text = _selectedBook.Year?.ToString() ?? "";
                LanguageBox.Text = _selectedBook.Language ?? "";
                PagesBox.Text = _selectedBook.Pages?.ToString() ?? "";
                AuthorCombo.SelectedValue = _selectedBook.AuthorId;
            }
        }

        /// <summary>
        /// Добавляет новую книгу в базу данных.
        /// Требует заполнения названия и выбора автора.
        /// </summary>
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var title = TitleBox.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Название обязательно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int? year = ParseInt(YearBox.Text);
            int? pages = ParseInt(PagesBox.Text);
            var author = AuthorCombo.SelectedItem as Author;
            if (author == null)
            {
                MessageBox.Show("Выберите автора!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var book = new Book
            {
                Title = title,
                Year = year,
                Language = LanguageBox.Text.Trim(),
                Pages = pages,
                AuthorId = author.AuthorId
            };

            await LibraryDbService.CreateBookAsync(book);
            LoadData();
            ClearFields();
        }

        /// <summary>
        /// Сохраняет изменения выбранной книги.
        /// Проверяет обязательные поля и корректность данных.
        /// </summary>
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedBook == null)
            {
                MessageBox.Show("Выберите книгу для сохранения.", "Информация", MessageBoxButton.OK);
                return;
            }

            var title = TitleBox.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Название обязательно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _selectedBook.Title = title;
            _selectedBook.Year = ParseInt(YearBox.Text);
            _selectedBook.Language = LanguageBox.Text.Trim();
            _selectedBook.Pages = ParseInt(PagesBox.Text);

            var author = AuthorCombo.SelectedItem as Author;
            if (author == null)
            {
                MessageBox.Show("Выберите автора!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _selectedBook.AuthorId = author.AuthorId;

            await LibraryDbService.UpdateBookAsync(_selectedBook);
            LoadData();
        }

        /// <summary>
        /// Удаляет выбранную книгу из базы данных после подтверждения.
        /// </summary>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedBook == null)
            {
                MessageBox.Show("Выберите книгу для удаления.", "Информация", MessageBoxButton.OK);
                return;
            }

            if (MessageBox.Show($"Удалить книгу '{_selectedBook.Title}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await LibraryDbService.DeleteBookAsync(_selectedBook.BookId);
                LoadData();
                ClearFields();
                _selectedBook = null;
            }
        }

        /// <summary>
        /// Обновляет данные, повторно загружая их из базы.
        /// </summary>
        private void RefreshButton_Click(object sender, RoutedEventArgs e) => LoadData();

        /// <summary>
        /// Очищает поля ввода и сбрасывает выбор.
        /// </summary>
        private void ClearFields()
        {
            TitleBox.Clear(); YearBox.Clear(); LanguageBox.Clear(); PagesBox.Clear();
            AuthorCombo.SelectedIndex = -1;
            _selectedBook = null;
            BooksGrid.SelectedIndex = -1;
        }

        /// <summary>
        /// Преобразует строку в целое число. Возвращает null при ошибке.
        /// </summary>
        /// <param name="s">Строка для преобразования.</param>
        /// <returns>Целое число или null.</returns>
        private static int? ParseInt(string s)
        {
            return int.TryParse(s, out int i) ? i : (int?)null;
        }
    }
}