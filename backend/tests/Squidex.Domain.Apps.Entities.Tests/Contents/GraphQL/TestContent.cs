﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using NodaTime;
using Squidex.Domain.Apps.Core.Contents;
using Squidex.Domain.Apps.Entities.Schemas;
using Squidex.Infrastructure;
using Squidex.Infrastructure.Json.Objects;

namespace Squidex.Domain.Apps.Entities.Contents.GraphQL;

public static class TestContent
{
    public const string AllFields = @"
            id
            version
            created
            createdBy
            createdByUser {
              id
              email
              displayName
            }
            editToken
            lastModified
            lastModifiedBy
            lastModifiedByUser {
              id
              email
              displayName
            }
            status
            statusColor
            newStatus
            newStatusColor
            url
            data {
              myJson {
                iv
                ivValue: iv(path: ""value"")
              }
              myJson2 {
                iv {
                  __typename
                  rootString,
                  rootInt, 
                  rootFloat, 
                  rootBoolean,
                  rootArray,
                  rootObject {
                    __typename
                    nestedString,
                    nestedInt, 
                    nestedFloat, 
                    nestedBoolean,
                    nestedArray,
                  }
                }
              }
              myString {
                iv
              }
              myStringEnum {
                iv
              }
              myLocalizedString {
                de_DE
              }
              myNumber {
                iv
              }
              myBoolean {
                iv
              }
              myDatetime {
                iv
              }
              myGeolocation {
                iv
              }
              myComponent__Dynamic {
                iv
              }
              myComponent {
                iv {
                  schemaId
                  schemaName
                  componentField
                }
              }
              myComponents__Dynamic {
                iv
              }
              myComponents {
                iv {
                  __typename
                  ... on MyReference1Component {
                    schemaId
                    schemaName
                    reference1Field
                  }
                  ... on MyReference2Component {
                    schemaId
                    schemaName
                    reference2Field
                  }
                  ... on MyComponentComponent {
                    schemaId
                    schemaName
                    componentField
                  }
                }
              }
              myTags {
                iv
              }
              myTagsEnum {
                iv
              }
              myArray {
                iv {
                  nestedNumber
                  nestedBoolean
                }
              }
            }";

    public const string AllFlatFields = @"
            id
            version
            created
            createdBy
            createdByUser {
              id
              email
              displayName
            }
            editToken
            lastModified
            lastModifiedBy
            lastModifiedByUser {
              id
              email
              displayName
            }
            status
            statusColor
            newStatus
            newStatusColor
            url
            flatData {
              myJson
              myJsonValue: myJson(path: 'value')
              myJson2 {
                __typename
                rootString,
                rootInt,
                rootFloat,
                rootBoolean,
                rootArray,
                rootObject {
                  __typename
                  nestedString,
                  nestedInt, 
                  nestedFloat, 
                  nestedBoolean,
                  nestedArray,
                }
              }
              myString
              myStringEnum
              myLocalizedString
              myNumber
              myBoolean
              myDatetime
              myGeolocation
              myComponent__Dynamic
              myComponent {
                schemaId
                schemaName
                componentField
              }
              myComponents__Dynamic
              myComponents {
                __typename
                ... on MyReference1Component {
                  schemaId
                  schemaName
                  reference1Field
                }
                ... on MyReference2Component {
                  schemaId
                  schemaName
                  reference2Field
                }
                ... on MyComponentComponent {
                  schemaId
                  schemaName
                  componentField
                }
              }
              myTags
              myTagsEnum
              myArray {
                nestedNumber
                nestedBoolean
              }
            }";

    public static IEnrichedContentEntity Create(DomainId id, DomainId refId = default, DomainId assetId = default, ContentData? data = null)
    {
        var now = SystemClock.Instance.GetCurrentInstant();

        data ??=
            new ContentData()
                .AddField("my-localized-string",
                    new ContentFieldData()
                        .AddLocalized("de-DE", "de-DE"))
                .AddField("my-string",
                    new ContentFieldData()
                        .AddInvariant(JsonValue.Null))
                .AddField("my-string-enum",
                    new ContentFieldData()
                        .AddInvariant("EnumA"))
                .AddField("my-assets",
                    new ContentFieldData()
                        .AddInvariant(JsonValue.Array(assetId.ToString())))
                .AddField("my-number",
                    new ContentFieldData()
                        .AddInvariant(1.0))
                .AddField("my-boolean",
                    new ContentFieldData()
                        .AddInvariant(true))
                .AddField("my-datetime",
                    new ContentFieldData()
                        .AddInvariant(now))
                .AddField("my-tags",
                    new ContentFieldData()
                        .AddInvariant(JsonValue.Array("tag1", "tag2")))
                .AddField("my-tags-enum",
                    new ContentFieldData()
                        .AddInvariant(JsonValue.Array("EnumA", "EnumB")))
                .AddField("my-references",
                    new ContentFieldData()
                        .AddInvariant(JsonValue.Array(refId.ToString())))
                .AddField("my-union",
                    new ContentFieldData()
                        .AddInvariant(JsonValue.Array(refId.ToString())))
                .AddField("my-geolocation",
                    new ContentFieldData()
                        .AddInvariant(
                            JsonValue.Object()
                                .Add("latitude", 10)
                                .Add("longitude", 20)))
                .AddField("my-component",
                    new ContentFieldData()
                        .AddInvariant(
                            JsonValue.Object()
                                .Add(Component.Discriminator, TestSchemas.Component.Id)
                                .Add(Component.Descriptor, TestSchemas.Component.SchemaDef.Name)
                                .Add("component-field", "Component1")))
                .AddField("my-components",
                    new ContentFieldData()
                        .AddInvariant(
                            JsonValue.Array(
                                JsonValue.Object()
                                    .Add(Component.Discriminator, TestSchemas.Reference1.Id)
                                    .Add(Component.Descriptor, TestSchemas.Reference1.SchemaDef.Name)
                                    .Add("reference1-field", "Component1"),
                                JsonValue.Object()
                                    .Add(Component.Discriminator, TestSchemas.Reference2.Id)
                                    .Add(Component.Descriptor, TestSchemas.Reference2.SchemaDef.Name)
                                    .Add("reference2-field", "Component2"),
                                JsonValue.Object()
                                    .Add(Component.Discriminator, TestSchemas.Component.Id)
                                    .Add(Component.Descriptor, TestSchemas.Component.SchemaDef.Name)
                                    .Add("component-field", "Component3"))))
                .AddField("my-json",
                    new ContentFieldData()
                        .AddInvariant(
                            JsonValue.Object()
                                .Add("value", 1)))
                .AddField("my-json2",
                    new ContentFieldData()
                        .AddInvariant(
                            JsonValue.Object()
                                .Add("rootString", "Root String")
                                .Add("rootInt", 42)
                                .Add("rootFloat", 3.14)
                                .Add("rootBoolean", true)
                                .Add("rootArray",
                                    JsonValue.Array()
                                        .Add("1")
                                        .Add("2")
                                        .Add("3"))
                                .Add("rootObject",
                                    JsonValue.Object()
                                        .Add("nestedString", "Nested String")
                                        .Add("nestedInt", 42)
                                        .Add("nestedFloat", 3.14)
                                        .Add("nestedBoolean", true)
                                        .Add("nestedArray",
                                            JsonValue.Array()
                                                .Add("1")
                                                .Add("2")
                                                .Add("3")))))
                .AddField("my-array",
                    new ContentFieldData()
                        .AddInvariant(JsonValue.Array(
                            JsonValue.Object()
                                .Add("nested-number", 42)
                                .Add("nested-boolean", true),
                            JsonValue.Object()
                                .Add("nested-number", 3.14)
                                .Add("nested-boolean", false))));

        if (assetId != default || refId != default)
        {
            data.AddField("my-embeds",
                new ContentFieldData()
                    .AddInvariant(JsonValue.Create($"assets:{assetId}, contents:{refId}")));
        }

        var content = new ContentEntity
        {
            Id = id,
            AppId = TestApp.DefaultId,
            Version = 1,
            Created = now,
            CreatedBy = RefToken.User("user1"),
            EditToken = $"token_{id}",
            LastModified = now,
            LastModifiedBy = RefToken.Client("client1"),
            Data = data,
            SchemaId = TestSchemas.Default.NamedId(),
            Status = Status.Draft,
            StatusColor = "red",
            NewStatus = Status.Published,
            NewStatusColor = "blue"
        };

        return content;
    }

    public static IEnrichedContentEntity CreateRef(NamedId<DomainId> schemaId, DomainId id, string field, string value)
    {
        var now = SystemClock.Instance.GetCurrentInstant();

        var data =
            new ContentData()
                .AddField(field,
                    new ContentFieldData()
                        .AddInvariant(value));

        var content = new ContentEntity
        {
            Id = id,
            AppId = TestApp.DefaultId,
            Version = 1,
            Created = now,
            CreatedBy = RefToken.User("user1"),
            EditToken = $"token_{id}",
            LastModified = now,
            LastModifiedBy = RefToken.User("user2"),
            Data = data,
            SchemaId = schemaId,
            Status = Status.Draft,
            StatusColor = "red",
            NewStatus = Status.Published,
            NewStatusColor = "blue"
        };

        return content;
    }

    public static object Response(IEnrichedContentEntity content)
    {
        return new
        {
            id = content.Id,
            version = 1,
            created = content.Created,
            createdBy = content.CreatedBy.ToString(),
            createdByUser = new
            {
                id = content.CreatedBy.Identifier,
                email = $"{content.CreatedBy.Identifier}@email.com",
                displayName = $"{content.CreatedBy.Identifier}name"
            },
            editToken = $"token_{content.Id}",
            lastModified = content.LastModified,
            lastModifiedBy = content.LastModifiedBy.ToString(),
            lastModifiedByUser = new
            {
                id = content.LastModifiedBy.Identifier,
                email = $"{content.LastModifiedBy}",
                displayName = content.LastModifiedBy.Identifier
            },
            status = "DRAFT",
            statusColor = "red",
            newStatus = "PUBLISHED",
            newStatusColor = "blue",
            url = $"contents/my-schema/{content.Id}",
            data = Data(content)
        };
    }

    public static object FlatResponse(IEnrichedContentEntity content)
    {
        return new
        {
            id = content.Id,
            version = 1,
            created = content.Created,
            createdBy = content.CreatedBy.ToString(),
            createdByUser = new
            {
                id = content.CreatedBy.Identifier,
                email = $"{content.CreatedBy.Identifier}@email.com",
                displayName = $"{content.CreatedBy.Identifier}name"
            },
            editToken = $"token_{content.Id}",
            lastModified = content.LastModified,
            lastModifiedBy = content.LastModifiedBy.ToString(),
            lastModifiedByUser = new
            {
                id = content.LastModifiedBy.Identifier,
                email = $"{content.LastModifiedBy}",
                displayName = content.LastModifiedBy.Identifier
            },
            status = "DRAFT",
            statusColor = "red",
            newStatus = "PUBLISHED",
            newStatusColor = "blue",
            url = $"contents/my-schema/{content.Id}",
            flatData = FlatData(content)
        };
    }

    public static object Input(IContentEntity content, DomainId refId = default, DomainId assetId = default)
    {
        var actual = new Dictionary<string, object>
        {
            ["myJson"] = new
            {
                iv = new
                {
                    value = 1
                }
            },
            ["myJson2"] = new
            {
                iv = new Dictionary<string, object>
                {
                    ["rootString"] = "Root String",
                    ["rootInt"] = 42,
                    ["rootFloat"] = 3.14,
                    ["rootBoolean"] = true,
                    ["rootArray"] = new[] { "1", "2", "3" },
                    ["rootObject"] = new Dictionary<string, object>
                    {
                        ["nestedString"] = "Nested String",
                        ["nestedInt"] = 42,
                        ["nestedFloat"] = 3.14,
                        ["nestedBoolean"] = true,
                        ["nestedArray"] = new[] { "1", "2", "3" },
                    }
                }
            },
            ["myString"] = new
            {
                iv = (string?)null
            },
            ["myStringEnum"] = new
            {
                iv = "EnumA"
            },
            ["myLocalizedString"] = new
            {
                de_DE = "de-DE"
            },
            ["myNumber"] = new
            {
                iv = 1.0
            },
            ["myBoolean"] = new
            {
                iv = true
            },
            ["myDatetime"] = new
            {
                iv = content.LastModified.ToString()
            },
            ["myGeolocation"] = new
            {
                iv = new
                {
                    latitude = 10,
                    longitude = 20
                }
            },
            ["myComponent"] = new
            {
                iv = new Dictionary<string, object>
                {
                    ["schemaId"] = TestSchemas.Component.Id.ToString(),
                    ["schemaName"] = TestSchemas.Component.SchemaDef.Name,
                    ["componentField"] = "Component1"
                }
            },
            ["myComponents"] = new
            {
                iv = new[]
                {
                    new Dictionary<string, object>
                    {
                        ["schemaId"] = TestSchemas.Reference1.Id.ToString(),
                        ["schemaName"] = TestSchemas.Reference1.SchemaDef.Name,
                        ["reference1Field"] = "Component1"
                    },
                    new Dictionary<string, object>
                    {
                        ["schemaId"] = TestSchemas.Reference2.Id.ToString(),
                        ["schemaName"] = TestSchemas.Reference2.SchemaDef.Name,
                        ["reference2Field"] = "Component2"
                    },
                    new Dictionary<string, object>
                    {
                        ["schemaId"] = TestSchemas.Component.Id.ToString(),
                        ["schemaName"] = TestSchemas.Component.SchemaDef.Name,
                        ["componentField"] = "Component3"
                    }
                }
            },
            ["myTags"] = new
            {
                iv = new[]
                {
                    "tag1",
                    "tag2"
                }
            },
            ["myTagsEnum"] = new
            {
                iv = new[]
                {
                    "EnumA",
                    "EnumB"
                }
            },
            ["myArray"] = new
            {
                iv = new[]
                {
                    new
                    {
                        nestedNumber = 42.0,
                        nestedBoolean = true
                    },
                    new
                    {
                        nestedNumber = 3.14,
                        nestedBoolean = false
                    }
                }
            }
        };

        if (refId != default)
        {
            actual["myReferences"] = new
            {
                iv = new[]
                {
                    refId
                }
            };

            actual["myUnion"] = new
            {
                iv = new[]
                {
                    refId
                }
            };
        }

        if (assetId != default)
        {
            actual["myAssets"] = new
            {
                iv = new[]
                {
                    assetId
                }
            };
        }

        if (assetId != default || refId != default)
        {
            actual["myEmbeds"] = new
            {
                iv = $"assets:{assetId}, contents:{refId}"
            };
        }

        return actual;
    }

    private static object Data(IContentEntity content)
    {
        var actual = new Dictionary<string, object>
        {
            ["myJson"] = new
            {
                iv = new
                {
                    value = 1
                },
                ivValue = 1
            },
            ["myJson2"] = new
            {
                iv = new Dictionary<string, object>
                {
                    ["__typename"] = "JsonObject2",
                    ["rootString"] = "Root String",
                    ["rootInt"] = 42,
                    ["rootFloat"] = 3.14,
                    ["rootBoolean"] = true,
                    ["rootArray"] = new[] { "1", "2", "3" },
                    ["rootObject"] = new Dictionary<string, object>
                    {
                        ["__typename"] = "JsonNested",
                        ["nestedString"] = "Nested String",
                        ["nestedInt"] = 42,
                        ["nestedFloat"] = 3.14,
                        ["nestedBoolean"] = true,
                        ["nestedArray"] = new[] { "1", "2", "3" },
                    }
                }
            },
            ["myString"] = new
            {
                iv = (string?)null
            },
            ["myStringEnum"] = new
            {
                iv = "EnumA"
            },
            ["myLocalizedString"] = new
            {
                de_DE = "de-DE"
            },
            ["myNumber"] = new
            {
                iv = 1.0
            },
            ["myBoolean"] = new
            {
                iv = true
            },
            ["myDatetime"] = new
            {
                iv = content.LastModified.ToString()
            },
            ["myGeolocation"] = new
            {
                iv = new
                {
                    latitude = 10,
                    longitude = 20
                }
            },
            ["myComponent__Dynamic"] = new
            {
                iv = new Dictionary<string, object>
                {
                    ["schemaId"] = TestSchemas.Component.Id.ToString(),
                    ["schemaName"] = TestSchemas.Component.SchemaDef.Name,
                    ["component-field"] = "Component1"
                }
            },
            ["myComponent"] = new
            {
                iv = new Dictionary<string, object>
                {
                    ["schemaId"] = TestSchemas.Component.Id.ToString(),
                    ["schemaName"] = TestSchemas.Component.SchemaDef.Name,
                    ["componentField"] = "Component1"
                }
            },
            ["myComponents__Dynamic"] = new
            {
                iv = new[]
                {
                    new Dictionary<string, object>
                    {
                        ["schemaId"] = TestSchemas.Reference1.Id.ToString(),
                        ["schemaName"] = TestSchemas.Reference1.SchemaDef.Name,
                        ["reference1-field"] = "Component1"
                    },
                    new Dictionary<string, object>
                    {
                        ["schemaId"] = TestSchemas.Reference2.Id.ToString(),
                        ["schemaName"] = TestSchemas.Reference2.SchemaDef.Name,
                        ["reference2-field"] = "Component2"
                    },
                    new Dictionary<string, object>
                    {
                        ["schemaId"] = TestSchemas.Component.Id.ToString(),
                        ["schemaName"] = TestSchemas.Component.SchemaDef.Name,
                        ["component-field"] = "Component3"
                    }
                }
            },
            ["myComponents"] = new
            {
                iv = new object[]
                {
                    new Dictionary<string, object>
                    {
                        ["__typename"] = "MyReference1Component",
                        ["schemaId"] = TestSchemas.Reference1.Id.ToString(),
                        ["schemaName"] = TestSchemas.Reference1.SchemaDef.Name,
                        ["reference1Field"] = "Component1"
                    },
                    new Dictionary<string, object>
                    {
                        ["__typename"] = "MyReference2Component",
                        ["schemaId"] = TestSchemas.Reference2.Id.ToString(),
                        ["schemaName"] = TestSchemas.Reference2.SchemaDef.Name,
                        ["reference2Field"] = "Component2"
                    },
                    new Dictionary<string, object>
                    {
                        ["__typename"] = "MyComponentComponent",
                        ["schemaId"] = TestSchemas.Component.Id.ToString(),
                        ["schemaName"] = TestSchemas.Component.SchemaDef.Name,
                        ["componentField"] = "Component3"
                    }
                }
            },
            ["myTags"] = new
            {
                iv = new[]
                {
                    "tag1",
                    "tag2"
                }
            },
            ["myTagsEnum"] = new
            {
                iv = new[]
                {
                    "EnumA",
                    "EnumB"
                }
            },
            ["myArray"] = new
            {
                iv = new[]
                {
                    new
                    {
                        nestedNumber = 42.0,
                        nestedBoolean = true
                    },
                    new
                    {
                        nestedNumber = 3.14,
                        nestedBoolean = false
                    }
                }
            }
        };

        return actual;
    }

    private static object FlatData(IContentEntity content)
    {
        var actual = new Dictionary<string, object?>
        {
            ["myJson"] = new
            {
                value = 1
            },
            ["myJsonValue"] = 1,
            ["myJson2"] = new Dictionary<string, object>
            {
                ["__typename"] = "JsonObject2",
                ["rootString"] = "Root String",
                ["rootInt"] = 42,
                ["rootFloat"] = 3.14,
                ["rootBoolean"] = true,
                ["rootArray"] = new[] { "1", "2", "3" },
                ["rootObject"] = new Dictionary<string, object>
                {
                    ["__typename"] = "JsonNested",
                    ["nestedString"] = "Nested String",
                    ["nestedInt"] = 42,
                    ["nestedFloat"] = 3.14,
                    ["nestedBoolean"] = true,
                    ["nestedArray"] = new[] { "1", "2", "3" },
                }
            },
            ["myString"] = null,
            ["myStringEnum"] = "EnumA",
            ["myLocalizedString"] = "de-DE",
            ["myNumber"] = 1.0,
            ["myBoolean"] = true,
            ["myDatetime"] = content.LastModified.ToString(),
            ["myGeolocation"] = new
            {
                latitude = 10,
                longitude = 20
            },
            ["myComponent__Dynamic"] = new Dictionary<string, object>
            {
                ["schemaId"] = TestSchemas.Component.Id.ToString(),
                ["schemaName"] = TestSchemas.Component.SchemaDef.Name,
                ["component-field"] = "Component1"
            },
            ["myComponent"] = new Dictionary<string, object>
            {
                ["schemaId"] = TestSchemas.Component.Id.ToString(),
                ["schemaName"] = TestSchemas.Component.SchemaDef.Name,
                ["componentField"] = "Component1"
            },
            ["myComponents__Dynamic"] = new[]
            {
                new Dictionary<string, object>
                {
                    ["schemaId"] = TestSchemas.Reference1.Id.ToString(),
                    ["schemaName"] = TestSchemas.Reference1.SchemaDef.Name,
                    ["reference1-field"] = "Component1"
                },
                new Dictionary<string, object>
                {
                    ["schemaId"] = TestSchemas.Reference2.Id.ToString(),
                    ["schemaName"] = TestSchemas.Reference2.SchemaDef.Name,
                    ["reference2-field"] = "Component2"
                },
                new Dictionary<string, object>
                {
                    ["schemaId"] = TestSchemas.Component.Id.ToString(),
                    ["schemaName"] = TestSchemas.Component.SchemaDef.Name,
                    ["component-field"] = "Component3"
                }
            },
            ["myComponents"] = new object[]
            {
                new Dictionary<string, object>
                {
                    ["__typename"] = "MyReference1Component",
                    ["schemaId"] = TestSchemas.Reference1.Id.ToString(),
                    ["schemaName"] = TestSchemas.Reference1.SchemaDef.Name,
                    ["reference1Field"] = "Component1"
                },
                new Dictionary<string, object>
                {
                    ["__typename"] = "MyReference2Component",
                    ["schemaId"] = TestSchemas.Reference2.Id.ToString(),
                    ["schemaName"] = TestSchemas.Reference2.SchemaDef.Name,
                    ["reference2Field"] = "Component2"
                },
                new Dictionary<string, object>
                {
                    ["__typename"] = "MyComponentComponent",
                    ["schemaId"] = TestSchemas.Component.Id.ToString(),
                    ["schemaName"] = TestSchemas.Component.SchemaDef.Name,
                    ["componentField"] = "Component3"
                }
            },
            ["myTags"] = new[]
            {
                "tag1",
                "tag2"
            },
            ["myTagsEnum"] = new[]
            {
                "EnumA",
                "EnumB"
            },
            ["myArray"] = new[]
            {
                new
                {
                    nestedNumber = 42.0,
                    nestedBoolean = true
                },
                new
                {
                    nestedNumber = 3.14,
                    nestedBoolean = false
                }
            }
        };

        return actual;
    }
}
