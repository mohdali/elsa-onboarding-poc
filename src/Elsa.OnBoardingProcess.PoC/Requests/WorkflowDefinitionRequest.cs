using Newtonsoft.Json.Linq;

namespace Elsa.OnBoardingProcess.PoC.Requests;

public class WorkflowDefinitionRequest
{
    public string CorrelationId { get; set; }
    public JToken Input { get; set; }
}