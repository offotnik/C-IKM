using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Представляет книгу в библиотечной системе.
/// </summary>
[Table("books")]
public class Book
{
    /// <summary>
    /// Уникальный идентификатор книги.
    /// </summary>
    [Column("book_id")]
    public int BookId { get; set; }

    /// <summary>
    /// Название книги.
    /// </summary>
    [Column("title")]
    public string Title { get; set; }

    /// <summary>
    /// Год публикации книги.
    /// </summary>
    [Column("year")]
    public int? Year { get; set; }

    /// <summary>
    /// Язык издания книги.
    /// </summary>
    [Column("language")]
    public string Language { get; set; }

    /// <summary>
    /// Количество страниц в книге.
    /// </summary>
    [Column("pages")]
    public int? Pages { get; set; }

    /// <summary>
    /// Идентификатор автора, связанного с книгой.
    /// </summary>
    [Column("author_id")]
    public int AuthorId { get; set; }
}