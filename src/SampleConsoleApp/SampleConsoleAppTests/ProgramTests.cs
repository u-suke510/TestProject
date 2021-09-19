using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SampleConsoleApp.Managers;
using System.Collections.Generic;

namespace SampleConsoleApp.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        private Mock<IConfigManager> mockConf;
        private Mock<IBatService> mockService;

        public ProgramTests()
        {
            mockConf = new Mock<IConfigManager>();
            mockService = new Mock<IBatService>();
        }

        [TestMethod()]
        public void MainTest_args_none_csv_null()
        {
            // test data
            var args = new string[] { };

            // setup mock
            mockService.Setup(x => x.ParseCsv(It.IsAny<string>())).Returns<List<string[]>>(null);

            Program.Setup(mockService.Object, mockConf.Object);
            Program.Main(args);

            // assert
            mockService.Verify(x => x.EnsureTable(false));
            mockService.Verify(x => x.ParseCsv(It.IsAny<string>()));
            mockService.Verify(x => x.ExistUser(It.IsAny<string>()), Times.Never);
        }

        [TestMethod()]
        public void MainTest_args_not_bool_csv_blank()
        {
            // test data
            var args = new string[] { "test1" };
            // mock data
            var csvData = new List<string[]>();

            // setup mock
            mockService.Setup(x => x.ParseCsv(It.IsAny<string>())).Returns(csvData);

            Program.Setup(mockService.Object, mockConf.Object);
            Program.Main(args);

            // assert
            mockService.Verify(x => x.EnsureTable(false));
            mockService.Verify(x => x.ParseCsv(It.IsAny<string>()));
            mockService.Verify(x => x.ExistUser(It.IsAny<string>()), Times.Never);
        }

        [TestMethod()]
        public void MainTest_args_true_csv_1()
        {
            // test data
            var args = new string[] { "true" };
            // mock data
            var csvData = new List<string[]> {
                new string[] { "Test1", "test1@test.com", "1999-10-10" }
            };

            // setup mock
            mockService.Setup(x => x.ParseCsv(It.IsAny<string>())).Returns(csvData);
            mockService.Setup(x => x.ExistUser(It.IsAny<string>())).Returns(true);

            Program.Setup(mockService.Object, mockConf.Object);
            Program.Main(args);

            // assert
            mockService.Verify(x => x.EnsureTable(true));
            mockService.Verify(x => x.ParseCsv(It.IsAny<string>()));
            mockService.Verify(x => x.ExistUser(It.IsAny<string>()), Times.Once);
            mockService.Verify(x => x.AddUser(It.IsAny<string[]>()), Times.Never);
        }

        [TestMethod()]
        public void MainTest_args_false_csv_3()
        {
            // test data
            var args = new string[] { "false" };
            // mock data
            var csvData = new List<string[]> {
                new string[] { "Test1", "test1@test.com", "1999-10-10" },
                new string[] { "Test2", "test2@test.com", "1999-11-11" },
                new string[] { "Test3", "test3@test.com", "1999-12-12" }
            };

            // setup mock
            mockService.Setup(x => x.ParseCsv(It.IsAny<string>())).Returns(csvData);
            mockService.Setup(x => x.ExistUser(It.IsAny<string>())).Returns<string>(x => x != "test2@test.com");

            Program.Setup(mockService.Object, mockConf.Object);
            Program.Main(args);

            // assert
            mockService.Verify(x => x.EnsureTable(false));
            mockService.Verify(x => x.ParseCsv(It.IsAny<string>()));
            mockService.Verify(x => x.ExistUser(It.IsAny<string>()), Times.Exactly(3));
            mockService.Verify(x => x.AddUser(It.Is<string[]>(x => x == csvData[1])), Times.Once);
        }
    }
}
