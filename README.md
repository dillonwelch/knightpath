# API Flow

The first API endpoint is `POST https://[...]/knightpath`. This takes two arguments, "source" and "target". These arguments need to match the format of an 8x8 chess board position ("A1", "D4", etc). The response value will be a Guid that you can use to query for the calculated results.

The second API endpoint is `GET https://[...]/knightpath`. This takes one argument, "operationId", that needs to be a valid Guid. This will return a JSON response in the following format:
```
{
    ShortestPath: string # Each position, concatenated by ":", for example "A2:B4:C2:A3".
    NumberOfMoves: int;
    Starting: string;
    Ending: string;
    OperationId: string; # A Guid.
}
```
or will return a 404 if not found (which could also mean the queue message has not been processed).

# Azure Functions

The APIs were implemented using Azure Functions. This was chosen due to its easy integration with C# and Visual Studio Code. I also just wanted to learn more about how it works.

The Function App contains 3 functions:

`CreateKnightPath`, which corresponds to the `POST` endpoint. This function adds the calculation request to an Azure Storage Queue named `knightpathqueue`.

`CalculateShortestPath`, which listens on the `knightpathqueue` queue. This function will call the `ShortestPathCalculator` and save the results into a SQL Server database table named `Paths`. 

`FindKnightPath`, which corresponds to the `GET` endpoint. This function queries the `Paths` table and serializes the results into JSON.

These functions use input and output bindings to cleanly flow from one step to the next without having to write boilerplate code for database and queue interaction.

Azure Storage Queue was used due to it being a lightweight solution for what was needed (a durable and transient storage for a calculation request) and it additionally came built-in with the Azure app setup.

SQL Server was used to store the calculation results due to me generally having a "set up a database" mindset from non-serverless Ruby on Rails infra. As I'm writing these notes, I realized that Azure Storage also has a Tables section that I probably could have used instead as a more lightweight solution like I did with the Queues. My code started out using Entity Framework (because my brain is used to using an [ORM](https://guides.rubyonrails.org/active_record_basics.html)), which is how I put together my database in `database.sql`. 

# Shortest Path Calculation

The calculation results were achieved by doing a Breadth-First Search on the chess board in combination with a Dynamic Programming approach to cache calculated paths in order to not repeat work. A knight has at most eight possible move choices due to its movement rules, so each of these are checked for viability, then checked if they've been "visited" before, and then calculated if not. This logic can be found in `ShortestPathCalculator`.

As-is, the class is hardcoded to use an 8x8 board. However, it could be pretty easily refactored to support arbitary sizes. The two main reasons I didn't are that I would need to change how I represent the positions as strings (do I use "AAAAA, 12345", do I convert the row into a number, etc etc) and that the extra work required for that was not a requirement for the assignment :).

For an 8x8 board, I did not do any performance optimizations besides the DP approach. However, when thinking about an arbitary sized board, this becomes more important both in runtime and memory usage. The main optimization that comes to mind is utilizing calculated results between different invocations of the API (or seeding a cache of results with a setup script). The easiest change would be to check for exact matches of existing results and pull that calculated result out. A more substantial change would be to additionally check for matches when calculating sub-paths.

# Input Validation
I took a defensive approach to validating inputs. Given that each function can act independently (another service could add an item to the queue, causing `CalculateShortestPath` to trigger), I validated at each step of the way for nulls, whitespaces, and matching proper formats for the Guid and board positions.

# Dependencies

For local development:
- An environment that can run C# (I developed on a Mac using VS Code).
- SQL Server DB in [Docker](https://medium.com/@ugurelsevket/setting-up-sql-server-with-docker-on-macos-a-step-by-step-guide-8742c725a63e).
  - Add `"SqlConnectionString": "YOURSTRING"` to `local.settings.json` under `ConnectionStrings`.
  - I had to add `TrustServerCertificate=true` to my string with the above Docker setup.
- [Azurite Queue Service](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio%2Cblob-storage).
  - Add `"AzureWebJobsStorage": "UseDevelopmentStorage=true"` to `local.settings.json` under `Values`.

Note that you can also use your Azure resources if you put the appropriate connection strings in, so that you don't have to set up the dependencies locally :) 

Within Azure:
- A Function App
- A Storage Account with a Queue named `knightpathqueue`.
- A SQL server and database (I had to set this up in East US 2 instead of East US for some reason).
- Maybe more? I'm not sure what resources get auto created when using the VS Code and CLI toolings.

Database Schema:
This can be found in `database.sql`. It's just one table!

# Testing

I used `NUnit` and a separate `KnightPath.Tests` folder for a test project with unit tests. 

Notes:
- There didn't seem to be a lot of resources out there for best practices for Azure Function testing, so it's possible I'm missing out on better ways to test. Take a look at `Mocks.cs` for an example of lacking functionaliy.
- I only just found [this help doc](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices) that dives deeper into C# testing conventions that I wasn't aware of.
- I wasn't sure how to do integration / E2E automated testing. I would love to have a test that calls both of the APIs and ensures the results get calculated. This would save a lot of manual testing between changes.

# Improvements

- Refactor data storage to use Azure Storage Tables instead of SQL Server.
- Add support for arbitary sized boards.
- Add class and function level documentation for easier maintainability.
- Add Dockerfile or similar for Azurite service for local development.
- Add Azure Infrastructure as Code tooling to automate resoure creation, [example guide](https://codefresh.io/learn/infrastructure-as-code/infrastructure-as-code-on-azure-tools-and-best-practices/) I found.
- Add a Github Action for running the test suite + checking for formatting or build warnings.
- Add a Github Action for deploying to prod on a successful merge to `main`.