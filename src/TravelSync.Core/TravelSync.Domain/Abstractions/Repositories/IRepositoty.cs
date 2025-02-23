namespace TravelSync.Domain.Abstractions.Repositories;

using System.Linq.Expressions;

/// <summary>
/// Interface định nghĩa các thao tác cơ bản của Repository.
/// </summary>
/// <typeparam name="TEntity">Loại entity.</typeparam>
/// <typeparam name="TKey">Kiểu dữ liệu của khóa chính.</typeparam>
public interface IRepository<TEntity, TKey>
    where TEntity : class
{
    /// <summary>
    /// Tìm một entity theo Id.
    /// </summary>
    /// <param name="id">Khóa chính của entity.</param>
    /// <param name="cancellationToken">Token để hủy bỏ tác vụ.</param>
    /// <returns>Entity nếu tìm thấy, ngược lại trả về null.</returns>
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy entity đầu tiên thỏa mãn điều kiện.
    /// </summary>
    /// <param name="predicate">Biểu thức điều kiện.</param>
    /// <param name="cancellationToken">Token để hủy bỏ tác vụ.</param>
    /// <returns>Entity đầu tiên thỏa mãn điều kiện hoặc null nếu không tìm thấy.</returns>
    Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy truy vấn IQueryable.
    /// </summary>
    /// <returns>IQueryable để thực hiện truy vấn.</returns>
    IQueryable<TEntity> GetQueryable();

    /// <summary>
    /// Lấy danh sách entity có điều kiện và ánh xạ sang DTO.
    /// </summary>
    /// <typeparam name="TDto">Kiểu dữ liệu DTO.</typeparam>
    /// <param name="predicate">Biểu thức điều kiện.</param>
    /// <param name="selector">Biểu thức ánh xạ sang DTO.</param>
    /// <param name="cancellationToken">Token để hủy bỏ tác vụ.</param>
    /// <returns>Danh sách DTO.</returns>
    Task<List<TDto>> GetListAsync<TDto>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TDto>> selector,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Lấy danh sách có phân trang từ truy vấn dữ liệu đầu vào.
    /// </summary>
    /// <typeparam name="TDto">Kiểu dữ liệu DTO.</typeparam>
    /// <param name="query">Truy vấn dữ liệu đầu vào dưới dạng IQueryable.</param>
    /// <param name="pageIndex">Chỉ mục trang (bắt đầu từ 0).</param>
    /// <param name="pageSize">Số lượng phần tử trên mỗi trang.</param>
    /// <param name="orderBy">Biểu thức sắp xếp (tùy chọn).</param>
    /// <param name="orderByDescending">Sắp xếp giảm dần hay không (mặc định: true).</param>
    /// <param name="cancellationToken">Token để hủy bỏ tác vụ.</param>
    /// <returns>Danh sách DTO đã phân trang và tổng số phần tử.</returns>
    Task<(List<TDto> Items, int TotalCount)> GetPagedListAsync<TDto>(
        IQueryable<TDto> query,
        int pageIndex,
        int pageSize,
        Expression<Func<TDto, object>>? orderBy = null,
        bool orderByDescending = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Thêm một entity vào database.
    /// </summary>
    /// <param name="entity">Entity cần thêm.</param>
    /// <param name="cancellationToken">Token để hủy bỏ tác vụ.</param>
    /// <returns>Không có dữ liệu trả về.</returns>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thêm nhiều entity vào database.
    /// </summary>
    /// <param name="entities">Danh sách entity cần thêm.</param>
    /// <param name="cancellationToken">Token để hủy bỏ tác vụ.</param>
    /// <returns>Không có dữ liệu trả về.</returns>
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thêm một entity vào database và lưu thay đổi ngay lập tức.
    /// </summary>
    /// <param name="entity">Entity cần thêm.</param>
    /// <param name="cancellationToken">Token để hủy bỏ tác vụ.</param>
    /// <returns>Không có dữ liệu trả về.</returns>
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thêm một entity vào database, lưu thay đổi và trả về Id của entity đó.
    /// </summary>
    /// <param name="entity">Entity cần thêm.</param>
    /// <param name="cancellationToken">Token để hủy bỏ tác vụ.</param>
    /// <returns>Id của entity vừa thêm.</returns>
    Task<TKey> InsertAndGetIdAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật thông tin của một entity nhưng không lưu thay đổi ngay.
    /// </summary>
    /// <param name="entity">Entity cần cập nhật.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Cập nhật entity trong database.
    /// </summary>
    /// <param name="entity">Entity cần cập nhật.</param>
    /// <param name="cancellationToken">Token để hủy bỏ tác vụ.</param>
    /// <returns>Không có dữ liệu trả về.</returns>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa entity theo Id.
    /// </summary>
    /// <param name="id">Id của entity cần xóa.</param>
    /// <param name="cancellationToken">Token để hủy bỏ tác vụ.</param>
    /// <returns>Không có dữ liệu trả về.</returns>
    Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa các entity thỏa mãn điều kiện.
    /// </summary>
    /// <param name="predicate">Biểu thức điều kiện.</param>
    /// <param name="cancellationToken">Token để hủy bỏ tác vụ.</param>
    /// <returns>Không có dữ liệu trả về.</returns>
    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa một entity theo Id nhưng không lưu thay đổi ngay.
    /// </summary>
    /// <param name="id">Id của entity cần xóa.</param>
    void Delete(TKey id);

    /// <summary>
    /// Xóa các entity thỏa mãn điều kiện nhưng không lưu thay đổi ngay.
    /// </summary>
    /// <param name="predicate">Biểu thức điều kiện để xác định entity cần xóa.</param>
    void Delete(Expression<Func<TEntity, bool>> predicate);
}
