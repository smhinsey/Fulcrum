Purpose
======

Fulcrum provides a self-describing HTTP API to browse, validate, and publish commands, and also to browse and execute queries. These commands and queries represent an application's capabilities by expressing operations which modify its internal state as commands and operations which return its internal state as queries. While Fulcrum can be used with just about any architectural style, it works best with a service-oriented domain driven design-based approach. 

The seed project integrates Fulcrum with Identity Server v3 and Membership Reboot, along with Angular, angular-schema-form, TV4.js, Bootstrap CSS, and other libraries to provide a default project which can be used to kickstart new development. Ideally, a NuGet package installer can be developed which would deploy the seed project into an empty ASP.NET Web Application project.

Getting Started
=======

To see a quick demo

* Clone the repo
* Using SQL Server, create a database called Fulcrum and update the ConnectionString elements in all app/web.config files in the project
* Build
* Run update-database
* Set API Test Harness as startup project & run it, it will launch a browser
* Navigate to /commands or /queries
* You can also use Postman to explore the API and it's necessary to use it when authentication is enabled

Core To-do
=======
1. Refactor CommandModelBinder and QueryController's approach to mapping HTTP requests to objects
1. Make CommandController claims-aware
1. Make QueryController claims-aware
1. Investigate replacing/augmenting log4net with serilog in ILoggingSource and in general
2. Better config management, specifically around the connection string

Seed To-Do
=========
1. Implement Identity Server 3 
1. Implement Membership Reboot
2. Create Angular shell
3. Implement directive to automatically render commands by reading their JSON schema from the API (via a read-through cache) and passing it to angular-schema-form
4. Implement validation of the angular-schema-form forms using the command schema and the TV4.js schema validation library
1. Create login & auth views, controllers, services, etc.
1. Create profile management views, controllers, etc.
1. Create user profile admin views, controllers, etc.
2. Create user authentication/session management views, controllers, etc.
1. Implement log4net with file & database appenders
2. Wire up session log IDs
1. Create log viewer UI to read from log4net database table
2. Implement "My Session" feature - a link in the footer to the log viewer, pre-filtered by the current user's session log ID
1. Install/configure ELMAH, tied to log4net
