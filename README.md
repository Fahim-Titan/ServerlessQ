# Azure Functions Queue Communication Solution

## Summary
This project is a C# Azure Functions-based solution demonstrating communication between two Azure Functions using Azure Storage Queues. The solution includes HTTP-triggered and Queue-triggered functions that interact with an in-memory SQLite database and a REST API to process data. The solution is designed to run locally using the Azurite emulator.

## Task Details
1. **Function A**:
   - Triggered by an HTTP request containing a JSON payload with `FirstName` and `LastName`.
   - Saves the provided data to an in-memory SQLite database.
   - Publishes a message to an Azure Storage Queue with the same data.
   - Returns a `200 OK` response if the operation is successful.

2. **Function B**:
   - Triggered by messages on the Azure Storage Queue.
   - Makes an HTTP call to a REST API (`https://tagdiscovery.com/api/get-initials?name=<FULL NAME>`) passing the concatenated `FirstName` and `LastName` as `<FULL NAME>`.
   - Saves the SVG response returned by the API to the SQLite database, associated with the respective `FirstName` and `LastName`.

## Setup Instructions
1. **Prerequisites**:
   - Visual Studio 2019 or later.
   - .NET Core SDK (3.1 or later).
   - Azure Functions Tools.
   - Azurite emulator for local Azure Storage emulation.

2. **Clone the Repository**:
   ```bash
   git clone <repository-url>
   cd <repository-directory>
   ```
3. Install Dependencies:

	- Open the solution in Visual Studio.
	- Restore NuGet packages for all projects.

4. Make sure the Azurite is installed. 
5. Configure Local Settings:
	- Update local.settings.json in the Functions project to configure Azure Storage and SQLite connection strings.
	```json
	{
		"IsEncrypted": false,
		"Values": {
			"AzureWebJobsStorage": "UseDevelopmentStorage=true",
			"FUNCTIONS_WORKER_RUNTIME": "dotnet"
		}
	}	
	```
6. Build and Run the Solution:
	- Build the solution in Visual Studio.
	- Run the solution locally to ensure Azurite and the Functions are correctly configured.

## How to Run

## Screenshots

