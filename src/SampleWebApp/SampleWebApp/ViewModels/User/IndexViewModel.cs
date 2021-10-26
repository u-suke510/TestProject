using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.ViewModels.User
{
    public class IndexViewModel
    {
        public List<ListItem> Items
        {
            get;
        } = new List<ListItem>();

        public class ListItem
        {
            public string Id
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }

            public string Email
            {
                get;
                set;
            }
        }
    }
}
