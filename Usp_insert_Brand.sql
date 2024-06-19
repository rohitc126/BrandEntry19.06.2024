USE [SGX_TEST]
GO

/****** Object:  StoredProcedure [AGG].[USP_INSERT_BRAND]    Script Date: 6/19/2024 4:24:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [AGG].[USP_INSERT_BRAND]
@ERRORSTR   VARCHAR(200) OUTPUT,
@BRAND_ID   int OUTPUT,
@BRAND_CODE VARCHAR(150) OUTPUT,
@BRAND_NAME varchar(150),
@Material_Id INT,
@Rate   NUMERIC,
@UOM_ID INT,
@Effective_Dt DATETIME,
@REMARKS    VARCHAR(50),
@ADD_BY      char(7)
AS
BEGIN
	SET NOCOUNT ON;
	SET @ERRORSTR='' 	 


	 
    IF EXISTS (SELECT 1 FROM [AGG].[tbl_Brand_Mst] WHERE BRAND_NAME = @BRAND_NAME AND Material_Id = @Material_Id AND  Status = 1  )
    BEGIN
        DECLARE @Existing_Effective_Dt DATETIME;

      
        SELECT @Existing_Effective_Dt = MAX( Effective_Dt)
        FROM [AGG].[tbl_Brand_Mst]
        WHERE BRAND_NAME = @BRAND_NAME AND Material_Id = @Material_Id AND STATUS=1;

     
        IF @Effective_Dt <= @Existing_Effective_Dt AND @BRAND_NAME = @BRAND_NAME AND @Material_Id = @Material_Id
        BEGIN
          
              -- SET @ERRORSTR = 'Entered Effective_Dt is not valid and This Product Of Brand Already Exists.';
			     SET @ERRORSTR = 'Brand Name already exist in system with effective date '+cast ( @Existing_Effective_Dt as varchar)+'.!!';
			  	 SET @BRAND_ID = -1;
				 SET @BRAND_CODE=''
            
            RETURN;
        END
    END


	DECLARE @PREFIX VARCHAR(150)  
	SELECT @PREFIX='BRAND'

	 DECLARE @IDTEMP INT
		  SELECT @IDTEMP = MAX(CAST(left(RIGHT(BRAND_CODE,4),5) AS INT)) FROM [AGG].[tbl_Brand_Mst]
		  SELECT @IDTEMP = CASE WHEN @IDTEMP IS NULL THEN 000001
		  ELSE @IDTEMP + 1
     END;

     SELECT @BRAND_CODE = @PREFIX + REPLICATE('0', 5-len(@IDTEMP)) + CAST(@IDTEMP AS VARCHAR)
 

     INSERT INTO [AGG].[tbl_Brand_Mst]
	 (
	  [BRAND_CODE]
	 ,[BRAND_NAME]
	 ,[Material_Id]
	 ,[Rate]
	 ,[UOM_ID]
	 ,[Effective_Dt]
	 ,[REMARKS]
	 ,[ADD_BY]

	 )
	 VALUES 
	 (
	 @BRAND_CODE,
	UPPER( @BRAND_NAME),
	 @Material_Id,
	 @Rate,
	 @UOM_ID,
	 @Effective_Dt,
	 @REMARKS,
	 @ADD_BY
	 )

	 SET @BRAND_ID =  SCOPE_IDENTITY();

	 IF @@ERROR <> 0
	 BEGIN
	 SET @ERRORSTR = 'DATA BASE ERROR OCCUR FOR TABLE BRAND ENTRY !'
	 SET @BRAND_ID = -1;
	 RETURN;
	 END
END
GO

