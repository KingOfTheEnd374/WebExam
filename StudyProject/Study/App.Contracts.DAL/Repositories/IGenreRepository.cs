using DALDTO = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IGenreRepository : IEntityRepository<DALDTO.Genre>
{
    // Custom methods here
}