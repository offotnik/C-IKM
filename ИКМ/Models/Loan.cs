using System;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Представляет запись о выдаче книги читателю.
/// </summary>
[Table("loans")]
public class Loan
{
    /// <summary>
    /// Уникальный идентификатор записи о выдаче, генерируемый базой данных.
    /// </summary>
    [Column("loan_id")]
    public int LoanId { get; set; }

    /// <summary>
    /// Идентификатор читателя, получившего книгу.
    /// Ссылается на таблицу Readers.
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// Идентификатор выданного экземпляра книги.
    /// Ссылается на таблицу BookCopies.
    /// </summary>
    [Column("copy_id")]
    public int CopyId { get; set; }

    /// <summary>
    /// Дата выдачи книги читателю.
    /// </summary>
    [Column("bear_date")]
    public DateTime BearDate { get; set; }

    /// <summary>
    /// Планируемая дата возврата книги.
    /// Может быть null.
    /// </summary>
    [Column("due_date")]
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Фактическая дата возврата книги.
    /// Может быть null, если книга ещё не возвращена.
    /// </summary>
    [Column("return_date")]
    public DateTime? ReturnDate { get; set; }

    /// <summary>
    /// Флаг возврата книги.
    /// true — книга возвращена, false — находится у читателя.
    /// </summary>
    [Column("is_returned")]
    public bool IsReturned { get; set; }
}