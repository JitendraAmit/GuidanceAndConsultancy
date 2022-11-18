using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuidanceConsultancy.Areas.Admin.Models.StudentViewModel
{
    public class StudentResultModel
    {
        public string SubTest { get; set; }
        public int? ScoreObtained { get; set; }
        public int? StenScore { get; set; }
        public string Performance { get; set; }
       
    }
}