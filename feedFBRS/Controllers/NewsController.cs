using feedFBRS.DAO;
using feedFBRS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace feedFBRS.Controllers
{
    public class NewsController : Controller
    {
        private NewsDAO newsDAO = new NewsDAO();

        // 🔹 Exibir todas as notícias
        public ActionResult Index()
        {
            var newsList = newsDAO.LoadNews();
            return View(newsList);
        }


        [HttpPost]
        public ActionResult Create(string title, string content, string author, HttpPostedFileBase[] images)
        {
            if (!string.IsNullOrEmpty(content)) // Apenas 'content' é obrigatório
            {
                List<string> imagePaths = new List<string>(); // Lista para armazenar os caminhos das imagens

                // 🔹 Definir a pasta correta para salvar as imagens
                string uploadDir = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // 🔹 Salva todas as imagens corretamente, se houverem
                if (images != null)
                {
                    foreach (var image in images)
                    {
                        if (image != null && image.ContentLength > 0)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                            string filePath = Path.Combine(uploadDir, fileName);
                            image.SaveAs(filePath);
                            imagePaths.Add("/Uploads/" + fileName); // Caminho relativo para exibição
                        }
                    }
                }

                var news = new News
                {
                    Id = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    Content = content,
                    Author = string.IsNullOrEmpty(author) ? "Anônimo" : author,
                    Approved = false,
                    Likes = 0,
                    Comments = new List<Comment>(),
                    ImageUrls = imagePaths // Agora é uma lista de imagens
                };

                newsDAO.AddNews(news);
            }

            return RedirectToAction("Index");
        }



        public JsonResult GetNews()
        {
            var newsList = newsDAO.LoadNews(); // Busca as notícias no banco ou JSON

            if (newsList == null || !newsList.Any())
            {
                return Json(new { success = false, message = "Nenhuma notícia encontrada." }, JsonRequestBehavior.AllowGet);
            }

            var orderedNewsList = newsList.OrderByDescending(n => n.Id).ToList();

            return Json(orderedNewsList, JsonRequestBehavior.AllowGet);
        }




        // 🔹 Aprovar Notícia (somente GGP ou Comunicação)
        public ActionResult Approve(string id)
        {
            var newsList = newsDAO.LoadNews();
            var news = newsList.Find(n => n.Id == id);
            if (news != null)
            {
                news.Approved = true;
                newsDAO.SaveNews(newsList);
            }
            return RedirectToAction("Index");
        }

        // Método para curtir uma notícia
        public ActionResult Like(string id)
        {
            newsDAO.AddLike(id);
            return RedirectToAction("Index");
        }

        // Método para adicionar um comentário
        [HttpPost]
        public ActionResult AddComment(string id, string commentText, string author)
        {
            if (!string.IsNullOrEmpty(commentText))
            {
                var comment = new Comment
                {
                    Id = Guid.NewGuid().ToString(),
                    NewsId = id,
                    Content = commentText,
                    Author = author,
                    Timestamp = DateTime.Now
                };
                newsDAO.AddComment(id, comment);
            }
            return RedirectToAction("Index");
        }

    }
}