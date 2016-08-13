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

using ConfigGen.Utilities.Preferences;

namespace ConfigGen.ConsoleApp.Tests
{
    public class AlternativeConsoleRunnerTestPreferencesGroup : PreferenceGroup<ConsoleRunnerTestPreferences>
    {
        static AlternativeConsoleRunnerTestPreferencesGroup()
        {
            IntParameterPreference = new Preference<ConsoleRunnerTestPreferences, int>(
                name: "IntParameter",
                shortName: "Int",
                description: "specifies the int parameter",
                parameterDescription: new PreferenceParameterDescription("<parameter value>", "the value of the parameter"),
                parseAction: int.Parse,
                setAction: (value, preferences) => preferences.IntParameter = value);

            AnotherBooleanSwitch = new Preference<ConsoleRunnerTestPreferences, bool>(
                name: "AnotherBooleanSwitch",
                shortName: "Another",
                description: "specifies the int parameter",
                parameterDescription: new PreferenceParameterDescription("another switch", "another switch"),
                parseAction: bool.Parse,
                setAction: (value, preferences) => preferences.BooleanSwitch = value);
        }

        public AlternativeConsoleRunnerTestPreferencesGroup() : base(
            name: "AlternativeConsoleRunnerTestPreferencesGroup",
            preferences: new [] { IntParameterPreference, AnotherBooleanSwitch })
        {
        }

        public static IPreference<ConsoleRunnerTestPreferences> AnotherBooleanSwitch { get; set; }

        public static IPreference<ConsoleRunnerTestPreferences> IntParameterPreference { get; set; }
    }
}