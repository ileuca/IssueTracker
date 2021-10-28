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
            TeamViewModel teamViewModel = new TeamViewModel();
            teamViewModel = TeamViewRepository.GetTEamViewModelsByTeamId(TeamId).Find(x => x.UserId == UserId);
            ViewBag.CreatedBy = TeamRepository.GetTeamById(TeamId).CreatedBy;
            ViewBag.CurrentUser = UserRepository.GetCurrentUser().UserId;
            return View("EditTeamMember",teamViewModel);
        }
        [HttpPost]
        public ActionResult EditTeamMember(Guid TeamId, Guid UserId, FormCollection formCollection)
        {
            TeamViewModel teamViewModel = new TeamViewModel();
            teamViewModel = TeamViewRepository.GetTEamViewModelsByTeamId(TeamId).Find(x => x.UserId == UserId);
            UpdateModel(teamViewModel);
            TeamViewRepository.UpdateTeamGroup(teamViewModel);
            return RedirectToAction("Details", new { id=teamViewModel.TeamId});
        }
        public ActionResult AddUser(Guid id)
        {
            TeamViewModel teamViewModel = new TeamViewModel();
            if (TeamViewRepository.GetTEamViewModelsByTeamId(id).Find(x => x.TeamId == id) != null)
            {
                teamViewModel.TeamId = TeamViewRepository.GetTEamViewModelsByTeamId(id).Find(x => x.TeamId == id).TeamId;
                return View("AddUser", teamViewModel);
            }
            else
            {
                teamViewModel.TeamId = id;
                return View("AddUser", teamViewModel);
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
                return RedirectToAction("Details",new { id = teamViewModel.TeamId });
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Details(Guid TeamId)
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
            catch(Exception ex)
            {
                return View(ex);
            }
        }
        public ActionResult EditTeam(Guid TeamId)
        {
            TeamModel teamModel = TeamRepository.GetTeamById(TeamId);
            return View("Edit",teamModel);
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
            ViewBag.UserName = UserRepository.GetUserById(UserId).UserName;
            ViewBag.TeamRoleName = UserTeamRoleRepository.GetTeamRoleModels().Find(x => x.UserTeamRoleId == TeamRoleId).UserTeamRoleName;
            return View();
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

                return RedirectToAction("Details", new { id = teamViewModel.TeamId });
            }
            catch
            {
                return View();
            }
        }
    }
}
