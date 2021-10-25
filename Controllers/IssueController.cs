using IssueTracker.Models;
using IssueTracker.Models.DBObjects;
using IssueTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IssueTracker.Controllers
{
    public class IssueController : Controller
    {
        private IssueRepository issueRepository = new IssueRepository();
        private UserRepository userRepository = new UserRepository();
        private TeamViewRepository teamViewRepository = new TeamViewRepository();
        private StatusRepository StatusRepository = new StatusRepository();

        // GET: Issue
        public ActionResult Index(Guid TeamId, Guid ProjectId)
        {

            ViewBag.TeamId = TeamId;
            ViewBag.Projectid = ProjectId;
            List<IssueModel> issuesByProjectId = issueRepository.GetIssuesByProjectId(ProjectId);
            return View("Index", issuesByProjectId);
        }

        // GET: Issue/Details/5
        public ActionResult Details(Guid ProjectId)
        {
            return View();
        }

        // GET: Issue/Create
        public ActionResult Create(Guid TeamId)
        {
            List<UserModel> userList = teamViewRepository.GetUsersByTeamId(TeamId);
            ViewBag.UserFromTeam = userList;
            return View();
        }

        // POST: Issue/Create
        [HttpPost]
        public ActionResult Create(Guid ProjectId, Guid TeamId, FormCollection collection)
        {
            IssueModel issueModel = new IssueModel();
            try
            {
                List<UserModel> userList = teamViewRepository.GetUsersByTeamId(TeamId);
                ViewBag.UserFromTeam = userList;
                if (ModelState.IsValid)
                {
                    UpdateModel(issueModel);
                    issueModel.ProjectId = ProjectId;
                    issueModel.StatusId = StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "In Progress").StatusId;
                    issueRepository.CreateIssue(issueModel);

                    return RedirectToAction("Index", new { TeamId = TeamId, ProjectId = ProjectId });
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Issue/Edit/5
        public ActionResult Edit(Guid ProjectId)
        {
            return View();
        }

        // POST: Issue/Edit/5
        [HttpPost]
        public ActionResult Edit(Guid ProjectId, FormCollection collection)
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

        // GET: Issue/Delete/5
        public ActionResult Delete(Guid ProjectId)
        {
            return View();
        }

        // POST: Issue/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid ProjectId, FormCollection collection)
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
