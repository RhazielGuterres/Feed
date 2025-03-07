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

        public void AddLike(string newsId)
        {
            var newsList = LoadNews();
            var news = newsList.Find(n => n.Id == newsId);
            if (news != null)
            {
                news.Likes++;
                SaveNews(newsList);
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
    }
}