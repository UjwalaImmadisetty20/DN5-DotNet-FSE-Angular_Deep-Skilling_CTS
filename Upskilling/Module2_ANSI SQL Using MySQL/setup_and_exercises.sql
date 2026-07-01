-- Module2: ANSI SQL Using MySQL
-- Schema creation, sample dataset, and exercise queries

DROP DATABASE IF EXISTS community_portal;
CREATE DATABASE community_portal;
USE community_portal;

-- Users table
CREATE TABLE Users (
  user_id INT PRIMARY KEY AUTO_INCREMENT,
  full_name VARCHAR(100) NOT NULL,
  email VARCHAR(100) NOT NULL UNIQUE,
  city VARCHAR(100) NOT NULL,
  registration_date DATE NOT NULL
);

-- Events table
CREATE TABLE Events (
  event_id INT PRIMARY KEY AUTO_INCREMENT,
  title VARCHAR(200) NOT NULL,
  description TEXT,
  city VARCHAR(100) NOT NULL,
  start_date DATETIME NOT NULL,
  end_date DATETIME NOT NULL,
  status ENUM('upcoming','completed','cancelled') NOT NULL,
  organizer_id INT,
  FOREIGN KEY (organizer_id) REFERENCES Users(user_id)
);

-- Sessions table
CREATE TABLE Sessions (
  session_id INT PRIMARY KEY AUTO_INCREMENT,
  event_id INT,
  title VARCHAR(200) NOT NULL,
  speaker_name VARCHAR(100) NOT NULL,
  start_time DATETIME NOT NULL,
  end_time DATETIME NOT NULL,
  FOREIGN KEY (event_id) REFERENCES Events(event_id)
);

-- Registrations table
CREATE TABLE Registrations (
  registration_id INT PRIMARY KEY AUTO_INCREMENT,
  user_id INT,
  event_id INT,
  registration_date DATE NOT NULL,
  FOREIGN KEY (user_id) REFERENCES Users(user_id),
  FOREIGN KEY (event_id) REFERENCES Events(event_id)
);

-- Feedback table
CREATE TABLE Feedback (
  feedback_id INT PRIMARY KEY AUTO_INCREMENT,
  user_id INT,
  event_id INT,
  rating INT CHECK (rating BETWEEN 1 AND 5),
  comments TEXT,
  feedback_date DATE NOT NULL,
  FOREIGN KEY (user_id) REFERENCES Users(user_id),
  FOREIGN KEY (event_id) REFERENCES Events(event_id)
);

-- Resources table
CREATE TABLE Resources (
  resource_id INT PRIMARY KEY AUTO_INCREMENT,
  event_id INT,
  resource_type ENUM('pdf','image','link') NOT NULL,
  resource_url VARCHAR(255) NOT NULL,
  uploaded_at DATETIME NOT NULL,
  FOREIGN KEY (event_id) REFERENCES Events(event_id)
);

-- Sample data: Users
INSERT INTO Users (full_name, email, city, registration_date) VALUES
('Alice Johnson','alice@example.com','New York','2024-12-01'),
('Bob Smith','bob@example.com','Los Angeles','2024-12-05'),
('Charlie Lee','charlie@example.com','Chicago','2024-12-10'),
('Diana King','diana@example.com','New York','2025-01-15'),
('Ethan Hunt','ethan@example.com','Los Angeles','2025-02-01');

-- Sample data: Events
INSERT INTO Events (title, description, city, start_date, end_date, status, organizer_id) VALUES
('Tech Innovators Meetup','A meetup for tech enthusiasts.','New York','2025-06-10 10:00:00','2025-06-10 16:00:00','upcoming',1),
('AI & ML Conference','Conference on AI and ML advancements.','Chicago','2025-05-15 09:00:00','2025-05-15 17:00:00','completed',3),
('Frontend Development Bootcamp','Hands-on training on frontend tech.','Los Angeles','2025-07-01 10:00:00','2025-07-03 16:00:00','upcoming',2);

-- Sample data: Sessions
INSERT INTO Sessions (event_id, title, speaker_name, start_time, end_time) VALUES
(1,'Opening Keynote','Dr. Tech','2025-06-10 10:00:00','2025-06-10 11:00:00'),
(1,'Future of Web Dev','Alice Johnson','2025-06-10 11:15:00','2025-06-10 12:30:00'),
(2,'AI in Healthcare','Charlie Lee','2025-05-15 09:30:00','2025-05-15 11:00:00'),
(3,'Intro to HTML5','Bob Smith','2025-07-01 10:00:00','2025-07-01 12:00:00');

-- Sample data: Registrations
INSERT INTO Registrations (user_id, event_id, registration_date) VALUES
(1,1,'2025-05-01'),
(2,1,'2025-05-02'),
(3,2,'2025-04-30'),
(4,2,'2025-04-28'),
(5,3,'2025-06-15');

-- Sample data: Feedback
INSERT INTO Feedback (user_id, event_id, rating, comments, feedback_date) VALUES
(3,2,4,'Great insights!','2025-05-16'),
(4,2,5,'Very informative.','2025-05-16'),
(2,1,3,'Could be better.','2025-06-11');

-- Sample data: Resources
INSERT INTO Resources (event_id, resource_type, resource_url, uploaded_at) VALUES
(1,'pdf','https://portal.com/resources/tech_meetup_agenda.pdf','2025-05-01 10:00:00'),
(2,'image','https://portal.com/resources/ai_poster.jpg','2025-04-20 09:00:00'),
(3,'link','https://portal.com/resources/html5_docs','2025-06-25 15:00:00');

-- Exercise queries

-- 1. User Upcoming Events
SELECT u.user_id, u.full_name, e.event_id, e.title, e.city, e.start_date
FROM Users u
JOIN Registrations r ON u.user_id = r.user_id
JOIN Events e ON r.event_id = e.event_id
WHERE e.status = 'upcoming'
  AND u.city = e.city
ORDER BY e.start_date;

-- 2. Top Rated Events (at least 10 feedback entries)
SELECT e.event_id, e.title, AVG(f.rating) AS avg_rating, COUNT(f.feedback_id) AS feedback_count
FROM Events e
JOIN Feedback f ON e.event_id = f.event_id
GROUP BY e.event_id, e.title
HAVING COUNT(f.feedback_id) >= 10
ORDER BY avg_rating DESC;

-- 3. Inactive Users (no registrations in last 90 days)
SELECT u.user_id, u.full_name, u.email
FROM Users u
LEFT JOIN Registrations r ON u.user_id = r.user_id
  AND r.registration_date >= DATE_SUB(CURDATE(), INTERVAL 90 DAY)
WHERE r.registration_id IS NULL;

-- 4. Peak Session Hours (10AM to 12PM per event)
SELECT e.event_id, e.title,
  SUM(CASE WHEN TIME(s.start_time) >= '10:00:00' AND TIME(s.start_time) < '12:00:00' THEN 1 ELSE 0 END) AS sessions_10_to_12
FROM Events e
LEFT JOIN Sessions s ON e.event_id = s.event_id
GROUP BY e.event_id, e.title;

-- 5. Most Active Cities (top 5 distinct user registrations)
SELECT u.city, COUNT(DISTINCT r.user_id) AS distinct_users
FROM Registrations r
JOIN Users u ON r.user_id = u.user_id
GROUP BY u.city
ORDER BY distinct_users DESC
LIMIT 5;

-- 6. Event Resource Summary
SELECT e.event_id, e.title,
  SUM(resource_type = 'pdf') AS pdf_count,
  SUM(resource_type = 'image') AS image_count,
  SUM(resource_type = 'link') AS link_count
FROM Events e
LEFT JOIN Resources res ON e.event_id = res.event_id
GROUP BY e.event_id, e.title;

-- 7. Low Feedback Alerts
SELECT u.full_name, u.email, e.title AS event_title, f.rating, f.comments
FROM Feedback f
JOIN Users u ON f.user_id = u.user_id
JOIN Events e ON f.event_id = e.event_id
WHERE f.rating < 3;

-- 8. Sessions per Upcoming Event
SELECT e.event_id, e.title, COUNT(s.session_id) AS session_count
FROM Events e
LEFT JOIN Sessions s ON e.event_id = s.event_id
WHERE e.status = 'upcoming'
GROUP BY e.event_id, e.title;

-- 9. Organizer Event Summary
SELECT u.user_id, u.full_name,
  SUM(1) AS total_events,
  SUM(e.status = 'upcoming') AS upcoming_count,
  SUM(e.status = 'completed') AS completed_count,
  SUM(e.status = 'cancelled') AS cancelled_count
FROM Events e
JOIN Users u ON e.organizer_id = u.user_id
GROUP BY u.user_id, u.full_name;

-- 10. Feedback Gap
SELECT e.event_id, e.title
FROM Events e
LEFT JOIN Registrations r ON e.event_id = r.event_id
LEFT JOIN Feedback f ON e.event_id = f.event_id
GROUP BY e.event_id, e.title
HAVING COUNT(r.registration_id) > 0
   AND COUNT(f.feedback_id) = 0;

-- 11. Daily New User Count (last 7 days)
SELECT registration_date, COUNT(*) AS new_users
FROM Users
WHERE registration_date >= DATE_SUB(CURDATE(), INTERVAL 7 DAY)
GROUP BY registration_date
ORDER BY registration_date;

-- 12. Event with Maximum Sessions
SELECT e.event_id, e.title, COUNT(s.session_id) AS session_count
FROM Events e
LEFT JOIN Sessions s ON e.event_id = s.event_id
GROUP BY e.event_id, e.title
ORDER BY session_count DESC
LIMIT 1;

-- 13. Average Rating per City
SELECT e.city, AVG(f.rating) AS average_rating
FROM Events e
JOIN Feedback f ON e.event_id = f.event_id
GROUP BY e.city;

-- 14. Most Registered Events (top 3)
SELECT e.event_id, e.title, COUNT(r.registration_id) AS registration_count
FROM Events e
JOIN Registrations r ON e.event_id = r.event_id
GROUP BY e.event_id, e.title
ORDER BY registration_count DESC
LIMIT 3;

-- 15. Event Session Time Conflict
SELECT s1.event_id, e.title,
  s1.session_id AS session_a,
  s2.session_id AS session_b,
  s1.start_time AS start_a,
  s1.end_time AS end_a,
  s2.start_time AS start_b,
  s2.end_time AS end_b
FROM Sessions s1
JOIN Sessions s2 ON s1.event_id = s2.event_id
  AND s1.session_id < s2.session_id
  AND s1.start_time < s2.end_time
  AND s2.start_time < s1.end_time
JOIN Events e ON s1.event_id = e.event_id;

-- 16. Unregistered Active Users (created last 30 days, no registrations)
SELECT u.user_id, u.full_name, u.email, u.registration_date
FROM Users u
LEFT JOIN Registrations r ON u.user_id = r.user_id
WHERE u.registration_date >= DATE_SUB(CURDATE(), INTERVAL 30 DAY)
  AND r.registration_id IS NULL;

-- 17. Multi-Session Speakers
SELECT s.speaker_name, COUNT(*) AS session_count
FROM Sessions s
GROUP BY s.speaker_name
HAVING COUNT(*) > 1;

-- 18. Resource Availability Check
SELECT e.event_id, e.title
FROM Events e
LEFT JOIN Resources res ON e.event_id = res.event_id
GROUP BY e.event_id, e.title
HAVING COUNT(res.resource_id) = 0;

-- 19. Completed Events with Feedback Summary
SELECT e.event_id, e.title,
  COUNT(DISTINCT r.registration_id) AS total_registrations,
  AVG(f.rating) AS avg_feedback_rating
FROM Events e
LEFT JOIN Registrations r ON e.event_id = r.event_id
LEFT JOIN Feedback f ON e.event_id = f.event_id
WHERE e.status = 'completed'
GROUP BY e.event_id, e.title;

-- 20. User Engagement Index
SELECT u.user_id, u.full_name,
  COUNT(DISTINCT r.event_id) AS events_attended,
  COUNT(DISTINCT f.feedback_id) AS feedback_submitted
FROM Users u
LEFT JOIN Registrations r ON u.user_id = r.user_id
LEFT JOIN Feedback f ON u.user_id = f.user_id
GROUP BY u.user_id, u.full_name;

-- 21. Top Feedback Providers
SELECT u.user_id, u.full_name, COUNT(f.feedback_id) AS feedback_count
FROM Users u
JOIN Feedback f ON u.user_id = f.user_id
GROUP BY u.user_id, u.full_name
ORDER BY feedback_count DESC
LIMIT 5;

-- 22. Duplicate Registrations Check
SELECT r.user_id, u.full_name, r.event_id, e.title, COUNT(*) AS registration_count
FROM Registrations r
JOIN Users u ON r.user_id = u.user_id
JOIN Events e ON r.event_id = e.event_id
GROUP BY r.user_id, u.full_name, r.event_id, e.title
HAVING COUNT(*) > 1;

-- 23. Registration Trends (month-wise last 12 months)
SELECT DATE_FORMAT(registration_date, '%Y-%m') AS month, COUNT(*) AS registration_count
FROM Registrations
WHERE registration_date >= DATE_SUB(CURDATE(), INTERVAL 12 MONTH)
GROUP BY month
ORDER BY month;

-- 24. Average Session Duration per Event
SELECT e.event_id, e.title,
  AVG(TIMESTAMPDIFF(MINUTE, s.start_time, s.end_time)) AS avg_duration_minutes
FROM Events e
JOIN Sessions s ON e.event_id = s.event_id
GROUP BY e.event_id, e.title;

-- 25. Events Without Sessions
SELECT e.event_id, e.title
FROM Events e
LEFT JOIN Sessions s ON e.event_id = s.event_id
GROUP BY e.event_id, e.title
HAVING COUNT(s.session_id) = 0;
