using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain.Models;
using NerdStore.Catalogo.Domain.VO;

namespace NerdStore.Catalogo.Application.AutoMapper
{
    public class VMToDomainMappingHelperProfile : Profile
    {
        public VMToDomainMappingHelperProfile()
        {
            CreateMap<CategoriaViewModel, Categoria>()
                .ConvertUsing(c => new Categoria(c.Nome, c.Codigo));

            CreateMap<ProdutoViewModel, Produto>()
                .ConstructUsing(p => 
                    new Produto(p.Nome, p.Descricao, p.Ativo,
                    p.Valor, p.CategoriaId, p.DataCadastro,
                    p.Imagem, new Dimensoes(p.Altura, p.Largura, p.Profundidade)));
        }
    }
}
