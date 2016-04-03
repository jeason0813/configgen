#region Copyright and License Notice
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

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace ConfigGen.Domain.Contract
{
    public class TemplateRenderResults
    {
        public TemplateRenderResults(
            TemplateRenderResultStatus status, 
            [CanBeNull] string renderedResult, 
            [CanBeNull] IEnumerable<string> usedTokens, 
            [CanBeNull] IEnumerable<string> unusedTokens,
            [CanBeNull] IEnumerable<string> unrecognisedTokens,
            [CanBeNull] IEnumerable<Error> errors)
        {
            Status = status;
            RenderedResult = renderedResult;
            UsedTokens = usedTokens?.ToArray() ?? new string[0];
            UnusedTokens = unusedTokens?.ToArray() ?? new string[0];
            UnrecognisedTokens = unrecognisedTokens?.ToArray() ?? new string[0];
            Errors = errors?.ToArray() ?? new Error[0];
        }

        public TemplateRenderResultStatus Status { get; }

        [CanBeNull]
        public string RenderedResult { get; }

        [NotNull]
        public Error[] Errors { get; }

        [NotNull]
        public string[] UsedTokens { get; }

        [NotNull]
        public string[] UnusedTokens { get; }

        [NotNull]
        public string[] UnrecognisedTokens { get; }
    }
}