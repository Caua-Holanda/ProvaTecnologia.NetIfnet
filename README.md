
# Documentação do Projeto - ProvaTecnologia.NetIfnet
**Aluno:** Cauã Holanda  
**Disciplina:** Tecnologia .NET 
**Projeto:** API de Gestão de Contatos (MedGroup)  
**Repositório:** [Caua-Holanda/ProvaTecnologia.NetIfnet](https://github.com/Caua-Holanda/ProvaTecnologia.NetIfnet)

---

## 1. Descrição do Problema e Solução

### 1.1. O Problema
O desafio central é o gerenciamento de contatos com regras de negócio rigorosas para garantir a integridade dos dados cadastrados. O sistema deve impedir:
- Cadastro de menores de 18 anos.
- Cadastro de pessoas com menos de 1 ano de idade.
- Uso de datas de nascimento futuras.
- Atualização de contatos que estejam marcados como inativos.

### 1.2. A Solução
A solução foi desenvolvida utilizando **.NET 8** com uma arquitetura robusta e testável, seguindo os princípios de **SOLID** e **Domain-Driven Design (DDD)** simplificado.

**Componentes Técnicos:**
- **API REST:** Desenvolvida com ASP.NET Core, utilizando versionamento de API e Swagger para documentação.
- **Camada de Domínio:** Contém as entidades e interfaces, garantindo que as regras de negócio sejam independentes de tecnologia.
- **Camada de Serviço:** Implementa a lógica de validação (idade, data, status) e coordena as operações.
- **Camada de Infraestrutura:** Utiliza **Entity Framework Core** com o padrão **Repository** e **Unit of Work** para persistência em banco de dados SQL Server.
- **Mapeamento:** Uso de **AutoMapper** para converter entre entidades de domínio e ViewModels da API.

---

## 2. Estratégia de Testes (TDD)

O desenvolvimento seguiu a abordagem **TDD (Test Driven Development)**, onde os testes foram planejados e escritos para validar as regras de negócio antes da implementação final.

### 2.1. Frameworks Utilizados
- **xUnit:** Framework de testes unitários.
- **Moq:** Biblioteca para criação de objetos simulados (mocks) de repositórios e Unit of Work.

### 2.2. Mapeamento Funcionalidade x Teste

| Funcionalidade | Teste Unitário | Validação Esperada |
| :--- | :--- | :--- |
| Adicionar Contato (≥18 anos) | `AdicionarContato_Sucesso` | Sucesso na criação e persistência. |
| Adicionar Contato (<18 anos) | `AdicionarContato_FalhaPorIdade` | Lançamento de exceção de negócio. |
| Validar Data Futura | `AdicionarContato_FalhaPorDataFutura` | Bloqueio de datas posteriores a hoje. |
| Validar Idade Mínima (1 ano) | `AdicionarContato_FalhaPorMenosDeUmAno` | Bloqueio de bebês com menos de 1 ano. |
| Atualizar Contato Ativo | `AtualizarContato_Sucesso` | Permissão de edição para contatos ativos. |
| Atualizar Contato Inativo | `AtualizarContato_FalhaInatividade` | Bloqueio de edição para contatos inativos. |
| Alternar Status Ativo | `AtualizarContatoAtivo_Sucesso` | Inversão correta da flag `Ativo`. |
| Listar por ID | `ListarContatoPorId_Sucesso` | Retorno do contato correto se estiver ativo. |

---
