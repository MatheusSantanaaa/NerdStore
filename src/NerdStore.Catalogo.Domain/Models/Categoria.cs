using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Domain.Models
{
    public class Categoria : Entity
    {
        public string Nome { get; private set; }
        public int Codigo { get; private set; }

        //EF Relation
        public IEnumerable<Produto> Produtos { get; set; }

        protected Categoria() {}

        public Categoria(string nome, int codigo)
        {
            Nome = nome;
            Codigo = codigo;

            Validar();
        }

        private void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, "Nome não preenchido");
            Validacoes.ValidarSeIgual(Codigo, 0, "Codigo não pode ser igual a zero");
        }

        public override string ToString()
        {
            return $"{Nome} - {Codigo}";
        }
    }
}
