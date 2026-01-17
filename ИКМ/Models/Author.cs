using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Представляет автора книги в библиотечной системе.
/// </summary>
[Table("authors")]
public class Author
{
    /// <summary>
    /// Уникальный идентификатор автора.
    /// </summary>
    [Column("author_id")]
    public int AuthorId { get; set; }

    /// <summary>
    /// Фамилия автора.
    /// </summary>
    [Column("last_name")]
    public string LastName { get; set; }

    /// <summary>
    /// Имя автора.
    /// </summary>
    [Column("first_name")]
    public string FirstName { get; set; }

    /// <summary>
    /// Отчество автора (может быть null).
    /// </summary>
    [Column("middle_name")]
    public string MiddleName { get; set; }
}