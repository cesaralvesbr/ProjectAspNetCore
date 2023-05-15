using CesarDev.Business.Interfaces;
using CesarDev.Business.Models;
using CesarDev.Business.Models.Validations;

namespace CesarDev.Business.Services
{
    public abstract class FornecedorService : BaseService, IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IProdutoRepository _produtoRepository;

        protected FornecedorService(IFornecedorRepository fornecedorRepository,
            IProdutoRepository produtoRepository)
        {
            _fornecedorRepository = fornecedorRepository;
            _produtoRepository = produtoRepository;
        }

        public async Task Adicionar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor) &&
                !ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco)) return;

            if (_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento).Result.Any())
            {
                Notificar("Já existe fornecedor com esse documento informado.");
                return;
            }

            await _fornecedorRepository.Adicionar(fornecedor);
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)) ;

            if (_fornecedorRepository.Buscar(f=>f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
            {
                Notificar("Já existe fornecedor com esse documento informado.");
                return;
            }
        }

        public async Task AtualizarEndereco(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)) ;

        }

        public async Task Remover(Fornecedor fornecedor)
        {
            throw new NotImplementedException();
        }
    }
}
