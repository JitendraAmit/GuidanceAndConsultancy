using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GuidanceConsultancy.Areas.Admin.Models.StudentViewModel;
using GuidanceConsultancy.Models.db;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
//using iTextSharp.text.pdf;

namespace GuidanceConsultancy.Controllers
{
    public class PdfController : Controller
    {
       public ActionResult Index()
        {
            //String HindiFont1 = @"LEOPALMHINDI15K710.TTF";
            //string HindiFont1 = GuidanceConsultancy.Helpers.AppConstants.BaseURL + "Assets/admin/freeserif/FreeSerif.ttf";
            string HindiFont1 = GuidanceConsultancy.Helpers.AppConstants.BaseURL + "Assets/admin/MANGAL.TTF";
            //string HindiFont1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
            //PageSize size = PageSize.A4;

            //PdfFontFactory.Register(HindiFont1, "HindiFont1");

            //Error at this line: Font Not Recognized
          //  PdfFont myHindiFont1 = PdfFontFactory.CreateRegisteredFont("HindiFont1", PdfEncodings.IDENTITY_H);

            //BaseFont bf = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\HINN.TTF", BaseFont.IDENTITY_H, true);

            //iTextSharp.text.Font Hindiheadingfonta = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD);

            //PdfPCell Cell15B = new PdfPCell(new Phrase("**fctsUæ**", Hindiheadingfonta));

            PdfFont f = PdfFontFactory.CreateFont(HindiFont1, PdfEncodings.IDENTITY_H);
            //Create Writer
            var autono = Guid.NewGuid().ToString() + "-" + Helpers.Utilities.GetCurrentDateTime().ToString("yyyy-MM-dd-hh-mm-ss");
            string path = Server.MapPath("~/ApplicationData/StudentPdf/" + @autono + ".pdf");
            // Must have write permissions to the path folder
            PdfWriter writer = new PdfWriter(path);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);
            //Paragraph header = new Paragraph("fctsUæ")
              Paragraph header = new Paragraph("मैं शिल्पकार वेलफेयर सोसाइटी का प्राथमिक सदस्य बन चुका हूं। मेरे द्वारा सोसाइटी के उत्थान हेतु अपना योगदान दिया जाएगा। शिल्पकारों के कल्याण हेतु समय.समय पर जो भी कार्यक्रम यधरनाए प्रदर्शनए सम्मेलन आदि द्ध किए जाएंगे उसमें मैं पूर्ण सहभागिता करूंगा")
              //Paragraph header = new Paragraph("\u0915\u093e\u0930 \u092a\u093e\u0930\u094d\u0915\u093f\u0902\u0917")
              .SetTextAlignment(TextAlignment.CENTER)
               .SetFontSize(20);
            document.SetFont(f);
            document.Add(header);
            document.Close();
           
            return View();
        }
    }
}