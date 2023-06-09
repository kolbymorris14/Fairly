USE [Fairly]
GO
/****** Object:  StoredProcedure [dbo].[UserTokens_Insert]    Script Date: 4/11/2023 5:14:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 --=============================================
 --Author: <Morris, Kolby>
 --Create date: <2023-04-07>
 --Description: <Insert for UserToken>
 --Code Reviewer: Jose Chism

 --MODIFIED BY: 
 --MODIFIED DATE:
 --Code Reviewer:
 --Note: 
 --=============================================

CREATE PROC [dbo].[UserTokens_Insert]
				@Token varchar(200)
				,@UserId int
				,@TokenType int

AS

/*-----------------TEST CODE ---------------------

	DECLARE		@Token varchar(200) = 'token string goes here'
				,@UserId int = '3'
				,@TokenType int = '1'

	EXECUTE	dbo.UserTokens_Insert
				@Token
				,@UserId
				,@TokenType

	SELECT		
				[Token] 
	FROM		dbo.UserTokens

*/

BEGIN

	INSERT INTO [dbo].[UserTokens]
				([Token]
				,[UserId]
				,[TokenType])
     VALUES
			   (@Token
			   ,@UserId
			   ,@TokenType)

END


GO
