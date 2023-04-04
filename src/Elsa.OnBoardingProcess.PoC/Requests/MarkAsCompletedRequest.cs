using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Elsa.OnBoardingProcess.PoC.Requests;

public class MarkAsCompletedRequest
{
    [JsonProperty("workflowInstanceId")]
    public string WorkflowInstanceId { get; set; } = string.Empty;

    [JsonProperty("input")]
    public JToken Input { get; set; } = JValue.CreateNull();
}