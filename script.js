// --- CONFIGURAÇÃO ---

const API_BASE_URL = 'http://localhost:5076'; // Verifique se a porta é essa mesma
 
let todasConcessionarias = [];

let todosCarros = [];
 
// Elementos da DOM

const selectDealer = document.getElementById('select-dealership');

const selectCar = document.getElementById('select-model');

const displayDiv = document.getElementById('car-display');

const dealerImg = document.getElementById('dealer-img');

const carImg = document.getElementById('car-img-real');

const selectDealerModal = document.getElementById('concessionariaIdModal');
 
// --- INICIALIZAÇÃO ---

async function init() {

    try {

        console.log("Conectando em:", API_BASE_URL);

        const [resLojas, resCarros] = await Promise.all([

            fetch(`${API_BASE_URL}/api/Concessionarias`),

            fetch(`${API_BASE_URL}/api/Carros`)

        ]);
 
        if (!resLojas.ok || !resCarros.ok) throw new Error("Erro na API");
 
        todasConcessionarias = await resLojas.json();

        todosCarros = await resCarros.json();
 
        preencherMenuLojas();

        preencherMenuModal();
 
    } catch (error) {

        console.error("Erro:", error);

    }

}
 
// Preenche Select Principal

function preencherMenuLojas() {

    if (!selectDealer) return;

    selectDealer.innerHTML = '<option value="">-- Selecione uma unidade --</option>';

    todasConcessionarias.forEach(loja => {

        const option = document.createElement('option');

        option.value = loja.id;

        option.textContent = loja.nome;

        selectDealer.appendChild(option);

    });

}
 
// Preenche Select do Modal

function preencherMenuModal() {

    if (!selectDealerModal) return;

    selectDealerModal.innerHTML = '<option value="">Selecione a Loja</option>';

    todasConcessionarias.forEach(loja => {

        const option = document.createElement('option');

        option.value = loja.id;

        option.textContent = loja.nome;

        selectDealerModal.appendChild(option);

    });

}
 
// --- EVENTOS ---

if (selectDealer) {

    selectDealer.addEventListener('change', (e) => {

        const idLoja = parseInt(e.target.value);

        selectCar.innerHTML = '<option value="">-- Selecione o Modelo --</option>';

        selectCar.disabled = true;

        displayDiv.style.display = 'none';

        dealerImg.style.display = 'none';
 
        if (!idLoja) return;
 
        const loja = todasConcessionarias.find(l => l.id === idLoja);

        // CORREÇÃO: Verifica se tem imagemUrl e monta o link completo

        if (loja && loja.imageUrl) { // Lojas geralmente usam imageUrl (verifique se não é imagemUrl tb)

            // Se não começar com http, adiciona o dominio da API

            const src = loja.imageUrl.startsWith('http') ? loja.imageUrl : `${API_BASE_URL}/${loja.imageUrl}`;

            dealerImg.src = src;

            dealerImg.style.display = 'block';

        }
 
        const carrosDaLoja = todosCarros.filter(c => c.concessionariaId === idLoja);

        if (carrosDaLoja.length > 0) {

            carrosDaLoja.forEach(carro => {

                const option = document.createElement('option');

                option.value = carro.id;

                option.textContent = `${carro.marca} ${carro.modelo}`;

                selectCar.appendChild(option);

            });

            selectCar.disabled = false;

        } else {

            selectCar.innerHTML = '<option>Sem estoque</option>';

        }

    });

}
 
if (selectCar) {

    selectCar.addEventListener('change', (e) => {

        const idCarro = parseInt(e.target.value);

        const carro = todosCarros.find(c => c.id === idCarro);

        if (carro) {

            document.getElementById('car-modelo').textContent = `${carro.marca} ${carro.modelo}`;

            document.getElementById('car-ano').textContent = carro.ano;

            document.getElementById('car-cor').textContent = carro.cor;

            document.getElementById('car-price').textContent = new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(carro.preco);
 
            // --- CORREÇÃO DA IMAGEM ---

            // 1. Pega o valor que veio do banco (imagemUrl com 'm')

            const caminhoImagem = carro.imagemUrl;
 
            if (caminhoImagem) {

                if (caminhoImagem.startsWith('http')) {

                    // Se for link da internet (https://...), usa direto

                    carImg.src = caminhoImagem;

                } else {

                    // Se for arquivo local (/images/...), coloca o endereço do servidor antes

                    carImg.src = `${API_BASE_URL}${caminhoImagem}`;

                }

            } else {

                carImg.src = 'https://via.placeholder.com/600x400?text=Sem+Foto';

            }

            // ---------------------------
 
            displayDiv.style.display = 'block';

        }

    });

}
 
// --- FUNÇÕES DE GERENCIAMENTO ---
 
// CADASTRAR

async function cadastrarCarro() {

    const novoCarro = {

        marca: document.getElementById('marca').value,

        modelo: document.getElementById('modelo').value,

        ano: parseInt(document.getElementById('ano').value),

        cor: document.getElementById('cor').value,

        preco: parseFloat(document.getElementById('preco').value),

        concessionariaId: parseInt(document.getElementById('concessionariaIdModal').value),

        // Envia como imagemUrl (Português) para o Backend aceitar

        imagemUrl: document.getElementById('imageUrl').value 

    };
 
    if (!novoCarro.marca || !novoCarro.concessionariaId) { return alert("Preencha Marca e Concessionária."); }
 
    try {

        const res = await fetch(`${API_BASE_URL}/api/Carros`, {

            method: 'POST',

            headers: { 'Content-Type': 'application/json' },

            body: JSON.stringify(novoCarro)

        });

        if (res.ok) { alert("Cadastrado!"); fecharModal(); init(); }

        else { alert("Erro ao cadastrar."); }

    } catch (err) { alert("Erro conexão."); }

}
 
// BUSCAR PARA EDITAR

async function buscarCarroParaEditar() {

    const id = document.getElementById('edit-search-id').value;

    if (!id) return alert("Digite o ID.");
 
    try {

        const res = await fetch(`${API_BASE_URL}/api/Carros/${id}`);

        if (res.ok) {

            const c = await res.json();

            document.getElementById('form-edit').style.display = 'block';

            document.getElementById('edit-id-hidden').value = c.id;

            document.getElementById('edit-marca').value = c.marca;

            document.getElementById('edit-modelo').value = c.modelo;

            document.getElementById('edit-ano').value = c.ano;

            document.getElementById('edit-cor').value = c.cor;

            document.getElementById('edit-preco').value = c.preco;

            document.getElementById('edit-concessionariaId').value = c.concessionariaId;

            // Preenche o campo de edição com o valor que veio do banco

            document.getElementById('edit-imageUrl').value = c.imagemUrl || '';
 
        } else { alert("Carro não encontrado."); }

    } catch (e) { alert("Erro ao buscar."); }

}
 
// SALVAR EDIÇÃO

async function salvarEdicao() {

    const id = document.getElementById('edit-id-hidden').value;
 
    const editado = {

        id: parseInt(id),

        marca: document.getElementById('edit-marca').value,

        modelo: document.getElementById('edit-modelo').value,

        ano: parseInt(document.getElementById('edit-ano').value),

        cor: document.getElementById('edit-cor').value,

        preco: parseFloat(document.getElementById('edit-preco').value),

        concessionariaId: parseInt(document.getElementById('edit-concessionariaId').value),

        // Envia como imagemUrl

        imagemUrl: document.getElementById('edit-imageUrl').value

    };
 
    try {

        const res = await fetch(`${API_BASE_URL}/api/Carros/${id}`, {

            method: 'PUT',

            headers: { 'Content-Type': 'application/json' },

            body: JSON.stringify(editado)

        });

        if (res.ok) { alert("Atualizado!"); fecharModal(); init(); } else { alert("Erro ao atualizar."); }

    } catch (e) { alert("Erro conexão."); }

}
 
// REMOVER

async function deletarCarro() {

    const id = document.getElementById('del-id').value;

    if (!id) return alert("Digite ID.");

    if (!confirm("Tem certeza?")) return;

    try {

        const res = await fetch(`${API_BASE_URL}/api/Carros/${id}`, { method: 'DELETE' });

        if (res.ok) { alert("Removido!"); fecharModal(); init(); } else { alert("Erro ao remover."); }

    } catch (e) { alert("Erro conexão."); }

}
 
// CONTROLE DO MODAL

function abrirModal() { document.getElementById('modalGerenciar').style.display = 'flex'; }

function fecharModal() { document.getElementById('modalGerenciar').style.display = 'none'; }

window.onclick = function(ev) { if (ev.target == document.getElementById('modalGerenciar')) fecharModal(); }
 
function mudarAba(aba) {

    ['add', 'edit', 'del'].forEach(a => {

        document.getElementById('tab-'+a).classList.remove('active');

        document.getElementById('btn-tab-'+a).classList.remove('active');

    });

    document.getElementById('tab-'+aba).classList.add('active');

    document.getElementById('btn-tab-'+aba).classList.add('active');

}
 
// Roda ao carregar

init();
 