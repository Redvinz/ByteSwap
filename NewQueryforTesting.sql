CREATE DATABASE ByteSwapDB

USE ITELECFINALS
DROP DATABASE ByteSwapDB
USE ByteSwapDB

CREATE TABLE Users (
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	FirstName NVARCHAR(MAX) NOT NULL,
	LastName NVARCHAR(MAX) NOT NULL,
	Username NVARCHAR(MAX) NOT NULL,
	Password NVARCHAR(MAX) NOT NULL,
	ContactNumber NVARCHAR(MAX) NOT NULL,
	Type INT NOT NULL,
	Email NVARCHAR(MAX) NOT NULL,
	ProfilePicFN NVARCHAR(MAX),
)

INSERT INTO Users VALUES
	('Laurence', 'Arcilla', 'laurence_admin', '123', '+639123456789', 0, 'laurence@email.com', 'laurence_pic.jpg'),
    ('Joseph', 'Paduga', 'joseph_admin', '123', '+639123456789', 0, 'joseph@email.com', 'joseph_pic.jpg'),
    ('Arvin', 'Alkuino', 'arvin_admin', '123', '+639123456789', 0, 'arvin@email.com', 'arvin_pic.jpg')

INSERT INTO Users VALUES
    ('John', 'Doe', 'john.doe', 'password123', '5551234', 1, 'john.doe@example.com', 'john_profile_pic.jpg'),
    ('Jane', 'Smith', 'jane.smith', 'securepass', '5555678', 1, 'jane.smith@example.com', 'jane_profile_pic.jpg'),
    ('Mike', 'Johnson', 'mike.j', 'mikepass', '5559012', 1, 'mike.j@example.com', 'mike_profile_pic.jpg'),
    ('Emily', 'Davis', 'emily_d', 'password456', '5553456', 1, 'emily_d@example.com', 'emily_profile_pic.jpg'),
    ('Alex', 'Taylor', 'alex.taylor', 'pass123word', '5557890', 1, 'alex.taylor@example.com', 'alex_profile_pic.jpg'),
    ('Sophia', 'Clark', 'sophia_c', 'securepass456', '5552345', 1, 'sophia_c@example.com', 'sophia_profile_pic.jpg'),
    ('Daniel', 'White', 'daniel.white', 'danielpass', '5556789', 1, 'daniel.white@example.com', 'daniel_profile_pic.jpg')



CREATE TABLE Contacts (
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    UsersId INT FOREIGN KEY REFERENCES Users(Id),
    Title NVARCHAR(MAX) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
)

INSERT INTO Contacts VALUES
	(1, 'Facebook', 'https://www.facebook.com/laurence.arcilla'),
	(1, 'Instagram', 'https://www.instagram.com/laurencearcilla')



CREATE TABLE Categories(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(MAX) NOT NULL
)

INSERT INTO Categories VALUES
    ('Keyboards'),
    ('Mice'),
    ('Monitors'),
    ('Printers'),
    ('Scanners'),
    ('Webcams'),
    ('Headsets'),
    ('Speakers'),
    ('Microphones'),
    ('External Hard Drives'),
    ('Internal Hard Drives'),
    ('Solid State Drives (SSD)'),
    ('Graphics Cards'),
    ('Motherboards'),
    ('RAM (Memory)'),
    ('CPUs (Processors)'),
    ('Power Supplies'),
    ('Computer Cases'),
    ('Cooling Systems'),
    ('Networking Devices'),
    ('Routers'),
    ('Switches'),
    ('Wireless Adapters'),
    ('USB Hubs'),
    ('Cables and Connectors'),
    ('Memory Cards'),
    ('External Optical Drives'),
    ('Internal Optical Drives'),
    ('Backup Batteries'),
    ('Docking Stations')

CREATE TABLE Conditions (
 Id INT IDENTITY(1,1) PRIMARY KEY,
 Name NVARCHAR(MAX) NOT NULL,
 Detail NVARCHAR(MAX) NOT NULL
)

INSERT INTO Conditions VALUES
	('Brand New', 'The item has never been used and is in its original, unopened packaging. It should be in perfect, unused condition'),
	('Like New', 'The item may have been used briefly but shows no signs of wear or damage. It is in excellent condition and may come with all its original accessories and packaging'),
	('Lightly Used', 'The item has been used but is still in good working order. There may be minor or unnoticeable signs of wear or cosmetic imperfections'),
	('Well Used', 'The item has been used and may have more noticeable signs of wear and cosmetic imperfections, but it is still functional and works as intended'),
	('Heavily Used', 'The item has been heavily used and may have significant wear and cosmetic damage. It should still function but may have reduced aesthetic appeal'),
	('For Salvage', 'The item is non-functional or intended for parts. It is not in working condition and may be suitable for repairs or salvage')

CREATE TABLE Products (
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	CategoriesId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	UsersId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL,
	Status INT NOT NULL,
	Title NVARCHAR(MAX) NOT NULL,
	Brand NVARCHAR(MAX),
	ConditionsId INT FOREIGN KEY REFERENCES Conditions(Id) NOT NULL,
	Description NVARCHAR(MAX) NOT NULL,
	Price DECIMAL (8,2) NOT NULL,
	Method NVARCHAR(MAX) NOT NULL,
	DatePosted DATETIME DEFAULT GETDATE() NOT NULL
)

INSERT INTO Products VALUES
	(6, 1, 0, 'HD Webcam', 'Logitech', 1, 'Brand new HD webcam with built-in microphone.', 2000.00, 'Meetup', GETDATE()),
    (1, 2, 0, 'Keyboard 4 Sale', 'Royal Kludge', 1, 'Used gaming keyboard.', 1500.00, 'Lalamove', GETDATE()),
    (9, 1, 0, 'Microphone 4 Sale', 'Razer', 1, 'Used gaming microphone.', 4000.00, 'Lalamove', GETDATE()),
	(2, 1, 0, 'Used Mouse', 'Steelseries', 1, 'Old Mouse.', 2000.00, 'Meetup', GETDATE()),
    (8, 2, 0, 'Speakers na malakas', 'Logitech', 1, 'Brand New Speakers kakaabili lang last week kunin niyo na mga ya.', 6000.00, 'Gcash', GETDATE())

INSERT INTO Products (CategoriesId, UsersId, Status, Title, Brand, ConditionsId, Description, Price, Method, DatePosted)
VALUES
    (1, 1, 1, 'Mechanical Keyboard', 'Corsair', 1, 'Brand new mechanical keyboard with RGB lighting', 99.99, 'Cash on delivery', GETDATE()),
    (2, 2, 1, 'Wireless Mouse', 'Logitech', 2, 'Like new wireless mouse with ergonomic design', 29.99, 'PayPal', GETDATE()),
    (3, 3, 1, '27-inch Gaming Monitor', 'Acer', 3, 'Lightly used gaming monitor with high refresh rate', 249.99, 'Credit card', GETDATE()),
    (4, 4, 1, 'Color Laser Printer', 'HP', 4, 'Well-used color laser printer with duplex printing', 199.99, 'Cash on delivery', GETDATE()),
    (5, 5, 1, 'Flatbed Scanner', 'Epson', 5, 'Heavily used flatbed scanner with USB connectivity', 79.99, 'Bank transfer', GETDATE()),
    (6, 6, 1, 'HD Webcam', 'Logitech', 6, 'Lightly used HD webcam with built-in microphone', 49.99, 'PayPal', GETDATE()),
    (7, 7, 1, 'Over-ear Headset', 'SteelSeries', 1, 'Like new over-ear headset with surround sound', 89.99, 'Credit card', GETDATE()),
    (8, 8, 1, 'Bluetooth Speakers', 'JBL', 2, 'Brand new Bluetooth speakers with long battery life', 129.99, 'Cash on delivery', GETDATE()),
    (9, 9, 1, 'USB Microphone', 'Blue Yeti', 3, 'Well-used USB microphone with adjustable stand', 59.99, 'PayPal', GETDATE()),
    (10, 10, 1, '2TB External Hard Drive', 'Seagate', 4, 'Lightly used 2TB external hard drive with USB 3.0', 79.99, 'Credit card', GETDATE());




SELECT * FROM Users
SELECT * FROM Products
    SELECT * FROM Images

DELETE FROM Products WHERE Id=20


CREATE TABLE Saves (
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    UsersId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL,
    ProductsId INT FOREIGN KEY REFERENCES Products(Id) NOT NULL
)

INSERT INTO Saves VALUES
(1,2),
(1,5)



    CREATE TABLE Images (
        Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
        ProductsId INT FOREIGN KEY REFERENCES Products(Id) NOT NULL,
        Filename NVARCHAR(MAX) NOT NULL
    )

DELETE FROM Images WHERE ProductsId=6

DROP TABLE Saves
DROP TABLE Contacts
DROP TABLE Images
DROP TABLE Products
DROP TABLE Users
DROP TABLE Conditions
DROP TABLE Categories

SELECT * FROM Users

