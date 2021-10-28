﻿using IssueTracker.Models;
using IssueTracker.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace IssueTracker.Controllers
{
    public class IssueController : Controller
    {
        private readonly IssueRepository issueRepository = new IssueRepository();
        private readonly UserRepository userRepository = new UserRepository();
        private readonly TeamViewRepository teamViewRepository = new TeamViewRepository();
        private readonly StatusRepository StatusRepository = new StatusRepository();
        public ActionResult Index(Guid TeamId, Guid ProjectId)
        {
            ViewBag.TeamId = TeamId;
            ViewBag.Projectid = ProjectId;
            List<IssueModel> issuesByProjectId = issueRepository.GetIssuesByProjectId(ProjectId);
            return View("Index", issuesByProjectId);
        }
        public ActionResult Details(Guid ProjectId)
        {
            return View();
        }
        public ActionResult Create(Guid TeamId)
        {
            List<UserModel> userList = teamViewRepository.GetUsersByTeamId(TeamId);
            ViewBag.UserFromTeam = userList;
            return View();
        }
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
                    return RedirectToAction("Index", new { TeamId, ProjectId });
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(Guid IssueId)
        {
            IssueModel issueModel = issueRepository.GetIssueById(IssueId);
            return View(issueModel);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            IssueModel issueModel = new IssueModel();
            try
            {
                UpdateModel(issueModel);
                issueRepository.UpdateIssue(issueModel);
                return RedirectToAction("Index",new { TeamId = issueRepository.GetTeamIdByIssueId(issueModel.IssueId), issueModel.ProjectId});
            }
            catch
            {
                return View(issueModel);
            }
        }
        public ActionResult Delete(Guid IssueId)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(Guid IssueId, FormCollection collection)
        {
            IssueModel issueModel = issueRepository.GetIssueById(IssueId);
            try
            {
                Guid TeamId = issueRepository.GetTeamIdByIssueId(issueModel.IssueId);
                issueRepository.DeleteIssue(issueModel);
                return RedirectToAction("Index", new { TeamId, issueModel.ProjectId });
            }
            catch
            {
                return View();
            }
        }
    }
}
