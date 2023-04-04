using Newtonsoft.Json;

namespace Elsa.OnBoardingProcess.PoC.Responses;

public class WorkflowDefinitionResponse
{
    [JsonProperty("workflowInstanceId")]
    public string WorkflowInstanceId { get; set; }
    
    [JsonProperty("activityId")]
    public string ActivityId { get; set; }
}