USE community_portal;

-- Add additional users to support more feedback and registrations
INSERT INTO Users (full_name, email, city, registration_date) VALUES
('Fiona Green','fiona@example.com','New York','2025-03-01'),
('George Brown','george@example.com','Los Angeles','2025-03-05'),
('Hannah White','hannah@example.com','Chicago','2025-03-10'),
('Ian Black','ian@example.com','New York','2025-04-01'),
('Jill Blue','jill@example.com','Los Angeles','2025-04-05'),
('Kevin Gray','kevin@example.com','Chicago','2025-04-10'),
('Lara North','lara@example.com','New York','2025-05-01'),
('Mike South','mike@example.com','Los Angeles','2025-05-05'),
('Nina East','nina@example.com','Chicago','2025-05-10'),
('Oscar West','oscar@example.com','New York','2025-05-15');

-- Add more registrations across months to build registration trends
-- Register many users for Event 1 and Event 2 and Event 3
INSERT INTO Registrations (user_id, event_id, registration_date) VALUES
(6,1,'2025-01-10'),(7,1,'2025-02-14'),(8,1,'2025-03-20'),(9,1,'2025-04-05'),(10,1,'2025-05-12'),
(6,2,'2025-01-12'),(7,2,'2025-02-16'),(8,2,'2025-03-22'),(9,2,'2025-04-07'),(10,2,'2025-05-14'),
(6,3,'2025-01-15'),(7,3,'2025-02-20'),(8,3,'2025-03-25'),(9,3,'2025-04-09'),(10,3,'2025-05-18');

-- Add duplicate registration intentionally for testing (user 6 registers twice for event 1)
INSERT INTO Registrations (user_id, event_id, registration_date) VALUES
(6,1,'2025-06-01');

-- Add many feedback rows for Event 1 to reach >=10 feedbacks
INSERT INTO Feedback (user_id, event_id, rating, comments, feedback_date) VALUES
(6,1,5,'Excellent','2025-06-11'),
(7,1,4,'Good','2025-06-11'),
(8,1,4,'Nice sessions','2025-06-11'),
(9,1,5,'Loved it','2025-06-11'),
(10,1,5,'Great speakers','2025-06-11'),
(1,1,4,'Very useful','2025-06-11'),
(2,1,3,'OK','2025-06-11'),
(3,1,5,'Fantastic','2025-06-11'),
(4,1,4,'Well organized','2025-06-11'),
(5,1,5,'Superb','2025-06-11');

-- Add some feedback for Event 3 to increase samples
INSERT INTO Feedback (user_id, event_id, rating, comments, feedback_date) VALUES
(6,3,4,'Good workshop','2025-07-04'),
(7,3,5,'Excellent','2025-07-04');

-- Add additional sessions to make one event have many sessions
INSERT INTO Sessions (event_id, title, speaker_name, start_time, end_time) VALUES
(1,'Panel: Future Tech','Panelist A','2025-06-10 13:00:00','2025-06-10 14:00:00'),
(1,'Workshop: Hands-on','Instructor B','2025-06-10 14:15:00','2025-06-10 15:30:00'),
(1,'Closing Remarks','Dr. Tech','2025-06-10 15:45:00','2025-06-10 16:00:00');

-- Add an event with no sessions and no resources to test those queries
INSERT INTO Events (title, description, city, start_date, end_date, status, organizer_id) VALUES
('Solo Meetup','A small meetup with no sessions yet.','Miami','2025-08-10 10:00:00','2025-08-10 12:00:00','upcoming',1);

-- Add an event with resources but no sessions
INSERT INTO Events (title, description, city, start_date, end_date, status, organizer_id) VALUES
('Resource Only Event','Has resources but no sessions.','Boston','2025-09-01 09:00:00','2025-09-01 17:00:00','upcoming',2);

-- Add resources for the Resource Only Event (assume its event_id is next)
-- We'll select the inserted event ids dynamically in one statement for portability

-- Resource inserts using sub-select to get event ids
INSERT INTO Resources (event_id, resource_type, resource_url, uploaded_at)
SELECT e.event_id, 'pdf', 'https://portal.com/resources/resource_only_agenda.pdf', NOW()
FROM Events e WHERE e.title = 'Resource Only Event';

-- Mark tasks done: these inserts will create feedback counts >=10 for event 1,
-- create duplicate registration, and events without sessions/resources.

SELECT 'ADDED_SAMPLE_DATA' AS status;