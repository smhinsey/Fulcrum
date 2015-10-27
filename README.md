Getting Started
=======

To see a quick demo

* Pull code
* Create database called Fulcrum (or update connection strings in app/web.configs)
* Build
* Run update-database
* Set API Test Harness as startup project & run it, it will launch a browser
* Navigate to /commands or /queries
* You can also use Postman to explore the API and it's necessary to use it when authentication is enabled


Goals
======

Fulcrum provides an HTTP-based self-describing API as well as the runtime infrastructure necessary to publish commands and execute queries.

The seed project will integrate Fulcrum with Identity Server v3 and Membership Reboot, along with Angular, angular-schema-form, TV4.js, Bootstrap CSS, and other libraries to provide a default project which can be used to kickstart new development.

Core To-do
=======
1. Refactor CommandModelBinder and QueryController's approach to mapping HTTP requests to objects
1. Make CommandController claims-aware
1. Make QueryController claims-aware
1. Investigate replacing/augmenting log4net with serilog in ILoggingSource

Seed To-Do
=========
1. Implement Identity Server 3 
1. Implement Membership Reboot
1. Create Angular login & auth views, controllers, services, etc.
1. Create profile management views, controllers, etc.
1. Implement logging
1. Install/configure ELMAH, tied to logging
1. Set up a log viewer UI
