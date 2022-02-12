using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace StepEbay.Common.Models.RefitModels
{
    public class ResponseData<T> : ResponseData
    {
        public T Data { get; set; }

        public static new ResponseData<T> Fail(string key, string value)
        {
            ResponseData<T> response = new ResponseData<T>();
            response.AddError(key, value);

            return response;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.ContentType = "application/json";

            if (Errors.Count != 0)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(Errors));
            }
            else
            {
                await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(Data));
            }
        }
    }

    public class ResponseData : IActionResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>> { };


        public void AddError(string key, string value)
        {
            if (Errors.ContainsKey(key))
                Errors[key].Add(value);
            else
                Errors.Add(key, new List<string> { value });
        }

        public static ResponseData Fail(string key, string value)
        {
            ResponseData response = new ResponseData();
            response.AddError(key, value);

            return response;
        }

        public static ResponseData Ok()
        {
            return new ResponseData();
        }

        public virtual async Task ExecuteResultAsync(ActionContext context)
        {
            if (Errors.Count != 0)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(Errors));
            }
        }
    }
}
