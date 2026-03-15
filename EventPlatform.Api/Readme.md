


--------------------------------
# EventPlatform
--------------------------------

EventsPlatform: 3 modules

User Authentication & Authorization Module: 
Secure login/registration for administrators, organizers, and attendees, with role-based access control. 
Event Creation & Management Module: 
Enables organizers to input event details (date, venue, description), create agendas, and manage speaker information. 
Registration & Ticketing Module: 
Allows participants to sign up, select ticket types, and process payments securely.

mkdir EventPlatform
cd EventPlatform

dotnet new sln -n EventPlatform
dotnet new webapi -n EventPlatform.Api -f net10.0
dotnet sln add EventPlatform.Api/EventPlatform.Api.csproj

dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 10.0.5
dotnet add package Microsoft.EntityFrameworkCore.Design --version 10.0.5
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 10.0.5


Error:
-------------------
Microsoft.EntityFrameworkCore.SqlServer package missing.

add Microsoft.EntityFrameworkCore.SqlServer to nugget.config
C:\Users\<username>\AppData\Roaming\NuGet\NuGet.Config

Error: 
---------------
Microsoft.EntityFrameworkCore.Sqlite does support net10.0, so this error usually means one of these is happening: 
You’re installing an older EF Core package version that doesn’t support .NET 10. Your NuGet/Visual Studio/MSBuild is too old 
to understand net10.0. Your EF packages are on mixed versions. EF Core packages should all use the same version.

.NET SQlLite cannot be used with with .NET 10.0.5, so we will switch to SQL Server for development. Update the connection string in appsettings.json and update the DbContext registration in Program.cs:

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); 
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 10.0.5 
dotnet add package Microsoft.EntityFrameworkCore.Design --version 10.0.5 
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 10.0.5



Add package sources to NuGet.Config:
-------------------------------------------
C:\Users\<USERNAME>\AppData\Roaming\NuGet\NuGet.Config

Folder:
----------------
Common/
Data/
Infrastructure/
Modules/Auth/
Modules/Events/
Modules/Registration/

clean build:
-----------------
dotnet clean
dotnet restore
dotnet build


Exception:
------------------
exception when we run the application Microsoft.Data.SqlClient.SqlException: 'Invalid object name 'AspNetRoles'.'

dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update

Migrations:
-----------------------------------

PM> dotnet ef migrations add FixTicketPricePrecision --project EventPlatform.Api
Build started...
Build succeeded.
Done. To undo this action, use 'ef migrations remove'
PM> dotnet ef database update --project EventPlatform.Api
Build started...
Build succeeded.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (12ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT 1
info: Microsoft.EntityFrameworkCore.Migrations[20411]
      Acquiring an exclusive lock for migration application. See https://aka.ms/efcore-docs-migrations-lock for more information if this takes too long.
Acquiring an exclusive lock for migration application. See https://aka.ms/efcore-docs-migrations-lock for more information if this takes too long.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (18ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      DECLARE @result int;
      EXEC @result = sp_getapplock @Resource = '__EFMigrationsLock', @LockOwner = 'Session', @LockMode = 'Exclusive';
      SELECT @result
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (3ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
      BEGIN
          CREATE TABLE [__EFMigrationsHistory] (
              [MigrationId] nvarchar(150) NOT NULL,
              [ProductVersion] nvarchar(32) NOT NULL,
              CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
          );
      END;
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT 1
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT OBJECT_ID(N'[__EFMigrationsHistory]');
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (12ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [MigrationId], [ProductVersion]
      FROM [__EFMigrationsHistory]
      ORDER BY [MigrationId];
info: Microsoft.EntityFrameworkCore.Migrations[20402]
      Applying migration '20260315112101_InitialCreate'.
Applying migration '20260315112101_InitialCreate'.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (9ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [AspNetRoles] (
          [Id] uniqueidentifier NOT NULL,
          [Name] nvarchar(256) NULL,
          [NormalizedName] nvarchar(256) NULL,
          [ConcurrencyStamp] nvarchar(max) NULL,
          CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (4ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [AspNetUsers] (
          [Id] uniqueidentifier NOT NULL,
          [FullName] nvarchar(max) NOT NULL,
          [UserName] nvarchar(256) NULL,
          [NormalizedUserName] nvarchar(256) NULL,
          [Email] nvarchar(256) NULL,
          [NormalizedEmail] nvarchar(256) NULL,
          [EmailConfirmed] bit NOT NULL,
          [PasswordHash] nvarchar(max) NULL,
          [SecurityStamp] nvarchar(max) NULL,
          [ConcurrencyStamp] nvarchar(max) NULL,
          [PhoneNumber] nvarchar(max) NULL,
          [PhoneNumberConfirmed] bit NOT NULL,
          [TwoFactorEnabled] bit NOT NULL,
          [LockoutEnd] datetimeoffset NULL,
          [LockoutEnabled] bit NOT NULL,
          [AccessFailedCount] int NOT NULL,
          CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (3ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [Events] (
          [Id] uniqueidentifier NOT NULL,
          [Title] nvarchar(200) NOT NULL,
          [Description] nvarchar(max) NOT NULL,
          [DateUtc] datetime2 NOT NULL,
          [Venue] nvarchar(200) NOT NULL,
          [OrganizerId] uniqueidentifier NOT NULL,
          CONSTRAINT [PK_Events] PRIMARY KEY ([Id])
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (3ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [Registrations] (
          [Id] uniqueidentifier NOT NULL,
          [EventId] uniqueidentifier NOT NULL,
          [TicketTypeId] uniqueidentifier NOT NULL,
          [UserId] uniqueidentifier NOT NULL,
          [RegisteredAtUtc] datetime2 NOT NULL,
          [PaymentStatus] nvarchar(max) NOT NULL,
          [PaymentReference] nvarchar(max) NOT NULL,
          CONSTRAINT [PK_Registrations] PRIMARY KEY ([Id])
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (10ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [AspNetRoleClaims] (
          [Id] int NOT NULL IDENTITY,
          [RoleId] uniqueidentifier NOT NULL,
          [ClaimType] nvarchar(max) NULL,
          [ClaimValue] nvarchar(max) NULL,
          CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
          CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (3ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [AspNetUserClaims] (
          [Id] int NOT NULL IDENTITY,
          [UserId] uniqueidentifier NOT NULL,
          [ClaimType] nvarchar(max) NULL,
          [ClaimValue] nvarchar(max) NULL,
          CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
          CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (7ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [AspNetUserLogins] (
          [LoginProvider] nvarchar(450) NOT NULL,
          [ProviderKey] nvarchar(450) NOT NULL,
          [ProviderDisplayName] nvarchar(max) NULL,
          [UserId] uniqueidentifier NOT NULL,
          CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
          CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [AspNetUserRoles] (
          [UserId] uniqueidentifier NOT NULL,
          [RoleId] uniqueidentifier NOT NULL,
          CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
          CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
          CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (3ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [AspNetUserTokens] (
          [UserId] uniqueidentifier NOT NULL,
          [LoginProvider] nvarchar(450) NOT NULL,
          [Name] nvarchar(450) NOT NULL,
          [Value] nvarchar(max) NULL,
          CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
          CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [AgendaItems] (
          [Id] uniqueidentifier NOT NULL,
          [EventId] uniqueidentifier NOT NULL,
          [Title] nvarchar(max) NOT NULL,
          [StartUtc] datetime2 NOT NULL,
          [EndUtc] datetime2 NOT NULL,
          CONSTRAINT [PK_AgendaItems] PRIMARY KEY ([Id]),
          CONSTRAINT [FK_AgendaItems_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events] ([Id]) ON DELETE CASCADE
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (4ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [Speakers] (
          [Id] uniqueidentifier NOT NULL,
          [EventId] uniqueidentifier NOT NULL,
          [Name] nvarchar(max) NOT NULL,
          [Bio] nvarchar(max) NOT NULL,
          CONSTRAINT [PK_Speakers] PRIMARY KEY ([Id]),
          CONSTRAINT [FK_Speakers_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events] ([Id]) ON DELETE CASCADE
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (4ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [TicketTypes] (
          [Id] uniqueidentifier NOT NULL,
          [EventId] uniqueidentifier NOT NULL,
          [Name] nvarchar(max) NOT NULL,
          [Price] decimal(18,2) NOT NULL,
          [QuantityAvailable] int NOT NULL,
          CONSTRAINT [PK_TicketTypes] PRIMARY KEY ([Id]),
          CONSTRAINT [FK_TicketTypes_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events] ([Id]) ON DELETE CASCADE
      );
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (10ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE INDEX [IX_AgendaItems_EventId] ON [AgendaItems] ([EventId]);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (4ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE INDEX [IX_Speakers_EventId] ON [Speakers] ([EventId]);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE INDEX [IX_TicketTypes_EventId] ON [TicketTypes] ([EventId]);
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (4ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
      VALUES (N'20260315112101_InitialCreate', N'10.0.5');
info: Microsoft.EntityFrameworkCore.Migrations[20402]
      Applying migration '20260315112412_FixTicketPricePrecision'.
Applying migration '20260315112412_FixTicketPricePrecision'.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
      VALUES (N'20260315112412_FixTicketPricePrecision', N'10.0.5');
Done.info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (3ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      DECLARE @result int;
      EXEC @result = sp_releaseapplock @Resource = '__EFMigrationsLock', @LockOwner = 'Session';
      SELECT @result

PM> 


Full Test Flow
-------------------------------

Register Organizer
        ↓
Login Organizer
        ↓
Create Event
        ↓
Add Ticket
        ↓
Register Attendee
        ↓
Register for Event
        ↓
Check Registrations


## // 5089

# 1️ Register Organizer

curl -X POST http://localhost:5089/api/auth/register \
-H "Content-Type: application/json" \
-d '{
  "fullName": "John Organizer",
  "email": "organizer@test.com",
  "password": "Pass@1234",
  "role": "Organizer"
}'

Expected response:

{
  "token": "JWT_TOKEN_HERE",
  "email": "organizer@test.com",
  "fullName": "John Organizer",
  "roles": ["Organizer"]
}

{
	"token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI0MjFkODVlNS1mN2FlLTQ5MmUtODIwOC1jNmNmOGMwZmVjN2MiLCJlbWFpbCI6Im9yZ2FuaXplckB0ZXN0LmNvbSIsInVuaXF1ZV9uYW1lIjoib3JnYW5pemVyQHRlc3QuY29tIiwiZnVsbE5hbWUiOiJKb2huIE9yZ2FuaXplciIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Ik9yZ2FuaXplciIsImV4cCI6MTc3MzU4MjUwOCwiaXNzIjoiRXZlbnRQbGF0Zm9ybSIsImF1ZCI6IkV2ZW50UGxhdGZvcm0uQ2xpZW50In0.8gFpzW66S4P4ZcHLf4s8_2nOam666m5UR-gOYl8RglI",
	"email": "organizer@test.com",
	"fullName": "John Organizer",
	"roles": [
		"Organizer"
	]
}

# 2️ Login Organizer

curl -X POST http://localhost:5089/api/auth/login \
-H "Content-Type: application/json" \
-d '{
  "email": "organizer@test.com",
  "password": "Pass@1234"
}'

{
	"token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI0MjFkODVlNS1mN2FlLTQ5MmUtODIwOC1jNmNmOGMwZmVjN2MiLCJlbWFpbCI6Im9yZ2FuaXplckB0ZXN0LmNvbSIsInVuaXF1ZV9uYW1lIjoib3JnYW5pemVyQHRlc3QuY29tIiwiZnVsbE5hbWUiOiJKb2huIE9yZ2FuaXplciIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Ik9yZ2FuaXplciIsImV4cCI6MTc3MzU4MjYzNiwiaXNzIjoiRXZlbnRQbGF0Zm9ybSIsImF1ZCI6IkV2ZW50UGxhdGZvcm0uQ2xpZW50In0.N15_v4k3cUxylVNn8W20NGhxXyYVv6euFZ_ihfQZzs4",
	"email": "organizer@test.com",
	"fullName": "John Organizer",
	"roles": [
		"Organizer"
	]
}


# 3 Create Event

curl -X POST http://localhost:5089/api/events \
-H "Authorization: Bearer ORGANIZER_TOKEN" \
-H "Content-Type: application/json" \
-d '{
  "title": "Dotnet Summit 2026",
  "description": "Cloud Architecture Event",
  "dateUtc": "2026-06-10T09:00:00Z",
  "venue": "Hyderabad"
}'

{
 "id": "EVENT_ID",
 "title": "Dotnet Summit 2026"
}


{
	"id": "7d4a67cd-f8e3-49a8-a3b4-9d6bae10d8f2",
	"title": "Dotnet Summit 2026",
	"description": "Cloud Architecture Event",
	"dateUtc": "2026-06-10T09:00:00Z",
	"venue": "Hyderabad",
	"organizerId": "421d85e5-f7ae-492e-8208-c6cf8c0fec7c",
	"agendaItems": [],
	"speakers": [],
	"ticketTypes": []
}

EVENT_ID=xxxx  <-- copy.

# 4 Add Ticket Type

ATTENDEE_TOKEN=xxxxx

curl -X POST http://localhost:5089/api/events/EVENT_ID/tickets \
-H "Authorization: Bearer ORGANIZER_TOKEN" \
-H "Content-Type: application/json" \
-d '{
  "name": "Standard Ticket",
  "price": 100,
  "quantityAvailable": 50
}'


{
 "id": "TICKET_ID",
 "name": "Standard Ticket"
}

http://localhost:5089/api/events/7d4a67cdf8e349a8a3b49d6bae10d8f2/tickets

{
  "name": "Standard Ticket",
  "price": 100,
  "quantityAvailable": 50
}

error:
-----------
System.Text.Json.JsonException: A possible object cycle was detected. 
This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32.

Solution: Add TicketTypeResponse

{
	"id": "e54ac86b-0c2d-4b4e-9aa1-db45975d6086",
	"eventId": "7d4a67cd-f8e3-49a8-a3b4-9d6bae10d8f2",
	"name": "Standard Ticket",
	"price": 100,
	"quantityAvailable": 50
}


# 5. Register Attendee

ATTENDEE_TOKEN=xxxxx

curl -X POST http://localhost:5089/api/auth/register \
-H "Content-Type: application/json" \
-d '{
  "fullName": "Alice Attendee",
  "email": "alice@test.com",
  "password": "Pass@1234",
  "role": "Attendee"
}'

{
	"token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjM2JjNDRkZi0yMTZlLTRmYmUtYjFlOC05NGM1YWYzYWRkMTYiLCJlbWFpbCI6ImFsaWNlQHRlc3QuY29tIiwidW5pcXVlX25hbWUiOiJhbGljZUB0ZXN0LmNvbSIsImZ1bGxOYW1lIjoiQWxpY2UgQXR0ZW5kZWUiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBdHRlbmRlZSIsImV4cCI6MTc3MzU4MzU3MCwiaXNzIjoiRXZlbnRQbGF0Zm9ybSIsImF1ZCI6IkV2ZW50UGxhdGZvcm0uQ2xpZW50In0.umFh4-rl8sh-LMllTsznXjI1BCToUlOQSVEqqpr5MaM",
	"email": "alice@test.com",
	"fullName": "Alice Attendee",
	"roles": [
		"Attendee"
	]
}



# 6. Register for Event

ATTENDEE_TOKEN=xxxxx


# make sure <token> belongs to a user with role Attendee or Administrator.

curl -X POST http://localhost:5000/api/auth/login \
-H "Content-Type: application/json" \
-d '{
  "email": "alice@test.com",
  "password": "Pass@1234"
}'




curl -X POST http://localhost:5089/api/registrations \
-H "Authorization: Bearer ATTENDEE_TOKEN" \
-H "Content-Type: application/json" \
-d '{
  "eventId": "EVENT_ID",
  "ticketTypeId": "TICKET_ID",
  "paymentMethod": "fake-card",
  "paymentToken": "tok_test_123"
}'

sample:
{
 "id": "REGISTRATION_ID",
 "eventId": "...",
 "ticketTypeId": "...",
 "paymentStatus": "Paid"
}


{
	"id": "37253fca-a9f5-4ec3-bfc2-ca5b1b88a25d",
	"eventId": "7d4a67cd-f8e3-49a8-a3b4-9d6bae10d8f2",
	"ticketTypeId": "e54ac86b-0c2d-4b4e-9aa1-db45975d6086",
	"userId": "c3bc44df-216e-4fbe-b1e8-94c5af3add16",
	"registeredAtUtc": "2026-03-15T12:17:11.0843924Z",
	"paymentStatus": "Paid",
	"paymentReference": "PAY-18c9e96c4731443a8ef2105cacc6c39e"
}

7. View my registrations

curl -X GET http://localhost:5089/api/registrations/my \
-H "Authorization: Bearer ATTENDEE_TOKEN"

sample:
[
 {
   "id": "REGISTRATION_ID",
   "eventId": "...",
   "ticketTypeId": "...",
   "paymentStatus": "Paid"
 }
]

[
	{
		"id": "37253fca-a9f5-4ec3-bfc2-ca5b1b88a25d",
		"eventId": "7d4a67cd-f8e3-49a8-a3b4-9d6bae10d8f2",
		"ticketTypeId": "e54ac86b-0c2d-4b4e-9aa1-db45975d6086",
		"userId": "c3bc44df-216e-4fbe-b1e8-94c5af3add16",
		"registeredAtUtc": "2026-03-15T12:17:11.0843924",
		"paymentStatus": "Paid",
		"paymentReference": "PAY-18c9e96c4731443a8ef2105cacc6c39e"
	},
	{
		"id": "73b2b906-71a1-4fde-bd6e-26d8f58bdea4",
		"eventId": "7d4a67cd-f8e3-49a8-a3b4-9d6bae10d8f2",
		"ticketTypeId": "e54ac86b-0c2d-4b4e-9aa1-db45975d6086",
		"userId": "c3bc44df-216e-4fbe-b1e8-94c5af3add16",
		"registeredAtUtc": "2026-03-15T12:17:03.5462758",
		"paymentStatus": "Paid",
		"paymentReference": "PAY-ca9037c485574599bd7622ecff8fd9f0"
	}
]

8. List Events (Public) 

curl http://localhost:5000/api/events

sample:
[
 {
   "id": "EVENT_ID",
   "title": "Dotnet Summit 2026",
   "venue": "Hyderabad"
 }
]

[
	{
		"id": "7d4a67cd-f8e3-49a8-a3b4-9d6bae10d8f2",
		"title": "Dotnet Summit 2026",
		"description": "Cloud Architecture Event",
		"dateUtc": "2026-06-10T09:00:00",
		"venue": "Hyderabad",
		"organizerId": "421d85e5-f7ae-492e-8208-c6cf8c0fec7c",
		"agendaItems": [],
		"speakers": [],
		"ticketTypes": [
			{
				"id": "ebf94bfd-7635-4c63-b5a0-3db33e3055f5",
				"eventId": "7d4a67cd-f8e3-49a8-a3b4-9d6bae10d8f2",
				"name": "Standard Ticket",
				"price": 100.00,
				"quantityAvailable": 50
			},
			{
				"id": "4125fd7d-37cf-4f5e-b3b1-d35da285cd55",
				"eventId": "7d4a67cd-f8e3-49a8-a3b4-9d6bae10d8f2",
				"name": "Standard Ticket",
				"price": 100.00,
				"quantityAvailable": 50
			},
			{
				"id": "e54ac86b-0c2d-4b4e-9aa1-db45975d6086",
				"eventId": "7d4a67cd-f8e3-49a8-a3b4-9d6bae10d8f2",
				"name": "Standard Ticket",
				"price": 100.00,
				"quantityAvailable": 48
			}
		]
	}
]


# Copilot Instructions

## General Guidelines
- First general instruction
- Second general instruction

## Database Seeding
- When seeding the database in this codebase, call `SeedData.InitializeAsync(IServiceProvider)` — `Program.InitializeAsync` does not exist.