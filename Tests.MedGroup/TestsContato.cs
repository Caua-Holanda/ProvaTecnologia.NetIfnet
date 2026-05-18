using Moq;
using ProvaMed.DomainModel.Interfaces.UoW;
using ProvaMedGroup.DomainModel.Entities;
using ProvaMedGroup.DomainModel.Exceptions;
using ProvaMedGroup.DomainModel.Interfaces.Repositories;
using ProvaMedGroup.DomainService;
using ProvaMedGroup.DomainModel.Factories;
using ProvaMedGroup.DomainModel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class ContatoServiceTests
{
    private readonly Mock<IContatoRepository> _contatoRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ContatoService _contatoService;

    public ContatoServiceTests()
    {
        _contatoRepositoryMock = new Mock<IContatoRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _contatoService = new ContatoService(_contatoRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact(DisplayName = "Adicionar Contato - Sucesso")]
    public async Task AdicionarContato_Sucesso()
    {
        var contato = ContatoFactory.CriarNovoContato("Caua", "Holanda", DateTime.Now.AddYears(-20), 'M');

        _contatoRepositoryMock.Setup(r => r.Create(contato)).Verifiable();
 
        var resultado = await _contatoService.AdicionarContato(contato);

        Assert.NotNull(resultado);
        Assert.True(resultado.Ativo);
        Assert.Equal("Caua", resultado.NomeCompleto.PrimeiroNome);
        Assert.Equal("Holanda", resultado.NomeCompleto.Sobrenome);
        _contatoRepositoryMock.Verify(r => r.Create(contato));
        _unitOfWorkMock.Verify(u => u.CommitAsync());
    }

    [Fact(DisplayName = "Adicionar Contato - Falha por Idade")]
    public async Task AdicionarContato_FalhaPorIdade()
    {
        var exception = await Assert.ThrowsAsync<TratedExceptions>(() => 
            Task.FromResult(ContatoFactory.CriarNovoContato("Caua", "Holanda", DateTime.Now.AddYears(-10), 'M'))
        );
        Assert.Equal("O contato deve ter no mínimo 18 anos.", exception.Message);
    }

    [Fact(DisplayName = "Adicionar Contato - Falha por Data Futura")]
    public async Task AdicionarContato_FalhaPorDataFutura()
    {
        var exception = await Assert.ThrowsAsync<TratedExceptions>(() => 
            Task.FromResult(ContatoFactory.CriarNovoContato("Caua", "Holanda", DateTime.Now.AddDays(1), 'M'))
        );
        Assert.Equal("A data de nascimento não pode ser futura.", exception.Message);
    }

    [Fact(DisplayName = "Adicionar Contato - Falha por Menos de 1 Ano")]
    public async Task AdicionarContato_FalhaPorMenosDeUmAno()
    {
        var exception = await Assert.ThrowsAsync<TratedExceptions>(() => 
            Task.FromResult(ContatoFactory.CriarNovoContato("Caua", "Holanda", DateTime.Now.AddMonths(-6), 'M'))
        );
        Assert.Equal("O contato deve ter mais de 1 ano.", exception.Message);
    }

    [Fact(DisplayName = "Atualizar Contato - Sucesso")]
    public async Task AtualizarContato_Sucesso()
    {
        var contatoId = Guid.NewGuid();
        var contatoExistente = ContatoFactory.CriarNovoContato("Caua", "Holanda", DateTime.Now.AddYears(-20), 'M');
        _contatoRepositoryMock.Setup(r => r.Read(contatoId)).ReturnsAsync(contatoExistente);

        var contatoParaAtualizar = await _contatoService.ListarContatoId(contatoId);
        contatoParaAtualizar.Atualizar("Novo", "Nome", DateTime.Now.AddYears(-25), 'F');

        _contatoRepositoryMock.Setup(r => r.Update(contatoParaAtualizar)).Verifiable();

        var resultado = await _contatoService.AtualizarContato(contatoParaAtualizar);

        Assert.NotNull(resultado);
        Assert.Equal("Novo", resultado.NomeCompleto.PrimeiroNome);
        Assert.Equal("Nome", resultado.NomeCompleto.Sobrenome);
        Assert.Equal(DateTime.Now.AddYears(-25).Date, resultado.DataNascimento.Date);
        Assert.Equal('F', resultado.Sexo);
        _contatoRepositoryMock.Verify(r => r.Update(contatoParaAtualizar));
        _unitOfWorkMock.Verify(u => u.CommitAsync());
    }

    [Fact(DisplayName = "Atualizar Contato Ativo - Sucesso")]
    public async Task AtualizarContatoAtivo_Sucesso()
    {
        var contatoId = Guid.NewGuid();
        var contatoExistente = ContatoFactory.CriarNovoContato("Caua", "Holanda", DateTime.Now.AddYears(-20), 'M');
        contatoExistente.AlternarStatusAtivo();

        _contatoRepositoryMock.Setup(r => r.Read(contatoId)).ReturnsAsync(contatoExistente);

        var contatoParaAtualizar = await _contatoService.ListarContatoId(contatoId);
        

        _contatoRepositoryMock.Setup(r => r.Update(contatoParaAtualizar)).Verifiable();

        var resultado = await _contatoService.AtualizarContatoAtivo(contatoParaAtualizar);

        Assert.NotNull(resultado);
        Assert.True(resultado.Ativo);
        _contatoRepositoryMock.Verify(r => r.Update(contatoParaAtualizar), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact(DisplayName = "Atualizar Contato - Falha por Idade")]
    public async Task AtualizarContato_FalhaPorIdade()
    {
        var contatoId = Guid.NewGuid();
        var contatoExistente = ContatoFactory.CriarNovoContato("Caua", "Holanda", DateTime.Now.AddYears(-20), 'M');
        _contatoRepositoryMock.Setup(r => r.Read(contatoId)).ReturnsAsync(contatoExistente);

        var contatoParaAtualizar = await _contatoService.ListarContatoId(contatoId);

        var exception = await Assert.ThrowsAsync<TratedExceptions>(() => 
            Task.Run(() => contatoParaAtualizar.Atualizar("Caua", "Holanda", DateTime.Now.AddYears(-10), 'M'))
        );
        Assert.Equal("O contato deve ter no mínimo 18 anos.", exception.Message);
    }

    [Fact(DisplayName = "Atualizar Contato - Falha por Inativo")]
    public async Task AtualizarContato_FalhaInatividade()
    {
        var contatoId = Guid.NewGuid();
        var contatoExistente = ContatoFactory.CriarNovoContato("Caua", "Holanda", DateTime.Now.AddYears(-20), 'M');
        contatoExistente.AlternarStatusAtivo();

        _contatoRepositoryMock.Setup(r => r.Read(contatoId)).ReturnsAsync(contatoExistente);

        var contatoParaAtualizar = await _contatoService.ListarContatoId(contatoId);

        var exception = await Assert.ThrowsAsync<TratedExceptions>(() => 
            Task.Run(() => contatoParaAtualizar.Atualizar("Caua", "Holanda", DateTime.Now.AddYears(-25), 'M'))
        );
        Assert.Equal("Não é possível atualizar um contato inativo.", exception.Message);
    }

    [Fact(DisplayName = "Listar Contato por ID - Sucesso")]
    public async Task ListarContatoPorId_Sucesso()
    {
        var contatoId = Guid.NewGuid();
        var contato = ContatoFactory.CriarNovoContato("Caua", "Holanda", DateTime.Now.AddYears(-20), 'M');
        _contatoRepositoryMock.Setup(r => r.Read(contatoId)).ReturnsAsync(contato);

        var resultado = await _contatoService.ListarContatoId(contatoId);

        Assert.NotNull(resultado);
        Assert.Equal(contatoId, resultado.Id);
        Assert.Equal("Caua", resultado.NomeCompleto.PrimeiroNome);
        Assert.Equal("Holanda", resultado.NomeCompleto.Sobrenome);
    }

    [Fact(DisplayName = "Deletar Contato - Sucesso")]
    public async Task DeletarContato_Sucesso()
    {
        var contatoId = Guid.NewGuid();

        _contatoRepositoryMock.Setup(r => r.Delete(contatoId)).Verifiable();
 
        var resultado = await _contatoService.DeletarContato(contatoId);

        Assert.True(resultado);
        _contatoRepositoryMock.Verify(r => r.Delete(contatoId));
        _unitOfWorkMock.Verify(u => u.CommitAsync());
    }
}
