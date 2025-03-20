function curtirNoticia(newsId) {
    let userId = localStorage.getItem("userId") || "anonimo";

    fetch(`/News/Like`, {
        method: "POST",
        headers: { "Content-Type": "application/x-www-form-urlencoded" },
        body: `id=${encodeURIComponent(newsId)}&userId=${encodeURIComponent(userId)}`
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                atualizarIconeCurtida(newsId, data.liked);
            }
        })
        .catch(error => console.error("Erro ao curtir a notícia:", error));
}

function verificarCurtidaFeed(newsId) {
    let userId = localStorage.getItem("userId") || "anonimo";

    fetch(`/News/CheckLike?id=${encodeURIComponent(newsId)}&userId=${encodeURIComponent(userId)}`)
        .then(response => response.json())
        .then(data => {
            atualizarIconeCurtida(newsId, data.liked);
        })
        .catch(error => console.error("Erro ao verificar curtida:", error));
}

function atualizarIconeCurtida(newsId, liked) {
    let botao = document.querySelector(`button[data-news-id="${newsId}"] svg path`);
    if (botao) {
        if (liked) {
            botao.setAttribute("fill", "#8371DF"); // Preenche o coração
        } else {
            botao.setAttribute("fill", "none"); // Deixa vazio
        }
    }
}