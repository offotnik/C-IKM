using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Представляет блокировку читателя за нарушение правил библиотеки.
/// </summary>
[Table("blocks")]
public class Block
{
    /// <summary>
    /// Уникальный идентификатор блокировки, генерируемый базой данных.
    /// </summary>
    [Column("block_id")]
    public int BlockId { get; set; }

    /// <summary>
    /// Идентификатор заблокированного читателя.
    /// Ссылается на таблицу Readers.
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// Флаг активной блокировки.
    /// true — читатель заблокирован, false — блокировка снята.
    /// </summary>
    [Column("is_blocked")]
    public bool IsBlocked { get; set; }

    /// <summary>
    /// Причина блокировки (например, "Просрочка возврата").
    /// </summary>
    [Column("block_reason")]
    public string BlockReason { get; set; }

    /// <summary>
    /// Сумма оплаченного штрафа в рублях.
    /// </summary>
    [Column("paid_amount")]
    public int PaidAmount { get; set; }
}