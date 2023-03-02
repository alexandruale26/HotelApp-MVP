using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAppLibrary.Data;

public class SqlData : IDatabaseData
{
    private readonly ISQLDataAccess db;
    private const string connectionStringName = "SqlDb";

    public SqlData(ISQLDataAccess db)
    {
        this.db = db;
    }

    public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
    {

        return db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetAvailableTypes",
                                                   new { startDate, endDate },
                                                   connectionStringName,
                                                   true);
    }

    public void BookGuest(string firstName,
                          string lastName,
                          DateTime startDate,
                          DateTime endDate,
                          int roomTypeId)
    {
        GuestModel guest = db.LoadData<GuestModel, dynamic>("dbo.spGuests_Insert",
                                                            new { firstName, lastName },
                                                            connectionStringName,
                                                            true).First();

        RoomTypeModel roomType = db.LoadData<RoomTypeModel, dynamic>(@"select * from dbo.RoomTypes where Id = @Id;",
                                                                     new { Id = roomTypeId }, connectionStringName,
                                                                     false).First();

        TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);

        List<RoomModel> availableRooms = db.LoadData<RoomModel, dynamic>("dbo.spRooms_GetAvailableRooms",
                                                                         new { startDate, endDate, roomTypeId },
                                                                         connectionStringName,
                                                                         true);

        db.SaveData("dbo.spBookings_Insert",
                    new { 
                        RoomId = availableRooms.First().Id,
                        GuestId = guest.Id,
                        StartDate = startDate,
                        EndDate = endDate,
                        TotalCost = timeStaying.Days * roomType.Price
                    },
                    connectionStringName,
                    true);
    }

    public List<BookingFullModel> SearchBookings(string lastName)
    {
        return db.LoadData<BookingFullModel, dynamic>("dbo.spBookings_Search",
                                                  new { lastName, startDate = DateTime.Now.Date },
                                                  connectionStringName,
                                                  true);
    }

    public void CheckInGuest(int bookingId)
    {
        db.SaveData("dbo.spBookings_CheckIn", new { Id = bookingId }, connectionStringName, true);
    }

    public RoomTypeModel GetRoomTypeByID(int id)
    {
        return db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetById",
                                                   new { Id = id },
                                                   connectionStringName,
                                                   true).FirstOrDefault();
    }
}
