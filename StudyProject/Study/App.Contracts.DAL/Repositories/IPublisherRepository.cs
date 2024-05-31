using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IPublisherRepository : IEntityRepository<DALDTO.Publisher>
{
    // Custom methods here
}