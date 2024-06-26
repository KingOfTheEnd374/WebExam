﻿using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IGameService :  IEntityRepository<App.BLL.DTO.Game>, IGameRepositoryCustom<App.BLL.DTO.Game>
{
    
}