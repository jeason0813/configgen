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

using ConfigGen.Settings.Text.Csv;
using ConfigGen.Tests.Common;
using Machine.Specifications;

namespace ConfigGen.Settings.Text.Tests.Csv.CsvSettingsLoaderTests
{
    [Subject(typeof(CsvSettingsLoader))]
    public class the_csv_settings_loader : MachineSpecificationTestBase<CsvSettingsLoader>
    {
        Establish context = () => Subject = new CsvSettingsLoader();

        It has_a_loader_type_of_csv = () => Subject.LoaderType.ShouldEqual("csv");

        It supports_the_file_extension_of_csv = () => Subject.SupportedExtensions.ShouldContainOnly(".csv");
    }

    //TODO: add csv tests
}
