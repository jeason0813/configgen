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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConfigGen.Tests.Common.Extensions;
using ConfigGen.Tests.Common.MSpecShouldExtensions.Error;
using Machine.Specifications;

namespace ConfigGen.Settings.Excel.Tests.ExcelSettingsLoaderTests
{
    public class the_xml_settings_loader : ExcelSettingsLoaderTestBase
    {
        It has_a_loader_type_of_excel = () => Subject.LoaderType.ShouldEqual("excel");

        It supports_the_file_extensions_of_xls_and_xlsx = () => Subject.SupportedExtensions.ShouldContainOnly(".xls", ".xlsx");
    }

    public class when_loading_a_simple_xlsx_file_containing_two_configurations : ExcelSettingsLoaderTestBase
    {
        Establish context = () =>
        {
            SourceTestFileName = "App.Config.Settings.xlsx";
        };

        Because of = () => Result = Subject.LoadSettings(SettingsFileFullPath);

        It the_result_is_not_null = () => Result.Value.ShouldNotBeNull();

        It the_result_indicates_success = () => Result.Error.ShouldBeNull();

        It the_result_should_contain_two_configurations = () => Result.Value.Count().ShouldEqual(2);

        It the_result_should_contain_a_configuration_named_Configuration1 =
            () => Result.Value.Get("Configuration1").ShouldNotBeNull();

        It Configuration1_should_contain_four_settings =
            () => Result.Value.Get("Configuration1").Count.ShouldEqual(4);

        It Configuration1_should_contain_the_correct_settings_and_values =
            () => Result.Value.Get("Configuration1").ShouldContainOnly(
                new KeyValuePair<string, object>("MachineName", "Configuration1"),
                new KeyValuePair<string, object>("ConfigFilePath", "Configuration1\\App.Config"),
                new KeyValuePair<string, object>("Setting1", "Configuration1_Setting1"),
                new KeyValuePair<string, object>("Setting2", "Configuration1_Setting2"));

        It the_result_should_contain_a_configuration_named_Configuration2 =
            () => Result.Value.Get("Configuration2").ShouldNotBeNull();

        It Configuration2_should_contain_four_settings =
            () => Result.Value.Get("Configuration2").Count.ShouldEqual(4);

        It Configuration2_should_contain_the_correct_settings_and_values =
            () => Result.Value.Get("Configuration2").ShouldContainOnly(
                new KeyValuePair<string, object>("MachineName", "Configuration2"),
                new KeyValuePair<string, object>("ConfigFilePath", "Configuration2\\App.Config"),
                new KeyValuePair<string, object>("Setting1", "Configuration2_Setting1"),
                new KeyValuePair<string, object>("Setting2", "Configuration2_Setting2"));
    }

    public class when_loading_a_simple_xls_file_containing_two_configurations : ExcelSettingsLoaderTestBase
    {
        Establish context = () =>
        {
            SourceTestFileName = "App.Config.Settings.xls";
            TargetTestFileName = "App.Config.Settings.xls";
        };

        Because of = () => Result = Subject.LoadSettings(SettingsFileFullPath);

        It the_result_is_not_null = () => Result.Value.ShouldNotBeNull();

        It the_result_indicates_success = () => Result.Error.ShouldBeNull();

        It the_result_should_contain_two_configurations = () => Result.Value.Count().ShouldEqual(2);

        It the_result_should_contain_a_configuration_named_Configuration1 =
            () => Result.Value.Get("Configuration1").ShouldNotBeNull();

        It Configuration1_should_contain_four_settings =
            () => Result.Value.Get("Configuration1").Count.ShouldEqual(4);

        It Configuration1_should_contain_the_correct_settings_and_values =
            () => Result.Value.Get("Configuration1").ShouldContainOnly(
                new KeyValuePair<string, object>("MachineName", "Configuration1"),
                new KeyValuePair<string, object>("ConfigFilePath", "Configuration1\\App.Config"),
                new KeyValuePair<string, object>("Setting1", "Configuration1_Setting1"),
                new KeyValuePair<string, object>("Setting2", "Configuration1_Setting2"));

        It the_result_should_contain_a_configuration_named_Configuration2 =
            () => Result.Value.Get("Configuration2").ShouldNotBeNull();

        It Configuration2_should_contain_four_settings =
            () => Result.Value.Get("Configuration2").Count.ShouldEqual(4);

        It Configuration2_should_contain_the_correct_settings_and_values =
            () => Result.Value.Get("Configuration2").ShouldContainOnly(
                new KeyValuePair<string, object>("MachineName", "Configuration2"),
                new KeyValuePair<string, object>("ConfigFilePath", "Configuration2\\App.Config"),
                new KeyValuePair<string, object>("Setting1", "Configuration2_Setting1"),
                new KeyValuePair<string, object>("Setting2", "Configuration2_Setting2"));
    }

    public class when_loading_a_simple_xls_file_that_is_locked_for_writing : ExcelSettingsLoaderTestBase
    {
        private static Exception CaughtException;

        Establish context = () =>
        {
            SourceTestFileName = "App.Config.Settings.xls";
            TargetTestFileName = "App.Config.Settings.xls";
        };

        Because of = () =>
        {
            using (var fileStream = new FileStream(SettingsFileFullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            {
                CaughtException = Catch.Exception(() => Result = Subject.LoadSettings(SettingsFileFullPath));
            }
        };

        It the_settings_file_can_still_be_loaded_without_an_exception = () => CaughtException.ShouldBeNull();

        It the_result_indicates_success = () => Result.Error.ShouldBeNull();

        It the_result_should_contain_two_configurations = () => Result.Value.Count().ShouldEqual(2);
    }

    public class when_loading_a_simple_xls_file_that_has_its_readonly_file_attribute_set : ExcelSettingsLoaderTestBase
    {
        private static Exception CaughtException;

        Establish context = () =>
        {
            SourceTestFileName = "App.Config.Settings.xls";
            TargetTestFileName = "App.Config.Settings.xls";
        };

        Because of = () =>
        {
            var settingsFile = new FileInfo(SettingsFileFullPath);
            settingsFile.Attributes |= FileAttributes.ReadOnly;

            CaughtException = Catch.Exception(() => Result = Subject.LoadSettings(SettingsFileFullPath));
        };

        It the_settings_file_can_still_be_loaded_without_an_exception = () => CaughtException.ShouldBeNull();

        It the_result_indicates_success = () => Result.Error.ShouldBeNull();

        It the_result_should_contain_two_configurations = () => Result.Value.Count().ShouldEqual(2);
    }

    public class when_loading_a_spreadsheet_which_does_not_exist_at_the_specified_path : ExcelSettingsLoaderTestBase
    {
        private static Exception CaughtException;

        Establish context = () =>
        {
            SourceTestFileName = "FileDoesNotExist.xls";
            TargetTestFileName = "App.Config.Settings.xls";
        };

        Because of = () => CaughtException = Catch.Exception(() => Result = Subject.LoadSettings(SourceTestFileName));

        It no_exception_is_thrown = () => CaughtException.ShouldBeNull();

        It the_result_indicates_a_file_not_found_error = () => Result.Error.ShouldContainSingleErrorWithCode(ExcelSettingsLoadErrorCodes.FileNotFound);
    }
}