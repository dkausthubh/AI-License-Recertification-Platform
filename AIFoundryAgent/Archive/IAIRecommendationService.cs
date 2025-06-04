public interface IAIRecommendationService
{
    Task<string> GetRecommendationAsync(int userId, int appId, int requestId);
}
