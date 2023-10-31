namespace Application.Repositories;

public interface IBuyRepository
{
    public Task SaveAsync(CancellationToken cancellationToken);
}