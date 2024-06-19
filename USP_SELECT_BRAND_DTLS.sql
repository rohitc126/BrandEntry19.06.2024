USE [SGX_TEST]
GO

/****** Object:  StoredProcedure [AGG].[USP_SELECT_BRAND_DTLS]    Script Date: 6/19/2024 4:31:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




/*
 
  CREATED BY : ROHIT CHAURASIYA
  CREATED DATE :08/04/2024

  [AGG].[USP_SELECT_BRAND_DTLS]1

*/               
CREATE PROCEDURE [AGG].[USP_SELECT_BRAND_DTLS]  
@Material_Id	INT = null
AS
BEGIN
 SELECT B.BRAND_ID,B.Material_Id,M.Material_Name, B.BRAND_CODE, B.BRAND_NAME, B.Rate, B.UOM_ID,U.UOM,convert(varchar(10),Effective_Dt,103) as Effective_Dt, B.REMARKS,B.STATUS 
 FROM [AGG].[tbl_Brand_Mst] B
 INNER JOIN [AGG].[Tbl_Material_Mst] M ON B.Material_Id = M.Material_Id
 INNER JOIN [dbo].[tbl_UOMMaster] U ON B.UOM_ID = U.UOM_ID
 WHERE B.Status = 1 
 AND  (B.Material_Id = @Material_Id OR ISNULL(@Material_ID,0) = 0)   
	
   
  
	END
GO

