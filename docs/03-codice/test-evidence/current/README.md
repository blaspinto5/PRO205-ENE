# Evidencia actual — cierre de fase 6

55 casos aprobados: [unitarias](unit-tests.trx), [integración SQLite](integration-tests.trx) y [Desktop](desktop-tests.trx). [Salida completa](verification-output.txt). [Verificación portable](portable-startup.json).

Las imágenes siguientes se renderizaron desde controles WPF reales, en una copia aislada con datos ficticios. No son prototipos HTML ni capturas de una terminal simulada. El escritorio y el marco de Windows no forman parte del render. [Errores de binding](ui/binding-errors.txt): archivo vacío, cero errores detectados.

| Evidencia | Pantalla / estado |
|---|---|
| [01](ui/01-Home.png) | Inicio y conteos. |
| [02](ui/02-Students.png) | Estudiantes: selección/edición. |
| [03](ui/03-Subjects.png) | Asignaturas. |
| [04](ui/04-Courses.png) | Cursos y matrículas. |
| [05](ui/05-Grades.png) | Notas de evaluación. |
| [06](ui/06-Attendance.png) | Presente/Ausente/Sin registrar. |
| [07](ui/07-Reports.png) | Informe por curso. |
| [08](ui/08-GradeValidation.png) | Nota fuera de rango rechazada; registro previo conservado. |
| [09](ui/09-MinimumWindow.png) | Ventana mínima, aviso visible y contenido desplazable. |
| [10](ui/10-CourseAssessments.png) | Pestaña evaluaciones. |
| [11](ui/11-CourseSessions.png) | Pestaña sesiones. |
| [12](ui/12-StudentReport.png) | Informe individual y filtro opcional. |

Las pruebas Desktop comprueban comandos/ViewModels y persistencia. El render comprueba carga de recursos, enlaces y presentación; no equivale a una certificación completa de interacción manual o accesibilidad.
