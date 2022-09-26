using FluentAssertions;
using Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using WebApi.ApiModels;
using WebApi.Tests.Fixtures;

namespace WebApi.Tests.ControllerApis;


public class AdController : AbstractControllerTest
{    
    private const string CREATE_AD_ROUTE = "/api/ad";
    private const string GET_AD_ROUTE = "/api/ad";
    private const string PUBLISH_AD_ROUTE = "api/ad/publish";
      

    public AdController(CustomWebApplicationFactory<WebMarker> factory) : base(factory)
    {

    }

    #region Create

    [Fact]
    public async Task Create_Ad_With_Default_Value_Should_Return_Created_And_Id()
    {
        //prepare
        var ad = new CreateAdDto()
        {
            Location = "marseille",
            PropertyType = Domain.Enums.PropertyType.House,
            Title = "Jolie maison",
            Price = 10
        };

        //act
        var response = await DefaultUserClient.PostAsJsonAsync(CREATE_AD_ROUTE, ad);

        var actual = await response.Content.ReadFromJsonAsync<int>();
        
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        actual.Should().BeGreaterThan(0);
    }


    [Fact]
    public async Task Create_Ad_With_Null_Value_Should_Return_BadRequest()
    {
        //prepare
        CreateAdDto? ad = null;

        //act
        var response = await DefaultUserClient.PostAsJsonAsync(CREATE_AD_ROUTE, ad);

        var validationProblemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        validationProblemDetails.Should().NotBeNull();
        validationProblemDetails?.Errors.Should().HaveCount(2);

    }

    [Fact]
    public async Task Create_Ad_With_Empty_Value_Should_Return_BadRequest()
    {
        //prepare
        CreateAdDto ad = new CreateAdDto()
        {
            Location = string.Empty,
            Title = string.Empty,
            PropertyType = null
        };

        //act
        var response = await DefaultUserClient.PostAsJsonAsync(CREATE_AD_ROUTE, ad);
        var validationProblemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();


        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        validationProblemDetails.Should().NotBeNull();
        validationProblemDetails?.Errors.Should().HaveCount(4);

    }
    #endregion Create

    #region publish
    [Fact]
    public async Task Publish_Ad_With_Role_User_Should_Return_Forbidden()
    {
        //prepare
        var ad = new CreateAdDto()
        {
            Location = "marseille",
            PropertyType = Domain.Enums.PropertyType.House,
            Title = "Jolie maison",
            Price = 10
        };
        var createResponse = await DefaultUserClient.PostAsJsonAsync(CREATE_AD_ROUTE, ad);
        var adId = await createResponse.Content.ReadFromJsonAsync<int>();

        //act
        var response = await DefaultUserClient.PatchAsync($"{PUBLISH_AD_ROUTE}/{adId}", null);

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

    }

    [Fact]
    public async Task Publish_Ad_With_Default_Value_Should_Return_NoContent()
    {
        //prepare
        var ad = new CreateAdDto()
        {
            Location = "marseille",
            PropertyType = Domain.Enums.PropertyType.House,
            Title = "Jolie maison",
            Price = 10
        };
        var createResponse = await DefaultUserClient.PostAsJsonAsync(CREATE_AD_ROUTE, ad);
        var adId = await createResponse.Content.ReadFromJsonAsync<int>();

        //act
        var response = await AdminClient.PatchAsync($"{PUBLISH_AD_ROUTE}/{adId}", null);

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
    }
    #endregion publish

    #region Get
    [Fact]
    public async Task Get_Published_Ad_Should_Return_Expected_Value()
    {
        //act        
        var actual = await DefaultUserClient.GetFromJsonAsync<GetAdDto>($"{GET_AD_ROUTE}/{ApplicationDbContextSeed.adAppartPublished.Id}");

        //assert
        var expected = ApplicationDbContextSeed.adAppartPublished;
        actual?.Location.Should().Be(expected.Location);
        actual?.PropertyType.Should().Be(expected.PropertyType);
        actual?.Title.Should().Be(expected.Title);
        actual?.Status.Should().Be(expected.Status);
    }

    [Fact]
    public async Task Get_Not_Existing_Ad_Should_Return_Not_Found()
    {
        //prepare        
        int adFakeId = 1000000;

        //act
        var response = await DefaultUserClient.GetAsync($"{GET_AD_ROUTE}/{adFakeId}");

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_Ad_Not_Published_Should_Return_Not_Found()
    {         
        //act
        var response = await DefaultUserClient.GetAsync($"{GET_AD_ROUTE}/{ApplicationDbContextSeed.adAwaitingPublish.Id}");

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    #endregion Get
}
