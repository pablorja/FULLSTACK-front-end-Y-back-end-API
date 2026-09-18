# E-commerce de Cafe

Aplicacion full stack para administrar un catalogo de cafe mediante un CRUD completo. El frontend esta construido con Angular y consume una API REST en ASP.NET Core, que persiste los datos en MySQL.

## Repositorio

https://github.com/pablorja/FULLSTACK-front-end-Y-back-end-API

## Tecnologias

- Angular 22 y TypeScript
- ASP.NET Core sobre .NET 10
- MySQL 8+ y MySqlConnector
- HTML y CSS responsive
- MySQL Workbench

## Estructura

```text
API_C#/                              API ASP.NET Core
  Controllers/                       Endpoints REST
  Models/                            Entidades Cafe y Marca
  Repositories/                      Acceso a datos MySQL
  database/cafe_ecommerce.sql        Script para Workbench
  appsettings.example.json           Plantilla sin secretos

DISEÑO-WEB/diseño web/primer_proyecto/  Frontend Angular
  src/app/pages/home/                Catalogo y formulario CRUD
  src/app/Services/                  Servicio HTTP
  src/environments/                 URL y banderas del frontend
```

## Base de datos

La base se llama `cafe_ecommerce`. El script `API_C#/database/cafe_ecommerce.sql` crea las tablas `marcas` y `cafes`, la clave foranea y datos iniciales.

En MySQL Workbench abre y ejecuta ese archivo. Luego comprueba:

```sql
USE cafe_ecommerce;
SHOW TABLES;
SELECT * FROM marcas;
SELECT * FROM cafes;
```

## Configuracion segura

Desde una terminal ubicada en `API_C#`, configura la contraseña local mediante User Secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:CafeDatabase" "Server=localhost;Port=3306;Database=cafe_ecommerce;User ID=root;Password=TU_CLAVE;"
```

Tambien puedes usar `ConnectionStrings__CafeDatabase`. La plantilla esta en `API_C#/appsettings.example.json`. Nunca subas contraseñas reales.

## Ejecutar

API, desde `API_C#`:

```powershell
dotnet restore
dotnet build
dotnet run --urls http://localhost:5031
```

Frontend, desde `DISEÑO-WEB/diseño web/primer_proyecto`:

```powershell
npm install
npm start
```

Abre `http://localhost:4200` y verifica el API en `http://localhost:5031/api/cafes`.

## Endpoints CRUD

```text
GET    /api/cafes
GET    /api/cafes/{id}
POST   /api/cafes
PUT    /api/cafes/{id}
DELETE /api/cafes/{id}
```

El frontend permite listar, crear, editar, eliminar y actualizar el catalogo sin recargar la pagina. Tambien valida nombre, cantidad y precio.

## Modulo visual opcional

Existe un modulo preparado para mostrar una imagen y una presentacion del cafe. Esta desactivado por defecto. Para activarlo, cambia a `true`:

```ts
showCoffeePresentation: true
```

La propiedad esta en `src/environments/environment.ts` y `src/environments/environment.development.ts`.

## Validacion

```powershell
dotnet build API_C#/ConcesionarioApi.csproj
cd "DISEÑO-WEB/diseño web/primer_proyecto"
npm run build
```

El CRUD fue probado con operaciones POST, PUT, GET y DELETE contra MySQL.

## Autor

Proyecto academico TEC-UPB.
