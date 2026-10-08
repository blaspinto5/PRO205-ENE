# Ajustes del UML al código de fase 5

Actualización de base de fase 6: setters y constructores privados permiten recuperar las entidades mediante EF sin dar acceso público a sus estados. La herencia se persiste mediante TPC y las notas como décimas enteras. No cambia la semántica de las nueve clases; detalles en [base de fase 6](../phase-6-foundation.md).

El diagrama implementado conserva las nueve clases del modelo conceptual. Las operaciones mostradas son una selección de responsabilidades; el código contiene además validaciones y operaciones de registro.

| Ajuste | Propósito |
|---|---|
| Fechas académicas `DateOnly`; instantes `DateTimeOffset` en UTC; sesiones `DateTime` local | Separar fechas sin hora, auditoría y horario académico. |
| `ChangeValue`, `ChangeStatus` y `MarkUpdated` reciben el instante | Correcciones reproducibles y rechazo de modificaciones con fecha anterior. |
| Conteos reciben corte `DateTime` y devuelven `AttendanceCounts` | Distinguir presente, ausente y sin registrar; excluir sesiones futuras o previas a matrícula. |
| Se retira `Subject.GetCourses` | Consultar cursos mediante repositorio evita mantener dos colecciones inversas que podrían divergir. |
| Enrollment registra notas y asistencia | Proteger vínculos, unicidad y colecciones dentro de la entidad responsable. |
| Tres repositorios: estudiantes, asignaturas y cursos | Curso concentra matrículas, evaluaciones y sesiones; Application coordina su guardado. |
| AcademicTime recibe reloj y zona horaria | Validar horarios locales; rechazar horas inexistentes o ambiguas sin inventar una regla académica. |

La falta de nota es un estado derivado, no una calificación ficticia. La falta de asistencia no amplía el enum Present/Absent: se calcula a partir de sesiones sin registro. Los resúmenes de dominio no dependen de WPF; la presentación podrá adaptar los textos.

Los informes consideran evaluaciones desde la matrícula. El corte limita los conteos de asistencia y se acota al momento actual; no reconstruye versiones históricas de notas o correcciones.
