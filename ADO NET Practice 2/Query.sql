USE Course

CREATE TABLE Groups(
Id int Identity (1,1) PRIMARY KEY,
GroupName nvarchar(100) not null
)

CREATE TABLE Students(
Id int Identity (1,1) PRIMARY KEY,
FirstName nvarchar(150) NOT NULL,
LastName nvarchar(200) NOT NULL,
GroupId int not NULL FOREIGN KEY REFERENCES Groups(Id)
)

CREATE TABLE Subjects(
Id int Identity (1,1) PRIMARY KEY,
Name nvarchar(100) not null
)

CREATE TABLE Marks(
Id int Identity (1,1) PRIMARY KEY,
StudentID int not null FOREIGN KEY REFERENCES Students(Id),
SubjectID int not null FOREIGN KEY REFERENCES Subjects(Id),
Mark int not null
)


INSERT INTO Groups (GroupName)
VALUES (N'680.18')

INSERT INTO Students(FirstName, LastName, GroupId)
VALUES (N'Anton', N'Shastun', 1)

INSERT INTO Students(FirstName, LastName, GroupId)
VALUES (N'Valeriy', N'Mamedov', 1)

INSERT INTO Subjects(Name)
VALUES (N'OOP')

INSERT INTO Subjects(Name)
VALUES (N'Math')

INSERT INTO Marks(StudentID, SubjectID, Mark)
VALUES (1,1, 10)

INSERT INTO Marks(StudentID, SubjectID, Mark)
VALUES (1,1, 8)

INSERT INTO Marks(StudentID, SubjectID, Mark)
VALUES (2,1, 4)

INSERT INTO Marks(StudentID, SubjectID, Mark)
VALUES (2,1, 5)

INSERT INTO Marks(StudentID, SubjectID, Mark)
VALUES (1,2, 7)

INSERT INTO Marks(StudentID, SubjectID, Mark)
VALUES (1,2, 6)

INSERT INTO Marks(StudentID, SubjectID, Mark)
VALUES (2,2, 4)

INSERT INTO Marks(StudentID, SubjectID, Mark)
VALUES (2,2, 4)


