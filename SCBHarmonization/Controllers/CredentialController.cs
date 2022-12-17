using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
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
    public class CredentialController : Controller
    {
        private SCBDBEntities db = new SCBDBEntities();
        HttpClientHelper modelbuilder1 = new HttpClientHelper();

        private readonly IDataAggregationInterface dataAggregationInterface1;
        public CredentialController()
        {
        }
        public CredentialController(IDataAggregationInterface dataAggregationInterface)
        {
            dataAggregationInterface1 = dataAggregationInterface;
        }
        // GET: Credential
        public ActionResult Index()
        {
            return View(db.Credentials.ToList());
        }

        // GET: Credential/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credential credential = db.Credentials.Find(id);
            if (credential == null)
            {
                return HttpNotFound();
            }
            return View(credential);
        }

        // GET: Credential/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Credential/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,USERNAME,KEY,IV,Date")] Credential credential)
        {
            if (ModelState.IsValid)
            {
                db.Credentials.Add(credential);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(credential);
        }

        // GET: Credential/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credential credential = db.Credentials.Find(id);
            if (credential == null)
            {
                return HttpNotFound();
            }
            return View(credential);
        }

        // POST: Credential/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,USERNAME,KEY,IV,Date")] Credential credential)
        {
            if (ModelState.IsValid)
            {
                db.Entry(credential).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(credential);
        }

        // GET: Credential/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credential credential = db.Credentials.Find(id);
            if (credential == null)
            {
                return HttpNotFound();
            }
            return View(credential);
        }

        // POST: Credential/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Credential credential = db.Credentials.Find(id);
            db.Credentials.Remove(credential);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        public async Task<ActionResult> Ping()
        {
            using (var serviceScope = modelbuilder1.Modelbuilder().Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                //All services
                var dataAggregationServices = services.GetRequiredService<IDataAggregationInterface>();

                //PING
                 var pingResponse = await dataAggregationServices.PingAsync();

                // ViewBag.Message = "Successfully";
                ViewBag.Status = pingResponse.Status;
                ViewBag.Message = pingResponse.Message;
                ViewBag.Description = pingResponse.Description;

                return View();
            }
        }
      


        public async Task<ActionResult> ResetCredentials()
        {
            using (var serviceScope = modelbuilder1.Modelbuilder().Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                //All services
                var dataAggregationServices = services.GetRequiredService<IDataAggregationInterface>();

                //Reset
                Credential credential = db.Credentials.Find(1);

                ResetModelRequest resetModelRequest = new ResetModelRequest { Username = credential.USERNAME };

                var resetModelResponseObject = await dataAggregationServices.ResetCredentialAsync(resetModelRequest);

                ViewBag.Status = resetModelResponseObject.Status;
                ViewBag.Message = resetModelResponseObject.Message;
                ViewBag.Description = resetModelResponseObject.Description;
                ViewBag.IV = resetModelResponseObject.IV;
                ViewBag.Key = resetModelResponseObject.Key;


            }
            return View();
        }

    }
}
