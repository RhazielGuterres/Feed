using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace feedFBRS.Models
{
    public class Comment
    {
        public string Id { get; set; } // ID do comentário
        public string NewsId { get; set; } // Associado à notícia
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime Timestamp { get; set; }
        public int Likes { get; set; } = 0; // Número de curtidas no comentário
        public HashSet<string> UsersWhoLiked { get; set; } = new HashSet<string>(); // Lista de usuários que curtiram


    }
}