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
using ConfigGen.Utilities.Annotations;

namespace ConfigGen.Api.Contract
{
    /// <summary>
    /// Represents the result of a generation attempt for a single file.
    /// </summary>
    public class GeneratedFile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="GeneratedFile"/> class.
        /// </summary>
        public GeneratedFile(
            [NotNull] string configurationName,
            [CanBeNull] string fullPath,
            [NotNull] IEnumerable<string> usedTokens,
            [NotNull] IEnumerable<string> unusedTokens,
            [NotNull] IEnumerable<string> unrecognisedTokens,
            [NotNull] IEnumerable<GenerationIssue> generationIssues,
            bool hasChanged)
        {
            if (configurationName == null) throw new ArgumentNullException(nameof(configurationName));
            if (usedTokens == null) throw new ArgumentNullException(nameof(usedTokens));
            if (unusedTokens == null) throw new ArgumentNullException(nameof(unusedTokens));
            if (unrecognisedTokens == null) throw new ArgumentNullException(nameof(unrecognisedTokens));
            if (generationIssues == null) throw new ArgumentNullException(nameof(generationIssues));

            ConfigurationName = configurationName;
            FullPath = fullPath;
            UsedTokens = usedTokens;
            UnusedTokens = unusedTokens;
            UnrecognisedTokens = unrecognisedTokens;
            GenerationIssues = generationIssues;
            HasChanged = hasChanged;
        }

        /// <summary>
        /// Gets the name of the configuration of this file.
        /// </summary>
        [NotNull]
        public string ConfigurationName { get; }

        /// <summary>
        /// Gets the full path to the generated file, if any, otherwise null.
        /// </summary>
        [CanBeNull]
        public string FullPath { get; }

        /// <summary>
        /// True if the generated file has changed, otherwise false.
        /// </summary>
        public bool HasChanged { get; }

        /// <summary>
        /// Get a collection of generation issues, if any, that occurred during the generation of this file.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<GenerationIssue> GenerationIssues { get; }

        /// <summary>
        /// Gets a collection of tokens which were used during the generation of this file.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<string> UsedTokens { get; }


        /// <summary>
        /// Gets a collection of tokens which were not used during the generation of this file.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<string> UnusedTokens { get; }


        /// <summary>
        /// Gets a collection of tokens which were specified in the template, but not in the configuration.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<string> UnrecognisedTokens { get; }
    }
}