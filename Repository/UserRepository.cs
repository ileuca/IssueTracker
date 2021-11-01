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
        private readonly Models.DBObjects.IssueTrackerModelsDataContext dbContext;
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
        public void SaveUserChanges(UserModel userModel)
        {
            User user = dbContext.Users.FirstOrDefault(x => x.UserId == userModel.UserId);
            user.UserDescription = userModel.UserDescription;
            dbContext.SubmitChanges();
        }
    }
}