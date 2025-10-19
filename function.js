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

                const actionCell = document.createElement('td');
                actionCell.innerHTML = `
                    <div class="action">
                        <button class="btn-edit" onclick="abrirEdicao(${aluno.id}, '${aluno.nome}', '${aluno.sobrenome}', '${aluno.telefone}', '${aluno.rm}', '${aluno.curso}', '${aluno.status}')">Editar</button>
                        <button class="btn-delete" onclick="abrirExclusao(${aluno.id})">Excluir</button>
                    </div>
                `;
                row.appendChild(actionCell);

                tbody.appendChild(row);
            });
        });
}

function abrirEdicao(id, nome, sobrenome, telefone, rm, curso, status) {
    document.getElementById('upid').value = id;
    document.getElementById('upnome').value = nome;
    document.getElementById('upsobrenome').value = sobrenome;
    document.getElementById('uptelefone').value = telefone;
    document.getElementById('upRM').value = rm;
    document.getElementById('upcurso').value = curso;
    document.getElementById('upstatus').value = status;

    openModal('editModal');
}

function abrirExclusao(id) {
    document.getElementById('deleId').value = id;
    openModal('deleteModal');
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
                showNotification('Usuário adicionado com sucesso!', 'success');
            }
        });

    console.log('Adicionar aluno');

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

function openModal(id) { document.getElementById(id).style.display = 'block'; }
function closeModal(id) { document.getElementById(id).style.display = 'none'; }
function confirmDelete() {
    if (confirm("Deseja realmente excluir este usuário?")) {
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

/* ===== LOGIN ===== */

async function login(e) {
    if (e) e.preventDefault();

    const rm = document.getElementById('username').value;
    const senha = document.getElementById('senha').value;

    try {
        const response = await fetch('https://localhost:7139/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ rm, senha })
        });

        const isAuthenticated = await response.json();

        if (response.ok && isAuthenticated === true) {
            localStorage.setItem('authenticated', 'true');
            localStorage.setItem('rm', rm);

            alert('Login realizado com sucesso!');
            window.location.href = 'indexBibli.html';
        } else {
            alert('RM ou senha inválidos!');
        }
    } catch (error) {
        console.error('❌ Erro:', error);
        alert('Erro ao conectar com o servidor.');
    }
}

function checkAuthentication() {
    const isAuthenticated = localStorage.getItem('authenticated');
    return isAuthenticated === 'true';
}

function logout() {
    localStorage.setItem('authenticated', 'false');
    localStorage.removeItem('rm');

    alert('Logout realizado com sucesso!');
    window.location.href = 'login.html';
}

document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.getElementById('loginForm');
    const logoutButton = document.getElementById('logoutButton');

    if (loginForm) {
        loginForm.addEventListener('submit', async (e) => {
            e.preventDefault(); // IMPEDE O RELOAD
            await login(e); // Passa o evento para a função
        });
    }

    const paginaAtual = window.location.pathname;
    const paginasPublicas = ['login.html', 'index.html', 'acervo.html'];

    const isPaginaPublica = paginasPublicas.some(pagina =>
        paginaAtual.includes(pagina) || paginaAtual === '/' || paginaAtual === ''
    );

    if (!isPaginaPublica) {
        if (!checkAuthentication()) {
            alert('Você precisa fazer login para acessar esta página!');
            window.location.href = 'login.html';
        }
    }

    if (logoutButton) {
        const isAuthenticated = localStorage.getItem('authenticated') === 'true';
        logoutButton.style.display = isAuthenticated ? 'inline-block' : 'none';
    }
});





/* ===== CADLIVROS===== */

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

    //  DADOS DO AUTOR 
    const Nome_Autor = document.getElementById('autor_nome').value;
    const Numero = document.getElementById('autor_numero').value;
    const Datas = document.getElementById('autor_datas').value;
    const Funcao = document.getElementById('autor_funcao').value;

    //  DADOS DA ENTIDADE
    const Nome_Entidade = document.getElementById('entidade_nome').value;
    const Subordinacao = document.getElementById('entidade_subordinacao').value

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

    const autor = {
        Nome_Autor: Nome_Autor,
        Numero: Numero_Autor,
        Datas: Datas_Autor,
        Funcao: Funcao_Autor,
        Tipo_Autor: Tipo_Autor
    };

    const entidade = {
        Nome_Entidade: Nome_Entidade,
        Subordinacao: Subordinacao
    };

    const BibliotecaRequest = {
        livro: livro,
        autor: autor,
        entidade: entidade
    };

    console.log('Dados do livro:', BibliotecaRequest);


    fetch('https://localhost:7139/Livro', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(bibliotecaRequest)
    })
        .then(response => {
            if (response.ok) {
                alert('Livro cadastrado com sucesso!');
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

function contarCaracteres(input) {
    const max = input.getAttribute("maxlength");
    const contador = input.parentElement.querySelector(".input-counter");
    contador.textContent = `${input.value.length}/${max}`;
}



/* ===== ACERVO ===== */

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
            const card = document.createElement('div');
            card.className = 'livro-card';
            card.innerHTML = `
                <strong>${livro.nome_Livro}</strong> ${livro.subtitulo ? livro.subtitulo + '<br>' : ''}
                (${livro.ano_Publicacao || 'Ano não informado'})<br>
                <strong>Autor/Responsável:</strong> ${livro.indicacao_Responsabilidade || 'Não informado'}<br>
                <strong>Autores:</strong> ${livro.autores || 'Não informado'}<br>
                <strong>ISBN:</strong> ${livro.isbn || 'Não informado'}<br>
                <strong>Assunto:</strong> ${livro.assunto_Termo || 'Não informado'}<br>
                <strong>Status:</strong> ${livro.status_Emprestimos || 'Não informado'}
            `;
            lista.appendChild(card);
        });


        resultadosDiv.appendChild(lista);

    } catch (error) {
        resultadosDiv.innerHTML = `<p style="color:red;">${error.message}</p>`;
        console.error(error);
    }
    // Função de limpar busca do acervo
    async function limparBuscar() {
        document.getElementById('busca').value = '';
        document.getElementById('resultados').innerHTML = '';
        showNotification('Busca limpa com sucesso!', 'info');
    };
}

/* ===== EMPRÉSTIMO ===== */

// Buscar livros para autocomplete
let livrosCache = []; // Cache para evitar múltiplas requisições
let timeoutBusca;

async function buscarLivrosAutocomplete(termo) {
    const sugestoesDiv = document.getElementById('livroSugestoes');
    clearTimeout(timeoutBusca);

    if (!termo || termo.length < 2) {
        sugestoesDiv.innerHTML = '';
        return;
    }

    timeoutBusca = setTimeout(async () => {
        try {
            const response = await fetch(`https://localhost:7139/Livro/search?termo=${encodeURIComponent(termo)}`);

            if (!response.ok) {
                throw new Error('Erro ao buscar livros');
            }

            const livros = await response.json();
            livrosCache = livros;

            sugestoesDiv.innerHTML = '';

            if (livros.length === 0) {
                sugestoesDiv.innerHTML = '<div class="autocomplete-item no-results">Nenhum livro encontrado</div>';
                return;
            }

            // Criar lista de sugestões
            livros.forEach(livro => {
                const item = document.createElement('div');
                item.className = 'autocomplete-item';
                item.innerHTML = `
                    <strong>${livro.nome_Livro}</strong>
                    <small>
                        ${livro.autores ? `Autor: ${livro.autores} | ` : ''}
                        ISBN: ${livro.isbn || 'N/A'} | 
                        Status: ${livro.status_Emprestimos || 'Disponível'}
                    </small>
                `;

                item.onclick = () => selecionarLivro(livro);

                sugestoesDiv.appendChild(item);
            });

        } catch (error) {
            console.error('Erro ao buscar livros:', error);
            sugestoesDiv.innerHTML = '<div class="autocomplete-item no-results">Erro ao buscar livros</div>';
        }
    }, 300);
}

function selecionarLivro(livro) {
    // Preencher o campo com o nome do livro
    document.getElementById('livro').value = livro.nome_Livro;

    // Guardar o ID do livro em um campo hidden
    document.getElementById('livroIdSelecionado').value = livro.id_Livro;

    document.getElementById('livroSugestoes').innerHTML = '';
}

// Fechar sugestões ao clicar fora
document.addEventListener('click', function (e) {
    const container = document.querySelector('.autocomplete-container');
    if (container && !container.contains(e.target)) {
        document.getElementById('livroSugestoes').innerHTML = '';
    }
});

function addEmprestimo() {
    const livroId = document.getElementById('livroIdSelecionado').value;

    if (!livroId) {
        alert('Por favor, selecione um livro da lista de sugestões!');
        return;
    }

    const emprestimo = {
        RM_Aluno: document.getElementById('RM').value,
        Id_Livro: parseInt(livroId),
        Data_Emprestimo: document.getElementById('dataEmprestimo').value,
        Data_Devolucao_Prevista: document.getElementById('dataDevolucao').value,
        Data_Devolucao_Real: null
    };

    fetch('https://localhost:7139/Emprestimo', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(emprestimo)
    })
        .then(response => {
            if (response.ok) {
                alert('Empréstimo cadastrado com sucesso!');
                document.getElementById('formEmprestimo').reset();
                document.getElementById('livroIdSelecionado').value = '';
                closeModal('addEmprestimo');
                getEmprestimos(); // Se você tiver uma função para listar empréstimos
            } else {
                response.text().then(text => {
                    alert('Erro ao cadastrar empréstimo: ' + text);
                });
            }
        })
        .catch(error => {
            console.error('Erro:', error);
            alert('Erro ao conectar com o servidor!');
        });
}


// Função para buscar e exibir
function getEmprestimos() {
    fetch('https://localhost:7139/Emprestimo')
        .then(response => {
            if (!response.ok) {
                throw new Error('Erro ao buscar empréstimos');
            }
            return response.json();
        })
        .then(data => {
            const emprestimoTable = document.getElementById('emprestimoTable');
            const tbody = emprestimoTable.querySelector('tbody');
            tbody.innerHTML = '';

            if (data.length === 0) {
                tbody.innerHTML = '<tr><td colspan="8" style="text-align:center;">Nenhum empréstimo encontrado</td></tr>';
                return;
            }

            data.forEach(emprestimo => {
                const row = document.createElement('tr');

                const idCell = document.createElement('td');
                idCell.textContent = emprestimo.id_Emprestimo;
                row.appendChild(idCell);

                const alunoCell = document.createElement('td');
                alunoCell.textContent = `${emprestimo.nomeAluno} (${emprestimo.rm_Aluno})`;
                row.appendChild(alunoCell);

                const livroCell = document.createElement('td');
                livroCell.textContent = emprestimo.nomeLivro;
                row.appendChild(livroCell);

                const dataEmpCell = document.createElement('td');
                dataEmpCell.textContent = formatarData(emprestimo.data_Emprestimo);
                row.appendChild(dataEmpCell);

                const dataPrevCell = document.createElement('td');
                dataPrevCell.textContent = formatarData(emprestimo.data_Devolucao_Prevista);
                row.appendChild(dataPrevCell);

                const dataRealCell = document.createElement('td');
                if (emprestimo.data_Devolucao_Real) {
                    dataRealCell.textContent = formatarData(emprestimo.data_Devolucao_Real);

                    const dataReal = new Date(emprestimo.data_Devolucao_Real);
                    const dataPrevista = new Date(emprestimo.data_Devolucao_Prevista);

                    if (dataReal > dataPrevista) {
                        dataRealCell.style.color = '#a20c0cff';
                        dataRealCell.style.fontWeight = '600';
                    } else {
                        dataRealCell.style.color = '#0d7c11ff';
                        dataRealCell.style.fontWeight = '600';
                    }
                } else {
                    dataRealCell.textContent = '-';
                    dataRealCell.style.color = '#999';
                }

                row.appendChild(dataRealCell);


                const statusCell = document.createElement('td');
                const status = calcularStatus(
                    emprestimo.data_Devolucao_Prevista,
                    emprestimo.data_Devolucao_Real
                );
                statusCell.innerHTML = `<span class="status-badge ${status.classe}">${status.texto}</span>`;
                row.appendChild(statusCell);

                const actionCell = document.createElement('td');
                actionCell.innerHTML = `
                    <div class="action">
                        ${emprestimo.data_Devolucao_Real === null ?
                        `<button class="btn-devolucao" onclick="registrarDevolucao(${emprestimo.id_Emprestimo})">Devolver</button>`
                        :''
                    }
                        <button class="btn-delete" onclick="abrirExclusaoEmprestimo(${emprestimo.id_Emprestimo})">Excluir</button>
                    </div>
                `;
                row.appendChild(actionCell);

                tbody.appendChild(row);
            });
        })
        .catch(error => {
            console.error('Erro ao buscar empréstimos:', error);
            showNotification('Erro ao carregar empréstimos', 'error');
        });
}

function formatarData(dataString) {
    if (!dataString) return '-';
    const data = new Date(dataString);
    return data.toLocaleDateString('pt-BR', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    });
}

function calcularStatus(dataDevolucaoPrevista, dataDevolucaoReal) {
    const hoje = new Date();
    hoje.setHours(0, 0, 0, 0);
    const dataPrevista = new Date(dataDevolucaoPrevista);
    dataPrevista.setHours(0, 0, 0, 0);

    if (dataDevolucaoReal) {
        return { texto: 'Devolvido', classe: 'status-devolvido' };
    }

    if (hoje > dataPrevista) {
        return { texto: 'Atrasado', classe: 'status-atrasado' };
    }

    return { texto: 'Ativo', classe: 'status-ativo' };
}

function registrarDevolucao(idEmprestimo) {
    if (!confirm('Confirmar devolução do livro?')) return;

    const dataAtual = new Date().toISOString().split('T')[0];

    fetch(`https://localhost:7139/Emprestimo/${idEmprestimo}/devolver`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ dataDevolucao: dataAtual })
    })
        .then(response => {
            if (response.ok) {
                showNotification('Devolução registrada com sucesso!', 'success');
                getEmprestimos(); // Recarrega a tabela
            } else {
                response.text().then(text => {
                    showNotification(text || 'Erro ao registrar devolução', 'error');
                });
            }
        })
        .catch(error => {
            console.error('Erro:', error);
            showNotification('Erro ao conectar com o servidor', 'error');
        });
}

function abrirExclusaoEmprestimo(id) {
    document.getElementById('deleIdEmprestimo').value = id;
    openModal('deleteModalEmprestimo');
}

function deleteEmprestimo() {
    const id = parseInt(document.getElementById('deleIdEmprestimo').value);

    fetch(`https://localhost:7139/Emprestimo/${id}`, {
        method: 'DELETE'
    })
        .then(response => {
            if (response.ok) {
                showNotification('Empréstimo excluído com sucesso!', 'success');
                getEmprestimos();
                closeModal('deleteModalEmprestimo');
            } else {
                showNotification('Erro ao excluir empréstimo', 'error');
            }
        })
        .catch(error => {
            console.error('Erro:', error);
            showNotification('Erro ao conectar com o servidor', 'error');
        });
}

if (window.location.pathname.includes('emprestimo')) {
    getEmprestimos();
}


/* ===== ANCORA ===== */

document.addEventListener('DOMContentLoaded', () => {
    const btnTopo = document.getElementById('btnTopo');

    function verificarRolagem() {
        const scrolled = window.scrollY;
        const totalScroll = document.documentElement.scrollHeight - window.innerHeight;

        if (totalScroll > 0 && scrolled > 0) {
            btnTopo.classList.add('mostrar');
        } else {
            btnTopo.classList.remove('mostrar');
        }
    }

    btnTopo.addEventListener('click', (e) => {
        e.preventDefault();
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });

    window.addEventListener('scroll', verificarRolagem);
    window.addEventListener('resize', verificarRolagem);
    verificarRolagem();
});

