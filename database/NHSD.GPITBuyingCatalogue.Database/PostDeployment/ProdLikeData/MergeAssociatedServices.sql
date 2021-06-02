﻿IF (UPPER('$(INSERT_TEST_DATA)') = 'TRUE')
BEGIN

    /*********************************************************************************************************************************************/
    /* AssociatedService */
    /*********************************************************************************************************************************************/

    CREATE TABLE #AssociatedService
    (
        CatalogueItemId nvarchar(14) NOT NULL,
        [Description] nvarchar(1000) NULL,
        OrderGuidance nvarchar(1000) NULL,
        LastUpdated datetime2(7) NULL,
        LastUpdatedBy uniqueidentifier NULL,
    );

    INSERT INTO #AssociatedService (CatalogueItemId, [Description], OrderGuidance, LastUpdated, LastUpdatedBy) 
         VALUES (N'10000-S-001', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10000-S-002', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10000-S-003', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10000-S-004', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10000-S-005', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10000-S-006', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10000-S-009', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10000-S-036', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10000-S-037', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10000-S-038', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10000-S-039', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10000-S-069', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10000-S-040', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10000-S-041', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10000-S-007', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),                
                (N'10000-S-008', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10047-S-001', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10047-S-002', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10004-S-002', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10004-S-004', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10004-S-003', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10004-S-001', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),                
                (N'10073-S-021', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10073-S-023', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10029-S-007', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10029-S-009', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10007-S-001', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10007-S-002', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10007-S-003', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10007-S-004', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10007-S-005', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10029-S-006', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10029-S-008', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10029-S-010', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10052-S-012', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10052-S-001', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10052-S-002', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10052-S-009', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10052-S-010', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10052-S-011', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10052-S-003', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10052-S-013', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10052-S-008', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10052-S-004', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10046-S-001', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10046-S-002', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10046-S-003', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10046-S-004', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10046-S-005', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10046-S-006', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10046-S-007', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10046-S-009', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10046-S-010', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10063-S-004', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10063-S-005', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10063-S-006', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10063-S-007', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10063-S-008', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10063-S-009', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10063-S-010', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10052-S-005', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10072-S-005', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10072-S-010', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10072-S-011', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10072-S-012', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10072-S-013', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10046-S-008', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10030-S-001', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10073-S-022', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000'),
                (N'10000-S-141', N'DESCRIPTION', N'', GETUTCDATE(), N'00000000-0000-0000-0000-000000000000');

    MERGE INTO dbo.AssociatedService AS TARGET
    USING #AssociatedService AS SOURCE
    ON TARGET.AssociatedServiceId = SOURCE.CatalogueItemId 
    WHEN MATCHED THEN
           UPDATE SET TARGET.[Description] = SOURCE.[Description],
                      TARGET.OrderGuidance = SOURCE.OrderGuidance,
                      TARGET.LastUpdated = SOURCE.LastUpdated,
                      TARGET.LastUpdatedBy = SOURCE.LastUpdatedBy
    WHEN NOT MATCHED BY TARGET THEN
    INSERT (AssociatedServiceId, [Description], OrderGuidance, LastUpdated, LastUpdatedBy)
    VALUES (SOURCE.CatalogueItemId, SOURCE.[Description], SOURCE.OrderGuidance, SOURCE.LastUpdated, SOURCE.LastUpdatedBy);
END;
GO