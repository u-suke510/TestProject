using System;
using System.ComponentModel.DataAnnotations;

namespace SampleWebApp.ViewModels.User
{
    public class FormViewModel
    {
        public string Id
        {
            get;
            set;
        }

        [Required]
        public string Name
        {
            get;
            set;
        }

        [Required]
        public string Email
        {
            get;
            set;
        }

        public DateTime Birthday
        {
            get;
            set;
        } = DateTime.MinValue;
    }
}
