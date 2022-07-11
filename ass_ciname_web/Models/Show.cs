using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ass_ciname_web.Models
{
    public partial class Show
    {
        [Display(Name = "Id:")]
        public int ShowId { get; set; }

        [Display(Name = "Room:")]
        public int RoomId { get; set; }

        public int FilmId { get; set; }
        [Display(Name = "Show date:")]
        public DateTime? ShowDate { get; set; }
        public decimal? Price { get; set; }
        public bool? Status { get; set; }
        public int? Slot { get; set; }

        public virtual Film Film { get; set; }
        public virtual Room Room { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
