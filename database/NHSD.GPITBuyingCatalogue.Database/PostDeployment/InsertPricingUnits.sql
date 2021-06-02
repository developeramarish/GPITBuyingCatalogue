﻿CREATE TABLE #PricingUnit
(
    PricingUnitId uniqueidentifier NOT NULL,
    [Name] nvarchar(20) NOT NULL,
    TierName nvarchar(30) NOT NULL,
    [Description] nvarchar(40) NOT NULL
);

INSERT INTO #PricingUnit(PricingUnitId, [Name], TierName, [Description])
VALUES
    ('F8D06518-1A20-4FBA-B369-AB583F9FA8C0', 'patient'            , 'patients'       , 'per patient'),
    ('D43C661A-0587-45E1-B315-5E5091D6E9D0', 'bed'                , 'beds'           , 'per bed'),
    ('774E5A1D-D15C-4A37-9990-81861BEAE42B', 'consultation'       , 'consultations'  , 'per consultation'),
    ('8BF9C2F9-2FD7-4A29-8406-3C6B7B2E5D65', 'licence'            , 'licences'       , 'per licence'),
    ('90119522-D381-4296-82EE-8FE630593B56', 'sms'                , 'SMS'            , 'per SMS'),
    ('aad2820e-472d-4bac-864e-853f92e9b3bc', 'practice'           , 'practices'      , 'per practice'),
    ('cc6ee39d-41f1-4671-b31a-800485d05752', 'unit'               , 'units'          , 'per unit'),
    ('9d3bade6-f232-4b6e-9809-88a8fbb5c881', 'group'              , 'groups'         , 'per group'),
    ('599a1105-a16a-4861-b54b-d65da84366c9', 'day'                , 'days'           , 'per day'),
    ('121bd710-b73b-48f9-a429-f269a7bb0bf2', 'halfDay'            , 'half days'      , 'per half day'),
    ('823f814d-82c9-4994-94af-4c413ee418dc', 'hour'               , 'hours'          , 'per hour'),
    ('7e4dd1fd-c953-41a8-9e62-64dc306a6307', 'installation'       , 'installations'  , 'per installation'),
    ('701afb98-699e-4eda-9a66-e79a91769614', 'implementation'     , 'implementations', 'per implementation'),
    ('f2bb1b9d-b546-40b3-bfd9-d55221d96880', 'practiceMergerSplit', 'mergers/splits' , 'per practice merge/split'),
    ('6f65c40f-e7cc-4140-85c5-592dcd216132', 'extract'            , 'extracts'       , 'per extract'),
    ('59fa7cad-87b8-4e78-92b7-5689f6b123dc', 'migration'          , 'migrations'     , 'per migration'),
    ('e17fbd0b-208f-453f-938a-9880bab1ec5e', 'course'             , 'courses'        , 'per course'),
    ('1d40c0d1-6bd5-40b3-ba2f-cf433f339787', 'trainingEnvironment', 'environments'   , 'per training environment'),
    ('4b9a4640-a97a-4e30-8ed5-cccae9829616', 'user'               , 'users'          , 'per user'),
    ('66443acc-7e40-4f95-955b-47234016cff1', 'document'           , 'documents'      , 'per document'),
    ('626b43e6-c9a0-4ede-99ed-da3a1ad2d8d3', 'seminar'            , 'seminars'       , 'per seminar'),
    ('60523726-bbaf-4ec3-b29c-dee2f3d3eca8', 'item'               , 'items'          , 'per item'),
    ('8a5e119f-9b33-4017-8cc9-552e86e20898', 'activeUser'         , 'active users'   , 'per active user'),
    ('84c10564-e85f-4f64-843b-528e88b7bf59', 'virtualPC'          , 'virtual PCs'    , 'per virtual PC'),
    ('7d709183-90ad-4b35-b399-010014bb1b9b', 'transaction'        , 'transactions'   , 'per transaction'),
    ('c74e980c-bb59-4b5a-96ce-1e616bdf827c', 'nurseReview'        , 'nurse reviews'  , 'per nurse review'),
    ('34acc3bf-c036-4125-9eab-23fbe39f6352', 'review'             , 'reviews'        , 'per review'),
    --Units in the data that may be amended / consolidated
    ('4b39590d-3f35-4963-83ba-bc7d0bfe988b', 'videoConsultation', 'consultations', 'per video consultation initiated'),
    ('372787ad-041f-4176-93e9-e4a303c39014', 'digitalFirstConsult', 'consultations', 'per digital first consultation'),
    ('a4012e6c-caf3-430c-b8d3-9c45ab9fd0de', 'unitMerge', 'unit merges', 'per unit merge'),
    ('bede8599-7a4e-4753-a928-f419681b7c93', 'unitSplit', 'unit splits', 'per unit split'),
    ('8eea4a69-977d-4fb1-b4d1-2f0971beb04b', 'hourSession', 'hour sessions', 'per 1 hour session'),
    ('A92C1326-4826-48B3-B429-4A368ADB9785', 'na','',''),
    --New Units in confirmed spreadsheet 27/08/2020
    ('60d07eb0-01ef-44e4-bed3-d34ad1352e19', 'consultationCore'    , 'consultations'                , 'per consultation – core hours'),
    ('93931091-8945-43a0-b181-96f2b41ed3c3', 'consultationExtended', 'consultations'                , 'per consultation – extended hours'),
    ('fec28905-5670-4c45-99f3-1f93c8aa156c', 'consultationOut'     , 'consultations'                , 'per consultation – out of hours'),
    ('9f8888de-69fb-4395-83ce-066d4a4dc120', 'systmOneUnit'        , 'SystmOne units'               , 'per SystmOne unit'),
    ('e72500e5-4cb4-4ddf-a8b8-d898187691ca', 'smsFragment'         , 'SMS fragments'                , 'per SMS fragment'),
    ('1ba99da5-44a8-4dc9-97e7-50c450842191', 'usersPerSite_5'      , 'users per site'               , 'up to 5 users per site'),
    ('bf5b9d2c-d690-41d2-9075-7558ad3f3f1a', 'usersPerSite_10'     , 'users per site'               , 'up to 10 users per site'),
    ('8a7fe8b5-83eb-4d12-939b-53e823ecc624', 'usersPerSite_15'     , 'users per site'               , 'up to 15 users per site'),
    ('b53a9db9-697b-4184-8177-28e9d0f66142', 'usersPerSite_16'     , 'users per site'               , '16+ users per site'),
    ('72f164c0-5eeb-4df1-b3ba-68943c0ae86c', 'traineesPerCourse_5' , 'trainees per course'          , 'per 5 trainees per course'),
    ('f5975c2e-a5fd-40a9-9030-cf02227e60b1', 'consultationRequest' , 'requests for consultation'    , 'per request for consultation'),
    ('2ca3b90b-6073-48c6-9162-acc89c6cd459', 'serviceForSites_5'   , 'sites'                        , 'service for up to 5 sites'),
    ('bbd35a61-8baf-43c2-bb93-7942b99bd004', 'serviceForSites_10'  , 'sites'                        , 'service for up to 10 sites'),
    ('f20469f3-7ef5-4dac-ae17-ad1b7c69e9c2', 'serviceForSites_11'  , 'sites'                        , 'service for 11+ sites'),
    ('e6946b09-28a8-4fb5-af57-12ad9247f850', 'callOff'             , 'Call-offs'                    , 'per Call-off'),
    ('720f2d4d-448d-4899-ad40-979b30911ca6', 'carePlan'            , 'care plan/Call-offs'          , 'per custom care plan/Call-off'),
    ('05281ffc-1077-41d5-a253-3077540ef2e9', 'organisation'        , 'organisations'                , 'per organisation'),
    ('a7eb74d3-2615-4fb5-8083-cabd40ca8cba', 'carePlans_1'         , 'care plans'                   , 'per patient for 1 care plan'),
    ('69329f3d-76ac-46f3-88dc-0ea0409975b8', 'carePlans_2'         , 'care plans'                   , 'per patient for 2–5 care plans'),
    ('a973174d-b4b1-4a28-8ab6-6334fb8159bd', 'carePlans_6'         , 'care plans'                   , 'per patient for 6–10 care plans'),
    ('68e39619-d5b1-4355-845d-5b2b20d4d0d3', 'carePlans_11'        , 'care plans'                   , 'per patient for 11–20 care plans'),
    ('af3c90a8-a8c1-46d9-a6de-d97ec3d6087f', 'carePlans_21'        , 'care plans'                   , 'per patient for 21+ care plans'),
    ('9a9cc023-e799-4a46-892f-6e98f462cd0e', 'service'             , 'services'                     , 'per service'),
    ('2cb9d70f-cd40-4f86-aa63-829f030e63dc', 'patients_0–50k'      , 'patients'                     , 'per patient for 0–49,999 patients'),
    ('5fff29ee-a360-4077-b712-73abff3a7f0b', 'patients_50–500k'    , 'patients'                     , 'per patient for 50,000–499,999 patients'),
    ('79b62a1b-8e86-4be5-95e6-c19aa65af4d4', 'patients_500k+'      , 'patients'                     , 'per patient for 500,000+ patients'),
    ('d96142d4-2190-43b4-83a1-1ad8ffc66532', 'additionalPractice'  , 'additional practices'         , 'per additional practice'),
    ('0b8b296e-3d5a-4fd2-8614-fd3df220b394', 'incomingPractice'    , 'incoming practices'           , 'per incoming practice'),
    ('cba9431d-115b-4c62-b0e5-bf11aa82dbd0', 'groupMigration'      , 'group migrations'             , 'per group migration'),
    ('d29a3db3-5426-44f4-9dc6-4569f4561958', 'session'             , 'sessions'                     , 'per session'),
    ('11ecd056-e2ac-45a7-bbf8-a274e0ca8320', 'system'              , 'systems'                      , 'per system'),
    ('fb3b6d1b-78fb-4733-a6cb-6d18582e273e', 'keystoneCapability'  , 'Keystone capabilities'        , 'per Keystone capability'),
    ('8a7683b3-e39a-4f44-b387-1f2f0e33f0d7', 'clinicalUser'        , 'concurrent clinical users'    , 'per concurrent clinical user'),        
    ('aef635b7-1f26-4c4d-b99c-bcf97bae5b55', 'letter'              , 'letters'                      , 'per letter'),
    ('aa6499fd-755c-4872-b1ff-f3db8deb1e14', 'agentPhoneCall'      , 'agent phone calls'            , 'per agent phone call'),
    ('8a8ee619-9980-458c-979d-9f6d968e8806', 'email'               , 'e-mails'                      , 'per e-mail'),
    ('12b53ce9-fdc0-48b5-a07d-d95bdd7220c7', 'smsWebEmail_500'     , 'responses per site'           , '500 SMS/web/email responses per site'),
    ('02438278-ab93-4689-b123-7ac4e78f59fe', 'smsWebEmail_1000'    , 'responses per site'           , '1000 SMS/web/email responses per site'),
    ('67e1c174-8443-4883-b51f-43c297fa9c08', 'smsWebEmail_1500'    , 'responses per site'           , '1500 SMS/web/email responses per site'),
    ('1a36f980-76a4-4b3d-b36d-46afc6655124', 'dataMigration'       , 'data migrations'              , 'per data migration'),
    ('8413d5e1-2995-4885-9ffa-ec961d82aa6d', 'sharedService'       , 'shared services'              , 'per shared service'),
    ('f24afc45-971d-41fa-9beb-6f577e987c6d', 'patients_1–20k'      , 'patients'                     , 'per patient for 1–19,999 patients'),
    ('0fb82859-aa6c-4db5-b513-c776c7278aca', 'patients_20–150k'    , 'patients'                     , 'per patient 20,000–149,999 patients'),
    ('7b2c46d4-e5c1-4315-a05e-021e43d485ee', 'patients_150–250k'   , 'patients'                     , 'per patient 150,000–249,999 patients'),
    ('d37f63e0-0bb2-4703-9f86-f22057231a00', 'patients_250k'       , 'patients'                     , 'per patient 250,000 patients & above'),
    ('950365df-456d-4c72-bc6f-74a0f7535728', 'patientRegistered'   , 'patients'                     , 'per registered patient'),
    ('2c25c251-168d-4f14-a637-3d09fe7440b7', 'onlineConsultationGP', 'consultations'                , 'per online consultation non-GP practices'),
    ('edb80481-d99a-4d25-8d58-da6881b453f7', 'videoConsultationGP' , 'consultations'                , 'per video consultation non-GP practices'),
    ('a2441010-cfd0-4a0a-bbf7-e73e279b4298', 'referralEps_1–5k'    , 'referral episodes'            , 'per referral episode 1–5000 episodes'),
    ('c4bfbaf0-f311-47d2-b02c-7ee76b9f779f', 'referralEps_5–20k'   , 'referral episodes'            , 'per referral 5001–20,000 episodes'),
    ('2f59e518-e211-44bb-b9b3-0fc518f4f46c', 'referralEps_20–50k'  , 'referral episodes'            , 'per referral 20,001–50,000 episodes'),
    ('5c863dc7-b17f-43aa-997a-e5d1226525d8', 'referralEps_50k'     , 'referral episodes'            , 'per referral 50,0001 or more episodes'),
    ('3d49fdd4-a949-40ab-adea-f541a6dbe034', 'trainingSession_360' , 'training sessions'            , 'per 4 x 90 min online training sessions'),
    ('035c6c00-773b-47f7-af61-4eb60a1ce639', 'language'            , 'languages'                    , 'per language'),
    ('2231d5e7-c9a8-4c77-ae35-ca9d5cc5d79a', 'solution'            , 'solutions'                    , 'per solution'),
    ('1783eb6d-dbc0-4a00-bc4f-e9ce9331632c', 'pcn'                 , 'pcn'                          , 'per PCN'),
    ('088655c4-83cb-40b6-a996-1f3eb9056fcd', 'call_1min'           , 'calls'                        , 'per 1 minute/part of a 1 min of call'),
    ('4c791800-02a4-4e34-a6a5-575c08f37ad2', 'trainingSession_90'  , 'training sessions'            , 'per 90 min online training session'),
    ('d88a3485-e2bc-4dc5-bb75-1ffb6df8a31d', 'smsSent'             , 'sms'                          , 'per SMS sent'),
    ('4a202b19-2cd0-4650-b4f7-b89e00762168', 'smsText'             , 'sms'                          , 'per SMS text'),
    ('ccd04c97-e688-4a3b-804b-1a1dbbb4f343', 'practiceSite'        , 'practices/sites'              , 'per practice/site'),
    ('caaa4d47-58fe-4a82-8cfd-fa90a5f7dc30', 'patients_50–150k'    , 'patients'                     , 'per patient for 50,000–149,999 patients'),
    ('03259d19-4ccf-4bfe-8c90-884d32f351bb', 'patients_150–500k'   , 'patients'                     , 'per patient for 150,000–499,999 patients'),
    ('53ed1ded-6a01-4e84-a02a-d90608948082', 'patients_1–90k'      , 'patients'                     , 'per patient for 1–89,999 patients'),
    ('bbb6622c-fd88-459f-9015-34c8bbca76ba', 'patients_90–900k'    , 'patients'                     , 'per patient 90,000–899,999 patients'),
    ('0d592388-3d61-4f9d-9e11-6b0f5858acc0', 'patients_899k'       , 'patients'                     , 'per patient 899,000 patients and above'),
    ('2891ed7e-64bb-4ed6-8108-95c1fad9e2ee', 'patientUser'         , 'patient users'                , 'per patient user'),
    ('a10ae1df-b8a1-4f8d-aeb2-cd636e24d966', 'minuteUser'          , 'minutes/users'                , 'per minute per end user'),
    ('8175143c-3350-4186-954c-d98172a7dfb0', 'gigabyte'            , 'gigabytes'                    , 'per gigabyte'),
    ('d9a7efe6-11a6-474a-99a1-03cd5909408c', 'pack_10k'            , 'packs'                        , 'per pack (10,000 mins/texts)'),
    ('a55a2ce5-a5a9-4ddf-a4a0-2cfa3d89d021', 'patients_oc'         , 'patients'                     , 'per patient for OC'),
    ('87bb2f80-6b4c-412b-996e-77ac04c2abbf', 'patients_vc'         , 'patients'                     , 'per patient for VC'),
    ('62803784-227c-442b-8e90-2d47034de45e', 'singleCharge_1'      , 'single charges'               , 'single charge for band 1'),
    ('ada6a7c3-2989-4cb1-b413-6232aff92e99', 'singleCharge_2'      , 'single charges'               , 'single charge for band 2'),
    ('2945185f-9a30-42c9-a2e6-d61f8ade1cc0', 'singleCharge_3'      , 'single charges'               , 'single charge for band 3'),
    ('163f1b70-f45d-4b2f-ad89-575bc1def629', 'singleCharge_4'      , 'single charges'               , 'single charge for band 4'),
    ('8e360df0-deed-4546-a022-c101e7f31a2b', 'singleCharge_5'      , 'single charges'               , 'single charge for band 5'),
    ('7ae52815-8697-4c5c-8d0a-dd8e18b71dfc', 'activeUsers_100'     , 'active users'                 , 'Up to 100 active users'),
    ('99f37c97-0add-488f-8a24-8f5f97389c91', 'activeUsers_250'     , 'active users'                 , 'Up to 250 active users'),
    ('701d3650-1334-4c61-b4db-03d351b6a49c', 'activeUsers_500'     , 'active users'                 , 'Up to 500 active users'),
    ('644bbe2f-9ce6-4f8f-a53b-e1ea43096f88', 'activeUsers_1k'      , 'active users'                 , 'Up to 1000 active users'),
    ('5a7098f5-db63-4d95-82b1-570035251c18', 'activeUsers_2k'      , 'active users'                 , 'Up to 2000 active users');

MERGE INTO dbo.PricingUnit AS TARGET
USING #PricingUnit AS SOURCE
ON TARGET.PricingUnitId = SOURCE.PricingUnitId
WHEN MATCHED THEN
    UPDATE SET TARGET.[Name] = SOURCE.[Name],
               TARGET.TierName = SOURCE.TierName,
               TARGET.[Description] = SOURCE.[Description]
WHEN NOT MATCHED BY TARGET THEN
    INSERT (PricingUnitId, [Name], TierName, [Description])
    VALUES (SOURCE.PricingUnitId, SOURCE.[Name], SOURCE.TierName, SOURCE.[Description]);

DROP TABLE #PricingUnit;
GO