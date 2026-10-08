# Códice · PRO205 ENE

**Fases 1–6 completadas mediante revisión interna.** Aplicación académica local C#/.NET 10, WPF/MVVM y SQLite. Calificación final sujeta al docente.

## Entrega

Repositorio público: https://github.com/blaspinto5/PRO205-ENE. El historial comienza el 8 de octubre con el estado auditado; no reconstruye commits de fases pasadas. Los ZIP de `entrega/` se distribuyen por separado y no se guardan en Git.

- [Aplicación portable Windows x64](entrega/Codice-win-x64.zip): extraer la carpeta completa y abrir Codice.Desktop.exe. Incluye .NET, SQLite nativo y Manual.pdf; crea su base en datos/.
- [Fuentes, documentos y evidencias](entrega/PRO205-fuentes-y-evidencias.zip): código, tests, configuración, enunciados, UML, defensas, PDF y resultados. SDK y paquetes de desarrollo se restauran por separado.
- [Carpeta de aplicación](entrega/Codice-win-x64-20261006-191506-2a2548a4/): edición descomprimida actual.
- [Cierre de fase 6](docs/03-codice/phase-6-review.md) y [manual editable](docs/03-codice/technical-manual.md).

La aplicación permite estudiantes/asignaturas, cursos/matrículas/evaluaciones/sesiones, registro y corrección de notas/asistencia e informes por estudiante/curso. Sin usuarios ni vista administrativa, conforme al alcance acordado.

**55 casos aprobados** (41 unitarios, 8 SQLite, 6 Desktop); compilación sin errores/advertencias. Doce capturas de controles WPF reales revisadas; cero errores de binding. Arranque autocontenido, falta de escritura, segunda instancia y traslado comprobados en este equipo. Otro Windows limpio no verificado.

## Navegación del proyecto

| Carpeta | Propósito |
|---|---|
| src/ | Domain, Application, Infrastructure y Desktop. |
| tests/ | UnitTests, IntegrationTests y DesktopTests. |
| [docs](docs/README.md) | Fuentes académicas, decisiones, defensas y evidencia. |
| tools/ | Compilar, probar, migrar, capturar y publicar. |
| output/pdf/ | Entregables documentales de las fases. |
| entrega/ | Aplicación actual y ZIPs de entrega. |

La raíz conserva solución y configuración. Plan maestro/rúbrica en docs/00-planning/sources. Temporales y herramientas locales bajo .tools, fuera de Git/entrega. Código en inglés; interfaz y documentación en español.

## Desarrollo y verificación

Desde PowerShell en la raíz:

- ./tools/test.ps1: compilar los siete proyectos y ejecutar las tres suites; -BuildOnly solo compila.
- ./tools/capture-ui.ps1: renderizar pantallas reales con datos ficticios en una copia desechable; requiere Release compilado.
- ./tools/publish-portable.ps1 -IncludeManual: generar otra carpeta autocontenida sin bases de trabajo.
- ./tools/verify-portable.ps1: comprobar el paquete en copia aislada y registrar resultados locales.
- ./tools/verify-release-ui.ps1: renderizar el ejecutable entregado, exigir doce imágenes nuevas y cero errores de binding; no certifica interacción manual.
- ./tools/package-delivery.ps1: crear ZIP portable y ZIP separado de fuentes/evidencias.
- ./tools/add-migration.ps1 -Name NombreDelCambio: generar una migración revisable; no aplica cambios a datos de usuario.

SDK 10.0.401 fijado en global.json. Selector: .tools/dotnet, SDK local del usuario, luego sistema. SDK actualmente fuera del proyecto. Paquetes fijados mediante packages.lock.json; EF CLI 10.0.12 en .config/dotnet-tools.json. Primera restauración/publicación puede requerir internet. La edición portable se utiliza sin instalar SDK.

## Estilos y datos

App.xaml carga Styles/Tokens.xaml, Styles/Controls.xaml y Views/Templates.xaml. Cambiar colores, tipografía y controles allí; mantener reglas académicas en Domain/Application.

Cerrar antes de copiar o respaldar toda la carpeta datos. No publicar encima de datos de trabajo. No usar la misma base desde dos instancias ni red compartida. El prototipo utiliza datos ficticios; no certifica protección institucional de datos reales.

[Evidencia actual](docs/03-codice/test-evidence/current/README.md) · [Trazabilidad](docs/03-codice/requirements/implementation-traceability.md) · [Rúbrica](docs/00-planning/rubric-matrix.md) · [Defensa de fase 5](docs/03-codice/phase-5-oral-defense.md)
