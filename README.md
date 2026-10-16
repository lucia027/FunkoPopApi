# FunkoApp — Práctica 3: API REST de Funkos

Proyecto de la asignatura **Desarrollo Web en Entorno Servidor** para construir y evolucionar una API REST de gestión de Funkos con ASP.NET Core.

El desarrollo se divide en **dos partes**, cada una conservada en una rama de Git. La segunda partirá del código terminado en la primera y lo refactorizará para utilizar otra base de datos y otro modelo de relaciones.

## Organización de ramas

| Rama | Propósito |
| --- | --- |
| `main` | Documentación general y punto de partida del repositorio. |
| `v-Sql` | API de Funkos con base de datos SQL, Entity Framework Core y relación con categorías. |
| `v-Mongo` | Evolución de la primera parte: MongoDB, categorías embebidas, proveedores referenciados y dos repositorios intercambiables. |

> Las ramas `v-Sql` y `v-Mongo` se desarrollan de forma secuencial: la segunda se creará **a partir de la primera cuando esté terminada**. Cada una tendrá su propio README con el alcance y las instrucciones correspondientes.

## Parte 1: SQL

- CRUD de Funkos: consultar, crear, actualizar y eliminar.
- Consulta de categorías: listar y obtener por identificador.
- Categorías precargadas mediante semillas (*seed data*).
- Relación **uno a muchos**: una categoría puede tener muchos Funkos y cada Funko pertenece a una categoría.
- Persistencia con **Entity Framework Core y PostgreSQL**, mediante Fluent API y migraciones.
- Borrado lógico de Funkos con `IsDeleted`.

## Parte 2: MongoDB

Se reutilizará y refactorizará la API de la primera parte:

- CRUD de Funkos y consultas de categorías y proveedores.
- Categoría **embebida** en el documento Funko.
- Proveedor **referenciado** desde Funko; proveedores precargados por semillas.
- Dos implementaciones del repositorio MongoDB: **driver nativo** y **Entity Framework Core**.
- Selección de la implementación mediante `appsettings.json` e inyección de dependencias.

## Enfoque técnico

El proyecto seguirá el contenido trabajado **hasta el tema 12** y las ampliaciones indicadas por el profesor. Se utilizarán controladores MVC, servicios, repositorios, DTOs, mapeadores, inyección de dependencias, gestión de errores y configuración externa.

Tecnologías y patrones previstos:

- **.NET 10 / C# 14** y **ASP.NET Core Web API**.
- **Scrutor** para el registro de servicios mediante inyección de dependencias.
- **CSharpFunctionalExtensions** con `Result` para gestionar errores de negocio (patrón ROP).
- **Fluent API** para configurar el modelo SQL de EF Core.
- **Serilog** para registros de la aplicación.
- **Data Annotations** y **FluentValidation**: incorporación por decidir durante el desarrollo.
- Pruebas de endpoints durante la implementación y pruebas funcionales finales con **Bruno**.

> Esta lista describe el **alcance previsto**, no funcionalidades ya implementadas.