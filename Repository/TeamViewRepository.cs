using IssueTracker.Models;
using IssueTracker.Models.DBObjects;
using IssueTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.Repository
{
    public class TeamViewRepository
    {
        private Models.DBObjects.IssueTrackerModelsDataContext dbContext;
        private UserRepository userRepository = new UserRepository();
        private TeamRepository teamRepository = new TeamRepository();
        private TeamGroupRepository teamGroupRepository = new TeamGroupRepository();
        private UserTeamRoleRepository userTeamRoleRepository = new UserTeamRoleRepository();

        public TeamViewRepository()
        {
            this.dbContext = new Models.DBObjects.IssueTrackerModelsDataContext();
        }
        public TeamViewRepository(Models.DBObjects.IssueTrackerModelsDataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TeamViewModel MapModelsToTeamViewModel(TeamGroupsModel teamGroupsModel)
        {
            TeamViewModel teamViewModel = new TeamViewModel();
            teamViewModel.TeamId = teamGroupsModel.TeamId;
            teamViewModel.TeamName = teamRepository.GetTeamById(teamGroupsModel.TeamId).TeamName;
            teamViewModel.TeamDescription = teamRepository.GetTeamById(teamGroupsModel.TeamId).TeamDescription;
            teamViewModel.UserId = teamGroupsModel.UserId;
            teamViewModel.UserNameSurname = userRepository.GetUserById(teamGroupsModel.UserId).UserName;
            teamViewModel.TeamRoleId = teamGroupsModel.UserTeamRoleId;
            teamViewModel.TeamRoleName = userTeamRoleRepository.GetTeamRoleModels().FirstOrDefault(x=>x.userTeamRoleId == teamGroupsModel.UserTeamRoleId).UserTeamRoleName;

            return teamViewModel;

        }
        public TeamGroup MapTeamViewModelToTeamGroup(TeamViewModel teamViewModel)
        {
            TeamGroup teamGroup = new TeamGroup();
            //teamGroup.TeamGroupId = teamGroupRepository.GetAllTeamGroups().Count + 1;
            teamGroup.TeamId = teamViewModel.TeamId;
            teamGroup.UserId = teamViewModel.UserId;
            teamGroup.UserTeamRoleId = teamViewModel.TeamRoleId;

            return teamGroup;
        }

        //Create -- CreateTeamView(CreateTeam + CreateTeamGroup)
        public void CreateTeamGroup(TeamViewModel teamViewModel)
        {  
            dbContext.TeamGroups.InsertOnSubmit(MapTeamViewModelToTeamGroup(teamViewModel));
            dbContext.SubmitChanges();
        }

        //Read
        //GetAllUsers(sa poata fi adaugati in Team)
        public List<UserModel> GetAllusers()
        {
            List<UserModel> userList = new List<UserModel>();
            userList = userRepository.GetAllUsers();
            return userList;
        }
        //GetUsersByTeamId(sa poata fi afisati in detalii)
        public List<UserModel> GetUsersByTeamId(Guid teamId)
        {
            List<UserModel> userList = new List<UserModel>();
            List<TeamGroupsModel> teamGroupModelList = new List<TeamGroupsModel>();
            List<UserModel> userListByTeam = new List<UserModel>();
            teamGroupModelList = teamGroupRepository.GetAllTeamGroups();
            userList = userRepository.GetAllUsers();

            foreach (TeamGroupsModel teamGroupModel in teamGroupModelList)
            {
                foreach (UserModel userModel in userList)
                {
                    if (userModel.UserId == teamGroupModel.UserId && teamId == teamGroupModel.UserId)
                    {
                        userListByTeam.Add(userModel);
                    }
                }
            }
            return userListByTeam;
        }
        //GetTeamNameByID
        public string GetTeamNameById(Guid teamId)
        {
            TeamViewModel teamViewModel = new TeamViewModel();
            teamViewModel.TeamName = teamRepository.GetTeamById(teamId).TeamName;
            return teamViewModel.TeamName;
        }
        //getAllTEamViewModels
        public List<TeamViewModel> GetAllTeamViewModels()
        {
            List<TeamViewModel> teamViewModels = new List<TeamViewModel>();
            foreach(TeamGroupsModel teamGroupsModel in GetAllTeamGroups())
            {
                teamViewModels.Add(MapModelsToTeamViewModel(teamGroupsModel));
            }
            return teamViewModels;
        }
        public List<TeamViewModel> GetTEamViewModelsByTeamId(Guid id)
        {
            List<TeamViewModel> teamViewModels = new List<TeamViewModel>();
            foreach(TeamViewModel teamViewModel in GetAllTeamViewModels())
            {
                if (teamViewModel.TeamId == id)
                {
                    teamViewModels.Add(teamViewModel);
                }
            }
            return teamViewModels;
        }
        //GetTeamRoles
        public List<UserTeamRoleModel> GetAllTeamRoles()
        {
            List<UserTeamRoleModel> userTeamRoleModels = new List<UserTeamRoleModel>();
            userTeamRoleModels = userTeamRoleRepository.GetTeamRoleModels();
            return userTeamRoleModels;
        }
        //GetAllTEamGroups
        public List<TeamGroupsModel> GetAllTeamGroups()
        {
            List<TeamGroupsModel> teamGroupsModels = new List<TeamGroupsModel>();
            teamGroupsModels = teamGroupRepository.GetAllTeamGroups();
            return teamGroupsModels;

        }
        //GetAllTeamViewModels?
        //bazeazate pe usersbyteamid si fa un roles by userid si rolesbyteamid

        //Update -- UpdateByUserID(Roluri) // UpdateByTeamID(Users)+Roluri?

        //Delete -- DeleteByUserID user din team Group // DeleteByTeamId (absolut Toate inregistrarile)
    }

}