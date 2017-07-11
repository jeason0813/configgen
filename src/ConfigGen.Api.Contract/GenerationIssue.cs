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
using ConfigGen.Utilities.Annotations;

namespace ConfigGen.Api.Contract
{
    /// <summary>
    /// Represents an issue (either an error or a warning) during file generation
    /// </summary>
    public class GenerationIssue
    {
        public GenerationIssue([NotNull] string code, [NotNull] string source, [CanBeNull] string detail)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (detail == null) throw new ArgumentNullException(nameof(detail));

            Code = code;
            Source = source;
            Detail = detail;
        }

        /// <summary>
        /// Gets the code for the issue.
        /// </summary>
        [NotNull]
        public string Code { get; }

        /// <summary>
        /// Gets the source of the issue (i.e. the component in which the issue occurred).
        /// </summary>
        [NotNull]
        public string Source { get; }

        /// <summary>
        /// Gets detail on the issue, if any, otherwise null.
        /// </summary>
        [CanBeNull]
        public string Detail { get; }

        public override string ToString()
        {
            return $"{Detail} ('{Code}' in '{Source}')";
        }
    }
}