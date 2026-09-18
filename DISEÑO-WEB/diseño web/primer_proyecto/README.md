# Fullstack CRUD - Angular + ASP.NET Core API

Este proyecto combina un frontend en Angular con una API REST en ASP.NET Core para gestionar productos en un inventario.

## Tecnologías

- Angular 22
- TypeScript
- ASP.NET Core
- .NET SDK
- MySQL 8+
- MySqlConnector
- HTML/CSS
- Visual Studio 2022 (para ejecutar la API)

## Estructura del proyecto

```text
primer_proyecto/         # Frontend Angular
API_C#/                  # Backend ASP.NET Core
README.md               # Documentación del proyecto
```

---

## Requisitos previos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

- Node.js 20+
- npm
- .NET SDK compatible con el proyecto
- Visual Studio 2022 (para correr la API)

---

## 1) Backend: ejecutar la API

### Pasos

1. Abre la carpeta `API_C#` en Visual Studio.
2. Asegúrate de que el proyecto cargue correctamente.
3. Compila y ejecuta con `F5`.
4. La API queda disponible normalmente en:

```text
http://localhost:5031
```

### Crear la base de datos en MySQL Workbench

La base de datos se llama `cafe_ecommerce`. Abre MySQL Workbench, ejecuta todo el archivo:

```text
API_C#/database/cafe_ecommerce.sql
```

El script crea las tablas `marcas` y `cafes`, sus relaciones, restricciones y dos cafés de ejemplo.

Configura la conexión de la API sin subir la contraseña a GitHub. Desde una terminal ubicada en `API_C#`:

```bash
dotnet user-secrets set "ConnectionStrings:CafeDatabase" "Server=localhost;Port=3306;Database=cafe_ecommerce;User ID=root;Password=TU_CLAVE;"
```

También puedes usar la variable de entorno `ConnectionStrings__CafeDatabase`. El archivo `appsettings.example.json` contiene el formato de referencia.

### Verificar que la API esté funcionando

Abre este enlace en el navegador:

```text
http://localhost:5031/api/cafes
```

Si todo está bien, deberías ver una respuesta JSON con los cafés disponibles o un arreglo vacío si aún no hay datos.

Si aparece un error tipo `ECONNREFUSED`, significa que la API no está levantada todavía.

### Endpoints esperados

```text
GET    http://localhost:5031/api/cafes
GET    http://localhost:5031/api/cafes/{id}
POST   http://localhost:5031/api/cafes
PUT    http://localhost:5031/api/cafes/{id}
DELETE http://localhost:5031/api/cafes/{id}
```

---

## 2) Frontend: ejecutar Angular

Abre una terminal en la carpeta `primer_proyecto` y ejecuta:

```bash
npm install
npm start
```

La app queda disponible en:

```text
http://localhost:4200
```

---

## 3) Configuración de entorno

El frontend apunta a la API mediante esta configuración:

```ts
export const environment = {
  production: false,
  ApiUrl: 'http://localhost:5031/api/'
};
```

Esto hace que las peticiones del CRUD salgan así:

```text
http://localhost:5031/api/cafes
```

---

## 4) Flujo del CRUD

El sistema permite:

- Ver productos
- Crear un nuevo producto
- Editar un producto existente
- Eliminar un producto

### Modelo esperado del producto

```ts
export interface Producto {
  id: number;
  nombre: string;
  cantidad: number;
  valor: number;
}
```

---

## 5) Validaciones y UX implementadas

Se agregaron validaciones básicas para mejorar la experiencia de usuario:

- No permite guardar un producto sin nombre
- Convierte `cantidad` y `valor` a número
- Evita valores vacíos o inválidos
- Muestra mensajes en consola para detectar errores de conexión o de backend
- El formulario limpia automáticamente después de guardar o actualizar
- Si el usuario está editando, se cambia el texto a `Actualizar producto`

### Recomendación importante

El proyecto depende de que la API esté corriendo antes de cargar la lista de productos. Si la API no responde, Angular mostrará un error de conexión como:

```text
ECONNREFUSED
```

Eso indica que el backend no está levantado en `localhost:5031`.

---

## 6) Cómo probarlo correctamente

### Orden correcto

1. Ejecuta la API en Visual Studio
2. Verifica `http://localhost:5031/api/cafes`
3. Ejecuta el frontend con Angular
4. Abre `http://localhost:4200`
5. Prueba crear, editar y eliminar productos

---

## 7) Comandos de GitHub

Si quieres subir el proyecto a GitHub, usa estos comandos:

```bash
git init
git add .
git commit -m "CRUD fullstack Angular + API"
git branch -M main
git remote add origin <URL_DEL_REPOSITORIO>
git push -u origin main
```

Ejemplo:

```bash
git remote add origin https://github.com/tu-usuario/tu-repositorio.git
```

---

## 8) Observaciones finales

Este proyecto está listo para:

- desarrollarse localmente,
- probarse con la API en ejecución,
- y publicarse como repositorio GitHub.

La parte crítica para que funcione correctamente es mantener la API levantada antes de usar la interfaz Angular.

---

## Autor

Proyecto académico — TEC-UPB
