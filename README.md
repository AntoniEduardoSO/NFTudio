# 🧾 NFTudio API

NFTudio é uma API REST desenvolvida em .NET 8 para gerenciar empresas parceiras (Associates), suas áreas de atuação e links relacionados. O projeto tem como foco a simplicidade, organização e estrutura leve, Ela foi projetada para a empresa NFTudio Lab.

---

## 🚀 Funcionalidades

- ✅ Cadastro de empresas parceiras
- 🔍 Listagem de empresas para exibição pública
- 🛠 Listagem de empresas para gestão interna
- ✏️ Atualização de dados de empresas
- 📁 Cadastro e listagem de áreas de atuação
- 🔗 Associação de links externos a cada empresa

---

## 🧱 Estrutura de Projeto

```
NFTudio/
├── NFTudio.Api/        # Camada de API com endpoints, configurações e handlers
│   ├── Endpoints/      # Endpoints REST agrupados por funcionalidade
│   ├── Handlers/       # Lógica de aplicação e manipulação de dados
│   ├── Data/           # DbContext (EF Core) e configuração de SQLite
│   └── Models/         # DTOs de entrada e saída
├── NFTudio.Core/       # Entidades de domínio como Associate, Link e Operation
├── NFTudio.sln         # Solução do Visual Studio
```

---

## ⚙️ Como Executar Localmente

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQLite

### Passos

```bash
git clone https://github.com/AntoniEduardoSO/NFTudio.git
cd NFTudio
dotnet restore
cd NFTudio.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" : "Data Source=CAMINHO_PARA_SEU_BD"
dotnet ef database update
dotnet run
```

Acesse a API em appsettings.json, fica salvo os valores da url`s tanto no back, como no front para questão de CORS.

---

## 🔚 Endpoints Principais

### 🔹 Associates
| Método | Rota                | Descrição                            |
|--------|---------------------|--------------------------------------|
| POST   | /associate          | Cria nova empresa                    |
| GET    | /associate/home     | Lista empresas públicas              |
| GET    | /associate/manage   | Lista empresas para administração    |
| PUT    | /associate          | Atualiza empresa existente           |

### 🔹 Operations
| Método | Rota              | Descrição                    |
|--------|-------------------|------------------------------|
| GET    | /operation/home   | Lista áreas de atuação       |

---

## 🧪 Tecnologias

- ASP.NET Core (.NET 8)
- SQLite (via EF Core)
- Minimal APIs
- DTOs e validação
- CORS configurado para acesso frontend

---

## 📄 Licença

Este projeto está sob a licença MIT.
