using feedFBRS.DAO;
using feedFBRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace feedFBRS.Controllers
{
    public class CommentController : Controller
    {

        private NewsDAO newsDAO = new NewsDAO();

        private CommentDAO CommentDAO = new CommentDAO();

        // ADICIONAR COMENTARIOS

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

        // BUSCAR COMENTARIOS

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
                bool alreadyLiked = CommentDAO.HasUserLiked(newsId, comentario.Id, userId);
                comentario.heavusedlogliked = alreadyLiked;
            }
            return Json(new { success = true, data = comments }, JsonRequestBehavior.AllowGet);
        }

        // BUSCAR QUANTIDADE DE COMENTARIOS

        [HttpGet]
        public JsonResult GetCommentCount(string newsId)
        {
            int count = newsDAO.GetCommentCount(newsId);
            return Json(new { success = true, count = count }, JsonRequestBehavior.AllowGet);
        }

        // ADICIONAR/REMOVER LIKE AO COMENTARIO
        public ActionResult LikeComment(string newsId, string commentId, string userId)
        {
            bool alreadyLiked = CommentDAO.HasUserLiked(newsId, commentId, userId);

            if (alreadyLiked)
            {
                CommentDAO.RemoveLike(newsId, commentId, userId); // Se já curtiu, remove a curtida (toggle)
                return Json(new { success = true, liked = false, message = "Curtida removida!" });
            }
            else
            {
                CommentDAO.AddLike(newsId, commentId, userId);
                return Json(new { success = true, liked = true, message = "Curtida registrada!" });
            }
        }

        public ActionResult CheckLikeComment(string newsId, string commentId, string userId)
        {
            bool liked = CommentDAO.HasUserLiked(newsId, commentId, userId);
            return Json(new { liked }, JsonRequestBehavior.AllowGet);
        }
    }
}