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
using System.Linq;
using ConfigGen.Utilities.Annotations;

namespace ConfigGen.Domain.Contract.Preferences
{
    public class PreferenceGroup<TPreferences> : IPreferenceGroup
    {
        [NotNull]
        private readonly IEnumerable<IPreference<TPreferences>> _preferences;

        public PreferenceGroup([NotNull] string name, [NotNull] IPreference<TPreferences>[] preferences)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (preferences == null) throw new ArgumentNullException(nameof(preferences));

            _preferences = preferences;
            Name = name;
        }

        [NotNull]
        public string Name { get; }

        [NotNull]
        public IEnumerable<IPreference> Preferences => _preferences.ToArray();
    }
}
