# Challenge Técnico Take‑Home — Gestor de Tickets / Incidencias Internas (WPF + EF Core)

**Duración estimada:** 6–8 horas  
**Perfil evaluado:** C# avanzado, fundamentos sólidos de WPF, Entity Framework Core

---

## ⚠️ Restricciones Tecnológicas (OBLIGATORIAS)

El challenge **debe** implementarse usando **exclusivamente** el siguiente stack:

- **.NET 10** (`net10.0`)
- **Entity Framework Core 10.x** (versión compatible con .NET 10; preferentemente el último patch disponible)
- **SQL Server** (LocalDB, Express o Developer Edition)
- **Persistencia real en base de datos** (no in‑memory)
- **WPF** como UI
- **Patrón MVVM**
  - ❌ No lógica de negocio en *code‑behind*
  - ✅ Commands, Binding, ViewModels, ObservableCollection

**Debe quedar explícito en la solución:**

- La aplicación **persiste datos en SQL Server**
- Se utilizan **migrations de EF Core**
- El proyecto **compila y ejecuta en `net10.0`**

---

## 🎯 Objetivo

Construir una aplicación de escritorio WPF para la **gestión de tickets/incidencias internas** de una organización.  
La aplicación debe permitir crear, consultar, actualizar y cerrar tickets, gestionar comentarios asociados y ofrecer búsquedas y filtros avanzados, manteniendo una UI fluida y un diseño limpio y mantenible.

---

## 🧩 Dominio del Problema

### Contexto

Un equipo interno de IT necesita una herramienta simple pero robusta para registrar y dar seguimiento a incidencias internas (tickets), asignarlas, cambiar su estado y agregar comentarios de seguimiento.

### Entidades mínimas

- **Ticket** (entidad principal)
- **Comentario** (entidad relacionada)

Relación:
- **Ticket (1) → (N) Comentarios**

Estados posibles del Ticket (ejemplo):
- Nuevo
- En Progreso
- En Espera
- Cerrado

El dominio debe justificar:
- Queries no triviales (filtros combinados, búsqueda por texto, includes)
- Asincronía (cargas, búsquedas, exportaciones)
- Manejo de estado (abierto/cerrado, soft delete opcional)

---

## ✅ Requisitos Funcionales Mínimos

### 🖥 Pantalla Principal (Listado de Tickets)

- Listado de tickets desde SQL Server
- Ordenamiento (por fecha, prioridad o estado)
- Filtros combinables:
  - Estado
  - Rango de fechas
  - Texto (título o descripción)
- Botón de **refresco de datos**
- Indicador visual de carga (loading)

---

### ✏️ CRUD de Ticket

- Crear ticket
- Editar ticket
- Eliminar ticket
  - ✅ Confirmación explícita de borrado
- Persistencia inmediata en base de datos

---

### 🔍 Pantalla de Detalle de Ticket

- Mostrar información completa del ticket
- Mostrar **lista de comentarios asociados**
- Agregar / editar comentarios
- Refresco del detalle tras cambios

---

### 🔎 Búsqueda

- Búsqueda por texto libre
- Filtros combinables (estado + fecha + texto)
- Queries ejecutadas con EF Core y LINQ

---

### 🗄 Persistencia con EF Core

- `DbContext` correctamente configurado
- Mapeo de relaciones
- Uso explícito de:
  - `Include()` cuando corresponda
  - `AsNoTracking()` para consultas de solo lectura
- Uso de **migrations**
- Creación automática de base de datos si no existe

---

### 🌱 Datos Iniciales

- Seed de datos o script inicial que incluya:
  - Tickets en distintos estados
  - Algunos tickets con comentarios
- Debe permitir probar la app sin carga manual inicial

---

## 🧪 Requisitos No Funcionales (Calidad)

### WPF / MVVM

- MVVM correcto:
  - `INotifyPropertyChanged`
  - `ObservableCollection`
  - Commands (`ICommand`)
- ❌ Sin lógica de negocio en code‑behind
- Navegación desacoplada (servicio o patrón simple)
- Estados de UI:
  - Loading
  - Error
  - Vacío

### Validaciones y UX

- Validación de inputs:
  - `IDataErrorInfo`, `INotifyDataErrorInfo` o similar
- Mensajes claros al usuario
- Manejo de errores controlado (no crashes)

### Async / Performance

- Uso correcto de `async` / `await`
- UI **no bloqueada**
- Operaciones de base de datos asincrónicas

### EF Core

- Queries eficientes
- Evitar N+1
- Manejo básico de excepciones de base de datos

---

## 🚀 Requisitos Avanzados (C# Avanzado)

Implementación parcial aceptada. Se evaluará diseño y criterio técnico.

### Arquitectura y Plataforma

- **Dependency Injection**
  - `Microsoft.Extensions.DependencyInjection`
  - Servicios (datos, navegación, exportación)
- **Logging**
  - `ILogger`
  - Niveles apropiados
  - Log de errores, operaciones CRUD y exportaciones

### Persistencia Avanzada

- **Soft Delete**
  - Campo `IsDeleted`
  - **Global Query Filters**
  - Opción de “mostrar eliminados” (si aplica)
- **Auditoría básica**
  - `CreatedAt`
  - `UpdatedAt`
  - Implementada en `SaveChanges / SaveChangesAsync`

### Exportación

- Exportar datos a:
  - **JSON**
  - **XML**
- Alcance:
  - Listado filtrado o
  - Ticket con comentarios
- Manejo de errores y feedback al usuario

### WPF y Concurrencia

- Operaciones pesadas en background:
  - Carga inicial
  - Búsquedas complejas
  - Exportación
- Actualización segura de UI:
  - `Dispatcher` / `SynchronizationContext`
- Ejemplo concreto:
  - Exportación async con estado
  - Carga async con indicador

---

## 📦 Entregables Esperados

- Repositorio Git o ZIP con la solución completa
- **README.md** que incluya:
  - Prerrequisitos:
    - SQL Server (LocalDB / Express / Developer)
    - .NET SDK 10
  - Ejemplo de connection string
  - Cómo ejecutar migrations
  - Cómo correr la app
- Documento o sección en README con:
  - Decisiones técnicas y trade‑offs
  - Features implementadas vs pendientes
- Capturas o video corto (opcional)

---

## ✅ Checklist de Implementación

### Setup

- [ ] Proyecto WPF en `net10.0`
- [ ] Paquetes EF Core 10.x configurados
- [ ] Configuración DI básica

### Base de Datos

- [ ] SQL Server configurado
- [ ] Connection string funcional
- [ ] DbContext definido
- [ ] Migrations creadas y aplicadas

### Dominio y Datos

- [ ] Entidades y relaciones modeladas
- [ ] Seed de datos funcional
- [ ] Soft delete (si aplica)
- [ ] Auditoría básica

### Servicios / Datos

- [ ] Repositorios o servicios de acceso a datos
- [ ] Queries con filtros y ordenamiento
- [ ] Uso correcto de Include / AsNoTracking

### UI WPF

- [ ] Views creadas
- [ ] ViewModels por pantalla
- [ ] Commands
- [ ] Navegación desacoplada
- [ ] Estados (loading / error / vacío)

### Funcionalidad

- [ ] CRUD completo de Tickets
- [ ] Manejo de comentarios
- [ ] Búsqueda por texto
- [ ] Filtros combinables
- [ ] Confirmaciones de borrado

### Calidad

- [ ] Validaciones de input
- [ ] Manejo de errores
- [ ] Logging con ILogger
- [ ] Async/Await correcto
- [ ] UI no bloqueada

### Avanzado / Bonus

- [ ] Exportación XML
- [ ] Exportación JSON
- [ ] Concurrencia y Dispatcher
- [ ] Cancelación con `CancellationToken`
- [ ] Paginado con `Skip/Take`

---

## 📊 Criterios de Evaluación (Ponderación)

- **Correctitud funcional:** 25%
- **Calidad MVVM y WPF:** 20%
- **Modelado de datos y EF Core:** 20%
- **Calidad de código C# (async, LINQ, diseño, threads):** 20%
- **Mantenibilidad (estructura, DI, logging):** 10%
- **UX y manejo de errores:** 5%

> La evaluación es **técnica, objetiva y basada en el código entregado**.

---

## ⭐ Bonus (Opcional)

- Cancelación de búsquedas con `CancellationToken`
- Exportación con progreso (`IProgress<T>`)
- Paginado real desde SQL Server con filtros aplicados

