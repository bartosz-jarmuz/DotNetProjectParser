using System.IO;
using System.Xml.Linq;
using DotNetProjectParser.ProjectModel;

namespace DotNetProjectParser.Readers
{
    internal interface IXmlProjectFileReader
    {
        Project ReadFile(FileInfo projectFile, XDocument projectXml);

    }
}