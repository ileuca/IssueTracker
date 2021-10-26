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
        public ActionResult Edit(Guid IssueId)
        {
            IssueModel issueModel = issueRepository.GetIssueById(IssueId);
            return View(issueModel);
        }

        // POST: Issue/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            IssueModel issueModel = new IssueModel();
            try
            {
                UpdateModel(issueModel);
                issueRepository.UpdateIssue(issueModel);
                return RedirectToAction("Index",new { TeamId = issueRepository.GetTeamIdByIssueId(issueModel.IssueId), ProjectId = issueModel.ProjectId});
            }
            catch
            {
                return View(issueModel);
            }
        }

        // GET: Issue/Delete/5
        public ActionResult Delete(Guid IssueId)
        {
            return View();
        }

        // POST: Issue/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid IssueId, FormCollection collection)
        {
            IssueModel issueModel = issueRepository.GetIssueById(IssueId);
            try
            {
                Guid TeamId = issueRepository.GetTeamIdByIssueId(issueModel.IssueId);
                issueRepository.DeleteIssue(issueModel);


                //Issue nu mai exista ca e ster trebuie teamId din alta parte??
                return RedirectToAction("Index", new { TeamId = TeamId , ProjectId = issueModel.ProjectId });
            }
            catch
            {
                return View();
            }
        }
    }
}
