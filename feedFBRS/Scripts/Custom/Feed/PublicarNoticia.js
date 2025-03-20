//Publicar a noticia no feed
document.getElementById("newsForm").addEventListener("submit", function (event) {
    event.preventDefault(); // Impede o reload da página

    let formData = new FormData(this);

    // Adiciona imagens ao FormData
    imagens.forEach((file) => {
        formData.append(`images`, file); // Adiciona cada imagem ao FormData
    });

    fetch("/News/Create", {
        method: "POST",
        body: formData
    }).then(response => {
        if (response.ok) {
            alert("Notícia publicada com sucesso!");
            location.reload(); // Recarrega o feed para exibir a nova notícia
        } else {
            alert("Erro ao publicar notícia!");
        }
    }).catch(error => {
        console.error("Erro:", error);
        alert("Erro ao publicar notícia!");
    });
});

function removerImagem(index) {
    imagens.splice(index, 1);
    atualizarPreview();

    // Atualizar o input file para refletir as imagens restantes
    let dataTransfer = new DataTransfer();
    imagens.forEach(file => dataTransfer.items.add(file));
    inputFile.files = dataTransfer.files;
}

publicarBtn.addEventListener("click", function () {
    publicacoes.innerHTML = "<h4>Publicação</h4>";

    imagens.forEach(file => {
        const reader = new FileReader();
        reader.onload = function (e) {
            let imgElement = document.createElement("img");
            imgElement.src = e.target.result;
            imgElement.width = 150;
            imgElement.style.margin = "5px";
            publicacoes.appendChild(imgElement);
        };
        reader.readAsDataURL(file);
    });

    imagens = [];
    atualizarPreview();

    // Fechar a modal corretamente se estiver usando Bootstrap
    let modalElement = document.getElementById("novaPublicacaoModal");
    if (modalElement && bootstrap.Modal.getInstance(modalElement)) {
        let modal = bootstrap.Modal.getInstance(modalElement);
        modal.hide();
    }
});