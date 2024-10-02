using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Authorization;

[Collection(nameof(AuthorizationFixture))]
public class AuthorizationTest
{
    private readonly AuthorizationFixture _fixture;
    private const string _unauthorizedUser = "unauthorized";
    private const string _unauthorizedPassword = "123456";

    public AuthorizationTest(AuthorizationFixture fixture)
    {
        _fixture = fixture;
    }

    private HttpMethod ToHttpMethod(string method)
        => method switch
        {
            "GET" => HttpMethod.Get,
            "POST" => HttpMethod.Post,
            "PUT" => HttpMethod.Put,
            "DELETE" => HttpMethod.Delete,
            _ => throw new System.Exception("Invalid Method")
        };

    [Theory(DisplayName = nameof(UnauthenticatedUserTest))]
    [Trait("EndToEnd/Api", "Authentication and Authorization")]
    [InlineData("/api/genres", "POST")]
    [InlineData("/api/genres", "GET")]
    [InlineData("/api/genres/49b9df21-e6b7-4834-93e8-a774c411723d", "GET")]
    [InlineData("/api/genres/49b9df21-e6b7-4834-93e8-a774c411723d", "PUT")]
    [InlineData("/api/genres/49b9df21-e6b7-4834-93e8-a774c411723d", "DELETE")]
    [InlineData("/api/categories", "POST")]
    [InlineData("/api/categories", "GET")]
    [InlineData("/api/categories/49b9df21-e6b7-4834-93e8-a774c411723d", "GET")]
    [InlineData("/api/categories/49b9df21-e6b7-4834-93e8-a774c411723d", "PUT")]
    [InlineData("/api/categories/49b9df21-e6b7-4834-93e8-a774c411723d", "DELETE")]
    [InlineData("/api/cast_members", "POST")]
    [InlineData("/api/cast_members", "GET")]
    [InlineData("/api/cast_members/49b9df21-e6b7-4834-93e8-a774c411723d", "GET")]
    [InlineData("/api/cast_members/49b9df21-e6b7-4834-93e8-a774c411723d", "PUT")]
    [InlineData("/api/cast_members/49b9df21-e6b7-4834-93e8-a774c411723d", "DELETE")]
    [InlineData("/api/videos", "POST")]
    [InlineData("/api/videos", "GET")]
    [InlineData("/api/videos/49b9df21-e6b7-4834-93e8-a774c411723d", "GET")]
    [InlineData("/api/videos/49b9df21-e6b7-4834-93e8-a774c411723d", "PUT")]
    [InlineData("/api/videos/49b9df21-e6b7-4834-93e8-a774c411723d", "DELETE")]
    public async Task UnauthenticatedUserTest(
        string route, string method)
    {
        HttpRequestMessage request = new(ToHttpMethod(method), route);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", "invalid");

        var response = await _fixture.ApiClient.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory(DisplayName = nameof(UnauthorizedUserTest))]
    [Trait("EndToEnd/Api", "Authentication and Authorization")]
    [InlineData("/api/genres", "POST")]
    [InlineData("/api/genres", "GET")]
    [InlineData("/api/genres/49b9df21-e6b7-4834-93e8-a774c411723d", "GET")]
    [InlineData("/api/genres/49b9df21-e6b7-4834-93e8-a774c411723d", "PUT")]
    [InlineData("/api/genres/49b9df21-e6b7-4834-93e8-a774c411723d", "DELETE")]
    [InlineData("/api/categories", "POST")]
    [InlineData("/api/categories", "GET")]
    [InlineData("/api/categories/49b9df21-e6b7-4834-93e8-a774c411723d", "GET")]
    [InlineData("/api/categories/49b9df21-e6b7-4834-93e8-a774c411723d", "PUT")]
    [InlineData("/api/categories/49b9df21-e6b7-4834-93e8-a774c411723d", "DELETE")]
    [InlineData("/api/cast_members", "POST")]
    [InlineData("/api/cast_members", "GET")]
    [InlineData("/api/cast_members/49b9df21-e6b7-4834-93e8-a774c411723d", "GET")]
    [InlineData("/api/cast_members/49b9df21-e6b7-4834-93e8-a774c411723d", "PUT")]
    [InlineData("/api/cast_members/49b9df21-e6b7-4834-93e8-a774c411723d", "DELETE")]
    [InlineData("/api/videos", "POST")]
    [InlineData("/api/videos", "GET")]
    [InlineData("/api/videos/49b9df21-e6b7-4834-93e8-a774c411723d", "GET")]
    [InlineData("/api/videos/49b9df21-e6b7-4834-93e8-a774c411723d", "PUT")]
    [InlineData("/api/videos/49b9df21-e6b7-4834-93e8-a774c411723d", "DELETE")]
    public async Task UnauthorizedUserTest(
        string route, string method)
    {
        HttpRequestMessage request = new(ToHttpMethod(method), route);
        var token = await _fixture.ApiClient.GetAccessTokenAsync(
            _unauthorizedUser, _unauthorizedPassword);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _fixture.ApiClient.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
