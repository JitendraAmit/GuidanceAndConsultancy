using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuidanceConsultancy.Models.db;

namespace GuidanceConsultancy.Areas.Admin.Models.SearchViewModel
{
    public class SearchDataModel
    {
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public int? CreatedById { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int PageNo { get; set; }
        public int PageLimit { get; set; }

        public List<StudentListViewModel> StudentListModel { get; set; }
        public SelectList SelectUserList { get; set; }
    }
}