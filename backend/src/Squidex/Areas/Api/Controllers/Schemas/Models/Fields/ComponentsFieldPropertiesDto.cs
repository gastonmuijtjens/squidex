﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Squidex.Domain.Apps.Core.Schemas;
using Squidex.Infrastructure;
using Squidex.Infrastructure.Collections;
using Squidex.Infrastructure.Reflection;
using Squidex.Web;

namespace Squidex.Areas.Api.Controllers.Schemas.Models.Fields;

[OpenApiRequest]
public sealed class ComponentsFieldPropertiesDto : FieldPropertiesDto
{
    /// <summary>
    /// The minimum allowed items for the field value.
    /// </summary>
    public int? MinItems { get; set; }

    /// <summary>
    /// The maximum allowed items for the field value.
    /// </summary>
    public int? MaxItems { get; set; }

    /// <summary>
    /// The ID of the embedded schemas.
    /// </summary>
    public ReadonlyList<DomainId>? SchemaIds { get; set; }

    /// <summary>
    /// The fields that must be unique.
    /// </summary>
    public ReadonlyList<string>? UniqueFields { get; set; }

    public static ComponentsFieldPropertiesDto FromDomain(ComponentsFieldProperties fieldProperties)
    {
        return SimpleMapper.Map(fieldProperties, new ComponentsFieldPropertiesDto());
    }

    public override FieldProperties ToProperties()
    {
        return SimpleMapper.Map(this, new ComponentsFieldProperties());
    }
}
