using System;
using System.IO;
using System.Linq;
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
        static string pat = Environment.GetEnvironmentVariable("PAT")??"error";

        [FunctionName("PulumiDestroy")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var creds = new VssBasicCredential(string.Empty, pat);
            var connection = new VssConnection(new Uri(collectionUri), creds);
            var buildHttpClient = connection.GetClient<BuildHttpClient>();
            var buildDefinitionReferences = await buildHttpClient.GetDefinitionsAsync(projectName);
            
            log.LogInformation($"Found {buildDefinitionReferences.Count} Pipelines: [{string.Join(',', buildDefinitionReferences.Select(p => p.Name))}]");

            var build = new Build {
                Definition = buildDefinitionReferences.Last(),
                Parameters = "{\"branchName\":\"passed-from-console-tool\"}"
            };
            
            var buildQueueResult = await buildHttpClient.QueueBuildAsync(build, projectName);
            log.LogInformation(buildQueueResult.Reason.ToString());
            
            
            return new OkObjectResult("");
        }
    }
}
