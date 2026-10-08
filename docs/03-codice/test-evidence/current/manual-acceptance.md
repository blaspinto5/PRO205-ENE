# Interacción directa en Windows — 8 de octubre de 2026

Se utilizó el ejecutable entregado en una copia aislada del paquete, con los datos ficticios creados previamente por el generador de vistas. Control de ventana mediante Computer Use; no se tocaron bases de trabajo ni el paquete original. Identidad del ejecutable y ensamblado en `release-ui.json`.

| Comprobación mediante controles reales | Resultado | Evidencia |
|---|---|---|
| Intentar guardar estudiante sin código | Rechazado: «Completa el código del estudiante.» | `manual/01-required-field.txt` |
| Crear QA-008 / Prueba / Aceptación | Guardado; ID `4ec61369-617b-4559-b762-f49df753bf7e` | `manual/02-student-created.txt` |
| Cambiar nombre a Prueba editada | Mismo código e ID; código bloqueado al editar | `manual/03-student-edited.txt` |
| Corregir nota de Ana a 8,0 | Rechazado con «La nota debe estar entre 1,0 y 7,0.» | `manual/04-grade-invalid.txt` |
| Corregir nota a 5,5 | Fila actualizada en evaluación práctica | `manual/05-grade-corrected.txt` |
| Corregir asistencia de Ana, sesión 6/10, de Presente a Ausente | Fila actualizada | `manual/06-attendance-corrected.txt` |
| Generar informe por curso | Ana: promedio 5,5; 0 presentes, 1 ausente, 1 sin registrar | `manual/07-course-report.txt` |
| Generar informe por estudiante | Mismos resultados; evaluación futura pendiente | `manual/08-student-report.txt` |
| Cerrar y reabrir | Inicio cuenta siete estudiantes; ficha editada conservada | `manual/10-reopened-student.txt` |
| Consultar base persistida después del cierre | Nota 55 décimas, asistencia Ausente, ficha con mismo ID; `integrity_check = ok` | `manual/09-persisted-db.json` |

Los archivos TXT contienen observaciones del árbol de accesibilidad de la ventana; no son capturas de terminal ni pruebas de ViewModel. La consulta de SQLite es una comprobación adicional de persistencia y se distingue de la interacción.

**Límites:** este recorrido parte de curso, matrícula, evaluación y sesión ficticios existentes. No certifica la creación manual completa de esos elementos desde una base vacía, navegación exclusivamente por teclado, accesibilidad completa, restauración manual ni funcionamiento en otro Windows sin .NET. El flujo de creación completo se comprobó automáticamente por `DesktopWorkflowTests.AcademicForms_WhenEntireFlowIsPerformed_PersistCorrectionsAndBothReportScopes`; la aceptación manual integral y externa está preparada en `../../portable/clean-windows-acceptance.md` y continúa pendiente. No declarar el punto 2 completamente cerrado.
