using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IGenreService :  IEntityRepository<App.BLL.DTO.Genre>
{
    
}