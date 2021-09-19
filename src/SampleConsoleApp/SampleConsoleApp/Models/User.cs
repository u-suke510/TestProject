using System;
using System.ComponentModel.DataAnnotations;

namespace SampleConsoleApp.Models
{
    public class User
    {
        [Key]
        public string Id
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string DispName
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public DateTime Birthday
        {
            get;
            set;
        }
    }
}
