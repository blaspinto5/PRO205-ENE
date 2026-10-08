# Decisiones de arquitectura — Fase 1

**Estado:** decisiones de planificación adoptadas desde el plan maestro. No representan software implementado ni herramientas instaladas/verificadas. La rúbrica prevalece ante cualquier contradicción.

## ADR-01 — Stack y propósito

| Elección | Razón | Límite o consecuencia |
|---|---|---|
| C# y .NET 10 LTS | Mantener la exigencia de C# y la base del plan maestro. | Comprobar SDK e IDE compatibles antes de crear la solución. |
| WPF y MVVM | Separar presentación, estado de pantalla y casos de uso. | Aplicación de escritorio para Windows; no incluir negocio en ventanas o ViewModels. |
| SQLite | Persistencia local sin desplegar un servidor. | Adecuada al alcance local propuesto; reconsiderar si el enunciado exige otro escenario. |
| Entity Framework Core | Implementar acceso a datos y mapeos en Infrastructure. | Elegir versión compatible con el runtime al implementar; Domain no incorpora dependencias del ORM. |
| xUnit | Unificar pruebas unitarias. | No mezclar frameworks; ninguna prueba unitaria depende de UI o base real. |
| Git y GitHub | Registrar evolución y prever un repositorio compartible. | Git local preparado en fase 1; GitHub pendiente de destino y publicación. |
| Draw.io | Elaborar UML editable y exportable. | Revisar semántica, visibilidad y multiplicidades; la herramienta no garantiza corrección. |
| Visual Studio | Entorno previsto para desarrollo WPF. | Verificar herramientas de desarrollo de escritorio y compatibilidad antes de implementar. |

Estas elecciones técnicas son internas; la rúbrica no exige WPF, SQLite, EF Core ni xUnit.

## ADR-02 — Capas y referencias

| Proyecto previsto | Responsabilidad | Referencias permitidas |
|---|---|---|
| `Codice.Domain` | Entidades, reglas, enums y excepciones de dominio; value objects solo si resuelven una necesidad. | Sin referencias a otros proyectos de la solución. |
| `Codice.Application` | Casos de uso, DTOs y contratos de repositorios. | Domain. |
| `Codice.Infrastructure` | DbContext, mapeos y repositorios concretos. | Application y Domain para implementar contratos y persistir entidades. |
| `Codice.Desktop` | Views, ViewModels, comandos, recursos, navegación y composición de la aplicación. | Application; Infrastructure únicamente al configurar dependencias al inicio. |
| `Codice.UnitTests` | Pruebas aisladas de reglas y casos de uso. | Domain y Application. |
| `Codice.IntegrationTests` | Verificación técnica de persistencia cuando aporte valor. | Los proyectos necesarios para la integración probada. |

El diagrama del plan maestro expresa la separación conceptual; esta tabla concreta las referencias necesarias para que Infrastructure implemente contratos de Application. Desktop puede registrar implementaciones al iniciar, pero sus pantallas y ViewModels consumen casos de uso, no repositorios concretos ni DbContext.

Flujo previsto: View → ViewModel → caso de uso de Application → regla de Domain → contrato de repositorio → implementación de Infrastructure. Domain nunca llama a la interfaz.

## ADR-03 — Responsabilidades y errores

Una clase tendrá una responsabilidad principal. Domain protege invariantes; Application coordina operaciones; Infrastructure guarda y recupera datos; Desktop presenta resultados y errores comprensibles. Un botón no calcula un promedio ni valida una regla académica. Las validaciones de negocio se prueban sin interfaz.

No crear abstracciones, servicios ni repositorios sin una necesidad concreta. No decidir aún las entidades definitivas: `Student`, `Course`, `Subject`, `Enrollment`, `Grade` y `AttendanceRecord` son sugerencias del plan pendientes del caso.

Las excepciones tendrán propósito y contexto útil; no se ignorarán con `catch` vacío. La presentación traducirá fallos conocidos a mensajes para el usuario sin trasladar reglas de negocio a WPF.

## ADR-04 — Pruebas y persistencia

Pruebas unitarias con xUnit, AAA, nombres descriptivos, asserts específicos y datos aislados. Los casos de uso se prueban mediante sustitutos de los contratos cuando haga falta. SQLite y EF Core reales se verifican mediante integración, sin sustituir las pruebas unitarias exigidas.

Se verificará primero Domain y Application; persistencia e interfaz se integrarán después de pasar pruebas. El motor debe poder ejecutarse y probarse sin Desktop.

## ADR-05 — Convenciones y cambios

Código en inglés; documentación e interfaz en español cuando corresponda. Aplicar convenciones del plan y `.editorconfig`; no añadir comentarios redundantes. Los cambios de stack, capas o alcance deben registrar motivo, impacto en rúbrica y consecuencias antes de adoptarse. Ninguna mejora técnica justifica omitir un criterio evaluado.

## Implementación de fase 5
Domain/Application net10.0 sin UI ni persistencia concreta; tres repositorios específicos y tiempo inyectado. DateOnly académico, DateTime local de sesiones y DateTimeOffset UTC de auditoría. Tests con reloj fijo y repositorios en memoria. Detalles y límites en docs/03-codice/phase-5-review.md.

## ADR-06 — Entrega portable Windows x64

Decisión del 6 de octubre de 2026, autorizada por el usuario: publicación autocontenida win-x64 en una carpeta completa, con runtime incluido y datos/codice.db junto al ejecutable. Sin trimming, AOT ni archivo único en esta entrega. Ruta derivada de AppContext.BaseDirectory; motor independiente de rutas y del almacenamiento. Cada publicación genera una carpeta nueva y conserva datos anteriores. SDK opcional en .tools/dotnet, separado del paquete de usuario; actualmente el SDK permanece instalado en el perfil del usuario.

Perfil y herramienta preparados, sin aplicación WPF publicada todavía. Arranque, exclusión de instancia, migraciones, permisos y traslado deben implementarse y verificarse en fase 6. Detalles y checklist en [arquitectura portable](../03-codice/portable/portable-architecture.md).

## Implementación de base portable — fase 6

ADR-06 implementada parcialmente: publicación autocontenida comprobada, ruta de base junto al ejecutable, verificación de escritura, bloqueo de archivo y migración inicial. EF Core 10.0.12 con TPC/Guid, décimas enteras e índices; contextos breves y guardado transaccional del curso. ViewModel de inicio consume CatalogService. Consultar phase-6-foundation.md para evidencia, límites y pendientes.

## ADR-07 — Estilos y cierre de presentación

Se mantiene WPF/MVVM con recursos XAML centralizados en Tokens y Controls, cargados por App.xaml. No incorporar React/Tailwind/WebView2: el alcance local puede resolverse sin motor web adicional. Views/Templates vincula siete ViewModels y vistas. Workspace coordina servicios y serializa operaciones; WorkspaceQueryService entrega DTOs sin exponer entidades mutables a las vistas. Publicación y pruebas finales en phase-6-review.md.
