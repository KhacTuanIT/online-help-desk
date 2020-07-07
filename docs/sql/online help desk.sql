CREATE DATABASE [OHD]
GO

USE [OHD]
GO

CREATE TABLE [User] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [user_identity_code] int UNIQUE NOT NULL,
  [full_name] nvarchar(255) NOT NULL,
  [email] nvarchar(255) UNIQUE NOT NULL,
  [username] nvarchar(255) UNIQUE NOT NULL,
  [password] nvarchar(255) NOT NULL,
  [address] nvarchar(255),
  [avatar] nvarchar(255),
  [role] int,
  [created_at] datetime DEFAULT (getdate())
)
GO

CREATE TABLE [Role] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [rolename] nvarchar(255) UNIQUE NOT NULL
)
GO

CREATE TABLE [Notification] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [user_id] int,
  [message] nvarchar(255) NOT NULL,
  [seen] bit DEFAULT (0),
  [created_at] datetime DEFAULT (getdate())
)
GO

CREATE TABLE [Report] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [report_name] nvarchar(255) NOT NULL,
  [report_resource] nvarchar(255) NOT NULL,
  [creator] int,
  [created_at] datetime DEFAULT (getdate())
)
GO

CREATE TABLE [Facility] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [facility_name] nvarchar(255) NOT NULL,
  [facility_heads] int UNIQUE,
  [created_at] datetime DEFAULT (getdate())
)
GO

CREATE TABLE [FacilityHead] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [user_id] int UNIQUE,
  [position_name] nvarchar(255)
)
GO

CREATE TABLE [Equipment] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [faciliti_id] int,
  [artifact_id] int
)
GO

CREATE TABLE [EquipmentType] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [artifact_name] nvarchar(255) UNIQUE NOT NULL
)
GO

CREATE TABLE [Request] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [petitioner] int,
  [assigned_to] int,
  [equipment_id] int,
  [message] nvarchar(255) NOT NULL,
  [request_type_id] int
)
GO

CREATE TABLE [RequestType] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [type_name] nvarchar(255) UNIQUE NOT NULL
)
GO

CREATE TABLE [RequestStatus] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [request_id] int,
  [status_id] int,
  [message] nvarchar(255) NOT NULL,
  [time] datetime DEFAULT (getdate())
)
GO

CREATE TABLE [StatusType] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [type] nvarchar(255) NOT NULL
)
GO

ALTER TABLE [User] ADD FOREIGN KEY ([role]) REFERENCES [Role] ([id])
GO

ALTER TABLE [Notification] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Report] ADD FOREIGN KEY ([creator]) REFERENCES [User] ([id])
GO

ALTER TABLE [FacilityHead] ADD FOREIGN KEY ([id]) REFERENCES [Facility] ([facility_heads])
GO

ALTER TABLE [FacilityHead] ADD FOREIGN KEY ([user_id]) REFERENCES [User] ([id])
GO

ALTER TABLE [Equipment] ADD FOREIGN KEY ([faciliti_id]) REFERENCES [Facility] ([id])
GO

ALTER TABLE [Equipment] ADD FOREIGN KEY ([artifact_id]) REFERENCES [EquipmentType] ([id])
GO

ALTER TABLE [Request] ADD FOREIGN KEY ([petitioner]) REFERENCES [User] ([id])
GO

ALTER TABLE [Request] ADD FOREIGN KEY ([assigned_to]) REFERENCES [FacilityHead] ([user_id])
GO

ALTER TABLE [Request] ADD FOREIGN KEY ([equipment_id]) REFERENCES [Equipment] ([id])
GO

ALTER TABLE [Request] ADD FOREIGN KEY ([request_type_id]) REFERENCES [RequestType] ([id])
GO

ALTER TABLE [RequestStatus] ADD FOREIGN KEY ([request_id]) REFERENCES [Request] ([id])
GO

ALTER TABLE [RequestStatus] ADD FOREIGN KEY ([status_id]) REFERENCES [StatusType] ([id])
GO
