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

using System.IO;
using System.Reflection;

namespace ConfigGen.Utilities.Extensions
{
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Returns a stream for the specified resource file.
        /// </summary>
        /// <param name="assembly">Assembly from which to get resource</param>
        /// <param name="resourcePath">Resource path of file, not including the assembly name; e.g. for resource MyDomain.MyAssembly.MyFolder.MyResource 
        /// in assembly MyDomain.MyAssembly, resource path is MyFolder.MyResource</param>
        /// <returns>Stream</returns>
        public static Stream GetEmbeddedResourceFileStream(this Assembly assembly, string resourcePath)
        {
            var resourceName = assembly.FullName.Split(',')[0] + "." + resourcePath;
            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}