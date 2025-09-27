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

