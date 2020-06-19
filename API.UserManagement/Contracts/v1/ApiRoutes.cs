using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.LoginAndRegister.Contracts.v1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Users
        {
            public const string GetAll = Base + "/users";
            public const string Get = Base + "/users/{userId}";
            public const string Create = Base + "/users";
            public const string Update = Base + "/users/{userId}";
            public const string Delete = Base + "/users/{userId}";
        }
    }
}
