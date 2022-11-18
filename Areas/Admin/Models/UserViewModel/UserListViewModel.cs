using GuidanceConsultancy.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuidanceConsultancy.Areas.Admin.Models.UserViewModel
{
    public class UserListViewModel
    {
        public List<Mst_Login> UserModel { get; set; }

     
    }
}