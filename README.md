# Fleet Management

Este repositório contém o código-fonte do projeto de gerenciamento de frotas. Siga as instruções abaixo para configurar e executar o projeto localmente de três maneiras diferentes: **usando Docker**, **via CLI**, ou **via IDE**.

---

## Como Fazer para Rodar o Projeto

### Pré-requisitos

Certifique-se de ter os seguintes softwares instalados:

- [.NET Core SDK](https://dotnet.microsoft.com/download) (versão 3.1 ou superior)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Docker](https://www.docker.com/get-started) (opcional, para Containeres)
- [IDE com suporte ao .NET](https://visualstudio.microsoft.com/) (como Visual Studio ou Rider)

---

## Assumptions

Esta seção descreve as suposições feitas durante o desenvolvimento do projeto:

- **.NET Core SDK**: Versão 3.1 ou superior instalada.
- **Docker**: Usado para Containerização recomendada.

---

### 1. Procedimentos Gerais

1. **Clone o Repositório**:

   ```sh
   git clone https://github.com/yan-karlo/FleetManagement.git
   cd FleetManagement
   ```

2. **Build das Imagens com Docker Compose**:

   ```sh
   docker-compose build
   ```

3. **Subir os Containeres**:

   Inicie os Containeres no modo interativo para configurar o ambiente:

   ```sh
   docker-compose up -d
   ```

4. **Aplicar as Migrações**:

   Depois de subir os containers, esse processo também pode ser feito de pelo Visual Studio:

   1. Navegue pelos menus: Tools -> NuGet Package Manager -> NuGet Package Manager Console
   2. Na borda superior da janela deste console, selecione "FleetManagement.WebUI" como projeto default.
   3. Digite o comando : `update-database`

5. **Acessar as Aplicações**:

   As aplicações estarão disponíveis nos containers em:

   - Fleet Management Web App : [http://localhost:5000/home]
   - Fleet Management API : [http://localhost:8080/api-docs/index.html]

6. **Parar os Containeres**:

   Para encerrar os Containeres, use:

   ```sh
   docker-compose down
   ```

---

### 2. Executando Localmente via CLI

1. **Clone o Repositório**:

   ```sh
   git clone https://github.com/yan-karlo/FleetManagement.git
   cd FleetManagement
   ```

2. **Build das Imagens com Docker Compose**:

   ```sh
   docker-compose build
   ```

3. **Subir os Containeres**:

   Inicie os Containeres no modo interativo para configurar o ambiente:

   ```sh
   docker-compose up -d
   ```

4. **Restaurar Dependências**:

   Restaure as dependências do projeto:

   ```sh
   dotnet restore
   ```

5. **Aplicar as Migrações**:

   Configure o esquema do banco de dados executando:

   ```sh
   dotnet ef database update
   ```

6. **Fazer o build**:

   Gere o build do projeto executando:

   ```sh
   dotnet build
   ```

7. **Executar o Projeto**:

   Inicie o projeto localmente:

   ```sh
   dotnet run
   ```

8. **Acessar as Aplicações**:

As aplicações estarão disponíveis nos containers em: - Fleet Management Web App : [http://localhost:5287/Home] - Fleet Management API : [http://localhost:5196/api-docs/index.html]

8. **Parar os Containeres**:

   Para encerrar os Containeres, use:

   ```sh
   docker-compose down
   ```

---

### 3. Executando via IDE

1. **Clone o Repositório**:

   Abra o Visual Studio e clone o repositório diretamente por meio da interface gráfica ou usando o terminal integrado:

   ```sh
   git clone https://github.com/yan-karlo/FleetManagement.git
   cd FleetManagement
   ```

2. **Build das Imagens com Docker Compose**:

   ```sh
   docker-compose build
   ```

3. **Subir os Containeres**:

   Inicie os Containeres no modo interativo para configurar o ambiente:

   ```sh
   docker-compose up -d
   ```

4. **Configurar o Banco de Dados**: (Apenas se vc quiser usar por conta própria outro SGBD)

   Atualize a string de conexão no arquivo `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=SEU_SERVIDOR;Database=SEU_BANCO_DE_DADOS;User Id=SEU_USUARIO;Password=SUA_SENHA;"
     }
   }
   ```

5. **Restaurar Dependências**:

   A IDE geralmente detectará dependências ausentes e oferecerá a opção de restaurá-las automaticamente. Se necessário, rode o comando manualmente no terminal integrado:

   ```sh
   dotnet restore
   ```

6. **Aplicar as Migrações**:

   No terminal integrado da IDE, aplique as migrações:

   Depois de subir os containers, esse processo também pode ser feito de pelo Visual Studio:

   1. Navegue pelos menus: Tools -> NuGet Package Manager -> NuGet Package Manager Console
   2. Na borda superior da janela deste console, selecione "FleetManagement.WebUI" como projeto default.
   3. Digite o comando : `update-database`

7. **Rodar o Projeto**:

   Antes certifique-se que a solução do projeto está configurada para múltiplos proejtos de startup:

   1. Clique com o botão direito do mouse sobre a solution e escolha a opção: "Cofigure Startup Projetcs" (Configurar os projetos de início)
   2. Verifique se a opção "Multiple Startup Projects" está selecionada. Caso não marque-a.
   3. Verifique se no quadro maior embaixo, se os projetos FleeManagement.WebUI e FleetManagement.API estão com o valor "Start" na coluna "Action". Caso não defina-as como tal.

   O procedimento acima fará com que ambas seja inicializadas quando a opção de **RUN** ou **DEBUG** for selecionada.

   Use a opção **Run** ou **Debug** da IDE para iniciar o servidor. Certifique-se de que o perfil selecionado esteja configurado corretamente para o ambiente de desenvolvimento.

8. **Parar os Containeres**:

   Para encerrar os Containeres, use:

   ```sh
   docker-compose down
   ```

---

Espero que estas instruções ajudem a configurar e executar o projeto. 🚀
