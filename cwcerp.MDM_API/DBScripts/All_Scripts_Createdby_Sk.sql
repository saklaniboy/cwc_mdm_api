-- =============================================
-- Author:		<Author,Santosh Kumar>
-- Create date: <Create Date,07-01-2025>
-- =============================================

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AuditLog')
BEGIN
    CREATE TABLE [dbo].[AuditLog](
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [nvarchar](128) NULL,
	[OperationType] [nvarchar](10) NULL,
	[RecordID] [int] NULL,
	[OldData] [nvarchar](max) NULL,
	[NewData] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](128) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](128) NULL,
	[ModifiedDate] [datetime] NULL,
	[IPAddress] [varchar](15) NULL,
	CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED 
	(
		[LogID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO


ALTER PROCEDURE [dbo].[USP_GetBankList]
AS
BEGIN
    SELECT BankId,            
           BankName,		   
		   BankCode,  
		   recordstatus_id,
		   created_date
    FROM Banks
    ORDER BY BankId ASC;  
END;
GO


ALTER PROCEDURE [dbo].[USP_GetBankIFSC]
    @bankId INT -- Declare parameter for BankId
AS
BEGIN
    -- Select data from the table, filtering based on the provided BankId
    SELECT IFSCCode,
           BankId,
		   BranchName, 
           recordstatus_id,          
           created_date
    FROM Bankbranchs 
    WHERE BankId = @bankId -- Apply filter based on BankId
    ORDER BY BankId ASC;  -- Sort the result by BankId
END;
GO



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ErrorHandling')
BEGIN
    CREATE TABLE [dbo].[ErrorHandling](
	[ErrorId] [int] IDENTITY(1,1) NOT NULL,
	[ErrorNumber] [int] NULL,
	[ErrorMessage] [nvarchar](max) NULL,
	[ErrorProcedure] [nvarchar](255) NULL,
	[ErrorDateTime] [datetime] NULL,
	[AdditionalDetails] [nvarchar](max) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ErrorId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO



ALTER PROCEDURE [dbo].[USP_GetROList]
    @PageNumber INT = NULL,           -- Page number for pagination
    @PageSize INT = NULL,            -- Number of records per page    
	@StatusId INT = NULL,				 -- Status to filter by, optional
	@TotalRecords INT OUTPUT		 -- Total records for pagination
AS
BEGIN
    -- Declare a variable to store the offset for pagination
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    -- Get the total count of records and assign it to @TotalRecords
    SELECT @TotalRecords = COUNT(*) 
    FROM dbo.Offices loc
    LEFT JOIN dbo.OfficeTypes lt ON loc.OfficeTypeId = lt.OfficeTypeId
    LEFT JOIN dbo.OfficeContacts cd ON loc.OfficeId = cd.OfficeId
	WHERE loc.OfficeTypeId=2
	AND (@StatusId IS NULL OR loc.recordstatus_id = @StatusId);

    -- Fetch paginated data for locations, contact details, and office types
    SELECT 
		loc.OfficeId, 
		loc.OfficeTypeId,
        loc.OfficeName,
        loc.OfficeCode,   
		-- loc.ApprovalStatus,	
		loc.recordstatus_id,
		loc.created_date,
		cd.Mobileno,
		os.ProcessId,
		os.StatusId as StepStatus,
		os.StepId
    FROM 
        dbo.Offices loc WITH (NOLOCK)
    LEFT JOIN 
        dbo.OfficeTypes lt WITH (NOLOCK) ON loc.OfficeTypeId = lt.OfficeTypeId
    LEFT JOIN 
        dbo.OfficeContacts cd WITH (NOLOCK) ON loc.OfficeId = cd.OfficeId
    LEFT JOIN 
        dbo.OfficeStatus os WITH (NOLOCK) ON loc.OfficeId = os.OfficeId
	WHERE loc.OfficeTypeId = 2 -- RO List
	AND (@StatusId IS NULL OR loc.recordstatus_id = @StatusId)
    ORDER BY loc.created_date DESC
	-- Apply OFFSET and FETCH only when pagination parameters are provided
    OFFSET 
        CASE 
            WHEN @PageNumber IS NULL OR @PageSize IS NULL THEN 0 -- No pagination
            ELSE @Offset  
        END ROWS
    FETCH NEXT 
        CASE 
            WHEN @PageNumber IS NULL OR @PageSize IS NULL THEN 2147483647  -- Fetch all rows if no pagination
            ELSE @PageSize 
        END ROWS ONLY;
END;
GO



ALTER PROCEDURE [dbo].[USP_GetROInfo]
    @OfficeId INT,  -- Input: OfficeId to fetch data for
    @OutputJson NVARCHAR(MAX) OUTPUT  -- Output: JSON response with retrieved data
AS
BEGIN
    BEGIN TRY
        -- Initialize @OutputJson as an empty string to build the JSON response
        SET @OutputJson = '{';

        -- Fetch office status data and concatenate to @OutputJson
        SET @OutputJson = @OutputJson + '"OfficeStatus":' + ISNULL((
            SELECT                                               
                OfficeId,
                OfficeTypeId,
                ProcessId,
                StatusId,
                StepId,
                created_date
            FROM OfficeStatus
            WHERE OfficeId = @OfficeId
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ), 'null');

        -- Fetch office details and concatenate to @OutputJson
        SET @OutputJson = @OutputJson + ', "BasicInfo":' + ISNULL((
            SELECT 
                t1.OfficeId,                                               
                OfficeCode, 
				OfficeName, 
				OfficeTypeId, 				
                OfficeParentId,                
				Latitude,
				Longitude,
				FromDate,
				EndDate,
				t1.created_userid,
                t1.created_date,
			    t2.AddressTypeId,
				t2.AddressLine1,
				t2.StateId,
				t2.DistrictId,
				t2.CityId,
				t2.ZoneId,
				s.StateName,
				d.DistrictName,
				c.CityName,
				t2.PinCode               
            FROM Offices t1
			LEFT JOIN OfficeAddress t2 WITH (NOLOCK) ON t1.OfficeId = t2.OfficeId and t2.AddressTypeId=1
			LEFT JOIN States s WITH (NOLOCK) ON s.StateId = t2.StateId
	        LEFT JOIN Districts d WITH (NOLOCK) ON d.DistrictId = t2.DistrictId
			LEFT JOIN Cities c WITH (NOLOCK) ON c.CityId = t2.CityId
            WHERE t1.OfficeId = @OfficeId 
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ), 'null');

        -- Fetch office address data and concatenate to @OutputJson (returning array)
        SET @OutputJson = @OutputJson + ', "BillingInfo":' + ISNULL((
            SELECT 
                t2.AddressTypeId,							
                t2.AddressLine1, 
                t2.AddressLine2, 
                t2.AddressLine3,    
				t2.StateId,
				t2.DistrictId,
				t2.CityId,
                s1.StateName,
				d1.DistrictName,
				c1.CityName,
				t2.ZoneId,
                t2.PinCode,
				t3.GstNumber, 
				t3.BusinessName, 
				t3.PanNumber, 
				t3.TanNumber
            FROM OfficeAddress t2
			LEFT JOIN OfficeTaxRelated t3 WITH (NOLOCK) ON t3.OfficeId = t2.OfficeId
			LEFT JOIN States s1 WITH (NOLOCK) ON s1.StateId = t2.StateId
	        LEFT JOIN Districts d1 WITH (NOLOCK) ON d1.DistrictId = t2.DistrictId
			LEFT JOIN Cities c1 WITH (NOLOCK) ON c1.CityId = t2.CityId
            WHERE t2.OfficeId = @OfficeId AND AddressTypeId=2
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ), 'null');
        
        -- Fetch office bank details and concatenate to @OutputJson (returning array)
		SET @OutputJson = @OutputJson + ', "BankInfo":' + ISNULL((
            SELECT 
			    b1.BankId as PrimaryBankId,
			    ob.PrimaryAccountNo, 
				ob.PrimaryIFSCCode, 
				bb.BranchName as PrimaryBranchName,
				b2.BankId as SecondaryBankId,
				bbs.BranchName as SecondaryBranchName,
				ob.Priority
            FROM OfficeBank ob			
			LEFT JOIN Bankbranchs bb WITH (NOLOCK) ON bb.IFSCCode = ob.PrimaryIFSCCode AND ob.Priority=1
			LEFT JOIN Bankbranchs bbs WITH (NOLOCK) ON bbs.IFSCCode = ob.PrimaryIFSCCode AND ob.Priority=2
			LEFT JOIN Banks b1 WITH (NOLOCK) ON b1.BankId = bb.BankId
			LEFT JOIN Banks b2 WITH (NOLOCK) ON b2.BankId = bbs.BankId
            WHERE OfficeId = @OfficeId
            FOR JSON PATH
        ), 'null');


		-- Fetch office contact details and concatenate to @OutputJson
        SET @OutputJson = @OutputJson + ', "ContactInfo":' + ISNULL((
            SELECT                
                Mobileno as ContactNumber,
                LandlineNo as FaxNumber,
				Email as EmailId
            FROM OfficeContacts
            WHERE OfficeId = @OfficeId
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ), 'null');

        -- Close the JSON object
        SET @OutputJson = @OutputJson + '}';

    END TRY
    BEGIN CATCH
        -- Error handling: In case of an error, return a failure message in JSON format
        SET @OutputJson = '{"status": "error", "message": "' + COALESCE(ERROR_MESSAGE(), 'Unknown error') + '"}';
    END CATCH
END;
GO



ALTER PROCEDURE [dbo].[USP_UpsertROBasicInfo]
    @input_json NVARCHAR(MAX), 
    @created_by INT,
    @output_json NVARCHAR(MAX) OUTPUT 
AS
BEGIN
    BEGIN TRY
        -- Start the transaction
        BEGIN TRANSACTION;

        -- Declare variables 
        DECLARE @v_OfficeCode NVARCHAR(255), @v_OfficeCodeExist NVARCHAR(255), @v_OfficeName NVARCHAR(255),
                @v_OfficeTypeId INT, @v_OfficeParentId INT,   
                @v_Latitude FLOAT, @v_Longitude FLOAT,
                @v_FromDate DATETIME, @v_EndDate DATETIME,                                              
                @v_AddressTypeId INT, @v_AddressLine1 NVARCHAR(255),            
                @v_StateId INT, @v_DistrictId INT, @v_CityId INT, @v_PinCode NVARCHAR(10),               
                @v_OfficeId INT, @v_MaxOfficeId INT, @v_AddressId INT;

        -- Parse JSON for Office details
        SET @v_OfficeId = JSON_VALUE(@input_json, '$.OfficeId');
        SET @v_OfficeCode = JSON_VALUE(@input_json, '$.OfficeCode');
        SET @v_OfficeName = JSON_VALUE(@input_json, '$.OfficeName');
        SET @v_OfficeTypeId = JSON_VALUE(@input_json, '$.OfficeTypeId');
        SET @v_OfficeParentId = JSON_VALUE(@input_json, '$.OfficeParentId');
        SET @v_Latitude = JSON_VALUE(@input_json, '$.Latitude');
        SET @v_Longitude = JSON_VALUE(@input_json, '$.Longitude');
        SET @v_FromDate = JSON_VALUE(@input_json, '$.FromDate');
        SET @v_EndDate = NULLIF(JSON_VALUE(@input_json, '$.EndDate'), '');        

        -- Parse Address details
        SET @v_AddressTypeId = JSON_VALUE(@input_json, '$.AddressTypeId');
        SET @v_AddressLine1 = JSON_VALUE(@input_json, '$.AddressLine1');        
        SET @v_StateId = JSON_VALUE(@input_json, '$.StateId');
        SET @v_DistrictId = JSON_VALUE(@input_json, '$.DistrictId');
        SET @v_CityId = JSON_VALUE(@input_json, '$.CityId');
        SET @v_PinCode = JSON_VALUE(@input_json, '$.PinCode');

        -- Validate required fields for Office
        IF @v_OfficeCode IS NULL OR @v_OfficeCode = '' OR @v_OfficeName IS NULL OR @v_OfficeName = ''
        BEGIN
            SET @output_json = '{"status": "failure", "message": "Office Code and Office Name are required."}';
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Check if Office already exists by OfficeCode
        SET @v_OfficeCodeExist = (SELECT TOP 1 OfficeCode FROM dbo.Offices WHERE OfficeCode = @v_OfficeCode);        
        -- Use SCOPE_IDENTITY() instead of MAX for more precise behavior in inserts
        SELECT @v_MaxOfficeId = COALESCE(MAX(OfficeId), 0) + 1 FROM dbo.Offices;

        -- Check if address exists for this Office
        SET @v_AddressId = (SELECT TOP 1 AddressId FROM dbo.OfficeAddress WHERE OfficeId = @v_OfficeId AND AddressTypeId = @v_AddressTypeId);

        -- Insert or update Office and Address based on existence
        IF @v_OfficeCodeExist IS NULL AND @v_OfficeId IS NULL
        BEGIN
            -- Insert Office
            INSERT INTO dbo.Offices (OfficeId, OfficeParentId, OfficeTypeId, OfficeName, OfficeCode, FromDate, EndDate, Latitude, Longitude, created_userid, created_date)
            VALUES (@v_MaxOfficeId, @v_OfficeParentId, @v_OfficeTypeId, @v_OfficeName, @v_OfficeCode, @v_FromDate, @v_EndDate, @v_Latitude, @v_Longitude, @created_by, GETDATE());

            SET @v_OfficeId = @v_MaxOfficeId;

            -- Insert Address if necessary
            IF @v_AddressId IS NULL
            BEGIN
                INSERT INTO dbo.OfficeAddress (OfficeId, AddressTypeId, AddressLine1, StateId, DistrictId, CityId, PinCode, created_userid, created_date)
                VALUES (@v_OfficeId, @v_AddressTypeId, @v_AddressLine1, @v_StateId, @v_DistrictId, @v_CityId, @v_PinCode, @created_by, GETDATE());
            END

            SET @output_json = '{"status": "success", "message": "The RO Information has been saved successfully.", "OfficeId": ' + CAST(@v_OfficeId AS NVARCHAR) + '}';

        END
        ELSE
        BEGIN
            IF @v_OfficeId IS NOT NULL
            BEGIN
                -- Update Office
                UPDATE dbo.Offices
                SET OfficeTypeId = @v_OfficeTypeId,
                    OfficeParentId = @v_OfficeParentId,
                    OfficeName = @v_OfficeName,
                    OfficeCode = @v_OfficeCode,
                    Latitude = @v_Latitude,
                    Longitude = @v_Longitude,
                    FromDate = @v_FromDate,
                    EndDate = @v_EndDate
                WHERE OfficeId = @v_OfficeId;
                
                -- Update Address if necessary
                IF @v_AddressId IS NOT NULL
                BEGIN
                    UPDATE dbo.OfficeAddress
                    SET AddressLine1 = @v_AddressLine1,
                        StateId = @v_StateId,
                        DistrictId = @v_DistrictId,
                        CityId = @v_CityId,
                        PinCode = @v_PinCode
                    WHERE AddressId = @v_AddressId;
                END

                SET @output_json = '{"status": "success", "message": "The RO Information has been updated successfully.", "OfficeId": ' + CAST(@v_OfficeId AS NVARCHAR) + '}';
            END
            ELSE
            BEGIN
                SET @output_json = '{"status": "failure", "message": "The RO Code you entered already exists. Please enter a unique code."}';
            END
        END

        -- Commit the transaction
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        -- Log error details into ErrorHandling table
        INSERT INTO dbo.ErrorHandling (ErrorNumber, ErrorMessage, ErrorProcedure, ErrorDateTime)
        VALUES (ERROR_NUMBER(), ERROR_MESSAGE(), 'USP_UpsertROBasicInfo', GETDATE());
        
        -- Capture error details in the output
        SET @output_json = '{"status": "failure", "message": "' + COALESCE(ERROR_MESSAGE(), 'Unknown error') + '"}';
    END CATCH
END;
GO



ALTER PROCEDURE [dbo].[USP_UpsertROBillingInfo]
    @input_json NVARCHAR(MAX), -- Input JSON containing billing address details
    @created_by INT,           -- ID of the user who created the record
    @output_json NVARCHAR(MAX) OUTPUT -- Output response
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Declare variables to store parsed values from JSON for Billing Address
        DECLARE @v_OfficeId INT,
                @v_AddressTypeId INT,
                @v_GstNumber NVARCHAR(50),
                @v_BusinessName NVARCHAR(255),
                @v_PanNumber NVARCHAR(50),
                @v_TanNumber NVARCHAR(50),
                @v_AddressLine1 NVARCHAR(255),
                @v_StateId INT,                
                @v_DistrictId INT,
                @v_CityId INT,               
                @v_PinCode NVARCHAR(10);

        -- Parse JSON for Billing Address details
        SET @v_OfficeId = JSON_VALUE(@input_json, '$.OfficeId');
        SET @v_AddressTypeId = JSON_VALUE(@input_json, '$.AddressTypeId');
        SET @v_GstNumber = JSON_VALUE(@input_json, '$.GstNumber');
        SET @v_BusinessName = JSON_VALUE(@input_json, '$.BusinessName');
        SET @v_PanNumber = JSON_VALUE(@input_json, '$.PanNumber');
        SET @v_TanNumber = JSON_VALUE(@input_json, '$.TanNumber');
        SET @v_AddressLine1 = JSON_VALUE(@input_json, '$.AddressLine1');                    
        SET @v_StateId = JSON_VALUE(@input_json, '$.StateId');
        SET @v_DistrictId = JSON_VALUE(@input_json, '$.DistrictId');
        SET @v_CityId = JSON_VALUE(@input_json, '$.CityId');
        SET @v_PinCode = JSON_VALUE(@input_json, '$.PinCode');

        -- Validate required fields
        IF @v_OfficeId IS NULL OR @v_OfficeId = 0
        BEGIN
            SET @output_json = '{"status": "failure", "message": "The OfficeId field is required."}';
            RETURN;
        END
        
        -- Check if the AddressTypeId exists in the master.OfficeAddressTypes table
        IF NOT EXISTS (SELECT 1 FROM dbo.AddressTypes WHERE AddressTypeId = @v_AddressTypeId)
        BEGIN
            SET @output_json = '{"status": "failure", "message": "The Address Type is invalid. It does not exist in the system."}';
            RETURN;
        END
        
        -- Check if Billing Address already exists for this Office
        DECLARE @v_BillingAddressId INT;
        SET @v_BillingAddressId = (SELECT AddressId FROM dbo.OfficeAddress WHERE OfficeId = @v_OfficeId AND AddressTypeId = @v_AddressTypeId);

        -- If Billing Address does not exist, insert a new record; otherwise, update the existing record
        IF @v_BillingAddressId IS NULL
        BEGIN
			-- Insert OfficeTaxRelated
			INSERT INTO dbo.OfficeTaxRelated (OfficeId, GstNumber, BusinessName, PanNumber, TanNumber, created_userid, created_date)
			VALUES (@v_OfficeId, @v_GstNumber, @v_BusinessName, @v_PanNumber, @v_TanNumber, @created_by, GETDATE());

            -- Insert into the OfficeAddress table
            INSERT INTO dbo.OfficeAddress (AddressTypeId, OfficeId, AddressLine1, CityId, DistrictId, StateId, CountryId, PinCode, created_userid, created_date)
            VALUES (@v_AddressTypeId, @v_OfficeId, @v_AddressLine1, @v_CityId, @v_DistrictId, @v_StateId, 1, @v_PinCode, @created_by, GETDATE());		
            
            SET @output_json = '{"status": "success", "message": "The Billing address has been saved successfully."}';
        END
        ELSE
        BEGIN	
			-- Update the existing OfficeTaxRelated
			UPDATE dbo.OfficeTaxRelated SET GstNumber = @v_GstNumber,
                BusinessName = @v_BusinessName, PanNumber = @v_PanNumber, TanNumber = @v_TanNumber
            WHERE OfficeId = @v_OfficeId;

            -- Update the existing Billing Address record
            UPDATE dbo.OfficeAddress SET AddressLine1 = @v_AddressLine1, CityId = @v_CityId,
                DistrictId = @v_DistrictId, StateId = @v_StateId, PinCode = @v_PinCode
            WHERE AddressId = @v_BillingAddressId;
            
            SET @output_json = '{"status": "success", "message": "The billing address has been updated successfully."}';
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
		ROLLBACK TRANSACTION;

        -- Log error details into db_error_handling table
        INSERT INTO dbo.ErrorHandling (ErrorNumber, ErrorMessage, ErrorProcedure, ErrorDateTime)
        VALUES (ERROR_NUMBER(), ERROR_MESSAGE(), 'USP_UpsertROBillingInfo', GETDATE());
        
        -- Capture error details
        SET @output_json = '{"status": "failure", "message": "' + COALESCE(ERROR_MESSAGE(), 'Unknown error') + '"}';
    END CATCH
END;
GO



ALTER PROCEDURE [dbo].[USP_UpsertROBankInfo]
    @input_json NVARCHAR(MAX),    -- Input JSON containing bank account details
    @created_by INT,              -- Created by User ID
    @output_json NVARCHAR(MAX) OUTPUT -- Output JSON response
AS
BEGIN
    -- Declare variables to store parsed values from JSON
    DECLARE @v_OfficeId INT,
            @v_PrimaryAccountNo NVARCHAR(50),
            @v_PrimaryIFSCCode NVARCHAR(20),
            @v_SecondaryAccountNo NVARCHAR(50),
            @v_SecondaryIFSCCode NVARCHAR(20),
            @v_LedgerCode NVARCHAR(50),
            @v_CostCentreCode NVARCHAR(50);

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Parse the input JSON
        SET @v_OfficeId = JSON_VALUE(@input_json, '$.officeId');
        SET @v_PrimaryAccountNo = JSON_VALUE(@input_json, '$.primaryAccountNumber');
        SET @v_PrimaryIFSCCode = JSON_VALUE(@input_json, '$.primaryIfscCode');
        SET @v_SecondaryAccountNo = JSON_VALUE(@input_json, '$.secondaryAccountNumber');
        SET @v_SecondaryIFSCCode = JSON_VALUE(@input_json, '$.secondaryIfscCode');

        -- Validate required fields
        IF @v_OfficeId IS NULL OR @v_PrimaryAccountNo IS NULL OR @v_PrimaryIFSCCode IS NULL
        BEGIN
            SET @output_json = '{"status": "failure", "message": "OfficeId, primaryAccountNumber, and primaryIfscCode are mandatory fields."}';
            ROLLBACK TRANSACTION;  -- Rollback the transaction in case of failure
            RETURN;
        END

        -- Check if the record exists for the OfficeId
        DECLARE @existingAccountId INT;
        SET @existingAccountId = (SELECT TOP 1 OfficeId FROM OfficeBank WHERE OfficeId = @v_OfficeId);

        -- If the record exists, update; otherwise, insert new record
        IF @existingAccountId IS NULL
        BEGIN
            -- Insert new record for primary account
            INSERT INTO OfficeBank (OfficeId, PrimaryAccountNo, PrimaryIFSCCode, Priority, Language_id, recordstatus_id, created_userid, created_date)
            VALUES (@v_OfficeId, @v_PrimaryAccountNo, @v_PrimaryIFSCCode, 1, 1, 1, @created_by, GETDATE());

            -- Insert new record for secondary account, if provided
			IF ISNULL(@v_SecondaryAccountNo, '') <> '' AND ISNULL(@v_SecondaryIFSCCode, '') <> '' AND ISNULL(@v_SecondaryAccountNo, '0') <> '0'
            BEGIN
                INSERT INTO OfficeBank (OfficeId, PrimaryAccountNo, PrimaryIFSCCode, Priority, Language_id, recordstatus_id, created_userid, created_date)
                VALUES (@v_OfficeId, @v_SecondaryAccountNo, @v_SecondaryIFSCCode, 2, 1, 1, @created_by, GETDATE());
            END

            SET @output_json = '{"status": "success", "message": "The bank account has been successfully added."}';
        END
        ELSE
        BEGIN
            -- Update the existing primary account record for the given OfficeId
            UPDATE OfficeBank
            SET PrimaryAccountNo = @v_PrimaryAccountNo,
                PrimaryIFSCCode = @v_PrimaryIFSCCode,
                Priority = 1, -- Setting priority for primary account
                modified_userid = @created_by,
                modified_date = GETDATE()
            WHERE OfficeId = @v_OfficeId AND Priority = 1;

            -- Update or insert the secondary account record, if provided
			IF ISNULL(@v_SecondaryAccountNo, '') <> '' AND ISNULL(@v_SecondaryIFSCCode, '') <> '' AND ISNULL(@v_SecondaryAccountNo, '0') <> '0'
            BEGIN
                DECLARE @existingSecondaryAccountId INT;
                SET @existingSecondaryAccountId = (SELECT TOP 1 OfficeId FROM OfficeBank WHERE OfficeId = @v_OfficeId AND Priority = 2);

                IF @existingSecondaryAccountId IS NULL
                BEGIN
                    -- Insert new record for secondary account
                    INSERT INTO OfficeBank (OfficeId, PrimaryAccountNo, PrimaryIFSCCode, Priority, Language_id, recordstatus_id, created_userid, created_date)
                    VALUES (@v_OfficeId, @v_SecondaryAccountNo, @v_SecondaryIFSCCode, 2, 1, 1, @created_by, GETDATE());
                END
                ELSE
                BEGIN
                    -- Update the existing secondary account record for the given OfficeId
                    UPDATE OfficeBank
                    SET PrimaryAccountNo = @v_SecondaryAccountNo,
                        PrimaryIFSCCode = @v_SecondaryIFSCCode,
                        Priority = 2, -- Setting priority for secondary account
                        modified_userid = @created_by,
                        modified_date = GETDATE()
                    WHERE OfficeId = @v_OfficeId AND Priority = 2;
                END
            END

            SET @output_json = '{"status": "success", "message": "The bank account has been successfully updated."}';
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
		ROLLBACK TRANSACTION;

        -- Log error details into ErrorHandling table
        INSERT INTO dbo.ErrorHandling (ErrorNumber, ErrorMessage, ErrorProcedure, ErrorDateTime)
        VALUES (ERROR_NUMBER(), ERROR_MESSAGE(), 'USP_UpsertROBasicInfo', GETDATE());

        -- Error handling
        SET @output_json = '{"status": "failure", "message": "' + COALESCE(ERROR_MESSAGE(), 'Unknown error') + '"}';       
    END CATCH
END;
GO



ALTER PROCEDURE [dbo].[USP_UpsertROContactInfo]
    @input_json NVARCHAR(MAX), -- Input JSON containing contact information
    @created_by INT,           -- ID of the user who created the record
    @output_json NVARCHAR(MAX) OUTPUT
AS
BEGIN
    BEGIN TRY
		BEGIN TRANSACTION;

        -- Declare variables to store parsed values from JSON for Contact Info
        DECLARE @v_OfficeId INT,
		        @v_EmailId NVARCHAR(255),
                @v_ContactNumber NVARCHAR(50),
                @v_Landline NVARCHAR(50),
                @v_FaxNumber NVARCHAR(50),
                @existingContactId INT,
                @oldData NVARCHAR(MAX),
                @newData NVARCHAR(MAX),
				@v_IPAddress NVARCHAR(50);    -- IP Address of the user

        -- Parse JSON for Contact Info details
		SET @v_OfficeId = JSON_VALUE(@input_json, '$.OfficeId');
        SET @v_EmailId = JSON_VALUE(@input_json, '$.EmailId');
        SET @v_ContactNumber = JSON_VALUE(@input_json, '$.ContactNumber');        
        SET @v_FaxNumber = JSON_VALUE(@input_json, '$.FaxNumber');
		SET @v_Landline = JSON_VALUE(@input_json, '$.Landline');
		SET @v_IPAddress = JSON_VALUE(@input_json, '$.IPAddress');
        
		 -- Validate required fields
        IF @v_OfficeId IS NULL OR @v_OfficeId = 0
        BEGIN
            SET @output_json = '{"status": "failure", "message": "OfficeId is required."}';
            RETURN;
        END

        -- Check if the contact already exists based on emailid
        SET @existingContactId = (SELECT OfficeId FROM OfficeContacts WHERE OfficeId = @v_OfficeId);

        -- If the contact does not exist, insert a new record; otherwise, update the existing record
        IF @existingContactId IS NULL
        BEGIN
            -- Insert into the OfficeContactDetails table
            INSERT INTO OfficeContacts (OfficeId, Mobileno, LandlineNo, Email, created_userid, created_date)
            VALUES (@v_OfficeId, @v_ContactNumber, @v_FaxNumber, @v_EmailId, @created_by, GETDATE());
            
            SET @output_json = '{"status": "success", "message": "The Contact information has been successfully added."}';
        END
        ELSE
        BEGIN
            -- Get old data for audit log
            SELECT @oldData = CONCAT('{"EmailId": "', Email, '", "ContactNumber": "', Mobileno, '", "Landline": "', LandlineNo, '"}')
            FROM OfficeContacts
            WHERE OfficeId = @existingContactId;

            -- Update the existing Contact Info record
            UPDATE OfficeContacts
            SET Mobileno = @v_ContactNumber,
                LandlineNo = @v_FaxNumber,
                Email = @v_EmailId,
                modified_userid = @created_by,
                modified_date = GETDATE()
            WHERE OfficeId = @existingContactId;

            -- Prepare new data for audit log
            SET @newData = CONCAT('{"EmailId": "', @v_EmailId, '", "ContactNumber": "', @v_ContactNumber, '", "Landline": "', @v_FaxNumber, '"}');

            -- Insert into the AuditLog table
            --INSERT INTO AuditLog (TableName, OperationType, RecordID, OldData, NewData, CreatedBy, CreatedDate, IPAddress)
            --VALUES ('OfficeContactDetails','Update',@existingContactId,@oldData,@newData,@created_by,GETDATE(), @v_IPAddress);

            SET @output_json = '{"status": "success", "message": "The contact information has been updated successfully."}';
        END

		COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
		ROLLBACK TRANSACTION;

        -- Log error details into db_error_handling table
        INSERT INTO ErrorHandling (ErrorNumber, ErrorMessage, ErrorProcedure, ErrorDateTime)
        VALUES (ERROR_NUMBER(), ERROR_MESSAGE(), 'USP_UpsertROContactInfo', GETDATE());
        
        -- Capture error details
        SET @output_json = '{"status": "failure", "message": "' + COALESCE(ERROR_MESSAGE(), 'Unknown error') + '"}';
    END CATCH
END;
GO


--=============================================
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'OfficeStatus')
BEGIN
CREATE TABLE [dbo].[OfficeStatus](
	[OfficeId] [int] NOT NULL,
	[OfficeTypeId] [int] NOT NULL,
	[ProcessId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[Reason] [nvarchar](250) NULL,
	[StepId] [int] NOT NULL,
	[ActionBy] [int] NULL,
	[Language_id] [smallint] NULL,
	[recordstatus_id] [smallint] NULL,
	[created_userid] [int] NULL,
	[created_date] [datetime] NULL,
	[modified_userid] [int] NULL,
	[modified_date] [datetime] NULL
) ON [mdm]
END
GO


--=============================================
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'OfficeStatusHistory')
BEGIN
CREATE TABLE [dbo].[OfficeStatusHistory](
	[OfficeId] [int] NOT NULL,
	[OfficeTypeId] [int] NOT NULL,
	[ProcessId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[Reason] [nvarchar](250) NULL,
	[StepId] [int] NOT NULL,
	[ActionBy] [int] NULL,
	[Language_id] [smallint] NULL,
	[Recordstatus_id] [smallint] NULL,
	[created_userid] [int] NULL,
	[created_date] [datetime] NULL,
	[modified_userid] [int] NULL,
	[modified_date] [datetime] NULL
) ON [mdm]
END
GO



ALTER PROCEDURE [dbo].[USP_UpsertOfficeStatus]
    @OfficeId INT,                -- Input: OfficeId
    @OfficeTypeId INT,            -- Input: OfficeTypeId
    @ProcessId INT,               -- Input: ProcessId
    @StatusId INT,                -- Input: StatusId
	@Reason NVARCHAR(255),        -- Input: Reason
    @StepId INT,                  -- Input: StepId
	@ActionBy INT,                  -- Input: ActionBy
    @OutputJson NVARCHAR(MAX) OUTPUT  -- Output: JSON response with the result of the operation
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ResultMessage NVARCHAR(500);
    DECLARE @OperationType NVARCHAR(10);

    -- Check if the office status exists
    IF EXISTS (SELECT 1
               FROM OfficeStatus
               WHERE OfficeId = @OfficeId
                 AND OfficeTypeId = @OfficeTypeId)
    BEGIN
        -- If exists, update the record
        UPDATE OfficeStatus
        SET ProcessId =  @ProcessId,
		    StatusId = @StatusId,
			Reason = @Reason,
            StepId = @StepId,
			ActionBy = @ActionBy,
            modified_date = GETDATE()
        WHERE OfficeId = @OfficeId
          AND OfficeTypeId = @OfficeTypeId;

        -- Set operation type and result message
        SET @OperationType = 'Update';
        SET @ResultMessage = 'Office status updated successfully.';
    END
    ELSE
    BEGIN
        -- If not exists, insert a new record
        INSERT INTO OfficeStatus (OfficeId, OfficeTypeId, ProcessId, StatusId, Reason, StepId, ActionBy, created_date)
        VALUES (@OfficeId, @OfficeTypeId, @ProcessId, @StatusId, @Reason, @StepId, @ActionBy, GETDATE());

        -- Set operation type and result message
        SET @OperationType = 'Insert';
        SET @ResultMessage = 'Office status inserted successfully.';
    END

	INSERT INTO OfficeStatusHistory(OfficeId, OfficeTypeId, ProcessId, StatusId, Reason, StepId, ActionBy, created_date)
    VALUES (@OfficeId, @OfficeTypeId, @ProcessId, @StatusId, @Reason, @StepId, @ActionBy, GETDATE());

    -- Prepare the output JSON manually using FOR JSON PATH
    SET @OutputJson = (
        SELECT 
            'success' AS status,
            @ResultMessage AS message,
            (
                SELECT 
                    @OfficeId AS OfficeId,
                    @OfficeTypeId AS OfficeTypeId,
                    @ProcessId AS ProcessId,
                    @StatusId AS StatusId,
                    @StepId AS StepId					
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            ) AS responseData
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    );
END;
GO