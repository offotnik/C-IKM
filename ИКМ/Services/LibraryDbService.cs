using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

/// <summary>
/// Сервис для выполнения CRUD-операций с библиотечной базой данных PostgreSQL.
/// Строка подключения задана напрямую в коде.
/// </summary>
public static class LibraryDbService
{
    /// <summary>
    /// Строка подключения к PostgreSQL. 
    /// </summary>
    private static readonly string ConnectionString =
        "Host=localhost;Port=5432;Database=LibraryDB;Username=postgres;Password=1111;";


    // =============== AUTHORS ===============

    /// <summary>
    /// Получает список всех авторов из базы данных.
    /// </summary>
    public static async Task<List<Author>> GetAllAuthorsAsync()
    {
        const string sql = @"
        SELECT 
            author_id AS AuthorId,
            last_name AS LastName,
            first_name AS FirstName,
            middle_name AS MiddleName
        FROM Authors";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            var authors = await connection.QueryAsync<Author>(sql);
            return new List<Author>(authors);
        }
    }

    /// <summary>
    /// Добавляет нового автора в базу данных.
    /// </summary>
    public static async Task<int> CreateAuthorAsync(Author author)
    {
        const string sql = @"
            INSERT INTO Authors (last_name, first_name, middle_name) 
            VALUES (@LastName, @FirstName, @MiddleName) 
            RETURNING author_id;";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            var id = await connection.QuerySingleAsync<int>(sql, author);
            return id;
        }
    }

    /// <summary>
    /// Обновляет данные существующего автора.
    /// </summary>
    public static async Task UpdateAuthorAsync(Author author)
    {
        const string sql = @"
            UPDATE Authors 
            SET last_name = @LastName, first_name = @FirstName, middle_name = @MiddleName 
            WHERE author_id = @AuthorId;";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            await connection.ExecuteAsync(sql, author);
        }
    }

    /// <summary>
    /// Удаляет автора из базы данных по идентификатору.
    /// </summary>
    public static async Task DeleteAuthorAsync(int id)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            await connection.ExecuteAsync("DELETE FROM Authors WHERE author_id = @Id", new { Id = id });
        }
    }

    // =============== READERS ===============

    /// <summary>
    /// Получает список всех читателей из базы данных.
    /// </summary>
    public static async Task<List<Reader>> GetAllReadersAsync()
    {
        const string sql = @"
        SELECT 
            user_id AS UserId,
            last_name AS LastName,
            first_name AS FirstName,
            middle_name AS MiddleName,
            birth_date AS BirthDate,
            phone AS Phone,
            email AS Email,
            address AS Address,
            get_rent AS GetRent,
            must_return AS MustReturn
        FROM Readers";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            var readers = await connection.QueryAsync<Reader>(sql);
            return new List<Reader>(readers);
        }
    }

    /// <summary>
    /// Добавляет нового читателя в базу данных.
    /// </summary>
    public static async Task<int> CreateReaderAsync(Reader reader)
    {
        const string sql = @"
            INSERT INTO Readers (last_name, first_name, middle_name, birth_date, phone, email, address, get_rent, must_return) 
            VALUES (@LastName, @FirstName, @MiddleName, @BirthDate, @Phone, @Email, @Address, @GetRent, @MustReturn) 
            RETURNING user_id;";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            var id = await connection.QuerySingleAsync<int>(sql, reader);
            return id;
        }
    }

    /// <summary>
    /// Обновляет данные существующего читателя.
    /// </summary>
    public static async Task UpdateReaderAsync(Reader reader)
    {
        const string sql = @"
            UPDATE Readers 
            SET last_name = @LastName, first_name = @FirstName, middle_name = @MiddleName,
                birth_date = @BirthDate, phone = @Phone, email = @Email, address = @Address,
                get_rent = @GetRent, must_return = @MustReturn
            WHERE user_id = @UserId;";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            await connection.ExecuteAsync(sql, reader);
        }
    }

    /// <summary>
    /// Удаляет читателя из базы данных по идентификатору.
    /// </summary>
    public static async Task DeleteReaderAsync(int id)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            await connection.ExecuteAsync("DELETE FROM Readers WHERE user_id = @Id", new { Id = id });
        }
    }

    // =============== BOOKS ===============

    /// <summary>
    /// Получает список всех книг из базы данных.
    /// </summary>
    public static async Task<List<Book>> GetAllBooksAsync()
    {

        const string sql = @"
        SELECT 
            book_id AS BookId,
            title AS Title,
            year AS Year,
            language AS Language,
            pages AS Pages,
            author_id AS AuthorId
        FROM Books";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            var books = await connection.QueryAsync<Book>(sql);
            return new List<Book>(books);
        }
    }

    /// <summary>
    /// Добавляет новую книгу в базу данных.
    /// </summary>
    public static async Task<int> CreateBookAsync(Book book)
    {
        const string sql = @"
            INSERT INTO Books (title, year, language, pages, author_id) 
            VALUES (@Title, @Year, @Language, @Pages, @AuthorId) 
            RETURNING book_id;";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            var id = await connection.QuerySingleAsync<int>(sql, book);
            return id;
        }
    }

    /// <summary>
    /// Обновляет данные существующей книги.
    /// </summary>
    public static async Task UpdateBookAsync(Book book)
    {
        const string sql = @"
            UPDATE Books 
            SET title = @Title, year = @Year, language = @Language, pages = @Pages, author_id = @AuthorId
            WHERE book_id = @BookId;";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            await connection.ExecuteAsync(sql, book);
        }
    }

    /// <summary>
    /// Удаляет книгу из базы данных по идентификатору.
    /// </summary>
    public static async Task DeleteBookAsync(int id)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            await connection.ExecuteAsync("DELETE FROM Books WHERE book_id = @Id", new { Id = id });
        }
    }

    // =============== BOOK COPIES ===============

    /// <summary>
    /// Получает список всех экземпляров книг из базы данных.
    /// </summary>
    public static async Task<List<BookCopy>> GetAllCopiesAsync()
    {

        const string sql = @"
        SELECT 
            copy_id AS CopyId,
            book_id AS BookId,
            is_available AS IsAvailable
        FROM BookCopies";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            var copies = await connection.QueryAsync<BookCopy>(sql);
            return new List<BookCopy>(copies);
        }
    }

    /// <summary>
    /// Добавляет новый экземпляр книги в базу данных.
    /// </summary>
    public static async Task<int> CreateCopyAsync(BookCopy copy)
    {
        const string sql = @"
            INSERT INTO BookCopies (book_id, is_available) 
            VALUES (@BookId, @IsAvailable) 
            RETURNING copy_id;";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            var id = await connection.QuerySingleAsync<int>(sql, copy);
            return id;
        }
    }

    /// <summary>
    /// Обновляет данные существующего экземпляра книги.
    /// </summary>
    public static async Task UpdateCopyAsync(BookCopy copy)
    {
        const string sql = @"
            UPDATE BookCopies 
            SET book_id = @BookId, is_available = @IsAvailable 
            WHERE copy_id = @CopyId;";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            await connection.ExecuteAsync(sql, copy);
        }
    }

    /// <summary>
    /// Удаляет экземпляр книги из базы данных по идентификатору.
    /// </summary>
    public static async Task DeleteCopyAsync(int id)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            await connection.ExecuteAsync("DELETE FROM BookCopies WHERE copy_id = @Id", new { Id = id });
        }
    }

    // =============== LOANS ===============

    /// <summary>
    /// Получает список всех записей о выдачах книг из базы данных.
    /// </summary>
    public static async Task<List<Loan>> GetAllLoansAsync()
    {

        const string sql = @"
        SELECT 
            loan_id AS LoanId,
            user_id AS UserId,
            copy_id AS CopyId,
            bear_date AS BearDate,
            due_date AS DueDate,
            return_date AS ReturnDate,
            is_returned AS IsReturned
        FROM Loans";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            var loans = await connection.QueryAsync<Loan>(sql);
            return new List<Loan>(loans);
        }
    }

    /// <summary>
    /// Добавляет новую запись о выдаче книги в базу данных.
    /// </summary>
    public static async Task<int> CreateLoanAsync(Loan loan)
    {
        const string sql = @"
            INSERT INTO Loans (user_id, copy_id, bear_date, due_date, return_date, is_returned) 
            VALUES (@UserId, @CopyId, @BearDate, @DueDate, @ReturnDate, @IsReturned) 
            RETURNING loan_id;";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            var id = await connection.QuerySingleAsync<int>(sql, loan);
            return id;
        }
    }

    /// <summary>
    /// Обновляет данные существующей записи о выдаче.
    /// </summary>
    public static async Task UpdateLoanAsync(Loan loan)
    {
        const string sql = @"
            UPDATE Loans 
            SET user_id = @UserId, copy_id = @CopyId, bear_date = @BearDate,
                due_date = @DueDate, return_date = @ReturnDate, is_returned = @IsReturned
            WHERE loan_id = @LoanId;";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            await connection.ExecuteAsync(sql, loan);
        }
    }

    /// <summary>
    /// Удаляет запись о выдаче из базы данных по идентификатору.
    /// </summary>
    public static async Task DeleteLoanAsync(int id)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            await connection.ExecuteAsync("DELETE FROM Loans WHERE loan_id = @Id", new { Id = id });
        }
    }

    // =============== BLOCKS ===============

    /// <summary>
    /// Получает список всех блокировок читателей из базы данных.
    /// </summary>
    public static async Task<List<Block>> GetAllBlocksAsync()
    {

        const string sql = @"
        SELECT 
            block_id AS BlockId,
            user_id AS UserId,
            is_blocked AS IsBlocked,
            block_reason AS BlockReason,
            paid_amount AS PaidAmount
        FROM Blocks";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            var blocks = await connection.QueryAsync<Block>(sql);
            return new List<Block>(blocks);
        }
    }

    /// <summary>
    /// Добавляет новую блокировку читателя в базу данных.
    /// </summary>
    public static async Task<int> CreateBlockAsync(Block block)
    {
        const string sql = @"
            INSERT INTO Blocks (user_id, is_blocked, block_reason, paid_amount) 
            VALUES (@UserId, @IsBlocked, @BlockReason, @PaidAmount) 
            RETURNING block_id;";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            var id = await connection.QuerySingleAsync<int>(sql, block);
            return id;
        }
    }

    /// <summary>
    /// Обновляет данные существующей блокировки.
    /// </summary>
    public static async Task UpdateBlockAsync(Block block)
    {
        const string sql = @"
            UPDATE Blocks 
            SET user_id = @UserId, is_blocked = @IsBlocked, block_reason = @BlockReason, paid_amount = @PaidAmount
            WHERE block_id = @BlockId;";

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            await connection.ExecuteAsync(sql, block);
        }
    }

    /// <summary>
    /// Удаляет блокировку из базы данных по идентификатору.
    /// </summary>
    public static async Task DeleteBlockAsync(int id)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            await connection.OpenAsync();
            await connection.ExecuteAsync("DELETE FROM Blocks WHERE block_id = @Id", new { Id = id });
        }
    }
}