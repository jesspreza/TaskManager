Instruções para Executar o Projeto
Para executar este projeto, siga as instruções abaixo:

Instalação do Docker: Certifique-se de ter o Docker instalado em sua máquina. Caso ainda não tenha, você pode baixá-lo aqui.

Iniciar o Container do SQL Server: Execute o seguinte comando no terminal para iniciar o container do SQL Server:

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Admin123!" -p 1433:1433 --name mssql --hostname mssql -d mcr.microsoft.com/mssql/server:2022-latest

Este comando iniciará um container Docker com o SQL Server e as configurações necessárias.

Iniciar o Projeto: Após o SQL Server estar em execução, você pode iniciar o projeto. As migrations serão aplicadas automaticamente durante a inicialização.

Informações sobre o Projeto: O projeto foi desenvolvido utilizando a arquitetura Clean Architecture, proporcionando uma estrutura organizada e modular. Foi construído com .NET 8 e Vue 3.

Criação de Usuários e Colaboradores: O sistema atualmente não inclui uma página de cadastro de usuários e colaboradores. Portanto, é necessário criar um usuário e um colaborador diretamente no banco de dados ou utilizando o Swagger.

Criar Usuário via Swagger:

Acesse o endpoint POST /api/User/register para criar um novo usuário.
Em seguida, faça o login utilizando o endpoint POST /api/User/login com seu nome de usuário e senha.
Copie o token gerado e vá até a opção "Authorize" no canto superior direito da página do Swagger. No campo "Value", insira a palavra Bearer seguida pelo token copiado.
Criar Colaborador via Swagger:

Com o token autenticado, utilize o endpoint POST /Collaborator para criar um novo colaborador, utilizando o ID gerado durante o registro do usuário.
