namespace ElementSuite.Common.Interface
{
    using System.IO;

    /// <summary>
    /// File service provided by Element Suite to handle file operations.
    /// </summary>
    public interface IFileService : IService
	{
        /// <summary>
        /// Opens a file for reading.
        /// </summary>
        /// <param name="path">Relative path to the file to be read.</param>
        /// <returns>A <see cref="System.IO.FileStream"/> of the opened file</returns>
		FileStream OpenFile(string path);

        /// <summary>
        /// Deletes the file at the given path.
        /// </summary>
        /// <param name="path">Relative path of the file to be deleted.</param>
        void DeleteFile(string path);

        /// <summary>
        /// Creates a directory.
        /// </summary>
        /// <param name="dir">Relative path of the directory to be created.</param>
        void CreateDirectory(string dir);

        /// <summary>
        /// Deletes a directory
        /// </summary>
        /// <param name="dir">Relative path of the directory to be deleted.</param>
        void DeleteDirectory(string dir);
	}
}