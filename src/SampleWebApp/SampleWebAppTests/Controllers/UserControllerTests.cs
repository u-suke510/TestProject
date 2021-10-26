using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SampleWebApp.Models;
using SampleWebApp.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleWebApp.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        private Mock<IServiceProvider> mockProvider;
        private Mock<ILoggerFactory> mockLoggerFactory;
        private Mock<IUser> mockModel;

        public UserControllerTests()
        {
            mockProvider = new Mock<IServiceProvider>();
            mockLoggerFactory = new Mock<ILoggerFactory>();
            mockModel = new Mock<IUser>();
        }

        #region IndexTest
        [TestMethod()]
        public void IndexTest()
        {
            // setup mock
            mockModel.Setup(x => x.GetIndexViewModel()).Returns(new IndexViewModel());

            var controller = new UserController(
                mockProvider.Object, mockLoggerFactory.Object, mockModel.Object);
            var actual = controller.Index() as ViewResult;

            // assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(string.IsNullOrEmpty(actual.ViewName));
            Assert.IsInstanceOfType(actual.Model, typeof(IndexViewModel));
            mockModel.Verify(x => x.GetIndexViewModel());
        }
        #endregion IndexTest

        #region FormTest
        [TestMethod()]
        public void FormTest_new_form()
        {
            // test data
            var id = string.Empty;

            var controller = new UserController(
                mockProvider.Object, mockLoggerFactory.Object, mockModel.Object);
            var actual = controller.Form(id) as ViewResult;

            // assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(string.IsNullOrEmpty(actual.ViewName));
            Assert.IsNull(actual.Model);
            mockModel.Verify(x => x.GetFormViewModel(It.IsAny<string>()), Times.Never);
        }

        [TestMethod()]
        public void FormTest_edit_form_user_not_exists()
        {
            // test data
            var id = "00000000-0000-0000-0000-000000000000";

            // setup mock
            mockModel.Setup(x => x.GetFormViewModel(It.IsAny<string>())).Returns<FormViewModel>(null);

            var controller = new UserController(
                mockProvider.Object, mockLoggerFactory.Object, mockModel.Object);
            var actual = controller.Form(id) as ViewResult;

            // assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(string.IsNullOrEmpty(actual.ViewName));
            Assert.IsNull(actual.Model);
            Assert.IsTrue(controller.ModelState.Keys.Contains("ExistUser"));
            Assert.AreEqual("user not exists.", controller.ModelState["ExistUser"].Errors[0].ErrorMessage);
            mockModel.Verify(x => x.GetFormViewModel(It.IsAny<string>()));
        }

        [TestMethod()]
        public void FormTest_edit_form()
        {
            // test data
            var id = "00000000-0000-0000-0000-000000000000";

            // setup mock
            mockModel.Setup(x => x.GetFormViewModel(It.IsAny<string>())).Returns(new FormViewModel());

            var controller = new UserController(
                mockProvider.Object, mockLoggerFactory.Object, mockModel.Object);
            var actual = controller.Form(id) as ViewResult;

            // assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(string.IsNullOrEmpty(actual.ViewName));
            Assert.IsInstanceOfType(actual.Model, typeof(FormViewModel));
            mockModel.Verify(x => x.GetFormViewModel(It.IsAny<string>()));
        }

        [TestMethod()]
        public void FormTest_post_isvalid_false()
        {
            // test data
            var viewModel = new FormViewModel();

            var controller = new UserController(
                mockProvider.Object, mockLoggerFactory.Object, mockModel.Object);
            controller.ModelState.AddModelError("TestError", "Test Error Message.");
            var actual = controller.Form(viewModel) as ViewResult;

            // assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(string.IsNullOrEmpty(actual.ViewName));
            Assert.AreEqual(viewModel, actual.Model);
            mockModel.Verify(x => x.ExistUser(It.IsAny<FormViewModel>()), Times.Never);
        }

        [TestMethod()]
        public void FormTest_post_exist_user_true()
        {
            // test data
            var viewModel = new FormViewModel();

            // setup mock
            mockModel.Setup(x => x.ExistUser(It.IsAny<FormViewModel>())).Returns(true);

            var controller = new UserController(
                mockProvider.Object, mockLoggerFactory.Object, mockModel.Object);
            var actual = controller.Form(viewModel) as ViewResult;

            // assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(string.IsNullOrEmpty(actual.ViewName));
            Assert.AreEqual(viewModel, actual.Model);
            Assert.IsTrue(controller.ModelState.Keys.Contains("ExistUser"));
            Assert.AreEqual("user email already exists.", controller.ModelState["ExistUser"].Errors[0].ErrorMessage);
            mockModel.Verify(x => x.ExistUser(It.IsAny<FormViewModel>()));
            mockModel.Verify(x => x.Registr(It.IsAny<FormViewModel>()), Times.Never);
        }

        [TestMethod()]
        public void FormTest_post()
        {
            // test data
            var viewModel = new FormViewModel();

            // setup mock
            mockModel.Setup(x => x.ExistUser(It.IsAny<FormViewModel>())).Returns(false);
            mockModel.Setup(x => x.Registr(It.IsAny<FormViewModel>())).Returns(true);

            var controller = new UserController(
                mockProvider.Object, mockLoggerFactory.Object, mockModel.Object);
            var actual = controller.Form(viewModel) as RedirectToActionResult;

            // assert
            Assert.IsNotNull(actual);
            Assert.AreEqual("Index", actual.ActionName);
            mockModel.Verify(x => x.ExistUser(It.IsAny<FormViewModel>()));
            mockModel.Verify(x => x.Registr(It.IsAny<FormViewModel>()));
        }
        #endregion FormTest

        #region DeleteTest
        [TestMethod()]
        public void DeleteTest()
        {
            // test data
            var ids = new List<string>();
            var viewHtmlContent = "test view html content.";

            // setup mock
            var mockTempData = new Mock<ITempDataDictionary>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockRouteData = new Mock<RouteData>();
            var mockActionDescriptor = new Mock<ControllerActionDescriptor>();
            var mockView = new Mock<IView>();
            mockView.Setup(x => x.RenderAsync(It.IsAny<ViewContext>())).Callback<ViewContext>(x => x.Writer.Write(viewHtmlContent));
            var mockEngine = new Mock<ICompositeViewEngine>();
            mockEngine.Setup(x => x.FindView(It.IsAny<ActionContext>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(ViewEngineResult.Found("test view", mockView.Object));
            mockProvider.Setup(x => x.GetService(typeof(ICompositeViewEngine))).Returns(mockEngine.Object);

            var controller = new UserController(mockProvider.Object, mockLoggerFactory.Object, mockModel.Object) {
                ControllerContext = new ControllerContext {
                    HttpContext = mockHttpContext.Object,
                    RouteData = mockRouteData.Object,
                    ActionDescriptor = mockActionDescriptor.Object
                },
                TempData = mockTempData.Object
            };
            var actual = controller.Delete(ids) as JsonResult;

            // assert
            Assert.IsNotNull(actual);
            var data = getJsonData(actual);
            Assert.AreEqual("200", data["status"]);
            Assert.AreEqual(viewHtmlContent, data["content"]);
            mockModel.Verify(x => x.Delete(It.IsAny<List<string>>()));
            mockModel.Verify(x => x.GetListItems());
        }
        #endregion DeleteTest

        private Dictionary<string, string> getJsonData(JsonResult jsonResult)
        {
            var result = new Dictionary<string, string>();

            var props = jsonResult.Value.GetType().GetProperties();
            foreach (var prop in props)
            {
                var name = prop.Name;
                result.Add(name, prop.GetValue(jsonResult.Value).ToString());
            }

            return result;
        }
    }
}
