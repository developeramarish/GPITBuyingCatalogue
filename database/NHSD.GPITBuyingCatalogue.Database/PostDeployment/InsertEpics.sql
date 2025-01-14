﻿CREATE TABLE #Epics
(
    Id nvarchar(10) NOT NULL PRIMARY KEY,
    [Name] nvarchar(500) NOT NULL,
    CapabilityId int NOT NULL,
    CompliancyLevelId int NULL,
    IsActive bit NOT NULL,
    SupplierDefined bit DEFAULT 0 NOT NULL
);

DECLARE @capabilityId AS int = 1;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive, SupplierDefined)
VALUES
('C1E1', 'Manage Appointments', @capabilityId, 1, 1, 0),
('C1E2', 'Manage Appointments by Proxy', @capabilityId, 3, 1, 0);

SET @capabilityId = 2;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C2E1', 'Manage communications – Patient', @capabilityId, 1, 1),
('C2E2', 'Manage communications – Proxy', @capabilityId, 3, 1);

SET @capabilityId = 3;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C3E1', 'Manage Repeat Medications – Patient', @capabilityId, 1, 1),
('C3E2', 'Manage my nominated EPS pharmacy', @capabilityId, 1, 1),
('C3E3', 'Manage my Preferred PharmacyAs a Patient', @capabilityId, 3, 1),
('C3E4', 'Manage Acute Medications', @capabilityId, 3, 1),
('C3E5', 'View medication information', @capabilityId, 3, 1),
('C3E6', 'Manage Repeat Medications as a Proxy', @capabilityId, 3, 1),
('C3E7', 'Manage nominated EPS pharmacy as a Proxy', @capabilityId, 3, 1),
('C3E8', 'Manage Preferred Pharmacy as a Proxy', @capabilityId, 3, 1),
('C3E9', 'Manage Acute Medications as a Proxy', @capabilityId, 3, 1),
('C3E10', 'View medication information as a Proxy', @capabilityId, 3, 1);

SET @capabilityId = 4;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C4E1', 'View Patient Record – Patient', @capabilityId, 1, 1),
('C4E2', 'View Patient Record – Proxy', @capabilityId, 3, 1);

SET @capabilityId = 5;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C5E1', 'Manage Session templates', @capabilityId, 1, 1),
('C5E2', 'Manage Sessions', @capabilityId, 1, 1),
('C5E3', 'Configure Appointments', @capabilityId, 1, 1),
('C5E4', 'Practice configuration', @capabilityId, 1, 1),
('C5E5', 'Manage Appointments', @capabilityId, 1, 1),
('C5E6', 'View Appointment reports', @capabilityId, 1, 1),
('C5E7', 'Access Patient Record', @capabilityId, 1, 1);

SET @capabilityId = 6;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C6E1', 'Access to Clinical Decision Support', @capabilityId, 1, 1),
('C6E2', 'Local configuration for Clinical Decision Support triggering', @capabilityId, 1, 1),
('C6E3', 'View Clinical Decision Support reports', @capabilityId, 1, 1),
('C6E4', 'Configuration for custom Clinical Decision Support', @capabilityId, 3, 1);

SET @capabilityId = 7;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C7E1', 'Manage communication consents for a Patient', @capabilityId, 1, 1),
('C7E2', 'Manage communication preferences for a Patient', @capabilityId, 1, 1),
('C7E3', 'Manage communication templates', @capabilityId, 1, 1),
('C7E4', 'Create communications', @capabilityId, 1, 1),
('C7E5', 'Manage automated communications', @capabilityId, 1, 1),
('C7E6', 'View communication reports', @capabilityId, 1, 1),
('C7E7', 'Access Patient Record', @capabilityId, 1, 1),
('C7E8', 'Manage communication consents for a Proxy', @capabilityId, 3, 1),
('C7E9', 'Manage communication preferences for a Proxy', @capabilityId, 3, 1),
('C7E10', 'Manage incoming communications', @capabilityId, 3, 1);

SET @capabilityId = 8;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C8E1', 'Manage Requests for Investigations', @capabilityId, 1, 1),
('C8E2', 'View Requests for Investigations reports', @capabilityId, 1, 1),
('C8E3', 'Create a Request for Investigation for multiple Patients', @capabilityId, 3, 1),
('C8E4', 'Receive external Request for Investigation information', @capabilityId, 3, 1);

SET @capabilityId = 9;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C9E1', 'Manage document classifications', @capabilityId, 1, 1),
('C9E2', 'Manage document properties', @capabilityId, 1, 1),
('C9E3', 'Manage document attributes', @capabilityId, 1, 1),
('C9E4', 'Manage document coded entries', @capabilityId, 1, 1),
('C9E5', 'Document workflows', @capabilityId, 1, 1),
('C9E6', 'Manage document annotation', @capabilityId, 1, 1),
('C9E7', 'Search for documents', @capabilityId, 1, 1),
('C9E8', 'Search document content', @capabilityId, 1, 1),
('C9E9', 'Document and Patient matching', @capabilityId, 1, 1),
('C9E10', 'Visually compare multiple documents', @capabilityId, 1, 1),
('C9E11', 'View any version of a document', @capabilityId, 1, 1),
('C9E12', 'Print documents', @capabilityId, 1, 1),
('C9E13', 'Export documents to new formats', @capabilityId, 1, 1),
('C9E14', 'Document reports', @capabilityId, 1, 1),
('C9E15', 'Receipt of electronic documents', @capabilityId, 1, 1),
('C9E16', 'Access Patient Record', @capabilityId, 1, 1),
('C9E17', 'Search for documents using document content', @capabilityId, 3, 1);

SET @capabilityId = 10;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C10E1', 'View GPES payment extract reports', @capabilityId, 1, 1),
('C10E2', 'View national GPES non-payment extract reports', @capabilityId, 3, 1);

SET @capabilityId = 11;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C11E1', 'Manage Referrals', @capabilityId, 1, 1),
('C11E2', 'View Referral reports', @capabilityId, 1, 1);

SET @capabilityId = 12;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C12E1', 'Manage General Practice and Branch site information', @capabilityId, 1, 1),
('C12E2', 'Manage General Practice Staff Members', @capabilityId, 1, 1),
('C12E3', 'Manage Staff Member inactivity periods', @capabilityId, 1, 1),
('C12E4', 'Manage Staff Member Groups', @capabilityId, 1, 1),
('C12E5', 'Manage Related Organisations information', @capabilityId, 1, 1),
('C12E6', 'Manage Related Organisation Staff Members', @capabilityId, 1, 1),
('C12E7', 'Manage Non-human Resources', @capabilityId, 3, 1);

SET @capabilityId = 13;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C13E1', 'Manage Patients ', @capabilityId, 1, 1),
('C13E2', 'Access Patient Record', @capabilityId, 1, 1),
('C13E3', 'Manage Patient Related Persons', @capabilityId, 1, 1),
('C13E4', 'Manage Patients for Citizen Services', @capabilityId, 1, 1),
('C13E5', 'Manage Patient Communications', @capabilityId, 1, 1),
('C13E6', 'Configure Patient notifications', @capabilityId, 1, 1),
('C13E7', 'Manage Practice notifications – Patient', @capabilityId, 1, 1),
('C13E8', 'Search for Patient Records', @capabilityId, 1, 1),
('C13E9', 'View Patient Reports', @capabilityId, 1, 1),
('C13E10', 'Configure Citizen service access for the Practice', @capabilityId, 1, 1),
('C13E11', 'Identify Patients outside of Catchment Area', @capabilityId, 1, 1),
('C13E12', 'Manage Patient Cohorts from Search Results', @capabilityId, 1, 1),
('C13E13', 'View Subject Access Request reports', @capabilityId, 3, 1),
('C13E14', 'Manage Acute Prescription Request Service', @capabilityId, 3, 1),
('C13E15', 'Notify the Patient of changes', @capabilityId, 3, 1),
('C13E16', 'Manage Subject Access Request (SAR) requests', @capabilityId, 3, 1),
('C13E17', 'Notify the Proxy of changes', @capabilityId, 3, 1),
('C13E18', 'Manage Practice notifications – Proxy', @capabilityId, 3, 1),
('C13E19', 'Configure Proxy notifications', @capabilityId, 3, 1),
('C13E20', 'Manage Proxy Communications', @capabilityId, 3, 1),
('C13E21', 'Manage Proxys for Citizen Services', @capabilityId, 3, 1);

SET @capabilityId = 14;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C14E1', 'Access prescribable items', @capabilityId, 1, 1),
('C14E2', 'Manage Formularies', @capabilityId, 1, 1),
('C14E3', 'Manage shared Formularies', @capabilityId, 1, 1),
('C14E4', 'Set default Formulary for Practice Users', @capabilityId, 1, 1),
('C14E5', 'Manage prescribed medication', @capabilityId, 1, 1),
('C14E6', 'Manage prescriptions', @capabilityId, 1, 1),
('C14E7', 'Manage Patient''s Preferred Pharmacy', @capabilityId, 1, 1),
('C14E8', 'Manage Patient medication reviews', @capabilityId, 1, 1),
('C14E9', 'View prescribed medication reports', @capabilityId, 1, 1),
('C14E10', 'Manage Repeat Medication requests', @capabilityId, 1, 1),
('C14E11', 'Manage Acute Medication requests', @capabilityId, 1, 1),
('C14E12', 'Manage Authorising Prescribers', @capabilityId, 1, 1),
('C14E13', 'Access Patient Record', @capabilityId, 1, 1),
('C14E14', 'View EPS Nominated Pharmacy changes', @capabilityId, 3, 1),
('C14E15', 'Configure warnings for prescribable items', @capabilityId, 3, 1),
('C14E16', 'Medications are linked to diagnoses', @capabilityId, 3, 1);

SET @capabilityId = 15;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C15E1', 'Record Consultation information', @capabilityId, 1, 1),
('C15E2', 'View report on calls and recalls', @capabilityId, 1, 1),
('C15E3', 'View report of Consultations', @capabilityId, 1, 1),
('C15E4', 'Access Patient Record', @capabilityId, 1, 1),
('C15E5', 'Manage Consultation form templates', @capabilityId, 1, 1),
('C15E6', 'Share Consultation form templates', @capabilityId, 1, 1),
('C15E7', 'Use supplier implemented national Consultation form templates', @capabilityId, 1, 1),
('C15E8', 'Extract Female Genital Mutilation data', @capabilityId, 1, 1);

SET @capabilityId = 16;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C16E1', 'Report data from other Capabilities', @capabilityId, 1, 1);

SET @capabilityId = 17;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C17E1', 'Scan documents', @capabilityId, 1, 1),
('C17E2', 'Image enhancement', @capabilityId, 3, 1);

SET @capabilityId = 18;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C18E1', 'Share monitoring data with my General Practice', @capabilityId, 1, 1),
('C18E2', 'Configure Telehealth for the Practice', @capabilityId, 1, 1),
('C18E3', 'Configure Telehealth for the Patient', @capabilityId, 1, 1),
('C18E4', 'Manage incoming Telehealth data', @capabilityId, 1, 1);

SET @capabilityId = 19;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C19E1', 'Document classification', @capabilityId, 1, 1),
('C19E2', 'Manage Document Classification rules', @capabilityId, 1, 1),
('C19E3', 'Document and Patient matching', @capabilityId, 1, 1);

SET @capabilityId = 20;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C20E1', 'Manage Task templates', @capabilityId, 1, 1),
('C20E2', 'Manage Workflow templates', @capabilityId, 1, 1),
('C20E3', 'Configure Task rules', @capabilityId, 1, 1),
('C20E4', 'Configure Workflow rules', @capabilityId, 1, 1),
('C20E5', 'Manage Tasks', @capabilityId, 1, 1),
('C20E6', 'Manage Workflows', @capabilityId, 1, 1),
('C20E7', 'Manage Task List configurations', @capabilityId, 1, 1),
('C20E8', 'Manage Workflows List configurations', @capabilityId, 1, 1),
('C20E9', 'View Task reports', @capabilityId, 1, 1),
('C20E10', 'View Workflow reports', @capabilityId, 1, 1),
('C20E11', 'Access Patient Record', @capabilityId, 1, 1),
('C20E12', 'Share Task List configuration', @capabilityId, 3, 1),
('C20E13', 'Share Workflow List configuration', @capabilityId, 3, 1);

SET @capabilityId = 21;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C21E1', 'Maintain Resident''s Care Home Record', @capabilityId, 1, 1),
('C21E2', 'Maintain Resident Proxy preferences', @capabilityId, 3, 1),
('C21E3', 'View and maintain End of Life Care Plans', @capabilityId, 3, 1),
('C21E4', 'Record incident and adverse events', @capabilityId, 3, 1),
('C21E5', 'Maintain Staff Records', @capabilityId, 3, 1),
('C21E6', 'Maintain Staff Task schedules', @capabilityId, 3, 1),
('C21E7', 'Manage Tasks', @capabilityId, 3, 1),
('C21E8', 'Reporting', @capabilityId, 3, 1);

SET @capabilityId = 22;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C22E1', 'Manage Cases', @capabilityId, 1, 1),
('C22E2', 'Maintain Caseloads', @capabilityId, 1, 1),
('C22E3', 'Generate and manage contact schedules', @capabilityId, 1, 1),
('C22E4', 'Update Case details', @capabilityId, 1, 1),
('C22E5', 'Review and comment on Caseload', @capabilityId, 3, 1),
('C22E6', 'Review and comment on contact schedule', @capabilityId, 3, 1),
('C22E7', 'View and update Patient/Service User''s Health or Care Record', @capabilityId, 3, 1),
('C22E8', 'Reporting', @capabilityId, 3, 1),
('C22E9', 'Care Pathway templates', @capabilityId, 3, 1);

SET @capabilityId = 23;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C23E1', 'Make Appointments available to external organisations', @capabilityId, 1, 1),
('C23E2', 'Search externally bookable Appointment slots', @capabilityId, 1, 1),
('C23E3', 'Book externally bookable Appointment slots', @capabilityId, 1, 1),
('C23E4', 'Maintain Appointments', @capabilityId, 1, 1),
('C23E5', 'Notifications', @capabilityId, 3, 1),
('C23E6', 'Manage Appointment Requests', @capabilityId, 3, 1),
('C23E7', 'Booking approval', @capabilityId, 3, 1),
('C23E8', 'Report on usage of Cross-Organisation Appointments', @capabilityId, 3, 1),
('C23E9', 'Manage Cross-Organisation Appointment Booking templates', @capabilityId, 3, 1);

SET @capabilityId = 24;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C24E1', 'Use Workflow to run a Cross-organisational Process', @capabilityId, 1, 1),
('C24E2', 'Maintain cross-organisational workflows', @capabilityId, 1, 1),
('C24E3', 'Maintain cross-organisational workflow templates', @capabilityId, 3, 1),
('C24E4', 'Share workflow templates', @capabilityId, 3, 1),
('C24E5', 'Manage automated notifications and reminders', @capabilityId, 3, 1),
('C24E6', 'Manage ad-hoc notifications', @capabilityId, 3, 1),
('C24E7', 'Report on Cross-organisational Workflows', @capabilityId, 3, 1);

SET @capabilityId = 25;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C25E1', 'Maintain service schedule', @capabilityId, 1, 1),
('C25E2', 'Share service schedule', @capabilityId, 1, 1),
('C25E3', 'Workforce management reporting', @capabilityId, 3, 1);

SET @capabilityId = 26;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C26E1', 'Analyse data across multiple organisations within the Integrated/Federated Care Setting (Federation)', @capabilityId, 1, 1),
('C26E2', 'Analyse data across different datasets', @capabilityId, 1, 1),
('C26E3', 'Create new or update existing reports ', @capabilityId, 1, 1),
('C26E4', 'Run existing reports', @capabilityId, 1, 1),
('C26E5', 'Present output', @capabilityId, 1, 1),
('C26E6', 'Define selection rules on reports', @capabilityId, 1, 1),
('C26E7', 'Create and run performance-based reports', @capabilityId, 3, 1),
('C26E8', 'Drill down to detailed information', @capabilityId, 3, 1),
('C26E9', 'Forecasting', @capabilityId, 3, 1),
('C26E10', 'Enable reporting at different levels', @capabilityId, 3, 1);

SET @capabilityId = 27;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C27E1', 'Maintain Domiciliary Care schedules', @capabilityId, 1, 1),
('C27E2', 'Share Domiciliary Care schedules', @capabilityId, 1, 1),
('C27E3', 'Manage Appointments', @capabilityId, 1, 1),
('C27E4', 'Service User manages their schedule for Domiciliary Care', @capabilityId, 3, 1),
('C27E5', 'Manage Care Plans for Service Users', @capabilityId, 3, 1),
('C27E6', 'Remote access to Domiciliary Care schedule', @capabilityId, 3, 1),
('C27E7', 'Receive notifications relating to Service User', @capabilityId, 3, 1),
('C27E8', 'Reports', @capabilityId, 3, 1),
('C27E9', 'Nominated individuals to view Domiciliary Care schedule', @capabilityId, 3, 1);

SET @capabilityId = 29;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C29E1', 'Health or Care Professional requests support', @capabilityId, 1, 1),
('C29E2', 'Respond to request for support from another Health or Care Professional', @capabilityId, 1, 1),
('C29E3', 'Link additional information to a request for support', @capabilityId, 3, 1),
('C29E4', 'Live Consultation: Health and Care Professionals', @capabilityId, 3, 1),
('C29E5', 'Link Consultation to Patient/Service User''s Record', @capabilityId, 3, 1),
('C29E6', 'Reports', @capabilityId, 3, 1);

SET @capabilityId = 30;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C30E1', 'Single unified medication view', @capabilityId, 1, 1),
('C30E2', 'Request medication changes', @capabilityId, 1, 1),
('C30E3', 'Identify Patients requiring medicines review', @capabilityId, 3, 1),
('C30E4', 'Maintain medicines review', @capabilityId, 3, 1),
('C30E5', 'Notify Patient and Proxies of medication changes', @capabilityId, 3, 1),
('C30E6', 'Notify other interested parties of medication changes', @capabilityId, 3, 1),
('C30E7', 'Configure medication substitutions', @capabilityId, 3, 1),
('C30E8', 'Use pre-configured medication substitutions', @capabilityId, 3, 1),
('C30E9', 'Maintain prescribed medication', @capabilityId, 3, 1),
('C30E10', 'Access national or local Medicines Optimisation guidance', @capabilityId, 3, 1),
('C30E11', 'Prescribing decision support', @capabilityId, 3, 1),
('C30E12', 'Medicines Optimisation reporting', @capabilityId, 3, 1),
('C30E13', 'Configure notifications for required Medicines Reviews', @capabilityId, 3, 1),
('C30E14', 'Receive notification for required medicines reviews', @capabilityId, 3, 1);

SET @capabilityId = 32;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C32E1', 'Manage Personal Health Budget', @capabilityId, 1, 1),
('C32E2', 'Record Personal Health Budget purchases', @capabilityId, 1, 1),
('C32E3', 'Assess Personal Health Budgets', @capabilityId, 1, 1),
('C32E4', 'Link Personal Health Budget with care plan', @capabilityId, 3, 1),
('C32E5', 'Support different models for management of Personal Health Budgets', @capabilityId, 3, 1),
('C32E6', 'Apply criteria for the use of Personal Health Budgets', @capabilityId, 3, 1),
('C32E7', 'Payments under Personal Health Budgets', @capabilityId, 3, 1),
('C32E8', 'Maintain directory of equipment, treatments and services', @capabilityId, 3, 1),
('C32E9', 'Search a directory of equipment, treatments and services', @capabilityId, 3, 1),
('C32E10', 'Manage multiple budgets', @capabilityId, 3, 1),
('C32E11', 'Link to Patient Record', @capabilityId, 3, 1),
('C32E12', 'Link to Workflow', @capabilityId, 3, 1),
('C32E13', 'Provider view', @capabilityId, 3, 1),
('C32E14', 'Management Information', @capabilityId, 3, 1),
('C32E15', 'Identify candidates for Personal Health Budgets', @capabilityId, 3, 1);

SET @capabilityId = 33;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C33E1', 'Maintain Personal Health Record content', @capabilityId, 1, 1),
('C33E2', 'Organise Personal Health Record', @capabilityId, 3, 1),
('C33E3', 'Manage access to Personal Health Record', @capabilityId, 3, 1),
('C33E4', 'Manage data coming into Personal Health Record', @capabilityId, 3, 1);

SET @capabilityId = 34;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C34E1', 'Access healthcare data', @capabilityId, 1, 1),
('C34E2', 'Maintain cohorts', @capabilityId, 1, 1),
('C34E3', 'Stratify population by risk', @capabilityId, 1, 1),
('C34E4', 'Data analysis and reporting', @capabilityId, 1, 1),
('C34E5', 'Outcomes', @capabilityId, 1, 1),
('C34E6', 'Dashboard', @capabilityId, 3, 1);

SET @capabilityId = 35;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C35E1', 'Run Risk Stratification algorithms', @capabilityId, 1, 1);

SET @capabilityId = 36;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C36E1', 'Create Shared Care Plan', @capabilityId, 1, 1),
('C36E2', 'View Shared Care Plan', @capabilityId, 1, 1),
('C36E3', 'Amend Shared Care Plan', @capabilityId, 1, 1),
('C36E4', 'Close Shared Care Plan', @capabilityId, 1, 1),
('C36E5', 'Assign Shared Care Plan actions', @capabilityId, 3, 1),
('C36E6', 'Access Shared Care Plans remotely', @capabilityId, 3, 1),
('C36E7', 'Search and view Shared Care Plans', @capabilityId, 3, 1),
('C36E8', 'Real-time access to Shared Care Plans', @capabilityId, 3, 1),
('C36E9', 'Notifications', @capabilityId, 3, 1),
('C36E10', 'Reports', @capabilityId, 3, 1),
('C36E11', 'Manage Shared Care Plan templates', @capabilityId, 3, 1),
('C36E12', 'Manage care schedules', @capabilityId, 3, 1);

SET @capabilityId = 37;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C37E1', 'Assess wellness or well-being of the Patient or Service User', @capabilityId, 1, 1),
('C37E2', 'Search the directory', @capabilityId, 1, 1),
('C37E3', 'Refer Patient/Service User to service(s)', @capabilityId, 1, 1),
('C37E4', 'Maintain referral record', @capabilityId, 1, 1),
('C37E5', 'Link to national or local directory of services', @capabilityId, 1, 1),
('C37E6', 'Maintain directory of services', @capabilityId, 1, 1),
('C37E7', 'Maintain service criteria', @capabilityId, 1, 1),
('C37E8', 'Refer Patient/Service User to Link Worker', @capabilityId, 3, 1),
('C37E9', 'Capture Patient/Service User consent', @capabilityId, 3, 1),
('C37E10', 'Patient self-referral', @capabilityId, 3, 1),
('C37E11', 'Integrate Social Prescribing Referral Record with Clinical Record', @capabilityId, 3, 1),
('C37E12', 'Receive notification of an Appointment', @capabilityId, 3, 1),
('C37E13', 'Remind Patients/Service Users of Appointments', @capabilityId, 3, 1),
('C37E14', 'Provide service feedback', @capabilityId, 3, 1),
('C37E15', 'View service feedback', @capabilityId, 3, 1),
('C37E16', 'Obtain Management Information (MI) on Social Prescribing', @capabilityId, 3, 1);

SET @capabilityId = 38;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C38E1', 'Define response to event', @capabilityId, 1, 1),
('C38E2', 'Monitor and alert', @capabilityId, 1, 1),
('C38E3', 'Receive alerts', @capabilityId, 1, 1),
('C38E4', 'Meet availability targets', @capabilityId, 1, 1),
('C38E5', 'Ease of use', @capabilityId, 3, 1),
('C38E6', 'Patient/Service Users with sensory impairment(s)', @capabilityId, 3, 1),
('C38E7', 'Obtain Management Information (MI) on Telecare', @capabilityId, 3, 1),
('C38E8', 'Enable 2-way communication with Patient/Service User', @capabilityId, 3, 1),
('C38E9', 'Remote testing of Telecare device', @capabilityId, 3, 1),
('C38E10', 'Manual testing of Telecare device', @capabilityId, 3, 1),
('C38E11', 'Sustainability of Telecare device', @capabilityId, 3, 1);

SET @capabilityId = 39;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C39E1', 'View Unified Care Record', @capabilityId, 1, 1),
('C39E2', 'Patient/Service User views the Unified Care Record', @capabilityId, 3, 1),
('C39E3', 'Default Views', @capabilityId, 3, 1);

SET @capabilityId = 40;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C40E1', 'Verify Medicinal Product Unique Identifiers', @capabilityId, 1, 1),
('C40E2', 'Decommission Medicinal Products', @capabilityId, 1, 1),
('C40E3', 'Record the integrity of Anti-tampering Devices', @capabilityId, 3, 1);

SET @capabilityId = 42;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C42E1', 'Manage Stock in a Dispensary', @capabilityId, 1, 1),
('C42E2', 'Manage Stock Orders', @capabilityId, 1, 1),
('C42E3', 'Manage Dispensing tasks for a Dispensary', @capabilityId, 1, 1),
('C42E4', 'Dispense Medication', @capabilityId, 1, 1),
('C42E5', 'Manage Dispensaries', @capabilityId, 1, 1),
('C42E6', 'Manage Endorsements', @capabilityId, 1, 1),
('C42E7', 'Manage Supplier Accounts', @capabilityId, 1, 1),
('C42E8', 'View Stock reports', @capabilityId, 1, 1);

SET @capabilityId = 45;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C45E1', 'Identify COVID-19 vaccination cohorts', @capabilityId, 1, 1),
('C45E2', 'Verify Patient information using Personal Demographics Service (PDS)', @capabilityId, 1, 1),
('C45E3', 'Import or consume COVID-19 vaccination data for Patients', @capabilityId, 1, 1),
('C45E4', 'Extract COVID-19 vaccination cohorts data in .CSV file format', @capabilityId, 3, 1),
('C45E5', 'Bulk send SMS messages for COVID-19 vaccination invite communications', @capabilityId, 3, 1),
('C45E6', 'Bulk create letters for COVID-19 vaccination invite communications', @capabilityId, 3, 1),
('C45E7', 'Bulk send email for COVID-19 vaccination invite communications', @capabilityId, 3, 1),
('C45E8', 'Automatically record which Patients have had COVID-19 vaccination invites created', @capabilityId, 3, 1),
('C45E9', 'View whether Patients have had a COVID-19 vaccination invite communication created', @capabilityId, 3, 1);

SET @capabilityId = 46;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C46E1', 'Define appointment availability for COVID-19 vaccination site', @capabilityId, 1, 1),
('C46E2', 'Book COVID-19 vaccination appointments for eligible Patients registered across different GP Practices', @capabilityId, 1, 1),
('C46E3', 'Record that a COVID-19 vaccination appointment for a Patient has been completed', @capabilityId, 1, 1),
('C46E4', 'Extract COVID-19 vaccination appointments data for NHS Digital', @capabilityId, 1, 1),
('C46E5', 'Import COVID-19 vaccination Patient cohorts data via .CSV file', @capabilityId, 3, 1),
('C46E6', 'Verify Patient information using Personal Demographics Service (PDS)', @capabilityId, 3, 1),
('C46E7', 'Bulk send SMS messages for COVID-19 vaccination invite communications', @capabilityId, 3, 1),
('C46E8', 'Bulk create letters for COVID-19 vaccination invite communications', @capabilityId, 3, 1),
('C46E9', 'Bulk send email for COVID-19 vaccination invite communications', @capabilityId, 3, 1),
('C46E10', 'Automatically record which Patients have had COVID-19 vaccination invites created', @capabilityId, 3, 1),
('C46E11', 'View whether Patients have had a COVID-19 vaccination invite communication created', @capabilityId, 3, 1),
('C46E12', 'Automatically bulk send booking reminders to Patients via SMS messages for COVID-19 vaccination invites', @capabilityId, 3, 1),
('C46E13', 'Automatically bulk create booking reminders to Patients as letters for COVID-19 vaccination invites', @capabilityId, 3, 1),
('C46E14', 'Automatically bulk send booking reminders to Patients via email for COVID-19 vaccination invites', @capabilityId, 3, 1),
('C46E15', 'Book Appointments across Solutions using GP Connect Appointments Management', @capabilityId, 3, 1),
('C46E16', 'Patients can book their own COVID-19 vaccination appointments', @capabilityId, 3, 1),
('C46E17', 'Patients can re-schedule their own future COVID-19 vaccination appointment', @capabilityId, 3, 1),
('C46E18', 'Patients can cancel their own future COVID-19 vaccination appointment', @capabilityId, 3, 1),
('C46E19', 'Automatically send booking notifications to Patients via SMS messages for COVID-19 vaccination appointments', @capabilityId, 3, 1),
('C46E20', 'Automatically create booking notifications to Patients as letters for COVID-19 vaccination appointments', @capabilityId, 3, 1),
('C46E21', 'Automatically send booking notifications to Patients via email for COVID-19 vaccination appointments', @capabilityId, 3, 1),
('C46E22', 'Create ad-hoc booking notifications to Patients for COVID-19 vaccination appointments', @capabilityId, 3, 1),
('C46E23', 'Automatically bulk send appointment reminders to Patients via SMS messages for COVID-19 vaccination appointments', @capabilityId, 3, 1),
('C46E24', 'Automatically bulk create booking reminders to Patients as letters for COVID-19 vaccination appointments', @capabilityId, 3, 1),
('C46E25', 'Automatically bulk send appointment reminders to Patients via email for COVID-19 vaccination appointments', @capabilityId, 3, 1),
('C46E26', 'Send ad-hoc appointment reminders to Patients for COVID-19 vaccination appointments', @capabilityId, 3, 1),
('C46E27', 'View all booked COVID-19 vaccination appointments for a specified time period', @capabilityId, 3, 1),
('C46E28', 'Export all booked COVID-19 vaccination appointments for a specified time period', @capabilityId, 3, 1),
('C46E29', 'Cancel booked COVID-19 vaccination appointments for Patients', @capabilityId, 3, 1),
('C46E30', 'Re-schedule booked COVID-19 vaccination appointments for Patients', @capabilityId, 3, 1),
('C46E31', 'Automatically send appointment cancellation notifications to Patients via SMS messages for COVID-19 appointments', @capabilityId, 3, 1),
('C46E32', 'Automatically create appointment cancellation notifications to Patients as letters for COVID-19 appointments', @capabilityId, 3, 1),
('C46E33', 'Automatically send appointment cancellation notifications to Patients via email for COVID-19 appointments', @capabilityId, 3, 1);

SET @capabilityId = 47;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('C47E1', 'Record structured COVID-19 vaccination data at the point of care for Patients registered at different GP Practices', @capabilityId, 1, 1),
('C47E2', 'Record structured COVID-19 adverse reaction data at the point of care for Patients registered at different GP Practices', @capabilityId, 1, 1),
('C47E3', 'Extract COVID-19 vaccination data for NHS Digital Daily Clinical Vaccination Extract', @capabilityId, 1, 1),
('C47E4', 'Extract COVID-19 adverse reaction data for NHS Digital Daily Clinical Adverse Reaction Extract', @capabilityId, 1, 1),
('C47E5', 'Automatically send vaccination data to Patient’s registered GP Practice Foundation Solution using Digital Medicines FHIR messages', @capabilityId, 1, 1),
('C47E6', 'Automatically send COVID-19 adverse reaction data to Patient’s registered GP Practice Foundation Solution using Digital Medicines FHIR messages', @capabilityId, 1, 1),
('C47E7', 'Automatically send COVID-19 vaccination data to the NHS Business Services Authority (NHSBSA)', @capabilityId, 1, 1),
('C47E8', 'View information from the GP Patient Record using GP Connect Access Record HTML', @capabilityId, 3, 1),
('C47E9', 'View information from the GP Patient Record held by the same Solution', @capabilityId, 3, 1),
('C47E10', 'View Summary Care Record (SCR) for a Patient', @capabilityId, 3, 1),
('C47E11', 'Scanning of a GS1 barcode when recording vaccination data', @capabilityId, 3, 1),
('C47E12', 'Record structured COVID-19 vaccination data at the point of care directly into GP Patient Record', @capabilityId, 3, 1),
('C47E13', 'Record structured COVID-19 adverse reaction data at the point of care directly into GP Patient Record', @capabilityId, 3, 1),
('C47E14', 'Verify Patient information using Personal Demographics Service (PDS)', @capabilityId, 1, 1),
('C47E15', 'Latest COVID-19 Clinical Screening Questions at the point of care for Patients registered at different GP Practices', @capabilityId, 1, 1),
('C47E16', 'Record structured COVID-19 vaccination data at the point of care for Patients using pre-configured vaccine batches', @capabilityId, 1, 1),
('C47E17', 'View vaccination information for a Patient held by the National Immunisation Management Service (NIMS) at point of care', @capabilityId, 1, 1),
('C47E18', 'Update previously recorded structured COVID-19 vaccination and adverse reaction data for Patients', @capabilityId, 1, 1),
('C47E19', 'Extract COVID-19 Extended Attributes data for NHS Digital Extended Attributes Extract', @capabilityId, 1, 1),
('C47E20', 'View reports on COVID-19 vaccination data', @capabilityId, 1, 1);

SET @capabilityId = 43;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('E00001', 'Online consultation', @capabilityId, 1, 1),
('E00002', 'Online consultation with a proxy', @capabilityId, 3, 1),
('E00003', 'Patient or service user requests for online consultation support and provides information', @capabilityId, 3, 1),
('E00004', 'Proxy requests for online consultation support and provides information', @capabilityId, 3, 1),
('E00005', 'Respond to Online Consultation requests for support from patients or service users', @capabilityId, 3, 1),
('E00006', 'Respond to Online Consultation requests for support from proxies', @capabilityId, 3, 1),
('E00007', 'Include attachments in online consultation requests', @capabilityId, 3, 1),
('E00008', 'Include attachments in online consultation requests from a proxy', @capabilityId, 3, 1),
('E00009', 'Automated response to online consultation requests for support from patients or service users', @capabilityId, 3, 1),
('E00010', 'Automated response to online consultation requests for support from proxies', @capabilityId, 3, 1),
('E00011', 'Patient or service user makes an administrative request', @capabilityId, 3, 1),
('E00012', 'Proxy makes an administrative request', @capabilityId, 3, 1),
('E00013', 'Respond to administrative requests for support from patients or service users', @capabilityId, 3, 1),
('E00014', 'Respond to administrative requests for support from proxies', @capabilityId, 3, 1),
('E00015', 'Automated responses to administrative requests from patients or service users', @capabilityId, 3, 1),
('E00016', 'Automated responses to administrative requests from proxies', @capabilityId, 3, 1),
('E00017', 'Link online consultation requests for support and responses', @capabilityId, 3, 1),
('E00018', 'Link online consultation requests for support from a proxy and responses', @capabilityId, 3, 1),
('E00019', 'Self-help and signposting', @capabilityId, 3, 1),
('E00020', 'Proxy supporting self-help and signposting', @capabilityId, 3, 1),
('E00021', 'Symptom checking', @capabilityId, 3, 1),
('E00022', 'Symptom checking by a proxy', @capabilityId, 3, 1),
('E00023', 'Direct messaging', @capabilityId, 3, 1),
('E00024', 'Direct messaging by a proxy', @capabilityId, 3, 1),
('E00025', 'View the Patient Record during online consultation', @capabilityId, 3, 1),
('E00026', 'Electronically share files during direct messaging', @capabilityId, 3, 1),
('E00027', 'Electronically share files during direct messaging with a proxy', @capabilityId, 3, 1),
('E00028', 'Customisation of report', @capabilityId, 3, 1),
('E00029', 'Report on utilisation of online consultation requests for support', @capabilityId, 3, 1),
('E00030', 'Report on outcomes or dispositions provided to the patient or service user', @capabilityId, 3, 1),
('E00031', 'Report on the status of online consultations', @capabilityId, 3, 1),
('E00032', 'Report on patient demographics using online consultation', @capabilityId, 3, 1),
('E00033', 'Manually prioritise online consultation requests for support', @capabilityId, 3, 1),
('E00034', 'Assign online consultation requests to a health or care professional manually', @capabilityId, 3, 1),
('E00035', 'Categorise outcome of online consultation requests', @capabilityId, 3, 1),
('E00037', 'Automatically prioritise online consultation requests', @capabilityId, 3, 1),
('E00038', 'Assign online consultation requests to health or care professional automatically', @capabilityId, 3, 1),
('E00056', 'Disable and enable direct messaging for a healthcare organisation', @capabilityId, 3, 1),
('E00057', 'Disable and enable direct messaging for a patient or service user', @capabilityId, 3, 1),
('E00058', 'Disable and enable electronic file sharing during direct messaging for a healthcare organisation', @capabilityId, 3, 1),
('E00075', 'Patient or service user feedback for online consultation', @capabilityId, 3, 1),
('E00076', 'Record online consultation outcome to the Patient Record', @capabilityId, 3, 1),
('E00077', 'Retain attachments (file and images) in the Patient Record', @capabilityId, 3, 1),
('E00078', 'Verify patient or service user details against Personal Demographics Service (PDS)', @capabilityId, 3, 1),
('E00079', 'SNOMED code online consultation', @capabilityId, 3, 1),
('E00080', 'Customisation of the question sets for patients or service users requesting online consultation support', @capabilityId, 3, 1),
('E00081', 'Accessibility options for online consultation', @capabilityId, 3, 1),
('E00082', 'Notification to patients or service users', @capabilityId, 3, 1),
('E00083', 'Customisation of instructions to patients or service users using online consultation Solution', @capabilityId, 3, 1),
('E00084', 'Categorise administration requests', @capabilityId, 3, 1),
('E00085', 'Disable and enable direct messaging for an online consultation request for support', @capabilityId, 3, 1),
('E00086', 'Configuration of the triage process', @capabilityId, 3, 1),
('E00089', 'Save the complete record of an online consultation to the Patient Record', @capabilityId, 3, 1),
('E00090', 'Health or care professional initiates an online consultations request', @capabilityId, 3, 1),
('E00091', 'Proxy verification', @capabilityId, 3, 1);

SET @capabilityId = 44;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive)
VALUES
('E00039', 'Conduct video consultation', @capabilityId, 1, 1),
('E00040', 'Conduct video consultation with a proxy', @capabilityId, 3, 1),
('E00041', 'Conduct video consultation with the patient or service user without registration', @capabilityId, 3, 1),
('E00042', 'Conduct video consultation with a proxy without registration', @capabilityId, 3, 1),
('E00043', 'End video consultation with a patient or service user', @capabilityId, 3, 1),
('E00045', 'Direct messaging during a video consultation', @capabilityId, 3, 1),
('E00047', 'View the Patient Record during video consultation', @capabilityId, 3, 1),
('E00048', 'Conduct group video consultations', @capabilityId, 3, 1),
('E00051', 'Electronically share files during a video consultation', @capabilityId, 3, 1),
('E00053', 'Health or care professional can share their screen during a video consultation', @capabilityId, 3, 1),
('E00055', 'Record video consultation outcome to the Patient Record ', @capabilityId, 3, 1),
('E00059', 'Health or care professional can record a video consultation', @capabilityId, 3, 1),
('E00060', 'Patient or service user can record a video consultation', @capabilityId, 3, 1),
('E00061', 'Accessibility options for video consultation', @capabilityId, 3, 1),
('E00062', 'Waiting room', @capabilityId, 3, 1),
('E00063', 'Disable and enable direct messaging during a video consultation for the patient or service user', @capabilityId, 3, 1),
('E00064', 'Record direct messages to the Patient Record', @capabilityId, 3, 1),
('E00065', 'Patient or service user name is not automatically visible in a group video consultation', @capabilityId, 3, 1),
('E00066', 'Invite new participants to an existing video consultation with a patient or service user', @capabilityId, 3, 1),
('E00067', 'Disable and enable electronic file sharing during a video consultation', @capabilityId, 3, 1),
('E00068', 'Disable and enable screen sharing during a video consultation', @capabilityId, 3, 1),
('E00069', 'Patient or service user feedback on video consultations', @capabilityId, 3, 1),
('E00070', 'Test the video consultation settings', @capabilityId, 3, 1),
('E00071', 'Consecutive consultations with multiple patients or service users via a single video consultation', @capabilityId, 3, 1),
('E00072', 'Reminder of upcoming or scheduled video consultation', @capabilityId, 3, 1),
('E00073', 'Disable and enable audio during a group video consultation', @capabilityId, 3, 1),
('E00074', 'Disable and enable video during a group video consultation', @capabilityId, 3, 1),
('E00087', 'Retain attachments (file and images) received during video consultation in the Patient Record', @capabilityId, 3, 1),
('E00088', 'SNOMED code video consultation', @capabilityId, 3, 1);

SET @capabilityId = 43;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive, SupplierDefined)
VALUES
('S00002', 'Alert practice if expected response times are exceeded', @capabilityId, 3, 1, 1),
('S00003', 'Allow practice to set target response times for patient requests', @capabilityId, 3, 1, 1),
('S00004', 'Allow to track whether a patient has read the practice response.', @capabilityId, 3, 1, 1),
('S00008', 'Automatic follow-up reminders', @capabilityId, 3, 1, 1),
('S00009', 'Capacity & Demand Modelling', @capabilityId, 3, 1, 1),
('S00011', 'DIRECT Automatic searching, matching, linking and opening of patient records in TPP SystmOne, Emis Web, Cegedim Vision - Lan & Aeros', @capabilityId, 3, 1, 1),
('S00012', 'Display a warning to a patient when they are uploading an image', @capabilityId, 3, 1, 1),
('S00013', 'Event Log', @capabilityId, 3, 1, 1),
('S00015', 'Filtering, sorting and searching all incoming messages', @capabilityId, 3, 1, 1),
('S00021', 'Out of  Hours (OOH) Requests', @capabilityId, 3, 1, 1),
('S00023', 'Preset Messages', @capabilityId, 3, 1, 1),
('S00024', 'Remote Monitoring', @capabilityId, 3, 1, 1),
('S00027', 'Search online consultations', @capabilityId, 3, 1, 1),
('S00029', 'To determine when a patient has read a practice response, if at all.', @capabilityId, 3, 1, 1),
('S00031', 'Viewing messages sent to patients', @capabilityId, 3, 1, 1);


SET @capabilityId = 44;

INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive, SupplierDefined)
VALUES
('S00001', '1-Way Video Triage Consultations', @capabilityId, 3, 1, 1),
('S00005', 'Appless Video', @capabilityId, 3, 1, 1),
('S00006', 'Auto Transcription', @capabilityId, 3, 1, 1),
('S00007', 'Automated  multiple reminders via multiple channels', @capabilityId, 3, 1, 1),
('S00010', 'Consultation Countdown', @capabilityId, 3, 1, 1),
('S00014', 'Facility for clinician to take notes during video consultation', @capabilityId, 3, 1, 1),
('S00016', 'In call video tools - video blurring', @capabilityId, 3, 1, 1),
('S00017', 'In call video tools - zoom', @capabilityId, 3, 1, 1),
('S00018', 'Instant Forwarding', @capabilityId, 3, 1, 1),
('S00019', 'Instant Location', @capabilityId, 3, 1, 1),
('S00020', 'Instant Vital Signs', @capabilityId, 3, 1, 1),
('S00022', 'Pop Out Floating Video', @capabilityId, 3, 1, 1),
('S00025', 'Reporting', @capabilityId, 3, 1, 1),
('S00026', 'Reporting Dashboards', @capabilityId, 3, 1, 1),
('S00028', 'Take screen captures during video consultation', @capabilityId, 3, 1, 1),
('S00030', 'Vidtu video consultation solution', @capabilityId, 3, 1, 1);

IF UPPER('$(INSERT_TEST_DATA)') = 'TRUE'
BEGIN
    SET @capabilityId = 1;

    INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive, SupplierDefined)
    VALUES
    ('S020X01E05', 'Supplier-defined epic 5', @capabilityId, 3, 1, 1),
    ('S020X01E06', 'Supplier-defined epic 6', @capabilityId, 3, 1, 1),
    ('S020X01E07', 'Supplier-defined epic 7', @capabilityId, 3, 0, 1),
    ('S020X01E08', 'Supplier-defined epic 8', @capabilityId, 3, 1, 1);

    SET @capabilityId = 43;

    INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive, SupplierDefined)
    VALUES
    ('S020X01E01', 'Supplier-defined epic 1', @capabilityId, 3, 1, 1),
    ('S020X01E02', 'Supplier-defined epic 2', @capabilityId, 3, 1, 1);

    SET @capabilityId = 44;

    INSERT INTO #Epics(Id, [Name], CapabilityId, CompliancyLevelId, IsActive, SupplierDefined)
    VALUES
    ('S020X01E03', 'Supplier-defined epic 3', @capabilityId, 3, 1, 1),
    ('S020X01E04', 'Supplier-defined epic 4', @capabilityId, 3, 1, 1);
END;

MERGE INTO catalogue.Epics AS TARGET
USING #Epics AS SOURCE
ON TARGET.Id = SOURCE.Id
WHEN MATCHED THEN
    UPDATE SET TARGET.[Name] = SOURCE.[Name],
               TARGET.CompliancyLevelId = SOURCE.CompliancyLevelId,
               TARGET.IsActive = SOURCE.IsActive
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, [Name], CapabilityId, CompliancyLevelId, IsActive, SupplierDefined)
    VALUES (SOURCE.Id, SOURCE.[Name], SOURCE.CapabilityId, SOURCE.CompliancyLevelId, IsActive, SOURCE.SupplierDefined);

DROP TABLE #Epics;
GO
