using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DotNetProjectParser.Readers
{
    internal class NetFrameworkProjectReader : IXmlProjectFileReader
    {
        private XmlNamespaceManager namespaceManager;

        private static class XmlNames
        {
            public static string PropertyGroup { get; }= "PropertyGroup";
            public static string Condition { get; }= "Condition";
            public static string OutputType { get; }= "OutputType";
            public static string AssemblyName { get; }= "AssemblyName";
            public static string TargetFrameworkVersion { get; }= "TargetFrameworkVersion";
            public static string ItemGroup { get; }= "ItemGroup";
            public static string Include { get; }= "Include";
            public static string CopyToOutputDirectory { get; }= "CopyToOutputDirectory";
            public const string PlatformTarget = "PlatformTarget";
            public const string Optimize = "Optimize";
            public const string OutputPath = "OutputPath";
            public const string DefineConstants = "DefineConstants";
            public const string AllowUnsafeBlocks = "AllowUnsafeBlocks";
            public const string Prefer32Bit = "Prefer32Bit";
            public const string TreatWarningsAsErrors = "TreatWarningsAsErrors";
            public const string WarningsAsErrors = "WarningsAsErrors";
            public const string DebugType = "DebugType";
        }

        public Project ReadFile(FileInfo projectFile, XDocument projectXml)
        {
            if (projectFile == null) throw new ArgumentNullException(nameof(projectFile));
            if (projectXml == null) throw new ArgumentNullException(nameof(projectXml));

            this.LoadNamespaceManager(projectXml);

            Project project = new Project();
            this.LoadStaticProperties(project, projectFile, projectXml);
            project.PropertyGroups = this.LoadPropertyGroups(projectXml);
            project.Items = this.LoadItems(project, projectXml);
            return project;
        }

        private void LoadNamespaceManager(XDocument projectXml)
        {
            this.namespaceManager = new XmlNamespaceManager(new NameTable());
            this.namespaceManager.AddNamespace("x", projectXml.Root.GetDefaultNamespace().NamespaceName);
        }

        private void LoadStaticProperties(Project project, FileInfo projectFile, XDocument xml)
        {
            Debug.Assert(xml.Root != null, "xml.Root != null");

            project.Name = projectFile.Name;
            project.FullPath = projectFile.FullName;
            project.DirectoryPath = projectFile.Directory?.FullName;
            project.ProjectXml = xml;


            XElement propertiesSection = xml.Root.Elements().FirstOrDefault(x =>
                x.Name.LocalName == XmlNames.PropertyGroup && x.Elements().Any(y => y.Name.LocalName == XmlNames.OutputType));

            if (propertiesSection != null)
            {
                project.AssemblyName = propertiesSection.GetByLocalName(XmlNames.AssemblyName)?.Value;
                project.OutputType = propertiesSection.GetByLocalName(XmlNames.OutputType)?.Value;
                project.TargetFramework = propertiesSection.GetByLocalName(XmlNames.TargetFrameworkVersion)?.Value;
            }

            SetExtension(project);
        }

        private List<PropertyGroup> LoadPropertyGroups(XDocument xml)
        {
            Debug.Assert(xml.Root != null, "xml.Root != null");
            var list = new List<PropertyGroup>();
            foreach (XElement propertiesSection in xml.Root.Elements().Where(x=>x.Name.LocalName == XmlNames.PropertyGroup && x.Attribute(XmlNames.Condition) != null))
            {
                var propertyGroup = new PropertyGroup();
                SetCondition(propertiesSection, propertyGroup);
                SetDefaults(propertyGroup, propertyGroup.Condition.Configuration);

                SetProperties(propertiesSection, propertyGroup);

                if (propertyGroup.PlatformTarget == Platform.Unspecified)
                {
                    propertyGroup.PlatformTarget = propertyGroup.Condition?.Platform??Platform.Unspecified;
                }
             
                list.Add(propertyGroup);
            }
            return list;
        }

        private static void SetProperties(XElement propertiesSection, PropertyGroup propertyGroup)
        {
            foreach (XElement property in propertiesSection.Elements())
            {
                UpdateAllPropertiesDictionary(propertyGroup, property);

                switch (property.Name.LocalName)
                {
                    case XmlNames.AllowUnsafeBlocks:
                        propertyGroup.AllowUnsafeBlocks = property.GetValue<bool>(false);
                        break;
                    case XmlNames.DefineConstants:
                        propertyGroup.DefineConstants = property.Value;
                        break;
                    case XmlNames.Optimize:
                        propertyGroup.Optimize = property.GetValue<bool>(false);
                        break;
                    case XmlNames.OutputPath:
                        propertyGroup.OutputPath = property.Value;
                        break;
                    case XmlNames.PlatformTarget:
                        propertyGroup.PlatformTarget = ConditionParser.ParsePlatform(property.Value);
                        break;
                    case XmlNames.Prefer32Bit:
                        propertyGroup.Prefer32Bit = property.GetValue<bool>(false);
                        break;
                    case XmlNames.TreatWarningsAsErrors:
                        propertyGroup.TreatWarningsAsErrors = property.GetValue<bool>(false);
                        break;
                    case XmlNames.WarningsAsErrors:
                        propertyGroup.WarningsAsErrors = property.Value;
                        break;
                }
            }
        }

        private static void SetCondition(XElement propertiesSection, PropertyGroup propertyGroup)
        {
            var conditionAttribute = propertiesSection.Attribute(XmlNames.Condition);
            if (conditionAttribute != null)
            {
                propertyGroup.Condition = ConditionParser.Parse(conditionAttribute);
            }
        }

        private static void SetDefaults(PropertyGroup propertyGroup, string configurationName)
        {
            propertyGroup.WarningLevel = 4;
            if (configurationName == "Debug")
            {
                propertyGroup.DefineConstants = "DEBUG;TRACE";
            }
            else if (configurationName == "Release")
            {
                propertyGroup.DefineConstants = "TRACE";
            }
        }


        private static void UpdateAllPropertiesDictionary(PropertyGroup propertyGroup, XElement property)
        {
            if (!propertyGroup.AllProperties.ContainsKey(property.Name.LocalName))
            {
                propertyGroup.AllProperties.Add(property.Name.LocalName, property.Value);
            }
        }


       


        private static void SetExtension(Project project)
        {
            project.TargetExtension = ".dll";
            if (string.Equals(project.OutputType, "exe", StringComparison.OrdinalIgnoreCase))
            {
                project.TargetExtension = ".exe";
            }
        }

        private List<ProjectItem> LoadItems(Project project,XDocument xml)
        {
            Debug.Assert(xml.Root != null, "xml.Root != null");


            IEnumerable<XElement> itemsSections = xml.Root.Elements().Where(x => x.Name.LocalName == XmlNames.ItemGroup);
            var list = new List<ProjectItem>();
            foreach (XElement itemsSection in itemsSections)
            {
                foreach (XElement xElement in itemsSection.Elements())
                {
                    ProjectItem item = new ProjectItem
                    {
                        Project = project,
                        ItemType = xElement.Name.LocalName,
                        Include = xElement.Attributes().FirstOrDefault(x => x.Name.LocalName == XmlNames.Include)
                            ?.Value
                    };
                    if (item.Include != null)
                    {
                        item.ResolvedIncludePath = Path.Combine(project.DirectoryPath, item.Include);
                        item.ItemName = Path.GetFileName(item.Include);
                    }

                    item.CopyToOutputDirectory = xElement.Elements().FirstOrDefault(x => x.Name.LocalName == XmlNames.CopyToOutputDirectory)?.Value;
                    list.Add(item);
                }
            }

            return list;
        }
    }
}
