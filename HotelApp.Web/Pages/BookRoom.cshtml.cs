using HotelAppLibrary.Data;
using HotelAppLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Web.Pages;

public class BookRoomModel : PageModel
{
    private readonly IDatabaseData db;

    [BindProperty(SupportsGet = true)]
    public int RoomTypeId { get; set; }

    [DataType(DataType.Date)]
    [BindProperty(SupportsGet = true)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    [BindProperty(SupportsGet = true)]
    public DateTime EndDate { get; set; }

    [BindProperty]
    public string FirstName { get; set; }

    [BindProperty]
    public string LastName { get; set; }

    public RoomTypeModel selectedRoomType { get; set; }

    public BookRoomModel(IDatabaseData db)
    {
        this.db = db;
    }

    public void OnGet()
    {
        if (RoomTypeId > 0)
        {
            selectedRoomType = db.GetRoomTypeByID(this.RoomTypeId);
        }
    }

    public IActionResult OnPost()
    {
        db.BookGuest(FirstName, LastName, StartDate, EndDate, RoomTypeId);
        return RedirectToPage("/Index");
    }
}
