# Base de fase 6 — implementación y orden del proyecto

Fecha: 6 de octubre de 2026. **Registro histórico de la base anterior al cierre.** Estado actual: [fase 6 completada internamente](phase-6-review.md), con interfaz completa y 55 casos aprobados. El resto conserva el diagnóstico de la preparación.

## Resultado

La solución tiene cuatro capas productivas y dos proyectos de pruebas. Infrastructure implementa SQLite/EF Core 10.0.12; Desktop tiene un arranque portable y una pantalla inicial MVVM de consulta del catálogo. La interfaz académica completa y el manual final continúan pendientes.

| Capa | Responsabilidad y dependencias |
|---|---|
| Domain | Nueve entidades y reglas; no referencia otras capas ni EF/WPF. |
| Application | Servicios, informes, catálogo y contratos; referencia Domain. |
| Infrastructure | Contexto, migraciones, repositorios, rutas y bloqueo de base; referencia Application y, transitivamente, Domain. |
| Desktop | Arranque, composición, ventana y ViewModel; usa Application y configura Infrastructure en App. |
| UnitTests | Reglas y coordinación mediante sustitutos. |
| IntegrationTests | SQLite real en carpetas temporales aisladas; no altera bases del usuario. |

Los ViewModels no usan DbContext ni calculan reglas académicas. La pantalla inicial solicita un resumen a CatalogService, muestra conteos/cursos y permite actualizar. No presenta todavía formularios de registro, notas, asistencia ni informes individuales.

## Persistencia

- Migración inicial versionada y snapshot de EF en `src/Codice.Infrastructure/Migrations/`. Inicialización mediante MigrateAsync, nunca borrado/recreación de una base existente.
- IDs Guid conservados al recuperar; constructores privados y setters privados permiten materialización sin abrir modificación pública. Las operaciones públicas del dominio mantienen sus validaciones.
- Herencia de registros mediante TPC: tablas separadas para notas y asistencia, con identidad Guid.
- Notas almacenadas como **décimas enteras**: 5,4 se almacena como 54 y vuelve a decimal. No se convierte a coma flotante.
- Claves foráneas, índices únicos para códigos, cursos, matrículas, evaluaciones, sesiones y registros; checks para fechas del curso, rango de nota y estado de asistencia.
- Evaluaciones indexadas por nombre normalizado con UpperInvariant, incluyendo acentos. El dominio conserva el nombre original.
- Guardado del curso y sus registros en una transacción. Referencias a estudiantes/asignaturas existentes no se modifican al guardar un curso, evitando sobrescribir nombres desde un contexto antiguo.
- Contextos breves por operación, sin pooling; carga completa de cursos con resolución de identidad y consultas divididas. Adecuado al prototipo local; el volumen y la concurrencia no están certificados para producción.

Las reglas entre cursos y fechas de matrícula siguen protegidas por Domain/Application. No se afirma que cada regla académica esté duplicada en una restricción SQL. Las ediciones concurrentes dentro de una instancia deben serializarse en los futuros formularios; no hay resolución de conflictos entre usuarios ni edición sobre red.

## Arranque portable

AppContext.BaseDirectory determina la raíz. `datos/codice.db` se construye mediante una ruta absoluta y SqliteConnectionStringBuilder. Antes de abrir interfaz se comprueba escritura, se adquiere un bloqueo de archivo exclusivo para la carpeta y se aplican migraciones. El bloqueo y los contextos se liberan al cerrar.

Los errores de inicio se presentan en español. No se cambia silenciosamente a una carpeta de datos distinta. `--verify-startup` permite comprobar inicialización y consultas sin abrir una ventana; devuelve 0 si termina correctamente y 3 si falla.

La publicación autocontenida incluye runtime .NET y Windows Desktop 10.0.12 y bibliotecas nativas de SQLite. Cada edición se genera en una carpeta nueva, sin bases existentes ni SDK. La publicación de preparación **no es la entrega final**.

## Verificación realizada

**Compilación: cero errores y advertencias. 49 casos aprobados: 41 unitarios y 8 de integración.** [Registro](test-evidence/current/verification-output.txt), [unitarias actuales](test-evidence/current/unit-tests.trx) e [integración](test-evidence/current/integration-tests.trx). La evidencia histórica de fase 5 se conserva separada.

Las ocho pruebas de integración comprueban recuperación de identidad/vínculos e informes, corrección sin duplicados, conservación de nombres editados por separado, unicidad de código, reversión ante clave foránea inexistente, checks de datos inválidos, copia/restauración con integrity_check y migración repetible, y exclusión de una segunda instancia sobre la misma carpeta.

El ejecutable publicado arrancó con salida 0 en una copia cuya ruta contiene espacios y acentos, con directorio de trabajo distinto. Creó la base en datos junto a esa copia. La traza del host confirma carga de coreclr.dll desde la carpeta portable y ejecución autocontenida. El paquete original permanece sin base de trabajo. [Registro de portabilidad](test-evidence/current/portable-startup.json).

Esta comprobación se hizo en el equipo de desarrollo; no sustituye una prueba en otro Windows compatible sin .NET instalado. No se ha hecho revisión visual de la ventana ni comprobación manual de formularios inexistentes.

## Orden de carpetas y herramientas

La raíz conserva README, solución/configuración y seis carpetas principales: src, tests, docs, tools, output y entrega. Fuentes del plan maestro y rúbrica se trasladaron a `docs/00-planning/sources/`, sin eliminarlas. Los temporales se trasladaron a `.tools/work/`; SDK/herramientas opcionales y publicaciones antiguas quedan fuera de la navegación principal y de Git.

- `./tools/test.ps1`: compila toda la solución y ejecuta ambas suites, con TRX separados.
- `./tools/publish-portable.ps1`: publicación de preparación; `-IncludeManual` exige el manual final.
- `./tools/add-migration.ps1 -Name NombreDelCambio`: restaura la herramienta EF fijada y genera una migración revisable. No la aplica a bases del usuario.
- `packages.lock.json` por proyecto conserva versiones resueltas; `.config/dotnet-tools.json` fija EF 10.0.12. Primera restauración/publicación puede requerir internet.

## Antes de concretar la fase 6

1. Construir formularios de estudiantes/asignaturas y contexto de cursos, matrícula, sesiones y evaluaciones.
2. Integrar registro/corrección de notas y asistencia e informes, reutilizando los servicios existentes.
3. Comprobar mensajes, operación rechazada sin datos parciales y los recorridos completos en la interfaz.
4. Verificar la publicación en otro Windows x64 compatible sin .NET instalado, permisos y actualización de una base representativa.
5. Preparar manual de máximo dos páginas, capturas, paquete final y trazabilidad completa contra 2.19 y los demás criterios.

La migración inicial demuestra creación del esquema y apertura repetida; todavía no demuestra compatibilidad entre dos versiones distintas del esquema. No marcar la fase 6 como terminada antes de completar estas verificaciones.
