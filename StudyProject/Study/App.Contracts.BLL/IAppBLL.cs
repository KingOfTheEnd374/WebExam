using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBLL : IBLL
{
    IGameService Games { get; }
    IGenreService Genres { get; }
    IPlatformService Platforms { get; }
    IDeveloperService Developers { get; }
    IPublisherService Publishers { get; }
    IReviewService Reviews { get; }
}