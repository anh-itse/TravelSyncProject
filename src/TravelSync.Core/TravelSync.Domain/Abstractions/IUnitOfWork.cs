using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace TravelSync.Domain.Abstractions;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Bắt đầu một giao dịch database mới. Nếu đã có giao dịch đang mở, sẽ trả về giao dịch hiện tại.
    /// </summary>
    /// <param name="cancellationToken">Token để hủy thao tác nếu cần.</param>
    /// <returns>Giao dịch hiện tại dưới dạng <see cref="IDbContextTransaction"/>.</returns>
    Task<IUnitOfWork> BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Lưu tất cả các thay đổi vào database và commit giao dịch hiện tại.
    /// </summary>
    /// <param name="cancellationToken">Token để hủy thao tác nếu cần.</param>
    /// <returns>Không trả về dữ liệu.</returns>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Hủy bỏ giao dịch hiện tại, hoàn tác tất cả các thay đổi trong giao dịch.
    /// </summary>
    /// <param name="cancellationToken">Token để hủy thao tác nếu cần.</param>
    /// <returns>Không trả về dữ liệu.</returns>
    Task RollbackAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Lưu tất cả thay đổi vào database mà không cần transaction.
    /// </summary>
    /// <param name="cancellationToken">Token để hủy thao tác nếu cần.</param>
    /// <returns>Số lượng bản ghi bị ảnh hưởng.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy giao dịch hiện tại nếu có.
    /// </summary>
    /// <returns>Giao dịch hiện tại dưới dạng <see cref="IDbTransaction"/> hoặc null nếu không có.</returns>
    IDbTransaction? GetCurrentTransaction();
}