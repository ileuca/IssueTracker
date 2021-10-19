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

        public TeamRepository()
        {
            this.dbContext = new Models.DBObjects.IssueTrackerModelsDataContext();
        }
        public TeamRepository(Models.DBObjects.IssueTrackerModelsDataContext dbContext)
        {
            this.dbContext = dbContext;
        }
        private User GetCurrentUser()
        {
            if (HttpContext.Current.User != null)
            {
                return dbContext.Users.FirstOrDefault(x => x.UserEmail == HttpContext.Current.User.Identity.Name);
            }
            return null;
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
            teamModel.CreatedBy = GetCurrentUser().UserId;
            dbContext.Teams.InsertOnSubmit(MapModelToDbObject(teamModel));
            dbContext.SubmitChanges();
        }
        //Read
        public List<TeamModel> GetTeamsCreatedBy()
        {
            List<TeamModel> teamList = new List<TeamModel>();
            foreach (Team team in dbContext.Teams.Where(x => x.User.UserId == GetCurrentUser().UserId))
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
        //Delete

    }
}