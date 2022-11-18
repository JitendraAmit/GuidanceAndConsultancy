using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuidanceConsultancy.Areas.Admin.Models.NormsViewModel
{
    public class CreateEditNormsViewModel
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int SchoolTypeId { get; set; }
        public int SubTestId { get; set; }
        public int MinRange { get; set; }
        public int MaxRange { get; set; }
        public int StenScore { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedOn { get; set; }

        public SelectList SelectClassList { get; set; }
        public SelectList SelectSchoolTypeList { get; set; }
        public SelectList SelectSubTestList { get; set; }
    }
}