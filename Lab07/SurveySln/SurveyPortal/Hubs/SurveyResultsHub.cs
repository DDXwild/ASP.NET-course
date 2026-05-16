using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SurveyPortal.Hubs
{
    [Authorize]
    public class SurveyResultsHub : Hub
    {
        public static string GetGroupName(long surveyId) => $"survey-{surveyId}";

        public Task JoinSurvey(long surveyId) =>
            Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(surveyId));

        public Task LeaveSurvey(long surveyId) =>
            Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(surveyId));
    }
}
