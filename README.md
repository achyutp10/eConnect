# eConnect

## Description

eConnect is a social networking application built with ASP.NET Core Web API and Angular. It allows users to connect with each other, send messages, like profiles, and manage their accounts. Administrators can manage users and moderate photos.

## Features and Functionality

-   **User Authentication:**
    -   Registration and login functionality using ASP.NET Core Identity.
    -   Token-based authentication using JWT.
    -   User roles (Member, Admin, Moderator).
-   **User Profiles:**
    -   Users can create and update their profiles with information like `KnownAs`, `Gender`, `DateOfBirth`, `City`, `Country`, `Introduction`, `Interests`, and `LookingFor`.
    -   Profile pictures can be uploaded and managed using Cloudinary integration.
-   **Like System:**
    -   Users can "like" other user profiles.
    -   Retrieval of users who liked a profile or were liked by a profile.
-   **Messaging:**
    -   Real-time messaging using SignalR.
    -   Private message threads between users.
    -   Message containers (Inbox, Outbox, Unread).
    -   Message deletion.
-   **Admin Features:**
    -   User management (viewing and editing roles).
    -   Photo moderation.
-   **Error Handling:**
    -   Global exception handling middleware.
    -   Custom API exception responses.
    -   Error interceptor in the Angular client.
-   **Pagination:**
    -   Server-side pagination for user lists, likes, and messages.
-   **Real-time Presence:**
    -   Online/offline status using SignalR.
-   **Role-Based Authorization:**
    -   Specific endpoints and features are protected by role-based authorization policies.
-   **Image Uploading**
    -   Upload images to Cloudinary storage service using `API/Services/PhotoService.cs`

## Technology Stack

### Backend (ASP.NET Core Web API)

-   ASP.NET Core 8.0
-   ASP.NET Core Identity
-   Entity Framework Core (with SQL Server)
-   AutoMapper
-   Cloudinary .NET SDK
-   JWT Authentication
-   SignalR
-   FluentValidation
-   Swashbuckle/Swagger for API documentation
-   .NET 8

### Frontend (Angular)

-   Angular 17
-   TypeScript
-   ngx-bootstrap
-   ng-gallery
-   ngx-spinner
-   ng2-file-upload
-   Font Awesome
-   Timeago

## Prerequisites

-   .NET SDK 8.0 or later
-   Node.js and npm
-   SQL Server
-   Cloudinary account (for image storage)
-   Angular CLI

## Installation Instructions

### Backend

1.  Clone the repository:

    ```bash
    git clone https://github.com/achyutp10/eConnect.git
    cd eConnect/API
    ```

2.  Update the database connection string in `appsettings.json`. For example:

    ```json
    "ConnectionStrings": {
        "EConnectConnectionString": "Data Source=your_server;Initial Catalog=EConnectDB;Integrated Security=True;TrustServerCertificate=True"
    }
    ```

3.  Apply database migrations:

    ```bash
    dotnet ef database update
    ```

4.  Seed initial data (users and roles):

    ```bash
    dotnet run seed
    ```

5.  Configure Cloudinary settings in `appsettings.json`:

    ```json
     "CloudinarySettings": {
        "CloudName": "your_cloud_name",
        "ApiKey": "your_api_key",
        "ApiSecret": "your_api_secret"
     }
    ```

6.  Run the API:

    ```bash
    dotnet run
    ```

### Frontend

1.  Navigate to the client directory:

    ```bash
    cd ../client
    ```

2.  Install dependencies:

    ```bash
    npm install
    ```

3.  Configure the API URL in `src/environments/environment.ts` and `src/environments/environment.development.ts`:

    ```typescript
    // src/environments/environment.development.ts
    export const environment = {
      production: false,
      apiUrl: 'https://localhost:7283/api/', // Adjust port if necessary
      hubsUrl: 'https://localhost:7283/hubs/' // Adjust port if necessary
    };

    // src/environments/environment.ts
    export const environment = {
      production: true,
      apiUrl: 'api/',
      hubsUrl: 'hubs/',
    };
    ```

4.  Run the Angular application:

    ```bash
    ng serve
    ```

## Usage Guide

### Registering a User

1.  Navigate to the home page in the Angular client.
2.  Click the "Register" button.
3.  Fill out the registration form with the required information.
4.  Click the "Register" button to submit the form.

### Logging In

1.  Enter your username and password in the login form in the nav bar.
2.  Click the "Login" button.

### Viewing Members

1.  After logging in, navigate to the "Matches" link in the navigation bar to view a list of members.
2.  Filter members by age, gender, and ordering options.

### Viewing Member Details

1.  Click on a member's profile to view their details, including photos, "About Me" information, interests, and photos.
2.  Navigate between the "About", "Interests", "Photos", and "Messages" tabs.

### Sending Messages

1.  Navigate to the "Messages" tab on a member's profile or click the "Message" button in the Member Card.
2.  Type a message in the input field and click the "Send" button.

### Liking Members

1.  Click the heart icon on a member's card or profile to "like" them.
2.  View the list of members you've liked or who have liked you under the "Lists" section.

### Editing Your Profile

1.  Click "Edit Profile" in the dropdown menu in the top navigation bar.
2.  Update your profile information in the "About" and "Edit photos" tabs.

### Admin Panel

1.  Login with an Admin user.
2.  Click the "Admin" link in the navigation bar to access the admin panel.
3.  Manage users and photos in the "User management" and "Photo management" tabs.

## API Documentation

The API documentation is available through Swagger UI.

1.  Run the API project.
2.  Navigate to `https://localhost:7283/swagger` in your browser (adjust port if necessary).

### Authentication Endpoints

-   `POST api/account/register`: Registers a new user. Requires a `RegisterDto` object.
-   `POST api/account/login`: Logs in an existing user. Requires a `LoginDto` object.

### User Endpoints

-   `GET api/users`: Retrieves a list of members (requires authentication). Supports pagination and filtering via query parameters.
-   `GET api/users/{username}`: Retrieves a specific member by username (requires authentication).
-   `PUT api/users`: Updates the current user's profile (requires authentication). Requires a `MemberUpdateDto` object.
-   `POST api/users/add-photo`: Adds a photo to the current user's profile (requires authentication). Requires an `IFormFile` as input.
-   `PUT api/users/set-main-photo/{photoId}`: Sets a photo as the main profile picture (requires authentication).
-   `DELETE api/users/delete-photo/{photoId}`: Deletes a photo from the current user's profile (requires authentication).

### Likes Endpoints

-   `POST api/likes/{targetUserId}`: Likes or unlikes a user (requires authentication).
-   `GET api/likes`: Retrieves a list of users who liked the current user or whom the current user liked (requires authentication). Supports pagination and filtering via query parameters.

### Messages Endpoints

-   `POST api/messages`: Sends a new message (requires authentication). Requires a `CreateMessageDto` object.
-   `GET api/messages`: Retrieves messages for the current user (requires authentication). Supports pagination and container filtering via query parameters.
-   `GET api/messages/thread/{username}`: Retrieves a message thread between the current user and another user (requires authentication).
-   `DELETE api/messages/{id}`: Deletes a message (requires authentication).

### Admin Endpoints

-   `GET api/admin/users-with-roles`: Retrieves a list of users with their roles (requires "RequiredAdminRole" policy).
-   `POST api/admin/edit-roles/{username}`: Edits the roles of a user (requires "RequiredAdminRole" policy).
-   `GET api/admin/photos-to-moderate`: Retrieves a list of photos for moderation (requires "ModeratePhotoRole" policy).

## Contributing Guidelines

Contributions are welcome! Please follow these guidelines:

1.  Fork the repository.
2.  Create a new branch for your feature or bug fix.
3.  Make your changes and commit them with clear, concise messages.
4.  Test your changes thoroughly.
5.  Submit a pull request.

## Contact/Support Information

For questions or support, please contact me via GitHub.
