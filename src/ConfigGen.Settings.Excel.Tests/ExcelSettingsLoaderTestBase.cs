﻿#region Copyright and License Notice
// Copyright (C)2010-2016 - INEX Solutions Ltd
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
using System.Reflection;
using ConfigGen.Domain.Contract;
using ConfigGen.Tests.Common;
using ConfigGen.Utilities.Extensions;
using Machine.Specifications;

namespace ConfigGen.Settings.Excel.Tests
{
    public abstract class ExcelSettingsLoaderTestBase : MachineSpecificationTestBase<ExcelSettingsLoader, IEnumerable<IConfiguration>>
    {
        private static Lazy<string> lazySettingsFileFullPath;
        private static IDisposable disposableFile;

        protected static string SourceTestFileName;
        protected static string TargetTestFileName;

        Establish context = () =>
        {
            Subject = new ExcelSettingsLoader(new SpreadsheetHeaderProcessor(), new SpreadsheetDataProcessor(new CellDataParser()), new ExcelFileLoader());
            SourceTestFileName = null;
            lazySettingsFileFullPath = new Lazy<string>(WriteOutTestFile);
            TargetTestFileName = "App.Config.Settings.xlsx";
            disposableFile = null;
        };

        Cleanup cleanup = () =>
        {
            disposableFile?.Dispose();
        };

        private static string WriteOutTestFile()
        {
            var testFile = Assembly.GetExecutingAssembly().GetEmbeddedResourceFile("SampleFiles." + SourceTestFileName, TargetTestFileName);
            disposableFile = testFile;
            return testFile.FullName;
        }

        protected static string SettingsFileFullPath => lazySettingsFileFullPath.Value;
    }
}