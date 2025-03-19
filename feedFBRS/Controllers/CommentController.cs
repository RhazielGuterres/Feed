using feedFBRS.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace feedFBRS.Controllers
{
    public class CommentController : Controller
    {

        private CommentDAO CommentDAO = new CommentDAO();
        // Método para curtir um comentario
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