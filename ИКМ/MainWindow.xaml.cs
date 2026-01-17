// MainWindow.xaml.cs
using System.Windows;

namespace LibraryWPFApp
{
    /// <summary>
    /// Главное окно приложения "Библиотечная система IKM".
    /// Служит центральным меню для навигации по модулям управления сущностями.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Инициализирует главное окно приложения.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Авторы" и открывает окно управления авторами.
        /// </summary>
        private void AuthorsButton_Click(object sender, RoutedEventArgs e) => new AuthorsWindow().ShowDialog();

        /// <summary>
        /// Обрабатывает нажатие кнопки "Книги" и открывает окно управления книгами.
        /// </summary>
        private void BooksButton_Click(object sender, RoutedEventArgs e) => new BooksWindow().ShowDialog();

        /// <summary>
        /// Обрабатывает нажатие кнопки "Читатели" и открывает окно управления читателями.
        /// </summary>
        private void ReadersButton_Click(object sender, RoutedEventArgs e) => new ReadersWindow().ShowDialog();

        /// <summary>
        /// Обрабатывает нажатие кнопки "Экземпляры книг" и открывает окно управления экземплярами.
        /// </summary>
        private void CopiesButton_Click(object sender, RoutedEventArgs e) => new BookCopiesWindow().ShowDialog();

        /// <summary>
        /// Обрабатывает нажатие кнопки "Выдачи" и открывает окно управления выдачами книг.
        /// </summary>
        private void LoansButton_Click(object sender, RoutedEventArgs e) => new LoansWindow().ShowDialog();

        /// <summary>
        /// Обрабатывает нажатие кнопки "Блокировки" и открывает окно управления блокировками читателей.
        /// </summary>
        private void BlocksButton_Click(object sender, RoutedEventArgs e) => new BlocksWindow().ShowDialog();
    }
}