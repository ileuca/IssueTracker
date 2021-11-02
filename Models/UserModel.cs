using System;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Models
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        [DataType(DataType.MultilineText)]
        public string UserDescription { get; set; }
    }
}