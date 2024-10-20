# PublicFunction.SessionManagement
This class, `SessionManagement`, provides a structure for managing sessions, including creating, deleting, updating, retrieving, and automatically closing sessions that have expired.

### Structure Overview:

1.  **`Session` Class**:
    
    -   This class represents a single session with properties such as `Id`, `Title`, `StartTime`, `Duration`, `Location`, and `Participants`.
    -   It has a **calculated property** `EndTime`, which is automatically computed based on `StartTime` and `Duration`.
    -   The `Session` class is used to store the essential details of a session, like its start time, duration, location, and the participants involved.
2.  **`ISessionManagement` Interface**:
    
    -   This defines the operations that the `SessionManagementClass` should implement, including:
        -   `CreateSession`: Adds a new session.
        -   `DeleteSession`: Removes a session by its ID.
        -   `UpdateSession`: Updates an existing session.
        -   `GetSession`: Retrieves a session by its ID.
        -   `GetAllSessions`: Retrieves a list of all sessions.
3.  **`SessionManagementClass`**:
    
    -   Implements the `ISessionManagement` interface, managing sessions through a list (`_sessions`).
    -   **Automatic Session Cleanup**: The class has a `Timer` that runs every 5 minutes to check for expired sessions and automatically removes them from the list. The session's `EndTime` (calculated as `StartTime + Duration`) is used to determine if it has expired.

### How to Use the `SessionManagement` Class:

#### 1. **Create a New Session**:

You can create a session by instantiating the `Session` class and passing it to the `CreateSession` method.

```csharp
// Create a session with 2-hour duration
var sessionManager = new PublicFunction.SessionManagement.SessionManagementClass();
var newSession = new PublicFunction.SessionManagement.Session(
    0, "Math Lecture", DateTime.Now.AddMinutes(10), TimeSpan.FromHours(2), "Room 101", new List<string> { "Alice", "Bob" });

// Create the session and get its ID
int sessionId = sessionManager.CreateSession(newSession);
Console.WriteLine($"Session created with ID: {sessionId}");

```

#### 2. **Update a Session**:

You can update the details of an existing session by modifying the properties and passing it back to the `UpdateSession` method.

```csharp
var updatedSession = new PublicFunction.SessionManagement.Session(
    sessionId, "Updated Math Lecture", DateTime.Now.AddMinutes(20), TimeSpan.FromHours(1), "Room 102", new List<string> { "Alice", "Charlie" });

bool success = sessionManager.UpdateSession(updatedSession);
if (success)
{
    Console.WriteLine($"Session with ID {sessionId} updated successfully.");
}

```

#### 3. **Delete a Session**:

To delete a session, simply call `DeleteSession` with the session ID.
```csharp
bool deleted = sessionManager.DeleteSession(sessionId);
if (deleted)
{
    Console.WriteLine($"Session with ID {sessionId} deleted.");
}
```
#### 4. **Retrieve a Session by ID**:

Use `GetSession` to retrieve a session by its ID.
```csharp
var session = sessionManager.GetSession(sessionId);
if (session != null)
{
    Console.WriteLine($"Session ID: {session.Id}, Title: {session.Title}, Start Time: {session.StartTime}");
}

```

#### 5. **Get All Sessions**:

To get a list of all sessions, you can call `GetAllSessions`.
```csharp
var allSessions = sessionManager.GetAllSessions();
foreach (var session in allSessions)
{
    Console.WriteLine($"Session ID: {session.Id}, Title: {session.Title}, End Time: {session.EndTime}");
}

```

### How It Handles Expired Sessions:

-   **Automatic Cleanup**: The `SessionManagementClass` has a `Timer` that runs every 5 minutes. This timer calls the `CleanupExpiredSessions` method, which:
    
    -   Checks if the current time is greater than the session's `EndTime`.
    -   If the session has expired, it removes it from the `_sessions` list.
    
    Example of what happens when a session is automatically closed:
### Example Workflow:

1.  **Create a Session**: Add a session with a specific start time and duration.
2.  **Check Sessions**: Periodically, the timer will automatically close any session that has passed its end time.
3.  **Update or Delete Sessions**: Sessions can be modified or removed as needed.

### Summary of Methods:

-   `CreateSession`: Adds a session and returns the ID.
-   `DeleteSession`: Removes a session by ID.
-   `UpdateSession`: Updates an existing session's properties.
-   `GetSession`: Retrieves a session by its ID.
-   `GetAllSessions`: Retrieves a list of all sessions.

This class structure provides a complete session management system with automatic session expiration and basic CRUD (Create, Read, Update, Delete) operations.

