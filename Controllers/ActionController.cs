using IssueTracker.Models;
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
        private readonly IssueRepository issueRepository = new IssueRepository();
        private readonly ProjectRepository projectRepository = new ProjectRepository();
        public ActionResult Index(Guid IssueId,string searchString)
        {
            try
            {
                ViewBag.IssueId = IssueId;
                ViewBag.IssueName = issueRepository.GetIssueById(IssueId).IssueName;
                ViewBag.TeamId = projectRepository.GetProjectByProjectId(issueRepository.GetIssueById(IssueId).ProjectId).TeamId;
                ViewBag.ProjectId = issueRepository.GetIssueById(IssueId).ProjectId;
                List<ActionModel> actionsForIssue = actionRepository.GetActionsByIssueId(IssueId);
                foreach (var action in actionsForIssue)
                {
                    if (action.EndDate < DateTime.Now && action.StatusId != StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "Finished").StatusId)
                    {
                        action.StatusId = StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "Delayed").StatusId;
                        actionRepository.UpdateAction(action);
                    }
                }
                if (!string.IsNullOrEmpty(searchString))
                {
                    actionsForIssue = actionsForIssue.Where(a => a.ActionName.Contains(searchString)
                                                    || a.ActionDescription.Contains(searchString)).ToList();
                }
                return View(actionsForIssue);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Create(Guid IssueId)
        {
            try
            {
                ViewBag.IssueId = IssueId;
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult Create(Guid IssueId, FormCollection collection)
        {
            ActionModel actionModel = new ActionModel();
            try
            {
                UpdateModel(actionModel);
                actionModel.IssueId = IssueId;
                IssueModel issueModel = issueRepository.GetIssueById(IssueId);
                if (issueModel.StartDate <= actionModel.StartDate && issueModel.EndDate >= actionModel.EndDate)
                {
                    if (actionModel.StartDate.Value.Date > DateTime.Now.Date && actionModel.EndDate.Value > DateTime.Now.Date)
                    {
                        actionModel.StatusId = StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "Not Started").StatusId;
                    }
                    else if (actionModel.StartDate.Value.Date <= DateTime.Now.Date && actionModel.EndDate.Value.Date > DateTime.Now.Date)
                    {
                        actionModel.StatusId = StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "In Progress").StatusId;
                    }
                    actionRepository.CreateAction(actionModel);
                    return RedirectToAction("Index", new { IssueId });
                }
                else
                {
                    return View("_error");
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(Guid ActionId)
        {try
            {
                ViewBag.IssueId = actionRepository.GetActionById(ActionId).IssueId;
                ActionModel actionToEdit = actionRepository.GetActionById(ActionId);
                return View(actionToEdit);
            }
            catch 
            { 
                return RedirectToAction("Index", "Home"); 
            }
        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            ActionModel actionModel = new ActionModel();
            try
            {
                UpdateModel(actionModel);
                IssueModel issueModel = issueRepository.GetIssueById(actionModel.IssueId);
                if (issueModel.StartDate <= actionModel.StartDate && issueModel.EndDate >= actionModel.EndDate)
                {
                    if (actionModel.StartDate > DateTime.Now && actionModel.EndDate > DateTime.Now)
                    {
                        actionModel.StatusId = StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "Not Started").StatusId;
                    }
                    else if (actionModel.StartDate <= DateTime.Now && actionModel.EndDate > DateTime.Now)
                    {
                        actionModel.StatusId = StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "In Progress").StatusId;
                    }
                    actionRepository.UpdateAction(actionModel);
                    return RedirectToAction("Index", new { actionModel.IssueId });
                }
                else
                {
                    return View("_error");
                }

            }
            catch
            {
                return View(actionModel);
            }
        }
        public ActionResult Delete(Guid ActionId)
        {
            try
            {
                ActionModel actiontoDelete = actionRepository.GetActionById(ActionId);
                return View(actiontoDelete);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
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
        public ActionResult SetAsDone(Guid ActionId)
        {
            ActionModel actionModel = actionRepository.GetActionById(ActionId);
            try
            {
                actionModel.StatusId = StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "Finished").StatusId;
                actionRepository.UpdateAction(actionModel);
                return RedirectToAction("Index", new { actionModel.IssueId });
            }
            catch
            {
                return View();
            }
        }
    }
}
