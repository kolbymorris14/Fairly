USE [Fairly]
GO
/****** Object:  StoredProcedure [dbo].[TokenTypes_SelectAll]    Script Date: 4/11/2023 5:14:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 --=============================================
 --Author: <Morris, Kolby>
 --Create date: <2023-04-07>
 --Description: <SelectAll for TokenTypes>
 --Code Reviewer: Jose Chism

 --MODIFIED BY: 
 --MODIFIED DATE:
 --Code Reviewer:
 --Note: 
 --=============================================

CREATE PROC [dbo].[TokenTypes_SelectAll]

AS

/*-----------------TEST CODE ---------------------

	EXECUTE dbo.TokenTypes_SelectAll
*/

BEGIN

	SELECT	[Id]
			,[Name]

	FROM	[dbo].[TokenTypes]

END


GO
