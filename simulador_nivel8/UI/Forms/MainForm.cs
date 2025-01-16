using System;
using System.Linq;
using System.Windows.Forms;
using simulador_nivel8.Data;
using simulador_nivel8.Data.Models;

namespace simulador_nivel8.UI.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar formulário
                if (string.IsNullOrEmpty(txtNome.Text) || string.IsNullOrEmpty(txtFabricante.Text))
                {
                    MessageBox.Show("Preencha todos os campos corretamente!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtPrecoCompra.Text, out var precoCompra) || !decimal.TryParse(txtPrecoVenda.Text, out var precoVenda))
                {
                    MessageBox.Show("Preços devem ser números válidos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var novoProduto = new Produto
                {
                    nome_da_peca = txtNome.Text,
                    fabricante = txtFabricante.Text,
                    tipo_veiculo = txtTipoVeiculo.Text,
                    categoria = txtCategoria.Text,
                    preco_compra = decimal.Parse(txtPrecoCompra.Text),
                    preco_venda = decimal.Parse(txtPrecoVenda.Text)
                };

                ProdutoData.InsereProduto(novoProduto);

                MessageBox.Show("Peça cadastrada com sucesso!", "Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpar campos
                txtNome.Clear();
                txtFabricante.Clear();
                txtTipoVeiculo.Clear();
                txtCategoria.Clear();
                txtPrecoCompra.Clear();
                txtPrecoVenda.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cadastrar peça: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                // Verifica se o usuário digitou alguma coisa
                string textoConsulta = txtNome.Text.Trim();
                if (string.IsNullOrEmpty(textoConsulta))
                {
                    MessageBox.Show("Digite parte do nome do produto para consultar!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Chama o método de consulta passando o texto digitado
                var produtos = ProdutoData.ConsultaProdutosPorNomeParcial(textoConsulta);

                // Verifica se existem produtos retornados
                if (produtos != null && produtos.Count > 0)
                {
                    listProdutos.Items.Clear(); // Limpa a lista para evitar duplicados
                    foreach (var produto in produtos)
                    {
                        listProdutos.Items.Add($"ID: {produto.id}, Nome: {produto.nome_da_peca}, Fabricante: {produto.fabricante}, Preço: {produto.preco_venda:C}");
                    }
                }
                else
                {
                    MessageBox.Show("Nenhum produto encontrado!", "Consulta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao consultar produtos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnEditarSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnEditarSalvar.Text == "Editar") // Modo de edição
                {
                    if (!int.TryParse(txtIdProduto.Text, out var id))
                    {
                        MessageBox.Show("Digite um ID válido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var produto = ProdutoData.ConsultaProdutos("").FirstOrDefault(p => p.id == id);

                    if (produto != null)
                    {
                        txtNome.Text = produto.nome_da_peca;
                        txtFabricante.Text = produto.fabricante;
                        txtTipoVeiculo.Text = produto.tipo_veiculo;
                        txtCategoria.Text = produto.categoria;
                        txtPrecoCompra.Text = produto.preco_compra.ToString();
                        txtPrecoVenda.Text = produto.preco_venda.ToString();

                        btnEditarSalvar.Text = "Salvar"; // Alternar para modo de salvar
                        MessageBox.Show("Produto carregado para edição!", "Edição", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Produto não encontrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (btnEditarSalvar.Text == "Salvar") // Modo de salvar
                {
                    if (!int.TryParse(txtIdProduto.Text, out var id))
                    {
                        MessageBox.Show("Digite um ID válido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var produtoAtualizado = new Produto
                    {
                        nome_da_peca = txtNome.Text,
                        fabricante = txtFabricante.Text,
                        tipo_veiculo = txtTipoVeiculo.Text,
                        categoria = txtCategoria.Text,
                        preco_compra = decimal.Parse(txtPrecoCompra.Text),
                        preco_venda = decimal.Parse(txtPrecoVenda.Text)
                    };

                    ProdutoData.AtualizarProduto(id, produtoAtualizado);

                    MessageBox.Show("Produto atualizado com sucesso!", "Edição", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    btnEditarSalvar.Text = "Editar"; // Alternar de volta para modo de edição

                    // Limpar os campos após salvar
                    txtIdProduto.Clear();
                    txtNome.Clear();
                    txtFabricante.Clear();
                    txtTipoVeiculo.Clear();
                    txtCategoria.Clear();
                    txtPrecoCompra.Clear();
                    txtPrecoVenda.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao processar ação: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnDeletar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtIdProduto.Text, out var id))
                {
                    MessageBox.Show("Digite um ID válido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var produto = ProdutoData.DeletarProduto(id);

                if (produto != null)
                {
                    MessageBox.Show("Produto deletado com sucesso!", "Exclusão", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Produto não encontrado para exclusão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao deletar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
