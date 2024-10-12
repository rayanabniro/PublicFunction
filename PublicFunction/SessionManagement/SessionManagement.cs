using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction
{
    public class SessionManagement
    {
        public class Session
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public DateTime StartTime { get; set; }
            public TimeSpan Duration { get; set; }  // Duration in TimeSpan format
            public string Location { get; set; }
            public List<string> Participants { get; set; }

            public DateTime EndTime => StartTime.Add(Duration);  // Calculate end time automatically

            // Constructor to initialize session properties
            public Session(int id, string title, DateTime startTime, TimeSpan duration, string location, List<string> participants)
            {
                Id = id;
                Title = title;
                StartTime = startTime;
                Duration = duration;
                Location = location;
                Participants = participants ?? new List<string>();
            }
        }
        public interface ISessionManagement
        {
            // Creates a new session
            int CreateSession(Session session);

            // Deletes a session by its ID
            bool DeleteSession(int sessionId);

            // Updates an existing session's information
            bool UpdateSession(Session session);

            // Retrieves session details by its ID
            Session GetSession(int sessionId);

            // Retrieves a list of all sessions
            List<Session> GetAllSessions();
        }
        public class SessionManagementClass : ISessionManagement
        {
            private List<Session> _sessions = new List<Session>();
            private int _nextId = 1;
            private Timer _sessionCleanupTimer;

            public SessionManagementClass()
            {
                // Set up a timer to check for expired sessions every 5 minutes
                _sessionCleanupTimer = new Timer(CleanupExpiredSessions, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            }

            // Method to check and remove expired sessions
            private void CleanupExpiredSessions(object state)
            {
                DateTime currentTime = DateTime.Now;
                var expiredSessions = _sessions.Where(s => s.EndTime < currentTime).ToList();

                foreach (var session in expiredSessions)
                {
                    _sessions.Remove(session);
                    Console.WriteLine($"Session with ID {session.Id} has been automatically closed. (Ended at {session.EndTime})");
                }
            }

            // Create a new session
            public int CreateSession(Session session)
            {
                session.Id = _nextId++;
                _sessions.Add(session);
                return session.Id;
            }

            // Delete a session by its ID
            public bool DeleteSession(int sessionId)
            {
                var session = _sessions.FirstOrDefault(s => s.Id == sessionId);
                if (session != null)
                {
                    _sessions.Remove(session);
                    return true;
                }
                return false;
            }

            // Update an existing session's details
            public bool UpdateSession(Session updatedSession)
            {
                var session = _sessions.FirstOrDefault(s => s.Id == updatedSession.Id);
                if (session != null)
                {
                    session.Title = updatedSession.Title;
                    session.StartTime = updatedSession.StartTime;
                    session.Duration = updatedSession.Duration;
                    session.Location = updatedSession.Location;
                    session.Participants = updatedSession.Participants;
                    return true;
                }
                return false;
            }

            // Get details of a session by its ID
            public Session GetSession(int sessionId)
            {
                return _sessions.FirstOrDefault(s => s.Id == sessionId);
            }

            // Get a list of all sessions
            public List<Session> GetAllSessions()
            {
                return _sessions;
            }
        }
    }
}
