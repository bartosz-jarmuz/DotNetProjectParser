using System.Xml.Serialization;

namespace DotNetProjectParser.ProjectModel
{
    /// <summary>
    /// An item of the project
    /// </summary>
    public class ProjectItem
    {
        /// <summary>
        /// The project which contains the item
        /// </summary>
        [XmlIgnore]
        public Project Project { get; set; }

        /// <summary>
        /// Type of item (Compile, Reference, EmbeddedResource etc)
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// The relative path of the item that is included 
        /// </summary>
        public string Include { get; set; }

        /// <summary>
        /// The absolute path of the included item
        /// </summary>
        public string ResolvedIncludePath { get; set; }

        /// <summary>
        /// Is the item be copied to output
        /// </summary>
        public string CopyToOutputDirectory { get; set; }
    }
}