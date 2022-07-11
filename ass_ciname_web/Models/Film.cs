using System;
using System.Collections.Generic;

#nullable disable

namespace ass_ciname_web.Models
{
    public partial class Film
    {
        public int FilmId { get; set; }
        public int GenreId { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string CountryCode { get; set; }
    }
}
