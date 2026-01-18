
MT Tombola is a application built in .Net 10 consisting of:
	1. MT.Beans.API – ASP.NET Core Web API
	2. MT.Beans.App – Blazor WebAssembly client

The API provides bean data, and the Blazor app displays beans, supports searching, and shows the Bean of the Day.The API uses Entity Framework Core with a SQL Server database to store bean information.
The API exposes endpoints for CRUD operations on beans.The API has Data Seeding functionality to populate the database with initial bean data from a JSON file.

The APP consumes the API to fetch and display bean information.

Prerequisites:
	- .NET 8.0 SDK or later
	- Visual Studio 2022 or later


To Create SQL Database:
	1. Delete the Migrations folder in the MT.Beans.API project.
	2. Give the correct connection string in appsettings.json file of MT.Beans.API project i.e., data source.
	3. Open a package manager console in Visual Studio.
	4. Run the command: `Add-Migration InitialCreate` and then `Update-Database` and default project must be MT.Beans.API. Else you can run the following command in the package manager console:
	   `Update-Database -Project MT.Beans.API -StartupProject MT.Beans.API`
	4. Then open SQL Server Management Studio and check whether the tables exist.
	5. Run the MT.Beans.API as a single project to insert the Json data into SQL tables.


To run the application:
	1. Open the solution in Visual Studio 2022 or later.
	2. Set MT.Beans.App and MT.Beans.API as startup projects.
	3. Run the application (F5).
	4. The Blazor app will open in your default web browser.
	5. Use the search box to find beans by name.
	6. Click on a bean to see detailed information.
	7. Check the Bean of the Day section for a featured bean.

API Endpoints:
	- GET /api/beans – Get all beans
	- GET /api/beans/{externalid} – Get bean by ID
	- GET /api/beans/bean-of-the-day – Get the Bean of the Day
	- POST /api/beans – Add a new bean
	- PUT /api/beans/{externalid} – Update an existing bean
	- DELETE /api/beans/{externalid} – Delete a bean

The API uses Swagger for API documentation and testing. You can access the Swagger UI at `https://localhost:{port}/swagger` when the API is running.
Please run the API project as a single project to use Swagger and disable the CORS in API Program.cs class before running it.


The Test projects are created for the following:
	1. MT.Beans.API.Service.Tests – Unit tests for the Api Service layer using NUnit and Moq.
	2. MT.Beans.API.Tests – Unit tests for the Api using NUnit and Moq.
	3. MT.Beans.App.Services.Tests – Unit tests for the App Service Layer using NUnit and Moq.

Please contact the developer for any issues or further assistance.


	

