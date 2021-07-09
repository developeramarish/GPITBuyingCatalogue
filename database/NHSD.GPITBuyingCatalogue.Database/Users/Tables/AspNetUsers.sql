﻿CREATE TABLE users.AspNetUsers
(
     Id nvarchar(450) NOT NULL,
     UserName nvarchar(256) NULL,
     NormalizedUserName nvarchar(256) NULL,
     Email nvarchar(256) NULL,
     NormalizedEmail nvarchar(256) NULL,
     EmailConfirmed bit NOT NULL,
     PasswordHash nvarchar(max) NULL,
     SecurityStamp nvarchar(max) NULL,
     ConcurrencyStamp nvarchar(max) NULL,
     PhoneNumber nvarchar(35) NULL,
     PhoneNumberConfirmed bit NOT NULL,
     TwoFactorEnabled bit NOT NULL,
     LockoutEnd datetimeoffset(7) NULL,
     LockoutEnabled bit NOT NULL,
     AccessFailedCount int NOT NULL,
     PrimaryOrganisationId UNIQUEIDENTIFIER NOT NULL, 
     OrganisationFunction NVARCHAR(50) NOT NULL, 
     Disabled bit NOT NULL,
     CatalogueAgreementSigned bit NOT NULL,
     FirstName NVARCHAR(100) NOT NULL, 
     LastName NVARCHAR(100) NOT NULL, 
    CONSTRAINT PK_AspNetUsers PRIMARY KEY CLUSTERED (Id),
     INDEX EmailIndex NONCLUSTERED (NormalizedEmail)
);