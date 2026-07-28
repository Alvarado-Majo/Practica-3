# VotacionNacional - Readme

1. Integrantes finales del grupo (se les asignará la nota del proyecto):
   - Méndez González Javier
   - Guatemala Camacho Angelik
   - Pasos Solano Keisly
   - Alvarado Fernández Maria José

2. Enlace del repositorio:
   - https://github.com/Alvarado-Majo/Practica-3/tree/main/VotacionNacional

3. Especificación básica del proyecto

a) Arquitectura del proyecto
   - Solución compuesta por varios proyectos:
	 - VotacionNacional (Razor Pages Web App, proyecto web principal)
	 - VotacionNacional.API (Web API para exponer servicios)
	 - VotacionNacional.BILL (Business Logic Layer, librería de lógica de negocio)
	 - VotacionNacional.DAL (Data Access Layer, librería de acceso a datos)
   - Target framework: .NET 10 (net10.0)
   - Separación clara de responsabilidades: presentación (Razor Pages/API), negocio (BLL) y datos (DAL).

b) Libraries / paquetes NuGet utilizados (por proyecto)
   - VotacionNacional (Razor Pages):
	 - AutoMapper (16.2.0)
	 - Microsoft.EntityFrameworkCore (10.0.10)
	 - Scalar.AspNetCore (2.16.16)
   - VotacionNacional.API:
	 - AutoMapper (16.2.0)
	 - Microsoft.AspNetCore.OpenApi (10.0.10)
	 - Microsoft.EntityFrameworkCore (10.0.10)
	 - Microsoft.EntityFrameworkCore.Design (10.0.10)
	 - Scalar.AspNetCore (2.16.16)
	 - Swashbuckle.AspNetCore (10.2.3)
   - VotacionNacional.BILL:
	 - AutoMapper (16.2.0)
	 - Microsoft.EntityFrameworkCore (10.0.10)
	 - Microsoft.Extensions.Configuration.UserSecrets (10.0.10)
	 - Scalar.AspNetCore (2.16.16)
   - VotacionNacional.DAL:
	 - AutoMapper (16.2.0)
	 - Microsoft.EntityFrameworkCore (10.0.10)
	 - Microsoft.EntityFrameworkCore.Design (10.0.10)
	 - Microsoft.EntityFrameworkCore.SqlServer (10.0.10)
	 - Microsoft.EntityFrameworkCore.Tools (10.0.10)
	 - Scalar.AspNetCore (2.16.16)

c) Principios SOLID y patrones de diseño utilizados
   - Principios SOLID (aplicados globalmente en la solución):
	 - S: Single Responsibility — cada proyecto y clase tiene una responsabilidad clara.
	 - O: Open/Closed — extensible mediante herencia/composición sin modificar código existente.
	 - L: Liskov Substitution — uso de interfaces para permitir sustitución de implementaciones.
	 - I: Interface Segregation — interfaces específicas por responsabilidad.
	 - D: Dependency Inversion — dependencias invertidas vía inyección de dependencias.
   - Patrones de diseño y prácticas usadas:
	 - Repository (en DAL) para abstracción de acceso a datos.
	 - Unit of Work (cuando aplica en la gestión de transacciones con EF Core).
	 - Dependency Injection (registro de servicios en el contenedor de ASP.NET Core).
	 - DTOs + AutoMapper para separar modelos de dominio de modelos de transporte.
	 - API docs con Swagger/Swashbuckle en el proyecto API.

Si necesita más detalle técnico (diagramas, contratos API o flujo de datos), indíquelo y lo agrego.
