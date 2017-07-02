#region Copyright and License Notice
// Copyright (C)2010-2017 - INEX Solutions Ltd
// https://github.com/inex-solutions/configgen
// 
// This file is part of ConfigGen.
// 
// ConfigGen is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// ConfigGen is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License and 
// the GNU Lesser General Public License along with ConfigGen.  
// If not, see <http://www.gnu.org/licenses/>
#endregion

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ConfigGen.Api.Contract;
using ConfigGen.Tests.Common;
using ConfigGen.Tests.Common.Extensions;
using ConfigGen.Tests.Common.MSpecShouldExtensions.GenerationError;
using ConfigGen.Utilities.Extensions;
using Machine.Specifications;

namespace ConfigGen.Api.Tests.WarningAndErrorTests
{
    [Subject(typeof(GenerationService))]
    internal class when_invoked_such_that_generation_is_successfull_and_no_errors_occur : GenerationServiceTestBase
    {
        Establish context = () =>
        {
            Assembly.GetExecutingAssembly().CopyEmbeddedResourceFileTo("TestResources.SimpleSettings.OneConfiguration.TwoValues.xls", "App.Config.Settings.xls");

            string template =
@"<xmlRoot>
  <Value1>[%Value1%]</Value1>
  <Value2>[%Value2%]</Value2>
</xmlRoot>";
            File.WriteAllText("App.Config.Template.xml", template);
        };

        Because of = () => Result = Subject.Generate(PreferencesToSupplyToGenerator);

        It one_file_is_generated = () => Result.GeneratedFiles.Count().ShouldEqual(1);

        It no_overall_generation_issues_are_reported = () => Result.GenerationIssues.ShouldBeEmpty();

        It no_individual_file_generation_issues_are_reported = () => Result.GeneratedFiles.SelectMany(f => f.GenerationIssues).ShouldBeEmpty();
    }

    [Subject(typeof(GenerationService))]
    internal class when_invoked_with_a_missing_template_file : GenerationServiceTestBase
    {
        Establish context = () =>
        {
            Assembly.GetExecutingAssembly().CopyEmbeddedResourceFileTo("TestResources.SimpleSettings.OneConfiguration.TwoValues.xls", "App.Config.Settings.xls");

            // missing template file will cause the overall process to fail
        };

        Because of = () => Result = Subject.Generate(PreferencesToSupplyToGenerator);

        It no_files_are_generated = () => Result.GeneratedFiles.ShouldBeEmpty();

        It one_overall_generation_issue_is_reported = () => Result.GenerationIssues.Count().ShouldEqual(1);

        It the_single_issue_reported_indicates_the_template_file_was_not_found =
            () => Result.GenerationIssues.ShouldContainAnItemWithCode(ErrorCodes.TemplateFileNotFound);

        It no_individual_file_generation_issues_are_reported = () => Result.GeneratedFiles.SelectMany(f => f.GenerationIssues).ShouldBeEmpty();
    }

    [Subject(typeof(GenerationService))]
    internal class when_invoked_with_a_missing_settings_file : GenerationServiceTestBase
    {
        Establish context = () =>
        {
            // missing settings file will cause the overall process to fail

            string template =
@"<xmlRoot>
  <Value1>[%Value1%]</Value1>
  <Value2>[%Value2%]</Value2>
</xmlRoot>";
            File.WriteAllText("App.Config.Template.xml", template);
        };

        Because of = () => Result = Subject.Generate(PreferencesToSupplyToGenerator);

        It no_files_are_generated = () => Result.GeneratedFiles.ShouldBeEmpty();

        It one_overall_generation_issue_is_reported = () => Result.GenerationIssues.Count().ShouldEqual(1);

        It the_single_generation_issue_indicates_the_template_file_was_not_found =
            () => Result.GenerationIssues.ShouldContainAnItemWithCode(ErrorCodes.SettingsFileNotFound);

        It no_individual_file_generation_issues_are_reported = () => Result.GeneratedFiles.SelectMany(f => f.GenerationIssues).ShouldBeEmpty();
    }

    [Subject(typeof(GenerationService))]
    internal class when_invoked_such_a_single_configuration_generation_causes_an_error_while_others_succeed : GenerationServiceTestBase
    {
        Establish context = () =>
        {
            Assembly.GetExecutingAssembly().CopyEmbeddedResourceFileTo("TestResources.SimpleSettings.TwoConfigurations.TwoValues.xls", "App.Config.Settings.xls");

            // this razor template will explode for Configuration2
            string template =
@"<root>

@Model.Settings.Value1
@Model.Settings.Value2

@if (Model.Settings.MachineName == ""Configuration2"")
{
    throw new InvalidOperationException();
}
</root>";
            File.WriteAllText("App.Config.Template.razor", template);

            PreferencesToSupplyToGenerator = new Dictionary<string, string>
            {
                {PreferenceNames.TemplateFilePath, "App.Config.Template.razor"}
            };
        };

        Because of = () => Result = Subject.Generate(PreferencesToSupplyToGenerator);

        It no_overall_generation_issues_are_reported = () => Result.GenerationIssues.ShouldBeEmpty();

        It no_individual_file_generation_issues_are_reported_for_the_generation_that_succeeded =
            () => Result.Configuration("Configuration1").GenerationIssues.ShouldBeEmpty();

        It a_single_file_generation_issue_is_reported_for_the_failed_generation =
            () => Result.Configuration("Configuration2").GenerationIssues.ShouldContainSingleItemWithCode("GeneralRazorTemplateError");
    }

    [Subject(typeof(GenerationService))]
    internal class when_invoked_such_that_a_single_configuration_generation_reports_unused_tokens : GenerationServiceTestBase
    {
        Establish context = () =>
        {
            Assembly.GetExecutingAssembly().CopyEmbeddedResourceFileTo("TestResources.SimpleSettings.TwoConfigurations.TwoValues.xls", "App.Config.Settings.xls");

            // this razor template will not use TokenTwo for Configuration2
            string template =
@"<root>
@if (Model.Settings.MachineName == ""Configuration1"")
{
    @Model.Settings.Value2
}
@Model.Settings.Value1

</root>";
            File.WriteAllText("App.Config.Template.razor", template);

            PreferencesToSupplyToGenerator = new Dictionary<string, string>
            {
                {PreferenceNames.TemplateFilePath, "App.Config.Template.razor"}
            };
        };

        Because of = () => Result = Subject.Generate(PreferencesToSupplyToGenerator);

        It no_overall_generation_issues_are_reported = () => Result.GenerationIssues.ShouldBeEmpty();

        It no_individual_file_generation_issues_are_reported = () => Result.GeneratedFiles.SelectMany(f => f.GenerationIssues).ShouldBeEmpty();

        It no_individual_file_generation_issues_are_reported_for_the_generation_that_succeeded =
            () => Result.Configuration("Configuration1").GenerationIssues.ShouldBeEmpty();

        It a_single_file_generation_issue_is_reported_for_the_failed_generation_that_did_not_use_all_tokens =
            () => Result.Configuration("Configuration2").GenerationIssues.ShouldContainSingleItemWithCode(GenerationServiceErrorCodes.UnusedTokenErrorCode);
    }

    [Subject(typeof(GenerationService))]
    internal class when_invoked_so_that_a_single_configuration_generation_reports_unrecognised_tokens : GenerationServiceTestBase
    {
        Establish context = () =>
        {
            Assembly.GetExecutingAssembly().CopyEmbeddedResourceFileTo("TestResources.SimpleSettings.TwoConfigurations.TwoValues.xls", "App.Config.Settings.xls");

            // this razor template will use an unrecognised token for Configuration2
            string template =
@"<root>
@if (Model.Settings.MachineName == ""Configuration2"")
{
    @Model.Settings.AnUnrecognisedToken
}
@Model.Settings.Value1
@Model.Settings.Value2
</root>";
            File.WriteAllText("App.Config.Template.razor", template);

            PreferencesToSupplyToGenerator = new Dictionary<string, string>
            {
                {PreferenceNames.TemplateFilePath, "App.Config.Template.razor"}
            };
        };

        Because of = () => Result = Subject.Generate(PreferencesToSupplyToGenerator);

        It no_overall_generation_issues_are_reported = () => Result.GenerationIssues.ShouldBeEmpty();

        It no_individual_file_generation_issues_are_reported = () => Result.GeneratedFiles.SelectMany(f => f.GenerationIssues).ShouldBeEmpty();

        It no_individual_file_generation_issues_are_reported_for_the_generation_that_succeeded =
            () => Result.Configuration("Configuration1").GenerationIssues.ShouldBeEmpty();

        It a_single_file_generation_issue_is_reported_for_the_failed_generation_that_did_not_use_all_tokens =
            () => Result.Configuration("Configuration2").GenerationIssues.ShouldContainSingleItemWithCode(GenerationServiceErrorCodes.UnrecognisedToken);
    }
}
