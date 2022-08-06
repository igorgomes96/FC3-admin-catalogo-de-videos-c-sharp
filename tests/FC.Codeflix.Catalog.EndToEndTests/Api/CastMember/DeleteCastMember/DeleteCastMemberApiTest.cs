﻿using System.Net;
using System.Threading.Tasks;
using FC.Codeflix.Catalog.Api.ApiModels.Response;
using FC.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using FC.Codeflix.Catalog.EndToEndTests.Api.CastMember.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.CastMember.DeleteCastMember;

[Collection(nameof(CastMemberApiBaseFixture))]
public class DeleteCastMemberApiTest
{
    private readonly CastMemberApiBaseFixture _fixture;

    public DeleteCastMemberApiTest(CastMemberApiBaseFixture fixture) 
        => _fixture = fixture;

    [Fact(DisplayName = nameof(Delete))]
    [Trait("EndToEnd/API", "CatMembers/Delete - EndPoints")]
    public async Task Delete()
    {
        var examples = _fixture.GetExampleCastMembersList(5);
        var example = examples[2];
        await _fixture.Persistence.InsertList(examples);

        var (response, output) =
            await _fixture.ApiClient.Delete<object>(
                $"castmembers/{example.Id.ToString()}"
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
        var castMemberExample = await _fixture.Persistence.GetById(example.Id);
        castMemberExample.Should().BeNull();
    }
}
