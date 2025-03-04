PRINT('/***** Start Create DB Data/Schema *****/')
GO

--CREATE TABLE [master].PartyMaster (

--    PartyId BIGINT IDENTITY(1,1) PRIMARY KEY,
--    PartyTypeId int,
--	MainTypeId int null,
--	PartyName VARCHAR(128) NOT NULL,
--    PrimaryMobileNumber varchar(20) NOT NULL,
--    PrimaryEmail VARCHAR(128) NULL,
--    PartyAccountCode varchar(50),
--	IsStorage bit,
--	IsExportImport bit,
--	IsCHA bit,
--	IsUserCreated bit,
--    CreatedDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
--    CreatedBy int NULL,
--    ModifiedBy int NULL,
--    ModifieDdate DATETIME NULL,
--    IsDeleted bit null,
--    DeletedDate DATETIME NULL,
    
--)
--go

--CREATE TABLE [master].[PartyAddress](

--	[AddressId] [bigint] primary key IDENTITY(1,1),
--	[AddressTypeId] [tinyint] NOT NULL,
--	[PartyId] [int] NOT NULL,
--	[GstNumber] [nvarchar](50) NULL,
--	[Name] [nvarchar](255) NULL,
--	[PanNumber] [nvarchar](50) NULL,
--	[TanNumber] [nvarchar](50) NULL,
--	[AddressLine1] [nvarchar](100) NULL,
--	[AddressLine2] [nvarchar](100) NULL,
--	[AddressLine3] [nvarchar](100) NULL,
--	[LatLong] [nvarchar](max) NULL,
--	[CityId] [int] NULL,
--	[DistrictId] [int] NULL,
--	[StateId] [int] NULL,
--	[CountryId] [int] NOT NULL,
--	[PinCode] [varchar](10) NULL,
--	[LangId] [smallint] NULL,
--	[StatusId] [int] NULL,
--)
--GO

--CREATE TABLE [master].[PartyContactInfo](
--	[ContactId] [int] Primary key IDENTITY(1,1),
--	[PartyId] [int] NOT NULL,
--	[ContactType] int,
--	[EmailId] [nvarchar](250) NULL,
--	[ContactNumber] [nvarchar](15) NULL,
--	[Mobile] [nvarchar](50) NULL,
--	[Landline] [nvarchar](50) NULL,
--	[FaxNumber] [nvarchar](50) NULL,
--	[LangId] [smallint] NULL,
--	[StatusId] [int] NULL,
--	[Designation] varchar(200)
--) 
--GO

--CREATE TABLE [master].[PartyExportLicenseDetail](
--	[ImportExportLicenseId] [int] Primary key IDENTITY(1,1),
--	[PartyId] [int] NOT NULL,
--	[LicenseNo] varchar(200),
--	[StartDate] [datetime],
--	[EndDate] [datetime],
--) 
--GO


--CREATE TABLE [master].[PartyAditionalDetail](
--	[AditionalId] [int] Primary key IDENTITY(1,1),
--	[PartyId] [int] NOT NULL,
--	[Name] varchar(150) NULL,
--) 
--GO

PRINT('/***** End Create DB Data/Schema  *****/')
GO

PRINT('/***** Start DATA UPDATE SQLs *****/')

GO

PRINT('/***** End DATA UPDATE SQLs *****/')

GO

PRINT('/***** Start PROCEDURES SCHEMA  *****/')
GO
IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[MDM_InsertPartyMasterWithDetails]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [dbo].[MDM_InsertPartyMasterWithDetails]
END
GO
Create OR ALTER   PROCEDURE [dbo].[MDM_InsertPartyMasterWithDetails]
    @PartyTypeId INT,
    @MainTypeId INT = NULL,
    @PartyName VARCHAR(128),
    @PrimaryMobileNumber VARCHAR(20),
    @PrimaryEmail VARCHAR(128) = NULL,
    @PartyAccountCode VARCHAR(50) = NULL,
    @IsStorage BIT,
    @IsExportImport BIT,
    @IsCHA BIT,
    @IsUserCreated BIT = 1,
    @CreatedBy BIGINT = 1,
    @ContactInfoJson NVARCHAR(MAX),
    @AddressInfoJson NVARCHAR(MAX),
    @AditionalDetailJson NVARCHAR(MAX),
    @ExportLicenseJson NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DECLARE @NewPartyId BIGINT;

        -- Ensure at least one address is provided
         IF @AddressInfoJson IS NULL OR NOT EXISTS (
            SELECT 1
            FROM OPENJSON(@AddressInfoJson)
        )
        BEGIN
            THROW 50002, 'At least one address is required.', 1;
        END

        -- Check if PartyAccountCode already exists
        IF EXISTS (
            SELECT 1 
            FROM PartyMaster 
            WHERE PartyAccountCode = @PartyAccountCode
        )
        BEGIN
            THROW 50001, 'PartyAccountCode already exists.', 1;
        END

        -- Insert PartyMaster record
        INSERT INTO PartyMaster (
            PartyTypeId, MainTypeId, PartyName, PrimaryMobileNumber, 
            PrimaryEmail, PartyAccountCode, IsStorage, IsExportImport, IsCHA, IsUserCreated, Created_userid
        )
        VALUES (
            @PartyTypeId, @MainTypeId, @PartyName, @PrimaryMobileNumber, 
            @PrimaryEmail, @PartyAccountCode, @IsStorage, @IsExportImport, @IsCHA, @IsUserCreated, @CreatedBy
        );

        SET @NewPartyId = SCOPE_IDENTITY();

        -- Handle ContactInfo
        IF @ContactInfoJson IS NOT NULL
        BEGIN
            INSERT INTO PartyContactInfo (
                PartyId, ContactType, EmailId, ContactNumber, Mobile, Landline, 
                FaxNumber, Designation
            )
            SELECT 
                @NewPartyId AS PartyId,
                ContactType, EmailId, ContactNumber, Mobile, Landline, 
                FaxNumber, Designation
            FROM OPENJSON(@ContactInfoJson)
            WITH (
                ContactType INT,
                EmailId NVARCHAR(250),
                ContactNumber NVARCHAR(15),
                Mobile NVARCHAR(50),
                Landline NVARCHAR(50),
                FaxNumber NVARCHAR(50),
                Designation VARCHAR(200)
            );
        END

        -- Handle AddressInfo
        INSERT INTO PartyAddress (
            AddressTypeId, PartyId, GstNumber, Name, PanNumber, TanNumber, 
            AddressLine1, AddressLine2, AddressLine3, LatLong, CityId, DistrictId, 
            StateId, CountryId, PinCode
        )
        SELECT 
            AddressTypeId, @NewPartyId AS PartyId, GstNumber, Name, PanNumber, TanNumber, 
            AddressLine1, AddressLine2, AddressLine3, LatLong, CityId, DistrictId, 
            StateId, CountryId, PinCode
        FROM OPENJSON(@AddressInfoJson)
        WITH (
            AddressTypeId INT,
            GstNumber NVARCHAR(50),
            Name NVARCHAR(255),
            PanNumber NVARCHAR(50),
            TanNumber NVARCHAR(50),
            AddressLine1 NVARCHAR(100),
            AddressLine2 NVARCHAR(100),
            AddressLine3 NVARCHAR(100),
            LatLong NVARCHAR(MAX),
            CityId INT,
            DistrictId INT,
            StateId INT,
            CountryId INT,
            PinCode NVARCHAR(10)
        );

        -- Handle AditionalDetail
        IF @AditionalDetailJson IS NOT NULL
        BEGIN
            INSERT INTO PartyAdditionalDetail (
                PartyId
            )
            SELECT 
                @NewPartyId AS PartyId
            FROM OPENJSON(@AditionalDetailJson)
            WITH (
                Name NVARCHAR(150)
            );
        END

        -- Handle ExportLicense
        IF @ExportLicenseJson IS NOT NULL
        BEGIN
            INSERT INTO PartyExportLicenseDetail (
                PartyId, LicenseNo, StartDate, EndDate
            )
            SELECT 
                @NewPartyId AS PartyId,
                LicenseNo, StartDate, EndDate
            FROM OPENJSON(@ExportLicenseJson)
            WITH (
                LicenseNo NVARCHAR(200),
                StartDate DATETIME,
                EndDate DATETIME
            );
        END

        SELECT 'Success' AS Result;
    END TRY
    BEGIN CATCH
        SELECT ERROR_MESSAGE() AS Result;
    END CATCH
END;

GO
IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[MDM_GetPartyList]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [dbo].[MDM_GetPartyList]
END
GO
Create PROCEDURE [master].[MDM_GetPartyList]  --[master].[MDM_GetPartyList] 1,20
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Calculate the OFFSET based on PageNumber and PageSize
        DECLARE @Offset INT;
        SET @Offset = (@PageNumber - 1) * @PageSize;

        -- 1. Get the total number of items (without pagination)
        DECLARE @TotalItems INT;
        SELECT @TotalItems = COUNT(*)
 
        FROM [master].PartyMaster;

        -- 2. Select data from PartyMaster with pagination
        SELECT 
            p.PartyId,
            p.PartyTypeId,
            p.MainTypeId,
            p.PartyName,
            p.PrimaryMobileNumber,
            p.PrimaryEmail,
            p.PartyAccountCode,
            p.IsStorage,
            p.IsExportImport,
            p.IsCHA
        FROM 
            [master].PartyMaster p
        ORDER BY 
            p.PartyId
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

        -- 3. Select data from PartyAddress for all PartyIds from the first query
        SELECT 
            a.PartyId,
            a.AddressTypeId,
            a.GstNumber,
            a.Name AS AddressName,
            a.PanNumber AS AddressPanNumber,
            a.TanNumber AS AddressTanNumber,
            a.AddressLine1,
            a.AddressLine2,
            a.AddressLine3,
            a.LatLong,
            a.CityId,
            mc.CityName,
            a.DistrictId,
            md.DistrictName,
            a.StateId,
            ms.StateName,
            a.CountryId,
            a.PinCode,
            a.LangId AS AddressLangId,
            a.StatusId AS AddressStatusId 
        FROM 
            [master].PartyAddress a
            LEFT JOIN Master.Cities mc on mc.CityId = a.CityId
            LEFT JOIN master.Districts md on md.DistrictId = a.DistrictId
            LEFT JOIN master.States ms on ms.StateId = a.StateId
        WHERE 
            a.PartyId IN (
                SELECT PartyId
                FROM [master].PartyMaster
                ORDER BY PartyId
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            )
        ORDER BY 
            a.PartyId;

        -- 4. Select data from Party Contact Info for all PartyIds from the first query
        SELECT 
            c.PartyId,
            c.ContactType,
            c.EmailId,
            c.ContactNumber,
            c.Mobile,
            c.Landline,
            c.FaxNumber,
            c.LangId AS ContactLangId,
            c.StatusId AS ContactStatusId,
            c.Designation
        FROM 
            [master].[PartyContactInfo] c
        WHERE 
            c.PartyId IN (
                SELECT PartyId
                FROM [master].PartyMaster
                ORDER BY PartyId
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            )
        ORDER BY 
            c.PartyId;

        -- 5. Select data from Party Additional Detail for all PartyIds from the first query
        SELECT 
            ad.PartyId,
            ad.Name AS AdditionalDetailName
        FROM 
            [master].[PartyAditionalDetail] ad
        WHERE 
            ad.PartyId IN (
                SELECT PartyId
                FROM [master].PartyMaster
                ORDER BY PartyId
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            )
        ORDER BY 
            ad.PartyId;

        -- 6. Select data from Party Export License Info for all PartyIds from the first query
        SELECT 
            el.PartyId,
            el.LicenseNo,
            el.StartDate,
            el.EndDate
        FROM 
            [master].PartyExportLicenseDetail el
        WHERE 
            el.PartyId IN (
                SELECT PartyId
                FROM [master].PartyMaster
                ORDER BY PartyId
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            )
        ORDER BY 
            el.PartyId;

        -- 7. Return total items count along with party data
        SELECT @TotalItems AS TotalItems;
    END TRY
    BEGIN CATCH
        -- Catch and return any error messages
        SELECT ERROR_MESSAGE() AS Result;
    END CATCH
END;

GO
IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[GetDepositorList]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [dbo].[GetDepositorList]
END
GO
CREATE PROCEDURE GetDepositorList  
    @PartyMainTypeId INT,  
    @PartyTypeId INT,  
    @Services VARCHAR(200) = ''  
AS  
BEGIN  
    SELECT   
        PartyId AS Id,   
        PartyName AS Name   
    FROM   
        dbo.PartyMaster   
    WHERE   
        (@PartyMainTypeId = 0 OR MainTypeId = @PartyMainTypeId) AND  
        (@PartyTypeId = 0 OR PartyTypeId = @PartyTypeId) AND  
        (  
            @Services = '' OR   
            (  
                (CHARINDEX('1', @Services) > 0 AND IsStorage = 1) OR  
                (CHARINDEX('2', @Services) > 0 AND IsExportImport = 1) OR  
                (CHARINDEX('3', @Services) > 0 AND IsCHA = 1)  
            )  
        );  
END  

GO

Create OR ALTER PROCEDURE usp_GetPartyMasterData
    @PartyId INT = NULL,
    @PartyTypeId INT = NULL,
    @MainTypeId INT = NULL,
    @PartyName NVARCHAR(255) = '',
    @PrimaryMobileNumber NVARCHAR(15) = '',
    @PrimaryEmail NVARCHAR(255) = '',
    @PartyAccountCode NVARCHAR(50) = '',
    @Services NVARCHAR(50) = NULL,  -- parameter for comma-separated services
    @IsUserCreated BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQLQuery NVARCHAR(MAX);
    SET @SQLQuery = '
    SELECT 
        PartyId,
        PartyTypeId,
        MainTypeId,
        PartyName,
        PrimaryMobileNumber,
        PrimaryEmail,
        PartyAccountCode,
        IsStorage,
        IsExportImport,
        IsCHA,
        IsUserCreated
        
    FROM dbo.[PartyMaster]
    WHERE 1=1';

    -- Check for PartyId
    IF @PartyId IS NOT NULL AND @PartyId <> 0
        SET @SQLQuery += ' AND PartyId = @PartyId';

    -- Check for PartyTypeId
    IF @PartyTypeId IS NOT NULL AND @PartyTypeId <> 0
        SET @SQLQuery += ' AND PartyTypeId = @PartyTypeId';

    -- Check for MainTypeId
    IF @MainTypeId IS NOT NULL AND @MainTypeId <> 0
        SET @SQLQuery += ' AND MainTypeId = @MainTypeId';

    -- Check for PartyName
    IF @PartyName <> '' AND @PartyName IS NOT NULL
        SET @SQLQuery += ' AND LTRIM(RTRIM(PartyName)) LIKE ''%'' + LTRIM(RTRIM(@PartyName)) + ''%''';

    -- Check for PrimaryMobileNumber
    IF @PrimaryMobileNumber <> '' AND @PrimaryMobileNumber IS NOT NULL
        SET @SQLQuery += ' AND PrimaryMobileNumber LIKE ''%'' + LTRIM(RTRIM(@PrimaryMobileNumber)) + ''%''';

    -- Check for PrimaryEmail
    IF @PrimaryEmail <> '' AND @PrimaryEmail IS NOT NULL
        SET @SQLQuery += ' AND PrimaryEmail LIKE ''%'' + LTRIM(RTRIM(@PrimaryEmail)) + ''%''';

    -- Check for PartyAccountCode
    IF @PartyAccountCode <> '' AND @PartyAccountCode IS NOT NULL
        SET @SQLQuery += ' AND PartyAccountCode = @PartyAccountCode';

    -- Handle the @Services parameter
    IF @Services IS NOT NULL AND LTRIM(RTRIM(@Services)) <> ''
    BEGIN
        -- Convert the comma-separated services into a table variable
        DECLARE @ServiceTable TABLE (ServiceId INT);
        INSERT INTO @ServiceTable (ServiceId)
        SELECT value FROM STRING_SPLIT(@Services, ',');

        -- Add conditions based on the services
        IF EXISTS (SELECT 1 FROM @ServiceTable WHERE ServiceId = 1)
            SET @SQLQuery += ' AND IsStorage = 1';

        IF EXISTS (SELECT 1 FROM @ServiceTable WHERE ServiceId = 2)
            SET @SQLQuery += ' AND IsExportImport = 1';

        IF EXISTS (SELECT 1 FROM @ServiceTable WHERE ServiceId = 3)
            SET @SQLQuery += ' AND IsCHA = 1';
    END;

    -- Check for IsUserCreated
    IF @IsUserCreated IS NOT NULL
        SET @SQLQuery += ' AND IsUserCreated = @IsUserCreated';

    -- Print the final SQL query (for debugging purposes)
    PRINT @SQLQuery;

    -- Execute the dynamic SQL query
    EXEC sp_executesql 
        @SQLQuery,
        N'@PartyId INT, 
          @PartyTypeId INT, 
          @MainTypeId INT, 
          @PartyName NVARCHAR(255),
          @PrimaryMobileNumber NVARCHAR(15), 
          @PrimaryEmail NVARCHAR(255),
          @PartyAccountCode NVARCHAR(50),
          @IsUserCreated BIT',
        @PartyId,
        @PartyTypeId,
        @MainTypeId,
        @PartyName,
        @PrimaryMobileNumber,
        @PrimaryEmail,
        @PartyAccountCode,
        @IsUserCreated;
END;


PRINT('/***** End PROCEDURES SCHEMA *****/')

GO