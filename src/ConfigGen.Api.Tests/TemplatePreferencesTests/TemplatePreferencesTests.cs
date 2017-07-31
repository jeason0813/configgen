﻿#region Copyright and License Notice
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
using System.Reflection;
using ConfigGen.Tests.Common;
using ConfigGen.Tests.Common.Extensions;
using ConfigGen.Tests.Common.MSpecShouldExtensions.GenerateResultExtensions;
using ConfigGen.Tests.Common.MSpecShouldExtensions.GenerationError;
using ConfigGen.Utilities.Extensions;
using Machine.Specifications;

namespace ConfigGen.Api.Tests.TemplatePreferencesTests
{
    namespace RazorTemplate
    {
        internal class when_the_XmlPrettyPrint_preference_is_enabled_in_the_template : GenerationServiceTestBase
        {
            Establish context = () =>
            {
                Assembly.GetExecutingAssembly().CopyEmbeddedResourceFileTo("TestResources.SimpleSettings.OneConfiguration.TwoValues.xls", "App.Config.Settings.xls");

                string template =
@"@{Model.Preferences.XmlPrettyPrint = true;}
<xmlRoot>
   <Value attr=""this_is_a_very_long_attribute"">@Model.Settings.Value1 - @Model.Settings.Value2</Value>
</xmlRoot>";
                File.WriteAllText("App.Config.Template.razor", template);

                PreferencesToSupplyToGenerator = new Dictionary<string, string>
                {
                    {PreferenceNames.TemplateFilePath, "App.Config.Template.razor"}
                };

                ExpectedResult = 
@"<xmlRoot>
   <Value
      attr=""this_is_a_very_long_attribute"">Config1-Value1 - Config1-Value2</Value>
</xmlRoot>";
            };

            Because of = () => Result = Subject.Generate(PreferencesToSupplyToGenerator);

            It the_result_indicates_success = () => Result.ShouldIndicateSuccess();

            It the_output_was_pretty_printed = () => Result.Configuration("Configuration1").ShouldContainText(ExpectedResult);
        }

        internal class when_the_XmlPrettyPrint_preference_is_enabled_in_the_template_but_disabled_by_the_caller : GenerationServiceTestBase
        {
            Establish context = () =>
            {
                Assembly.GetExecutingAssembly().CopyEmbeddedResourceFileTo("TestResources.SimpleSettings.OneConfiguration.TwoValues.xls", "App.Config.Settings.xls");

                string template =
@"@{Model.Preferences.XmlPrettyPrint = true;}
<xmlRoot>
   <Value attr=""this_is_a_very_long_attribute"">@Model.Settings.Value1 - @Model.Settings.Value2</Value>
</xmlRoot>";
                File.WriteAllText("App.Config.Template.razor", template);

                ExpectedResult =
@"<xmlRoot>
   <Value attr=""this_is_a_very_long_attribute"">Config1-Value1 - Config1-Value2</Value>
</xmlRoot>";

                PreferencesToSupplyToGenerator = new Dictionary<string, string>
                {
                    {PreferenceNames.TemplateFilePath, "App.Config.Template.razor"},
                    {PreferenceNames.XmlPrettyPrint, false.ToString()}
                };
            };

            Because of = () => Result = Subject.Generate(PreferencesToSupplyToGenerator);

            It the_result_indicates_success = () => Result.ShouldIndicateSuccess();

            It the_output_was_not_pretty_printed = () => Result.Configuration("Configuration1").ShouldContainXml(ExpectedResult);
        }

        internal class when_an_unknown_preference_is_supplied_in_the_template : GenerationServiceTestBase
        {
            Establish context = () =>
            {
                Assembly.GetExecutingAssembly().CopyEmbeddedResourceFileTo("TestResources.SimpleSettings.OneConfiguration.TwoValues.xls", "App.Config.Settings.xls");

                string template =
@"@{Model.Preferences.UnknownPreference = true;}
<xmlRoot>
   <Value attr=""this_is_a_very_long_attribute"">@Model.Settings.Value1 - @Model.Settings.Value2</Value>
</xmlRoot>";
                File.WriteAllText("App.Config.Template.razor", template);

                PreferencesToSupplyToGenerator = new Dictionary<string, string>
                {
                    {PreferenceNames.TemplateFilePath, "App.Config.Template.razor"}
                };
            };

            Because of = () => Result = Subject.Generate(PreferencesToSupplyToGenerator);

            It the_result_indicates_failure = () => Result.ShouldIndicateFailure();

            It the_error_code_indicates_an_unrecognised_preference = () => Result.AllErrors.ShouldContainSingleItemWithCode(ErrorCodes.UnrecognisedPreference);
        }
    }

    namespace XmlTemplate
    {
        internal class when_the_XmlPrettyPrint_preference_is_enabled_in_the_template : GenerationServiceTestBase
        {
            Establish context = () =>
            {
                Assembly.GetExecutingAssembly().CopyEmbeddedResourceFileTo("TestResources.SimpleSettings.OneConfiguration.TwoValues.xls", "App.Config.Settings.xls");

                string template =
@"<xmlRoot xmlns:cg=""http://roblevine.co.uk/Namespaces/ConfigGen/1/0/"">
  <cg:Preferences>
    <XmlPrettyPrint>True</XmlPrettyPrint>
  </cg:Preferences>
   <Value attr=""this_is_a_very_long_attribute"">[%Value1%] - [%Value2%]</Value>
</xmlRoot>";
                File.WriteAllText("App.Config.Template.xml", template);

                ExpectedResult =
@"<xmlRoot>
   <Value
      attr=""this_is_a_very_long_attribute"">Config1-Value1 - Config1-Value2</Value>
</xmlRoot>";
            };

            Because of = () => Result = Subject.Generate(PreferencesToSupplyToGenerator);

            It the_result_indicates_success = () => Result.ShouldIndicateSuccess();

            It the_output_was_pretty_printed = () => Result.Configuration("Configuration1").ShouldContainText(ExpectedResult);
        }

        internal class when_the_XmlPrettyPrint_preference_is_enabled_in_the_template_but_disabled_by_the_caller : GenerationServiceTestBase
        {
            Establish context = () =>
            {
                Assembly.GetExecutingAssembly().CopyEmbeddedResourceFileTo("TestResources.SimpleSettings.OneConfiguration.TwoValues.xls", "App.Config.Settings.xls");

                string template =
@"<xmlRoot xmlns:cg=""http://roblevine.co.uk/Namespaces/ConfigGen/1/0/"">
  <cg:Preferences>
    <XmlPrettyPrint>True</XmlPrettyPrint>
  </cg:Preferences>
   <Value attr=""this_is_a_very_long_attribute"">[%Value1%] - [%Value2%]</Value>
</xmlRoot>";
                File.WriteAllText("App.Config.Template.xml", template);

                ExpectedResult =
@"<xmlRoot>
  <Value attr=""this_is_a_very_long_attribute"">Config1-Value1 - Config1-Value2</Value>
</xmlRoot>";

                PreferencesToSupplyToGenerator = new Dictionary<string, string>
                {
                    {PreferenceNames.XmlPrettyPrint, false.ToString()}
                };
            };

            Because of = () => Result = Subject.Generate(PreferencesToSupplyToGenerator);

            It the_result_indicates_success = () => Result.ShouldIndicateSuccess();

            It the_output_was_not_pretty_printed = () => Result.Configuration("Configuration1").ShouldContainText(ExpectedResult);
        }

        internal class when_an_unknown_preference_is_supplied_in_the_template : GenerationServiceTestBase
        {
            Establish context = () =>
            {
                Assembly.GetExecutingAssembly().CopyEmbeddedResourceFileTo("TestResources.SimpleSettings.OneConfiguration.TwoValues.xls", "App.Config.Settings.xls");

                string template =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<xmlRoot xmlns:cg=""http://roblevine.co.uk/Namespaces/ConfigGen/1/0/"">
  <cg:Preferences>
    <UnknownPreference>true</UnknownPreference>
  </cg:Preferences>
   <Value1 attr=""1"">[%Value1%]</Value1>
   <Value2 attr=""this_is_a_very_long_attribute"">[%Value2%]</Value2>
</xmlRoot>";
                File.WriteAllText("App.Config.Template.xml", template);

                PreferencesToSupplyToGenerator = new Dictionary<string, string>
                {
                    {PreferenceNames.XmlPrettyPrint, false.ToString()}
                };
            };

            Because of = () => Result = Subject.Generate(PreferencesToSupplyToGenerator);

            It the_result_indicates_failure = () => Result.ShouldIndicateFailure();

            It the_error_code_indicates_an_unrecognised_preference = () => Result.AllErrors.ShouldContainSingleItemWithCode(ErrorCodes.UnrecognisedPreference);
        }
    }
}
