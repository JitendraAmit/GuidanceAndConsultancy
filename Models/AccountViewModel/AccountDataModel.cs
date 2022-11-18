using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GuidanceConsultancy.Models.AccountViewModel
{
    public class AccountDataModel
    {
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        public bool Remember { get; set; }
    }
}