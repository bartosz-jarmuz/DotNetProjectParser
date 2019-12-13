using System;

namespace DotNetProjectParser
{
    /// <summary>
    /// Exception thrown by parser
    /// </summary>
    public class ProjectParserException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ProjectParserException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ProjectParserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// The path of the project
        /// </summary>
        public string ProjectPath { get; private set; }

        /// <summary>
        /// Type of parser in use
        /// </summary>
        public Type ParserType { get; }

        /// <summary>
        /// 
        /// </summary>
        public ProjectParserException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="path"></param>
        /// <param name="parserType"></param>
        public ProjectParserException(string message, string path, Type parserType) : base(message)
        {
            this.ProjectPath = path;
            this.ParserType = parserType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="path"></param>
        /// <param name="parserType"></param>
        /// <param name="innerException"></param>
        public ProjectParserException(string message, string path, Type parserType, Exception innerException) : base(
            message, innerException)
        {
            this.ProjectPath = path;
            this.ParserType = parserType;
        }
    }
}