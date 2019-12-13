using System.IO;
using System.Xml.Linq;

namespace DotNetProjectParser.Readers
{
    internal interface IXmlProjectFileReader
    {
        Project ReadFile(FileInfo projectFile, XDocument projectXml);

    }
}