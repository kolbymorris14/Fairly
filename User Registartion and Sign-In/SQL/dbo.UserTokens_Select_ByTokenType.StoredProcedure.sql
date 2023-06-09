USE [Fairly]
GO
/****** Object:  StoredProcedure [dbo].[UserTokens_Select_ByTokenType]    Script Date: 4/11/2023 5:14:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 --=============================================
 --Author: <Morris, Kolby>
 --Create date: <2023-04-07>
 --Description: <Select_Select_ByTokenType for UserToken>
 --Code Reviewer: Jose Chism

 --MODIFIED BY: 
 --MODIFIED DATE:
 --Code Reviewer:
 --Note: 
 --=============================================

CREATE PROC [dbo].[UserTokens_Select_ByTokenType]
				@TokenType int

AS

/*-----------------TEST CODE ---------------------

	Declare		@TokenType int = '1'

	Execute dbo.UserTokens_Select_ByTokenType
				@TokenType

*/

BEGIN

	SELECT		[Token]
				,[UserId]
				,[TokenType]

	FROM		[dbo].[UserTokens]

	WHERE		TokenType = @TokenType

END


GO
