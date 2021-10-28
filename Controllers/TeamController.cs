using IssueTracker.Models;
using IssueTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IssueTracker.Controllers
{
    public class TeamController : Controller
    {
        private readonly Repository.TeamRepository TeamRepository = new Repository.TeamRepository();
        private readonly Repository.TeamViewRepository TeamViewRepository = new Repository.TeamViewRepository();
        private readonly Repository.UserRepository UserRepository = new Repository.UserRepository();
        private readonly Repository.UserTeamRoleRepository UserTeamRoleRepository = new Repository.UserTeamRoleRepository();
        private readonly Repository.TeamGroupRepository TeamGroupRepository = new Repository.TeamGroupRepository();
        public ActionResult Index()
        {
            try
            {
                List<TeamModel> userTeamRoleModels = new List<TeamModel>();
                userTeamRoleModels = TeamRepository.GetTeamsForCurrentUserId(UserRepository.GetCurrentUser().UserId);
                return View("Index", userTeamRoleModels);
            }
            catch
            {
                return View("Index","Home");
            }
        }
        public ActionResult EditTeamMember(Guid TeamId, Guid UserId)
        {
            try
            {
                TeamViewModel teamViewModel = new TeamViewModel();
                teamViewModel = TeamViewRepository.GetTEamViewModelsByTeamId(TeamId).Find(x => x.UserId == UserId);
                ViewBag.CreatedBy = TeamRepository.GetTeamById(TeamId).CreatedBy;
                ViewBag.CurrentUser = UserRepository.GetCurrentUser().UserId;
                return View("EditTeamMember", teamViewModel);
            }
            catch
            {
                return View("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult EditTeamMember(Guid TeamId, Guid UserId, FormCollection formCollection)
        {
            try
            {
                TeamViewModel teamViewModel = new TeamViewModel();
                teamViewModel = TeamViewRepository.GetTEamViewModelsByTeamId(TeamId).Find(x => x.UserId == UserId);
                UpdateModel(teamViewModel);
                TeamViewRepository.UpdateTeamGroup(teamViewModel);
                return RedirectToAction("Details", new { teamViewModel.TeamId });
            }
            catch
            {
                return View();
            }
        }
        public ActionResult AddUser(Guid TeamId)
        {
            try
            {
                List<UserModel> usersToAdd = new List<UserModel>();
                List<TeamGroupsModel> teamGroups = TeamGroupRepository.GetAllTeamGroups().FindAll(x => x.TeamId == TeamId);
                foreach (var user in UserRepository.GetAllUsers())
                {
                    if (!teamGroups.Exists(x => x.UserId == user.UserId))
                    {
                        usersToAdd.Add(user);
                    }
                }
                ViewBag.UsersToAdd = usersToAdd;
                TeamViewModel teamViewModel = new TeamViewModel();
                if (TeamViewRepository.GetTEamViewModelsByTeamId(TeamId).Find(x => x.TeamId == TeamId) != null)
                {
                    teamViewModel.TeamId = TeamViewRepository.GetTEamViewModelsByTeamId(TeamId).Find(x => x.TeamId == TeamId).TeamId;
                    return View("AddUser", teamViewModel);
                }
                else
                {
                    teamViewModel.TeamId = TeamId;
                    return View("AddUser", teamViewModel);
                }
            }
            catch
            {
                return View("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddUser(FormCollection collection)
        {
            try
            {
                TeamViewModel teamViewModel = new TeamViewModel();
                UpdateModel(teamViewModel);
                TeamViewRepository.CreateTeamGroup(teamViewModel);
                return RedirectToAction("Details",new { teamViewModel.TeamId });
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Details(Guid TeamId)
        {
            try
            {
                List<TeamViewModel> teamViewModels = TeamViewRepository.GetTEamViewModelsByTeamId(TeamId);
                ViewBag.CreatedBy = TeamRepository.GetTeamById(TeamId).CreatedBy;
                ViewBag.CurrentUser = UserRepository.GetCurrentUser().UserId;
                if (TeamViewRepository.GetTEamViewModelsByTeamId(TeamId).Count == 0)
                {
                    TeamViewModel teamViewModel = new TeamViewModel
                    {
                        TeamId = TeamId,
                        UserNameSurname = ""
                    };
                    teamViewModels.Add(teamViewModel);
                    return View("Details", teamViewModels);
                }
                return View("Details", teamViewModels);
            }
            catch
            {
                return View("Index", "Home");
            }
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                TeamModel teamModel = new TeamModel();
                UpdateModel(teamModel);
                TeamRepository.CreateTeam(teamModel);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult EditTeam(Guid TeamId)
        {
            try
            {
                TeamModel teamModel = TeamRepository.GetTeamById(TeamId);
                return View("Edit", teamModel);
            }
            catch
            {
                return View("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult EditTeam(Guid TeamId, FormCollection collection)
        {
            try
            {
                TeamModel teamModel = new TeamModel();
                UpdateModel(teamModel);
                TeamRepository.UpdateTeam(teamModel);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult DeleteTeam(Guid TeamId)
        {
            try
            {
                if (TeamRepository.GetTeamsCreatedBy().Find(x => x.TeamId == TeamId) != null)
                {
                    if (UserRepository.GetCurrentUser().UserId == TeamRepository.GetTeamsCreatedBy().Find(x => x.TeamId == TeamId).CreatedBy)
                    {
                        ViewBag.TeamName = TeamRepository.GetTeamsCreatedBy().Find(x => x.TeamId == TeamId).TeamName;
                        return View();
                    }
                    return View();
                }
                return View();
            }
            catch
            {
                return View("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult DeleteTeam(Guid TeamId, FormCollection collection)
        {
            try
            {
                if (TeamRepository.GetTeamsCreatedBy().Find(x => x.TeamId == TeamId) != null)
                {
                    if (UserRepository.GetCurrentUser().UserId == TeamRepository.GetTeamsCreatedBy().Find(x => x.TeamId == TeamId).CreatedBy)
                    {
                        TeamModel teamModel = new TeamModel
                        {
                            TeamId = TeamId
                        };
                        TeamRepository.DeleteTeam(teamModel);

                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult DeleteUserFromTeam(Guid TeamId, Guid UserId, Guid TeamRoleId)
        {
            try
            {
                ViewBag.UserName = UserRepository.GetUserById(UserId).UserName;
                ViewBag.TeamRoleName = UserTeamRoleRepository.GetTeamRoleModels().Find(x => x.UserTeamRoleId == TeamRoleId).UserTeamRoleName;
                return View();
            }
            catch
            {
                return View("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult DeleteUserFromTeam(Guid TeamId, Guid UserId, Guid TeamRoleId, FormCollection collection)
        {
            try
            {
                TeamViewModel teamViewModel = new TeamViewModel
                {
                    UserId = UserId,
                    TeamId = TeamId,
                    TeamRoleId = TeamRoleId
                };
                TeamGroupRepository.DeleteTeamGroup(teamViewModel);

                return RedirectToAction("Details", new { teamViewModel.TeamId });
            }
            catch
            {
                return View();
            }
        }
    }
}
