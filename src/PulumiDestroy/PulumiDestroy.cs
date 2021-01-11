using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace Xccelerated.Pulumi
{
    public static class PulumiDestroy
    {
        const string collectionUri = "https://dev.azure.com/norbinto0432";
        const string projectName = "pulumni";
        // personal access token
        const string pat = Environment.GetEnvironmentVariable("PAT");

        [FunctionName("PulumiDestroy")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var creds = new VssBasicCredential(string.Empty, pat);
            var connection = new VssConnection(new Uri(collectionUri), creds);
            var buildHttpClient = connection.GetClient<BuildHttpClient>();
            var buildDefinitionReferences = await buildHttpClient.GetDefinitionsAsync(projectName);
            
            Console.WriteLine($"Found {buildDefinitionReferences.Count} Pipelines: [{string.Join(',', buildDefinitionReferences.Select(p => p.Name))}]");

            var build = new Build {
                Definition = buildDefinitionReferences.Last(),
                Parameters = "{\"branchName\":\"passed-from-console-tool\"}"
            };
            
            var buildQueueResult = await buildHttpClient.QueueBuildAsync(build, projectName);
            Console.WriteLine(buildQueueResult.Reason.ToString());
            
            name = "";

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
