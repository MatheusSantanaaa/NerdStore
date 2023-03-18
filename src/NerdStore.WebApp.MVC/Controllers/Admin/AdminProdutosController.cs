using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Interfaces;
using NerdStore.Catalogo.Application.ViewModels;

namespace NerdStore.WebApp.MVC.Controllers.Admin
{
    public class AdminProdutosController : Controller
    {
        private readonly IProdutoAppService _produtoService;

        public AdminProdutosController(IProdutoAppService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        [Route("obter-todos")]
        public async Task<IActionResult> Index()
        {
            return View(await _produtoService.ObterTodos());
        }

        [HttpGet]
        [Route("novo-produto")]
        public async Task<IActionResult> NovoProduto()
        {
            return View(await PopularCategoria(new ProdutoViewModel()));
        }

        [HttpPost]
        [Route("novo-produto")]
        public async Task<IActionResult> NovoProduto(ProdutoViewModel produtoViewModel)
        {
            if(!ModelState.IsValid) return View(await PopularCategoria(new ProdutoViewModel()));

            await _produtoService.AdicionarProduto(produtoViewModel);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("editar-produto")]
        public async Task<IActionResult> AtualizarProduto(Guid id)
        {
            return View(await PopularCategoria(new ProdutoViewModel()));
        }

        [HttpPost]
        [Route("editar-produto")]
        public async Task<IActionResult> AtualizarProduto(Guid id, ProdutoViewModel produtoViewModel)
        {
            var produto = await _produtoService.ObterPorId(id);
            if(produto is null)
            {
                return RedirectToAction("Index");
            }
            produtoViewModel.QuantidadeEstoque = produto.QuantidadeEstoque;

            ModelState.Remove("QuantidadeEstoque");
            if (!ModelState.IsValid) return View(await PopularCategoria(produtoViewModel));


            await _produtoService.AtualizarProduto(produtoViewModel);

            return RedirectToAction("Index");
        }


        [HttpGet]
        [Route("produtos-atualizar-estoque")]
        public async Task<IActionResult> AtualizarEstoque(Guid id)
        {
            return View("AtualizarEstoque", await _produtoService.ObterPorId(id));
        }

        [HttpPost]
        [Route("produtos-atualizar-estoque")]
        public async Task<IActionResult> AtualizarEstoque(Guid id, int quantidade)
        {
            if(quantidade > 0)
            {
                await _produtoService.ReporEstoque(id, quantidade);
            }
            else
            {
                await _produtoService.DebitarEstoque(id, quantidade);
            }

            return View("Index", await _produtoService.ObterTodos());
        }

        private async Task<ProdutoViewModel> PopularCategoria(ProdutoViewModel produto)
        {
            produto.Categorias = await _produtoService.ObterCategorias();

            return produto;
        }
    }
}
