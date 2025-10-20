function getAluno() {
  fetch("https://localhost:7139/Aluno")
    .then((response) => response.json())
    .then((data) => {
      const alunoTable = document.getElementById("alunoTable");
      const tbody = alunoTable.querySelector("tbody");
      tbody.innerHTML = "";

      data.forEach((aluno) => {
        const row = document.createElement("tr");
        const idCell = document.createElement("td");
        idCell.textContent = aluno.id;
        row.appendChild(idCell);

        const nomeCell = document.createElement("td");
        nomeCell.textContent = aluno.nome;
        row.appendChild(nomeCell);

        const sobrenomeCell = document.createElement("td");
        sobrenomeCell.textContent = aluno.sobrenome;
        row.appendChild(sobrenomeCell);

        const telCell = document.createElement("td");
        telCell.textContent = aluno.telefone;
        row.appendChild(telCell);

        const rmCell = document.createElement("td");
        rmCell.textContent = aluno.rm;
        row.appendChild(rmCell);

        const curCell = document.createElement("td");
        curCell.textContent = aluno.curso;
        row.appendChild(curCell);

        const statusCell = document.createElement("td");
        statusCell.textContent = aluno.status;
        row.appendChild(statusCell);

        const actionCell = document.createElement("td");
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
  document.getElementById("upid").value = id;
  document.getElementById("upnome").value = nome;
  document.getElementById("upsobrenome").value = sobrenome;
  document.getElementById("uptelefone").value = telefone;
  document.getElementById("upRM").value = rm;
  document.getElementById("upcurso").value = curso;
  document.getElementById("upstatus").value = status;

  openModal("editModal");
}

function abrirExclusao(id) {
  document.getElementById("deleId").value = id;
  openModal("deleteModal");
}

function addAluno() {
  const Nome = document.getElementById("nome").value;
  const Sobrenome = document.getElementById("sobrenome").value;
  const RM = document.getElementById("RM").value;
  const Telefone = document.getElementById("telefone").value;
  const Curso = document.getElementById("curso").value.toString();
  const Status = document.getElementById("status").value.toString();

  const aluno = {
    Nome: Nome,
    Sobrenome: Sobrenome,
    RM: RM,
    Telefone: Telefone,
    Curso: Curso,
    Status: Status,
  };
  console.log(Status);
  fetch("https://localhost:7139/Aluno", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(aluno),
  }).then((response) => {
    if (response.ok) {
      document.getElementById("nome").value = "";
      document.getElementById("sobrenome").value = "";
      document.getElementById("telefone").value = "";
      document.getElementById("RM").value = "";
      document.getElementById("curso").value = "";
      document.getElementById("status").value = "";
      getAluno();
      showNotification("Usuário adicionado com sucesso!", "success");
    }
  });

  console.log("Adicionar aluno");
}

function deleteAluno() {
  const id = parseInt(document.getElementById("deleId").value);
  fetch(`https://localhost:7139/Aluno/${id}`, {
    method: "DELETE",
  }).then((response) => {
    if (response.ok) {
      getAluno();
    }
  });
  console.log("Deletar aluno");
  showNotification("Usuário excluído com sucesso!", "error");
}

function openModal(id) {
  document.getElementById(id).style.display = "block";
}
function closeModal(id) {
  document.getElementById(id).style.display = "none";
}
function confirmDelete() {
  if (confirm("Deseja realmente excluir este usuário?")) {
    deleteAluno();
    closeModal("deleteModal");
  }
}

function updateAluno() {
  const upId = parseInt(document.getElementById("upid").value);
  const upNome = document.getElementById("upnome").value;
  const upSobrenome = document.getElementById("upsobrenome").value;
  const upRM = document.getElementById("upRM").value.toString();
  const upTelefone = document.getElementById("uptelefone").value;
  const upCurso = document.getElementById("upcurso").value.toString();
  const upStatus = document.getElementById("upstatus").value.toString();

  const aluno = {
    Nome: upNome,
    Sobrenome: upSobrenome,
    RM: upRM,
    Telefone: upTelefone,
    Curso: upCurso,
    Status: upStatus,
  };

  fetch(`https://localhost:7139/Aluno/${upId}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(aluno),
  }).then((response) => {
    if (response.ok) {
      document.getElementById("upnome").value = "";
      document.getElementById("upsobrenome").value = "";
      document.getElementById("uptelefone").value = "";
      document.getElementById("upRM").value = "";
      document.getElementById("upcurso").value = "";
      document.getElementById("upstatus").value = "";

      getAluno();
    }
  });

  console.log("Atualizar aluno");
  showNotification("Usuário atualizado com sucesso!", "info");
}

getAluno();

/* ===== LOGIN ===== */

async function login(e) {
  if (e) e.preventDefault();

  const rm = document.getElementById("username").value;
  const senha = document.getElementById("senha").value;

  try {
    const response = await fetch("https://localhost:7139/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ rm, senha }),
    });

    const isAuthenticated = await response.json();

    if (response.ok && isAuthenticated === true) {
      localStorage.setItem("authenticated", "true");
      localStorage.setItem("rm", rm);

      alert("Login realizado com sucesso!");
      window.location.href = "indexBibli.html";
    } else {
      alert("RM ou senha inválidos!");
    }
  } catch (error) {
    console.error("❌ Erro:", error);
    alert("Erro ao conectar com o servidor.");
  }
}

function checkAuthentication() {
  const isAuthenticated = localStorage.getItem("authenticated");
  return isAuthenticated === "true";
}

function logout() {
  localStorage.setItem("authenticated", "false");
  localStorage.removeItem("rm");

  alert("Logout realizado com sucesso!");
  window.location.href = "login.html";
}

document.addEventListener("DOMContentLoaded", () => {
  const loginForm = document.getElementById("loginForm");
  const logoutButton = document.getElementById("logoutButton");

  if (loginForm) {
    loginForm.addEventListener("submit", async (e) => {
      e.preventDefault(); // IMPEDE O RELOAD
      await login(e); // Passa o evento para a função
    });
  }

  const paginaAtual = window.location.pathname;
  const paginasPublicas = ["login.html", "index.html", "acervo.html"];

  const isPaginaPublica = paginasPublicas.some(
    (pagina) =>
      paginaAtual.includes(pagina) || paginaAtual === "/" || paginaAtual === ""
  );

  if (!isPaginaPublica) {
    if (!checkAuthentication()) {
      alert("Você precisa fazer login para acessar esta página!");
      window.location.href = "login.html";
    }
  }

  if (logoutButton) {
    const isAuthenticated = localStorage.getItem("authenticated") === "true";
    logoutButton.style.display = isAuthenticated ? "inline-block" : "none";
  }
});

/* ===== CADLIVROS===== */

function addLivro() {
  // Campo 020 - ISBN
  const ISBN = document.getElementById("isbn_a").value;
  const Cond_Encardenacao = document.getElementById("isbn_c").value;

  // Campo 040 - Fonte da Catalogação
  const Agen_Catalogadora = document.getElementById("agencia").value;
  const Idi_Catalogacao = document.getElementById("idioma_catalogacao").value;
  const Agen_Transcricao = document.getElementById("agencia_transcricao").value;
  const Agen_Modigicacao = document.getElementById("agencia_modificacao").value;

  // Campo 041 - Código de Idioma
  const Idi_Texto = document.getElementById("idioma_texto").value;
  const Idi_Resumo = document.getElementById("idioma_resumo").value;
  const Idi_Legenda = document.getElementById("idioma_legenda").value;

  // Campo 082 - Classificação CDD
  const Numero_CDD = document.getElementById("cdd").value;
  const Numero_Item_CDD = document.getElementById("cdd_numero").value;

  // Campo 090 - Classificação Local
  const Num_Cham_Local = document.getElementById("classificacao_local").value;
  const Num_Item_Local = document.getElementById("classificacao_local_b").value;

  // Campo 095 - Classificação Local Secundária
  const Num_Cham_Secundaria = document.getElementById(
    "classificacao_local_sec"
  ).value;

  // Campo 245 - Título (CORRIGIDO - estava faltando)
  const Nome = document.getElementById("titulo").value;
  const Subtitulo = document.getElementById("subtitulo").value;
  const Indi_Responsabilidade =
    document.getElementById("responsabilidade").value;
  const Indi_Arti_Inicial = document.getElementById("indicador").value;

  // Campo 250 - Edição
  const Num_Edicao = document.getElementById("edicao").value;
  const Mencao_Responsa_Edicao = document.getElementById("edicao_mencao").value;

  // Campo 260 - Publicação
  const Local_Publicacao = document.getElementById("local").value;
  const Editora = document.getElementById("editora").value;
  const Ano_Publicacao = parseInt(document.getElementById("ano").value) || 0;

  // Campo 300 - Descrição Física
  const Paginas = document.getElementById("paginas").value;
  const Ilustracoes = document.getElementById("ilustracoes").value;
  const Dimensoes = document.getElementById("dimensoes").value;
  const Material_Adicional =
    document.getElementById("material_adicional").value;

  // Campo 490 - Série
  const Titulo_Serie = document.getElementById("serie").value;
  const Num_Serie = document.getElementById("serie_numero").value;

  // Campo 500 - Notas Gerais
  const Notas_Gerais = document.getElementById("nota").value;

  // Campo 600 - Assunto - Nome Pessoal
  const Nome_Pess_Assunto = document.getElementById("assunto_nome").value;
  const Datas_Pessoais = document.getElementById("autor_datas").value;
  const Funcao_Pessoal = document.getElementById("autor_funcao").value;
  const Topico = document.getElementById("assunto_topico").value;

  // Campo 630 - Assunto - Título Uniforme
  const Titulo_Uniforme = document.getElementById("assunto_titulo").value;
  const Forma_Uniforme = document.getElementById("assunto_forma").value;
  const Periodo_Historico = document.getElementById("assunto_tempo").value;
  const Local_Uniforme = document.getElementById("assunto_local").value;

  // Campo 650 - Assunto - Termo Tópico
  const Assunto_Termo = document.getElementById("assunto_topico2").value;
  const Forma_Termo = document.getElementById("assunto_forma2").value;
  const Periodo_Histo_Termo = document.getElementById("assunto_tempo2").value;
  const Local_Termo = document.getElementById("assunto_local2").value;

  // Campo 949 - Controle Local
  const Info_Local = document.getElementById("controle_local").value;
  const Status_Item = document
    .getElementById("controle_status")
    .value.toString();
  const Status_Emprestimos = document
    .getElementById("status_emprestimo")
    .value.toString();

  //  DADOS DO AUTOR
  const Nome_Autor = document.getElementById("autor_nome").value;
  const Numero = document.getElementById("autor_numero").value;
  const Datas = document.getElementById("autor_datas").value;
  const Funcao = document.getElementById("autor_funcao").value;

  //  DADOS DA ENTIDADE
  const Nome_Entidade = document.getElementById("entidade_nome").value;
  const Subordinacao = document.getElementById("entidade_subordinacao").value;

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
    Status_Emprestimos: Status_Emprestimos,
  };

  const autor = {
    Nome_Autor: Nome_Autor,
    Numero: Numero_Autor,
    Datas: Datas_Autor,
    Funcao: Funcao_Autor,
    Tipo_Autor: Tipo_Autor,
  };

  const entidade = {
    Nome_Entidade: Nome_Entidade,
    Subordinacao: Subordinacao,
  };

  const BibliotecaRequest = {
    livro: livro,
    autor: autor,
    entidade: entidade,
  };

  console.log("Dados do livro:", BibliotecaRequest);

  fetch("https://localhost:7139/Livro", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(bibliotecaRequest),
  })
    .then((response) => {
      if (response.ok) {
        alert("Livro cadastrado com sucesso!");
        document.querySelector("form").reset();
      } else {
        response.text().then((text) => {
          console.error("Erro do servidor:", text);
          alert("Erro ao cadastrar livro. Verifique o console para detalhes.");
        });
      }
    })
    .catch((error) => {
      console.error("Erro na requisição:", error);
      alert("Erro ao conectar com o servidor!");
    });
}

function contarCaracteres(input) {
  const max = input.getAttribute("maxlength");
  const contador = input.parentElement.querySelector(".input-counter");
  contador.textContent = `${input.value.length}/${max}`;
}

/* ===== ACERVO ===== */

const MAPA_MARC21 = {
  // Campo 020 - ISBN
  isbn: {
    numero: "020",
    subcampo: "a",
    secao: "020 - ISBN",
    descricao: "Número do ISBN",
  },
  isbn_c: {
    numero: "020",
    subcampo: "c",
    secao: "020 - ISBN",
    descricao: "Termos de disponibilidade",
  },

  // Campo 040 - Fonte da Catalogação
  agen_Catalogadora: {
    numero: "040",
    subcampo: "a",
    secao: "040 - Fonte da Catalogação",
    descricao: "Agência catalogadora original",
  },
  idi_Catalogacao: {
    numero: "040",
    subcampo: "b",
    secao: "040 - Fonte da Catalogação",
    descricao: "Idioma da catalogação",
  },
  agen_Transcricao: {
    numero: "040",
    subcampo: "c",
    secao: "040 - Fonte da Catalogação",
    descricao: "Agência de transcrição",
  },
  agen_Modificacao: {
    numero: "040",
    subcampo: "d",
    secao: "040 - Fonte da Catalogação",
    descricao: "Agência de modificação",
  },

  // Campo 041 - Código de Idioma
  idi_Texto: {
    numero: "041",
    subcampo: "a",
    secao: "041 - Código de Idioma",
    descricao: "Código de idioma do texto ou trilha sonora",
  },
  idi_Resumo: {
    numero: "041",
    subcampo: "b",
    secao: "041 - Código de Idioma",
    descricao: "Código de idioma do resumo ou abstract",
  },
  idi_Legenda: {
    numero: "041",
    subcampo: "j",
    secao: "041 - Código de Idioma",
    descricao: "Código de idioma das legendas ou títulos",
  },

  // Campo 082 - Classificação CDD
  numero_CDD: {
    numero: "082",
    subcampo: "a",
    secao: "082 - Classificação CDD",
    descricao: "Número da classificação CDD",
  },
  numero_Item_CDD: {
    numero: "082",
    subcampo: "b",
    secao: "082 - Classificação CDD",
    descricao: "Número da edição da CDD",
  },

  // Campo 090 - Classificação Local
  num_Cham_Local: {
    numero: "090",
    subcampo: "a",
    secao: "090 - Classificação Local",
    descricao: "Classificação local",
  },
  num_Item_Local: {
    numero: "090",
    subcampo: "b",
    secao: "090 - Classificação Local",
    descricao: "Número da classificação local",
  },

  // Campo 095 - Classificação Local Secundária
  num_Cham_Secundaria: {
    numero: "095",
    subcampo: "a",
    secao: "095 - Classificação Local Secundária",
    descricao: "Classificação local secundária",
  },

  // Campo 100 - Autor Principal
  nome_Pess_Assunto: {
    numero: "100",
    subcampo: "a",
    secao: "100 - Autor Principal",
    descricao: "Nome do autor principal",
  },
  autor_numero: {
    numero: "100",
    subcampo: "b",
    secao: "100 - Autor Principal",
    descricao: "Número do autor",
  },
  datas_Pessoais: {
    numero: "100",
    subcampo: "d",
    secao: "100 - Autor Principal",
    descricao: "Datas do autor",
  },
  funcao_Pessoal: {
    numero: "100",
    subcampo: "e",
    secao: "100 - Autor Principal",
    descricao: "Função do autor",
  },

  // Campo 110 - Entidade Corporativa
  entidade_nome: {
    numero: "110",
    subcampo: "a",
    secao: "110 - Entidade Corporativa",
    descricao: "Nome da entidade corporativa",
  },
  entidade_subordinada: {
    numero: "110",
    subcampo: "b",
    secao: "110 - Entidade Corporativa",
    descricao: "Subdivisão da entidade",
  },

  // Campo 245 - Título
  nome: {
    numero: "245",
    subcampo: "a",
    secao: "245 - Título",
    descricao: "Título principal",
  },
  subtitulo: {
    numero: "245",
    subcampo: "b",
    secao: "245 - Título",
    descricao: "Subtítulo",
  },
  indi_Responsabilidade: {
    numero: "245",
    subcampo: "c",
    secao: "245 - Título",
    descricao: "Indicação de responsabilidade",
  },
  indi_Arti_Inicial: {
    numero: "245",
    subcampo: "",
    secao: "245 - Título",
    descricao: "Indicador de título",
  },

  // Campo 250 - Edição
  num_Edicao: {
    numero: "250",
    subcampo: "a",
    secao: "250 - Edição",
    descricao: "Indicação de edição",
  },
  mencao_Responsa_Edicao: {
    numero: "250",
    subcampo: "b",
    secao: "250 - Edição",
    descricao: "Menção de responsabilidade da edição",
  },

  // Campo 260 - Publicação
  local_Publicacao: {
    numero: "260",
    subcampo: "a",
    secao: "260 - Publicação",
    descricao: "Local de publicação",
  },
  editora: {
    numero: "260",
    subcampo: "b",
    secao: "260 - Publicação",
    descricao: "Nome da editora",
  },
  ano_Publicacao: {
    numero: "260",
    subcampo: "c",
    secao: "260 - Publicação",
    descricao: "Ano de publicação",
  },

  // Campo 300 - Descrição Física
  paginas: {
    numero: "300",
    subcampo: "a",
    secao: "300 - Descrição Física",
    descricao: "Extensão (número de páginas)",
  },
  ilustracoes: {
    numero: "300",
    subcampo: "b",
    secao: "300 - Descrição Física",
    descricao: "Outras características físicas",
  },
  dimensoes: {
    numero: "300",
    subcampo: "c",
    secao: "300 - Descrição Física",
    descricao: "Dimensões",
  },
  material_Adicional: {
    numero: "300",
    subcampo: "e",
    secao: "300 - Descrição Física",
    descricao: "Material adicional",
  },

  // Campo 490 - Série
  titulo_Serie: {
    numero: "490",
    subcampo: "a",
    secao: "490 - Série",
    descricao: "Título da série",
  },
  num_Serie: {
    numero: "490",
    subcampo: "v",
    secao: "490 - Série",
    descricao: "Número da série",
  },

  // Campo 500 - Notas Gerais
  notas_Gerais: {
    numero: "500",
    subcampo: "a",
    secao: "500 - Notas Gerais",
    descricao: "Nota geral",
  },
  info_Local: {
    numero: "500",
    subcampo: "a",
    secao: "500 - Notas Gerais",
    descricao: "Nota geral",
  },

  // Campo 600 - Assunto - Nome Pessoal
  nome_Pess_Assunto: {
    numero: "600",
    subcampo: "a",
    secao: "600 - Assunto - Nome Pessoal",
    descricao: "Nome pessoal",
  },
  datas_Pessoais: {
    numero: "600",
    subcampo: "d",
    secao: "600 - Assunto - Nome Pessoal",
    descricao: "Datas associadas",
  },
  funcao_Pessoal: {
    numero: "600",
    subcampo: "e",
    secao: "600 - Assunto - Nome Pessoal",
    descricao: "Função",
  },
  topico: {
    numero: "600",
    subcampo: "x",
    secao: "600 - Assunto - Nome Pessoal",
    descricao: "Tópico geral",
  },

  // Campo 630 - Assunto - Título Uniforme
  titulo_Uniforme: {
    numero: "630",
    subcampo: "a",
    secao: "630 - Assunto - Título Uniforme",
    descricao: "Título uniforme",
  },
  forma_Uniforme: {
    numero: "630",
    subcampo: "v",
    secao: "630 - Assunto - Título Uniforme",
    descricao: "Forma do item",
  },
  periodo_Historico: {
    numero: "630",
    subcampo: "y",
    secao: "630 - Assunto - Título Uniforme",
    descricao: "Período cronológico",
  },
  local_Uniforme: {
    numero: "630",
    subcampo: "z",
    secao: "630 - Assunto - Título Uniforme",
    descricao: "Local geográfico",
  },

  // Campo 650 - Assunto - Termo Tópico
  assunto_Termo: {
    numero: "650",
    subcampo: "a",
    secao: "650 - Assunto - Termo Tópico",
    descricao: "Termo tópico",
  },
  forma_Termo: {
    numero: "650",
    subcampo: "v",
    secao: "650 - Assunto - Termo Tópico",
    descricao: "Forma do item",
  },
  periodo_Histo_Termo: {
    numero: "650",
    subcampo: "y",
    secao: "650 - Assunto - Termo Tópico",
    descricao: "Período cronológico",
  },
  local_Termo: {
    numero: "650",
    subcampo: "z",
    secao: "650 - Assunto - Termo Tópico",
    descricao: "Local geográfico",
  },

  // Campo 700 - Autor Adicional
  autor_adicional: {
    numero: "700",
    subcampo: "a",
    secao: "700 - Autor Adicional",
    descricao: "Nome do autor adicional",
  },
  autores: {
    numero: "700",
    subcampo: "a",
    secao: "700 - Autor Adicional",
    descricao: "Nome do autor adicional",
  },
  autor_adicional_datas: {
    numero: "700",
    subcampo: "d",
    secao: "700 - Autor Adicional",
    descricao: "Datas do autor adicional",
  },
  autor_adicional_funcao: {
    numero: "700",
    subcampo: "e",
    secao: "700 - Autor Adicional",
    descricao: "Função do autor adicional",
  },

  // Campo 949 - Controle Local
  controle_local: {
    numero: "949",
    subcampo: "a",
    secao: "949 - Controle Local",
    descricao: "Controle local",
  },
  status_Item: {
    numero: "949",
    subcampo: "s",
    secao: "949 - Controle Local",
    descricao: "Status do controle",
  },
  status_Emprestimo: {
    numero: "949",
    subcampo: "s",
    secao: "949 - Controle Local",
    descricao: "Status de empréstimo",
  },

  // Exemplares
  exemplares_numero: {
    numero: "EX",
    subcampo: "",
    secao: "Exemplares",
    descricao: "Número do exemplar",
  },
  exemplares_numeros_volume: {
    numero: "EX",
    subcampo: "",
    secao: "Exemplares",
    descricao: "Números do volume",
  },
  exemplares_quantidade_volume: {
    numero: "EX",
    subcampo: "",
    secao: "Exemplares",
    descricao: "Quantidade de volumes",
  },
  exemplares_data: {
    numero: "EX",
    subcampo: "",
    secao: "Exemplares",
    descricao: "Data do exemplar",
  },
  exemplares_biblioteca: {
    numero: "EX",
    subcampo: "",
    secao: "Exemplares",
    descricao: "Biblioteca do exemplar",
  },
  exemplares_aquisicao: {
    numero: "EX",
    subcampo: "",
    secao: "Exemplares",
    descricao: "Modo de aquisição",
  },
};

// Busca e exibe os detalhes MARC21
async function carregarMarc21Completo(idLivro) {
  const detalhesDiv = document.getElementById("detalhesMarc21");
  detalhesDiv.innerHTML = `<p>Carregando...</p>`;

  try {
    // Busca o livro específico
    const response = await fetch(`https://localhost:7139/Livro/${idLivro}`);
    if (!response.ok) throw new Error("Erro ao buscar livro");
    const livro = await response.json();

    // Busca todos os autores e entidades
    const responseAutor = await fetch(`https://localhost:7139/Livro_Autor`);
    const responseEntidade = await fetch(`https://localhost:7139/Livro_Entidade`);
    
    if (!responseAutor.ok) throw new Error("Erro ao buscar Autor");
    if (!responseEntidade.ok) throw new Error("Erro ao buscar Entidade");

    const todosAutores = await responseAutor.json();
    const todasEntidades = await responseEntidade.json();

    // Filtra autores e entidades relacionados a este livro
    const autoresDoLivro = todosAutores.filter(a => a.idLivro === idLivro);
    const entidadesDoLivro = todasEntidades.filter(e => e.idLivro === idLivro);

    console.log("Livro:", livro);
    console.log("Autores do livro:", autoresDoLivro);
    console.log("Entidades do livro:", entidadesDoLivro);

    // Organiza campos do livro por número MARC21
    const camposPorNumero = {};

    // Inicializa todos os campos do MAPA_MARC21
    Object.entries(MAPA_MARC21).forEach(([campo, info]) => {
      const numero = info.numero;
      if (!camposPorNumero[numero]) {
        camposPorNumero[numero] = { secao: info.secao, subcampos: [] };
      }
      
      const valor = livro[campo] || null;
      camposPorNumero[numero].subcampos.push({
        subcampo: info.subcampo,
        descricao: info.descricao,
        campo,
        valor,
      });
    });

    // Renderiza o HTML
    let html = "<h2>Informações do Livro</h2>";
    
    // Renderiza campos MARC21 do livro
    Object.entries(camposPorNumero)
      .sort(([a], [b]) => a.localeCompare(b, undefined, { numeric: true }))
      .forEach(([numero, data]) => {
        const titulo = `${numero} - ${data.secao.split(" - ")[1] || data.secao}`;
        html += `<h3>${titulo}</h3>`;

        data.subcampos.forEach((sub) => {
          const label = sub.subcampo ? `‡${sub.subcampo}` : "Indicador";
          const descricao = sub.descricao;
          const valorFormatado = sub.valor
            ? Array.isArray(sub.valor)
              ? sub.valor.join(", ")
              : sub.valor
            : "Não informado";
          html += `<p><strong>${label} - ${descricao}:</strong> ${valorFormatado}</p>`;
        });
      });

    // Renderiza informações dos autores
    if (autoresDoLivro.length > 0) {
      html += "<h2>Autores</h2>";
      autoresDoLivro.forEach((autor, index) => {
        html += `<h3>Autor ${index + 1}</h3>`;
        Object.entries(autor).forEach(([chave, valor]) => {
          if (chave !== 'idLivro') {
            const valorFormatado = valor !== null && valor !== undefined ? valor : "Não informado";
            html += `<p><strong>${chave}:</strong> ${valorFormatado}</p>`;
          }
        });
      });
    } else {
      html += "<h2>Autores</h2><p>Nenhum autor associado a este livro.</p>";
    }

    // Renderiza informações das entidades
    if (entidadesDoLivro.length > 0) {
      html += "<h2>Entidades</h2>";
      entidadesDoLivro.forEach((entidade, index) => {
        html += `<h3>Entidade ${index + 1}</h3>`;
        Object.entries(entidade).forEach(([chave, valor]) => {
          if (chave !== 'idLivro') {
            const valorFormatado = valor !== null && valor !== undefined ? valor : "Não informado";
            html += `<p><strong>${chave}:</strong> ${valorFormatado}</p>`;
          }
        });
      });
    } else {
      html += "<h2>Entidades</h2><p>Nenhuma entidade associada a este livro.</p>";
    }

    detalhesDiv.innerHTML = html;
  } catch (error) {
    detalhesDiv.innerHTML = `<p style="color: red;">Erro: ${error.message}</p>`;
  }
}

// async function carregarMarc21Completo(idLivro) {
//   const detalhesDiv = document.getElementById("detalhesMarc21");
//   detalhesDiv.innerHTML = `<p>Carregando...</p>`;

//   try {
//     const response = await fetch(`https://localhost:7139/Livro/${idLivro}`);
//     const responseAutor = await fetch(
//       `https://localhost:7139/Livro_Autor`
//     );
//     const responseEntidade = await fetch(
//       `https://localhost:7139/Livro_Entidade`
//     )

//     if (!response.ok) throw new Error("Erro ao buscar livro");
//     if (!responseAutor.ok) throw new Error("Erro ao buscar Autor");
//     if (!responseEntidade.ok) throw new Error("Erro ao buscar Entidade");


//     const livro = await response.json();
//     const autor = await responseAutor.json();
//     const entidade = await responseEntidade.json();

//     console.log(autor);
//     console.log(entidade);
//     console.log(livro);

//     // Organiza por número do campo MARC21, incluindo todos os campos do MAPA_MARC21
//     const camposPorNumero = {};

//     // Primeiro, inicializa todos os campos do MAPA_MARC21
//     Object.entries(MAPA_MARC21).forEach(([campo, info]) => {
//       const numero = info.numero;
//       if (!camposPorNumero[numero]) {
//         camposPorNumero[numero] = { secao: info.secao, subcampos: [] };
//       }
//       // Adiciona o subcampo, mesmo se não tiver valor
//       const valor = livro[campo] || null;
//       camposPorNumero[numero].subcampos.push({
//         subcampo: info.subcampo,
//         descricao: info.descricao,
//         campo,
//         valor,
//       });
//     });

//     // Renderiza o HTML
//     let html = "";
//     // Ordena os campos por número crescente (numericamente)
//     Object.entries(camposPorNumero)
//       .sort(([a], [b]) => a.localeCompare(b, undefined, { numeric: true }))
//       .forEach(([numero, data]) => {
//         // Título do campo (ex: "020 - ISBN") com cor azul
//         const titulo = `${numero} - ${
//           data.secao.split(" - ")[1] || data.secao
//         }`;
//         html += `<h3>${titulo}</h3>`;

//         // Lista os subcampos
//         data.subcampos.forEach((sub) => {
//           const label = sub.subcampo ? `‡${sub.subcampo}` : "Indicador";
//           const descricao = sub.descricao;
//           const valorFormatado = sub.valor
//             ? Array.isArray(sub.valor)
//               ? sub.valor.join(", ")
//               : sub.valor
//             : "Não informado";
//           html += `<p><strong>${label} - ${descricao}:</strong> ${valorFormatado}</p>`;
//         });
//       });

//     detalhesDiv.innerHTML = html;
//   } catch (error) {
//     detalhesDiv.innerHTML = `<p>Erro: ${error.message}</p>`;
//   }
//}

async function buscarLivros() {
  const termo = document.getElementById("busca").value.trim();
  const resultadosDiv = document.getElementById("resultados");
  const detalhesDiv = document.getElementById("detalhesMarc21");

  resultadosDiv.innerHTML = "";
  detalhesDiv.innerHTML =
    "<p>Selecione um livro para ver os detalhes MARC21.</p>";

  try {
    const response = await fetch(
      `https://localhost:7139/Livro/search?termo=${encodeURIComponent(termo)}`
    );
    if (!response.ok) throw new Error("Erro ao buscar livros");

    const livros = await response.json();

    if (livros.length === 0) {
      resultadosDiv.innerHTML = "<p>Nenhum livro encontrado.</p>";
      return;
    }

    livros.forEach((livro) => {
      const card = document.createElement("div");
      card.className = "livro-card";
      card.innerHTML = `
                <strong>${livro.nome_Livro}</strong> ${
        livro.subtitulo ? livro.subtitulo + "<br>" : ""
      }
                (${livro.ano_Publicacao || "Ano não informado"})<br>
                <strong>Autor/Responsável:</strong> ${
                  livro.indicacao_Responsabilidade || "Não informado"
                }<br>
                <strong>Autores:</strong> ${
                  livro.autores || "Não informado"
                }<br>
                <strong>ISBN:</strong> ${livro.isbn || "Não informado"}<br>
                <strong>Assunto:</strong> ${
                  livro.assunto_Termo || "Não informado"
                }<br>
                <strong>Status:</strong> ${
                  livro.status_Emprestimos || "Não informado"
                }
                <button class="btn-verMarc">Ver MARC21</button>
            `;

      card
        .querySelector(".btn-verMarc")
        .addEventListener("click", () =>
          carregarMarc21Completo(livro.id_Livro)
        );

      resultadosDiv.appendChild(card);
    });
  } catch (error) {
    resultadosDiv.innerHTML = `<p style="color:red;">${error.message}</p>`;
    console.error(error);
  }

  // Função de limpar busca do acervo
  async function limparBuscar() {
    document.getElementById("busca").value = "";
    document.getElementById("resultados").innerHTML = "";
    showNotification("Busca limpa com sucesso!", "info");
  }

  /* ===== EMPRÉSTIMO ===== */

  // Buscar livros para autocomplete
  let livrosCache = [];
  let timeoutBusca;

  async function buscarLivrosAutocomplete(termo) {
    const sugestoesDiv = document.getElementById("livroSugestoes");
    clearTimeout(timeoutBusca);

    if (!termo || termo.length < 2) {
      sugestoesDiv.innerHTML = "";
      return;
    }

    timeoutBusca = setTimeout(async () => {
      try {
        const response = await fetch(
          `https://localhost:7139/Livro/search?termo=${encodeURIComponent(
            termo
          )}`
        );

        if (!response.ok) {
          throw new Error("Erro ao buscar livros");
        }

        const livros = await response.json();
        livrosCache = livros;

        sugestoesDiv.innerHTML = "";

        if (livros.length === 0) {
          sugestoesDiv.innerHTML =
            '<div class="autocomplete-item no-results">Nenhum livro encontrado</div>';
          return;
        }

        // Criar lista de sugestões
        livros.forEach((livro) => {
          const item = document.createElement("div");
          item.className = "autocomplete-item";
          item.innerHTML = `
                    <strong>${livro.nome_Livro}</strong>
                    <small>
                        ${livro.autores ? `Autor: ${livro.autores} | ` : ""}
                        ISBN: ${livro.isbn || "N/A"} | 
                        Status: ${livro.status_Emprestimos || "Disponível"}
                    </small>
                `;

          item.onclick = () => selecionarLivro(livro);

          sugestoesDiv.appendChild(item);
        });
      } catch (error) {
        console.error("Erro ao buscar livros:", error);
        sugestoesDiv.innerHTML =
          '<div class="autocomplete-item no-results">Erro ao buscar livros</div>';
      }
    }, 300);
  }

  function selecionarLivro(livro) {
    // Preencher o campo com o nome do livro
    document.getElementById("livro").value = livro.nome_Livro;

    // Guardar o ID do livro em um campo hidden
    document.getElementById("livroIdSelecionado").value = livro.id_Livro;

    document.getElementById("livroSugestoes").innerHTML = "";
  }

  // Fechar sugestões ao clicar fora
  document.addEventListener("click", function (e) {
    const container = document.querySelector(".autocomplete-container");
    if (container && !container.contains(e.target)) {
      document.getElementById("livroSugestoes").innerHTML = "";
    }
  });

  function addEmprestimo() {
    const livroId = document.getElementById("livroIdSelecionado").value;

    if (!livroId) {
      alert("Por favor, selecione um livro da lista de sugestões!");
      return;
    }

    const emprestimo = {
      RM_Aluno: document.getElementById("RM").value,
      Id_Livro: parseInt(livroId),
      Data_Emprestimo: document.getElementById("dataEmprestimo").value,
      Data_Devolucao_Prevista: document.getElementById("dataDevolucao").value,
      Data_Devolucao_Real: null,
    };

    fetch("https://localhost:7139/Emprestimo", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(emprestimo),
    })
      .then((response) => {
        if (response.ok) {
          alert("Empréstimo cadastrado com sucesso!");
          document.getElementById("formEmprestimo").reset();
          document.getElementById("livroIdSelecionado").value = "";
          closeModal("addEmprestimo");
          getEmprestimos(); // Se você tiver uma função para listar empréstimos
        } else {
          response.text().then((text) => {
            alert("Erro ao cadastrar empréstimo: " + text);
          });
        }
      })
      .catch((error) => {
        console.error("Erro:", error);
        alert("Erro ao conectar com o servidor!");
      });
  }

  // Função para buscar e exibir
  function getEmprestimos() {
    fetch("https://localhost:7139/Emprestimo")
      .then((response) => {
        if (!response.ok) {
          throw new Error("Erro ao buscar empréstimos");
        }
        return response.json();
      })
      .then((data) => {
        const emprestimoTable = document.getElementById("emprestimoTable");
        const tbody = emprestimoTable.querySelector("tbody");
        tbody.innerHTML = "";

        if (data.length === 0) {
          tbody.innerHTML =
            '<tr><td colspan="8" style="text-align:center;">Nenhum empréstimo encontrado</td></tr>';
          return;
        }

        data.forEach((emprestimo) => {
          const row = document.createElement("tr");

          const idCell = document.createElement("td");
          idCell.textContent = emprestimo.id_Emprestimo;
          row.appendChild(idCell);

          const alunoCell = document.createElement("td");
          alunoCell.textContent = `${emprestimo.nomeAluno} (${emprestimo.rm_Aluno})`;
          row.appendChild(alunoCell);

          const livroCell = document.createElement("td");
          livroCell.textContent = emprestimo.nomeLivro;
          row.appendChild(livroCell);

          const dataEmpCell = document.createElement("td");
          dataEmpCell.textContent = formatarData(emprestimo.data_Emprestimo);
          row.appendChild(dataEmpCell);

          const dataPrevCell = document.createElement("td");
          dataPrevCell.textContent = formatarData(
            emprestimo.data_Devolucao_Prevista
          );
          row.appendChild(dataPrevCell);

          const dataRealCell = document.createElement("td");
          if (emprestimo.data_Devolucao_Real) {
            dataRealCell.textContent = formatarData(
              emprestimo.data_Devolucao_Real
            );

            const dataReal = new Date(emprestimo.data_Devolucao_Real);
            const dataPrevista = new Date(emprestimo.data_Devolucao_Prevista);

            if (dataReal > dataPrevista) {
              dataRealCell.style.color = "#a20c0cff";
              dataRealCell.style.fontWeight = "600";
            } else {
              dataRealCell.style.color = "#0d7c11ff";
              dataRealCell.style.fontWeight = "600";
            }
          } else {
            dataRealCell.textContent = "-";
            dataRealCell.style.color = "#999";
          }

          row.appendChild(dataRealCell);

          const statusCell = document.createElement("td");
          const status = calcularStatus(
            emprestimo.data_Devolucao_Prevista,
            emprestimo.data_Devolucao_Real
          );
          statusCell.innerHTML = `<span class="status-badge ${status.classe}">${status.texto}</span>`;
          row.appendChild(statusCell);

          const actionCell = document.createElement("td");
          actionCell.innerHTML = `
                    <div class="action">
                        ${
                          emprestimo.data_Devolucao_Real === null
                            ? `<button class="btn-devolucao" onclick="registrarDevolucao(${emprestimo.id_Emprestimo})">Devolver</button>`
                            : ""
                        }
                        <button class="btn-delete" onclick="abrirExclusaoEmprestimo(${
                          emprestimo.id_Emprestimo
                        })">Excluir</button>
                    </div>
                `;
          row.appendChild(actionCell);

          tbody.appendChild(row);
        });
      })
      .catch((error) => {
        console.error("Erro ao buscar empréstimos:", error);
        showNotification("Erro ao carregar empréstimos", "error");
      });
  }

  function formatarData(dataString) {
    if (!dataString) return "-";
    const data = new Date(dataString);
    return data.toLocaleDateString("pt-BR", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
    });
  }

  function calcularStatus(dataDevolucaoPrevista, dataDevolucaoReal) {
    const hoje = new Date();
    hoje.setHours(0, 0, 0, 0);
    const dataPrevista = new Date(dataDevolucaoPrevista);
    dataPrevista.setHours(0, 0, 0, 0);

    if (dataDevolucaoReal) {
      return { texto: "Devolvido", classe: "status-devolvido" };
    }

    if (hoje > dataPrevista) {
      return { texto: "Atrasado", classe: "status-atrasado" };
    }

    return { texto: "Ativo", classe: "status-ativo" };
  }

  function registrarDevolucao(idEmprestimo) {
    if (!confirm("Confirmar devolução do livro?")) return;

    const dataAtual = new Date().toISOString().split("T")[0];

    fetch(`https://localhost:7139/Emprestimo/${idEmprestimo}/devolver`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ dataDevolucao: dataAtual }),
    })
      .then((response) => {
        if (response.ok) {
          showNotification("Devolução registrada com sucesso!", "success");
          getEmprestimos(); // Recarrega a tabela
        } else {
          response.text().then((text) => {
            showNotification(text || "Erro ao registrar devolução", "error");
          });
        }
      })
      .catch((error) => {
        console.error("Erro:", error);
        showNotification("Erro ao conectar com o servidor", "error");
      });
  }

  function abrirExclusaoEmprestimo(id) {
    document.getElementById("deleIdEmprestimo").value = id;
    openModal("deleteModalEmprestimo");
  }

  function deleteEmprestimo() {
    const id = parseInt(document.getElementById("deleIdEmprestimo").value);

    fetch(`https://localhost:7139/Emprestimo/${id}`, {
      method: "DELETE",
    })
      .then((response) => {
        if (response.ok) {
          showNotification("Empréstimo excluído com sucesso!", "success");
          getEmprestimos();
          closeModal("deleteModalEmprestimo");
        } else {
          showNotification("Erro ao excluir empréstimo", "error");
        }
      })
      .catch((error) => {
        console.error("Erro:", error);
        showNotification("Erro ao conectar com o servidor", "error");
      });
  }

  if (window.location.pathname.includes("emprestimo")) {
    getEmprestimos();
  }

  /* ===== ANCORA ===== */

  document.addEventListener("DOMContentLoaded", () => {
    const btnTopo = document.getElementById("btnTopo");

    function verificarRolagem() {
      const scrolled = window.scrollY;
      const totalScroll =
        document.documentElement.scrollHeight - window.innerHeight;

      if (totalScroll > 0 && scrolled > 0) {
        btnTopo.classList.add("mostrar");
      } else {
        btnTopo.classList.remove("mostrar");
      }
    }

    btnTopo.addEventListener("click", (e) => {
      e.preventDefault();
      window.scrollTo({
        top: 0,
        behavior: "smooth",
      });
    });

    window.addEventListener("scroll", verificarRolagem);
    window.addEventListener("resize", verificarRolagem);
    verificarRolagem();
  });
}