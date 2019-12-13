using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DotNetProjectParser.ProjectModel;

namespace DotNetProjectParser.Readers
{
    internal class NetFrameworkProjectReader : IXmlProjectFileReader
    {
        private static class XmlNames
        {
            public static string PropertyGroup { get; }= "PropertyGroup";
            public static string OutputType { get; }= "OutputType";
            public static string AssemblyName { get; }= "AssemblyName";
            public static string TargetFrameworkVersion { get; }= "TargetFrameworkVersion";
            public static string ItemGroup { get; }= "ItemGroup";
            public static string Include { get; }= "Include";
            public static string CopyToOutputDirectory { get; }= "CopyToOutputDirectory";
        }

      
        public Project ReadFile(FileInfo projectFile, XDocument projectXml)
        {
            if (projectFile == null) throw new ArgumentNullException(nameof(projectFile));
            if (projectXml == null) throw new ArgumentNullException(nameof(projectXml));

            Project project = new Project();
            this.LoadProperties(project, projectFile, projectXml);
            this.LoadItems(project, projectXml);
            return project;
        }

        private void LoadProperties(Project project, FileInfo projectFile, XDocument xml)
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

        private static void SetExtension(Project project)
        {
            project.TargetExtension = ".dll";
            if (string.Equals(project.OutputType, "exe", StringComparison.OrdinalIgnoreCase))
            {
                project.TargetExtension = ".exe";
            }
        }

        private void LoadItems(Project project,XDocument xml)
        {
            Debug.Assert(xml.Root != null, "xml.Root != null");


            IEnumerable<XElement> itemsSections = xml.Root.Elements().Where(x => x.Name.LocalName == XmlNames.ItemGroup);

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
                    }

                    item.CopyToOutputDirectory = xElement.Elements().FirstOrDefault(x => x.Name.LocalName == XmlNames.CopyToOutputDirectory)?.Value;
                    project.Items.Add(item);
                }
            }
        }
    }
}
