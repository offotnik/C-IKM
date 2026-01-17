using System;
using System.ComponentModel.DataAnnotations.Schema;

// <summary>
/// Представляет читателя библиотеки.
/// </summary>
[Table("readers")]
public class Reader
{
    /// <summary>
    /// Уникальный идентификатор читателя.
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// Фамилия читателя.
    /// </summary>
    [Column("last_name")]
    public string LastName { get; set; }

    /// <summary>
    /// Имя читателя.
    /// </summary>
    [Column("first_name")]
    public string FirstName { get; set; }

    /// <summary>
    /// Отчество читателя.
    /// </summary>
    [Column("middle_name")]
    public string MiddleName { get; set; }

    /// <summary>
    /// Дата рождения читателя.
    /// </summary>
    [Column("birth_date")]
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Контактный телефон читателя.
    /// </summary>
    [Column("phone")]
    public string Phone { get; set; }

    /// <summary>
    /// Электронная почта читателя.
    /// </summary>
    [Column("email")]
    public string Email { get; set; }

    /// <summary>
    /// Адрес проживания читателя.
    /// </summary>
    [Column("address")]
    public string Address { get; set; }

    /// <summary>
    /// Флаг, разрешающий выдачу книг.
    /// </summary>
    [Column("get_rent")]
    public bool GetRent { get; set; }

    /// <summary>
    /// Дата, до которой читатель должен вернуть книгу.
    /// </summary>
    [Column("must_return")]
    public DateTime? MustReturn { get; set; }
}