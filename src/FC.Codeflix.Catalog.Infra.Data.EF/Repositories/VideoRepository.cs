﻿using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.Domain.SeedWork;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.Infra.Data.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Infra.Data.EF.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly CodeflixCatalogDbContext _context;
    private DbSet<Video> _videos => _context.Set<Video>();
    private DbSet<VideosCategories> _videosCategories 
        => _context.Set<VideosCategories>();
    private DbSet<VideosGenres> _videosGenres
        => _context.Set<VideosGenres>();
    private DbSet<VideosCastMembers> _videosCastMembers
            => _context.Set<VideosCastMembers>();


    public VideoRepository(CodeflixCatalogDbContext context)
        => _context = context;


    public async Task Insert(Video video, CancellationToken cancellationToken)
    {
        await _videos.AddAsync(video, cancellationToken);

        if(video.Categories.Count > 0)
        {
            var relations = video.Categories
                .Select(categoryId => new VideosCategories(
                    categoryId,
                    video.Id
                ));
            await _videosCategories.AddRangeAsync(relations);
        }
        if(video.Genres.Count > 0)
        {
            var relations = video.Genres
                .Select(genreId => new VideosGenres(
                    genreId,
                    video.Id
                ));
            await _videosGenres.AddRangeAsync(relations);
        }
        if(video.CastMembers.Count > 0)
        {
            var relations = video.CastMembers
                .Select(castMemberId => new VideosCastMembers(
                    castMemberId,
                    video.Id
                ));
            await _videosCastMembers.AddRangeAsync(relations);
        }
    }

    public Task Update(Video aggregate, CancellationToken cancellationToken)
    {
        _videos.Update(aggregate);
        return Task.CompletedTask;
    }

    public Task Delete(Video aggregate, CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task<Video> Get(Guid id, CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task<SearchOutput<Video>> Search(SearchInput input, CancellationToken cancellationToken) => throw new NotImplementedException();
}