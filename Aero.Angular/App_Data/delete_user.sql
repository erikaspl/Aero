DECLARE @userId INT

select @userId = up.UserId from UserProfile up
where up.UserName LIKE 'erikos'

SELECT @userId
BEGIN TRAN
delete from webpages_Membership
where UserId = @userId

delete from UserProfile
where UserId = @userId

--Rollback
commit