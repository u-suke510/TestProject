using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SampleConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleConsoleApp.Tests
{
    [TestClass()]
    public class BatServiceTests
    {
        private Mock<ILogger<BatService>> mockLog;
        private Mock<AppDbContext> mockDbContext;
        private Mock<DbSet<User>> mockDbSet;
        private Mock<DatabaseFacade> mockDatabase;

        public BatServiceTests()
        {
            mockLog = new Mock<ILogger<BatService>>();
            mockDbContext = new Mock<AppDbContext>();
        }

        #region AddUserTest
        [TestMethod()]
        public void AddUserTest_param_null()
        {
            // setup mock
            mockDbContext.Setup(x => x.Add(It.IsAny<object>()));

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.AddUser(null);

            // assert
            Assert.IsFalse(actual);
            mockDbContext.Verify(x => x.Add(It.IsAny<object>()), Times.Never);
            mockDbContext.Verify(x => x.SaveChanges(), Times.Never);
            mockLog.Verify(x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "Data is Null."),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [TestMethod()]
        public void AddUserTest_param_data_blank()
        {
            // test data
            var data = new string[] { };

            // setup mock
            mockDbContext.Setup(x => x.Add(It.IsAny<object>()));

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.AddUser(data);

            // assert
            Assert.IsFalse(actual);
            mockDbContext.Verify(x => x.Add(It.IsAny<object>()), Times.Never);
            mockDbContext.Verify(x => x.SaveChanges(), Times.Never);
            mockLog.Verify(x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "Data is Null."),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [TestMethod()]
        public void AddUserTest_param_data_len_2()
        {
            // test data
            var data = new string[] { "DispName", "test@test.com" };

            // setup mock
            mockDbContext.Setup(x => x.Add(It.IsAny<object>()));

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.AddUser(data);

            // assert
            Assert.IsFalse(actual);
            mockDbContext.Verify(x => x.Add(It.IsAny<object>()), Times.Never);
            mockDbContext.Verify(x => x.SaveChanges(), Times.Never);
            mockLog.Verify(x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "Data Size 2."),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [TestMethod()]
        public void AddUserTest_param_data_len_4()
        {
            // test data
            var data = new string[] { "DispName", "test@test.com", "2021-09-01", "test col 4." };

            // setup mock
            mockDbContext.Setup(x => x.Add(It.IsAny<object>()));

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.AddUser(data);

            // assert
            Assert.IsFalse(actual);
            mockDbContext.Verify(x => x.Add(It.IsAny<object>()), Times.Never);
            mockDbContext.Verify(x => x.SaveChanges(), Times.Never);
            mockLog.Verify(x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "Data Size 4."),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [TestMethod()]
        public void AddUserTest_param_data0_empty()
        {
            // test data
            var data = new string[] { "", "test@test.com", "2021-09-01" };

            // setup mock
            mockDbContext.Setup(x => x.Add(It.IsAny<object>()));

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.AddUser(data);

            // assert
            Assert.IsFalse(actual);
            mockDbContext.Verify(x => x.Add(It.IsAny<object>()), Times.Never);
            mockDbContext.Verify(x => x.SaveChanges(), Times.Never);
            mockLog.Verify(x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "DispName is Empty."),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [TestMethod()]
        public void AddUserTest_param_data1_empty()
        {
            // test data
            var data = new string[] { "DispName", "", "2021-09-01" };

            // setup mock
            mockDbContext.Setup(x => x.Add(It.IsAny<object>()));

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.AddUser(data);

            // assert
            Assert.IsFalse(actual);
            mockDbContext.Verify(x => x.Add(It.IsAny<object>()), Times.Never);
            mockDbContext.Verify(x => x.SaveChanges(), Times.Never);
            mockLog.Verify(x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "UserName is Empty."),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [TestMethod()]
        public void AddUserTest_param_data2_fmt_err()
        {
            // test data
            var data = new string[] { "DispName", "test@test.com", "20210901" };

            // setup mock
            mockDbContext.Setup(x => x.Add(It.IsAny<object>()));

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.AddUser(data);

            // assert
            Assert.IsFalse(actual);
            mockDbContext.Verify(x => x.Add(It.IsAny<object>()), Times.Never);
            mockDbContext.Verify(x => x.SaveChanges(), Times.Never);
            mockLog.Verify(x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "Birthday is Format Error."),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [TestMethod()]
        public void AddUserTest_ok()
        {
            // test data
            var data = new string[] { "DispName", "test@test.com", "2021-09-01" };

            // setup mock
            mockDbContext.Setup(x => x.Add(It.IsAny<User>())).Callback<User>(x => {
                Assert.IsTrue(Guid.TryParse(x.Id, out Guid guid));
                Assert.AreEqual(data[1], x.UserName);
                Assert.AreEqual(data[0], x.DispName);
                Assert.AreEqual(data[1], x.Email);
                Assert.AreEqual(data[2], $"{x.Birthday:yyyy-MM-dd}");
            });

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.AddUser(data);

            // assert
            Assert.IsTrue(actual);
            mockDbContext.Verify(x => x.Add(It.IsAny<User>()));
            mockDbContext.Verify(x => x.SaveChanges());
            mockLog.Verify(x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Never);
        }
        #endregion AddUserTest

        #region EnsureTableTest
        [TestMethod()]
        public void EnsureTableTest_param_none()
        {
            // setup mock
            mockDatabase = new Mock<DatabaseFacade>(mockDbContext.Object);
            mockDatabase.Setup(x => x.EnsureCreated()).Returns(true);
            mockDbContext.SetupGet(x => x.Database).Returns(mockDatabase.Object);
            mockDbContext.Setup(x => x.ExecuteSqlRaw(It.IsAny<string>(), It.IsAny<object[]>()));

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.EnsureTable();

            // assert
            Assert.IsTrue(actual);
            mockDbContext.Verify(x => x.ExecuteSqlRaw(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
            mockDatabase.Verify(x => x.EnsureCreated());
        }

        [TestMethod()]
        public void EnsureTableTest_param_false()
        {
            // setup mock
            mockDatabase = new Mock<DatabaseFacade>(mockDbContext.Object);
            mockDatabase.Setup(x => x.EnsureCreated()).Returns(true);
            mockDbContext.SetupGet(x => x.Database).Returns(mockDatabase.Object);
            mockDbContext.Setup(x => x.ExecuteSqlRaw(It.IsAny<string>(), It.IsAny<object[]>()));

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.EnsureTable(false);

            // assert
            Assert.IsTrue(actual);
            mockDbContext.Verify(x => x.ExecuteSqlRaw(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
            mockDatabase.Verify(x => x.EnsureCreated());
        }

        [TestMethod()]
        public void EnsureTableTest_param_true()
        {
            // setup mock
            mockDatabase = new Mock<DatabaseFacade>(mockDbContext.Object);
            mockDatabase.Setup(x => x.EnsureCreated()).Returns(true);
            mockDbContext.SetupGet(x => x.Database).Returns(mockDatabase.Object);
            mockDbContext.Setup(x => x.ExecuteSqlRaw(It.IsAny<string>(), It.IsAny<object[]>()));

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.EnsureTable(true);

            // assert
            Assert.IsTrue(actual);
            mockDbContext.Verify(x => x.ExecuteSqlRaw(It.IsAny<string>(), It.IsAny<object[]>()));
            mockDatabase.Verify(x => x.EnsureCreated());
        }
        #endregion EnsureTableTest

        #region ExistUserTest
        [TestMethod()]
        public void ExistUserTest_usrNm_null()
        {
            // mock data
            var dataEntity = new List<User> {
                new User { UserName = "test1@test.com" },
                new User { UserName = "test2@test.com" },
                new User { UserName = "test3@test.com" }
            }.AsQueryable();

            // setup mock
            mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<Type>>().Setup(x => x.Provider).Returns(dataEntity.Provider);
            mockDbSet.As<IQueryable<Type>>().Setup(x => x.Expression).Returns(dataEntity.Expression);
            mockDbSet.As<IQueryable<Type>>().Setup(x => x.ElementType).Returns(dataEntity.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(x => x.GetEnumerator()).Returns(dataEntity.GetEnumerator());
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.ExistUser(null);

            // assert
            Assert.IsFalse(actual);
        }

        [TestMethod()]
        public void ExistUserTest_usrNm_blank()
        {
            // test data
            var usrNm = string.Empty;
            // mock data
            var dataEntity = new List<User> {
                new User { UserName = "test1@test.com" },
                new User { UserName = "test2@test.com" },
                new User { UserName = "test3@test.com" }
            }.AsQueryable();

            // setup mock
            mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<Type>>().Setup(x => x.Provider).Returns(dataEntity.Provider);
            mockDbSet.As<IQueryable<Type>>().Setup(x => x.Expression).Returns(dataEntity.Expression);
            mockDbSet.As<IQueryable<Type>>().Setup(x => x.ElementType).Returns(dataEntity.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(x => x.GetEnumerator()).Returns(dataEntity.GetEnumerator());
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.ExistUser(usrNm);

            // assert
            Assert.IsFalse(actual);
        }

        [TestMethod()]
        public void ExistUserTest_usrNm_not_exist()
        {
            // test data
            var usrNm = "test4@test.com";
            // mock data
            var dataEntity = new List<User> {
                new User { UserName = "test1@test.com" },
                new User { UserName = "test2@test.com" },
                new User { UserName = "test3@test.com" }
            }.AsQueryable();

            // setup mock
            mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<Type>>().Setup(x => x.Provider).Returns(dataEntity.Provider);
            mockDbSet.As<IQueryable<Type>>().Setup(x => x.Expression).Returns(dataEntity.Expression);
            mockDbSet.As<IQueryable<Type>>().Setup(x => x.ElementType).Returns(dataEntity.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(x => x.GetEnumerator()).Returns(dataEntity.GetEnumerator());
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.ExistUser(usrNm);

            // assert
            Assert.IsFalse(actual);
        }

        [TestMethod()]
        public void ExistUserTest_usrNm_exist()
        {
            // test data
            var usrNm = "test2@test.com";
            // mock data
            var dataEntity = new List<User> {
                new User { UserName = "test1@test.com" },
                new User { UserName = "test2@test.com" },
                new User { UserName = "test3@test.com" }
            }.AsQueryable();

            // setup mock
            mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<Type>>().Setup(x => x.Provider).Returns(dataEntity.Provider);
            mockDbSet.As<IQueryable<Type>>().Setup(x => x.Expression).Returns(dataEntity.Expression);
            mockDbSet.As<IQueryable<Type>>().Setup(x => x.ElementType).Returns(dataEntity.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(x => x.GetEnumerator()).Returns(dataEntity.GetEnumerator());
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.ExistUser(usrNm);

            // assert
            Assert.IsTrue(actual);
        }
        #endregion ExistUserTest

        #region ParseCsvTest
        [TestMethod()]
        public void ParseCsvTest_path_null()
        {
            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.ParseCsv(null);

            // assert
            Assert.IsNull(actual);
            mockLog.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "[Params] - path:"),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
            mockLog.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "File Not Found.()"),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [TestMethod()]
        public void ParseCsvTest_path_empty()
        {
            var path = "";

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.ParseCsv(path);

            // assert
            Assert.IsNull(actual);
            mockLog.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "[Params] - path:"),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
            mockLog.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "File Not Found.()"),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [TestMethod()]
        public void ParseCsvTest_path_not_found()
        {
            var path = @"csv\test.csv";

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.ParseCsv(path);

            // assert
            Assert.IsNull(actual);
            mockLog.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == $"[Params] - path:{path}"),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
            mockLog.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == $"File Not Found.({path})"),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [TestMethod()]
        public void ParseCsvTest_csv_blank()
        {
            var path = @"csv\test_blank.csv";

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.ParseCsv(path);

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
            mockLog.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == $"[Params] - path:{path}"),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [TestMethod()]
        public void ParseCsvTest_csv_single_line()
        {
            var path = @"csv\test_single_line.csv";
            var expected = new string[] { "column1", "column2", "column3" };

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.ParseCsv(path);

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            CollectionAssert.AreEqual(expected, actual[0]);
            mockLog.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == $"[Params] - path:{path}"),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [TestMethod()]
        public void ParseCsvTest_csv_multiple_lines()
        {
            var path = @"csv\test_multiple_lines.csv";
            var expected = new List<string[]> {
                new string[] { "row1-col1", "row1-col2", "row1-col3", "row1-col4", "row1-col5" },
                new string[] { "row2-col1", "row2-col2", "row2-col3", "row2-col4", "row2-col5" },
                new string[] { "row3-col1", "row3-col2", "row3-col3", "row3-col4", "row3-col5" }
            };

            var service = new BatService(mockLog.Object, mockDbContext.Object);
            var actual = service.ParseCsv(path);

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Count, actual.Count);
            CollectionAssert.AreEqual(expected[0], actual[0]);
            CollectionAssert.AreEqual(expected[1], actual[1]);
            CollectionAssert.AreEqual(expected[2], actual[2]);
            mockLog.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == $"[Params] - path:{path}"),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
        #endregion ParseCsvTest
    }
}
