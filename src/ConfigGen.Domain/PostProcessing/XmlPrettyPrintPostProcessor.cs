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
using System.IO;
using ConfigGen.Domain.Contract.PostProcessing;
using ConfigGen.Domain.Contract.Preferences;
using ConfigGen.Utilities.Annotations;
using ConfigGen.Utilities.Xml;

namespace ConfigGen.Domain.PostProcessing
{
    public class XmlPrettyPrintPostProcessor : IPostprocessor
    {
        [NotNull]
        private readonly IXmlStreamFormatter _prettyPrintFormatter;

        [NotNull]
        private readonly IPreferencesManager _preferencesManager;

        public XmlPrettyPrintPostProcessor(
            [NotNull] IXmlStreamFormatter prettyPrintFormatter,
            [NotNull] IPreferencesManager preferencesManager)
        {
            if (prettyPrintFormatter == null) throw new ArgumentNullException(nameof(prettyPrintFormatter));
            if (preferencesManager == null) throw new ArgumentNullException(nameof(preferencesManager));

            _prettyPrintFormatter = prettyPrintFormatter;
            _preferencesManager = preferencesManager;
        }

        public string Process(string renderedOutput)
        {
            var preferences = _preferencesManager.GetPreferenceInstance<PostProcessingPreferences>();

            if (!preferences.XmlPrettyPrintEnabled)
            {
                return renderedOutput;
            }

            var xmlStreamFormatterOptions = XmlStreamFormatterOptions.Default;
            xmlStreamFormatterOptions.MaxElementLineLength = preferences.XmlPrettyPrintLineLength;
            xmlStreamFormatterOptions.IndentChars = "".PadRight(preferences.XmlPrettyPrintTabSize, ' ');
           
            using (var sourceStream = new MemoryStream())
            using (var targetStream = new MemoryStream())
            {
                var writer = new StreamWriter(sourceStream);
                writer.Write(renderedOutput);
                writer.Flush();
                sourceStream.Position = 0;
                
                _prettyPrintFormatter.Format(sourceStream, targetStream, xmlStreamFormatterOptions);

                targetStream.Position = 0;

                var reader = new StreamReader(targetStream);
                return reader.ReadToEnd();
            }
        }
    }
}