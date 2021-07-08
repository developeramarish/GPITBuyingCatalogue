﻿CREATE TABLE ordering.Orders
(
    Id int IDENTITY(10001, 1) NOT NULL,
    Revision tinyint CONSTRAINT DF_Order_Revision DEFAULT 1 NOT NULL,
    CallOffId AS CONCAT('C', FORMAT(Id, '000000'), '-', FORMAT(Revision, '00')),
    [Description] nvarchar(100) NOT NULL,
    OrderingPartyId uniqueidentifier NOT NULL,
    OrderingPartyContactId int NULL,
    SupplierId nvarchar(6) NULL,
    SupplierContactId int NULL,
    CommencementDate date NULL,
    FundingSourceOnlyGMS bit NULL,
    Created datetime2 CONSTRAINT DF_Order_Created DEFAULT GETUTCDATE() NOT NULL,
    LastUpdated datetime2 CONSTRAINT DF_Order_LastUpdated DEFAULT GETUTCDATE() NOT NULL CONSTRAINT Order_LastUpdatedNotBeforeCreated CHECK (LastUpdated >= Created),
    LastUpdatedBy uniqueidentifier NOT NULL,
    LastUpdatedByName nvarchar(256) NULL,
    Completed datetime2 NULL CONSTRAINT Order_CompletedNotBeforeCreated CHECK (Completed >= LastUpdated AND Completed >= Created),
    OrderStatusId int NOT NULL,
    IsDeleted bit CONSTRAINT DF_Order_IsDeleted DEFAULT 0 NOT NULL,
    CONSTRAINT PK_OrderId PRIMARY KEY (Id),
    CONSTRAINT FK_Order_OrderingParty FOREIGN KEY (OrderingPartyId) REFERENCES dbo.Organisations (OrganisationId),
    CONSTRAINT FK_Order_OrderingPartyContact FOREIGN KEY (OrderingPartyContactId) REFERENCES ordering.Contacts (Id),
    CONSTRAINT FK_Order_Supplier FOREIGN KEY (SupplierId) REFERENCES catalogue.Suppliers (Id),
    CONSTRAINT FK_Order_SupplierContact FOREIGN KEY (SupplierContactId) REFERENCES ordering.Contacts (Id),
    CONSTRAINT FK_Order_OrderStatus FOREIGN KEY (OrderStatusId) REFERENCES ordering.OrderStatus (Id),
    INDEX IX_Order_IsDeleted NONCLUSTERED (IsDeleted),
);
