using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuidanceConsultancy.Areas.Admin.Models.SchoolViewModel
{
    public class CreateEditSchoolViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "School Type")]
        public int? SchoolTypeId { get; set; }

        [Required]
        [Display(Name = "School Medium")]
        public int? SchoolMediumId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedOn { get; set; }

        public SelectList SelectSchoolTypeList { get; set; }
        public SelectList SelectSchoolMediumList { get; set; }

    }
}