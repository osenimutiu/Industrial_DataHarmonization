using ClosedXML.Excel;
using Rotativa;
using SCBHarmonization.Models;
using SCBHarmonization.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCBHarmonization.Controllers
{
    public class LoadingGuideLineController : Controller
    {
        private SCBDBEntities objdb;
        public LoadingGuideLineController()
        {
            objdb = new SCBDBEntities();
        }
        // GET: LoadingGuide
        public ActionResult Index()
        {
            var result = (from e in objdb.tbl_LoadingGiudeLine
                          select new tbl_LoadingGiudeLineViewModel()
                          {
                              ParameterName = e.ParameterName,
                              Description = e.Description,
                              Comment = e.Comment,
                              Id = e.Id
                          }).ToList();
            return View(result);
        }

        public ActionResult LoadingGuideines()
        {
            var report = new ActionAsPdf("Index");
            return report;
        }
    }
}