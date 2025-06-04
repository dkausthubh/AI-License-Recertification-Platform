using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HackathonPOC.Models;
using System.Configuration;
using Microsoft.VisualBasic;


namespace HackathonPOC.Controllers;

public class HomeController : Controller
{
    
    private readonly DatabricksRepository _repo;
    private readonly OpenAIAgentService _aiService;
    public HomeController(OpenAIAgentService aiService, DatabricksRepository repo)
    {
        _aiService = aiService;
        _repo = repo;
    }

    public async Task<IActionResult> Index()
    {
        var apps = await _repo.GetApplicationsDataAsync();   // List<Application>
        var users = await _repo.GetUsersDataAsync();         // List<User>
        var accesses = await _repo.GetUserAccessDataAsync(); // List<UserAccess>
        var requests = await _repo.GetRecertRequestsDataAsync();   // List<Request>
        
        var ViewUsersunderOwner= (from access in accesses
                         join app in apps on access.application_id equals app.application_id
                         join user in users on access.user_id equals user.user_id  
                         where app.owner_id == "994272"
                         select new OwnerunderUserViewModel
                             {                                 
                                 UserName = user.name                                
                                 
                             }).ToList();

        var ViewOwnerApplications = (from app in apps
                         where app.owner_id == "994272"                        
                         select new OwnerApplicationsViewModel
                             {                                 
                                 ApplicationName = app.application_name                               
                                 
                             }).ToList();
        // Join the data to create a view model
        var Viewapplicationaccesslist = (from access in accesses
                         join app in apps on access.application_id equals app.application_id
                         join user in users on access.user_id equals user.user_id  
                         where app.owner_id == "994272" // Filter for owner_id
                             //&& access.status == "Active" // Optional: filter for active access
                             //&& access.last_accessed_date >= DateTime.Now.AddDays(-90) // Optional: filter for last 90 days
                             //&& req.status == "Pending" // Optional: filter for pending requests
                             //&& req.request_id == access.access_id // Optional: filter by request ID'                       
                         select new ApplicationAccessViewModel
                             {
                                ApplicationId = app.application_id,
                                UserId = user.user_id,
                                access_id = access.access_id,
                                 ApplicationName = app.application_name,
                                 Description = app.description,
                                 UserName = user.name,
                                 LastUsed = access.last_accessed_date,
                                 // AIRecommendation = req.ai_recommendation
                             }).ToList();
            
// Create the view model
    var viewModel = new OwnerDashboardViewModel
    {
        ApplicationAccessList = Viewapplicationaccesslist,
        UsersUnderOwner = ViewUsersunderOwner,
        OwnerApplications = ViewOwnerApplications
    };
        return View(viewModel);
   
    }

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> GetRecommendation([FromBody] RecommendationRequestModel model)
{
        if ( model != null )
         { var a = model.UserId;
            var b = model.ApplicationId; }
    // Fetch access stats from Databricks
        // var usageData = await _repo.GetUserAccessStatsAsync(model.UserId, model.AppId, model.access_id);
        string message = "Get Confidence score for User ID: " + model.UserId+ "," + "App ID: "+ model.ApplicationId;
    // Call the AI agent with that usage data
    var recommendation = await _aiService.GetAgentResponseAsync(message);
    // Return the recommendation as JSON
    if (string.IsNullOrEmpty(recommendation))
    {
        return Json(new { recommendation = "No recommendation available." });
    }
    return Json(new { recommendation });
}

 
   /* private readonly ILogger<HomeController> _logger;

    // public HomeController(ILogger<HomeController> logger)
     {
         _logger = logger;
     }

    // public IActionResult Index()
     {
         return View();
     }
     */

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
