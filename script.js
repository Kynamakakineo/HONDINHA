const API_URL = "http://localhost:3000"; // ⚠️ TROQUE PELA URL DO SEU BACKEND

const listaConcessionarias = document.getElementById('lista-concessionarias');
const listaCarros = document.getElementById('lista-carros');
const cardDestaque = document.getElementById('card-destaque');

// 1. Função executada assim que a tela carrega
async function carregarConcessionarias() {
    try {
        const response = await fetch(`${API_URL}/concessionarias`);
        const concessionarias = await response.json();

        listaConcessionarias.innerHTML = ''; // Limpa tabela

        concessionarias.forEach(loja => {
            const row = document.createElement('tr');
            
            // Adiciona classe para o CSS estilizar o cursor
            row.classList.add('linha-clicavel');

            row.innerHTML = `
                <td>${loja.nome}</td> <td>${loja.cidade}</td>
                <td>${loja.estado}</td>
            `;

            // EVENTO DE CLIQUE: Aqui acontece a mágica
            row.addEventListener('click', () => {
                // Remove destaque visual das outras linhas
                document.querySelectorAll('.linha-clicavel').forEach(r => r.classList.remove('selecionada'));
                row.classList.add('selecionada');

                // Chama a função para carregar os carros desta loja específica
                carregarCarros(loja.id); 
            });

            listaConcessionarias.appendChild(row);
        });
    } catch (error) {
        console.error("Erro ao buscar concessionárias:", error);
    }
}

// 2. Função para buscar carros de uma concessionária específica
async function carregarCarros(idConcessionaria) {
    try {
        // ⚠️ Ajuste a rota conforme seu backend. 
        // Pode ser: /concessionarias/${idConcessionaria}/carros 
        // Ou: /carros?concessionariaId=${idConcessionaria}
        const response = await fetch(`${API_URL}/carros?concessionariaId=${idConcessionaria}`);
        const carros = await response.json();

        listaCarros.innerHTML = ''; // Limpa a tabela de carros anterior

        if (carros.length === 0) {
            listaCarros.innerHTML = '<tr><td colspan="4">Nenhum carro disponível nesta unidade.</td></tr>';
            return;
        }

        carros.forEach(carro => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td><img src="images/${carro.imagemUrl}" class="carro-thumbnail" alt="${carro.modelo}"></td>
                <td>${carro.modelo}</td>
                <td>${carro.ano}</td>
                <td>R$ ${carro.preco}</td>
            `;

            // Se clicar no carro, exibe no destaque (código que você já tinha)
            row.addEventListener('click', () => {
                atualizarDestaque(carro);
            });

            listaCarros.appendChild(row);
        });
    } catch (error) {
        console.error("Erro ao buscar carros:", error);
    }
}

// 3. Função para atualizar o card de destaque (baseado no seu código)
function atualizarDestaque(carro) {
    cardDestaque.innerHTML = `
        <h3>${carro.modelo}</h3>
        <img src="images/${carro.imagemUrl}" alt="${carro.modelo}" class="imagem-destaque">
        <p><strong>Ano:</strong> ${carro.ano}</p>
        <p><strong>Preço:</strong> R$ ${carro.preco}</p>
        <p class="descricao-carro">${carro.descricao || 'Sem descrição disponível.'}</p>
    `;
}

// Inicia o processo
carregarConcessionarias();