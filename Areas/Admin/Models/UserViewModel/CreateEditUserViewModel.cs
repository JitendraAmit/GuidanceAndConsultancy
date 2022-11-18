using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuidanceConsultancy.Areas.Admin.Models.UserViewModel
{
    public class CreateEditUserViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "User Name")]
        [Remote("UserAlreadyExist", "User", ErrorMessage = "this username already exists")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Pwd { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedOn { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int? RoleId { get; set; }

        public SelectList SelectRoleList { get; set; }
    }
}