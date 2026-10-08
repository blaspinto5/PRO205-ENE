# PRO205 ENE — Master Project Plan

> **Proyecto:** Evaluación Nacional de Especialidad — PRO205 Taller de Programación  
> **Objetivo:** desarrollar la ENE como un proyecto académico con estándares cercanos a un proyecto profesional real, manteniendo el alcance controlado, la trazabilidad con la rúbrica y una arquitectura modular.  
> **Regla principal:** cada fase termina con una revisión contra rúbrica. No se avanza de fase hasta que exista evidencia verificable de cumplimiento.

---

# 1. Principios del proyecto

Este proyecto se desarrollará con seis fases claramente separadas. Cada fase debe producir entregables concretos, verificables y vinculados a criterios de evaluación.

La prioridad será:

1. Cumplir completamente la rúbrica.
2. Mantener una arquitectura modular y entendible.
3. Mantener trazabilidad entre requerimientos, diseño, código, pruebas y documentación.
4. Evitar sobreingeniería que no aporte a la evaluación.
5. Dejar una base suficientemente sólida para que el proyecto pueda mostrarse posteriormente en GitHub.

La definición de terminado será:

```text
Rúbrica
   =
Entregable
   =
Evidencia
```

Si alguno de los tres elementos falta, la fase no se considera terminada.

---

# 2. Reglas no negociables

## 2.1 Idioma

### Código

Todo elemento relacionado con código debe estar en **inglés**:

- nombres de clases;
- interfaces;
- métodos;
- propiedades;
- variables;
- parámetros;
- namespaces;
- nombres de proyectos;
- nombres de carpetas técnicas;
- nombres de pruebas;
- DTOs;
- servicios;
- repositorios;
- excepciones;
- enums;
- archivos `.cs`;
- ramas técnicas de Git cuando corresponda.

Ejemplos correctos:

```csharp
Student
AttendanceRecord
Grade
RegisterStudent()
CalculateAverage()
studentId
attendanceDate
IStudentRepository
```

Ejemplos incorrectos:

```csharp
Estudiante
RegistroAsistencia
Calificacion
RegistrarEstudiante()
CalcularPromedio()
idEstudiante
```

### Documentación

La documentación académica puede estar en **español**:

- análisis;
- justificaciones;
- explicación de UML;
- requerimientos;
- manual técnico;
- comentarios para la entrega;
- infografías;
- textos exigidos por la ENE.

### Interfaz

La interfaz de usuario podrá estar en **español**, porque el sistema está pensado para usuarios del Instituto Códice.

Ejemplo:

```text
Registrar estudiante
Asistencia
Calificaciones
Reportes
```

Internamente el código seguirá en inglés.

---

# 3. Política de comentarios

## 3.1 Código de producción

Los comentarios estarán **limitados al mínimo**.

La regla general es:

> Si el código necesita un comentario para explicar qué hace, primero se debe intentar mejorar el nombre, dividir el método o reducir su complejidad.

No se aceptan comentarios redundantes.

Incorrecto:

```csharp
// Add student to the list
students.Add(student);
```

Correcto:

```csharp
students.Add(student);
```

Los comentarios en código de producción solo se permiten cuando explican:

- una decisión técnica no evidente;
- una restricción externa;
- una regla de negocio difícil de inferir;
- un workaround inevitable;
- una razón importante que no puede expresarse mediante nombres o estructura.

No se usarán comentarios para narrar línea por línea el código.

---

## 3.2 Pruebas unitarias

En pruebas unitarias se permiten y recomiendan comentarios de estructura:

```csharp
// Arrange

// Act

// Assert
```

Esto se hace porque facilita la lectura académica y evidencia una estructura de prueba correcta.

Ejemplo:

```csharp
[Fact]
public void CalculateAverage_WhenGradesExist_ReturnsCorrectAverage()
{
    // Arrange
    var student = new Student();
    student.AddGrade(5.0);
    student.AddGrade(6.0);
    student.AddGrade(7.0);

    // Act
    var result = student.CalculateAverage();

    // Assert
    Assert.Equal(6.0, result);
}
```

No se agregarán comentarios innecesarios dentro de cada bloque.

---

# 4. Stack tecnológico oficial del proyecto

La parte funcional del proyecto Códice se estandarizará con:

```text
Language            C#
Runtime             .NET 10 LTS
Desktop UI          WPF
UI Pattern          MVVM
Database            SQLite
ORM                 Entity Framework Core
Unit Testing        xUnit
Version Control     Git
Repository          GitHub
UML                 Draw.io
IDE                  Visual Studio
```

## 4.1 Decisiones técnicas

### C#

Se utiliza porque la ENE exige una aplicación en C#.

### .NET 10 LTS

Será la base del proyecto.

### WPF

Se utilizará en lugar de Windows Forms porque permite una separación más clara entre interfaz y lógica y favorece una estructura modular.

### MVVM

Se utilizará para evitar lógica de negocio dentro de las ventanas.

La interfaz no debe contener reglas de negocio.

### SQLite

Se utilizará como motor de persistencia local.

Ventajas:

- liviano;
- no necesita servidor;
- fácil de distribuir;
- suficiente para el alcance académico;
- permite demostrar persistencia real.

### Entity Framework Core

Se utilizará para separar la lógica de dominio de la implementación concreta de base de datos.

### xUnit

Será el framework estándar para pruebas unitarias.

No se mezclarán xUnit, NUnit y MSTest dentro del mismo proyecto.

---

# 5. Arquitectura

El sistema Códice seguirá una arquitectura por capas.

```text
Codice.Desktop
      |
      v
Codice.Application
      |
      v
Codice.Domain
      ^
      |
Codice.Infrastructure
```

La dirección de dependencias debe mantenerse controlada.

---

## 5.1 Codice.Domain

Contiene el núcleo de negocio.

Debe incluir:

- entidades;
- reglas de negocio;
- value objects si son necesarios;
- enums;
- excepciones de dominio;
- comportamiento propio de las entidades.

Ejemplos:

```text
Student
Course
Subject
Enrollment
Grade
AttendanceRecord
```

Reglas:

- no depende de WPF;
- no depende de SQLite;
- no depende de Entity Framework Core cuando pueda evitarse;
- no contiene código de interfaz;
- debe poder probarse de forma aislada.

---

## 5.2 Codice.Application

Contiene los casos de uso.

Ejemplos:

```text
StudentService
AttendanceService
GradeService
ReportService
```

También puede contener:

```text
DTOs
Repository interfaces
Application services
Use cases
```

Ejemplos de interfaces:

```csharp
IStudentRepository
IAttendanceRepository
IGradeRepository
```

Regla:

> Application conoce contratos, pero no debe conocer detalles concretos de SQLite o WPF.

---

## 5.3 Codice.Infrastructure

Contiene implementaciones técnicas.

Responsabilidades:

```text
SQLite
Entity Framework Core
DbContext
Repositories
Persistence configuration
Database mappings
```

Ejemplos:

```text
CodiceDbContext
StudentRepository
GradeRepository
AttendanceRepository
```

Si mañana se reemplaza SQLite por SQL Server o PostgreSQL, el dominio no debería necesitar ser reescrito.

---

## 5.4 Codice.Desktop

Contiene la aplicación WPF.

Estructura:

```text
Views
ViewModels
Commands
Navigation
UI Services
Resources
```

Reglas:

- una View no contiene reglas de negocio;
- un botón no calcula directamente un promedio;
- los ViewModels coordinan la interacción;
- Application ejecuta los casos de uso;
- Domain contiene la lógica de negocio.

---

# 6. Estructura de carpetas

La estructura base será:

```text
PRO205-ENE/
|
|-- README.md
|-- .gitignore
|-- .editorconfig
|-- Codice.sln
|
|-- docs/
|   |
|   |-- 00-planning/
|   |   |-- project-plan.md
|   |   |-- rubric-matrix.md
|   |   |-- architecture-decisions.md
|   |   `-- phase-checklists.md
|   |
|   |-- 01-huellitas/
|   |   |-- analysis/
|   |   |-- preliminary-uml/
|   |   |-- final-uml/
|   |   |-- technical-justification/
|   |   `-- covered-requirements/
|   |
|   |-- 02-tecnobox/
|   |   |-- lifecycle/
|   |   |-- oop-design/
|   |   |-- infographic/
|   |   `-- technical-commentary/
|   |
|   `-- 03-codice/
|       |-- requirements/
|       |-- uml/
|       |-- screenshots/
|       |-- test-evidence/
|       `-- technical-manual/
|
|-- src/
|   |-- Codice.Domain/
|   |-- Codice.Application/
|   |-- Codice.Infrastructure/
|   `-- Codice.Desktop/
|
`-- tests/
    |-- Codice.UnitTests/
    `-- Codice.IntegrationTests/
```

`Codice.IntegrationTests` puede existir aunque la rúbrica no lo exija. No debe reemplazar a `Codice.UnitTests`.

---

# 7. Convenciones de código

## 7.1 Clases

PascalCase:

```csharp
Student
AttendanceRecord
PostAdoptionFollowUp
```

## 7.2 Métodos

PascalCase y verbo explícito:

```csharp
RegisterStudent()
UpdateStudent()
RecordAttendance()
AddGrade()
CalculateAverage()
GenerateReport()
```

Evitar nombres ambiguos:

```csharp
Process()
DoStuff()
Handle()
ExecuteThing()
```

---

## 7.3 Variables y parámetros

camelCase:

```csharp
studentId
courseId
attendanceDate
gradeValue
```

---

## 7.4 Interfaces

Prefijo `I`:

```csharp
IStudentRepository
IGradeRepository
IAttendanceRepository
```

---

## 7.5 Booleanos

Deben expresar condición:

```csharp
isActive
isPresent
hasGrades
canEnroll
```

Evitar:

```csharp
status
flag
check
```

cuando sean booleanos.

---

## 7.6 Métodos

Un método debe tener una responsabilidad clara.

Evitar métodos que hagan simultáneamente:

```text
validar
guardar
mostrar ventana
calcular
crear reporte
```

Si ocurre, debe dividirse.

---

# 8. Reglas de modularización

Aplicaremos estas reglas:

1. Una clase debe tener una responsabilidad principal.
2. La interfaz no contiene lógica de negocio.
3. La persistencia no define reglas de negocio.
4. El dominio no depende de la interfaz.
5. El dominio no debe depender del motor de base de datos.
6. Los repositorios se consumen mediante interfaces.
7. Se evitarán clases genéricas tipo `Helper`, `Utils` o `Manager` sin responsabilidad clara.
8. Se evitarán archivos excesivamente grandes.
9. Las dependencias entre módulos deben ser explícitas.
10. No se agregará una abstracción si no resuelve un problema concreto.

---

# 9. Estrategia de requerimientos y trazabilidad

Todos los requerimientos deberán tener ID.

Ejemplo:

```text
RF01
RF02
RF03
RNF01
RNF02
```

Se mantendrá una matriz de trazabilidad:

```text
Requirement
    ->
Class / Module
    ->
Method / Use Case
    ->
Unit Test
    ->
UI Screen
```

Ejemplo:

```text
RF04 Record Attendance
    ->
AttendanceRecord
    ->
AttendanceService.RecordAttendance()
    ->
RecordAttendance_WhenDataIsValid_CreatesRecord()
    ->
AttendanceView
```

Esta matriz será obligatoria antes de terminar el proyecto.

---

# 10. Política de pruebas

Las pruebas no se dejarán para el final.

Las pruebas forman parte del diseño.

---

## 10.1 Framework

```text
xUnit
```

No se mezclará con otros frameworks.

---

## 10.2 Proyecto

Todas las pruebas unitarias estarán dentro de:

```text
tests/Codice.UnitTests/
```

Posible estructura:

```text
Codice.UnitTests/
|
|-- Domain/
|   |-- StudentTests.cs
|   |-- GradeTests.cs
|   `-- AttendanceRecordTests.cs
|
`-- Application/
    |-- StudentServiceTests.cs
    |-- GradeServiceTests.cs
    `-- AttendanceServiceTests.cs
```

---

## 10.3 Nomenclatura de pruebas

Formato estándar:

```text
MethodName_Scenario_ExpectedResult
```

Ejemplos:

```csharp
CalculateAverage_WhenGradesExist_ReturnsCorrectAverage()

AddGrade_WhenGradeIsInvalid_ThrowsException()

RecordAttendance_WhenRecordAlreadyExists_PreventsDuplicate()

RegisterStudent_WhenDataIsValid_PersistsStudent()
```

Los nombres deben describir:

1. qué se está probando;
2. bajo qué condición;
3. qué resultado se espera.

---

## 10.4 Estructura AAA

Toda prueba debe ser legible mediante:

```text
Arrange
Act
Assert
```

Ejemplo:

```csharp
[Fact]
public void AddGrade_WhenGradeIsValid_AddsGrade()
{
    // Arrange
    var student = new Student();

    // Act
    student.AddGrade(6.5);

    // Assert
    Assert.Single(student.Grades);
}
```

---

## 10.5 Independencia

Una prueba unitaria:

- no depende de otra prueba;
- no necesita que otra prueba se ejecute primero;
- no comparte estado mutable;
- no debe depender de la interfaz gráfica;
- no debe depender de una base de datos real;
- debe ejecutarse rápidamente.

---

## 10.6 Qué se probará

Como mínimo:

### Student

```text
CreateStudent_WhenDataIsValid_CreatesStudent
CreateStudent_WhenNameIsEmpty_ThrowsException
```

### Grade

```text
AddGrade_WhenGradeIsValid_AddsGrade
AddGrade_WhenGradeIsInvalid_ThrowsException
CalculateAverage_WhenGradesExist_ReturnsCorrectAverage
```

### Attendance

```text
RecordAttendance_WhenDataIsValid_CreatesRecord
RecordAttendance_WhenRecordAlreadyExists_PreventsDuplicate
```

### Application

```text
RegisterStudent_WhenDataIsValid_PersistsStudent
UpdateGrade_WhenGradeExists_UpdatesGrade
```

Meta interna:

```text
8 a 12 pruebas unitarias útiles
```

La rúbrica exige menos, pero nuestra meta será superior al mínimo.

---

## 10.7 Qué NO debe probar una prueba unitaria

No se probarán directamente en unit tests:

```text
WPF windows
XAML
SQLite real
Entity Framework real
botones
diálogos
filesystem real
```

Para esos casos se podrán utilizar pruebas de integración cuando aporten valor.

---

## 10.8 Asserts

Cada prueba debe usar assertions específicas.

Preferir:

```csharp
Assert.Equal()
Assert.True()
Assert.False()
Assert.Single()
Assert.Empty()
Assert.NotNull()
Assert.Throws<T>()
```

Evitar asserts genéricos que dificulten saber qué falló.

---

## 10.9 Pruebas negativas

No se probará solo el camino correcto.

También se probarán errores de negocio.

Ejemplos:

```text
nota fuera de rango
estudiante sin nombre
asistencia duplicada
curso inexistente
registro incompleto
```

---

## 10.10 Evidencia

Antes de la entrega se debe ejecutar:

```bash
dotnet test
```

La evidencia debe mostrar:

```text
Passed
Failed: 0
Total
```

Se guardará captura dentro de:

```text
docs/03-codice/test-evidence/
```

---

# 11. Git

## 11.1 Commits

Los commits deben ser pequeños y descriptivos.

Formato recomendado:

```text
feat: add student registration
feat: implement attendance recording
test: add grade validation tests
docs: add Huellitas UML justification
refactor: extract grade calculation logic
fix: prevent duplicate attendance records
```

Evitar:

```text
cambios
avance
final
final2
ahora_si
cosas
```

---

## 11.2 Commits de código

Los mensajes técnicos pueden estar en inglés.

---

## 11.3 Regla

No realizar un único commit al finalizar todo el proyecto.

Git debe evidenciar evolución del trabajo.

---

# 12. Manejo de errores

No se deben ocultar errores con:

```csharp
catch
{
}
```

Las excepciones deben:

- tener propósito;
- entregar información útil;
- manejarse en la capa correcta.

Las validaciones de dominio deben estar cerca de la lógica que protegen.

---

# 13. FASE 1 — Planning & Architecture

## Objetivo

Establecer la base del proyecto antes de producir entregables académicos.

## Actividades

- definir stack;
- crear repositorio;
- crear estructura de carpetas;
- definir arquitectura;
- definir idioma;
- definir convenciones;
- definir estrategia de pruebas;
- crear matriz rúbrica;
- crear documento de decisiones técnicas.

## Entregables

```text
project-plan.md
rubric-matrix.md
architecture-decisions.md
phase-checklists.md
```

## Recapitulación

Antes de cerrar la fase:

- [ ] Stack definido.
- [ ] Arquitectura definida.
- [ ] Estructura de carpetas definida.
- [ ] Política de idioma definida.
- [ ] Política de comentarios definida.
- [ ] Convenciones de código definidas.
- [ ] Estrategia de pruebas definida.
- [ ] Estrategia Git definida.
- [ ] Matriz rúbrica → entregables definida.
- [ ] No existe lógica dependiente innecesariamente de WPF.

**Gate de fase:** si falta cualquiera de estos elementos, no se inicia la Fase 2.

---

# 14. FASE 2 — Fundación Huellitas

## Objetivo

Completar los criterios relacionados con análisis POO, relaciones y UML.

## Trabajo

Crear:

- análisis del dominio;
- esquema preliminar;
- justificación técnica;
- UML final;
- lista de requerimientos cubiertos.

## UML

Debe incluir al menos:

```text
Association
Aggregation
Composition
Inheritance
```

Con:

```text
5+ classes
attributes
methods
visibility
multiplicity
generalization/specialization
```

## Recapitulación según rúbrica

- [ ] Existen 5 o más clases.
- [ ] Las clases están correctamente estructuradas.
- [ ] Existen al menos 3 tipos de relaciones.
- [ ] Existe jerarquía clara de generalización/especialización.
- [ ] La justificación técnica explica cada relación.
- [ ] Cada relación tiene fundamento.
- [ ] El UML contiene atributos.
- [ ] El UML contiene métodos.
- [ ] El UML utiliza `+`, `-`, `#` correctamente.
- [ ] Las multiplicidades están correctamente representadas.
- [ ] La notación UML es consistente.
- [ ] Existe lista de requerimientos cubiertos.

**Gate de fase:** solo avanzar si el resultado apunta a nivel Sobresaliente.

---

# 15. FASE 3 — Tecnobox

## Objetivo

Relacionar diseño orientado a objetos con el ciclo de vida del software.

## Etapas mínimas a trabajar

```text
Analysis
Design
Implementation
Testing
Maintenance
```

Puede incluirse además:

```text
Requirements
Deployment
```

## Conceptos POO a relacionar

```text
Abstraction
Encapsulation
Inheritance
Polymorphism
Modularity
Reusability
Maintainability
```

## Entregables

```text
OO lifecycle comparison table
Written lifecycle explanation
OOP lifecycle infographic
Technical commentary
```

## Recapitulación según rúbrica

- [ ] Se identifican al menos 4 etapas clave.
- [ ] Las etapas están correctamente ubicadas.
- [ ] Cada etapa tiene objetivo.
- [ ] Cada etapa tiene actividades.
- [ ] Cada etapa tiene productos.
- [ ] El esquema es claro.
- [ ] El lenguaje técnico es adecuado.
- [ ] El mapa representa todas las etapas relevantes.
- [ ] Existe relación directa entre etapas y POO.
- [ ] Se explica modularidad.
- [ ] Se explica encapsulación.
- [ ] Se explica reutilización.
- [ ] Se argumenta cómo POO mejora calidad y mantenibilidad.

**Gate de fase:** todos los criterios deben tener evidencia.

---

# 16. FASE 4 — Códice: Requirements & UML

## Objetivo

Transformar el caso Códice en un diseño técnico trazable.

## Requerimientos

Meta:

```text
8 functional requirements
4 non-functional requirements
```

Mínimo exigido:

```text
6 total
4 functional
```

Cada requerimiento debe contener:

```text
ID
Name
Type
Description
Priority
```

Ejemplo:

```text
RF04
Name: Record Attendance
Type: Functional
Description: The system allows attendance to be recorded for a student on a specific date.
Priority: High
```

## UML

Modelo inicial sugerido:

```text
Student
Course
Subject
Enrollment
Grade
AttendanceRecord
```

Se agregarán otras clases solo si tienen responsabilidad real.

## Matriz de trazabilidad

Debe comenzar en esta fase.

```text
Requirement -> Class -> Method -> Test -> Screen
```

## Recapitulación según rúbrica

- [ ] Existen 6 o más requerimientos.
- [ ] Existen al menos 4 funcionales.
- [ ] Todos están correctamente clasificados.
- [ ] Todos tienen nombre.
- [ ] Todos tienen tipo.
- [ ] Todos tienen descripción.
- [ ] Todos tienen prioridad.
- [ ] La redacción es técnica y clara.
- [ ] El UML tiene 5 o más clases.
- [ ] Cada clase tiene atributos.
- [ ] Cada clase relevante tiene métodos.
- [ ] Las relaciones están correctamente representadas.
- [ ] La notación UML es estándar.
- [ ] El UML refleja los requerimientos.
- [ ] Comenzó la matriz de trazabilidad.

**Gate de fase:** no iniciar implementación si no existe trazabilidad entre requisitos y modelo.

---

# 17. FASE 5 — Domain, Application & Unit Tests

## Objetivo

Construir el motor del sistema antes de desarrollar la interfaz.

## Orden obligatorio

```text
Domain
   ->
Application
   ->
Unit Tests
```

La interfaz todavía no debe controlar la lógica.

## Desarrollo

Construir:

```text
Student
Course
Subject
Enrollment
Grade
AttendanceRecord
```

y los casos de uso necesarios.

Servicios posibles:

```text
StudentService
AttendanceService
GradeService
ReportService
```

## Regla crítica

El motor debe seguir funcionando aunque `Codice.Desktop` sea eliminado.

## Pruebas

Meta:

```text
8-12 unit tests
```

Todas deben:

- usar xUnit;
- seguir AAA;
- tener nombre descriptivo;
- ser independientes;
- ejecutar sin errores;
- validar requisitos reales.

## Recapitulación según rúbrica

- [ ] Existen al menos 3 pruebas unitarias funcionales.
- [ ] Meta interna de 8-12 pruebas cumplida o justificada.
- [ ] Los asserts son específicos.
- [ ] Los nombres son descriptivos.
- [ ] Se usa Arrange / Act / Assert.
- [ ] Las pruebas validan requerimientos.
- [ ] Existen pruebas positivas.
- [ ] Existen pruebas negativas.
- [ ] No dependen de WPF.
- [ ] No dependen de SQLite real.
- [ ] `dotnet test` finaliza sin errores.
- [ ] Existe evidencia de ejecución.
- [ ] Domain no depende de UI.
- [ ] Application no depende de UI.
- [ ] La lógica está modularizada.

**Gate de fase:** `dotnet test` debe estar verde antes de integrar la interfaz.

---

# 18. FASE 6 — Persistence, WPF & Final Delivery

## Objetivo

Integrar la arquitectura completa y producir la entrega final.

## Persistencia

Implementar:

```text
SQLite
Entity Framework Core
Repositories
```

## Interfaz

Pantallas propuestas:

```text
Dashboard
Students
Attendance
Grades
Reports
```

## Requerimientos funcionales

La rúbrica exige al menos 3 implementados.

Meta interna:

```text
Register/Edit Student
Record Attendance
Register/Edit Grade
Calculate Average
Generate Report
```

## Documentación

Preparar:

```text
Technical Manual
Architecture Diagram
Class Diagram
Requirements Matrix
Unit Test Evidence
Application Screenshots
README
```

## Recapitulación según rúbrica

- [ ] Aplicación ejecuta correctamente.
- [ ] Implementa al menos 3 requerimientos funcionales.
- [ ] La estructura POO es válida.
- [ ] La interfaz es funcional.
- [ ] La interfaz es clara.
- [ ] Existe persistencia.
- [ ] Existe documentación técnica.
- [ ] Las clases principales están documentadas.
- [ ] Las pantallas están descritas.
- [ ] El flujo de uso está descrito.
- [ ] Existe ejecutable funcional.
- [ ] Existe manual técnico.
- [ ] La matriz de trazabilidad está completa.
- [ ] Las pruebas unitarias continúan pasando.

### Criterios transversales

- [ ] Se evidencia modularidad.
- [ ] Se evidencia extensibilidad.
- [ ] Se utilizan herramientas digitales adecuadamente.
- [ ] La solución está organizada.
- [ ] Las decisiones importantes están justificadas.
- [ ] Se considera una propuesta sostenible y mantenible.
- [ ] La documentación muestra autonomía y responsabilidad en el proceso.

---

# 19. Definition of Done global

El proyecto no se considera terminado hasta que:

```text
dotnet build
```

termine correctamente.

Y:

```text
dotnet test
```

termine con:

```text
Failed: 0
```

Además:

- [ ] Todos los criterios de la rúbrica tienen evidencia.
- [ ] No existen requerimientos críticos sin implementación o explicación.
- [ ] Todos los identificadores de código están en inglés.
- [ ] No existen comentarios redundantes.
- [ ] Las pruebas siguen la convención establecida.
- [ ] La arquitectura mantiene separación de responsabilidades.
- [ ] El UML corresponde al código cuando aplica.
- [ ] Los requerimientos corresponden al código.
- [ ] Las pruebas corresponden a requerimientos.
- [ ] La interfaz llama a Application, no implementa reglas de negocio.
- [ ] La persistencia está aislada en Infrastructure.
- [ ] La documentación académica está organizada.
- [ ] El repositorio está limpio y entendible.

---

# 20. Reglas para trabajar con ChatGPT durante el proyecto

Cuando se solicite ayuda a ChatGPT dentro de este proyecto, se deben respetar estas reglas:

1. No cambiar el stack sin justificarlo.
2. No cambiar la arquitectura de forma implícita.
3. Todo código nuevo debe estar en inglés.
4. Toda propuesta debe indicar en qué capa corresponde.
5. No colocar lógica de negocio en WPF.
6. No crear repositorios concretos dentro de Domain.
7. No crear comentarios redundantes.
8. Las pruebas deben usar xUnit.
9. Las pruebas deben seguir AAA.
10. Todo nuevo requerimiento debe tener ID.
11. Todo requerimiento implementado debe ser trazable a código y pruebas.
12. Antes de cerrar una fase, se debe revisar su checklist.
13. No asumir que una fase está terminada porque el código compila.
14. Si una decisión contradice la rúbrica, prevalece la rúbrica.
15. Si una mejora técnica agrega complejidad pero no aporta valor, no implementarla.
16. Antes de generar código grande, definir primero responsabilidad y ubicación.
17. Evitar archivos gigantes.
18. Evitar clases `Helper`, `Utils`, `Manager` sin propósito específico.
19. Mantener la solución ejecutable durante todo el desarrollo.
20. Nunca sacrificar claridad para aparentar mayor complejidad técnica.

---

# 21. Flujo de trabajo

```text
FASE 1
Planning & Architecture
        |
        v
RECAP + RUBRIC CHECK
        |
        v
FASE 2
Huellitas — OOP & UML
        |
        v
RECAP + RUBRIC CHECK
        |
        v
FASE 3
Tecnobox — Software Lifecycle
        |
        v
RECAP + RUBRIC CHECK
        |
        v
FASE 4
Codice — Requirements & UML
        |
        v
RECAP + RUBRIC CHECK
        |
        v
FASE 5
Codice — Domain, Application & Unit Tests
        |
        v
RECAP + RUBRIC CHECK
        |
        v
FASE 6
Codice — Persistence, WPF & Documentation
        |
        v
FINAL RUBRIC AUDIT
```

---

# 22. Regla final

Este proyecto debe mantenerse:

```text
Clear
Modular
Testable
Traceable
Maintainable
Rubric-Compliant
```

La complejidad solo se agrega cuando existe una razón técnica o académica concreta.

El objetivo no es construir el sistema más complejo posible.

El objetivo es construir una solución **correcta, defendible, modular, probada y claramente alineada con la rúbrica**.
