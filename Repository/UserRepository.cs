using IssueTracker.Models;
using IssueTracker.Models.DBObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.Repository
{
    public class UserRepository
    {
        private Models.DBObjects.IssueTrackerModelsDataContext dbContext;

        public UserRepository()
        {
            this.dbContext = new Models.DBObjects.IssueTrackerModelsDataContext();
        }
        public UserRepository(Models.DBObjects.IssueTrackerModelsDataContext dbContext)
        {
            this.dbContext = dbContext;
        }
        private UserModel MapDbObjectToModel(User dbUser)
        {
            UserModel userModel = new UserModel();
            if (dbUser != null)
            {
                userModel.UserId = dbUser.UserId;
                userModel.UserName = dbUser.UserName;
                userModel.UserEmail = dbUser.UserEmail;
                userModel.UserDescription = dbUser.UserDescription;

                return userModel;
            }
            return null;
        }


        private User MapModelToDbObject(UserModel userModel)
        {
            User dbUser = new User();
            if (userModel != null)
            {
                dbUser.UserId = userModel.UserId;
                dbUser.UserName = userModel.UserName;
                dbUser.UserEmail = userModel.UserEmail;
                dbUser.UserDescription = userModel.UserDescription;

                return dbUser;
            }
            return null;
        }
        //Create
        public void CreateUser(UserModel userModel)
        {
            userModel.UserId = Guid.NewGuid();
            dbContext.Users.InsertOnSubmit(MapModelToDbObject(userModel));
            dbContext.SubmitChanges();
        }
        //Read
        public User GetCurrentUser()
        {
            if (HttpContext.Current.User != null)
            {
                return dbContext.Users.FirstOrDefault(x => x.UserEmail == HttpContext.Current.User.Identity.Name);
            }
            return null;
        }
        public UserModel GetUserById(Guid guid)
        {
            return MapDbObjectToModel(dbContext.Users.FirstOrDefault(x => x.UserId == guid));
        }
        public List<UserModel> GetAllUsers()
        {
            List<UserModel> usersList = new List<UserModel>();
            foreach(User user in dbContext.Users)
            {
                usersList.Add(MapDbObjectToModel(user));
            }
            return usersList;
        }
        public List<UserModel> GetUserByTeam(Team team)
        {
            List<UserModel> usersByTeamList = new List<UserModel>();
            foreach(TeamGroup teamGroup in dbContext.TeamGroups.Where(x=>x.TeamId == team.TeamId))
            {
                usersByTeamList.Add(GetUserById(teamGroup.UserId));
            }
            return usersByTeamList;
        }
        //Update
        public void UpdateUser(UserModel userModel)
        {
            User existingUser = dbContext.Users.FirstOrDefault(x => x.UserId == userModel.UserId);
            existingUser = MapModelToDbObject(userModel);
            dbContext.SubmitChanges();
        }
        //Delete
        public void DeleteUser(UserModel userModel)
        {
            User existingUser = dbContext.Users.FirstOrDefault(x => x.UserId == userModel.UserId);
            if(existingUser != null)
            {
                dbContext.Users.DeleteOnSubmit(existingUser);
                dbContext.SubmitChanges();
            }
        }

    }
}