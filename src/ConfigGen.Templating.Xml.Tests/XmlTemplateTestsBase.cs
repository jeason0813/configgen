﻿using System;
using ConfigGen.Domain.Contract;
using ConfigGen.Tests.Common;
using Machine.Specifications;

namespace ConfigGen.Templating.Xml.Tests
{
    [Subject(typeof(XmlTemplate))]
    public abstract class XmlTemplateTestsBase
    {
        private static Lazy<XmlTemplate> lazySubject; 
        protected static string TemplateContents;
        protected static TokenValuesCollection TokenValues;
        protected static TemplateRenderResults Result;
        protected static string ExpectedOutput;

        Establish context = () =>
        {
            TemplateContents = null;
            lazySubject = new Lazy<XmlTemplate>(() => new XmlTemplate(TemplateContents));
            TokenValues = null;
            Result = null;
            ExpectedOutput = null;
        };

        protected static XmlTemplate Subject => lazySubject.Value;
    }
}