/* public class ApplicationAccessViewModel
{
    public List<recert_users> Users { get; set; }
    public List<recert_applications> Applications { get; set; }
    public List<recert_user_access> User_Accesses { get; set; }
    public List<recert_requests> Requests { get; set; }
    public int OverallProgress { get; set; }
} */
using System.Text.Json.Serialization;

public class AgentResult
{
    [JsonPropertyName("output")]
    public string Output { get; set; }
}

public class AgentResponse
{
    [JsonPropertyName("result")]
    public AgentResult Result { get; set; }
}
public class OwnerDashboardViewModel
{
    public List<ApplicationAccessViewModel> ApplicationAccessList { get; set; }
    public List<OwnerunderUserViewModel> UsersUnderOwner { get; set; }
    public List<OwnerApplicationsViewModel> OwnerApplications { get; set; }
}
public class OwnerunderUserViewModel
{
    public string UserName { get; set; }
}
public class OwnerApplicationsViewModel
{
    public string ApplicationName { get; set; }
}
public class ApplicationAccessViewModel
{
    public string ApplicationId { get; set; }
    public string UserId { get; set; }
    public string access_id { get; set; } // Needed for lookup or AI agent

    public string ApplicationName { get; set; }
    public string Description { get; set; }
    public string UserName { get; set; }
    public DateTime? LastUsed { get; set; }
    public string AIRecommendation { get; set; } // Will be filled on demand
}
public class RecommendationRequestModel
{
    public string UserId { get; set; }
    public string ApplicationId { get; set; }
    public string access_id { get; set; }
}

public class recert_users
{
    public int id { get; set; }
    public string user_id { get; set; }
    public string employee_id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string department { get; set; }

    public string job_title { get; set; }

    public string location { get; set; }

    public string manager_id { get; set; }

    public string hire_date { get; set; }
    public bool is_active { get; set; }
    public DateTime last_updated { get; set; }
}

public class recert_applications
{
    public int id { get; set; }
    public string application_name { get; set; }
    public string app_type { get; set; }
    public string description { get; set; }
    public string owner_id { get; set; }
    public string criticality { get; set; }
    public string data_classification { get; set; }
    public string recertification_frequency_days { get; set; }
    public bool requires_manager_approval { get; set; }
    public bool requires_owner_approval { get; set; }
    public DateTime last_updated { get; set; }
}

public class recert_user_access
{
    public int id { get; set; }
    public string access_id { get; set; }
    public string user_id { get; set; }
    public string application_id { get; set; }
    public string access_type { get; set; }

    public string status { get; set; }
    public DateTime granted_Date { get; set; }
    public DateTime last_recertified_date { get; set; }
    public DateTime expiry_date { get; set; }
    public string justification { get; set; }
    public string granted_by { get; set; }
    public DateTime last_accessed_date { get; set; } 
    public int access_frequency_last_90_days { get; set; } 
     
}
public class recert_requests
{
    public int id { get; set; }
    public string request_id { get; set; }
    public string access_id { get; set; }
    public string user_id { get; set; }
    public string application_id { get; set; }
    public string reviewer_id { get; set; }
    public DateTime request_date { get; set; }
    public DateTime due_date { get; set; }
    public DateTime completion_date { get; set; }
    public string status { get; set; }
    public string decision { get; set; }
    public string decision_type { get; set; }
    public double ai_confidence_score { get; set; }
    public string risk_level { get; set; }
    public string reviewer_comments { get; set; }
    public string ai_recommendation { get; set; }
    public string ai_insights { get; set; }
}
