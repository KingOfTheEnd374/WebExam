using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IReviewRepository : IEntityRepository<DALDTO.Review>, IReviewRepositoryCustom<DALDTO.Review>
{
    // Custom methods here
}

public interface IReviewRepositoryCustom<TEntity>
{
    Task<Boolean> HasGameIdDuplicate(Guid userId, Guid gameId);
    Task<TEntity> FindReviewForGame(Guid userId, Guid gameId);
    
    Task<IEnumerable<TEntity>> GetAllReviews(Guid userId);
}