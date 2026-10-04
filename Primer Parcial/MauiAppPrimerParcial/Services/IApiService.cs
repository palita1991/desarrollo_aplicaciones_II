using HTTPmaui.Models;

namespace HTTPmaui.Services;

// Interfaz que define el contrato de un servicio de acceso a datos vía HTTP.
public interface IApiService
{
    Task<IReadOnlyList<Post>> GetPostsAsync(CancellationToken ct = default);

    Task<Post?> GetPostByIdAsync(int id, CancellationToken ct = default);
}