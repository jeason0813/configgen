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
using System.Xml.Linq;
using ConfigGen.Domain.Contract;
using ConfigGen.Templating.Xml.NodeProcessing.ExpressionEvaluation;
using ConfigGen.Utilities.Extensions;
using JetBrains.Annotations;

namespace ConfigGen.Templating.Xml.NodeProcessing
{
    /// <summary>
    /// Node processor for processing "appyWhen" attribute.
    /// </summary>
    internal class ApplyWhenAttributeProcessor : IConfigGenNodeProcessor
    {
        private readonly ConfigurationExpressionEvaluator _evaluator;
   //     private static readonly ILog Log = LogManager.GetLogger(typeof(ApplyWhenAttributeProcessor));

        public ApplyWhenAttributeProcessor(ConfigurationExpressionEvaluator evaluator)
        {
            _evaluator = evaluator;
        }

        [NotNull]
        public ProcessNodeResults ProcessNode(
            [NotNull] XElement element, 
            [NotNull] ITokenDataset dataset)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (dataset == null) throw new ArgumentNullException(nameof(dataset));

            var attributeName = XName.Get("applyWhen", XmlTemplate.ConfigGenXmlNamespace);
            var attribute = element.Attribute(attributeName);
            if (attribute == null)
            {
                throw new ArgumentException("Supplied element should contain an attribute with name 'applyWhen' in the configgen namespace.", nameof(element));
            }

            var expression = attribute.Value;
            if (expression.IsNullOrEmpty())
            {
                attribute.Remove();
                return new ProcessNodeResults(null, null, "Condition error: and empty condition was encountered");
            }

            IEnumerable<string> locatedTokens = _evaluator.PrepareExpression(ref expression);

            var usedTokens = new List<string>();
            var unrecognisedTokens = new List<string>();

            foreach (var locatedToken in locatedTokens)
            {
                if (dataset.Contains(locatedToken))
                {
                    usedTokens.Add(locatedToken);
                }
                else
                {
                    unrecognisedTokens.Add(locatedToken);
                }
            }

            bool result = _evaluator.Evaluate(
                dataset.Name,
                expression);

        //    Log.DebugFormat("Expression evaluated '{0}' for machine '{1}': {2}", result, machineConfigurationSettings.MachineName, expression);

            if (result)
            {
                attribute.Remove();
            }
            else
            {
                element.Remove();
            }

            return new ProcessNodeResults(usedTokens, unrecognisedTokens, null);
        }
    }
}
