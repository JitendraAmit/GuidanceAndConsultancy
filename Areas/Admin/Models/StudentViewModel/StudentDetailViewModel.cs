using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuidanceConsultancy.Areas.Admin.Models.StudentViewModel
{
    public class StudentDetailViewModel
    {
        public int Id { get; set; }
        public List<StudentResultModel> StudentResultList { get; set; }
        public string BarChartImagePath { get; set; }

        public string Name { get; set; }
        public string FatherName { get; set; }
        public string ContactNo { get; set; }
        public string Class { get; set; }
        public string Gender { get; set; }
        public string SchoolType { get; set; }
        public string School { get; set; }
        public string EmailId { get; set; }

        [AllowHtml]
        public string Suggestions { get; set; }
        public string PdfPath { get; set; }
        public bool IsPdfPath { get; set; }
        public bool? IsVerified { get; set; }
        public int? SchoolMediumId { get; set; }
    }
}