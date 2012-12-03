namespace ElementSuite.Common
{
    using System;

    /// <summary>
    /// Encapsulates work queue contextual information relative to the specific instatiation of the work queue.
    /// </summary>
    [Serializable]
	public sealed class WorkContext 
	{
        /// <summary>
        /// A <see cref="System.Reflection.Assembly"/> file loaded as a common object file format (COFF)-based image via a byte array.
        /// Refer to http://msdn.microsoft.com/en-us/library/h538bck7.aspx for details on converting the (COFF)-based
        /// image back into an <see cref="System.Reflection.Assembly"/> file.
        /// </summary>
        public Byte[] WorkCommandFile { get; set; }
    }
}

