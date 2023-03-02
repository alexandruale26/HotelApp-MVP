CREATE PROCEDURE [dbo].[spBookings_Insert]
	@RoomId int,
	@GuestId int,
	@StartDate date,
	@EndDate date,
	@TotalCost money
AS
begin
	set nocount on;

	insert into dbo.Bookings (RoomId, GuestId, StartDate, EndDate, TotalCost)
	values (@RoomId, @GuestId, @StartDate, @EndDate, @TotalCost);
end
