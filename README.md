# IdentityJwtApi
Web Api project that implements Identity and JWT

This is a POC for starting a new project with Entity Framework, Identity, Jwt.

# IIS local
Install IIS localy 
Follow this documentation : https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/development-time-iis-support?view=aspnetcore-2.1
Rename your application URL and launch URL by {your_ApplicationName}
Open IIS. Go to application pools, right click on yours and select Advanced Settings : Change the Identity to LocalSystem. https://i.stack.imgur.com/toz1W.png
On SqlServer (create a Database) you have to allow Allow AUTHORITY/SYSTEM to server Role as sysadmin :https://stackoverflow.com/questions/24822076/sql-server-login-error-login-failed-for-user-nt-authority-system?answertab=active#tab-top

# ENTITY FRAMEWORK.
For the database, we use EntityFramework code first.
You can see how to do that here https://docs.microsoft.com/en-us/ef/core/get-started/aspnetcore/new-db
We use the Entity Framework convention names for the table relationship.
For the migration (create a model snapshot), we have to specify the project assembly. You can see it at the startup file ( MigrationsAssembly("WebApplication1") ).
See this documentation to handle your Database with EF: https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/

# IDENTITY.
For Identity: check this documentation : https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?tabs=visual-studio%2Caspnetcore2x
And this page for Account confirmation with email sending:  https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?tabs=aspnetcore2x

# JWT.
We also implement Jwt: Json Web Token for the security.
You can check this tutorial to learn how we implement it for this POC: https://fullstackmark.com/post/13/jwt-authentication-with-aspnet-core-2-web-api-angular-5-net-core-identity-and-facebook-login 

# DEPENDENCY INJECTION.
We use the Dependency Injection to inject our repositories and services or something else into other classes.
You can see the official documentation : https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection

# INIT DATABSE.
We init some datas (Data folder) to the DB by  adding some users and roles at the begining. You can see it at the “Seed” region from startup file.
It is commented, you have to do a migration before to init youre tables and then activate this code to init some Datas to the DB (do that to init roles for exemple)
See this : https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/

# SWAGGER API documentation
We implement swagger for th API documentation: https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-2.1&tabs=visual-studio
The URL is http://localhost:<random_port>/swagger


# CONFIGURATION FILE
The configuration file is appsetteings.json : You have to change it for your environment (connection string, for exemple).

#
This POC demonstrates an exemple of implemention for the login: You can register an user with the accountController, then you can log in with the AuthController, you will receive a JSON response with the JWT Token that you have to retrieve. Then you can navigate to the dashboardController with your JWT token. You have to put this token into the header of the request (check the jwt tutorial for more information). The configuration of Jwt (Expery time of the token for exemple) is written on configuration file.

You can test all the process with Postman.
This solution has been released with VS2017 or VS2019 professional, .NET Core 2.1, AspNetCore.Identity.EntityFrameworkCo, and EntityFrameworkCore.

