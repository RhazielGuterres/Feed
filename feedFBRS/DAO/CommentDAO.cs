using feedFBRS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace feedFBRS.DAO
{
	public class CommentDAO
	{

        private readonly string filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/news.json");


        public List<News> LoadNews()
        {
            if (!File.Exists(filePath)) return new List<News>();
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<News>>(json) ?? new List<News>();
        }

        public void AddLike(string newsId, string commentId, string userId)
        {
            var newsList = LoadNews();
            var news = newsList.Find(n => n.Id == newsId);
            if (news != null)
            {
                var comment = news.Comments.Find(c => c.Id == commentId);
                if (comment != null)
                {
                    if (!comment.UsersWhoLiked.Contains(userId)) // Verifica se o usuário já curtiu
                    {
                        comment.UsersWhoLiked.Add(userId);
                        comment.Likes++;
                        SaveNews(newsList);
                    }
                }
            }
        }



        public bool HasUserLiked(string newsId, string commentId, string userId)
        {
            var newsList = LoadNews();
            var news = newsList.Find(n => n.Id == newsId);

            if (news != null)

            {
                var comment = news.Comments.Find(c => c.Id == commentId);
                if (comment != null)
                {
                    return comment.UsersWhoLiked.Contains(userId);
                }         
            }

            return false;
        }

        public void RemoveLike(string newsId, string commentId, string userId)
        {
            var newsList = LoadNews();
            var news = newsList.Find(n => n.Id == newsId);

            if (news != null)
            {
                var comment = news.Comments.Find(c => c.Id == commentId);
                if (comment != null)
                {
                    if (comment.UsersWhoLiked.Contains(userId)) // Verifica se o usuário curtiu
                    {
                        comment.UsersWhoLiked.Remove(userId);
                        comment.Likes = Math.Max(0, comment.Likes - 1); // Garante que não fique negativo
                        SaveNews(newsList);
                    }
                }
            }
        }

        public void SaveNews(List<News> newsList)
        {
            var json = JsonConvert.SerializeObject(newsList, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }



    }
}