USE [SGX_TEST]
GO

/****** Object:  Table [AGG].[tbl_Brand_Mst]    Script Date: 6/19/2024 4:30:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [AGG].[tbl_Brand_Mst](
	[BRAND_ID] [int] IDENTITY(1,1) NOT NULL,
	[Material_Id] [int] NULL,
	[BRAND_CODE] [varchar](10) NOT NULL,
	[BRAND_NAME] [varchar](150) NULL,
	[Rate] [numeric](8, 2) NULL,
	[UOM_ID] [int] NULL,
	[Effective_Dt] [datetime] NULL,
	[REMARKS] [varchar](50) NULL,
	[ADD_ON] [datetime] NOT NULL,
	[ADD_BY] [char](7) NOT NULL,
	[STATUS] [bit] NOT NULL,
	[UpdateOn] [datetime] NULL,
	[UpdateBy] [char](7) NULL,
 CONSTRAINT [PK_tbl_Brand_Mst] PRIMARY KEY CLUSTERED 
(
	[BRAND_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [AGG].[tbl_Brand_Mst] ADD  CONSTRAINT [DF_Table_1_Addon]  DEFAULT (getdate()) FOR [ADD_ON]
GO

ALTER TABLE [AGG].[tbl_Brand_Mst] ADD  CONSTRAINT [DF_Table_1_Status]  DEFAULT ((1)) FOR [STATUS]
GO

