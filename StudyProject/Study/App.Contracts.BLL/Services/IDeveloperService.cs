using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IDeveloperService :  IEntityRepository<App.BLL.DTO.Developer>
{
    
}