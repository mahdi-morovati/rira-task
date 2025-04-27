using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Api.Middlewares.Models;

public class CustomProblemDetails: ProblemDetails
{
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}