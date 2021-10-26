using Microsoft.Extensions.Logging;
using SampleLibs;
using SampleLibs.Entities;
using SampleWebApp.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleWebApp.Models
{
    public interface IUser
    {
        bool Delete(List<string> ids);

        bool ExistUser(FormViewModel viewModel);

        IndexViewModel GetIndexViewModel();

        FormViewModel GetFormViewModel(string id);

        IEnumerable<IndexViewModel.ListItem> GetListItems();

        bool Registr(FormViewModel viewModel);
    }

    public class User : IUser
    {
        private ILogger<IUser> logger;
        private AppDbContext context;

        public User(ILoggerFactory loggerFactory, AppDbContext context)
        {
            logger = loggerFactory.CreateLogger<IUser>();
            this.context = context;
        }

        public bool Delete(List<string> ids)
        {
            if (ids == null || !ids.Any())
            {
                return true;
            }

            foreach (var id in ids)
            {
                var items = context.Users.Where(x => x.Id == id);
                if (items.Any())
                {
                    context.Users.Remove(items.First());
                }
            }
            context.SaveChanges();

            return true;
        }

        public bool ExistUser(FormViewModel viewModel)
        {
            return context.Users.Any(x => x.Id != viewModel.Id && x.Email == viewModel.Email);
        }

        public FormViewModel GetFormViewModel(string id)
        {
            var items = context.Users.Where(x => x.Id == id);
            if (!items.Any())
            {
                return null;
            }
            var user = items.First();

            return new FormViewModel {
                Id = user.Id,
                Name = user.DispName,
                Email = user.Email,
                Birthday = user.Birthday
            };
        }

        public IndexViewModel GetIndexViewModel()
        {
            var viewModel = new IndexViewModel();
            viewModel.Items.AddRange(GetListItems());

            return viewModel;
        }

        public IEnumerable<IndexViewModel.ListItem> GetListItems()
        {
            var result = context.Users.OrderBy(x => x.Email).Select(x => new IndexViewModel.ListItem {
                Id = x.Id,
                Name = x.DispName,
                Email = x.Email
            });
            return result;
        }

        public bool Registr(FormViewModel viewModel)
        {
            var entity = new MUser {
                Id = viewModel.Id,
                UserName = viewModel.Email,
                DispName = viewModel.Name,
                Email = viewModel.Email,
                Birthday = viewModel.Birthday
            };

            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
                context.Add(entity);
            }
            else
            {
                context.Update(entity);
            }
            context.SaveChanges();

            return true;
        }
    }
}
