using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using CodeKata.Models;
using System.Text;

namespace CodeKata.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult ProcessFile()
        {


            if (Request.Files.Count > 0)
            {
                try
                {
                    // Get all files from Request object
                    HttpFileCollectionBase files = Request.Files;

                    for (int i = 0; i < files.Count; i++)
                    {

                        HttpPostedFileBase file = files[i];
                        string fname="DataUploaded";
                       
                        // Get the complete folder path and store the file inside it.
                        var FolderPath = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(FolderPath))
                        {
                            // Try to create the directory.
                            DirectoryInfo di = Directory.CreateDirectory(FolderPath);
                        }
                        
                        fname = Path.Combine(FolderPath, fname);
                        file.SaveAs(fname);
                       
                    }

                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred.Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }

        }
        public List<Driver> GetDrivers(List<string> logList)
        {

            logList = logList.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            List<Driver> drivers = new List<Driver>();
            List<Trip> trips = new List<Trip>();
            List<string> tripsInfo = logList.Where(s => s.Contains("Trip")).ToList();
            trips = tripsInfo.Select(r => r.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                           .Select(r => new Trip
                           {
                               driverName = (r[1]).ToString(),
                               startTime = TimeSpan.Parse(r[2]),
                               endTime = TimeSpan.Parse(r[3]),
                               milesDriven = double.Parse((r[4])),
                               hoursDriven = (TimeSpan.Parse(r[3]) - TimeSpan.Parse(r[2])).TotalHours,
                               speed = double.Parse((r[4])) / (TimeSpan.Parse(r[3]) - TimeSpan.Parse(r[2])).TotalHours
                           })
                            .Where(r => r.speed > 5 && r.speed < 100 && r.startTime < r.endTime)
                            .ToList();


            foreach (string d in logList.Where(s => s.Contains("Driver")).Distinct().ToList())
            {
                Driver dr = new Driver();
                dr.driverName = d.Replace("Driver", "").Trim();
                dr.totalMiles = Math.Round((from x in trips where x.driverName == dr.driverName select x.milesDriven).Sum());
                dr.totalHours = Math.Round((from x in trips where x.driverName == dr.driverName select x.hoursDriven).Sum(),2);
                dr.averageSpeed = Math.Round((dr.totalMiles == 0 || dr.totalHours == 0) ? 0 : dr.totalMiles / dr.totalHours);
                
                drivers.Add(dr);
            }


            return drivers;
        }

        public ActionResult SearchTable()
        {
            try
            {
               
         var fileContents = System.IO.File.ReadAllLines(Server.MapPath(@"~/Uploads/DataUploaded"));
                var logList = new List<string>(fileContents);
                List <Driver> drivers = (GetDrivers(logList)).OrderByDescending(o => o.totalMiles).ToList();
                return View(drivers);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }
        }
        public FileStreamResult CreateReportFile()
        {
            var fileContents = System.IO.File.ReadAllLines(Server.MapPath(@"~/Uploads/DataUploaded"));
            var logList = new List<string>(fileContents);
            List<Driver> drivers = (GetDrivers(logList)).OrderByDescending(o => o.totalMiles).ToList();
            List<string> OutputLines = new List<string>();

            StringBuilder sb = new StringBuilder();
            foreach (Driver s in drivers)
            {
                if (s.totalMiles > 0)
                    sb.AppendLine(s.driverName + ": " + s.totalMiles + "miles  @" + s.averageSpeed + " mph");
                else
                    sb.AppendLine(s.driverName + ": " + s.totalMiles + "miles");

            }

            var string_with_your_data = sb.ToString();
            var byteArray = Encoding.ASCII.GetBytes(string_with_your_data);
            var stream = new MemoryStream(byteArray);
            return File(stream, "text/plain", "Report Drivers-Trips " + ".txt");
        }


}
}