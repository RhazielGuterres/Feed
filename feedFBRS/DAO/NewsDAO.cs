using feedFBRS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace feedFBRS.DAO
{
    public class NewsDAO
    {
        private readonly string filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/news.json");


        public List<News> LoadNews()
        {
            if (!File.Exists(filePath)) return new List<News>();
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<News>>(json) ?? new List<News>();
        }

        public void AddLike(string newsId, string userId)
        {
            var newsList = LoadNews();
            var news = newsList.Find(n => n.Id == newsId);
            if (news != null)
            {
                if (!news.UsersWhoLiked.Contains(userId)) // Verifica se o usuário já curtiu
                {
                    news.UsersWhoLiked.Add(userId);
                    news.Likes++;
                    SaveNews(newsList);
                }
            }
        }

        public bool HasUserLiked(string newsId, string userId)
        {
            var newsList = LoadNews();
            var news = newsList.Find(n => n.Id == newsId);

            if (news != null)
            {
                return news.UsersWhoLiked.Contains(userId);
            }

            return false;
        }

        public void RemoveLike(string newsId, string userId)
        {
            var newsList = LoadNews();
            var news = newsList.Find(n => n.Id == newsId);

            if (news != null)
            {
                if (news.UsersWhoLiked.Contains(userId)) // Verifica se o usuário curtiu
                {
                    news.UsersWhoLiked.Remove(userId);
                    news.Likes = Math.Max(0, news.Likes - 1); // Garante que não fique negativo
                    SaveNews(newsList);
                }
            }
        }



        public void AddComment(string newsId, Comment comment)
        {
            var newsList = LoadNews();
            var news = newsList.Find(n => n.Id == newsId);
            if (news != null)
            {
                news.Comments.Add(comment);
                SaveNews(newsList);
            }
        }


        public void SaveNews(List<News> newsList)
        {
            var json = JsonConvert.SerializeObject(newsList, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void AddNews(News news)
        {
            var newsList = LoadNews();
            newsList.Add(news);
            SaveNews(newsList);
        }

        public List<Comment> GetComments(string newsId)
        {
            var newsList = LoadNews();
            var news = newsList.Find(n => n.Id == newsId);
            return news?.Comments ?? new List<Comment>(); // Retorna os comentários ou uma lista vazia
        }

        public int GetCommentCount(string newsId)
        {
            var newsList = LoadNews();
            var news = newsList.Find(n => n.Id == newsId);
            return news?.Comments.Count ?? 0; // Retorna o número de comentários
        }





        public void AddLikeToComment(string newsId, string commentId, string userId)
        {
            var newsList = LoadNews();
            var news = newsList.Find(n => n.Id == newsId);

            if (news != null)
            {
                var comment = news.Comments.Find(c => c.Id == commentId);
                if (comment != null)
                {
                    if (comment.UsersWhoLiked.Contains(userId)) // Se já curtiu, remove
                    {
                        comment.UsersWhoLiked.Remove(userId);
                        comment.Likes--;
                    }
                    else // Se não curtiu, adiciona
                    {
                        comment.UsersWhoLiked.Add(userId);
                        comment.Likes++;
                    }
                    SaveNews(newsList);
                }
            }
        }




    }
}