USE [SGX_TEST]
GO

/****** Object:  Table [AGG].[Tbl_Material_Mst]    Script Date: 6/19/2024 4:29:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [AGG].[Tbl_Material_Mst](
	[Material_Id] [int] IDENTITY(1,1) NOT NULL,
	[Material_Name] [varchar](50) NOT NULL,
	[Description] [varchar](500) NULL,
	[MC_ID] [int] NULL,
	[SeqNo] [int] NULL,
	[Status] [varchar](1) NULL,
 CONSTRAINT [PK_Tbl_Material_Mst] PRIMARY KEY CLUSTERED 
(
	[Material_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [AGG].[Tbl_Material_Mst] ADD  CONSTRAINT [DF_Tbl_Material_Mst_Status]  DEFAULT ('Y') FOR [Status]
GO

