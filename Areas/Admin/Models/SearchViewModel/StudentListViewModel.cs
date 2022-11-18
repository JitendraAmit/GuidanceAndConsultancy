using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuidanceConsultancy.Areas.Admin.Models.SearchViewModel
{
    public class StudentListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string ContactNo { get; set; }
        
        public string Gender { get; set; }
        public string Class { get; set; }
        public string School { get; set; }
        public string Status { get; set; }

    }
}