namespace DotNetProjectParser
{
    /// <summary>
    /// The build condition
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// The condition value as string
        /// </summary>
        public string Expression { get; internal set; }
        
        /// <summary>
        /// The configuration
        /// </summary>
        public string Configuration { get; internal set; }
        
        /// <summary>
        /// The Platform
        /// </summary>
        public Platform Platform { get; internal set; }
    }
}