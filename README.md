Instruções para Executar o Projeto
Para rodar este projeto, siga as instruções abaixo:

Certifique-se de ter o Docker instalado em sua máquina. Caso ainda não tenha, você pode baixá-lo aqui.
Execute o seguinte comando no terminal para iniciar o container do SQL Server:

docker run -it --name sqlserver -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Admin123!" -p 1433:1433 -d mcr.microsoft.com/mssql/server

Este comando iniciará um container Docker contendo o SQL Server com as configurações necessárias.

Após o SQL Server estar em execução, você pode iniciar o projeto. As migrations serão aplicadas automaticamente durante a inicialização do projeto.

O projeto foi desenvolvido utilizando a arquitetura Clean Architecture, proporcionando uma estrutura organizada e modular. Além disso, é importante mencionar que o projeto foi construído utilizando o .NET 8 e Vue 3.
