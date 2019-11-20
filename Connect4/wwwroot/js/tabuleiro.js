var tabuleiroDiv;
function AtualizarPosicao(coluna, linha, valor) {
    var posicaoDiv = document.querySelector("#Tabuleiro")
        .querySelector("#linha-" + linha)
        .querySelector("#posCol-" + coluna);

    posicaoDiv.classList.remove('Jogador1');
    posicaoDiv.classList.remove('Jogador2');
    if (valor == 1) {
        posicaoDiv.classList.add('Jogador1');
    } else if (valor == 2) {
        posicaoDiv.classList.add('Jogador2');
    }
}
function CriarLinha(colunas) {
    var linha = document.createElement('div');
    linha.classList.add('row');
    for (var i = 0; i < colunas; i++) {
        var posicaoDiv = document.createElement('div');
        posicaoDiv.id = 'posCol-' + i;
        posicaoDiv.classList.add('square');
        linha.appendChild(posicaoDiv);
    }

    return linha;
}

function CriarTabuleiro(colunas, linhas) {
    tabuleiroDiv = document.getElementById("Tabuleiro");
    tabuleiroDiv.querySelectorAll('*').forEach(n => n.remove());
    for (var i = linhas - 1; i >= 0; i--) {
        var linhaDiv = CriarLinha(colunas);
        linhaDiv.id = 'linha-' + i;
        tabuleiroDiv.appendChild(linhaDiv);
    }
}



function obterJogoServidor() {
    var xhttp = new XMLHttpRequest();
    xhttp.responseType = 'json'
    var URLObterJogo = "api/Jogo/Obter"
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4) {
            if (this.response == null &&
                this.responseURL != "") {
                window.location.replace(this.responseURL);
            }
            if (this.status == 200) {
                MontarTabuleiro(this.response);
            }
        }
    };
    //Prepara uma chamada GET no Servidor.
    xhttp.open("GET", URLObterJogo, true);
    //Envia a chamada.
    xhttp.send();
}

function MontarTabuleiro(Tabuleiro) {
    var TamanhoColunas = Tabuleiro.representacaoTabuleiro.length,
        TamanhoLinhas = Tabuleiro.representacaoTabuleiro[0].length;
    CriarTabuleiro(TamanhoColunas, TamanhoLinhas);
    for (coluna = 0; coluna < TamanhoColunas; coluna++) {
        for (linha = 0; linha < TamanhoLinhas; linha++) {
            AtualizarPosicao(coluna, linha, Tabuleiro.representacaoTabuleiro[coluna][linha]);
        }
    }
}