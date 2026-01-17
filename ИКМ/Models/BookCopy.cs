using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Представляет физический экземпляр книги в библиотеке.
/// </summary>
[Table("bookcopies")]
public class BookCopy
{
    /// <summary>
    /// Уникальный идентификатор экземпляра, генерируемый базой данных.
    /// </summary>
    [Column("copy_id")]
    public int CopyId { get; set; }

    /// <summary>
    /// Идентификатор книги, к которой относится экземпляр.
    /// Ссылается на таблицу Books.
    /// </summary>
    [Column("book_id")]
    public int BookId { get; set; }

    /// <summary>
    /// Флаг доступности экземпляра для выдачи.
    /// true — доступен, false — выдан или заблокирован.
    /// </summary>
    [Column("is_available")]
    public bool IsAvailable { get; set; }
}