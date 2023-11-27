IF OBJECT_ID('course', 'U') IS NULL
BEGIN
	CREATE TABLE course (
		[name] varchar(32),
		credits int NOT NULL,
		PRIMARY KEY([name])
	);
END;

IF OBJECT_ID('teacher', 'U') IS NULL
BEGIN
	CREATE TABLE teacher (
		id varchar(32),
		firstName varchar(16) NOT NULL,
		lastName varchar(16) NOT NULL,
		infix varchar(5),
		isMentor bit NOT NULL DEFAULT 0,
		PRIMARY KEY (id)
	);
END;

IF OBJECT_ID('student', 'U') IS NULL
BEGIN
    CREATE TABLE student (
        id varchar(32),
        firstName varchar(16) NOT NULL,
        lastName varchar(16) NOT NULL,
        infix varchar(5),
        mentor varchar(32),
        totalCredits int,
        totalCreditsFromP int,
        class varchar(32),
        PRIMARY KEY (id),
		FOREIGN KEY (mentor) REFERENCES teacher(id)
    );
END;

IF OBJECT_ID('upcomingExam', 'U') IS NULL
BEGIN
	CREATE TABLE upcomingExam (
		studentId varchar(32),
		courseName varchar(32),
		[date] DATETIME NOT NULL,
		PRIMARY KEY (studentId, courseName),
		FOREIGN KEY (courseName) REFERENCES course([name]),
		FOREIGN KEY (studentId) REFERENCES student([name])
	);
END;

IF OBJECT_ID('grade', 'U') IS NULL
BEGIN
    CREATE TABLE grade (
        studentId varchar(32),
        courseName varchar(32),
        attempt int,
        grade decimal(3, 1) NOT NULL,
        isDefinitive bit NOT NULL,
        [date] DATETIME NOT NULL,
        PRIMARY KEY (studentId, courseName, attempt),
        FOREIGN KEY (studentId) REFERENCES student(id)
    );
END;

IF OBJECT_ID('courseTeacher', 'U') IS NULL
BEGIN
	CREATE TABLE courseTeacher (
		teacherId varchar(32),
		courseName varchar(32),
		PRIMARY KEY(teacherId, courseName),
		FOREIGN KEY (courseName) REFERENCES course([name]),
		FOREIGN KEY (teacherId) REFERENCES teacher(id)
	);
END;

IF OBJECT_ID('semester', 'U') IS NULL
BEGIN 
	CREATE TABLE semester (
		[name] varchar(32),
		[description] TEXT NOT NULL,
		requiredCreditsFromP int DEFAULT 0,
		PRIMARY KEY([name])
	);
END;

IF OBJECT_ID('semesterAvailability', 'U') IS NULL
BEGIN
	CREATE TABLE semesterAvailability (
		semesterName varchar(32),
		availableInSemester int,
		PRIMARY KEY(semesterName, availableInSemester),
		FOREIGN KEY (semesterName) REFERENCES semester([name])
	)
END;

IF OBJECT_ID('semesterCourse', 'U') IS NULL
BEGIN
	CREATE TABLE semesterCourse (
		semesterName varchar(32),
		courseName varchar(32),
		PRIMARY KEY (semesterName, courseName),
		FOREIGN KEY (semesterName) REFERENCES semester([name]),
		FOREIGN KEY (courseName) REFERENCES course([name])
	);
END;

IF OBJECT_ID('semesterCriteria', 'U') IS NULL
BEGIN 
	CREATE TABLE semesterCriteria (
		semesterName varchar(32),
		courseName varchar(32),
		PRIMARY KEY (semesterName, courseName),
		FOREIGN KEY (semesterName) REFERENCES semester([name]),
		FOREIGN KEY (courseName) REFERENCES course([name])
	);
END;

IF OBJECT_ID('class', 'U') IS NULL 
BEGIN
	CREATE TABLE class (
		[name] varchar(32),
		PRIMARY KEY([name])
	);
END;

