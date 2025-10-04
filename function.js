function getAluno() {
    fetch('https://localhost:7139/Aluno')

        .then(response => response.json())
        .then(data => {
            const alunoTable = document.getElementById('alunoTable');
            const tbody = alunoTable.querySelector('tbody');
            tbody.innerHTML = '';

            data.forEach(aluno => {
                const row = document.createElement('tr');
                const idCell = document.createElement('td');
                idCell.textContent = aluno.id;
                row.appendChild(idCell);

                const nomeCell = document.createElement('td');
                nomeCell.textContent = aluno.nome;
                row.appendChild(nomeCell);

                const sobrenomeCell = document.createElement('td');
                sobrenomeCell.textContent = aluno.sobrenome;
                row.appendChild(sobrenomeCell);

                const telCell = document.createElement('td');
                telCell.textContent = aluno.telefone;
                row.appendChild(telCell);

                const rmCell = document.createElement('td');
                rmCell.textContent = aluno.rm;
                row.appendChild(rmCell);

                const curCell = document.createElement('td');
                curCell.textContent = aluno.curso;
                row.appendChild(curCell);

                const statusCell = document.createElement('td');
                statusCell.textContent = aluno.status;
                row.appendChild(statusCell);


                tbody.appendChild(row);
            });
        });
}

function addAluno() {

    const Nome = document.getElementById('nome').value;
    const Sobrenome = document.getElementById('sobrenome').value;
    const RM = document.getElementById('RM').value;
    const Telefone = document.getElementById('telefone').value;
    const Curso = document.getElementById('curso').value.toString();
    const Status = document.getElementById('status').value.toString();


    const aluno = {
        Nome: Nome,
        Sobrenome: Sobrenome,
        RM: RM,
        Telefone: Telefone,
        Curso: Curso,
        Status: Status
    };
    console.log(Status);
    fetch('https://localhost:7139/Aluno', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(aluno)
    })
        .then(response => {
            if (response.ok) {
                document.getElementById('nome').value = '';
                document.getElementById('sobrenome').value = '';
                document.getElementById('telefone').value = '';
                document.getElementById('RM').value = '';
                document.getElementById('curso').value = '';
                document.getElementById('status').value = '';
                getAluno();
            }
        });

    console.log('Adicionar aluno');
    showNotification('Usuário adicionado com sucesso!', 'success');
}


function deleteAluno() {
    const id = parseInt(document.getElementById('deleId').value);
    fetch(`https://localhost:7139/Aluno/${id}`, {
        method: 'DELETE'
    })
        .then(response => {
            if (response.ok) {
                getAluno();
            }
        });
    console.log('Deletar aluno');
    showNotification('Usuário excluído com sucesso!', 'error');
}

function openModal(id){ document.getElementById(id).style.display='block'; }
function closeModal(id){ document.getElementById(id).style.display='none'; }
function confirmDelete(){
   if(confirm("Deseja realmente excluir este usuário?")){
       deleteAluno();
       closeModal('deleteModal');
   }
}

function updateAluno() {


    const upId = parseInt(document.getElementById('upid').value);
    const upNome = document.getElementById('upnome').value;
    const upSobrenome = document.getElementById('upsobrenome').value;
    const upRM = document.getElementById('upRM').value.toString();
    const upTelefone = document.getElementById('uptelefone').value;
    const upCurso = document.getElementById('upcurso').value.toString();
    const upStatus = document.getElementById('upstatus').value.toString();


    const aluno = {
        Nome: upNome,
        Sobrenome: upSobrenome,
        RM: upRM,
        Telefone: upTelefone,
        Curso: upCurso,
        Status: upStatus
        
    };

    fetch(`https://localhost:7139/Aluno/${upId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(aluno)
    })
        .then(response => {
            if (response.ok) {
                document.getElementById('upnome').value = '';
                document.getElementById('upsobrenome').value = '';
                document.getElementById('uptelefone').value = '';
                document.getElementById('upRM').value = '';
                document.getElementById('upcurso').value = '';
                document.getElementById('upstatus').value = '';

                getAluno();
            }
        });

    console.log('Atualizar aluno');
    showNotification('Usuário atualizado com sucesso!', 'info');
}



getAluno();
//login//

function login() {
    const RM = document.getElementById("username").value;
    const Senha = document.getElementById("senha").value;

    const login = {
        RM: RM,
        Senha: Senha
    }

    fetch('https://localhost:7139/Login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(login)
    })
        .then(response => response.json())
        .then(data => {
            if (data) {
                localStorage.setItem('authenticated', 'true');
                window.location.href = 'CadUser.html';
            } else {
                alert('Login falhou');
            }
        })
        .catch(error => {
            console.error('Erro', error);
        })
}

// function checkAuthentication(){
//     const isAuthenticated = localStorage.getItem('authenticated');

//     return isAuthenticated === 'true';
// }

// window.addEventListener('DOMContentLoaded', () => {
//     if(!checkAuthentication()){
//         window.location.href = 'login.html';
//     }
// });

// //fazer lougout, rodapé, retira admin, cadastro em botao e pop-up, arrumar o dashboard
// function logout(){
//     localStorage.setItem('authenticated', 'false');

//     window.location.href = 'login.html';
// }


//CadLivro

function addLivro() {
    // Campo 020 - ISBN
    const ISBN = document.getElementById('isbn_a').value;
    const Cond_Encardenacao = document.getElementById('isbn_c').value;

    // Campo 040 - Fonte da Catalogação
    const Agen_Catalogadora = document.getElementById('agencia').value;
    const Idi_Catalogacao = document.getElementById('idioma_catalogacao').value;
    const Agen_Transcricao = document.getElementById('agencia_transcricao').value;
    const Agen_Modigicacao = document.getElementById('agencia_modificacao').value;

    // Campo 041 - Código de Idioma
    const Idi_Texto = document.getElementById('idioma_texto').value;
    const Idi_Resumo = document.getElementById('idioma_resumo').value;
    const Idi_Legenda = document.getElementById('idioma_legenda').value;

    // Campo 082 - Classificação CDD
    const Numero_CDD = document.getElementById('cdd').value;
    const Numero_Item_CDD = document.getElementById('cdd_numero').value;

    // Campo 090 - Classificação Local
    const Num_Cham_Local = document.getElementById('classificacao_local').value;
    const Num_Item_Local = document.getElementById('classificacao_local_b').value;

    // Campo 095 - Classificação Local Secundária
    const Num_Cham_Secundaria = document.getElementById('classificacao_local_sec').value;

    // Campo 245 - Título (CORRIGIDO - estava faltando)
    const Nome = document.getElementById('titulo').value;
    const Subtitulo = document.getElementById('subtitulo').value;
    const Indi_Responsabilidade = document.getElementById('responsabilidade').value;
    const Indi_Arti_Inicial = document.getElementById('indicador').value;

    // Campo 250 - Edição
    const Num_Edicao = document.getElementById('edicao').value;
    const Mencao_Responsa_Edicao = document.getElementById('edicao_mencao').value;

    // Campo 260 - Publicação
    const Local_Publicacao = document.getElementById('local').value;
    const Editora = document.getElementById('editora').value;
    const Ano_Publicacao = parseInt(document.getElementById('ano').value) || 0;

    // Campo 300 - Descrição Física
    const Paginas = document.getElementById('paginas').value;
    const Ilustracoes = document.getElementById('ilustracoes').value;
    const Dimensoes = document.getElementById('dimensoes').value;
    const Material_Adicional = document.getElementById('material_adicional').value;

    // Campo 490 - Série
    const Titulo_Serie = document.getElementById('serie').value;
    const Num_Serie = document.getElementById('serie_numero').value;

    // Campo 500 - Notas Gerais
    const Notas_Gerais = document.getElementById('nota').value;

    // Campo 600 - Assunto - Nome Pessoal
    const Nome_Pess_Assunto = document.getElementById('assunto_nome').value;
    const Datas_Pessoais = document.getElementById('autor_datas').value;
    const Funcao_Pessoal = document.getElementById('autor_funcao').value;
    const Topico = document.getElementById('assunto_topico').value;

    // Campo 630 - Assunto - Título Uniforme
    const Titulo_Uniforme = document.getElementById('assunto_titulo').value;
    const Forma_Uniforme = document.getElementById('assunto_forma').value;
    const Periodo_Historico = document.getElementById('assunto_tempo').value;
    const Local_Uniforme = document.getElementById('assunto_local').value;

    // Campo 650 - Assunto - Termo Tópico
    const Assunto_Termo = document.getElementById('assunto_topico2').value;
    const Forma_Termo = document.getElementById('assunto_forma2').value;
    const Periodo_Histo_Termo = document.getElementById('assunto_tempo2').value;
    const Local_Termo = document.getElementById('assunto_local2').value;

    // Campo 949 - Controle Local
    const Info_Local = document.getElementById('controle_local').value;
    const Status_Item = document.getElementById('controle_status').value.toString();
    const Status_Emprestimos = document.getElementById('status_emprestimo').value.toString();

    // Objeto com os mesmos nomes da classe C# Livro.cs
    const livro = {
        ISBN: ISBN,
        Cond_Encardenacao: Cond_Encardenacao,
        Agen_Catalogadora: Agen_Catalogadora,
        Idi_Catalogacao: Idi_Catalogacao,
        Agen_Transcricao: Agen_Transcricao,
        Agen_Modigicacao: Agen_Modigicacao,
        Idi_Texto: Idi_Texto,
        Idi_Resumo: Idi_Resumo,
        Idi_Legenda: Idi_Legenda,
        Numero_CDD: Numero_CDD,
        Numero_Item_CDD: Numero_Item_CDD,
        Num_Cham_Local: Num_Cham_Local,
        Num_Item_Local: Num_Item_Local,
        Num_Cham_Secundaria: Num_Cham_Secundaria,
        Nome: Nome,
        Subtitulo: Subtitulo,
        Indi_Responsabilidade: Indi_Responsabilidade,
        Indi_Arti_Inicial: Indi_Arti_Inicial,
        Num_Edicao: Num_Edicao,
        Mencao_Responsa_Edicao: Mencao_Responsa_Edicao,
        Local_Publicacao: Local_Publicacao,
        Editora: Editora,
        Ano_Publicacao: Ano_Publicacao,
        Paginas: Paginas,
        Ilustracoes: Ilustracoes,
        Dimensoes: Dimensoes,
        Material_Adicional: Material_Adicional,
        Titulo_Serie: Titulo_Serie,
        Num_Serie: Num_Serie,
        Notas_Gerais: Notas_Gerais,
        Nome_Pess_Assunto: Nome_Pess_Assunto,
        Datas_Pessoais: Datas_Pessoais,
        Funcao_Pessoal: Funcao_Pessoal,
        Topico: Topico,
        Titulo_Uniforme: Titulo_Uniforme,
        Forma_Uniforme: Forma_Uniforme,
        Periodo_Historico: Periodo_Historico,
        Local_Uniforme: Local_Uniforme,
        Assunto_Termo: Assunto_Termo,
        Forma_Termo: Forma_Termo,
        Periodo_Histo_Termo: Periodo_Histo_Termo,
        Local_Termo: Local_Termo,
        Info_Local: Info_Local,
        Status_Item: Status_Item,
        Status_Emprestimos: Status_Emprestimos
    };

    console.log('Dados do livro:', livro);

    fetch('https://localhost:7139/Livro', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(livro)
    })
    .then(response => {
        if (response.ok) {
            alert('Livro cadastrado com sucesso!');
            // Limpar formulário
            document.querySelector('form').reset();
        } else {
            response.text().then(text => {
                console.error('Erro do servidor:', text);
                alert('Erro ao cadastrar livro. Verifique o console para detalhes.');
            });
        }
    })
    .catch(error => {
        console.error('Erro na requisição:', error);
        alert('Erro ao conectar com o servidor!');
    });
}


// Buscar livro acervo

async function buscarLivros() {
    const termo = document.getElementById('busca').value.trim();
    const resultadosDiv = document.getElementById('resultados');

    resultadosDiv.innerHTML = '';

    try {
        const response = await fetch(`https://localhost:7139/Livro/search?termo=${encodeURIComponent(termo)}`);
        if (!response.ok) throw new Error('Erro ao buscar livros');

        const livros = await response.json();

        if (livros.length === 0) {
            resultadosDiv.innerHTML = '<p>Nenhum livro encontrado.</p>';
            return;
        }

        const lista = document.createElement('ul');
        lista.style.listStyle = "none";

        livros.forEach(livro => {
            const item = document.createElement('li');
            item.style.border = "1px solid #ccc";
            item.style.margin = "5px";
            item.style.padding = "10px";
            item.style.borderRadius = "5px";

            item.innerHTML = `
                <strong>${livro.Nome_Livro}</strong> (${livro.Ano_Publicacao || 'Ano não informado'})<br>
                ${livro.Subtitulo ? livro.Subtitulo + '<br>' : ''}
                Autor/Responsável: ${livro.Indicacao_Responsabilidade || 'Não informado'}<br>
                Autores: ${livro.Autores || 'Não informado'}<br>
                ISBN: ${livro.ISBN || 'Não informado'}<br>
                Assunto: ${livro.Assunto_Termo || 'Não informado'}
            `;
            lista.appendChild(item);
        });

        resultadosDiv.appendChild(lista);

    } catch (error) {
        resultadosDiv.innerHTML = `<p style="color:red;">${error.message}</p>`;
        console.error(error);
    }
}