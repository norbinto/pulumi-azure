# pulumi-azure
Pulumi managed infrastructure project for Azure

## Structure

There are four C# projects here:

* Azure Function
* Unit Tests project
* API Tests project
* Pulumi infrastructure project


## Pipelines

There are two pipelines used for this project.

* Azure Pipelines
* GitHub Actions

### Azure Pipelines

Azure Pipelines is used to build and test the Azure Function code, compile and run Pulumi, deploy Function to Azure (staging slot), Verify that it's up and running and swap the slot to puduction.

![Pipeline](docs/pipeline.png)

pul-a58c42ac1a77bc94f877d7baf7c2ad8175dbf0cd