# Códice — fundamento del modelo

El UML conceptual contiene nueve clases: Student, Subject, Course, Enrollment, Assessment, ClassSession, AcademicRecord, Grade y AttendanceRecord. AttendanceStatus es un enum y Counts un resultado estructurado; no se cuentan como clases para alcanzar el mínimo.

Student representa identidad estable; Subject el catálogo de asignaturas; Course una sección de una asignatura en un período. Enrollment separa persona de participación académica y concentra consultas de sus registros. Assessment identifica la evaluación común del curso, mientras Grade es la calificación individual. ClassSession identifica la sesión común y AttendanceRecord el estado individual.

## Herencia

AcademicRecord es abstracta: contiene identidad, matrícula asociada, fecha de registro y última modificación. Grade y AttendanceRecord son registros académicos individuales que heredan ese contexto; especializan valor numérico o estado de presencia. GetSummary() expresa un resumen conceptual del registro y se redefine; no renderiza WPF. MarkUpdated() es protegida para actualizar metadatos al cambiar valor/estado. Las validaciones específicas permanecen en cada especialización.

La base no concentra todas las reglas ni impone que una nota y una asistencia sean equivalentes. La relación AcademicRecord–Enrollment se hereda: cada registro corresponde a una matrícula, y cada matrícula puede tener muchos registros. Una nota añade una evaluación; una asistencia añade una sesión. Application coordina creación, unicidad y pertenencia al mismo curso antes de persistir.

## Relaciones

- **Agregación Subject–Course:** Subject agrupa sus ofertas. Course conserva su identidad e historial aunque la asignatura deje de ofrecerse en el catálogo activo. No se permite eliminar físicamente una asignatura referenciada ni perder la referencia; se conserva el conjunto histórico. Rombo vacío en Subject; cada Course referencia un Subject y cada Subject agrupa 0..* cursos. La agregación describe agrupación, no independencia de todas las restricciones de referencia.
- **Asociaciones Student–Enrollment y Course–Enrollment:** matrícula con un estudiante y un curso; cada participante tiene 0..* matrículas. La persona no es una parte poseída por la matrícula.
- **Composición Course–Assessment y Course–ClassSession:** evaluaciones y sesiones pertenecen a un único curso y no se trasladan entre cursos. Se conservan junto a su contexto; no se prescribe cascada SQL ni borrado de historia. Curso tiene 0..* de cada parte.
- **Asociaciones Grade–Assessment y AttendanceRecord–ClassSession:** cada registro individual referencia una evaluación o sesión; cada evento común tiene 0..* registros. Unicidad adicional por matrícula/evento en BR02 y BR08.
- **Generalización Grade/AttendanceRecord hacia AcademicRecord:** triángulo vacío apunta a la clase general y no lleva multiplicidad.

## Capas y responsabilidades de creación

Domain contiene estas entidades y reglas locales. Application contiene StudentService, SubjectService, CourseService, GradeService, AttendanceService y ReportService, con contratos de repositorios. Los servicios coordinan registro/consulta y crean entidades válidas: el diagrama no presenta sus constructores como métodos de negocio.

Infraestructura implementa repositorios y restricciones de persistencia; Desktop coordina pantallas. El UML es de dominio y no duplica DbContext, ventanas o DTOs de todas las consultas como clases académicas. Los casos de uso aparecen en la matriz de trazabilidad.

Los atributos relacionales se representan mediante líneas, sin duplicar StudentId, CourseId y otras claves en cada compartimento. Los constructores de implementación deberán recibir esas relaciones. Date, DateTime, Guid y Decimal son tipos conceptuales. Las restricciones de fechas, rango, redondeo y claves están en business-rules.md.

## Reglas que requieren colaboración

Calcular promedio usa las Grade de la matrícula y consulta evaluaciones del curso para informar pendientes. Contar asistencia usa sesiones realizadas del curso desde la matrícula hasta la fecha de corte. Application carga ese contexto y coordina el informe: no se deduce que una colección parcial permita calcular un resultado completo.

La fecha de corte de asistencia se compara con inicio real de sesión: no registrar sesiones futuras. Las fechas y textos se validan según BR01–BR20. Un código estable no se modifica mediante las operaciones UpdateName.
