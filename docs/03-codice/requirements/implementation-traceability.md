# Trazabilidad final — Códice

Fecha: 6 de octubre de 2026. Funciones implementadas en servicios C#, persistencia SQLite y pantallas WPF. Revisión interna, no calificación docente.

| Requisito | Pantalla y acción | Servicio | Evidencia principal |
|---|---|---|---|
| RF01 Registrar estudiante | Estudiantes: Nuevo / Guardar estudiante | StudentService.RegisterStudentAsync | StudentAndSubjectForms_WhenCreatedAndEdited_PreserveCodesAndIdentity; validación de campos; captura 02. |
| RF02 Editar estudiante | Estudiantes: seleccionar fila / Guardar | StudentService.UpdateStudentAsync | Identidad/código conservados en prueba de formulario; preservación de vínculos y nombres en integración. |
| RF03 Registrar asignatura | Asignaturas: Nuevo / Guardar asignatura | SubjectService.RegisterSubjectAsync | Prueba de creación y edición de formularios; captura 03. |
| RF04 Editar asignatura | Asignaturas: seleccionar fila / Guardar | SubjectService.UpdateSubjectAsync | Prueba de formulario conserva ID/código; prueba Application conserva vínculos. |
| RF05 Gestionar asistencia | Asistencia: curso, sesión, estudiante; Registrar / Corregir | AttendanceService | Recorrido completo verifica corrección e ID; sesión futura rechazada; captura 06. |
| RF06 Gestionar notas | Calificaciones: curso, evaluación, estudiante; Registrar / Corregir | GradeService | Recorrido completo acepta coma/punto, corrige y rechaza 7,1 conservando 5,5; capturas 05/08. |
| RF07 Informe estudiante | Informes: Por estudiante; filtro opcional de curso / Todos los cursos | ReportService.GetStudentReportAsync | Recorrido completo y filtro; pendientes/referencia/conteos; captura 12. |
| RF08 Informe curso | Informes: Por curso; corte de asistencia y detalle de fila | ReportService.GetCourseReportAsync | Recorrido completo; corte y separación de cursos probados en Application; captura 07. |
| RF09 Crear curso | Cursos: Nuevo curso / Crear curso | CourseService.CreateCourseAsync | Recorrido completo y unicidad probada; captura 04. |
| RF10 Matricular | Cursos: Matrículas / Matricular estudiante | CourseService.EnrollStudentAsync | Recorrido completo, duplicados/fechas de dominio; captura 04. |
| RF11 Crear sesión | Cursos: Sesiones / Crear sesión | CourseService.CreateSessionAsync | Recorrido completo, HH:mm inválido y hora local inválida; captura 11. |
| RF12 Crear evaluación | Cursos: Evaluaciones / Crear evaluación | CourseService.CreateAssessmentAsync | Recorrido completo, fechas/nombres únicos de dominio; captura 10. |

| No funcional | Implementación y comprobación |
|---|---|
| RNF01 Persistencia | Migración inicial, datos junto al ejecutable, recuperación en contextos nuevos, correcciones recuperadas y copia/restauración de base cerrada. |
| RNF02 Integridad | Invariantes, claves foráneas, índices únicos, checks y transacción; rechazo real con reversión; operaciones de pantalla serializadas. |
| RNF03 Claridad | Español, campos obligatorios, código estable, selección guiada y mensajes superiores; doce capturas WPF revisadas sin errores de binding. |
| RNF04 Mantenibilidad | Cuatro capas; reglas fuera de vistas/ViewModels; estilos compartidos y DTOs de consulta; motor probado sin WPF ni SQLite. |

Ubicaciones de pruebas: `tests/Codice.UnitTests`, `tests/Codice.IntegrationTests` y `tests/Codice.DesktopTests`. Las de Desktop ejecutan comandos y ViewModels sobre SQLite real; no son clics manuales ni automatización de teclado. Las capturas renderizan los controles WPF reales, con datos ficticios en una copia aislada.

Evidencia actual: [TRX unitarias](../test-evidence/current/unit-tests.trx), [TRX integración](../test-evidence/current/integration-tests.trx), [TRX Desktop](../test-evidence/current/desktop-tests.trx), [salida](../test-evidence/current/verification-output.txt) y `test-evidence/current/ui/`. Límites de publicación y cierre en [revisión de fase 6](../phase-6-review.md).
