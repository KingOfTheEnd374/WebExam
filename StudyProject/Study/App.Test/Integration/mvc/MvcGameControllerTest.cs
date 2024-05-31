using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using AngleSharp.Html.Dom;
using App.DTO.v1_0;
using Base.Test.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace App.Test.Integration.mvc;

[Collection("NonParallel")]
public class MvcGameControllerTest: IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;


    public MvcGameControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
    
    [Fact]
    public async Task LoadProtectedPageRedirects()
    {
        const string protectedUri = "/Games";
        
        var getResponse = await _client.GetAsync(protectedUri);

        Assert.Equal(HttpStatusCode.Found, getResponse.StatusCode);
    }

    [Fact]
    public async Task RegisterUserAsync()
    {
        const string registerUri = "/Identity/Account/Register";

        // this just gets the headers, body can be xxx length and is loaded later
        var getRegisterResponse = await _client.GetAsync(registerUri);

        getRegisterResponse.EnsureSuccessStatusCode();
        
        // get the actual content from response
        var getRegisterContent = await HtmlHelpers.GetDocumentAsync(getRegisterResponse);


        // get the form element from page content
        var formRegister = (IHtmlFormElement) getRegisterContent.QuerySelector("#registerForm")!;

        var formRegisterValues = new Dictionary<string, string>
        {
            ["Input_Username"] = "Username",
            ["Input_Email"] = "test@test.ee",
            ["Input_Password"] = "Foo.bar1",
            ["Input_ConfirmPassword"] = "Foo.bar1",
        };
        
        // send form with data to server, method (POST) is detected from form element
        var postRegisterResponse = await _client.SendAsync(formRegister, formRegisterValues);
        
        Assert.Equal(HttpStatusCode.Found, postRegisterResponse.StatusCode);
        
        
        
        const string protectedUri = "/Games";
        
        var getResponse = await _client.GetAsync(protectedUri);
        getResponse.EnsureSuccessStatusCode();

    }

    [Fact]
    public async Task Login()
    {
        const string loginUrl = "/Identity/Account/Login";

        // this just gets the headers, body can be xxx length and is loaded later
        var getLoginResponse = await _client.GetAsync(loginUrl);

        getLoginResponse.EnsureSuccessStatusCode();
        
        // get the actual content from response
        var getLoginContent = await HtmlHelpers.GetDocumentAsync(getLoginResponse);


        // get the form element from page content
        var formLogin = (IHtmlFormElement) getLoginContent.QuerySelector("#account")!;

        var formLoginValues = new Dictionary<string, string>
        {
            ["Input_Email"] = "admin2@eesti.ee",
            ["Input_Password"] = "Kala.maja2",
        };
        
        // send form with data to server, method (POST) is detected from form element
        var postLoginResponse = await _client.SendAsync(formLogin, formLoginValues);
        
        Assert.Equal(HttpStatusCode.Found, postLoginResponse.StatusCode);
        
        const string protectedUri = "/Games";
        
        var getResponse = await _client.GetAsync(protectedUri);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        getResponse.EnsureSuccessStatusCode();

    }
    
    [Fact]
    public async Task AddGame()
    {
        const string loginUrl = "/Identity/Account/Login";

        // this just gets the headers, body can be xxx length and is loaded later
        var getLoginResponse = await _client.GetAsync(loginUrl);

        getLoginResponse.EnsureSuccessStatusCode();
        
        // get the actual content from response
        var getLoginContent = await HtmlHelpers.GetDocumentAsync(getLoginResponse);


        // get the form element from page content
        var formLogin = (IHtmlFormElement) getLoginContent.QuerySelector("#account")!;

        var formLoginValues = new Dictionary<string, string>
        {
            ["Input_Email"] = "admin2@eesti.ee",
            ["Input_Password"] = "Kala.maja2",
        };
        
        // send form with data to server, method (POST) is detected from form element
        var postLoginResponse = await _client.SendAsync(formLogin, formLoginValues);
        
        Assert.Equal(HttpStatusCode.Found, postLoginResponse.StatusCode);
        
        
        const string protectedUri = "/Games/Create";
        
        // this just gets the headers, body can be xxx length and is loaded later
        var getGameResponse = await _client.GetAsync(protectedUri);

        getGameResponse.EnsureSuccessStatusCode();
        
        // get the actual content from response
        var getGameContent = await HtmlHelpers.GetDocumentAsync(getGameResponse);


        // get the form element from page content
        var formGame = (IHtmlFormElement) getGameContent.QuerySelector("#create_game")!;

        var formGameValues = new Dictionary<string, string>
        {
            ["game_name"] = "Game Name",
            ["game_release"] = "2020-10-10",
            ["game_price"] = "0",
            ["game_playtime"] = "0",
            ["game_excitement"] = "0",
            ["game_status"] = "0",
        };
        
        // send form with data to server, method (POST) is detected from form element
        var postGameResponse = await _client.SendAsync(formGame, formGameValues);
        
        Assert.Equal(HttpStatusCode.Found, postGameResponse.StatusCode);
    }
    
    [Fact]
    public async Task SearchGame()
    {
        const string loginUrl = "/Identity/Account/Login";

        // this just gets the headers, body can be xxx length and is loaded later
        var getLoginResponse = await _client.GetAsync(loginUrl);

        getLoginResponse.EnsureSuccessStatusCode();
        
        // get the actual content from response
        var getLoginContent = await HtmlHelpers.GetDocumentAsync(getLoginResponse);


        // get the form element from page content
        var formLogin = (IHtmlFormElement) getLoginContent.QuerySelector("#account")!;

        var formLoginValues = new Dictionary<string, string>
        {
            ["Input_Email"] = "admin2@eesti.ee",
            ["Input_Password"] = "Kala.maja2",
        };
        
        // send form with data to server, method (POST) is detected from form element
        var postLoginResponse = await _client.SendAsync(formLogin, formLoginValues);
        
        Assert.Equal(HttpStatusCode.Found, postLoginResponse.StatusCode);
        
        
        const string protectedUri = "/Games/Create";
        
        // this just gets the headers, body can be xxx length and is loaded later
        var getGameResponse = await _client.GetAsync(protectedUri);

        getGameResponse.EnsureSuccessStatusCode();
        
        // get the actual content from response
        var getGameContent = await HtmlHelpers.GetDocumentAsync(getGameResponse);


        // get the form element from page content
        var formGame = (IHtmlFormElement) getGameContent.QuerySelector("#create_game")!;

        var formGameValues1 = new Dictionary<string, string>
        {
            ["game_name"] = "Game Name1",
            ["game_release"] = "2025-10-10",
            ["game_price"] = "0",
            ["game_playtime"] = "0",
            ["game_excitement"] = "0",
            ["game_status"] = "0",
        };
        
        var formGameValues2 = new Dictionary<string, string>
        {
            ["game_name"] = "Game Name2",
            ["game_release"] = "2020-10-10",
            ["game_price"] = "0",
            ["game_playtime"] = "0",
            ["game_excitement"] = "0",
            ["game_status"] = "0",
        };
        
        // send form with data to server, method (POST) is detected from form element
        var postGameResponse = await _client.SendAsync(formGame, formGameValues1);
        
        Assert.Equal(HttpStatusCode.Found, postGameResponse.StatusCode);
        
        postGameResponse = await _client.SendAsync(formGame, formGameValues2);
        
        Assert.Equal(HttpStatusCode.Found, postGameResponse.StatusCode);


        const string gamesUrl = "/Games?search=2";

        var getGamesResponse = await _client.GetAsync(gamesUrl);
        var getGame = await HtmlHelpers.GetDocumentAsync(getGamesResponse);
        
        var firstGame = getGame.QuerySelector("tbody tr td")!.FirstChild!.TextContent.Trim();
        Assert.Equal("Game Name2", firstGame);
    }
    
    [Fact]
    public async Task Logout()
    {
        const string loginUrl = "/Identity/Account/Login";

        // this just gets the headers, body can be xxx length and is loaded later
        var getLoginResponse = await _client.GetAsync(loginUrl);

        getLoginResponse.EnsureSuccessStatusCode();
        
        // get the actual content from response
        var getLoginContent = await HtmlHelpers.GetDocumentAsync(getLoginResponse);


        // get the form element from page content
        var formLogin = (IHtmlFormElement) getLoginContent.QuerySelector("#account")!;

        var formLoginValues = new Dictionary<string, string>
        {
            ["Input_Email"] = "admin2@eesti.ee",
            ["Input_Password"] = "Kala.maja2",
        };
        
        // send form with data to server, method (POST) is detected from form element
        var postRegisterResponse = await _client.SendAsync(formLogin, formLoginValues);
        
        Assert.Equal(HttpStatusCode.Found, postRegisterResponse.StatusCode);
        
        const string protectedUri = "/Identity/Account/Logout";
        
        var getResponse = await _client.GetAsync(protectedUri);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        getResponse.EnsureSuccessStatusCode();

    }
}