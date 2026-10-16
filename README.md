# FunkoApp — Parte 1: API REST con SQL

Primera parte de la Práctica de Desarrollo Web en Entorno Servidor. Su objetivo es construir una **API REST de Funkos** con ASP.NET Core, controladores MVC y persistencia SQL mediante Entity Framework Core.


## Funcionalidades previstas

### Funkos

- Listar todos los Funkos y consultar uno por su identificador.
- Buscar Funkos por nombre mediante un parámetro de consulta.
- Crear y actualizar Funkos.
- Eliminar Funkos de forma lógica mediante `IsDeleted`.

Cada Funko tendrá, como mínimo: `Id`, `Nombre`, `Precio`, `Stock`, `CategoriaId`, `Categoria`, `IsDeleted`, `CreatedAt` y `UpdatedAt`.

### Categorías

- Listar todas las categorías y consultar una por su identificador.
- Cargar las categorías iniciales mediante **semillas** (*seed data*).
- No se expondrán operaciones POST, PUT ni DELETE para categorías en esta parte.

## Relación entre entidades

La relación entre **Funko** y **Categoria** es **uno a muchos (1:N)**:

- Un Funko pertenece a **una categoría**.
- Una categoría puede estar asociada a **muchos Funkos**.
- `Funko` guarda `CategoriaId` (clave foránea) y la referencia `Categoria`.
- `Categoria` puede incluir la colección de Funkos asociados.

Esta relación se configurará en el `DbContext` utilizando **Fluent API**, siguiendo el ejemplo `Producto`–`Categoria` de `TiendaDawApi-NetCore`.

## Endpoints previstos

| Método | Ruta | Operación | Éxito |
| --- | --- | --- | --- |
| `GET` | `/api/funkos` | Listar Funkos | `200 OK` |
| `GET` | `/api/funkos/{id}` | Obtener un Funko | `200 OK` |
| `GET` | `/api/funkos?nombre=...` | Buscar por nombre | `200 OK` |
| `POST` | `/api/funkos` | Crear un Funko | `201 Created` |
| `PUT` | `/api/funkos/{id}` | Actualizar un Funko completo | `200 OK` |
| `DELETE` | `/api/funkos/{id}` | Borrado lógico de un Funko | `204 No Content` |
| `GET` | `/api/categorias` | Listar categorías | `200 OK` |
| `GET` | `/api/categorias/{id}` | Obtener una categoría | `200 OK` |

Además de las respuestas de éxito, se contemplarán errores de validación, recursos no encontrados y conflictos de negocio, utilizando los códigos HTTP correspondientes.

## Tecnologías y patrones previstos

| Tecnología / patrón | Uso |
| --- | --- |
| ASP.NET Core / C# 14 | Desarrollo de la API con controladores MVC. |
| Entity Framework Core | Persistencia en PostgreSQL. |
| Fluent API | Relaciones, claves, restricciones y configuración del modelo. |
| Migraciones y semillas | Crear y evolucionar el esquema, e insertar categorías iniciales. |
| Scrutor | Registrar servicios y repositorios mediante inyección de dependencias. |
| CSharpFunctionalExtensions (`Result`) | Gestionar resultados y errores de negocio mediante ROP. |
| DTOs y mapeadores | Separar los modelos internos de los datos de entrada y salida. |
| Serilog | Registrar operaciones y errores de la API. |
| Data Annotations / FluentValidation | Validaciones por valorar durante la implementación. |
| Bruno | Comprobación incremental de endpoints y pruebas funcionales al final. |

## Organización del código (propuesta inicial)

```text
FunkoApp/
├── Controllers/
│   ├── FunkosController.cs
│   └── CategoriasController.cs
├── Models/
├── Factory/
│   └── Seed/
├── Dtos/
├── Mappers/
├── Repositories/
├── Services/
├── Errors/
├── Infrastructures/
├── Validators/          # Si se incorporan validadores propios
├── Program.cs
└── appsettings.json
```

La estructura se ajustará cuando se creen los archivos, tomando como referencia la organización de la API de ejemplo del profesor.


## Ejecución y pruebas

Las instrucciones concretas de arranque, configuración de la base de datos, migraciones y pruebas se incorporarán **cuando la implementación esté disponible y verificada**. No se documentan comandos de arranque como si ya se hubieran probado.


## Evolución posterior

Una vez terminada y comprobada esta parte, se creará la rama `parte-2-mongo` **a partir de `parte-1-sql`**. Allí se refactorizará la persistencia para MongoDB, con categorías embebidas, proveedores referenciados y selección entre el driver nativo y EF Core desde la configuración.
