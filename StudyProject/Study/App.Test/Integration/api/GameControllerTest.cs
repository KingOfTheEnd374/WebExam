using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
//using App.BLL.DTO;
using App.DTO.v1_0;
using App.DTO.v1_0.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using NuGet.Protocol;

namespace App.Test.Integration.api;

[Collection("NonParallel")]
public class GameControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    
    public GameControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    
    [Fact]
    public async Task IndexRequiredLogin()
    {
        // Try to get games
        var response = await _client.GetAsync("/api/v1.0/Games");

        // Unauthorized error, because user is not logged in
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    [Fact]
    public async Task Register()
    {
        // Register and get jwt
        var response =
            await _client.PostAsJsonAsync("/api/v1.0/identity/Account/Register", new {email = "test@test.te", password = "Test123.", username = "TestName"});
        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var loginData = JsonSerializer.Deserialize<JWTResponse>(contentStr, options);

        Assert.NotNull(loginData);
        Assert.NotNull(loginData.Jwt);
        Assert.True(loginData.Jwt.Length > 0);

        // Get games, shouldn't get errors, because user is logged in
        var msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Games");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Login()
    {
        var user = "admin@eesti.ee";
        var pass = "Kala.maja1";

        // Login and get jwt
        var response =
            await _client.PostAsJsonAsync("/api/v1.0/identity/Account/Login", new {email = user, password = pass});
        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var loginData = JsonSerializer.Deserialize<JWTResponse>(contentStr, options);

        Assert.NotNull(loginData);
        Assert.NotNull(loginData.Jwt);
        Assert.True(loginData.Jwt.Length > 0);

        // Get games, shouldn't get errors, because user is logged in
        var msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Games");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task AddGame()
    {
        var user = "admin@eesti.ee";
        var pass = "Kala.maja1";

        // Login and get jwt
        var response =
            await _client.PostAsJsonAsync("/api/v1.0/identity/Account/Login", new {email = user, password = pass});
        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var loginData = JsonSerializer.Deserialize<JWTResponse>(contentStr, options);

        Assert.NotNull(loginData);
        Assert.NotNull(loginData.Jwt);
        Assert.True(loginData.Jwt.Length > 0);

        // Create new Game
        var game = new Game();
        game.Id = Guid.NewGuid();
        game.Name = "Game Name";
        game.Release = "2020-10-10";
        game.Price = 0;
        game.EstimatedPlaytime = 0;
        game.Owned = false;
        game.Excitement = 0;
        game.Status = '0';
        game.Comment = "bad game";
        game.GenreIds = [];
        game.PlatformIds = [];
        game.DeveloperIds = [];
        game.PublisherIds = [];
        
        string gameJson = JsonSerializer.Serialize(game);
        
        // Add it to DB
        var msg = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Games");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Content = new StringContent(gameJson, System.Text.Encoding.UTF8, "application/json");
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task SearchGame()
    {
        var user = "admin@eesti.ee";
        var pass = "Kala.maja1";

        // Login and get jwt
        var response =
            await _client.PostAsJsonAsync("/api/v1.0/identity/Account/Login", new {email = user, password = pass});
        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var loginData = JsonSerializer.Deserialize<JWTResponse>(contentStr, options);

        Assert.NotNull(loginData);
        Assert.NotNull(loginData.Jwt);
        Assert.True(loginData.Jwt.Length > 0);

        // Create new games and add them to DB
        var game1 = new Game();
        game1.Id = Guid.NewGuid();
        game1.Name = "Game Name1";
        game1.Release = "2025-10-10";
        game1.Price = 0;
        game1.EstimatedPlaytime = 0;
        game1.Owned = false;
        game1.Excitement = 0;
        game1.Status = '0';
        game1.Comment = "bad game";
        game1.GenreIds = [];
        game1.PlatformIds = [];
        game1.DeveloperIds = [];
        game1.PublisherIds = [];
        
        string gameJson = JsonSerializer.Serialize(game1);
        
        var msg = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Games");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Content = new StringContent(gameJson, System.Text.Encoding.UTF8, "application/json");
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();
        
        var game2 = new Game();
        game2.Id = Guid.NewGuid();
        game2.Name = "Game Name2";
        game2.Release = "2020-10-10";
        game2.Price = 5;
        game2.EstimatedPlaytime = 10;
        game2.Owned = true;
        game2.Excitement = 10;
        game2.Status = '0';
        game2.Comment = "Good game";
        game2.GenreIds = [];
        game2.PlatformIds = [];
        game2.DeveloperIds = [];
        game2.PublisherIds = [];

        string gameJson2 = JsonSerializer.Serialize(game2);
        
        var msg2 = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Games");
        msg2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg2.Content = new StringContent(gameJson2, System.Text.Encoding.UTF8, "application/json");
        msg2.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg2);

        response.EnsureSuccessStatusCode();

        // Search for a game that contains "2"
        var msg3 = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Games?search=2");
        msg3.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg3.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg3);
        response.EnsureSuccessStatusCode();
        var res2 = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        // Check that result is the correct game
        var resGames = JsonSerializer.Deserialize<List<DTO.v1_0.Game>>(res2, new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
        Assert.Equal(1, resGames!.Count);
        Assert.Equal(game2.Name, resGames[0].Name);
    }
    
    [Fact]
    public async Task Logout()
    {
        var user = "admin@eesti.ee";
        var pass = "Kala.maja1";

        // Login and get jwt
        var response =
            await _client.PostAsJsonAsync("/api/v1.0/identity/Account/Login", new {email = user, password = pass});
        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var loginData = JsonSerializer.Deserialize<JWTResponse>(contentStr, options);

        Assert.NotNull(loginData);
        Assert.NotNull(loginData.Jwt);
        Assert.True(loginData.Jwt.Length > 0);

        var logout = new
        {
            refreshToken = loginData.RefreshToken
        };
        string logoutJson = JsonSerializer.Serialize(logout);
        
        // Logout
        var msg = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/identity/Account/Logout");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Content = new StringContent(logoutJson, System.Text.Encoding.UTF8, "application/json");
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();
    }
}