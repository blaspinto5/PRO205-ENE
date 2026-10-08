# Códice — trazabilidad preliminar

Fecha: 6 de octubre de 2026. Estado: propuesta de análisis. Ninguna clase, operación, prueba o pantalla de esta tabla está implementada; los candidatos deben revisarse al diseñar el UML. Escenarios de prueba son planes, no evidencia de ejecución.

| Requisito | Clases/módulos candidatos | Caso de uso previsto | Verificación prevista | Pantalla prevista |
|---|---|---|---|---|
| RF01 | Student; Application | RegisterStudent | Datos válidos crean ficha; datos obligatorios ausentes se rechazan según D03. | Students |
| RF02 | Student; Application | UpdateStudent | Actualizar datos conserva vínculos académicos. | Students |
| RF03 | Subject; Application | RegisterSubject | Asignatura válida se registra; unicidad según D03. | Subjects |
| RF04 | Subject; Application | UpdateSubject | Actualizar datos conserva registros relacionados. | Subjects |
| RF05 | AttendanceRecord; Enrollment; Application | RecordAttendance / GetAttendance | Registro en fecha/contexto correctos; duplicidad y estados según D02. | Attendance |
| RF06 | Grade; Enrollment; Subject; Application | RegisterGrade / UpdateGrade | Registro/modificación correctos y rechazo de valores inválidos según D01. | Grades |
| RF07 | Student; ReportService; DTOs | GetStudentReport | Selección correcta y correspondencia con registros; integración para consulta real. | Reports |
| RF08 | Course; Enrollment; ReportService; DTOs | GetCourseReport | Miembros del curso correcto y exclusión de otros; relación según D04. | Reports |
| RNF01 | Infrastructure; contratos | Persistencia local | Prueba de integración o recorrido de guardar/cerrar/reabrir; no prueba unitaria con base real. | Transversal |
| RNF02 | Domain; Application; Infrastructure | Validación e integridad | Unitarias para reglas; integración para relaciones/transacciones; dependiente de D01–D04. | Formularios |
| RNF03 | Desktop | Presentación de formularios y resultados | Revisión manual de recorrido; no se prueba XAML mediante unitarias. | Todas |
| RNF04 | Todas las capas | Dependencias permitidas | Revisión de referencias y ejecución de unitarias sin UI ni base real. | No aplica: condición arquitectónica. |

## Uso posterior

Al producir el UML, reemplazar candidatos por clases y operaciones definidas y registrar su ubicación. En fase 5 añadir nombres y archivos reales de pruebas; en fase 6 añadir pantallas y evidencias verificadas. Justificar cualquier columna no aplicable. Mantener esta matriz separada de la matriz de criterios de rúbrica.

## Actualización con reglas adoptadas

D01–D06 se resolvieron en business-rules.md. Las alusiones anteriores a decisiones pendientes se sustituyen por BR01–BR20. RF07/RF08 calculan promedio simple y conteos de asistencia, distinguiendo pendientes y sin registrar.

| Requisito de soporte | Candidatos | Caso de uso previsto | Verificación prevista | Pantalla prevista |
|---|---|---|---|---|
| RF09 | Course, Subject | CreateCourse | Unicidad y fechas según BR12. | Courses |
| RF10 | Enrollment, Student, Course | EnrollStudent | Duplicidad y fechas según BR13. | Courses |
| RF11 | ClassSession, Course | CreateSession | Unicidad y fechas según BR06/BR14. | Attendance |
| RF12 | Assessment, Course | CreateAssessment | Nombre/curso únicos y fechas según BR02/BR14. | Grades |

El promedio tendrá pruebas de límites, redondeo y ausencia de notas; la asistencia tendrá pruebas de unicidad y corrección. Son escenarios previstos, no pruebas implementadas.

Referencia vigente de diseño: [design-traceability.md](design-traceability.md). Esta matriz conserva los candidatos históricos.
