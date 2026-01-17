// AuthorsWindow.xaml.cs
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LibraryWPFApp
{
    /// <summary>
    /// Окно управления авторами: просмотр, добавление, редактирование и удаление записей.
    /// </summary>
    public partial class AuthorsWindow : Window
    {
        private List<Author> _authors;
        private Author _selectedAuthor;

        /// <summary>
        /// Инициализирует окно управления авторами и загружает данные из базы данных.
        /// </summary>
        public AuthorsWindow()
        {
            InitializeComponent();
            LoadData();
        }

        /// <summary>
        /// Загружает список всех авторов из базы данных и обновляет отображение в DataGrid.
        /// </summary>
        private async void LoadData()
        {
            _authors = await LibraryDbService.GetAllAuthorsAsync();
            AuthorsGrid.ItemsSource = _authors;
        }

        /// <summary>
        /// Обрабатывает событие изменения выбранного элемента в таблице авторов.
        /// Заполняет поля формы данными выбранного автора.
        /// </summary
        private void AuthorsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedAuthor = AuthorsGrid.SelectedItem as Author;
            if (_selectedAuthor != null)
            {
                LastNameBox.Text = _selectedAuthor.LastName;
                FirstNameBox.Text = _selectedAuthor.FirstName;
                MiddleNameBox.Text = _selectedAuthor.MiddleName ?? "";
            }
        }

        /// <summary>
        /// Добавляет нового автора в базу данных на основе данных из полей формы.
        /// Выполняет валидацию обязательных полей (фамилия, имя).
        /// </summary>
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var author = new Author
            {
                LastName = LastNameBox.Text.Trim(),
                FirstName = FirstNameBox.Text.Trim(),
                MiddleName = string.IsNullOrWhiteSpace(MiddleNameBox.Text) ? null : MiddleNameBox.Text.Trim()
            };

            if (string.IsNullOrEmpty(author.LastName) || string.IsNullOrEmpty(author.FirstName))
            {
                MessageBox.Show("Фамилия и имя обязательны!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            await LibraryDbService.CreateAuthorAsync(author);
            LoadData();
            ClearFields();
        }

        /// <summary>
        /// Сохраняет изменения выбранного автора в базе данных.
        /// Выполняет валидацию обязательных полей.
        /// </summary>
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAuthor == null)
            {
                MessageBox.Show("Выберите автора для сохранения.", "Информация", MessageBoxButton.OK);
                return;
            }

            _selectedAuthor.LastName = LastNameBox.Text.Trim();
            _selectedAuthor.FirstName = FirstNameBox.Text.Trim();
            _selectedAuthor.MiddleName = string.IsNullOrWhiteSpace(MiddleNameBox.Text) ? null : MiddleNameBox.Text.Trim();

            if (string.IsNullOrEmpty(_selectedAuthor.LastName) || string.IsNullOrEmpty(_selectedAuthor.FirstName))
            {
                MessageBox.Show("Фамилия и имя обязательны!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            await LibraryDbService.UpdateAuthorAsync(_selectedAuthor);
            LoadData();
        }

        /// <summary>
        /// Удаляет выбранного автора из базы данных после подтверждения пользователем.
        /// </summary>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAuthor == null)
            {
                MessageBox.Show("Выберите автора для удаления.", "Информация", MessageBoxButton.OK);
                return;
            }

            if (MessageBox.Show($"Удалить автора {_selectedAuthor.LastName} {_selectedAuthor.FirstName}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await LibraryDbService.DeleteAuthorAsync(_selectedAuthor.AuthorId);
                LoadData();
                ClearFields();
                _selectedAuthor = null;
            }
        }

        /// <summary>
        /// Обновляет список авторов, повторно загружая данные из базы данных.
        /// </summary>
        private void RefreshButton_Click(object sender, RoutedEventArgs e) => LoadData();

        /// <summary>
        /// Очищает все поля ввода и сбрасывает текущий выбор в таблице.
        /// </summary>
        private void ClearFields()
        {
            LastNameBox.Clear();
            FirstNameBox.Clear();
            MiddleNameBox.Clear();
            _selectedAuthor = null;
            AuthorsGrid.SelectedIndex = -1;
        }
    }
}