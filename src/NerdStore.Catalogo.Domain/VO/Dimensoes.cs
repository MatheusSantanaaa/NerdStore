using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain.VO
{
    public class Dimensoes
    {
        public Dimensoes(decimal altura, decimal largura, decimal profundidade)
        {
            Validacoes.ValidarSeMenorIgualMinimo(altura, 1, "O cammpo altura não pode ser menor que 1");
            Validacoes.ValidarSeMenorIgualMinimo(largura, 1, "O cammpo largura não pode ser menor que 1");
            Validacoes.ValidarSeMenorIgualMinimo(profundidade, 1, "O cammpo profundidade não pode ser menor que 1");

            Altura = altura;
            Largura = largura;
            Profundidade = profundidade;
        }

        public decimal Altura { get; private set; }
        public decimal Largura { get; private set; }
        public decimal Profundidade { get; private set; }

        public string DescricaoFormatada()
        {
            return $"LxAxP: {Largura} x {Altura} x {Profundidade}";
        }

        public override string ToString()
        {
            return DescricaoFormatada();
        }
    }
}
