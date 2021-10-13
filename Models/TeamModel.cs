﻿using IssueTracker.ViewModels;
using System;

namespace IssueTracker.Models
{
    public class TeamModel
    {
        public Guid TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamDescription { get; set; }
        public Guid CreatedBy { get; set; }
        public TeamModel()
        {
            TeamViewModel teamViewModel = new TeamViewModel();
            teamViewModel.TeamId = TeamId;
            teamViewModel.TeamName = TeamName;
        }
    }
}