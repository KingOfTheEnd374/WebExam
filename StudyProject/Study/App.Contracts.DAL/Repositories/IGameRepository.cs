using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IGameRepository : IEntityRepository<DALDTO.Game>, IGameRepositoryCustom<DALDTO.Game>
{
    // DAL only Custom methods here
}

// Shader (with BLL) custom methods here
public interface IGameRepositoryCustom<TEntity>
{
    //Task<IEnumerable<TEntity>> GetAllSortedAsync(Guid userId);
    
    Task<IEnumerable<TEntity>> GetAllFilteredSortedAsync(Guid userId, string search, int sortBy, bool ascending, int status, bool owned, int released, int minTime, int maxTime);
}