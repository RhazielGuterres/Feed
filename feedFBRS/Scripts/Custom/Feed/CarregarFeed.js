document.addEventListener("DOMContentLoaded", function () {
    carregarFeed(); // Chama a função ao carregar a página

    function carregarFeed() {
        fetch("/News/GetNews") // Faz uma requisição para buscar as notícias
            .then(response => response.json())
            .then(data => {
                let feedContainer = document.getElementById("feedNoticias");
                feedContainer.innerHTML = ""; // Limpa o feed antes de adicionar novas notícias

                data.forEach(news => {
                    console.log("Notícia recebida:", news);
                    let noticiaHTML = `
                                                        <div class="noticia">
                                                            <!-- Perfil do Usuário -->
                                                            <div class="user-profile">
                                                                <div class="user-avatar publicacao"></div>
                                                                <div class="user-info alinhartexto-publicacao">
                                                                    <h5 class="m-0 tamanhonomeusuario">${news.Author}</h5>
                                                                </div>
                                                                <div class="horario">
                                                                    ${formatarData(news.Id)}
                                                                </div>
                                                            </div>

                                                            <!-- Conteúdo da notícia -->
                                                            <div class="card-body" style="word-wrap: break-word; margin: 22px;">
                                                                <p class="card-text">${news.Content}</p>
                                                            </div>

                                                            <div id="carousel-${news.Id}" class="carousel slide">
                                                            <div class="carousel-inner">
                                                                ${news.ImageUrls.map((url, index) => `
                                                                    <div class="carousel-item ${index === 0 ? 'active' : ''}">
                                                                        <img src="${url}" class="d-block w-100" alt="Imagem do post">
                                                                    </div>
                                                                `).join('')}
                                                            </div>
                                                            <button class="carousel-control-prev" type="button" data-bs-target="#carousel-${news.Id}" data-bs-slide="prev">
                                                                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                                                <span class="visually-hidden">Previous</span>
                                                            </button>
                                                            <button class="carousel-control-next" type="button" data-bs-target="#carousel-${news.Id}" data-bs-slide="next">
                                                                <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                                                <span class="visually-hidden">Next</span>
                                                            </button>
                                                            </div>
                                                            <!-- Ações -->
                                                            <div class="card-footer d-flex justify-content-between align-items-center postiondivi" style="display: flex; margin-right: 4%; margin-left: 5%;">
                                                    <div>
                                                        <button class="heart-btn tirarlinha" style="padding: 0px;" onclick="curtirNoticia('${news.Id}')" data-news-id="${news.Id}">
                                                            <svg class="heart-icon tirarcor" width="27" height="27" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                                                                <path fill="none" stroke="#8371DF" stroke-width="2"
                                                                    d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z" />
                                                            </svg>
                                                        </button>

                                                    </div>
                                                    <div>
                                                        <button class="tirarlinha" sytle=""
                                                            onclick="abrirModalComentarios('${news.Id}')">
                                                            <svg aria-label="Comentar" fill="currentColor" height="24" role="img"
                                                                viewBox="0 0 24 24" width="24">
                                                                <title>Comentar</title>
                                                                <path d="M20.656 17.008a9.993 9.993 0 1 0-3.59 3.615L22 22Z"
                                                                    fill="none" stroke="gray" stroke-linejoin="round" stroke-width="2">
                                                                </path>
                                                            </svg>
                                                        </button>
                                                    </div>
                                                    <div class="comentario-box">
                                                        <textarea id="novoComentario_${news.Id}" class="comentario-input" placeholder="Comente algo aqui..."
                                                            rows="1"></textarea>
                                                    </div>
                                                    <div style="padding: 6px;">
                                                        <button class="icon-container" style="border: none; outline: none; padding: 2px"
                                                            data-news-id="${news.Id}"
                                                            data-textarea-id="novoComentario_${news.Id}"
                                                            onclick="adicionarComentario(this)">
                                                            <svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" viewBox="0 0 24 24"
                                                                fill="white">
                                                                <path d="M11.5003 12H5.41872M5.24634 12.7972L4.24158 15.7986C3.69128 17.4424 3.41613 18.2643 3.61359 18.7704C3.78506 19.21 4.15335 19.5432 4.6078 19.6701C5.13111 19.8161 5.92151 19.4604 7.50231 18.7491L17.6367 14.1886C19.1797 13.4942 19.9512 13.1471 20.1896 12.6648C20.3968 12.2458 20.3968 11.7541 20.1896 11.3351C19.9512 10.8529 19.1797 10.5057 17.6367 9.81135L7.48483 5.24303C5.90879 4.53382 5.12078 4.17921 4.59799 4.32468C4.14397 4.45101 3.77572 4.78336 3.60365 5.22209C3.40551 5.72728 3.67772 6.54741 4.22215 8.18767L5.24829 11.2793C5.34179 11.561 5.38855 11.7019 5.407 11.8459C5.42338 11.9738 5.42321 12.1032 5.40651 12.231C5.38768 12.375 5.34057 12.5157 5.24634 12.7972Z"
                                                                    stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
                                                            </svg>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div>
                                                    <div id="comentarios-${news.Id}" class="positioncomentfeed"></div>
                                                </div>
                                                        </div>
                                                    `;

                    // Adiciona a notícia ao feed
                    feedContainer.innerHTML += noticiaHTML;

                    // ✅ Agora verificamos se o usuário já curtiu a notícia ao carregar a página
                    verificarCurtidaFeed(news.Id);

                    // Buscar e atualizar o número de comentários
                    fetch(`/Comment/GetCommentCount?newsId=${news.Id}`)
                        .then(response => response.json())
                        .then(countData => {
                            let comentariosDiv = document.getElementById(`comentarios-${news.Id}`);

                            if (countData.count === 0) {
                                comentariosDiv.style.display = "none"; // Esconde se não houver comentários
                            } else {
                                let textoComentario = countData.count === 1 ? "1 comentário" : `${countData.count} comentários`;

                                comentariosDiv.innerHTML = `
                                                        ${textoComentario},
                                                        <span onclick="abrirModalComentarios('${news.Id}')"
                                                              style="color: #4d4d4d; cursor: pointer; text-decoration: none;">
                                                            <i>ver todos...</i>
                                                        </span>
                                                    `;
                                comentariosDiv.style.display = "block"; // Garante que aparece se houver comentários
                            }
                        })
                        .catch(error => {
                            console.error("Erro ao buscar quantidade de comentários:", error);
                            document.getElementById(`comentarios-${news.Id}`).innerHTML = "Erro ao carregar comentários.";
                        });


                });
            })
            .catch(error => console.error("Erro ao carregar o feed:", error));
    }

    function formatarData(id) {
        let ano = id.substring(0, 4);
        let mes = id.substring(4, 6);
        let dia = id.substring(6, 8);
        let hora = id.substring(8, 10);
        let minuto = id.substring(10, 12);
        return `${dia}/${mes}/${ano} ${hora}:${minuto}`;
    }
    
});


