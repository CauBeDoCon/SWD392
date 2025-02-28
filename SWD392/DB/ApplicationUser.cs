using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392.DB
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public DateTime? Birthday { get; set; }
        public int? CartId { get; set; }
        public int? WalletId { get; set; }

        [ForeignKey("CartId")]
        public Cart Cart { get; set; }

        [ForeignKey("WalletId")]
        public Wallet Wallet { get; set; }

        public ICollection<Blog> Blogs{ get; set; } = new List<Blog>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public ICollection<ResultQuiz> ResultQuizzes { get; set; } = new List<ResultQuiz>();

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public ICollection<TimeFrame> TimeFrames { get; set; } = new List<TimeFrame>();

    }
}
