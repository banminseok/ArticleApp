﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string? Title { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        public string? Genre { get; set; } = string.Empty;

        public decimal Price { get; set; }

    }
}
