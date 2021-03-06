﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.API.Controllers
{
    public static class ApiRoutes
    {
        public const string Base = "api";

        public static class Users
        {
            public const string GetAll = Base + "/users";
            public const string Get = Base + "/users/{userId}";
            public const string Create = Base + "/users";
            public const string Update = Base + "/users/{userId}";
            public const string Delete = Base + "/users/{userId}";
        }

        public static class Identity
        {
            public const string Login = Base + "/login";
            public const string Register = Base + "/register";
            public const string Refresh = Base + "/refresh";
        }


    }
}
