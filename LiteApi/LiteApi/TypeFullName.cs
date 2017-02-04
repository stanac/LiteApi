namespace LiteApi
{
    /// <summary>
    /// Type of type name, full name
    /// </summary>
    public enum TypeFullName
    {
        /// <summary>
        /// Always use full name with namespace of the type
        /// </summary>
        FullName,

        /// <summary>
        /// Always use short name without namespace of the type
        /// </summary>
        ShortName,

        /// <summary>
        /// Use full name with namespace for uncommon types, it will use short name for all primitive types and 
        /// types in System, System.Collections.Generic and System.Threading.Tasks
        /// </summary>
        FullNameForUncommonTypes
    }
}
