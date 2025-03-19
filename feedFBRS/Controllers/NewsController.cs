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

        private CommentDAO commentDAO = new CommentDAO();

        // 🔹 Exibir todas as notícias
        public ActionResult Index()
        {
            var newsList = newsDAO.LoadNews();
            return View(newsList);
        }


        [HttpPost]
        public ActionResult Create(string title, string content, string author, HttpPostedFileBase[] images)
        {

            // Verifica se há conteúdo de texto ou pelo menos uma imagem válida
            bool hasContent = !string.IsNullOrEmpty(content);
            bool hasImages = images != null && images.Any(img => img != null && img.ContentLength > 0);

            if (hasContent || hasImages) // verificar se apenas um dos dois valores está preenchido (imagem ou texto)
            {
                List<string> imagePaths = new List<string>(); // Lista para armazenar os caminhos das imagens

                // 🔹 Definir a pasta correta para salvar as imagens
                string uploadDir = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // 🔹 Salva todas as imagens corretamente, se houverem
                if (hasImages)
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
        public ActionResult Like(string id, string userId)
        {
            bool alreadyLiked = newsDAO.HasUserLiked(id, userId);

            if (alreadyLiked)
            {
                newsDAO.RemoveLike(id, userId); // Se já curtiu, remove a curtida (toggle)
                return Json(new { success = true, liked = false, message = "Curtida removida!" });
            }
            else
            {
                newsDAO.AddLike(id, userId);
                return Json(new { success = true, liked = true, message = "Curtida registrada!" });
            }
        }

        public ActionResult CheckLike(string id, string userId)
        {
            bool liked = newsDAO.HasUserLiked(id, userId);
            return Json(new { liked }, JsonRequestBehavior.AllowGet);
        }



        // Método para adicionar um comentário
        [HttpPost]
        public JsonResult AddComment(string id, string commentText, string author)
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
                return Json(new { success = true, message = "Comentário adicionado com sucesso!" });
            }
            return Json(new { success = false, message = "Comentário inválido." });
        }

        [HttpGet]
        public JsonResult GetComments(string newsId, string userId)
        {
            var comments = newsDAO.GetComments(newsId);

            if (comments == null || comments.Count == 0)
            {
                return Json(new { success = false, message = "Nenhum comentário encontrado." }, JsonRequestBehavior.AllowGet);
            }
            foreach (var comentario in comments)
            {
                bool alreadyLiked = commentDAO.HasUserLiked(newsId, comentario.Id, userId);
                comentario.heavusedlogliked = alreadyLiked;
            }
            return Json(new { success = true, data = comments }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCommentCount(string newsId)
        {
            int count = newsDAO.GetCommentCount(newsId);
            return Json(new { success = true, count = count }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult LikeComment(string newsId, string commentId, string userId)
        {
            newsDAO.AddLikeToComment(newsId, commentId, userId);
            return Json(new { success = true, message = "Comentário curtido com sucesso!" });
        }

       

    }
}