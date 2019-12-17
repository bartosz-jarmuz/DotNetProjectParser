using System.Collections.Generic;

namespace DotNetProjectParser
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyGroup
    {
        /// <summary>
        /// The build condition
        /// </summary>
        public Condition Condition { get; internal set; }

        /// <summary>
        /// All properties
        /// </summary>
        public Dictionary<string, string> AllProperties { get; internal set; } = new Dictionary<string, string>();

        /// <summary>
        /// Treat All Warnings As Errors
        /// </summary>
        public bool TreatWarningsAsErrors { get; internal set; }

        /// <summary>
        /// Specifies which warnings are treated as errors
        /// </summary>
        public string WarningsAsErrors { get; internal set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public Platform PlatformTarget { get; internal set; }


        /// <summary>
        /// 
        /// </summary>
        public bool Optimize{ get; internal set; }
       
        /// <summary>
        /// 
        /// </summary>
        public bool AllowUnsafeBlocks { get; internal set; }
        

        /// <summary>
        /// 
        /// </summary>
        public string OutputPath { get; internal set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string DefineConstants { get; internal set; } = "";


        /// <summary>
        /// 
        /// </summary>
        public int WarningLevel { get; internal set; }
     
        /// <summary>
        /// 
        /// </summary>
        public bool Prefer32Bit { get; internal set; }



    }
}