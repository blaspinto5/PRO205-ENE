# Preparación de fase 5 — Domain, Application y pruebas

Fecha: 6 de octubre de 2026. Estado actual: completada; consultar phase-5-review.md. Se conserva el diagnóstico inicial como antecedente. La preparación sigue a la defensa de fase 4.

## Propósito y fuentes

Construir el motor de Códice y demostrar reglas reales con pruebas unitarias antes de integrar WPF/SQLite. Referencias: [reglas adoptadas](requirements/business-rules.md), [trazabilidad de diseño](requirements/design-traceability.md), [fundamento UML](uml/model-justification.md) y [revisión de fase 4](phase-4-review.md).

La fase cubre C.E. 2.18: al menos tres pruebas funcionales que validen requisitos clave, asserts correctos, nombres descriptivos y ejecución sin errores. Meta interna: ocho a doce métodos de prueba útiles; casos parametrizados pueden producir más resultados. La cantidad no sustituye cobertura.

## Comprobación del entorno

Resultado de `dotnet --info` el 6 de octubre de 2026: Windows x64, SDK 9.0.318; runtimes .NET 8.0.31 y 9.0.20, incluido WindowsDesktop. No se encontró SDK .NET 10 ni `global.json`. Solo hay carpetas reservadas y `.gitkeep`; aún no hay solución o proyectos C#.

**Diagnóstico previo, resuelto en fase 5:** disponer de SDK .NET 10 conforme al stack adoptado. Esta preparación no instala herramientas ni cambia a .NET 9. No afirmar que el entorno ya puede compilar el objetivo net10.0. La compatibilidad de Visual Studio no se verificó; no confundir runtime instalado con SDK o compatibilidad del IDE.

## Orden de ejecución y propósito

| Paso | Trabajo | Resultado verificable |
|---|---|---|
| 1 | Resolver SDK .NET 10 y comprobarlo. | Entorno y versión registrados; fijar SDK compatible del proyecto. |
| 2 | Crear solución y proyectos Domain, Application y UnitTests; referencias permitidas. | Compilación inicial; Domain independiente, Application referencia Domain, tests referencian ambos. |
| 3 | Implementar Domain por responsabilidades y dependencias. | Entidades e invariantes acordadas; colecciones protegidas; no WPF/EF Core/SQLite. |
| 4 | Definir contratos y desarrollar Application. | Casos de uso que coordinan validación, contexto y repositorios abstractos. |
| 5 | Implementar xUnit y sustitutos en memoria dentro de tests. | Casos positivos/negativos y resultados específicos; sin persistencia real. |
| 6 | Ejecutar compilación/pruebas y corregir fallos. | Resultado sin errores, registro textual y captura de ejecución exigida. |
| 7 | Actualizar UML/trazabilidad ante ajustes fundamentados y revisar cierre. | Evidencia vinculada a requisitos; fase 5 cerrada antes de integrar UI. |

No crear aún implementaciones EF Core ni pantallas. Las carpetas Infrastructure/Desktop/IntegrationTests permanecen reservadas para fase 6. Si se realiza un ajuste entre modelo conceptual y código, registrar su razón y sincronizar documentación.

## Ubicación de responsabilidades

| Ubicación | Contenido previsto | Reglas importantes |
|---|---|---|
| `src/Codice.Domain/` | Student, Subject, Course, Enrollment, Assessment, ClassSession, AcademicRecord, Grade, AttendanceRecord; AttendanceStatus; resultado de conteos y excepciones necesarias. | BR01–BR18 locales; invariantes cerca de entidades; fechas/identidad inmutables cuando corresponda. |
| `src/Codice.Application/` | StudentService, SubjectService, CourseService, GradeService, AttendanceService, ReportService; contratos mínimos y DTOs. | Cargar contexto completo; verificar unicidad, cursos y operaciones de registro/edición/consulta. |
| `tests/Codice.UnitTests/Domain/` | Pruebas de invariantes y cálculos. | Independencia, AAA, `decimal`, casos negativos; comparar estado y resultado. |
| `tests/Codice.UnitTests/Application/` | Pruebas de coordinación mediante sustitutos de contratos. | Verificar operaciones y rechazo sin modificaciones parciales; sin DB real. |
| `docs/03-codice/test-evidence/` | Resultados de ejecución, captura y revisión. | Solo generar evidencia de ejecución cuando existan pruebas que realmente corran. |

## Precisiones para implementar sin ambigüedad

- Mapear Date a `DateOnly` cuando no hay hora; separar instantes de modificación y horario local de sesiones. Elegir una convención consistente y documentarla antes de codificar.
- Inyectar el tiempo de referencia para validar sesiones futuras y modificaciones; las pruebas usarán un reloj fijo. No depender de la fecha actual de la máquina para que un caso pase.
- Conservar precisión decimal. Promedio vacío devuelve ausencia de resultado, no cero; separar cálculo y presentación de referencia ≥4,0.
- La comparación normalizada se aplica a códigos y nombres de evaluación para unicidad; no modifica nombres propios innecesariamente.
- Para pendientes de notas, considerar evaluaciones del curso desde la matrícula; para asistencia, sesiones realizadas entre matrícula y corte. Cargar esas colecciones completas antes de informar totales.
- La unicidad requiere coordinación Application y posterior restricción de persistencia. Los sustitutos de tests no prueban transacciones ni índices reales de SQLite.
- No introducir un repositorio genérico o una clase Helper por comodidad. Definir contratos por operaciones necesarias y mantener comentarios mínimos.

## Selección inicial de pruebas — doce métodos previstos

| N.º | Nombre previsto | Qué verifica y trazabilidad |
|---|---|---|
| 1 | CreateStudent_WhenRequiredNameIsEmpty_ThrowsException | Nombre/apellidos vacíos, BR10/RF01; parametrizar campos. |
| 2 | RegisterStudent_WhenDataIsValid_PersistsStudent | Caso positivo y código normalizado, BR10/RF01. |
| 3 | RegisterStudent_WhenNormalizedCodeExists_RejectsDuplicate | Identidad única ignorando espacios extremos/case, BR10/RF01. |
| 4 | ChangeValue_WhenValueIsInvalid_ThrowsException | Rango/precisión y estado anterior conservado, BR01/RF06; parametrizar fuera de rango y dos decimales. |
| 5 | ChangeValue_WhenValueIsValid_UpdatesValueAndTimestamp | Aceptar 1,0/7,0 y modificación con reloj fijo, BR01/RF06. |
| 6 | CalculateAverage_WhenGradesExist_ReturnsRoundedMean | Media simple y empate 5,45→5,5, BR04/RF07; parametrizar también caso sin empate. |
| 7 | GetStudentReport_WhenGradesAreMissing_PreservesPendingState | Sin notas y notas parciales, BR03/BR16; no fabricar cero ni incluir pendientes. |
| 8 | RecordAttendance_WhenRecordAlreadyExists_RejectsDuplicate | Una por matrícula/sesión y corrección por vía separada, BR08/RF05. |
| 9 | UpdateAttendance_WhenRecordExists_ChangesStatusAndTimestamp | Corrección explícita sin duplicar registro, BR08/RF05. |
| 10 | RegisterAcademicRecord_WhenContextIsInvalid_RejectsRecord | Curso distinto, anterior a matrícula o asistencia futura, BR14/RF05/RF06; parametrizar escenarios pertinentes. |
| 11 | EnrollStudent_WhenEnrollmentIsInvalid_RejectsEnrollment | Duplicado y fechas fuera del curso, BR13/RF10. |
| 12 | GetCourseReport_WhenCutoffIsApplied_ReturnsCorrectStudentsAndCounts | Excluir otros cursos, sesiones futuras/pre-matrícula; distinguir Present/Absent/Sin registrar, BR09/BR17/RF08. |

Los nombres se ajustarán a métodos reales al implementar. Esta selección no pretende probar íntegramente cada función del catálogo con doce métodos. Unicidad de asignaturas/cursos/evaluaciones/sesiones, fechas de sus creaciones y edición de vínculos requieren revisión adicional; ampliar pruebas si la implementación o riesgos concretos lo justifican, registrando el ajuste de meta interna.

## Condición de cierre

- [x] Entorno y SDK compatibles comprobados.
- [x] Domain/Application implementados con separación y correspondencia con UML/reglas.
- [x] Tres o más unitarias útiles como mínimo; meta interna cumplida o ajuste explicado.
- [x] Pruebas positivas/negativas, nombres descriptivos, AAA y asserts específicos.
- [x] Compilación y `dotnet test` sin errores; ejecución repetible sin UI ni SQLite real.
- [x] Captura y resultado guardados; trazabilidad actualizada con archivos/nombres reales.
- [x] Revisión final contra 2.18 y documentación de diferencias del modelo.

**Cierre actualizado:** SDK 10.0.401 instalado localmente, Domain/Application implementados y 41 casos aprobados. Meta ampliada a 23 métodos por invariantes concretas. Evidencia y límites en [revisión de fase 5](phase-5-review.md).

