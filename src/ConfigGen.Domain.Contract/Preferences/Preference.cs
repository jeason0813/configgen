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

using System;
using ConfigGen.Utilities;
using ConfigGen.Utilities.Annotations;

namespace ConfigGen.Domain.Contract.Preferences
{
    public class Preference<TPreference, TTarget> : IPreference<TPreference>
    {
        [NotNull]
        private readonly Func<string, TTarget> _parseAction;

        [NotNull]
        private readonly Action<TTarget, TPreference> _setAction;

        public Preference(
            [NotNull] string name,
            [CanBeNull] string shortName,
            [NotNull] string description,
            [CanBeNull] string argumentHelpText,
            [NotNull] Func<string, TTarget> parseAction,
            [NotNull] Action<TTarget, TPreference> setAction)
        {
            _parseAction = parseAction;
            _setAction = setAction;
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (description == null) throw new ArgumentNullException(nameof(description));
            if (parseAction == null) throw new ArgumentNullException(nameof(parseAction));
            if (setAction == null) throw new ArgumentNullException(nameof(setAction));

            Name = name;
            ShortName = shortName;
            Description = description;
            ArgumentHelpText = argumentHelpText;
        }

        [NotNull]
        public string Name { get; }

        [CanBeNull]
        public string ShortName { get; }

        [NotNull]
        public string Description { get; }

        [CanBeNull]
        public string ArgumentHelpText { get; }

        [NotNull]
        public Type PreferenceInstanceType => typeof(TPreference);

        [NotNull]
        public Type TargetPropertyType => typeof(TTarget);

        public void Set([NotNull] TPreference target, [CanBeNull] string value)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            TTarget actualValue = _parseAction(value);
            _setAction(actualValue, target);
        }

        public IResult<object, string> TestValue(string value)
        {
            try
            {
                TTarget actualValue = _parseAction(value);
                return Result<object, string>.CreateSuccessResult(actualValue);
            }
            catch (Exception ex)
            {
                return Result<object, string>.CreateFailureResult($"{ex.Message}");
            }
        }
    }
}