using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using simulador_nivel8.Data.Models;

namespace simulador_nivel8.Data
{
    public class ProdutoData
    {
        public static Produto InsereProduto(Produto p)
        {
            Produto produto = new Produto();
            var sessionfac = ConfigDB.SimpleConfig().BuildSessionFactory();

            using (var session = sessionfac.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    produto = new Produto { nome_da_peca = p.nome_da_peca, fabricante = p.fabricante, tipo_veiculo = p.tipo_veiculo, categoria = p.categoria, preco_compra = p.preco_compra, preco_venda = p.preco_venda };
                    session.Save(p);
                    transaction.Commit();
                }
            }
            return produto;

        }

        public static Produto DeletarProduto(int id)
        {
            Produto produto = new Produto();
            var sessionfac = ConfigDB.SimpleConfig().BuildSessionFactory();

            using (var session = sessionfac.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    produto = session.Get<Produto>(id);
                    session.Delete(produto);
                    transaction.Commit();
                }
            }
            return produto;
        }

        public static Produto AtualizarProduto(int id, Produto p)
        {
            Produto produto = new Produto();
            var sessionfac = ConfigDB.SimpleConfig().BuildSessionFactory();

            using (var session = sessionfac.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.CreateQuery("update Produto set Nome = :nome, Fabricante = :fabricante, TipoVeiculo = :tipoVeiculo, Categoria = :categoria, PrecoCompra = :precoCompra, PrecoVenda = :precoVenda where Id = :id");
                    query.SetParameter("nome", p.nome_da_peca);
                    query.SetParameter("fabricante", p.fabricante);
                    query.SetParameter("tipoVeiculo", p.tipo_veiculo);
                    query.SetParameter("categoria", p.categoria);
                    query.SetParameter("precoCompra", p.preco_compra);
                    query.SetParameter("precoVenda", p.preco_venda);
                    query.SetParameter("id", id);
                    int rowsUpdated = query.ExecuteUpdate();
                    transaction.Commit();
                }
            }
            return produto;
        }

        public static List<Produto> ConsultaProdutos(string nome)
        {
            List<Produto> produtos = new List<Produto>();
            var sessionfac = ConfigDB.SimpleConfig().BuildSessionFactory();

            using (var session = sessionfac.OpenSession())
            {
                var query = session.CreateQuery("from Produto p where p.Nome like :nome");
                query.SetParameter("nome", String.Format("%{0}%", nome));
                produtos = query.List<Produto>().ToList();
            }

            return produtos;

        }

        public static List<Produto> ConsultaProdutosPorNomeParcial(string nomeParcial)
        {
            // Ajuste a string de conexão para o seu banco de dados
            string connectionString = "Data Source=AMANDAPS\\SQLEXPRESS;Initial Catalog=gerenciador_de_pecas_veiculos;Trusted_Connection=True";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id, nome_da_peca, fabricante, tipo_veiculo, categoria, preco_compra, preco_venda FROM dbo.tb_pecas_veiculos WHERE nome_da_peca LIKE @NomeParcial";

                    using (var command = new SqlCommand(query, connection))
                    {
                        // Configura o parâmetro da consulta com busca parcial
                        command.Parameters.AddWithValue("@NomeParcial", $"%{nomeParcial}%");

                        using (var reader = command.ExecuteReader())
                        {
                            var produtos = new List<Produto>();
                            while (reader.Read())
                            {
                                produtos.Add(new Produto
                                {
                                    id = reader.GetInt32(0),
                                    nome_da_peca = reader.GetString(1),
                                    fabricante = reader.GetString(2),
                                    tipo_veiculo = reader.GetString(3),
                                    categoria = reader.GetString(4),
                                    preco_compra = reader.GetDecimal(5),
                                    preco_venda = reader.GetDecimal(6) 
                                });
                            }
                            return produtos;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Se a conexão falhar, o erro será capturado aqui
                throw new Exception("Erro ao consultar o banco de dados: " + ex.Message, ex);
            }
        }

    }
}
