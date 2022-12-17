using Microsoft.Extensions.DependencyInjection;
using SCBHarmonization.Models;
using SCBHarmonization.NibssClass;
using SCBHarmonization.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity;

namespace SCBHarmonization.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private SCBDBEntities obj;
        // private  readonly IExceptionManager _exceptionManager;
        HttpClientHelper modelbuilder1 = new HttpClientHelper();

        private readonly IDataAggregationInterface dataAggregationInterface1;
       
        public TransactionController(IDataAggregationInterface dataAggregationInterface)
        {
            dataAggregationInterface1 = dataAggregationInterface;
        }
        public TransactionController()
        {
            obj = new SCBDBEntities();

        }
        // GET: Transaction
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(tbl_TransactionViewModel objViewModel)
        {
            string message = string.Empty;
            if (objViewModel.Id == 0)
            {
                tbl_TransactionCount objTrans = new tbl_TransactionCount()
                {
                    SucTransCount = objViewModel.SucTransCount,
                    FailTransCount = objViewModel.FailTransCount,
                    TransDate = DateTime.Now
                };
                obj.tbl_TransactionCount.Add(objTrans);
                message = "Saved";
            }
            obj.SaveChanges();
            return Json(new { message = "Successfully " + message, success = true }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetAll()
        {
            IEnumerable<tbl_TransactionDetailsViewModel> listOfDetailsViewModels =
                (
                from objTrans in obj.tbl_TransactionCount
                select new tbl_TransactionDetailsViewModel()
                {
                    SucTransCount = objTrans.SucTransCount,
                    FailTransCount = objTrans.FailTransCount,
                    TransDate = objTrans.TransDate,
                    Id = objTrans.Id
                }).ToList();
            return PartialView("_TransactionDetailsPartial", listOfDetailsViewModels);
        }

        [HttpGet]
        public async Task<ActionResult> GetTransactionTypeList()
        {

            using (var serviceScope = modelbuilder1.Modelbuilder().Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                //All services
                var dataAggregationServices = services.GetRequiredService<IDataAggregationInterface>();
                //GET ALL TRANSACTION TYPES
                var allTransactions = await dataAggregationServices.GetAllTransactionTypeAsync();

                //var grouped = allTransactions.GroupBy(a => a.TransactionGroupName);

                //foreach (var singleGroup in grouped)
                //{
                //    List<string> grouplist = new List<string>();
                //    grouplist.Add(($"Group Name: {singleGroup.Key}"));
                //    string[] group = grouplist.ToArray();
                //    //string[] group = ($"Group Name: {singleGroup.Key}");
                //    foreach (var groupItems in singleGroup)
                //    {
                //        List<string> codelist = new List<string>();
                //        codelist.Add(($"Code:{groupItems.Code}  Name:{groupItems.Name}"));
                //        string[] code = codelist.ToArray();
                //        //var code = ($"Code:{groupItems.Code}  Name:{groupItems.Name}");

                //        ViewBag.Message = group + code;
                //    }

                //}
                //ViewData = grouped.ToString();
                ViewData["TransactionType"] = allTransactions;

                return View();
            }
        }

        public ActionResult DeleteTransactionStatus(int id)
        {
            tbl_TransactionCount employee = obj.tbl_TransactionCount.Find(id);
            if (employee == null)
            {
                return null;
                //ViewBag.Message = string.Format("No record with 0 id value");
            }

            obj.tbl_TransactionCount.Remove(employee);
            obj.SaveChanges();

            return RedirectToAction("Index");
        }

        //public ActionResult Audit()
        //{
        //    if (User.Identity.IsAuthenticated && User.IsInRole("System Admin"))
        //    {
        //        var auditList = obj.TBL_AUDIT.OrderByDescending(s => s.AUDITID).Take(2000).ToArray();
        //        return View(auditList);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //}

        public ActionResult Audit(string sortOrder, string currentFilter, string search, int? page, DateTime? startDate, DateTime? endDate)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("System Admin"))
            {
                ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var transactionItem = (from s in obj.TBL_AUDIT 
                                   select new AuditViewModel()
                                   {
                                       AUDITID = s.AUDITID,
                                       DATETIME = s.DATETIME,
                                       DEVICENAME = s.DEVICENAME,
                                       DETAIL = s.DETAIL,
                                       IPADDRESS = s.IPADDRESS,
                                       OSNAME = s.OSNAME,
                                       URL = s.URL,
                                       UserId = s.UserId,
                                       UserName = s.UserName
                                   });
            try
            {
                if (search == null && startDate == null && endDate == null)
                {
                    page = 1;
                    search = currentFilter;

                    ViewBag.TotalCount = transactionItem.Count();
                    transactionItem = transactionItem.Take(1000);
                    ViewBag.RecordCount = transactionItem.Count();
                }
                else if (!String.IsNullOrEmpty(search) && startDate == null && endDate == null)
                {
                    page = 1;
                    ViewBag.TotalCount = transactionItem.Count();
                    transactionItem = transactionItem.Where(t => t.UserName.Contains(search));
                    ViewBag.RecordCount = transactionItem.Count();
                }
                else if (String.IsNullOrEmpty(search) && startDate != null && endDate != null)
                {
                    page = 1;
                    ViewBag.TotalCount = transactionItem.Count();
                        transactionItem = transactionItem.Where(r => DbFunctions.TruncateTime(r.DATETIME) >= startDate && DbFunctions.TruncateTime(r.DATETIME) <= endDate);
                        ViewBag.RecordCount = transactionItem.Count();
                }
                 else if (!String.IsNullOrEmpty(search) && startDate != null && endDate != null)
                 {
                        page = 1;
                        ViewBag.TotalCount = transactionItem.Count();
                        transactionItem = transactionItem.Where(r => r.UserName.Contains(search) && DbFunctions.TruncateTime(r.DATETIME) >= startDate && DbFunctions.TruncateTime(r.DATETIME) <= endDate);
                        ViewBag.RecordCount = transactionItem.Count();
                 }

                    switch (sortOrder)
                {
                    case "name_desc":
                        transactionItem = transactionItem.OrderByDescending(s => s.AUDITID);
                        break;
                    case "Date":
                        transactionItem = transactionItem.OrderByDescending(s => s.AUDITID);
                        break;
                    case "date_desc":
                        transactionItem = transactionItem.OrderByDescending(s => s.AUDITID);
                        break;
                    default: 
                        transactionItem = transactionItem.OrderByDescending(s => s.AUDITID);
                        break;
                }
            }
            catch (Exception)
            {

            }
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(transactionItem.ToPagedList(pageNumber, pageSize));
        }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}