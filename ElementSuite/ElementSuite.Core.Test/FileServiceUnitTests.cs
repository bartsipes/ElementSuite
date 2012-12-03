using ElementSuite.Core.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace ElementSuite.Core.Test
{
    /// <summary>
    ///This is a test class for CHRWTrucksCarrierRepositoryTest and is intended
    ///to contain all CHRWTrucksCarrierRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileServiceUnitTest
    {
        private TestContext testContextInstance;
 
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
 
        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod]
        [Priority(3)]
        public void TestFileServiceCtor()
        {
            var fs = new FileService();
            Assert.IsNotNull(fs, "FileService ctor did not return an object");
        }
        [TestMethod]
        [Priority(2)]
        public void TestFileServiceOpenFile()
        {
            var fs = new FileService();
            var stream = fs.OpenFile("TestOpenFile.txt");
            Assert.IsNotNull(stream, "FileService OpenFile did not return an object");
            stream.Close();
        }
        [TestMethod]
        [Priority(2)]
        public void TestFileServiceOpenFileAndSave()
        {
            var fileName = "TestOpenFileAndSave.txt";
            var fs = new FileService();
            var stream = fs.OpenFile(fileName);
            Assert.IsNotNull(stream, "FileService OpenFile did not return an object");
            var sw = new StreamWriter(stream);
            sw.WriteLine("Testing testing");
            sw.Flush();
            sw.Close();
            stream.Close();
            stream = fs.OpenFile(fileName);
            var sr = new StreamReader(stream);
            var readValue = sr.ReadToEnd();
            Assert.IsFalse(string.IsNullOrEmpty(readValue), "No text was read from the saved file");
            stream.Close();
        }
        [TestMethod]
        [Priority(2)]
        public void TestCreateDirectory()
        {
            var fs = new FileService();
            fs.CreateDirectory("TestDirectory");
        }
        [TestMethod]
        [Priority(1)]
        public void TestFileServiceDeleteOpenedFile()
        {
            var fs = new FileService();
            Assert.IsNotNull(fs, "FileService ctor did not return an object");
            fs.DeleteFile("TestOpenFile.txt");
        }
        [TestMethod]
        [Priority(1)]
        public void TestFileServiceDeleteOpenAndSavedFile()
        {
            var fs = new FileService();
            Assert.IsNotNull(fs, "FileService ctor did not return an object");
            fs.DeleteFile("TestOpenFileAndSave.txt");
        }
        [TestMethod]
        [Priority(1)]
        public void TestDeleteDirectory()
        {
            var fs = new FileService();
            fs.DeleteDirectory("TestDirectory");
        }
    }
}