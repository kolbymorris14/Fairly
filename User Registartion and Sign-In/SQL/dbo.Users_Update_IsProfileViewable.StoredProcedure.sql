USE [Fairly]
GO
/****** Object:  StoredProcedure [dbo].[Users_Update_IsProfileViewable]    Script Date: 4/11/2023 5:14:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 --=============================================
 --Author: <Morris, Kolby>
 --Create date: <2023-04-07>
 --Description: <Update_IsProfileViewable for Users>
 --Code Reviewer: Jose Chism

 --MODIFIED BY: 
 --MODIFIED DATE:
 --Code Reviewer:
 --Note: 
 --=============================================
 
 CREATE PROC [dbo].[Users_Update_IsProfileViewable]
					@UserId int
					,@IsProfileViewable bit

			
AS
/*---------------- TEST CODE -----------------

	DECLARE			@UserId int = 11
					,@IsProfileViewable bit = 0

	Execute dbo.Users_Select_ById @UserId

	EXECUTE dbo.Users_Update_IsProfileViewable
					@UserId
					,@IsProfileViewable

	Execute dbo.Users_Select_ById @UserId

*/

BEGIN
	
	DECLARE @DateModified datetime2(7) = GETUTCDATE();

	UPDATE [dbo].[Users]
	SET				
					[IsProfileViewable] = @IsProfileViewable
					,[DateModified] = @DateModified
	
	WHERE Id = @UserId

END
GO
