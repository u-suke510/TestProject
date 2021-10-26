using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SampleLibs;
using SampleLibs.Entities;
using SampleWebApp.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleWebApp.Models.Tests
{
    [TestClass()]
    public class UserTests
    {
        private Mock<ILogger<IUser>> mockLog;
        private Mock<ILoggerFactory> mockLoggerFactory;
        private Mock<AppDbContext> mockDbContext;
        private Mock<DbSet<MUser>> mockDbSet;

        public UserTests()
        {
            mockLog = new Mock<ILogger<IUser>>();
            mockLoggerFactory = new Mock<ILoggerFactory>();
            mockDbContext = new Mock<AppDbContext>();
        }

        #region DeleteTest
        [TestMethod()]
        public void DeleteTest_param_null()
        {
            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.Delete(null);

            // assert
            Assert.IsTrue(actual);
            mockDbContext.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void DeleteTest_ids_empty()
        {
            // test data
            var ids = new List<string>();

            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.Delete(ids);

            // assert
            Assert.IsTrue(actual);
            mockDbContext.Verify(x => x.SaveChanges(), Times.Never);
        }

        [TestMethod()]
        public void DeleteTest_user_not_found()
        {
            // test data
            var ids = new List<string> { "10000000-0000-0000-0000-000000000000" };
            // mock data
            var dataEntity = new List<MUser> {
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000001",
                    DispName = "テスト太郎",
                    Email = "t.taro@test.com"
                }
            }.AsQueryable();

            // setup mock
            mockDbSet = createMockDbSet(dataEntity);
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.Delete(ids);

            // assert
            Assert.IsTrue(actual);
            mockDbContext.Verify(x => x.Remove(It.IsAny<MUser>()), Times.Never);
            mockDbContext.Verify(x => x.SaveChanges());
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // test data
            var ids = new List<string> {
                "10000000-0000-0000-0000-000000000001",
                "10000000-0000-0000-0000-000000000002",
                "10000000-0000-0000-0000-000000000003"
            };
            // mock data
            var dataEntity = new List<MUser> {
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000001",
                    DispName = "テスト太郎",
                    Email = "t.taro@test.com"
                },
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000002",
                    DispName = "テスト二郎",
                    Email = "j.taro@test.com"
                },
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000004",
                    DispName = "テスト三郎",
                    Email = "s.taro@test.com"
                }
            }.AsQueryable();

            // setup mock
            mockDbSet = createMockDbSet(dataEntity);
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.Delete(ids);

            // assert
            Assert.IsTrue(actual);
            mockDbContext.Verify(x => x.Users.Remove(It.IsAny<MUser>()), Times.Exactly(2));
            mockDbContext.Verify(x => x.SaveChanges());
        }
        #endregion DeleteTest

        #region ExistUserTest
        [TestMethod()]
        public void ExistUserTest_exists()
        {
            // test data
            var viewModel = new FormViewModel { Email = "t.taro@test.com" };
            // mock data
            var dataEntity = new List<MUser> {
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000001",
                    DispName = "テスト太郎",
                    Email = "t.taro@test.com"
                },
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000002",
                    DispName = "テスト二郎",
                    Email = "t.jiro@test.com"
                },
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000004",
                    DispName = "テスト三郎",
                    Email = "t.saburo@test.com"
                }
            }.AsQueryable();

            // setup mock
            mockDbSet = createMockDbSet(dataEntity);
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.ExistUser(viewModel);

            // assert
            Assert.IsTrue(actual);
        }

        [TestMethod()]
        public void ExistUserTest_own_email()
        {
            // test data
            var viewModel = new FormViewModel { Id = "10000000-0000-0000-0000-000000000001", Email = "t.taro@test.com" };
            // mock data
            var dataEntity = new List<MUser> {
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000001",
                    DispName = "テスト太郎",
                    Email = "t.taro@test.com"
                },
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000002",
                    DispName = "テスト二郎",
                    Email = "j.taro@test.com"
                },
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000004",
                    DispName = "テスト三郎",
                    Email = "s.taro@test.com"
                }
            }.AsQueryable();

            // setup mock
            mockDbSet = createMockDbSet(dataEntity);
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.ExistUser(viewModel);

            // assert
            Assert.IsFalse(actual);
        }

        [TestMethod()]
        public void ExistUserTest_not_exists()
        {
            // test data
            var viewModel = new FormViewModel { Email = "t.siro@test.com" };
            // mock data
            var dataEntity = new List<MUser> {
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000001",
                    DispName = "テスト太郎",
                    Email = "t.taro@test.com"
                },
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000002",
                    DispName = "テスト二郎",
                    Email = "j.taro@test.com"
                },
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000004",
                    DispName = "テスト三郎",
                    Email = "s.taro@test.com"
                }
            }.AsQueryable();

            // setup mock
            mockDbSet = createMockDbSet(dataEntity);
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.ExistUser(viewModel);

            // assert
            Assert.IsFalse(actual);
        }
        #endregion ExistUserTest

        #region GetFormViewModelTest
        [TestMethod()]
        public void GetFormViewModelTest_user_not_exists()
        {
            // test data
            var id = "10000000-0000-0000-0000-000000000004";
            // mock data
            var dataEntity = new List<MUser> {
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000001",
                    DispName = "テスト太郎",
                    Email = "t.taro@test.com"
                },
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000002",
                    DispName = "テスト二郎",
                    Email = "t.jiro@test.com"
                },
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000003",
                    DispName = "テスト三郎",
                    Email = "t.saburo@test.com"
                }
            }.AsQueryable();
            // expected data
            var expected = dataEntity.First();

            // setup mock
            mockDbSet = createMockDbSet(dataEntity);
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.GetFormViewModel(id);

            // assert
            Assert.IsNull(actual);
        }

        [TestMethod()]
        public void GetFormViewModelTest()
        {
            // test data
            var id = "10000000-0000-0000-0000-000000000001";
            // mock data
            var dataEntity = new List<MUser> {
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000001",
                    DispName = "テスト太郎",
                    Email = "t.taro@test.com"
                },
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000002",
                    DispName = "テスト二郎",
                    Email = "t.jiro@test.com"
                },
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000004",
                    DispName = "テスト三郎",
                    Email = "t.saburo@test.com"
                }
            }.AsQueryable();
            // expected data
            var expected = dataEntity.First();

            // setup mock
            mockDbSet = createMockDbSet(dataEntity);
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.GetFormViewModel(id);

            // assert
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.DispName, actual.Name);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Birthday, actual.Birthday);
        }
        #endregion GetFormViewModelTest

        #region GetIndexViewModelTest
        [TestMethod()]
        public void GetIndexViewModelTest_item_zero()
        {
            // mock data
            var dataEntity = new List<MUser>().AsQueryable();

            // setup mock
            mockDbSet = createMockDbSet(dataEntity);
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.GetIndexViewModel();

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(dataEntity.Count(), actual.Items.Count);
        }

        [TestMethod()]
        public void GetIndexViewModelTest_item_once()
        {
            // mock data
            var dataEntity = new List<MUser> {
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000001",
                    DispName = "テスト太郎",
                    Email = "t.taro@test.com"
                }
            }.AsQueryable();
            // expected data
            var expected = dataEntity.First();

            // setup mock
            mockDbSet = createMockDbSet(dataEntity);
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.GetIndexViewModel();

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(dataEntity.Count(), actual.Items.Count);
            var item = actual.Items.First();
            Assert.AreEqual(expected.Id, item.Id);
            Assert.AreEqual(expected.DispName, item.Name);
            Assert.AreEqual(expected.Email, item.Email);
        }

        [TestMethod()]
        public void GetIndexViewModelTest_some_items()
        {
            // mock data
            var dataEntity = new List<MUser> {
                new MUser { Email = "user03@test.com" },
                new MUser { Email = "user02@test.com" },
                new MUser { Email = "user01@test.com" },
                new MUser { Email = "user05@test.com" },
                new MUser { Email = "user04@test.com" }
            }.AsQueryable();
            // expected data
            var expected = new string[] {
                "user01@test.com",
                "user02@test.com",
                "user03@test.com",
                "user04@test.com",
                "user05@test.com"
            };

            // setup mock
            mockDbSet = createMockDbSet(dataEntity);
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.GetIndexViewModel();

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(dataEntity.Count(), actual.Items.Count);
            CollectionAssert.AreEqual(expected, actual.Items.Select(x => x.Email).ToArray());
        }
        #endregion GetIndexViewModelTest

        #region GetListItemsTest
        [TestMethod()]
        public void GetListItemsTest_data_0()
        {
            // mock data
            var dataEntity = new List<MUser>().AsQueryable();

            // setup mock
            mockDbSet = createMockDbSet(dataEntity);
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.GetListItems();

            // assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.Any());
        }

        [TestMethod()]
        public void GetListItemsTest_data_once()
        {
            // mock data
            var dataEntity = new List<MUser> {
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000001",
                    DispName = "テスト太郎",
                    Email = "t.taro@test.com"
                }
            }.AsQueryable();
            // expected data
            var expected = dataEntity.First();

            // setup mock
            mockDbSet = createMockDbSet(dataEntity);
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.GetListItems();

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count());
            var data = actual.First();
            Assert.AreEqual(expected.Id, data.Id);
            Assert.AreEqual(expected.DispName, data.Name);
            Assert.AreEqual(expected.Email, data.Email);
        }

        [TestMethod()]
        public void GetListItemsTest_some_data()
        {
            // mock data
            var dataEntity = new List<MUser> {
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000001",
                    DispName = "テスト太郎",
                    Email = "t.taro@test.com"
                },
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000002",
                    DispName = "テスト二郎",
                    Email = "t.jiro@test.com"
                },
                new MUser {
                    Id = "10000000-0000-0000-0000-000000000003",
                    DispName = "テスト三郎",
                    Email = "t.saburo@test.com"
                }
            }.AsQueryable();
            // expected data
            var expected = new string[] {
                "10000000-0000-0000-0000-000000000002",
                "10000000-0000-0000-0000-000000000003",
                "10000000-0000-0000-0000-000000000001"
            };

            // setup mock
            mockDbSet = createMockDbSet(dataEntity);
            mockDbContext.Setup(x => x.Users).Returns(mockDbSet.Object);

            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.GetListItems();

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(3, actual.Count());
            CollectionAssert.AreEqual(expected, actual.Select(x => x.Id).ToArray());
        }
        #endregion GetListItemsTest

        #region RegistrTest
        [TestMethod()]
        public void RegistrTest_insert()
        {
            // test data
            var viewModel = new FormViewModel {
                Email = "t.taro@test.com",
                Name = "テスト太郎",
                Birthday = DateTime.Parse("2021/10/26")
            };

            // setup mock
            mockDbContext.Setup(x => x.Add(It.IsAny<MUser>())).Callback<MUser>(x => {
                Assert.IsTrue(Guid.TryParse(x.Id, out Guid guid));
                Assert.AreEqual(viewModel.Email, x.UserName);
                Assert.AreEqual(viewModel.Name, x.DispName);
                Assert.AreEqual(viewModel.Email, x.Email);
                Assert.AreEqual(viewModel.Birthday, x.Birthday);
            });

            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.Registr(viewModel);

            // assert
            Assert.IsTrue(actual);
            mockDbContext.Verify(x => x.Add(It.IsAny<MUser>()));
            mockDbContext.Verify(x => x.Update(It.IsAny<MUser>()), Times.Never);
            mockDbContext.Verify(x => x.SaveChanges());
        }

        [TestMethod()]
        public void RegistrTest_update()
        {
            // test data
            var viewModel = new FormViewModel {
                Id = "10000000-0000-0000-0000-000000000001",
                Email = "t.taro@test.com",
                Name = "テスト太郎",
                Birthday = DateTime.Parse("2021/10/26")
            };

            // setup mock
            mockDbContext.Setup(x => x.Update(It.IsAny<MUser>())).Callback<MUser>(x => {
                Assert.AreEqual(viewModel.Id, x.Id);
                Assert.AreEqual(viewModel.Email, x.UserName);
                Assert.AreEqual(viewModel.Name, x.DispName);
                Assert.AreEqual(viewModel.Email, x.Email);
                Assert.AreEqual(viewModel.Birthday, x.Birthday);
            });

            var service = new User(mockLoggerFactory.Object, mockDbContext.Object);
            var actual = service.Registr(viewModel);

            // assert
            Assert.IsTrue(actual);
            mockDbContext.Verify(x => x.Add(It.IsAny<MUser>()), Times.Never);
            mockDbContext.Verify(x => x.Update(It.IsAny<MUser>()));
            mockDbContext.Verify(x => x.SaveChanges());
        }
        #endregion RegistrTest

        private Mock<DbSet<MUser>> createMockDbSet(IQueryable<MUser> entities)
        {
            var result = new Mock<DbSet<MUser>>();
            result.As<IQueryable<Type>>().Setup(x => x.Provider).Returns(entities.Provider);
            result.As<IQueryable<Type>>().Setup(x => x.Expression).Returns(entities.Expression);
            result.As<IQueryable<Type>>().Setup(x => x.ElementType).Returns(entities.ElementType);
            result.As<IQueryable<MUser>>().Setup(x => x.GetEnumerator()).Returns(entities.GetEnumerator());

            return result;
        }
    }
}
