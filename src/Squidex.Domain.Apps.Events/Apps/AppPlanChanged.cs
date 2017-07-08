﻿// ==========================================================================
//  AppPlanChanged.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using Squidex.Infrastructure;

namespace Squidex.Domain.Apps.Events.Apps
{
    [TypeName("AppPlanChanged")]
    public sealed class AppPlanChanged : AppEvent
    {
        public string PlanId { get; set; }
    }
}
