using IssueTracker.Models;
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
        private readonly ProjectRepository projectRepository = new ProjectRepository();
        private readonly UserTeamRoleRepository userTeamRoleRepository = new UserTeamRoleRepository();
        private readonly ActionRepository actionRepository = new ActionRepository();
        public ActionResult Index(Guid TeamId, Guid ProjectId,string searchString)
        {
            try
            {
                ViewBag.TeamId = TeamId;
                ViewBag.ProjectId = ProjectId;
                ViewBag.ProjectName = projectRepository.GetProjectByProjectId(ProjectId).ProjectName;

                var currentUser = userRepository.GetCurrentUser();
                bool userIsMasterInTeam = teamViewRepository.GetAllTeamGroups().FindAll(x => x.UserId == currentUser.UserId)
                    .FindAll(x => x.UserTeamRoleId == userTeamRoleRepository.GetTeamRoleModels()
                    .Find(y => y.UserTeamRoleName == "Master").UserTeamRoleId).Exists(x => x.TeamId == TeamId);
                ViewBag.UserIsMaster = userIsMasterInTeam;


                List<IssueModel> issuesToBeReturned = new List<IssueModel>();

                List<IssueModel> allIssuesInThisProject = issueRepository.GetIssuesByProjectId(ProjectId);

                if (userIsMasterInTeam)
                {
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        allIssuesInThisProject = allIssuesInThisProject.Where(i => i.IssueName.Contains(searchString)
                                                        || i.IssueDescription.Contains(searchString)).ToList();
                    }
                    return View("Index", allIssuesInThisProject);
                }
                else
                {
                    foreach (var issue in allIssuesInThisProject)
                    {
                        if (issue.EndDate < DateTime.Now)
                        {
                            issue.StatusId = StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "Delayed").StatusId;
                            issueRepository.UpdateIssue(issue);
                        }
                        if (currentUser.UserId == issue.UserId)
                        {
                            issuesToBeReturned.Add(issue);
                        }
                    }
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        issuesToBeReturned = issuesToBeReturned.Where(i => i.IssueName.Contains(searchString)
                                                        || i.IssueDescription.Contains(searchString)).ToList();
                    }
                    return View("Index", issuesToBeReturned);
                }

            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Details(Guid ProjectId)
        {
            try
            {
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Create(Guid TeamId, Guid ProjectId)
        {
            try
            {
                List<UserModel> userList = new List<UserModel>();
                var teamViewModels = teamViewRepository.GetTeamViewModelsByTeamId(TeamId);
                var teamViewModelForCurrentUser = teamViewModels.Find(x => x.UserId == userRepository.GetCurrentUser().UserId);
                var masterId = userTeamRoleRepository.GetTeamRoleModels().Find(x => x.UserTeamRoleName == "Master").UserTeamRoleId;

                if (teamViewModelForCurrentUser.TeamRoleId == masterId)
                {
                    userList = teamViewRepository.GetUsersByTeamId(TeamId);
                }
                else
                {
                    userList.Add(userRepository.GetUserById(userRepository.GetCurrentUser().UserId));
                }


                ViewBag.UserFromTeam = userList;
                ViewBag.ProjectId = ProjectId;
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
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
                    ProjectModel projectModel = projectRepository.GetProjectByProjectId(ProjectId);
                    if (projectModel.StartDate <= issueModel.StartDate && projectModel.EndDate >= issueModel.EndDate)
                    {
                        if (issueModel.StartDate > DateTime.Now && issueModel.EndDate > DateTime.Now)
                        {
                            issueModel.StatusId = StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "Not Started").StatusId;
                        }
                        else if (issueModel.StartDate <= DateTime.Now && issueModel.EndDate > DateTime.Now)
                        {
                            issueModel.StatusId = StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "In Progress").StatusId;
                        }
                        issueRepository.CreateIssue(issueModel);
                        return RedirectToAction("Index", new { TeamId, ProjectId });
                    }
                    else
                    {
                        return View("_error");
                    }
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
            if (ModelState.IsValid)
            {
                try
                {
                    List<UserModel> userList = teamViewRepository.GetUsersByTeamId(issueRepository.GetTeamIdByIssueId(IssueId));
                    ViewBag.UserFromTeam = userList;
                    ViewBag.TeamId = projectRepository.GetProjectByProjectId(issueRepository.GetIssueById(IssueId).ProjectId).TeamId;
                    ViewBag.ProjectId = issueRepository.GetIssueById(IssueId).ProjectId;
                    IssueModel issueModel = issueRepository.GetIssueById(IssueId);
                    return View(issueModel);
                }
                catch
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            IssueModel issueModel = new IssueModel();
            try
            {
                UpdateModel(issueModel);
                ProjectModel projectModel = projectRepository.GetProjectByProjectId(issueModel.ProjectId);
                if (projectModel.StartDate <= issueModel.StartDate && projectModel.EndDate >= issueModel.EndDate)
                {
                    if (issueModel.StartDate > DateTime.Now && issueModel.EndDate > DateTime.Now)
                    {
                        issueModel.StatusId = StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "Not Started").StatusId;
                    }
                    else if (issueModel.StartDate <= DateTime.Now && issueModel.EndDate > DateTime.Now)
                    {
                        issueModel.StatusId = StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "In Progress").StatusId;
                    }
                    issueRepository.UpdateIssue(issueModel);
                    return RedirectToAction("Index", new { TeamId = issueRepository.GetTeamIdByIssueId(issueModel.IssueId), issueModel.ProjectId });
                }
                else
                {
                    return View("_error");
                }

            }
            catch
            {
                return View(issueModel);
            }
        }
        public ActionResult Delete(Guid IssueId)
        {
            try
            {
                ViewBag.TeamId = projectRepository.GetProjectByProjectId(issueRepository.GetIssueById(IssueId).ProjectId).TeamId;
                ViewBag.ProjectId = issueRepository.GetIssueById(IssueId).ProjectId;
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
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
        public ActionResult SetAsDone(Guid IssueId)
        {
            IssueModel issueModel = issueRepository.GetIssueById(IssueId);
            try
            {
                issueModel.StatusId = StatusRepository.GetStatuses().FirstOrDefault(x => x.StatusName == "Finished").StatusId;
                issueRepository.UpdateIssue(issueModel);
                List<ActionModel> actionsForThisIssue = actionRepository.GetActionsByIssueId(IssueId);
                foreach(var action in actionsForThisIssue)
                {
                    action.StatusId = issueModel.StatusId;
                    actionRepository.UpdateAction(action);
                }
                return RedirectToAction("Index", new { projectRepository.GetProjectByProjectId(issueModel.ProjectId).TeamId, issueModel.ProjectId });
            }
            catch
            {
                return View();
            }
        }
    }
}
