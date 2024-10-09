using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using UserLogManagement.Models;
using UserLogManagement.ViewModels;

namespace UserLogManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbContextFile db;

        public HomeController(DbContextFile db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ManageTime()
        {
            return View();
        }

        [HttpPost]
        public JsonResult PunchIn()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Json(new { success = false, message = "User is not logged in." });
            }

            var userlog = new userlog
            {
                UserId = userId,
                PunchIn = DateTime.Now,
                Status = "PunchIn"
            };

            db.userlogs.Add(userlog);
            db.SaveChanges();

            return Json(new { success = true });

        }


        [HttpPost]
        public JsonResult TakeBreak()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Json(new { success = false, message = "User is not logged in." });
            }

            var userlog = db.userlogs.Where(ul => ul.UserId == userId && ul.PunchIn.Value.Date == DateTime.Now.Date).FirstOrDefault();

            if (userlog == null)
            {
                return Json(new { success = false, message = "No active session found." });
            }

            userlog.BreakStart = DateTime.Now;
            userlog.Status = "Break";

            db.userlogs.Update(userlog);
            db.SaveChanges();

            return Json(new { success = true });

        }

        [HttpPost]
        public JsonResult ContinueWork()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Json(new { success = false, message = "User is not logged in." });
            }

            var userlog = db.userlogs.Where(ul => ul.UserId == userId && ul.PunchIn.Value.Date == DateTime.Now.Date && ul.BreakStart != null).FirstOrDefault();

            if (userlog == null)
            {
                return Json(new { success = false, message = "Not valid  user data found." });
            }

            userlog.BreakEnd = DateTime.Now;
            userlog.Status = "PunchIn";

            TimeSpan alreadybreaktime;
            TimeSpan breakDifference;

            if (userlog.BreakDiffrent != null)
            {
                alreadybreaktime = userlog.BreakDiffrent.Value;
                breakDifference = userlog.BreakEnd.Value - userlog.BreakStart.Value;
                alreadybreaktime = alreadybreaktime + breakDifference;
                userlog.BreakDiffrent = alreadybreaktime;
            }
            else
            {
                breakDifference = userlog.BreakEnd.Value - userlog.BreakStart.Value;
                userlog.BreakDiffrent = breakDifference;
            }

            db.userlogs.Update(userlog);
            db.SaveChanges();

            return Json(new { success = true });

        }


        [HttpGet]
        public IActionResult GetData()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //var date1 = userlog.PunchIn.Value.Date;

            //var date2 = DateTime.Now.Date;


            var userlog = db.userlogs.FirstOrDefault(ul => ul.UserId == userId && ul.PunchIn.Value.Date == DateTime.Now.Date);

            if (userlog != null)
            {
               
                if (userlog.PunchIn.Value != null)
                {
                                     
                    if (userlog.BreakStart != null)
                    {

                        if (userlog.BreakDiffrent != null)
                        {

                            if (userlog.PunchOut != null)
                            {

                                return Json(new
                                {
                                    success = true,
                                    punchInTime = userlog.PunchIn.Value.ToString("o"), // ISO 8601 format
                                    breakDiffrent = userlog.BreakDiffrent.HasValue ? userlog.BreakDiffrent.Value.ToString(@"hh\:mm\:ss") : null,
                                    breakStart = userlog.BreakStart.Value.ToString("o"),
                                    status = userlog.Status,
                                    punchOutTime = userlog.PunchOut.HasValue ? userlog.PunchOut.Value.ToString("o") : null,
                                    workinghours = userlog.Diffrent.HasValue ? userlog.Diffrent.Value.ToString(@"hh\:mm\:ss") : null
                            });
                            }
                            else
                            {
                                return Json(new
                                {
                                    success = true,
                                    punchInTime = userlog.PunchIn.Value.ToString("o"), // ISO 8601 format
                                    breakDiffrent = userlog.BreakDiffrent.HasValue ? userlog.BreakDiffrent.Value.ToString(@"hh\:mm\:ss") : null,
                                    breakStart = userlog.BreakStart.Value.ToString("o"),
                                    status = userlog.Status,
                                    punchOutTime = userlog.PunchOut.HasValue ? userlog.PunchOut.Value.ToString("o") : null,
                                    workinghours = userlog.Diffrent.HasValue ? userlog.Diffrent.Value.ToString(@"hh\:mm\:ss") : null
                                });
                            }

                           
                        }
                        else
                        {
                            if (userlog.PunchOut != null)
                            {
                                return Json(new
                                {
                                    success = true,
                                    punchInTime = userlog.PunchIn.Value.ToString("o"), // ISO 8601 format
                                    breakDiffrent = false,
                                    breakStart = userlog.BreakStart.Value.ToString("o"),
                                    status = userlog.Status,
                                    punchOutTime = userlog.PunchOut.HasValue ? userlog.PunchOut.Value.ToString("o") : null,
                                    workinghours = userlog.Diffrent.HasValue ? userlog.Diffrent.Value.ToString(@"hh\:mm\:ss") : null
                                });
                            }
                            else
                            {
                                return Json(new
                                {
                                    success = true,
                                    punchInTime = userlog.PunchIn.Value.ToString("o"), // ISO 8601 format
                                    breakDiffrent = false,
                                    breakStart = userlog.BreakStart.Value.ToString("o"),
                                    status = userlog.Status,
                                    punchOutTime = userlog.PunchOut.HasValue ? userlog.PunchOut.Value.ToString("o") : null,
                                    workinghours = userlog.Diffrent.HasValue ? userlog.Diffrent.Value.ToString(@"hh\:mm\:ss") : null

                                });
                            }
                     
                        }


                    }
                    else
                    {

                        if (userlog.PunchOut != null)
                        {
                            return Json(new
                            {
                                success = true,
                                punchInTime = userlog.PunchIn.Value.ToString("o"), // ISO 8601 format
                                breakDiffrent = false,
                                status = userlog.Status,
                                punchOutTime = userlog.PunchOut.HasValue ? userlog.PunchOut.Value.ToString("o") : null,
                                workinghours = userlog.Diffrent.HasValue ? userlog.Diffrent.Value.ToString(@"hh\:mm\:ss") : null
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                success = true,
                                punchInTime = userlog.PunchIn.Value.ToString("o"), // ISO 8601 format
                                breakDiffrent = false,
                                status = userlog.Status,
                                punchOutTime = userlog.PunchOut.HasValue ? userlog.PunchOut.Value.ToString("o") : null,
                                workinghours = userlog.Diffrent.HasValue ? userlog.Diffrent.Value.ToString(@"hh\:mm\:ss") : null
                            });
                        }


                    }


                }
                else
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                return Json(new { success = false });
            }
        }


        [HttpPost]
        public JsonResult PunchOut()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Json(new { success = false, message = "User is not logged in." });
            }

            var userlog = db.userlogs
                .Where(ul => ul.UserId == userId && ul.PunchIn.Value.Date == DateTime.Now.Date)
                .FirstOrDefault();

            if (userlog == null)
            {
                return Json(new { success = false, message = "No active session found." });
            }

            userlog.PunchOut = DateTime.Now;

            TimeSpan totalWorked = userlog.PunchOut.Value - userlog.PunchIn.Value;
            TimeSpan breakTime = userlog.BreakDiffrent ?? TimeSpan.Zero;
            TimeSpan actualWorkTime = totalWorked - breakTime;

            // Ensure actual work time is not negative
            if (actualWorkTime < TimeSpan.Zero)
            {
                actualWorkTime = TimeSpan.Zero;
            }

            if (userlog.Diffrent != null)
            {
                userlog.Diffrent += actualWorkTime;
            }
            else
            {
                userlog.Diffrent = actualWorkTime;
            }

            userlog.Status = "PunchOut";

            db.userlogs.Update(userlog);
            db.SaveChanges();

            return Json(new { success = true });
        }


        [HttpPost]
        public JsonResult ContinueWorkingHours()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Json(new { success = false, message = "User is not logged in." });
            }

            var userlog = db.userlogs.Where(ul => ul.UserId == userId && ul.PunchIn.Value.Date == DateTime.Now.Date).FirstOrDefault();

            if (userlog == null)
            {
                return Json(new { success = false, message = "No active session found." });
            }


            //TimeSpan alreadyDiffrenttime;
            //TimeSpan Different;

            //if (userlog.Diffrent != null)
            //{
            //    alreadyDiffrenttime = userlog.Diffrent.Value;
            //    Different = DateTime.Now - userlog.PunchOut.Value;
            //    alreadyDiffrenttime = alreadyDiffrenttime + Different;
            //    userlog.Diffrent = alreadyDiffrenttime;
            //}
            //else
            //{
            //    Different = DateTime.Now - userlog.PunchOut.Value;
            //    userlog.Diffrent = Different;
            //}

            userlog.PunchIn = DateTime.Now;
            userlog.Status = "PunchIn";
            userlog.BreakDiffrent = null;
            db.userlogs.Update(userlog);
            db.SaveChanges();

            return Json(new { success = true });

        }


        public IActionResult ManageCalendar(int year, int month)
        {
            if (year == 0 || month == 0)
            {
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
            }
            var viewModel = new CalendarViewModel
            {
                Year = year,
                Month = month
            };

            var userLogs = db.userlogs
                                   .Where(ul => ul.PunchIn.HasValue && ul.PunchIn.Value.Year == year && ul.PunchIn.Value.Month == month)
                                   .ToList();

            var daysInMonth = DateTime.DaysInMonth(year, month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(year, month, day);
                var userLog = userLogs.FirstOrDefault(ul => ul.PunchIn.HasValue && ul.PunchIn.Value.Date == date);
                var dayViewModel = new CalendarDayViewModel { Date = date, UserLog = userLog };

                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    dayViewModel.Color = "gray";
                }
                else if (userLog == null)
                {
                    dayViewModel.Color = "white";
                }
                else
                {
                    //var totalHours = (userLog.Diffrent ?? TimeSpan.Zero) + (userLog.BreakDiffrent ?? TimeSpan.Zero);

                    //var totalHours = userLog.PunchOut - userLog.PunchIn;

                    //if (userLog.BreakDiffrent != null)
                    //{
                    //    if (userLog.Diffrent != null)
                    //    {
                    //        totalHours = (((userLog.PunchOut - userLog.PunchIn) - userLog.BreakDiffrent ?? TimeSpan.Zero) + userLog.Diffrent);
                    //    }
                    //    else
                    //    {
                    //        totalHours = ((userLog.PunchOut - userLog.PunchIn) - userLog.BreakDiffrent ?? TimeSpan.Zero);
                    //    }
                    //}

                 


                    var totalHours = userLog.Diffrent.HasValue ? userLog.Diffrent.Value : TimeSpan.Zero;
                    
                    

                    if (userLog.PunchIn.Value.Date == DateTime.Now.Date)
                    {
                        dayViewModel.Color = "skyblue";
                    }
                    else if ((totalHours >= TimeSpan.FromHours(4.25)) && (totalHours < TimeSpan.FromHours(8)))
                    {
                        dayViewModel.Color = "orange";
                    }
                    else if (totalHours >= TimeSpan.FromHours(8))
                    {
                        dayViewModel.Color = "green";
                    }
                    else if (totalHours < TimeSpan.FromHours(4.25))
                    {
                        dayViewModel.Color = "red";
                    }
                }

                viewModel.Days.Add(dayViewModel);
            }

            return View(viewModel);
        }


        [HttpGet]
        public JsonResult GetDayDetails(DateTime date)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Json(new { success = false, message = "User is not logged in." });
            }

            var userLog = db.userlogs.FirstOrDefault(ul => ul.UserId == userId && ul.PunchIn.Value.Date == date.Date);

            if (userLog == null)
            {
                return Json(new { success = false, message = "No data found for the selected date." });
            }

            if (userLog.PunchOut == null)
            {
                return Json(new { success = false, message = "No data found for the selected date." });
            }
           

                return Json(new
                {
                    success = true,
                    punchIn = userLog.PunchIn?.ToString("HH:mm") ?? "N/A",
                    punchOut = userLog.PunchOut?.ToString("HH:mm") ?? "N/A",
                    totalHours = userLog.Diffrent.HasValue ? userLog.Diffrent.Value.ToString(@"hh\:mm\:ss") : null,
                    date = userLog.PunchIn.Value.Date.ToString("dd/MM/yyyy")
                });

                //var totalHours = userLog.PunchOut - userLog.PunchIn;

                //if (userLog.BreakDiffrent != null)
                //{
                //    if (userLog.Diffrent != null)
                //    {

                //        totalHours = (((userLog.PunchOut - userLog.PunchIn) - userLog.BreakDiffrent ?? TimeSpan.Zero) + userLog.Diffrent);

                //        return Json(new
                //        {
                //            success = true,
                //            punchIn = userLog.PunchIn?.ToString("HH:mm") ?? "N/A",
                //            punchOut = userLog.PunchOut?.ToString("HH:mm") ?? "N/A",
                //            totalHours = totalHours.Value.ToString(@"hh\:mm\:ss"),
                //            date = userLog.PunchIn.Value.Date.ToString("dd/MM/yyyy")
                //        });
                //    }
                //    else
                //    {
                //        totalHours = ((userLog.PunchOut - userLog.PunchIn) - userLog.BreakDiffrent ?? TimeSpan.Zero);

                //        return Json(new
                //        {
                //            success = true,
                //            punchIn = userLog.PunchIn?.ToString("HH:mm") ?? "N/A",
                //            punchOut = userLog.PunchOut?.ToString("HH:mm") ?? "N/A",
                //            totalHours = totalHours.Value.ToString(@"hh\:mm\:ss"),
                //            date = userLog.PunchIn.Value.Date.ToString("dd/MM/yyyy")
                //        });
                //    }
                //}
                //else
                //{
                //    if (userLog.Diffrent != null)
                //    {
                //        totalHours = ((userLog.PunchOut - userLog.PunchIn) + userLog.Diffrent);

                //        return Json(new
                //        {
                //            success = true,
                //            punchIn = userLog.PunchIn?.ToString("HH:mm") ?? "N/A",
                //            punchOut = userLog.PunchOut?.ToString("HH:mm") ?? "N/A",
                //            totalHours = totalHours.Value.ToString(@"hh\:mm\:ss"),
                //            date = userLog.PunchIn.Value.Date.ToString("dd/MM/yyyy")
                //        });
                //    }
                //    else
                //    {
                //        totalHours = userLog.PunchOut - userLog.PunchIn;

                //        return Json(new
                //        {
                //            success = true,
                //            punchIn = userLog.PunchIn?.ToString("HH:mm") ?? "N/A",
                //            punchOut = userLog.PunchOut?.ToString("HH:mm") ?? "N/A",
                //            totalHours = totalHours.Value.ToString(@"hh\:mm\:ss"),
                //            date = userLog.PunchIn.Value.Date.ToString("dd/MM/yyyy")
                //        });
                //    }
                //}
            


        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
