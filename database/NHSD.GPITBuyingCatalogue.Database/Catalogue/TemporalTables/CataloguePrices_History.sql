﻿CREATE TABLE catalogue.CataloguePrices_History
(
    CataloguePriceId int NOT NULL,
    CatalogueItemId nvarchar(14) NOT NULL,
    ProvisioningTypeId int NOT NULL,
    CataloguePriceTypeId int NOT NULL,
    PricingUnitId smallint NOT NULL,
    TimeUnitId int NULL,
    CurrencyCode nvarchar(3) NOT NULL,
    LastUpdated datetime2(7) NOT NULL,
    LastUpdatedBy int NULL,
    Price decimal(18, 4) NULL,
    SysStartTime datetime2(0) NOT NULL,
    SysEndTime datetime2(0) NOT NULL
);