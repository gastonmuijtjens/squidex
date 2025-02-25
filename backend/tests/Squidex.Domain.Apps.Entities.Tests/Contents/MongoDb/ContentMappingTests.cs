﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using NodaTime;
using Squidex.Domain.Apps.Core.Contents;
using Squidex.Domain.Apps.Entities.Contents.DomainObject;
using Squidex.Domain.Apps.Entities.MongoDb.Contents;
using Squidex.Domain.Apps.Entities.TestHelpers;
using Squidex.Infrastructure;
using Squidex.Infrastructure.States;

namespace Squidex.Domain.Apps.Entities.Contents.MongoDb;

public class ContentMappingTests : GivenContext
{
    [Fact]
    public async Task Should_map_content_without_new_version_to_draft()
    {
        var source = CreateContentWithoutNewVersion();

        var snapshotJob = new SnapshotWriteJob<ContentDomainObject.State>(source.UniqueId, source, source.Version);
        var snapshot = await MongoContentEntity.CreateCompleteAsync(snapshotJob, AppProvider, default);

        Assert.Equal(source.CurrentVersion.Data, snapshot.Data);
        Assert.Null(snapshot.DraftData);
        Assert.Null(snapshot.NewStatus);
        Assert.NotNull(snapshot.ScheduleJob);
        Assert.True(snapshot.IsSnapshot);

        var mapped = snapshot.ToState();

        mapped.Should().BeEquivalentTo(source);
    }

    [Fact]
    public async Task Should_map_content_without_new_version_to_published()
    {
        var source = CreateContentWithoutNewVersion();

        var snapshotJob = new SnapshotWriteJob<ContentDomainObject.State>(source.UniqueId, source, source.Version);
        var snapshot = await MongoContentEntity.CreatePublishedAsync(snapshotJob, AppProvider, default);

        Assert.Equal(source.CurrentVersion.Data, snapshot.Data);
        Assert.Null(snapshot.DraftData);
        Assert.Null(snapshot.NewStatus);
        Assert.Null(snapshot.ScheduleJob);
        Assert.False(snapshot.IsSnapshot);
    }

    [Fact]
    public async Task Should_map_content_with_new_version_to_draft()
    {
        var source = CreateContentWithNewVersion();

        var snapshotJob = new SnapshotWriteJob<ContentDomainObject.State>(source.UniqueId, source, source.Version);
        var snapshot = await MongoContentEntity.CreateCompleteAsync(snapshotJob, AppProvider, default);

        Assert.Equal(source.NewVersion?.Data, snapshot.Data);
        Assert.Equal(source.CurrentVersion.Data, snapshot.DraftData);
        Assert.NotNull(snapshot.NewStatus);
        Assert.NotNull(snapshot.ScheduleJob);
        Assert.True(snapshot.IsSnapshot);

        var mapped = snapshot.ToState();

        mapped.Should().BeEquivalentTo(source);
    }

    [Fact]
    public async Task Should_map_content_with_new_version_to_published()
    {
        var source = CreateContentWithNewVersion();

        var snapshotJob = new SnapshotWriteJob<ContentDomainObject.State>(source.UniqueId, source, source.Version);
        var snapshot = await MongoContentEntity.CreatePublishedAsync(snapshotJob, AppProvider, default);

        Assert.Equal(source.CurrentVersion?.Data, snapshot.Data);
        Assert.Null(snapshot.DraftData);
        Assert.Null(snapshot.NewStatus);
        Assert.Null(snapshot.ScheduleJob);
        Assert.False(snapshot.IsSnapshot);
    }

    private ContentDomainObject.State CreateContentWithoutNewVersion()
    {
        var data =
            new ContentData()
                .AddField("my-field",
                    new ContentFieldData()
                        .AddInvariant(42));

        var time = SystemClock.Instance.GetCurrentInstant();

        var state = new ContentDomainObject.State
        {
            Id = DomainId.NewGuid(),
            AppId = AppId,
            Created = time,
            CreatedBy = User,
            CurrentVersion = new ContentVersion(Status.Archived, data),
            IsDeleted = true,
            LastModified = time,
            LastModifiedBy = User,
            ScheduleJob = new ScheduleJob(DomainId.NewGuid(), Status.Published, User, time),
            SchemaId = SchemaId,
            Version = 42
        };

        return state;
    }

    private ContentDomainObject.State CreateContentWithNewVersion()
    {
        var data =
            new ContentData()
                .AddField("my-field",
                    new ContentFieldData()
                        .AddInvariant(42));

        var newData =
            new ContentData()
                .AddField("my-field",
                    new ContentFieldData()
                        .AddInvariant(13));

        var time = SystemClock.Instance.GetCurrentInstant();

        var state = new ContentDomainObject.State
        {
            Id = DomainId.NewGuid(),
            AppId = AppId,
            Created = time,
            CreatedBy = User,
            CurrentVersion = new ContentVersion(Status.Archived, data),
            IsDeleted = true,
            LastModified = time,
            LastModifiedBy = User,
            NewVersion = new ContentVersion(Status.Published, newData),
            ScheduleJob = new ScheduleJob(DomainId.NewGuid(), Status.Published, User, time),
            SchemaId = SchemaId,
            Version = 42
        };

        return state;
    }
}
