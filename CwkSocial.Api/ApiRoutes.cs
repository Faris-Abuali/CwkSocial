﻿namespace CwkSocial.Api;

public class ApiRoutes
{
public const string BaseRoute = "api/v{version:apiVersion}/[controller]";

    public static class UserProfiles
    {
        public const string IdRoute = "{id}";
    }

    public static class Posts
    {
        public const string GetById = "{id}";
    }
}
