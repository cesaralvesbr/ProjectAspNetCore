using AutoMapper;
using CesarDev.App.ViewModels;
using CesarDev.Business.Interfaces;
using CesarDev.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CesarDev.App.Controllers
{
    public class ProdutosController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public ProdutosController(IFornecedorRepository fornecedorRepository,
            IProdutoRepository produtoRepository,
            IProdutoService produtoService, IMapper mapper, INotificador notificador) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("lista-de-produtos")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await ObterProdutosFornecedores()));
        }

        [AllowAnonymous]
        [Route("dados-do-produto/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
                return NotFound();

            return View(produtoViewModel);
        }

        //[ClaimsAuthorize("Produto","Adicionar")]
        [Route("novo-produto")]
        public async Task<IActionResult> Create()
        {
            ProdutoViewModel produtoViewModel = await PopularFornecedores(new ProdutoViewModel());
            return View(produtoViewModel);
        }

        //[ClaimsAuthorize("Produto", "Adicionar")]
        [Route("novo-produto")]
        [HttpPost]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopularFornecedores(produtoViewModel);
            if (!ModelState.IsValid)
                return View(produtoViewModel);

            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
                return View(produtoViewModel);

            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            Produto produto = _mapper.Map<Produto>(produtoViewModel);

            await _produtoService.Adicionar(produto);

            if(!OperacaoValida()) return View(produto);

            return RedirectToAction("Index");
        }

        //[ClaimsAuthorize("Produto", "Editar")]
        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            ProdutoViewModel produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
                return NotFound();

            return View(produtoViewModel);
        }

        //[ClaimsAuthorize("Produto", "Editar")]
        [Route("editar-produto/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id) return NotFound();

            var produtoAtualizacao = await ObterProduto(id);
            produtoViewModel.Fornecedor = produtoAtualizacao.Fornecedor;
            produtoViewModel.Imagem = produtoAtualizacao.Imagem;

            if (!ModelState.IsValid)
                return View(produtoViewModel);

            if (produtoViewModel.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
                    return View(produtoViewModel);

                produtoAtualizacao.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            }

            produtoAtualizacao.Nome = produtoViewModel.Nome;
            produtoAtualizacao.Descricao = produtoViewModel.Descricao;
            produtoAtualizacao.Valor = produtoViewModel.Valor;
            produtoAtualizacao.Ativo = produtoViewModel.Ativo;

            Produto produto = _mapper.Map<Produto>(produtoAtualizacao);
            await _produtoService.Atualizar(produto);

            if (!OperacaoValida()) return View(produto);

            return RedirectToAction(nameof(Index));
        }

        //[ClaimsAuthorize("Produto", "Excluir")]
        [Route("excluir-produto/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            ProdutoViewModel produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
                return NotFound();

            return View(produtoViewModel);
        }

        //[ClaimsAuthorize("Produto", "Excluir")]
        [Route("excluir-produto/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            ProdutoViewModel produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null) return NotFound();
            await _produtoService.Remover(id);

            if (!OperacaoValida()) return View(produtoViewModel);

            return RedirectToAction("Index");
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            ProdutoViewModel produto = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return produto;
        }

        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produto)
        {
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return produto;
        }

        private async Task<IEnumerable<ProdutoViewModel>> ObterProdutosPorFornecedor(Guid id)
        {
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosPorFornecedor(id));
        }

        private async Task<IEnumerable<ProdutoViewModel>> ObterProdutosFornecedores()
        {
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores());
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com esse nome");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }
    }
}
