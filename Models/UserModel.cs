﻿using System;
using System.Collections.Generic;
using System.Collections;

namespace IssueTracker.Models
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserDescription { get; set; }


    }
}