﻿using FC.Codeflix.Catalog.Api.ApiModels.Video;
using FC.Codeflix.Catalog.Application.UseCases.Video.Common;
using FC.Codeflix.Catalog.Domain.Enum;
using FC.Codeflix.Catalog.EndToEndTests.Api.Genre.Common;
using System.IO;
using System.Text;
using System;
using Xunit;
using FC.Codeflix.Catalog.Domain.Extensions;
using System.Collections.Generic;
using System.Linq;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using System.Threading;
using FC.Codeflix.Catalog.EndToEndTests.Api.CastMember.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Video.Common;

[CollectionDefinition(nameof(VideoBaseFixture))]
public class VideoBaseFixtureCollection
    : ICollectionFixture<VideoBaseFixture>
{ }

public class VideoBaseFixture
    : GenreBaseFixture
{
    public readonly VideoPersistence VideoPersistence;
    public readonly CastMemberPersistence CastMemberPersistence;
    public VideoBaseFixture() :base() {
        VideoPersistence = new VideoPersistence(DbContext);
        CastMemberPersistence = new CastMemberPersistence(DbContext);
    }

    #region Video
    public CreateVideoApiInput GetBasicCreateVideoInput()
    {
        return new CreateVideoApiInput
        {
            Description = GetValidDescription(),
            Title = GetValidTitle(),
            YearLaunched = GetValidYearLaunched(),
            Opened = GetRandomBoolean(),
            Published = GetRandomBoolean(),
            Duration = GetValidDuration(),
            Rating = GetRandomRating().ToStringSignal()
        };
    }

    public Rating GetRandomRating()
    {
        var enumValue = Enum.GetValues<Rating>();
        var random = new Random();
        return enumValue[random.Next(enumValue.Length)];
    }

    public string GetValidTitle()
        => Faker.Lorem.Letter(100);

    public string GetValidDescription()
        => Faker.Commerce.ProductDescription();

    public string GetTooLongDescription()
        => Faker.Lorem.Letter(4001);

    public int GetValidYearLaunched()
        => Faker.Date.BetweenDateOnly(
            new DateOnly(1960, 1, 1),
            new DateOnly(2022, 1, 1)
        ).Year;

    public int GetValidDuration()
        => (new Random()).Next(100, 300);

    public string GetTooLongTitle()
        => Faker.Lorem.Letter(400);

    public string GetValidImagePath()
        => Faker.Image.PlaceImgUrl();

    public string GetValidMediaPath()
    {
        var exampleMedias = new string[]
        {
            "https://www.googlestorage.com/file-example.mp4",
            "https://www.storage.com/another-example-of-video.mp4",
            "https://www.S3.com.br/example.mp4",
            "https://www.glg.io/file.mp4"
        };
        var random = new Random();
        return exampleMedias[random.Next(exampleMedias.Length)];
    }

    public FileInput GetValidImageFileInput()
    {
        var exampleStream = new MemoryStream(Encoding.ASCII.GetBytes("test"));
        var fileInput = new FileInput("jpg", exampleStream, "image/jpeg");
        return fileInput;
    }

    public FileInput GetValidMediaFileInput()
    {
        var exampleStream = new MemoryStream(Encoding.ASCII.GetBytes("test"));
        var fileInput = new FileInput("mp4", exampleStream, "video/mp4");
        return fileInput;
    }

    public DomainEntity.Media GetValidMedia()
        => new(GetValidMediaPath());
    #endregion

    #region CastMember
    public DomainEntity.CastMember GetExampleCastMember()
        => new(GetValidName(), GetRandomCastMemberType());

    public string GetValidName()
        => Faker.Name.FullName();

    public CastMemberType GetRandomCastMemberType()
        => (CastMemberType)(new Random()).Next(1, 2);

    public List<DomainEntity.CastMember> GetExampleCastMembersList(int quantity = 10)
        => Enumerable
            .Range(1, quantity)
            .Select(_ => {
                Thread.Sleep(1);
                return GetExampleCastMember();
            }).ToList();
    #endregion
}