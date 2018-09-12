using System.Collections.ObjectModel;
using System.Text;
using AddressProcessing.CSV;
using AddressProcessing.FileSystem;
using NSubstitute;
using NUnit.Framework;

namespace Csv.Tests
{
    [TestFixture]
    public class CsvReaderWriterTests
    {
        private ICsvReaderWriter _csvReaderWriter;
        private IFile _file;

        [SetUp]
        public void SetUp()
        {
            _file = Substitute.For<IFile>();
            _csvReaderWriter = new CsvReaderWriter(_file);
        }

        [Test]
        public void ShouldWriteColumnsProperly_OnWrite()
        {
            _csvReaderWriter.Write("1", "2", "3");
            _file.Received().WriteLine("1\t2\t3");
        }

        [Test]
        public void ShouldReadFirstTwoColumns_OnRead()
        {
            _file.ReadLine().Returns("1\t2\t3");

            Assert.IsTrue(_csvReaderWriter.Read(out var first, out var second));
            
            Assert.AreEqual("1", first);
            Assert.AreEqual("2", second);
        }

        [Test]
        public void ShouldReturnFalse_IfFileIsEmpty_OnRead()
        {
            _file.ReadLine().Returns(string.Empty);

            Assert.IsFalse(_csvReaderWriter.Read(out var first, out var second));
            Assert.IsNull(first);
            Assert.IsNull(second);
        }
        
        [Test]
        public void ShouldReturnFalse_IfFileReturnsNull_OnRead()
        {
            _file.ReadLine().Returns((string)null);

            Assert.IsFalse(_csvReaderWriter.Read(out var first, out var second));
            Assert.IsNull(first);
            Assert.IsNull(second);
        }

        [Test]
        public void ShouldReadAllColumns_OnRead()
        {
            _file.ReadLine().Returns("1\t2\t3");

            Assert.IsTrue(_csvReaderWriter.TryRead(out var columns));
            
            CollectionAssert.AreEquivalent(new [] {"1", "2", "3"}, columns);
        }

        [Test]
        public void ShouldNotThrow_IfCloseTwice()
        {
            _csvReaderWriter.Close();
            _csvReaderWriter.Close();
        }
    }
}
