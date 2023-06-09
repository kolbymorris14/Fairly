USE [Fairly]
GO
/****** Object:  StoredProcedure [dbo].[Users_Select_AuthData]    Script Date: 4/11/2023 5:14:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 --=============================================
 --Author: <Morris, Kolby>
 --Create date: <2023-04-07>
 --Description: <Select_AuthData for Users>
 --Code Reviewer: Jose Chism

 --MODIFIED BY: 
 --MODIFIED DATE:
 --Code Reviewer:
 --Note: 
 --=============================================
 
 CREATE PROC [dbo].[Users_Select_AuthData]
			@Email nvarchar(255)
AS
/*---------------- TEST CODE -----------------

	Declare @Email nvarchar(255) = 'TestInsert2@test.com'

	Execute dbo.Users_Select_AuthData
			@Email

*/

BEGIN

	SELECT	u.[Id]
			,u.[Email]
			,u.[Password]
			,Roles = 
					(SELECT	r.Name
					 FROM	dbo.Roles as r
							INNER JOIN dbo.UserRoles as ur
								ON r.Id = ur.RoleId
					 WHERE	UserId = u.Id
					 FOR JSON AUTO
					 )

	FROM	[dbo].[Users] as u

	WHERE	[Email] = @Email

END
GO
