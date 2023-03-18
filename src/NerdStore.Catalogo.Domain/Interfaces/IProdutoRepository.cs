using NerdStore.Catalogo.Domain.Models;
using NerdStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetList();
        Task<IEnumerable<Categoria>> GetListCategory();
        Task<Produto> GetById(Guid id);
        Task<Produto> GetByCategoria(int codigo);

        void Register(Produto produto);
        void Update(Produto produto);

        void Register(Categoria categoria);
        void Update(Categoria categoria);
    }
}
