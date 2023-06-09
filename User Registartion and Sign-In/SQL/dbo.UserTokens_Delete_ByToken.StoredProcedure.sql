USE [Fairly]
GO
/****** Object:  StoredProcedure [dbo].[UserTokens_Delete_ByToken]    Script Date: 4/11/2023 5:14:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 --=============================================
 --Author: <Morris, Kolby>
 --Create date: <2023-04-07>
 --Description: <Delete_ByTokenType for UserToken>
 --Code Reviewer: Jose Chism

 --MODIFIED BY: 
 --MODIFIED DATE:
 --Code Reviewer:
 --Note: 
 --=============================================

CREATE PROC [dbo].[UserTokens_Delete_ByToken]
				@Token varchar(200)
AS

/*-----------------TEST CODE ---------------------
	DECLARE		@Token varchar(200) = 'token string goes here'

	SELECT		[Token]
	FROM		dbo.UserTokens
	WHERE		Token = @Token

	EXECUTE dbo.UserTokens_Delete_ByToken
				@Token

	SELECT		[Token]
	FROM		dbo.UserTokens
	WHERE		Token = @Token
*/

BEGIN

	DELETE
	FROM		[dbo].UserTokens
	WHERE		Token = @Token

END


GO
