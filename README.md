# API-DATA-BASE

Proyecto API REST para gestión de datos — repositorio preparado para mostrarse en tu CV y LinkedIn.

Por qué destacar este proyecto
- Implementación completa de una API con buenas prácticas: arquitectura clara, manejo de errores, logs y tests.
- Seguridad: configuración sensible separada del código (User Secrets / variables de entorno). La cadena de conexión no está en el repositorio.
- Preparado para CI/CD y despliegue en contenedores (Docker) o servicios en la nube.

Qué incluye
- README con instrucciones rápidas.
- appsettings.example.json para referencia de configuración.
- docs/ con guías de seguridad y documentación técnica.

Cómo mostrarlo en tu CV
- En la sección ‘Proyectos’: "API-DATA-BASE — API REST con .NET 10, EF Core, pruebas unitarias e integración, pipeline CI/CD y despliegue en contenedores."
- Añade bullets cortos: "Separación de secretos, migraciones automáticas, Swagger/OpenAPI, pruebas y logging estructurado."

Instalación rápida (local)
1. Clona el repositorio:
   git clone https://github.com/pablorja/API-DATA-BASE.git
2. Copia la configuración de ejemplo y añade tu cadena de conexión de forma segura:
   - Copia DISEÑO-WEB/API_CON_DB/API_CON_DB/appsettings.example.json -> appsettings.json (o usa User Secrets / variables de entorno)
3. Restaura y ejecuta (ejemplo .NET):
   dotnet restore
   dotnet ef database update
   dotnet run

Seguridad
- Nunca subir appsettings.json con cadenas reales. Usa User Secrets (dotnet user-secrets), variables de entorno o un gestor de secretos. Hay más detalles en docs/.

Contacto
- https://github.com/pablorja
