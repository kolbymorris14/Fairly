USE [Fairly]
GO
/****** Object:  StoredProcedure [dbo].[Users_Confirm]    Script Date: 4/11/2023 5:14:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 --=============================================
 --Author: <Morris, Kolby>
 --Create date: <2023-04-07>
 --Description: <Select_ById for Users>
 --Code Reviewer: Jose Chism 

 --MODIFIED BY: 
 --MODIFIED DATE:
 --Code Reviewer:
 --Note: 
 --=============================================
 
 CREATE PROC [dbo].[Users_Confirm]
					@UserId int

			
AS
/*---------------- TEST CODE -----------------

	DECLARE			@UserId int = 11

	Execute dbo.Users_Select_ById @UserId

	EXECUTE dbo.Users_Confirm
					@UserId

	
	Execute dbo.Users_Select_ById @UserId

*/

BEGIN
	
	DECLARE @DateModified datetime2(7) = GETUTCDATE();

	UPDATE [dbo].[Users]
	SET				
					[IsConfirmed] = 1
					,[DateModified] = @DateModified
	
	WHERE Id = @UserId

END
GO
