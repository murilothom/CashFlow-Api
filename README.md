## Sobre o projeto

Esta API, desenvolvida utilizando **.NET 8**, adota os princípios do **Domain-Driven Design (DDD)** para oferecer uma solução estruturada e eficaz no gerenciamento de despesas pessoais. O principal objetivo é permitir que os
usuários registrem suas despesas, detalhando informações como título, data e hora, descrição, valor e tipo de pagamento, com os dados sendo armazenados de forma segura em um banco de dados **MySQL**

A arquitetura da **API** baseia-se em **REST**, utilizando métodos **HTTP** padrão para uma comunicação eficiente e simplificada. Além disso, é complementada por uma documentação **Swagger**, que proporciona uma interface gráfica
interativa para que os desenvolvedores possam explorar e testar os endpoints de maneira fácil.

Dentre os pacotes **NuGet** utilizados, o **AutoMapper** é o responsável pelo mapeamento entre objetos de domínio e requisição/resposta, reduzindo a necessidade de código repetitivo e manual. O **FluentAssertions** é utilizado nos
testes de unidade para tornar as verificações mais legíveis, ajudando a escrever testes claros e compreensíveis. Para as validações, o **FluentValidation** é usado para implementar regras de validação de forma simples e intuitiva
nas classes de requisições, mantendo o código limpo e fácil de manter. Por fim, o **EntityFramework** atua como um **ORM (Object-Relational Mapper)** que simplifica as interações com o banco de dados, permitindo o uso de
objetos .NET para manipular dados diretamente, sem a necessidade de lidar com consultas SQL.

## Getting Started

Para obter uma cópia local funcionando, siga estes passos.

### Requisitos

* <a href="https://dotnet.microsoft.com/pt-br/download" target="_blank">.NET SDK</a>
* <a href="https://dev.mysql.com/downloads/mysql/" target="_blank">MySQL Server</a>

### Instalação

1. Faça o clone do repositório:
 ```sh
  git clone https://github.com/murilothom/CashFlow-Api.git
 ```   
2. Ajuste o arquivo `appsetings.Development.json`
3. Faça a execução do App