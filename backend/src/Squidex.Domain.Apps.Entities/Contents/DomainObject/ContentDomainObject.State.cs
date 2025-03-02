﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Text.Json.Serialization;
using Squidex.Domain.Apps.Core.Contents;
using Squidex.Domain.Apps.Events.Contents;
using Squidex.Infrastructure;
using Squidex.Infrastructure.EventSourcing;
using Squidex.Infrastructure.Reflection;

namespace Squidex.Domain.Apps.Entities.Contents.DomainObject;

public partial class ContentDomainObject
{
    public sealed class State : DomainObjectState<State>, IContentEntity
    {
        public NamedId<DomainId> AppId { get; set; }

        public NamedId<DomainId> SchemaId { get; set; }

        public ContentVersion? NewVersion { get; set; }

        public ContentVersion CurrentVersion { get; set; }

        public ScheduleJob? ScheduleJob { get; set; }

        public bool IsDeleted { get; set; }

        [JsonIgnore]
        public DomainId UniqueId
        {
            get => DomainId.Combine(AppId, Id);
        }

        [JsonIgnore]
        public ContentData Data
        {
            get => NewVersion?.Data ?? CurrentData;
        }

        [JsonIgnore]
        public ContentData CurrentData
        {
            get => CurrentVersion.Data;
        }

        [JsonIgnore]
        public Status? NewStatus
        {
            get => NewVersion?.Status;
        }

        [JsonIgnore]
        public Status Status
        {
            get => CurrentVersion?.Status ?? default;
        }

        public override bool ApplyEvent(IEvent @event, EnvelopeHeaders headers)
        {
            switch (@event)
            {
                case ContentCreated e:
                    {
                        Id = e.ContentId;

                        SimpleMapper.Map(e, this);

                        CurrentVersion = new ContentVersion(e.Status, e.Data);

                        break;
                    }

                case ContentDraftCreated e:
                    {
                        var newData = e.MigratedData?.UseSameFields(CurrentData) ?? CurrentData;

                        NewVersion = new ContentVersion(e.Status, newData);

                        // Implictely cancels any pending update jobs.
                        ScheduleJob = null;
                        break;
                    }

                case ContentDraftDeleted:
                    {
                        NewVersion = null;

                        // Implictely cancels any pending update jobs.
                        ScheduleJob = null;
                        break;
                    }

                case ContentStatusChanged e:
                    {
                        ScheduleJob = null;

                        if (NewVersion != null)
                        {
                            if (e.Status == Status.Published)
                            {
                                CurrentVersion = new ContentVersion(e.Status, NewVersion.Data.UseSameFields(CurrentData));

                                NewVersion = null;
                            }
                            else
                            {
                                NewVersion = NewVersion.WithStatus(e.Status);
                            }
                        }
                        else
                        {
                            CurrentVersion = CurrentVersion.WithStatus(e.Status);
                        }

                        break;
                    }

                case ContentSchedulingCancelled:
                    {
                        ScheduleJob = null;
                        break;
                    }

                case ContentStatusScheduled e:
                    {
                        ScheduleJob = ScheduleJob.Build(e.Status, e.Actor, e.DueTime);
                        break;
                    }

                case ContentUpdated e:
                    {
                        if (NewVersion != null)
                        {
                            NewVersion = NewVersion.WithData(e.Data.UseSameFields(Data));
                        }
                        else
                        {
                            CurrentVersion = CurrentVersion.WithData(e.Data.UseSameFields(CurrentData));
                        }

                        break;
                    }

                case ContentDeleted:
                    {
                        IsDeleted = true;
                        break;
                    }
            }

            return true;
        }
    }
}
