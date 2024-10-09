using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserLogManagement.Models
{
    public class userlog
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime? PunchIn { get; set; }

        public DateTime? PunchOut { get; set; }

        public DateTime? BreakStart { get; set; }

        public DateTime? BreakEnd { get; set; }

        public TimeSpan? Diffrent { get; set; }

        public TimeSpan? BreakDiffrent { get; set; }

        public string? Status { get; set; }

        [ForeignKey("UserId")]
        public virtual Applicationuser Applicationuser { get; set; }
    }
}
