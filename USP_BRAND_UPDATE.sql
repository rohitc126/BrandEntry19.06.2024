USE [SGX_TEST]
GO

/****** Object:  StoredProcedure [AGG].[USP_BRAND_UPDATE]    Script Date: 6/19/2024 4:32:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
CREATE BY		: Rohit Chaurasiya
CREATED DATE	: 12/02/2024

*/
	CREATE PROCEDURE [AGG].[USP_BRAND_UPDATE] 
	@ERRORSTR VARCHAR(200) OUTPUT,
	@BRAND_ID INT , 
	@Updateby   char(7)

	AS
	BEGIN

		  SET @ERRORSTR = ''
		  DECLARE @COUNT INT

		  UPDATE [AGG].[tbl_Brand_Mst] SET 
		  UpdateOn=GetDate(),    
           UpdateBy=@Updateby  , STATUS=0	  WHERE BRAND_ID = @BRAND_ID
		  SET @COUNT=(SELECT ISNULL(@@ROWCOUNT,0))
		  
		

		  IF(@COUNT>0)
		  BEGIN
			  SET @ERRORSTR = ''
		  END
		  ELSE
		  BEGIN
			  SET @ERRORSTR = 'RECORD UPDATION FAILED !!'
		  END
	

		  IF @@ERROR <> 0
		  BEGIN 
			   SET @ERRORSTR = 'DATABASE ERROR OCCUR FOR TABLE BRAND ENTRY !'
		  RETURN
		  END
	
	END
GO

