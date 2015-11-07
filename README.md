Purpose
======

Fulcrum provides a self-describing HTTP API to browse, validate, and publish commands, and also to browse and execute queries. These commands and queries represent an application's capabilities by expressing operations which modify its internal state as commands and operations which return its internal state as queries. While Fulcrum can be used with just about any architectural style, it works best with a service-oriented domain driven design-based approach. 

The seed project integrates Fulcrum with Identity Server v3 and Membership Reboot, along with Angular, Angular UI, angular-schema-form, TV4.js, Bootstrap CSS, and other libraries to provide a default project which can be used to kickstart new development. Ideally, a NuGet package installer can be developed which would deploy the seed project into an empty ASP.NET Web Application project.

Using the Seed Project
=======

Eventually, I'd like to have a script for taking the seed project and renaming it, for now, you can rename the projects and namespaces manually.

* Clone the repo
* Using SQL Server, create a database and verify that the following 3 connection strings point to it, as they appear in WebApi and WebUI's web.configs: MembershipReboot, FulcrumSeedDb, and CommandPipelineDbContext.
* Set the FulcrumSeed.WebUI project as the startup project
* Press CTRL-F5 for Launch without Debugging
* By default, the system seeds the account testAdmin@example.com/password, which you can log in as.
* You can the Fulcrum API at /api/commands and /api/queries, but you'll need to use Postman to set up a bearer token header to publish commands or run queries.

Near-Term To-do
=========
1. ~~Implement Identity Server 3 in Seed~~
1. ~~Implement Membership Reboot in Seed~~
1. ~~Create Angular shell for Seed~~
1. ~~Implement directive to automatically render commands by reading their JSON schema from the API (via a read-through cache) and passing it to angular-schema-form for Seed~~
1. ~~Implement validation of the angular-schema-form forms using the command schema and the TV4.js schema validation library for Seed~~
1. ~~Create login & auth views, controllers, services, etc. for Seed~~
1. Make CommandController claims-aware
1. Make QueryController claims-aware
1. Create profile management Angular views, controllers, etc. for Seed
1. Create user profile admin Angular views, controllers, etc. for Seed
1. Create user authentication/session management Angular views, controllers, etc. for Seed
1. Implement log4net with file & database appenders for Seed
1. Wire up session log IDs for Seed
1. Create log viewer UI to read from log4net database table for Seed
1. Implement "My Session" feature - a link in the footer to the log viewer, pre-filtered by the current user's session log ID for Seed
1. Install/configure ELMAH, tied to log4net for Seed


Long-Term To-do
=======
1. Refactor CommandModelBinder and QueryController's approach to mapping HTTP requests to objects
1. Create custom AuthorizeAttribute with on/off config switch
1. Investigate replacing/augmenting log4net with serilog in ILoggingSource and in general
1. Better config management, specifically around the connection string
1. Refactor SimpleCommandPipeline, implement pure async
1. Investigate a way to have IQuery implementations return inline/anonymous projections rather than requiring often-redundant projection types.
1. Verify that query-based command validation still works and implement anything Seed needs to take advantage of it