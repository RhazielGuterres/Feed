using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace feedFBRS.Models
{
    public class News
    {
        public string Id { get; set; } // ID baseado na data
        public string Content { get; set; }
        public string Author { get; set; }
        public bool Approved { get; set; } // Indica se GGP ou Comunicação aprovaram
        public int Likes { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}