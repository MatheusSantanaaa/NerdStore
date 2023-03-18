using NerdStore.Catalogo.Domain.VO;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Domain.Models
{
    public class Produto : Entity, IAggregateRoot
    {
        public Guid CategoriaId { get; set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public string Imagem { get; private set; }
        public int QuantidadeEstoque { get; private set; }
        public Dimensoes Dimensoes{ get; set; }

        /* EF Relations */
        public Categoria Categoria { get; private set; }

        protected Produto(){}

        public Produto(string nome, string descricao, bool ativo, decimal valor, Guid categoriaId, DateTime dataCadastro, string imagem, Dimensoes dimensoes)
        {
            CategoriaId = categoriaId;
            Nome = nome;
            Descricao = descricao;
            Ativo = ativo;
            Valor = valor;
            DataCadastro = dataCadastro;
            Imagem = imagem;
            Dimensoes = dimensoes;

            Validar();
        }

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;

        public void AlterarCategoria(Categoria categoria)
        {
            Categoria = categoria;
            CategoriaId = categoria.Id;
        }

        public void AlterarDescricao(string descricao)
        {
            Validacoes.ValidarSeNulo(descricao, "A descrição o produto não pode ser vazia");
            Descricao = descricao;
        }

        public void DebitarEstoque(int quantidade)
        {
            if (quantidade < 0) quantidade *= -1;
            if (!PossuiEstoque(quantidade)) throw new Exception("Estoque insuficiente");
            QuantidadeEstoque -= quantidade;
        }

        public void ReporEstoque(int quantidade)
        {
            QuantidadeEstoque += quantidade;
        }
        public bool PossuiEstoque(int quantidade)
        {
            return QuantidadeEstoque >= quantidade;
        }

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, "Nome não preenchido");
            Validacoes.ValidarSeVazio(Descricao, "Descricao não preenchido");
            Validacoes.ValidarSeIgual(CategoriaId, Guid.Empty, "Categoria não preenchido");
            Validacoes.ValidarSeMenorQue(Valor, 1, "Valor não pode ser menor do que 1");
            Validacoes.ValidarSeVazio(Imagem, "Imagem não preenchido");
        }
    }
}
