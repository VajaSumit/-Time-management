using UserLogManagement.Models;

namespace UserLogManagement.ViewModels
{
    public class CalendarDayViewModel
    {
        public DateTime Date { get; set; }
        public string Color { get; set; }
        public userlog UserLog { get; set; }
    }
}
