using System.Collections;
using AngleSharp.Common;
using App.BLL;
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.DAL.EF;
using App.Domain.Identity;
using App.DTO.v1_0;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using WebApp.ApiControllers;

namespace App.Test;

public class ApiControllerGamesTest
{
    private AppDbContext _ctx;
    private IAppBLL _bll;

    private IAppUnitOfWork _uow;

    private UserManager<AppUser> _userManager;
    
    // sut
    private GamesController _controller;
    
    public ApiControllerGamesTest()
    {
        // set up mock database - inmemory
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // use random guid as db instance id
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        
        _ctx = new AppDbContext(optionsBuilder.Options);
        
        var configUow = new MapperConfiguration(cfg => cfg.CreateMap<App.Domain.Game, App.DAL.DTO.Game>().ReverseMap());
        var mapperUow = configUow.CreateMapper();
        _uow = new AppUOW(_ctx, mapperUow);
        
        var configBll = new MapperConfiguration(cfg => cfg.CreateMap<App.DAL.DTO.Game, App.BLL.DTO.Game>().ReverseMap());
        var mapperBll = configBll.CreateMapper();
        _bll = new AppBLL(_uow, mapperBll);
        
        var configWeb = new MapperConfiguration(cfg => cfg.CreateMap<App.BLL.DTO.Game, App.DTO.v1_0.Game>().ReverseMap());
        var mapperWeb = configWeb.CreateMapper();

        var storeStub = Substitute.For<IUserStore<AppUser>>();
        var optionsStub = Substitute.For<IOptions<IdentityOptions>>();
        var hasherStub = Substitute.For<IPasswordHasher<AppUser>>();

        var validatorStub = Substitute.For<IEnumerable<IUserValidator<AppUser>>>();
        var passwordStub = Substitute.For<IEnumerable<IPasswordValidator<AppUser>>>();
        var lookupStub = Substitute.For<ILookupNormalizer>();
        var errorStub = Substitute.For<IdentityErrorDescriber>();
        var serviceStub = Substitute.For<IServiceProvider>();
        var loggerStub = Substitute.For<ILogger<UserManager<AppUser>>>();

        _userManager = Substitute.For<UserManager<AppUser>>(
            storeStub, 
            optionsStub, 
            hasherStub,
            validatorStub, passwordStub, lookupStub, errorStub, serviceStub, loggerStub
        );

        
        _controller = new GamesController(_bll, _userManager, mapperWeb);
         _userManager.GetUserId(_controller.User).Returns(/*Guid.NewGuid().ToString()*/ "c50da13e-81d7-454e-836d-2e50d2a16cbe");
    }
    
    [Fact]
    public async Task GetEmptyTest()
    {
        // Get all games, should be empty
        var result = await _controller.GetGames();
        var okRes = result.Result as OkObjectResult;
        var val = okRes!.Value as IEnumerable<App.DTO.v1_0.Game>;
        Assert.Empty(val);
    }
    
    [Fact]
    public async Task GetTest()
    {
        // Get all games, should be empty
        var result = await _controller.GetGames();
        var okRes = result.Result as OkObjectResult;
        var val = okRes!.Value as IEnumerable<App.DTO.v1_0.Game>;
        Assert.Empty(val);
        
        // Create and add new AppUser
        var user = new AppUser();
        user.Id = Guid.Parse("c50da13e-81d7-454e-836d-2e50d2a16cbe");
        user.Email = "test@email.test";
        user.Username = "TestUser";
        await _ctx.Users.AddAsync(user);
        await _ctx.SaveChangesAsync();
        
        // Create new game object
        var game = new App.DTO.v1_0.Game();
        game.Name = "Game Name";
        game.Release = "2020-10-10";
        
        //Add game to DB
        var result1 = await _controller.PostGame(game);
        var createdRes = result1.Result as CreatedAtActionResult;
        var val2 = createdRes!.Value as App.DTO.v1_0.Game;
        Assert.NotNull(val2);
        
        // Get all games, shouldn't be empty now
        var result3 = await _controller.GetGames();
        var okRes3 = result3.Result as OkObjectResult;
        var val3 = okRes3!.Value as IEnumerable<App.DTO.v1_0.Game>;
        Assert.NotEmpty(val3);
    }

    [Fact]
    public async Task PutTest()
    {
        // Create new game object
        var game = new App.DTO.v1_0.Game();
        game.Name = "Game Name";
        game.Release = "2020-10-10";
        
        //Add game to DB
        var result = await _controller.PostGame(game);
        var createdRes = result.Result as CreatedAtActionResult;
        var val = createdRes!.Value as App.DTO.v1_0.Game;
        Assert.NotNull(val);
    }
    
    [Fact]
    public async Task GetSingleTest()
    {
        // Create and add new AppUser
        var user = new AppUser();
        user.Id = Guid.Parse("c50da13e-81d7-454e-836d-2e50d2a16cbe");
        user.Email = "test@email.test";
        user.Username = "TestUser";
        await _ctx.Users.AddAsync(user);
        await _ctx.SaveChangesAsync();
        
        // Create new game object
        var game = new App.DTO.v1_0.Game();
        game.Name = "Game Name";
        game.Release = "2020-10-10";
        
        //Add game to DB
        var result = await _controller.PostGame(game);
        var createdRes = result.Result as CreatedAtActionResult;
        var val = createdRes!.Value as App.DTO.v1_0.Game;
        Assert.NotNull(val);

        var id = val.Id;

        // Try to find a game with invalid ID, will return Not Found
        var notFound = await _controller.GetGame(Guid.Empty);
        Assert.IsType<NotFoundResult>(notFound.Result);
        
        // Find game with ID
        var result1 = await _controller.GetGame(id);
        val = result1!.Value;
        Assert.Equal(id, val!.Id);
        Assert.Equal(game.Name, val!.Name);
        Assert.Equal(game.Release, val!.Release);
    }
    
    [Fact]
    public async Task EditTest()
    {
        // Create and add new AppUser
        var user = new AppUser();
        user.Id = Guid.Parse("c50da13e-81d7-454e-836d-2e50d2a16cbe");
        user.Email = "test@email.test";
        user.Username = "TestUser";
        await _ctx.Users.AddAsync(user);
        await _ctx.SaveChangesAsync();

        // Create new game object
        var game = new App.DTO.v1_0.Game();
        game.Name = "Game Name";
        game.Release = "2020-10-10";
        
        //Add game to DB
        var result = await _controller.PostGame(game);
        var createdRes = result.Result as CreatedAtActionResult;
        var val = createdRes!.Value as App.DTO.v1_0.Game;
        Assert.NotNull(val);
        Assert.Equal(game.Name, val.Name);

        var id = val.Id;
        var newName = "Game Name Edited";
        
        // Create second game object
        var game2 = new App.DTO.v1_0.Game();
        game2.Id = id;
        game2.Name = newName;
        game2.Release = "2020-10-10";

        // Detach the existing entity from the context to prevent tracking conflicts
        var trackedEntity = _ctx.ChangeTracker.Entries<App.Domain.Game>().FirstOrDefault(e => e.Entity.Id == id);
        if (trackedEntity != null)
        {
            _ctx.Entry(trackedEntity.Entity).State = EntityState.Detached;
        }

        // Try to edit game with invalid Id, will receive BadRequest
        var badRequest = await _controller.PutGame(Guid.Empty, game2);
        Assert.IsType<BadRequestResult>(badRequest);
        
        // Edit game
        await _controller.PutGame(id, game2);
        
        // Get game and check new values
        var result1 = await _controller.GetGame(id);
        val = result1!.Value;
        Assert.Equal(id, val!.Id);
        Assert.Equal(newName, val!.Name);
        Assert.Equal(game.Release, val!.Release);
    }
    
    [Fact]
    public async Task DeleteTest()
    {
        // Create and add new AppUser
        var user = new AppUser();
        user.Id = Guid.Parse("c50da13e-81d7-454e-836d-2e50d2a16cbe");
        user.Email = "test@email.test";
        user.Username = "TestUser";
        await _ctx.Users.AddAsync(user);
        await _ctx.SaveChangesAsync();
        
        // Create new game object
        var game = new App.DTO.v1_0.Game();
        game.Name = "Game Name";
        game.Release = "2020-10-10";
        
        //Add game to DB
        var result = await _controller.PostGame(game);
        var createdRes = result.Result as CreatedAtActionResult;
        var val = createdRes!.Value as App.DTO.v1_0.Game;
        Assert.NotNull(val);

        var id = val.Id;
        
        // Find game with ID
        var result1 = await _controller.GetGame(id);
        val = result1!.Value;
        Assert.Equal(id, val!.Id);
        Assert.Equal(game.Name, val!.Name);
        Assert.Equal(game.Release, val!.Release);

        // Try to delete game with invalid ID, receive NotFound
        var notFound = await _controller.DeleteGame(Guid.Empty);
        Assert.IsType<NotFoundResult>(notFound);
        
        // Delete game
        await _controller.DeleteGame(id);
        
        // Check that the game has been deleted 
        var result2 = await _controller.GetGame(id);
        Assert.IsType<NotFoundResult>(result2.Result);
    }

    [Fact]
    public async Task FilterTest()
    {
        // Create and add new AppUser
        var user = new AppUser();
        user.Id = Guid.Parse("c50da13e-81d7-454e-836d-2e50d2a16cbe");
        user.Email = "test@email.test";
        user.Username = "TestUser";
        await _ctx.Users.AddAsync(user);
        await _ctx.SaveChangesAsync();

        // Create new game object
        var game1 = new App.DTO.v1_0.Game();
        game1.Name = "Game Name1";
        game1.Release = "2025-10-10";
        game1.Price = 0;
        game1.EstimatedPlaytime = 0;
        game1.Owned = false;
        game1.Excitement = 0;
        game1.Status = '0';
        game1.Comment = "bad game";
        
        // Add game to DB
        var result = await _controller.PostGame(game1);
        var createdRes = result.Result as CreatedAtActionResult;
        var val = createdRes!.Value as App.DTO.v1_0.Game;
        Assert.NotNull(val);
        Assert.Equal(game1.Name, val.Name);
        
        
        // Create second game object
        var game2 = new App.DTO.v1_0.Game();
        game2.Name = "Game Name2";
        game2.Release = "2020-10-10";
        game2.Price = 5;
        game2.EstimatedPlaytime = 10;
        game2.Owned = true;
        game2.Excitement = 10;
        game2.Status = '0';
        game2.Comment = "Good game";
        
        // Add second game to DB
        result = await _controller.PostGame(game2);
        createdRes = result.Result as CreatedAtActionResult;
        val = createdRes!.Value as App.DTO.v1_0.Game;
        Assert.NotNull(val);
        Assert.Equal(game2.Name, val.Name);

        var response = await _controller.GetGames("2", 0, 1, 0, 1, 1, 5, 12);
        var okRes = response.Result as OkObjectResult;
        var games = (List<App.DTO.v1_0.Game>)okRes!.Value!;
        Assert.Single(games);
        Assert.Equal(game2.Name, games[0].Name);
        
        response = await _controller.GetGames("", 0, 0, 0, 0, 0, 0, 0);
        okRes = response.Result as OkObjectResult;
        games = (List<App.DTO.v1_0.Game>)okRes!.Value!;
        Assert.Single(games);
        Assert.Equal(game1.Name, games[0].Name);
        
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                response = await _controller.GetGames("", j, i, 0, 0, 0, 0, 0);
                Assert.IsType<OkObjectResult>(response.Result);
            }
        }
    }
}