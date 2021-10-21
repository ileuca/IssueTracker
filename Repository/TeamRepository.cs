using IssueTracker.Models;
using IssueTracker.Models.DBObjects;
using IssueTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace IssueTracker.Repository
{
    public class TeamRepository
    {
        private Models.DBObjects.IssueTrackerModelsDataContext dbContext;
        private UserRepository UserRepository = new UserRepository();
        private TeamGroupRepository TeamGroupRepository = new TeamGroupRepository();

        public TeamRepository()
        {
            this.dbContext = new Models.DBObjects.IssueTrackerModelsDataContext();
        }
        public TeamRepository(Models.DBObjects.IssueTrackerModelsDataContext dbContext)
        {
            this.dbContext = dbContext;
        }
        private TeamModel MapDbObjectToModel(Team dbTeam)
        {
            TeamModel teamModel = new TeamModel();
            if (dbTeam != null)
            {
                teamModel.TeamId= dbTeam.TeamId;
                teamModel.TeamName = dbTeam.TeamName;
                teamModel.TeamDescription = dbTeam.TeamDescription;
                teamModel.CreatedBy = dbTeam.CreatedBy;

                return teamModel;
            }
            return null;
        }
        private Team MapModelToDbObject(TeamModel teamModel)
        {
            Team dbTeam = new Team();
            if (teamModel != null)
            {
                dbTeam.TeamId = teamModel.TeamId;
                dbTeam.TeamName = teamModel.TeamName;
                dbTeam.TeamDescription = teamModel.TeamDescription;
                dbTeam.CreatedBy = teamModel.CreatedBy;

                return dbTeam;
            }
            return null;
        }
        //Create
        public  void CreateTeam(TeamModel teamModel)
        {
            teamModel.TeamId = Guid.NewGuid();
            teamModel.CreatedBy = UserRepository.GetCurrentUser().UserId;
            dbContext.Teams.InsertOnSubmit(MapModelToDbObject(teamModel));
            dbContext.SubmitChanges();
        }
        //Read
        public List<TeamModel> GetTeamsForCurrentUserId(Guid currentUserID)
        {
            List<TeamGroupsModel> teamGroupList = new List<TeamGroupsModel>();
            List<TeamModel> teamModelList = new List<TeamModel>();
            teamGroupList = TeamGroupRepository.GetAllTeamGroups().Where(x => x.UserId == currentUserID).ToList();
            foreach(var teamGroup in teamGroupList)
            {
                if(!teamModelList.Exists(x=>x.TeamId == teamGroup.TeamId))
                {
                    teamModelList.Add(GetTeamById(teamGroup.TeamId));
                }
            }
            return teamModelList;
        }
        public List<TeamModel> GetTeamsCreatedBy()
        {
            List<TeamModel> teamList = new List<TeamModel>();
            if (UserRepository.GetCurrentUser() == null)
            {
                return teamList;
            }
            foreach (Team team in dbContext.Teams.Where(x => x.User.UserId == UserRepository.GetCurrentUser().UserId))
            {
                teamList.Add(MapDbObjectToModel(team));
            }
            return teamList;
        }
        public TeamModel GetTeamById(Guid guid)
        {
            return MapDbObjectToModel(dbContext.Teams.FirstOrDefault(x => x.TeamId == guid));
        }
        //Update
        public void UpdateTeam(TeamModel teamModel)
        {
            Team existingTeam = dbContext.Teams.FirstOrDefault(x => x.TeamId == teamModel.TeamId);
            existingTeam.TeamName = teamModel.TeamName;
            existingTeam.TeamDescription = teamModel.TeamDescription;
            dbContext.SubmitChanges();
        }
        //Delete
        public void DeleteTeam(TeamModel teamModel)
        {
            Team existingTeam = dbContext.Teams.FirstOrDefault(x => x.TeamId == teamModel.TeamId);
            if (existingTeam != null)
            {
                foreach(var teamGroup in dbContext.TeamGroups.Where(x=>x.TeamId == teamModel.TeamId))
                {
                    dbContext.TeamGroups.DeleteOnSubmit(teamGroup);
                    dbContext.SubmitChanges();
                }
                dbContext.Teams.DeleteOnSubmit(existingTeam);
                dbContext.SubmitChanges();
                return;
            }
            return;
        }

    }
}