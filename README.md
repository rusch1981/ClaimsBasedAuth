# ClaimsBasedAuth
## Server

Currently configureable to automatically authenticate users using Windows Authentication.  If Windows authentication fails the user is the prompted 
to login (active directory) or re-attempt automatic login via the "Windows Authentication" shown on the screen.  

### Running for the First Time
* Clone this repo.
* Remove InitializeDatabase() method call and method in Startup.Configure().  This is only present to show how the Initial setup was achieved.
* Update the "IdentityServerDatabase" Connection String in the appsetting.json file
* Restore the database using the scripts/IdentityServer.bak
* Run Create... and Set... stored procedures found in the scripts directory and described [here](#Setting-up-Users-and-Client-in-the-Database).
* Configure a Client in the sample directory by setting the Startup.cs ConfigureServices().  GrantTypes, Secrets, Scopes/Resources must between 
Client, and Client, Identity and User Database configuration.

You should now be able to browse to the discovery document and test the server login using the  MVC_Client project. 
See **Users and UserStores** below for additional information on user configuration.  

### Initial Setup Notes:

Full Tutorial can be found [here.](http://docs.identityserver.io/en/release/quickstarts/8_entity_framework.html)

Created from an Empty Core 2.1 Webapp

Install: IdentityServer4, IdentityServer4.EntityFramework, Microsoft.EntityFrameworkCore.Tools.DotNet,
Microsoft.Extensions.Configuration, and System.DirectoryServices.AccountManagement.

Logging install:  Serilog.AspNetCore, Serilog.Sinks.Console, and Serilog.Sinks.File 

Install Entity Framework Core Tools as explained [here.](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet)

Run below to test Net Entity Framework tooling is istalled.  
```cmd
dotnet ef 
```

Copy in QuickstartIdentityServer directory's from [Quickstart 8](https://github.com/IdentityServer/IdentityServer4.Samples/tree/release/Quickstarts/8_EntityFrameworkStorage/src/QuickstartIdentityServer)
and set your connection string with userId and password.  

Run the migration Commands from the project directory
```cmd
dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
```

At this point the Identity Server Database will be created when you run the project.  The sample data used to populate the database is pulled from the 
TestUsers.cs file.

Set up user tables and some sample data using the IdSrvUserTearDownSetUp.sql.

Migrate EF data model to project.  
```cmd
Scaffold-DbContext "Data Source=localhost;database=IdentityServer;trusted_connection=yes;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
```
***Caution**: by scaffolding the model the dbcontext class autmatically generates\d a connection string that was being used.
Instead the dbcontext should be injected and passed the connection string via appsettings.json  (see Startup.cs for example).*

### Automatic Windows Authentication Configuration
To configure Automatic Windows Authentication set 'Autodetect' to 'true'
``` json
appsettings.json
"LoginConfiguration": {
    "Autodetect" :  "false"
```

### Logging
The current configuration is set to log to console and write to log file.  You can configure logging in the Program.cs Main Method.  

### Enabling CORS
CORS is enabled per Client Configuration or globaly by implementing ICorsPolicyService.  More information can be found [here.](http://docs.identityserver.io/en/release/topics/cors.html)

### Users UserStores and ProfileServices
IUserStore.cs is used to search, validate, etc. users.  IUserStore is injected in the Startup.cs.  
The implementation [ActiveDirectoryUserStore.cs](IdetityServer/UserStores/ActiveDirectoryUserStore.cs) uses Active Directory.  In order for of 
*ActiveDirectoryUserStore.cs* to build correctly the "User*" tables must be created in the IdentityServer Data base for this project.  
The Implementation of [InMemoryUserStore.cs](IdetityServer/UserStores/InMemoryUserStore.cs) can be used with out the use of "User*" tables and is based on
static users.

IProfileService.cs is used retreive user specific claims based on client context.  IProfileService is injected in the Startup.cs.  
The implementation [ActiveDirectoryProfileService.cs](IdetityServer/ProfileServices/ActiveDirectoryProfileService.cs) uses Active Directory.  In order for of 
*ActiveDirectoryProfileService.cs* to build correctly the "User*" tables must be created in the IdentityServer Data base for this project.  
The Implementation of [InMemoryProfileService.cs](IdetityServer/ProfileServices/InMemoryProfileService.cs) can be used with out the use of "User*" tables and 
is based onstatic users.

*Note - IUserStore must be configured in conjunction with correct implementation of IProfileService also
located in [Startup.cs](IdetityServer/Startup.cs).  The Automatic Windows Authentication Configuration may not work with 
InMemoryUserStore.cs and InMemoryProfileService.cs if not configured correctly*


### Setting up Users, Clients and Identities in the Database

[Database Restore File](scripts/IdentityServer.bak)

Scripts to assist with data creation:
* [IdSrvClientSetUp.sql](scripts/IdSrvClientSetUp.sql)
* [IdSrvUserSetUp.sql](scripts/IdSrvUserSetUp.sql)

Stored Procedures included in the [Database Restore File](scripts/IdentityServer.bak) to Assist with Data Creation
* [sp_CreateClient.sql](scripts/sp_CreateClient.sql)
* [sp_CreateUser.sql](scripts/sp_CreateUser.sql)
* [sp_SetClientDetails.sql](scripts/sp_SetClientDetails.sql)
* [sp_SetClientSecret.sql](scripts/sp_SetClientSecret.sql)

#### General Data Table Relationships
The Users Tables provided are custom built and not provided by the makers of Identity Server 4.  The Clients and Identities Tables 
were generated via EF.  

Identity, and API Claims are just key/value resources (data values) we are protecting.  Claims belong to and 
define Resources.  Users, API's and Clients can all have Claims.  Clients are different in that they are the resource and they can
also have properties.  Client claims belong to the client not a resource.  Client Scopes are the allowed user, and api resources that 
a specific Client is allowed to access.  When configuring Identity Server Database, resources and claims tables will need to be define, 
then the client should be defined to allow access to the resources/claims.  Once the database is configured, then the webapp client can 
physically request and retrieve the resource for use.


##### Users Tables
* Users - System users that will login (typically people)
* UserRoles - User Claims(data key/values)
* UsersClientsRoles - Assignment of Roles to Users by Client.  (Many to Many to Many)

##### Clients Tables
* Clients - Webapps
* ClientScopes - the api and user resources that a client allows
* ClientSecrets - Secrets that a physical webapp client can use to authenticate with Identity Server 
* ClientRedirectUris and PostClientRedirectUries - The Uris that a physical webapp client can use when requesting call back.
* ClientClaims and ClientProperties - the protected data key/values of a client (will be included in the access token).
* ClientCorsOrigins - If specified, will be used by the default CORS policy service implementations (In-Memory and EF) to build
a CORS policy for JavaScript clients.
* ClientIdPRestrictions - Specifies which external IdPs can be used with this client (if list is empty all IdPs are allowed). 
Defaults to empty. 
* ClientGrantTypes - Grant types are a way to specify how a client wants to interact with IdentityServer. The OpenID Connect and
OAuth 2 specs define the below grant types.  
  * Implicit
  * Authorization code
  * Hybrid
  * Client credentials
  * Resource owner password
  * Refresh tokens
  * Extension grants

More information on Grant Types can be found [here](http://docs.identityserver.io/en/release/topics/grant_types.html).

*Note:  When defining GrantTypes in the Database for MVC you will most typically use "hybrid" (lower case).  Hybrid will allow use securce 
backchannel using a combination of implicit and authorization code flow.  The client secret will also be utilized during authentication*  

##### Identitys Tables
* IdentityClaims - List of associated user claim types that should be included in the identity token
* IdentityResources - Defined by the Identity Claims and the value that is used by client to request those claims.

##### Api Tables
* ApiResources - Defined by the Api Claims and the way value that is used by client to request those claims.
* ApiClaims - List of associated Api claim types that should be included in the Api access token
* ApiScopesClaims - the user claims that define the ApiScopes that a client can request
* ApiScopes - user resources that can be requested by and client and should be included in the Api access token  
* ApiSecrets - The API secret is used for the introspection endpoint. The API can authenticate with introspection using 
the API name and secret.

#### Client Secret Hash

When inserting a *Secret* in the *ClientSecrets* table you can use the following code to create your
hash:
``` c#
using System;
using System.Security.Cryptography;
using System.Text;
					
public class Program
{
    public static void Main()
    {
        using (var sha = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes("secret");
            var hash = sha.ComputeHash(bytes);
            string hashForDatabase = Convert.ToBase64String(hash);
		
	    Console.WriteLine(hashForDatabase);
	}
    }
}
```
 
## Samples

### MVC_Client (Core)

Create Empty Core 2.1 Webapp

Install IdentityModel

Copy in MVCClient directories from [Quickstart 8](https://github.com/IdentityServer/IdentityServer4.Samples/tree/release/Quickstarts/8_EntityFrameworkStorage/src/MvcClient)
and Edit namespace of all files.  
  
How to configure authentication with Identity Server 4 can be seen in the Startup.cs.   


[ClaimsTroubleShooting 2.0](https://leastprivilege.com/2017/11/15/missing-claims-in-the-asp-net-core-2-openid-connect-handler/)
and [ClaimsTroubleShooting 2.1](https://leastprivilege.com/2018/06/14/improvements-in-claim-mapping-in-the-asp-net-core-2-1-openid-connect-handler/)
https://github.com/IdentityServer/IdentityServer4/issues/2213


### MVCIdentityServer (Full Framework)

Create Empty Full FrameWork Webapp

Install IdentityModel

Set up MVCClient directories as in [Getting Started: MVC Authentication & Web APIs](https://identityserver.github.io/Documentation/docsv2/overview/mvcGettingStarted.html).  

**Notes:** 
  
How to configure authentication with Identity Server 4 can be seen in the Startup.cs.   