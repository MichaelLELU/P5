# 🚗 OC P5 Express Voitures

Ce projet est une application **ASP.NET Core MVC en C#** permettant la gestion de voitures (création, édition, suppression, gestion d’images, etc.).  
Le frontend utilise **Razor Pages**, et le backend repose sur **Entity Framework Core** pour l’accès aux données.

---

## 🛠️ Prérequis

- [Visual Studio 2022](https://visualstudio.microsoft.com/) (ou VS Code + SDK .NET)
- [SDK .NET 8.0](https://dotnet.microsoft.com/download) (ou version compatible définie dans le `.csproj`)
- [SQL Server / LocalDB](https://learn.microsoft.com/sql/sql-server)



## ▶️ Installation & Lancement

### 1️⃣ Cloner le dépôt
```bash
git clone https://github.com/MichaelLELU/P5.git
```

### 2️⃣ Configurer la base de données

Ouvrir appsettings.json et mettre à jour la connexion :

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=VoituresDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 3️⃣ Appliquer les migrations

```bash
dotnet ef database update
```

### 4️⃣ Lancer l’application

En CLI :
```bash
dotnet run
```


### 📌 Fonctionnalités principales

 Gestion des voitures (CRUD complet)

 Gestion des images associées aux voitures

 Authentification et gestion d’utilisateurs via Identity

 Validation des formulaires avec messages d’erreur

 Architecture MVC claire (Controllers, Models, Views, Services)
