
var tabuleiroDiv;
var jogoId;

//Função responsável por atualizar uma posição específica do tabuleiro no cliente.
function AtualizarPosicao(coluna, linha, valor)
{
    var posicaoDiv=document.querySelector("#Tabuleiro")
        .querySelector("#linha-"+linha)
        .querySelector("#posCol-"+coluna);

    posicaoDiv.classList.remove('Jogador1');
    posicaoDiv.classList.remove('Jogador2');
    if(valor==1)
    {
        posicaoDiv.classList.add('Jogador1');
    } else if(valor==2)
    {
        posicaoDiv.classList.add('Jogador2');
    }
}

//Esta função cria uma linha na Div do Tabuleiro.
function CriarLinha(colunas)
{
    var linha=document.createElement('div');
    linha.classList.add('row');
    for(var i=0; i<colunas; i++)
    {
        var posicaoDiv=document.createElement('div');
        posicaoDiv.id='posCol-'+i;
        posicaoDiv.classList.add('square');
        linha.appendChild(posicaoDiv);
        //posicaoDiv.click = function () { jogarServidor(jogoId, this.id.substr(7)); };
    }
    return linha;
}



//Esta função irá montar o tabuleiro, invocando a função criar 
//linhas com o número de linhas existete no tabuleiro e 
// pasando o número de colunas como parámetro.
function CriarTabuleiro(colunas, linhas)
{
    //Obtem a Div do Tabuleiro.CSHTML na qual irá ser montado o tabuleiro.
    tabuleiroDiv=document.getElementById("Tabuleiro");
    //Remove quaisquer divs existentes anteriormente.
    //útil para apagar um tabuleiro existente.
    tabuleiroDiv.querySelectorAll('*').forEach(n => n.remove());
    //Criando as linhas e adicionando a DIV do tabuleiro.
    for(var i=linhas-1; i>=0; i--)
    {
        //Cria a linha
        var linhaDiv=CriarLinha(colunas);
        //Nomeia com o número da linha.
        linhaDiv.id='linha-'+i;
        //Adiciona na DIV.
        tabuleiroDiv.appendChild(linhaDiv);
    }
}

///Função que faz a chamada no servidor da jogada executada no cliente
///E depois atualiza o Tabuleiro.
function jogarServidor(jogoId, posicao)
{
    //Cria uma requisição HTTP.
    var xhttp=new XMLHttpRequest();
    //Define qual o tipo de resposta, no caso um objeto JSON.
    xhttp.responseType='json';
    //Monta a URL que será invocada.
    var URLObterJogo="/api/Jogo/Jogar?JogoId="+jogoId
        +"&Pos="+posicao;
    //Função que será invocada em uma modificação
    //no estado da chamada xhttp.

    tabuleiroDiv.classList.add("disabled");
    xhttp.onreadystatechange=function()
    {
        //estado da chamada xhttp. 4 Significa done.
        if(this.readyState==4)
        {
            tabuleiroDiv.classList.remove("disabled");
            //Se a resposta for nula significa que algo falhou.
            if(this.response==null&&
                this.responseURL!="")
            {
                window.location.replace(this.responseURL);
            }
            //Se o estado é 200 e a resposta não é nula montar o tabuleiro.
            if(this.status==200)
            {
                MontarTabuleiro(this.response);
            }
        }
    };
    //Prepara uma chamada GET no Servidor.
    xhttp.open("GET", URLObterJogo, true);
    //Envia a chamada.
    xhttp.send();
}


function obterJogoServidor(id)
{
    //Faz uma chamada no servidor.
    var xhttp=new XMLHttpRequest();
    //Define qual o tipo de resposta, no caso um objeto JSON.
    xhttp.responseType='json'
    //Monta a URL que será invocada.
    var URLObterJogo="/api/Jogo/Obter/"+id
    //Função que será invocada em uma modificação
    //no estado da chamada xhttp.
    xhttp.onreadystatechange=function()
    {
        if(this.readyState==4)
        {
            if(this.response==null&&
                this.responseURL!="")
            {
                window.location.replace(this.responseURL);
            }
            if(this.status==200)
            {
                MontarTabuleiro(this.response);
            }
        }
    };
    //Prepara uma chamada GET no Servidor.
    xhttp.open("GET", URLObterJogo, true);
    //Envia a chamada.
    xhttp.send();
}


//Função que monta um tabuleiro.
function MontarTabuleiro(Tabuleiro)
{
    tabuleiroObj=Tabuleiro;
    //Tamanho de colunas e linhas de acordo com o tamanho do tabuleiro.
    var TamanhoColunas=Tabuleiro.representacaoTabuleiro.length,
        TamanhoLinhas=Tabuleiro.representacaoTabuleiro[0].length;
    //Cria o tabuleiro com o número de linhas e colunas adequados.
    CriarTabuleiro(TamanhoColunas, TamanhoLinhas);
    //Verifica cada uma das posições se está limpa, com uma peça do jogador 1 ou 2 e
    //modifica a apresentação visual de acordo com o objeto Tabuleiro.
    for(coluna=0; coluna<TamanhoColunas; coluna++)
    {
        for(linha=0; linha<TamanhoLinhas; linha++)
        {
            AtualizarPosicao(coluna, linha, Tabuleiro.representacaoTabuleiro[coluna][linha]);
        }
    }
    ///Adiciona um evento para no caso de ser clicado um dos espaços realizar uma jogada.    
    for(coluna=0; coluna<TamanhoColunas; coluna++)
    {
        $("#posCol-"+coluna).click(function()
        {
            jogarServidor(jogoId, this.id.substr(7));
        });
    }



}
/*
$(document).ready(function () {
    
});  
*/