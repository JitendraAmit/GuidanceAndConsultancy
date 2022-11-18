using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuidanceConsultancy.Models.db;

namespace GuidanceConsultancy.Areas.Admin.Models.StudentViewModel
{
    public class CreateEditStudentViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Father Name")]
        public string FatherName { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public int? GenderId { get; set; }

        //[Required]
        //[Display(Name = "School Type")]
        //public int SchoolTypeId { get; set; }
        [Required]
        [Display(Name = "School")]
        public int? SchoolId { get; set; }

        [Required]
        [Display(Name = "Class")]
        public int? ClassId { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
        public int? CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedOn { get; set; }

        public SelectList SelectGenderList { get; set; }
        public SelectList SelectSchoolTypeList { get; set; }
        public SelectList SelectSchoolList { get; set; }
        public SelectList SelectClassList { get; set; }
        public List<Mst_SubTest> SubTestModel { get; set; }
        public List<EditSubTestModel> EditDataSubTestModel { get; set; }
    }
}