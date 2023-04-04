using System.Text;
using Elsa.OnBoardingProcess.PoC.Requests;
using Elsa.OnBoardingProcess.PoC.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Elsa.OnBoardingProcess.PoC.Services;

public class WorkflowDefinitionService
{
    private readonly IHttpClientFactory httpClientFactory;

    public WorkflowDefinitionService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<WorkflowDefinitionResponse> StartWorkflowDefinition(string workflowDefinitionId, string? correlationId = null, JToken? input= null)
    {
        var httpClient = httpClientFactory.CreateClient("WorkflowDefinitionServiceClient");
        
        var data = new WorkflowDefinitionRequest
        {
            CorrelationId = correlationId,
            Input = input ?? JValue.CreateNull()
        };

        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync($"v1/workflows/{workflowDefinitionId}/dispatch", content);

        return await response.Content.ReadFromJsonAsync<WorkflowDefinitionResponse>();
    }
}