using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuidanceConsultancy.Areas.Admin.Models.StudentViewModel;
using GuidanceConsultancy.Models.db;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net;

namespace GuidanceConsultancy.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        public readonly DishaGuidanceEntities _context;
        public HomeController()
        {
            _context = new DishaGuidanceEntities();
        }
        //public ActionResult Index()
        //{
        //    return View();
        //}


        public ActionResult Index()
        {
            int Id = 13;
            var getStudentData = _context.Mst_Student.FirstOrDefault(m => m.Id == Id);
            var getGraphData = _context.Student_Graph.FirstOrDefault(m => m.StudentId == getStudentData.Id);

            var testResult = (from a in _context.Mst_Student
                              join b in _context.Student_Result on a.Id equals b.StudentId
                              join c in _context.Mst_SubTest on b.SubTestId equals c.Id
                              where a.Id == Id
                              select new { b, SubTest = c.Name });

            //string Kruti_Dev = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "KRDEV010.TTF");
            //string ARIALUNI = "C:/Windows/Fonts/ARIALUNI.TTF";
            //string fontFamily = "C:/Windows/Fonts/MICROSS.TTF";
            // string ARIALUNI = "C:/Windows/Fonts/freeserif.ttf";
            //string Mangal = GuidanceConsultancy.Helpers.AppConstants.BaseURL + "Assets/admin/freeserif/FreeSerif.ttf";
            string Mangal = GuidanceConsultancy.Helpers.AppConstants.BaseURL + "Assets/admin/ARIALUNI.ttf";
            BaseFont bf = BaseFont.CreateFont(Mangal, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            Font NormalHindiFont = new Font(bf, 10, Font.NORMAL);
            Font BoldHindiFont = new Font(bf, 10, Font.BOLD);
            Font SmallHindiFont = new Font(bf, 8, Font.NORMAL);
            Font hindifont = new Font(bf, 7, Font.NORMAL);

            var boldfont = new Font(bf, 11, Font.BOLD, BaseColor.BLACK);
            var Headingboldfont = new Font(bf, 13, Font.BOLD, BaseColor.BLACK);

            var autono = Guid.NewGuid().ToString() + "-" + Helpers.Utilities.GetCurrentDateTime().ToString("yyyy-MM-dd-hh-mm-ss");
            string path = Server.MapPath("~/ApplicationData/StudentPdf/" + @autono + ".pdf");
            //string serverPath = "~/" + path;
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();

            string imageURL = Helpers.AppConstants.BaseURL + "Assets/admin/img/logo.png";
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageURL);
            //Resize image depend upon your need
            img.ScaleToFit(140f, 120f);
            img.SpacingBefore = 10f; //Give space before image
            img.SpacingAfter = 1f; //Give some space after the image

            img.Alignment = Element.ALIGN_LEFT;
            doc.Add(img);
            PdfPTable tblheading = new PdfPTable(1);
            tblheading.WidthPercentage = 100;
            PdfPCell cellheading = new PdfPCell(new Phrase("Apptitude Test Report Sheet", Headingboldfont));
            cellheading.HorizontalAlignment = Element.ALIGN_CENTER;
            cellheading.PaddingBottom = 10;
            cellheading.PaddingLeft = 0;
            cellheading.PaddingRight = 0;
            cellheading.DisableBorderSide(Rectangle.BOX);
            tblheading.AddCell(cellheading);
            doc.Add(tblheading);

            // Set Student Information
            PdfPTable tbl = new PdfPTable(3);
            tbl.WidthPercentage = 100;
            PdfPCell cell = new PdfPCell(new Phrase("नाम : " + getStudentData.Name, NormalHindiFont)); //Name
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.PaddingLeft = 0;
            cell.PaddingRight = 0;
            cell.DisableBorderSide(Rectangle.BOX);
            tbl.AddCell(cell);
            cell = new PdfPCell(new Phrase("पिता का नाम: " + getStudentData.FatherName, NormalHindiFont));  //Father Name
            cell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            //cell.Padding = 0;
            cell.PaddingLeft = 0;
            cell.PaddingRight = 0;
            cell.DisableBorderSide(Rectangle.BOX);
            tbl.AddCell(cell);
            cell = new PdfPCell(new Phrase("मोबाइल न.: " + getStudentData.ContactNo)); //Contact No
            cell.DisableBorderSide(Rectangle.BOX);
            cell.PaddingLeft = 0;
            cell.PaddingRight = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            tbl.AddCell(cell);


            // second row
            cell = new PdfPCell(new Phrase("कक्षा: " + getStudentData.Mst_Class.Name + "(" + getStudentData.Mst_Gender.Name + ")")); //Class
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.PaddingBottom = 10;
            cell.PaddingLeft = 0;
            cell.PaddingRight = 0;
            cell.DisableBorderSide(Rectangle.BOX);
            tbl.AddCell(cell);
            cell = new PdfPCell(new Phrase("विद्यालय का प्रकार: " + getStudentData.Mst_School.Mst_SchoolType.Name)); // School Type
            cell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 10;
            cell.PaddingLeft = 0;
            cell.PaddingRight = 0;
            cell.DisableBorderSide(Rectangle.BOX);
            tbl.AddCell(cell);
            cell = new PdfPCell(new Phrase("विद्यालय: " + getStudentData.Mst_School.Name));  // School
            cell.DisableBorderSide(Rectangle.BOX);
            cell.PaddingLeft = 0;
            cell.PaddingRight = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.PaddingBottom = 10;
            tbl.AddCell(cell);
            doc.Add(tbl);
            // end student information
            // Student Score Start Here

            PdfPTable tblScore = new PdfPTable(7);
            tblScore.WidthPercentage = 100;
            PdfPCell cellScore = new PdfPCell();
            cellScore = new PdfPCell(new Phrase("Sr No", boldfont));
            cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
            cellScore.Border = Rectangle.BOX;
            cellScore.Rowspan = 2;
            tblScore.AddCell(cellScore);
            cellScore = new PdfPCell(new Phrase("Sub Test", boldfont));
            cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
            cellScore.Border = Rectangle.BOX;
            cellScore.Rowspan = 2;
            tblScore.AddCell(cellScore);
            cellScore = new PdfPCell(new Phrase("Obtained Marks", boldfont));
            cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
            cellScore.Border = Rectangle.BOX;
            cellScore.Rowspan = 2;
            tblScore.AddCell(cellScore);
            cellScore = new PdfPCell(new Phrase("Sten Score", boldfont));
            cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
            cellScore.Border = Rectangle.BOX;
            cellScore.Rowspan = 2;
            tblScore.AddCell(cellScore);
            cellScore = new PdfPCell(new Phrase("Performance", boldfont));
            cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
            cellScore.Border = Rectangle.BOX;
            cellScore.Colspan = 3;
            tblScore.AddCell(cellScore);

            // new row colspan 
            cellScore = new PdfPCell(new Phrase("High", boldfont));
            cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
            cellScore.Border = Rectangle.BOX;
            tblScore.AddCell(cellScore);
            cellScore = new PdfPCell(new Phrase("Average", boldfont));
            cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
            cellScore.Border = Rectangle.BOX;
            tblScore.AddCell(cellScore);
            cellScore = new PdfPCell(new Phrase("Low", boldfont));
            cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
            cellScore.Border = Rectangle.BOX;
            tblScore.AddCell(cellScore);

            string correctSelected = Helpers.AppConstants.BaseURL + "Assets/admin/img/correct.png";
            iTextSharp.text.Image tickImg = iTextSharp.text.Image.GetInstance(correctSelected);



            int SrNo = 0;
            foreach (var items in testResult)
            {
                SrNo++;
                cellScore = new PdfPCell(new Phrase(SrNo.ToString()));
                cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
                cellScore.Border = Rectangle.BOX;
                tblScore.AddCell(cellScore);

                cellScore = new PdfPCell(new Phrase(items.SubTest));
                cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
                cellScore.Border = Rectangle.BOX;
                tblScore.AddCell(cellScore);

                cellScore = new PdfPCell(new Phrase(items.b.ObtainScore.ToString()));
                cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
                cellScore.Border = Rectangle.BOX;
                tblScore.AddCell(cellScore);

                cellScore = new PdfPCell(new Phrase(items.b.StenScore.ToString()));
                cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
                cellScore.Border = Rectangle.BOX;
                tblScore.AddCell(cellScore);


                if (items.b.PerformanceScore == "High")
                {

                    cellScore = new PdfPCell();
                    cellScore.AddElement(new Chunk(tickImg, 30, 0));

                    tblScore.AddCell(cellScore);
                    cellScore = new PdfPCell(new Phrase(""));
                    cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellScore.Border = Rectangle.BOX;
                    tblScore.AddCell(cellScore);
                    cellScore = new PdfPCell(new Phrase(""));
                    cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellScore.Border = Rectangle.BOX;
                    tblScore.AddCell(cellScore);
                }
                else if (items.b.PerformanceScore == "Average")
                {

                    cellScore = new PdfPCell(new Phrase(""));
                    cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellScore.Border = Rectangle.BOX;
                    tblScore.AddCell(cellScore);
                    cellScore = new PdfPCell();
                    cellScore.AddElement(new Chunk(tickImg, 30, 0));
                    cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
                    tblScore.AddCell(cellScore);

                    cellScore = new PdfPCell(new Phrase(""));
                    cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellScore.Border = Rectangle.BOX;
                    tblScore.AddCell(cellScore);
                }
                else if (items.b.PerformanceScore == "Low")
                {
                    cellScore = new PdfPCell(new Phrase(""));
                    cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellScore.Border = Rectangle.BOX;
                    tblScore.AddCell(cellScore);

                    cellScore = new PdfPCell(new Phrase(""));
                    cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellScore.Border = Rectangle.BOX;
                    tblScore.AddCell(cellScore);

                    cellScore = new PdfPCell();
                    cellScore.AddElement(new Chunk(tickImg, 30, 0));
                    cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
                    tblScore.AddCell(cellScore);
                }
                else
                {
                    cellScore = new PdfPCell(new Phrase(""));
                    cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellScore.Border = Rectangle.BOX;
                    tblScore.AddCell(cellScore);
                    cellScore = new PdfPCell(new Phrase(""));
                    cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellScore.Border = Rectangle.BOX;
                    tblScore.AddCell(cellScore);
                    cellScore = new PdfPCell(new Phrase(""));
                    cellScore.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellScore.Border = Rectangle.BOX;
                    tblScore.AddCell(cellScore);
                }


            }

            doc.Add(tblScore);

            // Student Score End Here


            string barchartImageUrl = Helpers.AppConstants.BaseURL + getGraphData.GraphImagePath;
            iTextSharp.text.Image graphImg = iTextSharp.text.Image.GetInstance(barchartImageUrl);
            //graphImg.WidthPercentage = 60;
            graphImg.ScaleToFit(600f, 250f);  //Resize image depend upon your need
            //graphImg.SpacingBefore = 10f;   //Give space before image
            //graphImg.SpacingAfter = 1f;  //Give some space after the image
            graphImg.Alignment = Element.ALIGN_CENTER;
            doc.Add(graphImg);

            PdfPTable tbl2 = new PdfPTable(1);
            tbl2.WidthPercentage = 100;
            PdfPCell cell2 = new PdfPCell(new Phrase("APTITUDE PROFILE", boldfont));
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;

            cell2.PaddingLeft = 0;
            cell2.PaddingRight = 0;
            cell2.DisableBorderSide(Rectangle.BOX);
            tbl2.AddCell(cell2);

            cell2 = new PdfPCell(new Phrase("सुझाव / टिप्पणी", boldfont));   //Suggestion / Remark
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.PaddingTop = 10;
            cell2.PaddingLeft = 0;
            cell2.PaddingRight = 0;
            cell2.DisableBorderSide(Rectangle.BOX);
            tbl2.AddCell(cell2);

            //var htmlcon = htmlcon. WebUtility.HtmlDecode(getStudentData.Suggestions);
            cell2 = new PdfPCell(new Phrase(Server.HtmlDecode(getStudentData.Suggestions)));
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.PaddingTop = 10;
            cell2.PaddingLeft = 0;
            cell2.PaddingRight = 0;
            cell2.DisableBorderSide(Rectangle.BOX);
            tbl2.AddCell(cell2);

            cell2 = new PdfPCell(new Phrase("शिक्षक के हस्ताक्षर"));   //Teacher's Signature
            cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell2.PaddingTop = 30;
            cell2.PaddingLeft = 0;
            cell2.PaddingRight = 0;
            cell2.DisableBorderSide(Rectangle.BOX);
            tbl2.AddCell(cell2);
            doc.Add(tbl2);


            // Setting Document properties e.g.
            // 1. Title
            // 2. Subject
            // 3. Keywords
            // 4. Creator
            // 5. Author
            // 6. Header
            //doc.AddTitle("Hello World example");
            //doc.AddSubject("This is an Example 4 of Chapter 1 of Book 'iText in Action'");
            //doc.AddKeywords("Metadata, iTextSharp 5.4.4, Chapter 1, Tutorial");
            //doc.AddCreator("iTextSharp 5.4.4");
            //doc.AddAuthor("Debopam Pal");
            //doc.AddHeader("Nothing", "No Header");
            doc.Close();
            return View();
        }
    }
}