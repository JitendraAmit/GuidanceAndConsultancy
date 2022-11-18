using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GuidanceConsultancy.Models.db;

namespace GuidanceConsultancy.Areas.Admin.Models.DashboardViewModel
{
    public class DashboardListModel
    {
       public List<Mst_Student> StudentModel { get; set; }
    }
}