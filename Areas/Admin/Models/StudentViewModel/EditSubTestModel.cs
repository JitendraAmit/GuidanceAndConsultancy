using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuidanceConsultancy.Areas.Admin.Models.StudentViewModel
{
    public class EditSubTestModel
    {
        public int? TestId { get; set; }
        public string TestName { get; set; }
        public int? ObtainMarkes { get; set; }
    }
}