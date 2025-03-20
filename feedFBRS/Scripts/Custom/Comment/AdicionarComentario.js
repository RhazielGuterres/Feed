function adicionarComentario(button) {
    let postId;
    let textareaId;

    let modal = document.getElementById("modalComentarios");

    if (modal && modal.classList.contains("show")) {
        // Se a modal está aberta, pega o ID do post dela
        postId = modal.dataset.postId;
        textareaId = "novoComentarioModal";
    } else {
        // Se for um comentário no feed, pega o ID do post do botão clicado
        postId = button.dataset.newsId;
        textareaId = button.dataset.textareaId;
    }

    let texto = document.getElementById(textareaId).value.trim();
    let author = "Usuário Exemplo";

    if (texto === "") {
        alert("Digite um comentário antes de enviar!");
        return;
    }

    fetch("/Comment/AddComment", {
        method: "POST",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded"
        },
        body: `id=${encodeURIComponent(postId)}&commentText=${encodeURIComponent(texto)}&author=${encodeURIComponent(author)}`
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                document.getElementById(textareaId).value = ""; // Limpa o campo
                carregarComentarios(postId); // Atualiza os comentários na modal
                atualizarContadorComentarios(postId); // Atualiza a contagem de comentários no feed
            } else {
                alert("Erro ao adicionar comentário: " + data.message);
            }
        })
        .catch(error => console.error("Erro ao adicionar comentário:", error));


}
