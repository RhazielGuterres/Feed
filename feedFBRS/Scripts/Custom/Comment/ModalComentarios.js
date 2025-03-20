function abrirModalComentarios(postId) {
    let modal = document.getElementById("modalComentarios");
    modal.dataset.postId = postId; // Armazena o ID do post na modal
    carregarComentarios(postId);
    new bootstrap.Modal(modal).show();
}


function carregarComentarios(postId) {
    let lista = document.getElementById("listaComentarios");
    let userId = localStorage.getItem("userId") || "anonimo";
    // lista.innerHTML = "<p class='text-muted'>Carregando comentários...</p>";

    fetch(`/Comment/GetComments?newsId=${postId}&userId=${userId}`)
        .then(response => response.json())
        .then(response => {
            console.log("Comentários recebidos:", response);

            if (!response.success || response.data.length === 0) {
                lista.innerHTML = "<p class='text-muted'>Nenhum comentário ainda.</p>";
                return;
            }

            lista.innerHTML = ""; // Limpa antes de adicionar os novos

            response.data.forEach((c, index) => {
                lista.innerHTML += `
                                    <div class="comentario">
                                        <div class="comentario-conteudo" style="display: flex;flex-direction: column;gap: 10px;">
                                            <div style="display: flex;align-items: center;gap: 5px">
                                            <span class="user-avatar publicacao" style="width: 27px;height: 27px;margin-right: 5px;">${c.Author.charAt(0)}</span>
                                            <span class="comentario-usuario">${c.Author}</span>
                                            <span style="font-size: 12px; color: gray;">${formatarData(c.Timestamp)}</span>
                                            </div>
                                            <p class="comentario-texto" style="margin-bottom: 0rem;word-wrap: break-word;">${c.Content}</p>
                                            <div class="comentario-acoes">
                                                <button class="tirarlinha" style="padding: 0;"
                                                        data-news-id="${postId}"
                                                        data-comment-id="${c.Id}"
                                                        onclick="curtirComentario('${postId}', '${c.Id}')">
                                                    <svg class="heart-icon tirarcor" width="25" height="25" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                                                        <path fill="${c.heavusedlogliked ? '#8371DF' : 'none'}" stroke="#8371DF" stroke-width="2" d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z"></path>
                                                    </svg>
                                                </button>
                                                <span style="font-size: 15px;color: rgb(131, 113, 223);" data-news-id="${postId}" data-comment-id="${c.Id}" id="curtidas-${postId}-${c.Id}" class="comentario-likes">
                                                    <span>${c.Likes}</span> curtidas
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                `;
            });
        })
        .catch(error => console.error("Erro ao carregar comentários:", error));
}



function formatarData(timestamp) {
    if (!timestamp) return "Data inválida";

    // Extrai o número dentro de /Date(XXXXXXXXXXXXX)/
    let match = timestamp.match(/\d+/);
    if (!match) return "Data inválida";

    let data = new Date(parseInt(match[0]));

    if (isNaN(data.getTime())) {
        return "Data inválida";
    }

    return data.toLocaleString("pt-BR", {
        day: "2-digit",
        month: "2-digit",
        year: "numeric",
        hour: "2-digit",
        minute: "2-digit",
        second: "2-digit"
    });
}

function atualizarContadorComentarios(newsId) {
    fetch(`/Comment/GetCommentCount?newsId=${newsId}`)
        .then(response => response.json())
        .then(countData => {
            let comentariosDiv = document.getElementById(`comentarios-${newsId}`);

            if (countData.count === 0) {
                comentariosDiv.style.display = "none"; // Esconde se não houver comentários
            } else {
                let textoComentario = countData.count === 1 ? "1 comentário" : `${countData.count} comentários`;

                comentariosDiv.innerHTML = `
                                    ${textoComentario},
                                    <span onclick="abrirModalComentarios('${newsId}')"
                                          style="color: #4d4d4d; cursor: pointer; text-decoration: none;">
                                        <i>ver todos...</i>
                                    </span>
                                `;
                comentariosDiv.style.display = "block"; // Mostra se houver comentários
            }
        })
        .catch(error => {
            console.error("Erro ao buscar quantidade de comentários:", error);
            document.getElementById(`comentarios-${newsId}`).innerHTML = "Erro ao carregar comentários.";
        });
}

