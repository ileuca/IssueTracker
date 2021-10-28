﻿using IssueTracker.Models;
using IssueTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace IssueTracker.Controllers
{
    public class ActionController : Controller
    {
        private readonly ActionRepository actionRepository = new ActionRepository();
        private readonly StatusRepository StatusRepository = new StatusRepository();
        public ActionResult Index(Guid IssueId)
        {
            ViewBag.IssueId = IssueId;
            List<ActionModel> actionsForIssue = actionRepository.GetActionsByIssueId(IssueId);
            return View(actionsForIssue);
        }
        public ActionResult Create(Guid IssueId)
        {
            ViewBag.IssueId = IssueId;
            return View();
        }
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
                return RedirectToAction("Index",new { IssueId });
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(Guid ActionId)
        {
            ActionModel actionToEdit = actionRepository.GetActionById(ActionId);
            return View(actionToEdit);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            ActionModel editedAction = new ActionModel();
            try
            {
                UpdateModel(editedAction);
                actionRepository.UpdateAction(editedAction);
                return RedirectToAction("Index",new { editedAction.IssueId});
            }
            catch
            {
                return View(editedAction);
            }
        }
        public ActionResult Delete(Guid ActionId)
        {
            ActionModel actiontoDelete = actionRepository.GetActionById(ActionId);
            return View(actiontoDelete);
        }
        [HttpPost]
        public ActionResult Delete(Guid ActionId,FormCollection collection)
        {
            ActionModel actionToDelete = actionRepository.GetActionById(ActionId);
            try
            {   
                Guid IssueId = actionToDelete.IssueId;
                actionRepository.DeleteAction(actionToDelete);
                return RedirectToAction("Index",new { IssueId });
            }
            catch
            {
                return View();
            }
        }
    }
}
