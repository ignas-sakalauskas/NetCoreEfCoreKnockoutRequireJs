# .Net Core EF Core Knockout RequireJs Web Application

## Introduction
This web application uses modern UI and RESTful API, and as long as interfaces are the same, could be easily swapped out with a different application. RESTful API written in loosely coupled manner to keep the maintenance easier, and unit tests included to cover the logic implemented. The application requires minimal configuration to start.

## Technology
Solution was built using Visual Studio 2017 .NET Core 1.1 and MS SQL Express 2016.
Library and framework dependencies are downloaded as Nuget and Bower packages automatically before build.
For database operations Entity Framework Core ORM was used. Started with code first approach, and added a migration later to have a snapshot of final database schema. On application start-up, automatic sample data seeding will occur if no previous records were found in database.
In back-end default .NET Core Microsoft Dependency Injection framework is used to inject class dependencies.
Errors are logged using NLog extension for Microsoft LoggerFactory. 

## Unit tests:
*	Back-end unit tests use MSTest to run tests, FluentAssertions to assert tests, and Moq to mock dependencies.
*	Front-end (JavaScript) unit tests use Jasmine framework (unit testing + mocks), and Chutzpah to run the tests.
UI built using Knockout JavaScript framework for components logic, and Bootstrap CSS framework for design. Knockout configured to use RequireJS dependency injection framework to get JavaScript data services injected into view models or other services.
UI communication with back-end. UI calls promise based JavaScript services, and these service make AJAX calls to RESTful WebAPI on back-end. Requests and responses are sent in JSON.
Application uses Front-End validation, however on unsuccessful response from back-end the response will displayed on UI as well.
Source code coding-style consistency and comments were validated by StyleCop.Analyzers.

## Setup
Please make sure you have Visual Studio 2017 .NET Core 1.1 installed. 
The web application was built using MS SQL Express 2016, however it should be able to run using older MS SQL server if you get database created automatically by the application, i.e. manual backup restore might require the same MS SQL server version. 
Before running the application, please check following:
1.	Database connection string. Check appsettings.json file in WebApplication1.Web project. AppDbContext is set to use “.\SQLEXPRESS01” database server. Please point it to your MS SQL server.
2.	Logs. Check nlog.config file in WebApplication1.Web project. By default all logs are stored in “C:\temp\logs” folder. Please make sure application has write access to it.
By default application runs in Development mode using IIS Express, however you can switch to “IIS Express (Production)” profile in Visual Studio to test error pages, and switch to loading static resources from CDN when possible.
URLs:
*	Launching the application from Visual Studio will open: http://localhost:51757/ 
*	Swagger UI for RESTful API testing can be found here: http://localhost:51757/swagger/ui/ 

## Security
*	Knockout protects against XSS by encoding all output values, unless explicitly specifying data binding like “html” to output raw code.
*	Razor View rendering engine protects against XSS by encoding all output values, unless explicitly specifying output like “@Html.Raw” to output raw code.
*	Entity Framework automatically parameterize all queries to protect against SQL injection.

## Testing
Unit tests:
*	Back-end. Simply use Visual Studio Test Explorer, or ReSharper.
*	Front-end. I use “Chutzpah Test Runner Context Menu Extension” or “Chutzpah Test Adapter for the Test Explorer” Visual Studio extensions to run tests from Visual Studio.
Besides the functionality required by the technical test, following was added:
*	CreatedOn field which stores record creation date and time. Displayed as read-only in UI.
*	Search field on the main page. Performs filtering of loaded clients (UI filtering only).
*	Category option was added to assign a category to each client – the same category could be assigned to multiple clients. To keep the application simple, only Create and Get operations available.

## Blog Post
Blog post with more details about the implementation: https://ignas.me/tech/net-core-knockoutjs-web-application/
