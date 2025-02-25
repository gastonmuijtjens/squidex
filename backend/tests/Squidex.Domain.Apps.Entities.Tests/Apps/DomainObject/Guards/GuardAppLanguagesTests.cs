﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Squidex.Domain.Apps.Core.TestHelpers;
using Squidex.Domain.Apps.Entities.Apps.Commands;
using Squidex.Domain.Apps.Entities.TestHelpers;
using Squidex.Infrastructure;
using Squidex.Infrastructure.Validation;

namespace Squidex.Domain.Apps.Entities.Apps.DomainObject.Guards;

public class GuardAppLanguagesTests : GivenContext, IClassFixture<TranslationsFixture>
{
    [Fact]
    public void CanAddLanguage_should_throw_exception_if_language_is_null()
    {
        var command = new AddLanguage();

        ValidationAssert.Throws(() => GuardAppLanguages.CanAdd(command, App),
            new ValidationError("Language code is required.", "Language"));
    }

    [Fact]
    public void CanAddLanguage_should_throw_exception_if_language_already_added()
    {
        var command = new AddLanguage { Language = Language.EN };

        ValidationAssert.Throws(() => GuardAppLanguages.CanAdd(command, App),
            new ValidationError("Language has already been added."));
    }

    [Fact]
    public void CanAddLanguage_should_not_throw_exception_if_language_valid()
    {
        var command = new AddLanguage { Language = Language.IT };

        GuardAppLanguages.CanAdd(command, App);
    }

    [Fact]
    public void CanRemoveLanguage_should_throw_exception_if_language_is_null()
    {
        var command = new RemoveLanguage();

        ValidationAssert.Throws(() => GuardAppLanguages.CanRemove(command, App),
            new ValidationError("Language code is required.", "Language"));
    }

    [Fact]
    public void CanRemoveLanguage_should_throw_exception_if_language_not_found()
    {
        var command = new RemoveLanguage { Language = Language.IT };

        Assert.Throws<DomainObjectNotFoundException>(() => GuardAppLanguages.CanRemove(command, App));
    }

    [Fact]
    public void CanRemoveLanguage_should_throw_exception_if_language_is_master()
    {
        var command = new RemoveLanguage { Language = Language.EN };

        ValidationAssert.Throws(() => GuardAppLanguages.CanRemove(command, App),
            new ValidationError("Master language cannot be removed."));
    }

    [Fact]
    public void CanRemoveLanguage_should_not_throw_exception_if_language_is_valid()
    {
        var command = new RemoveLanguage { Language = Language.DE };

        GuardAppLanguages.CanRemove(command, App);
    }

    [Fact]
    public void CanUpdateLanguage_should_throw_exception_if_language_is_null()
    {
        var command = new UpdateLanguage();

        ValidationAssert.Throws(() => GuardAppLanguages.CanUpdate(command, App),
            new ValidationError("Language code is required.", "Language"));
    }

    [Fact]
    public void CanUpdateLanguage_should_throw_exception_if_language_is_optional_and_master()
    {
        var command = new UpdateLanguage { Language = Language.EN, IsOptional = true };

        ValidationAssert.Throws(() => GuardAppLanguages.CanUpdate(command, App),
            new ValidationError("Master language cannot be made optional.", "IsMaster"));
    }

    [Fact]
    public void CanUpdateLanguage_should_throw_exception_if_fallback_language_defined_and_master()
    {
        var command = new UpdateLanguage { Language = Language.EN, Fallback = new[] { Language.DE } };

        ValidationAssert.Throws(() => GuardAppLanguages.CanUpdate(command, App),
            new ValidationError("Master language cannot have fallback languages.", "Fallback"));
    }

    [Fact]
    public void CanUpdateLanguage_should_throw_exception_if_language_has_invalid_fallback()
    {
        var command = new UpdateLanguage { Language = Language.DE, Fallback = new[] { Language.IT } };

        ValidationAssert.Throws(() => GuardAppLanguages.CanUpdate(command, App),
            new ValidationError("App does not have fallback language 'Italian'.", "Fallback"));
    }

    [Fact]
    public void CanUpdateLanguage_should_throw_exception_if_not_found()
    {
        var command = new UpdateLanguage { Language = Language.IT };

        Assert.Throws<DomainObjectNotFoundException>(() => GuardAppLanguages.CanUpdate(command, App));
    }

    [Fact]
    public void CanUpdateLanguage_should_not_throw_exception_if_language_is_valid()
    {
        var command = new UpdateLanguage { Language = Language.DE, Fallback = new[] { Language.EN } };

        GuardAppLanguages.CanUpdate(command, App);
    }
}
