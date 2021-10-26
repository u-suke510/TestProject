using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleWebApp.Models;
using SampleWebApp.ViewModels.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SampleWebApp.Controllers
{
    public class UserController : Controller
    {
        protected readonly ICompositeViewEngine engine;

        private ILogger<UserController> logger;
        private IUser model;

        public UserController(IServiceProvider provider, ILoggerFactory loggerFactory, IUser model)
        {
            engine = provider.GetService<ICompositeViewEngine>();
            logger = loggerFactory.CreateLogger<UserController>();
            this.model = model;
        }

        public IActionResult Index()
        {
            var viewModel = model.GetIndexViewModel();
            return View(viewModel);
        }

        public IActionResult Form(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }

            var viewModel = model.GetFormViewModel(id);
            if (viewModel == null)
            {
                ModelState.AddModelError("ExistUser", "user not exists.");
            }
            return View(viewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Form(FormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (model.ExistUser(viewModel))
                {
                    ModelState.AddModelError("ExistUser", "user email already exists.");
                }
                else
                {
                    model.Registr(viewModel);
                    return RedirectToAction("Index");
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Delete(List<string> ids)
        {
            model.Delete(ids);

            ViewData.Model = model.GetListItems();
            var result = new { status = (int)HttpStatusCode.OK, content = CreateViewContent("_UserList") };

            return Json(result);
        }

        private string CreateViewContent(string viewName)
        {
            var viewString = string.Empty;
            using (var writer = new StringWriter())
            {
                var viewResult = engine.FindView(ControllerContext, viewName, false);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, writer, new HtmlHelperOptions());
                viewResult.View.RenderAsync(viewContext);
                viewString = writer.GetStringBuilder().ToString();
            }
            return viewString;
        }
    }
}
