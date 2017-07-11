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
using System.Linq;
using ConfigGen.Api.Contract;
using ConfigGen.Utilities.Annotations;
using Machine.Specifications;

namespace ConfigGen.Tests.Common.MSpecShouldExtensions.GenerateResultExtensions
{
    public static class GenerateResultExtensions
    {
        /// <summary>
        /// Asserts the supplied result indicates success.
        /// </summary>
        [NotNull]
        public static GenerateResult ShouldIndicateSuccess([NotNull] this GenerateResult results)
        {
            if (results == null) throw new ArgumentNullException(nameof(results));

            var combinedIssues = results.GenerationIssues.Union(results.GeneratedFiles.SelectMany(f => f.GenerationIssues)).ToArray();

            if (!combinedIssues.Any())
            {
                return results;
            }

            throw new SpecificationException($"Should indicate success, but indicates failure with the following issues {string.Join("\n- ", combinedIssues.Select(e => e.ToString()))}");
        }

        /// <summary>
        /// Asserts the supplied result indicates success.
        /// </summary>
        [NotNull]
        public static GenerateResult ShouldIndicateFailure([NotNull] this GenerateResult results)
        {
            if (results == null) throw new ArgumentNullException(nameof(results));

            if (results.GenerationIssues.Any()
                || results.GeneratedFiles.Any(f => f.GenerationIssues.Any()))
            {
                return results;
            }

            throw new SpecificationException($"Should indicate failure, but indicates success");
        }
    }
}