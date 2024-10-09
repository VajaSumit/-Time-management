namespace UserLogManagement.ViewModels
{
    public class CalendarViewModel
    {
        public List<CalendarDayViewModel> Days { get; set; } = new List<CalendarDayViewModel>();
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
