# ResourceBookingSystem

Simple resource booking web app built with .NET 10 and Blazor server components.

Prerequisites
- .NET 10 SDK

Run locally
1. Restore packages:
   ``dotnet restore``
2. Build the solution:
   ``dotnet build``
3. Apply database migrations:
   ``dotnet ef database update --project Domain\Domain.csproj --startup-project ResourceBookingSystem\ResourceBookingSystem.csproj --context Domain.DatabaseContext``
4. Run the web app (from repo root):
   ``dotnet run --project ResourceBookingSystem\ResourceBookingSystem.csproj``

The app will print the local URLs (e.g. http://localhost:5113) in the console — open one in your browser.

Run tests
- ``dotnet run --project ResourceBookingSystemTests\ResourceBookingSystemTests.csproj``  
  This runs in the in-process xUnit runner.

### Design decisions:
- The app was built on the Blazor starter project template for easier building.
- Blazor Server was chosen for faster development (no extra API to be created)
- SQLite was chosen for it's simple deployment. This can be switched by providing a different DatabaseContext.
- Domain project is created to store entity models, possible API call response models
- Data project is for data interaction (repositories, services)
- All services are using injection for simple development
- Disabled resources can not be seen from the resource view, or used in bookings.

### Booking conflicts

- All of the new booking requests are sent through a validator, that checks whether the selected time is available.
- Validator checks, if the booking in any way is overlapping with the existing bookings.
- If the booking is in conflict, an error is shown to the user. Otherwise, nothing happens and it will be saved.

### My improvements

- I would first add some sort of authorization management to this project, so the regular users would not see any admin view panel. Currently, all users can see the "admin view" option on the sidebar.
- CSS to make the design better (currently there is almost no CSS added to the UI)
- If the project gets larger, separate logic into an API to call from the UI (remove the always-online connection)
- Speed up initial load. When first opening a page that triggers a database call, the page waits for 1 second. All other openings are instant.

### AI use
- AI was largely used to understand Blazor architecture, this was new for me.
- Also, all tests are initially created with AI, with validation/cleanup/error fixes/optimisations done afterwards.