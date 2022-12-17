using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SCBHarmonization.Models;
using SCBHarmonization.NibssClass;
using SCBHarmonization.NibssModels;

namespace SCBHarmonization.Controllers
{
    [Authorize]
    public class TransactionStatusController : Controller
    {
        private SCBDBEntities db = new SCBDBEntities();
        HttpClientHelper modelbuilder1 = new HttpClientHelper();
        private readonly IDataAggregationInterface dataAggregationInterface1;

        public TransactionStatusController()
        {
        }
        public TransactionStatusController(IDataAggregationInterface dataAggregationInterface)
        {
            dataAggregationInterface1 = dataAggregationInterface;
        }

        // GET: TransactionStatus
        public ActionResult Index()
        {
            return View(db.TransactionStatuses.ToList());
        }

        // GET: TransactionStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionStatus transactionStatus = db.TransactionStatuses.Find(id);
            if (transactionStatus == null)
            {
                return HttpNotFound();
            }
            return View(transactionStatus);
        }

        // GET: TransactionStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TransactionStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TenantId,Status,Message,Description,Data,SendDate")] TransactionStatus transactionStatus)
        {
            if (ModelState.IsValid)
            {
                db.TransactionStatuses.Add(transactionStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(transactionStatus);
        }

        // GET: TransactionStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionStatus transactionStatus = db.TransactionStatuses.Find(id);
            if (transactionStatus == null)
            {
                return HttpNotFound();
            }
            return View(transactionStatus);
        }

        // POST: TransactionStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TenantId,Status,Message,Description,Data,SendDate")] TransactionStatus transactionStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transactionStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transactionStatus);
        }

        // GET: TransactionStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionStatus transactionStatus = db.TransactionStatuses.Find(id);
            if (transactionStatus == null)
            {
                return HttpNotFound();
            }
            return View(transactionStatus);
        }

        // POST: TransactionStatus/Delete/5
        [HttpPost, ActionName("Delete")]
       // [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TransactionStatus transactionStatus = db.TransactionStatuses.Find(id);
            db.TransactionStatuses.Remove(transactionStatus);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> SendTransaction()
        {
            using (var serviceScope = modelbuilder1.Modelbuilder().Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                //All services
                var dataAggregationServices = services.GetRequiredService<IDataAggregationInterface>();
                var transactionItem = from s in db.tbl_PreHarmonization select s;
                //var SendDataResponse;
                foreach (var transactionsItem in transactionItem)
                {
                    TransactionRequest transactionRequest = new TransactionRequest
                    {
                        amount = transactionsItem.Amount,
                        vat = transactionsItem.Vat,
                        fee = transactionsItem.Fee,
                        transID = transactionsItem.TransID,
                        //new Random().Next(1000000, 9999999).ToString(),
                        srcAcctNo = transactionsItem.SrcAcctNo,
                        srcInstCode = transactionsItem.SrcInstCode,
                        srcInstBranchCode = transactionsItem.SrcInstBranchCode,
                        srcInstType = transactionsItem.SrcInstType,
                        srcInstUniqueID = transactionsItem.SrcInstUniqueID,
                        destAcctNo = transactionsItem.DestAcctNo,
                        destInstCode = transactionsItem.DestInstCode,
                        destInstBranchCode = transactionsItem.DestInstBranchCode,
                        destInstType = transactionsItem.DestInstType,
                        destInstUniqueID = transactionsItem.DestInstUniqueID,
                        bankIncome = transactionsItem.BankIncome,
                        transDate = transactionsItem.TransDate,
                        //DateTime.Now.ToString("dd-MM-yyy HH:mm:ss", System.Globalization.CultureInfo.GetCultureInfo("en-US")),
                        psspParty = transactionsItem.PsspParty,
                        accountType = transactionsItem.AccountType,
                        accountClass = transactionsItem.AccountClass,
                        accountDesignation = transactionsItem.AccountDesignation,
                        currency = transactionsItem.Currency,
                        paymentType = transactionsItem.PaymentType,
                        channel = transactionsItem.Channels,
                        transactionTypeCode = transactionsItem.TransactionTypeCode,
                        cyberSecurityLevyExempt = transactionsItem.CypherSecurityLevyExempt,
                        pepDesignatedAccount = transactionsItem.PepDesignatedAccount,
                        stampdutyExempt = transactionsItem.StampDutyExempt,
                        inflow = transactionsItem.Inflow,
                        emtl = transactionsItem.Emtl,
                        receiverLocation = transactionsItem.ReceiverLocation
                    };
                    Credential credential = db.Credentials.Find(1);

                    string username = credential.USERNAME;
                    string iv = credential.IV;
                    string key = credential.KEY;

                    var SendDataResponse = await dataAggregationServices.SendTransactionDataAsync(transactionRequest, (iv: iv, key: key, username: username));
                   //var sendDataResponse = await 
                   // SendDataResponse.Message = "SUCCESSFUL";
                    TransactionStatusController savestatus = new TransactionStatusController();
                    TransactionStatus transactionStatus = new TransactionStatus();
                    transactionStatus.Message = SendDataResponse.Message;
                    transactionStatus.Status = SendDataResponse.Status;
                    transactionStatus.Description = SendDataResponse.Description;
                    transactionStatus.Data = SendDataResponse.Data;
                    transactionStatus.SendDate = DateTime.Now;
                    ViewBag.Status = SendDataResponse.Status;
                    ViewBag.Message = SendDataResponse.Message;
                    ViewBag.Description = SendDataResponse.Description;
                    ViewBag.Data = SendDataResponse.Data;

                    savestatus.Create(transactionStatus);

                }

                return View();
            }
        }

        public async Task<ActionResult> SendTransactionList()
        {
            using (var serviceScope = modelbuilder1.Modelbuilder().Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                //All services
                var dataAggregationServices = services.GetRequiredService<IDataAggregationInterface>();
                var transactionItem = (from s in db.tbl_PreHarmonization select new TransactionRequest()
                {
                    amount = s.Amount,
                    vat = s.Vat,
                    fee = s.Fee,
                    transID = s.TransID,
                    srcAcctNo = s.SrcAcctNo,
                    srcInstCode = s.SrcInstCode,
                    srcInstBranchCode = s.SrcInstBranchCode,
                    srcInstType = s.SrcInstType,
                    srcInstUniqueID = s.SrcInstUniqueID,
                    destAcctNo = s.DestAcctNo,
                    destInstCode = s.DestInstCode,
                    destInstBranchCode = s.DestInstBranchCode,
                    destInstType = s.DestInstType,
                    destInstUniqueID = s.DestInstUniqueID,
                    bankIncome = s.BankIncome,
                    transDate = s.TransDate,
                    psspParty = s.PsspParty,
                    accountType = s.AccountType,
                    accountClass = s.AccountClass,
                    accountDesignation = s.AccountDesignation,
                    currency = s.Currency,
                    paymentType = s.PaymentType,
                    channel = s.Channels,
                    transactionTypeCode = s.TransactionTypeCode,
                    cyberSecurityLevyExempt = s.CypherSecurityLevyExempt,
                    pepDesignatedAccount = s.PepDesignatedAccount,
                    stampdutyExempt = s.StampDutyExempt,
                    inflow = s.Inflow,
                    emtl = s.Emtl,
                    receiverLocation = s.ReceiverLocation
                }).ToList();

                Credential credential = db.Credentials.Find(1);

                string username = credential.USERNAME;
                string iv = credential.IV;
                string key = credential.KEY;

                var SendDataResponse = await dataAggregationServices.SendTransactionDataListAsync(transactionItem, (iv: iv, key: key, username: username));
                //var sendDataResponse = await 
                // SendDataResponse.Message = "SUCCESSFUL";
                TransactionStatusController savestatus = new TransactionStatusController();
                TransactionStatus transactionStatus = new TransactionStatus();
                transactionStatus.Message = SendDataResponse.Message;
                transactionStatus.Status = SendDataResponse.Status;
                transactionStatus.Description = SendDataResponse.Description;
                transactionStatus.Data = SendDataResponse.Data;
                transactionStatus.SendDate = DateTime.Now;
                ViewBag.Status = SendDataResponse.Status;
                ViewBag.Message = SendDataResponse.Message;
                ViewBag.Description = SendDataResponse.Description;
                ViewBag.Data = SendDataResponse.Data;
                savestatus.Create(transactionStatus);
               
                return View();
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
