using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;


    public static class UrlExtension
    {
        public static string PathAndQuery(this HttpRequest request) =>
        request.QueryString.HasValue
        ? $"{request.Path}{request.QueryString}"
        : request.Path.ToString();
    }

//use
// <a  asp-action="Login"  asp-route-returnUrl="@ViewContext.HttpContext.Request.PathAndQuery()">content</a>