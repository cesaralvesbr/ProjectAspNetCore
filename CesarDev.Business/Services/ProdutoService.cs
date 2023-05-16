using CesarDev.Business.Interfaces.Services;
using CesarDev.Business.Models;
using CesarDev.Business.Models.Validations;

namespace CesarDev.Business.Services
{
    public abstract class ProdutoService : BaseService, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        protected ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task Adicionar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) ;

            await _produtoRepository.Adicionar(produto);

        }

        public async Task Atualizar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) ;

            await _produtoRepository.Atualizar(produto);

        }

        public async Task Remover(Guid id)
        {
            await _produtoRepository.Remover(id);
        }

        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }
    }
}
