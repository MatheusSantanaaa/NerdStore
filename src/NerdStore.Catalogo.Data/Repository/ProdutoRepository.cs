using Microsoft.EntityFrameworkCore;
using NerdStore.Catalogo.Data.Context;
using NerdStore.Catalogo.Domain.Interfaces;
using NerdStore.Catalogo.Domain.Models;
using NerdStore.Core.Data;

namespace NerdStore.Catalogo.Data.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CatalogoContext _context;

        public ProdutoRepository(CatalogoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

      

        public async Task<Produto> GetByCategoria(int codigo)
        {
            return await _context.Produtos.AsNoTracking()
                    .Include(p => p.Categoria)
                    .Where(c => c.Categoria.Equals(codigo))
                    .FirstOrDefaultAsync();
        }

        public async Task<Produto> GetById(Guid id)
        {
            return await _context.Produtos.AsNoTracking()
                          .Where(p => p.Id == id)
                          .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Produto>> GetList()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }

        public void Register(Produto produto)
        {
            _context.Produtos.Add(produto);
        }

        public void Register(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
        }

        public void Update(Produto produto)
        {
            _context.Produtos.Update(produto);
        }

        public void Update(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
        }
        public void Dispose()
        {
            _context?.Dispose(); 
        }

        public async Task<IEnumerable<Categoria>> GetListCategory()
        {
            return await _context.Categorias.AsNoTracking().ToListAsync();
        }
    }
}
