function curtirComentario(newsId, commentId) {
    let userId = localStorage.getItem("userId") || "anonimo";

    fetch(`/Comment/LikeComment`, {
        method: "POST",
        headers: { "Content-Type": "application/x-www-form-urlencoded" },
        body: `newsId=${encodeURIComponent(newsId)}&commentId=${encodeURIComponent(commentId)}&userId=${encodeURIComponent(userId)}`
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                atualizarIconeCurtidaComentario(newsId, commentId, data.liked);
            }
        })
        .catch(error => console.error("Erro ao curtir a notícia:", error));
}

function verificarCurtida(newsId, commentId) {
    let userId = localStorage.getItem("userId") || "anonimo";

    fetch(`/Comment/CheckLikeComment?newsId=${encodeURIComponent(newsId)}&commentId=${encodeURIComponent(commentId)}&userId=${encodeURIComponent(userId)}`)
        .then(response => response.json())
        .then(data => {
            atualizarIconeCurtida(commentId, data.liked); // Passando o commentId corretamente
        })
        .catch(error => console.error("Erro ao verificar curtida:", error));
}


function atualizarIconeCurtidaComentario(newsId, commentId, liked) {
    let botao = document.querySelector(`button[data-news-id="${newsId}"][data-comment-id="${commentId}"] svg path`);
    let qtdlikes = document.querySelector(`span[data-news-id="${newsId}"][data-comment-id="${commentId}"] span`)
    if (botao) {
        if (liked) {
            var likes = qtdlikes.innerHTML;
            var atualizado = parseInt(likes) + 1;
            qtdlikes.innerHTML = atualizado;
            botao.setAttribute("fill", "#8371DF"); // Preenche o coração
        } else {
            var likes = qtdlikes.innerHTML;
            var atualizado = parseInt(likes) - 1;
            qtdlikes.innerHTML = atualizado;
            botao.setAttribute("fill", "none"); // Deixa vazio
        }
    }
}