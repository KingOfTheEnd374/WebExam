using App.Contracts.DAL.Repositories;
using App.Domain.Identity;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    IGameRepository Games { get; }
    IGenreRepository Genres { get; }
    IPlatformRepository Platforms { get; }
    IDeveloperRepository Developers { get; }
    IPublisherRepository Publishers { get; }
    IReviewRepository Reviews { get; }
    
    IEntityRepository<AppUser> Users { get; }
}