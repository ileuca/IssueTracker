using IssueTracker.Models;
using IssueTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IssueTracker.Controllers
{
    public class ActionController : Controller
    {
        private ActionRepository actionRepository = new ActionRepository();
        private StatusRepository StatusRepository = new StatusRepository();
        // GET: Action
        public ActionResult Index(Guid IssueId)
        {
            List<ActionModel> actionsForIssue = new List<ActionModel>();
            ViewBag.IssueId = IssueId;
            actionsForIssue = actionRepository.GetActionsByIssueId(IssueId);
            return View(actionsForIssue);
        }

        // GET: Action/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Action/Create
        public ActionResult Create(Guid IssueId)
        {
            ViewBag.IssueId = IssueId;
            return View();
        }

        // POST: Action/Create
        [HttpPost]
        public ActionResult Create(Guid IssueId, FormCollection collection)
        {
            ActionModel actionModel = new ActionModel();
            try
            {
                UpdateModel(actionModel);
                actionModel.IssueId = IssueId;
                //de modificat sa verifice datele pentru status
                actionModel.StatusId = StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "In Progress").StatusId;
                actionRepository.CreateAction(actionModel);
                return RedirectToAction("Index",new { IssueId = IssueId});
            }
            catch
            {
                return View();
            }
        }

        // GET: Action/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Action/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Action/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Action/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
