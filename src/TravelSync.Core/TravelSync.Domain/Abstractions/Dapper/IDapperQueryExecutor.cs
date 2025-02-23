using System.Data;

namespace TravelSync.Domain.Abstractions.Dapper;

/// <summary>
/// Interface định nghĩa các phương thức thực thi truy vấn bằng Dapper.
/// </summary>
public interface IDapperQueryExecutor
{
    /// <summary>
    /// Thực hiện truy vấn và trả về danh sách kết quả.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của kết quả.</typeparam>
    /// <param name="sql">Tên Stored Procedure hoặc câu lệnh SQL.</param>
    /// <param name="param">Danh sách tham số đầu vào (tùy chọn).</param>
    /// <param name="commandType">Loại lệnh (Stored Procedure hoặc Text).</param>
    /// <param name="transaction">Transaction hiện tại, nếu có.</param>
    /// <param name="cancellationToken">Token hủy tác vụ.</param>
    /// <returns>Danh sách kết quả.</returns>
    Task<List<T>> QueryAsync<T>(
        string sql,
        object? param = null,
        CommandType commandType = CommandType.StoredProcedure,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Thực hiện truy vấn và trả về phần tử đầu tiên hoặc giá trị mặc định nếu không có dữ liệu.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của kết quả.</typeparam>
    /// <param name="sql">Tên Stored Procedure hoặc câu lệnh SQL.</param>
    /// <param name="param">Danh sách tham số đầu vào (tùy chọn).</param>
    /// <param name="commandType">Loại lệnh (Stored Procedure hoặc Text).</param>
    /// <param name="transaction">Transaction hiện tại, nếu có.</param>
    /// <param name="cancellationToken">Token hủy tác vụ.</param>
    /// <returns>Phần tử đầu tiên hoặc giá trị mặc định.</returns>
    Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        object? param = null,
        CommandType commandType = CommandType.StoredProcedure,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Thực hiện truy vấn và đảm bảo chỉ có một phần tử được trả về.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của kết quả.</typeparam>
    /// <param name="sql">Tên Stored Procedure hoặc câu lệnh SQL.</param>
    /// <param name="param">Danh sách tham số đầu vào (tùy chọn).</param>
    /// <param name="commandType">Loại lệnh (Stored Procedure hoặc Text).</param>
    /// <param name="transaction">Transaction hiện tại, nếu có.</param>
    /// <param name="cancellationToken">Token hủy tác vụ.</param>
    /// <returns>Một phần tử duy nhất.</returns>
    Task<T> QuerySingleAsync<T>(
        string sql,
        object? param = null,
        CommandType commandType = CommandType.StoredProcedure,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Thực hiện các câu lệnh SQL như INSERT, UPDATE, DELETE và trả về số dòng bị ảnh hưởng.
    /// </summary>
    /// <param name="sql">Tên Stored Procedure hoặc câu lệnh SQL.</param>
    /// <param name="param">Danh sách tham số đầu vào (tùy chọn).</param>
    /// <param name="commandType">Loại lệnh (Stored Procedure hoặc Text).</param>
    /// <param name="transaction">Transaction hiện tại, nếu có.</param>
    /// <param name="cancellationToken">Token hủy tác vụ.</param>
    /// <returns>Số dòng bị ảnh hưởng.</returns>
    Task<int> ExecuteAsync(
        string sql,
        object? param = null,
        CommandType commandType = CommandType.StoredProcedure,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy kết nối cơ sở dữ liệu hiện tại.
    /// </summary>
    /// <returns>Đối tượng IDbConnection.</returns>
    IDbConnection GetDbConnection();
}

