using System.Xml.Serialization;

namespace DotNetProjectParser
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
        public Project Project { get; internal set; }

        /// <summary>
        /// Name of item
        /// </summary>
        public string ItemName { get; internal set; }

        /// <summary>
        /// Type of item (Compile, Reference, EmbeddedResource etc)
        /// </summary>
        public string ItemType { get; internal set; }

        /// <summary>
        /// The relative path of the item that is included 
        /// </summary>
        public string Include { get; internal set; }

        /// <summary>
        /// The absolute path of the included item
        /// </summary>
        public string ResolvedIncludePath { get; internal set; }

        /// <summary>
        /// Is the item be copied to output
        /// </summary>
        public string CopyToOutputDirectory { get; internal set; }
    }
}