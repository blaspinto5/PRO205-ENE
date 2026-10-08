# Códice — trazabilidad de diseño consolidada

Estado: diseño definido; código, pruebas y pantallas previstos para fases 5/6. Los nombres de pruebas son propuestas y no resultados de ejecución. Complementa el catálogo v0.2 y sustituye los candidatos de la matriz preliminar como referencia vigente.

| ID | Clases/módulo | Caso de uso u operación | Verificación prevista | Pantalla |
|---|---|---|---|---|
| RF01 | Student / StudentService | RegisterStudent | RegisterStudent_WhenDataIsValid_CreatesStudent; campos/código BR10. | Students |
| RF02 | Student / StudentService | UpdateStudent / UpdateName | UpdateStudent_WhenNameChanges_PreservesAcademicLinks. | Students |
| RF03 | Subject / SubjectService | RegisterSubject | RegisterSubject_WhenCodeExists_RejectsDuplicate. | Subjects |
| RF04 | Subject / SubjectService | UpdateSubject / UpdateName | UpdateSubject_WhenNameChanges_PreservesCourses. | Subjects |
| RF05 | AttendanceRecord, AcademicRecord, Enrollment, ClassSession / AttendanceService | RecordAttendance / UpdateAttendance / ChangeStatus | RecordAttendance_WhenRecordExists_RejectsDuplicate; UpdateAttendance_WhenValid_ChangesStatusAndTimestamp. | Attendance |
| RF06 | Grade, AcademicRecord, Assessment, Enrollment / GradeService | RegisterGrade / UpdateGrade / ChangeValue | ChangeValue_WhenOutOfRange_ThrowsException; RegisterGrade_WhenAssessmentBelongsToAnotherCourse_RejectsRecord. | Grades |
| RF07 | Student, Enrollment, Course, Grade, AttendanceRecord / ReportService | GetStudentReport / CalculateAverage / GetAttendanceCounts | CalculateAverage_WhenNoGrades_ReturnsNull; CalculateAverage_WhenMidpoint_RoundsUp; informe con pendientes y fecha de corte. | Reports |
| RF08 | Course, Enrollment, Subject / ReportService | GetCourseReport | GetCourseReport_WhenCourseSelected_ExcludesOtherCourses. | Reports |
| RF09 | Course, Subject / CourseService | CreateCourse | CreateCourse_WhenSectionPeriodSubjectExists_RejectsDuplicate; fechas BR12. | Courses |
| RF10 | Enrollment, Student, Course / CourseService | EnrollStudent / Enroll | EnrollStudent_WhenAlreadyEnrolled_RejectsDuplicate; fecha BR13. | Courses |
| RF11 | ClassSession, Course / CourseService | CreateSession / AddSession | CreateSession_WhenStartExists_RejectsDuplicate; fechas BR06/BR14. | Attendance |
| RF12 | Assessment, Course / CourseService | CreateAssessment / AddAssessment | CreateAssessment_WhenNameExists_RejectsDuplicate; fechas BR02/BR14. | Grades |
| RNF01 | Infrastructure | Persistencia y recuperación; respaldo manual documentado | Integración: guardar/cerrar/reabrir; restaurar copia consistente en fase 6. | Transversal |
| RNF02 | Domain/Application/Infrastructure | Validaciones BR01–BR18 e integridad | Unitarias de reglas y fallos; integración de claves/relaciones/transacciones. | Formularios |
| RNF03 | Desktop | Mensajes, estados vacíos y campos obligatorios | Recorrido manual de crear/editar/consultar; sin tests unitarios de XAML. | Todas |
| RNF04 | Capas de la solución | Referencias según arquitectura | Revisar dependencias; probar Domain/Application sin Desktop ni SQLite real. | No aplica: arquitectura. |

Reglas: BR01–BR05 → RF06/RF07/RF08/RF12; BR06–BR09 → RF05/RF07/RF08/RF11; BR10–BR11 → RF01–RF04; BR12–BR15 → RF09–RF12 y vínculos de RF05/RF06; BR16–BR18 → RF07/RF08; BR19–BR20 → RNF01–RNF04 y manual de fase 6.

No es necesario crear todas las pruebas de esta tabla para llegar a una cantidad. En fase 5 se seleccionarán pruebas útiles del núcleo y se justificará la cobertura, manteniendo mínimo tres funcionales y meta de ocho a doce; pruebas de persistencia real son integración. Actualizar ubicaciones y evidencia cuando existan implementaciones.
