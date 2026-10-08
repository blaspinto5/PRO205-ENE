# Defensa oral — fase 5 de Códice

Fecha: 6 de octubre de 2026. Material de exposición basado en código y evidencia reales. Referencias: [cierre](phase-5-review.md), [trazabilidad](requirements/implementation-traceability.md), [ajustes UML](uml/implementation-adjustments.md) y [resultados](test-evidence/test-report.html).

## Guion oral — cuatro a cinco minutos

### Propósito y resultado

En la fase 4 definí los requisitos y el diseño de Códice, un sistema para gestionar estudiantes, asignaturas, calificaciones y asistencia. En la fase 5 convertí ese diseño en un motor de software y comprobé sus reglas antes de construir la interfaz.

El resultado es una solución en C# y .NET 10 con tres proyectos: Domain, Application y UnitTests. La compilación terminó sin errores ni advertencias. Se ejecutaron 41 casos de prueba y todos pasaron. Estos resultados están conservados en un archivo de ejecución, un informe y una captura.

### Organización y programación orientada a objetos

Domain contiene las nueve clases principales del modelo. Student identifica al estudiante; Subject representa la asignatura; Course una sección durante un período; Enrollment vincula al estudiante con ese curso. Assessment y ClassSession representan eventos comunes, mientras Grade y AttendanceRecord son los registros individuales.

El encapsulamiento permite proteger las reglas: las colecciones se exponen para consulta, pero las notas y asistencias se incorporan mediante operaciones que verifican su validez. Por ejemplo, Enrollment rechaza una nota cuya evaluación pertenezca a otro curso.

AcademicRecord reúne identidad, matrícula y fechas de registro y modificación. Grade y AttendanceRecord heredan ese contexto y especializan GetSummary. Una prueba utiliza ambos como AcademicRecord y verifica que cada uno conserva su comportamiento particular; así demuestro polimorfismo ejecutado, además de representarlo en el UML.

Application coordina los casos de uso: registrar, modificar y consultar. Utiliza contratos de repositorio para estudiantes, asignaturas y cursos. Esto permite probar el motor con datos en memoria y conectar posteriormente SQLite sin llevar reglas académicas a las ventanas.

### Reglas demostradas con ejemplos

Las notas admiten valores de uno a siete con una décima. Una modificación inválida, como 7,1 o 5,45, se rechaza y conserva la nota y su fecha anteriores. Para calcular el promedio utilizo decimal, media simple y redondeo final a una décima. Por ejemplo, 5,4 y 5,5 producen 5,45, que se informa como 5,5.

Una evaluación pendiente no se convierte en cero ni en uno. Si no existen notas, el promedio no tiene valor; si faltan evaluaciones, el informe identifica el resultado como provisional. El cuatro es una referencia informativa del prototipo, no una declaración de aprobación institucional.

En asistencia verifico que sesión y matrícula correspondan al mismo curso, que la sesión no sea futura y que no sea anterior a la matrícula. Un segundo registro para la misma sesión se rechaza; corregir el estado utiliza una operación explícita y actualiza su fecha. Los informes distinguen presentes, ausentes y sesiones sin registrar.

### Pruebas y relación con la rúbrica

El criterio 2.18 exige al menos tres pruebas funcionales que validen requisitos clave, nombres descriptivos, asserts correctos y ejecución sin errores. Mi implementación tiene 23 métodos de prueba que generan 41 casos, porque algunos prueban varios valores o escenarios.

Cada prueba prepara un contexto, ejecuta una operación y comprueba el resultado esperado. Las pruebas positivas verifican registros, modificaciones e informes correctos. Las negativas verifican rechazos y, cuando corresponde, que el objeto conserve su estado o que no se solicite guardar.

Uso un reloj fijo para que la validación de sesiones futuras y fechas de modificación no dependa del día en que se ejecuten las pruebas. Los repositorios en memoria aíslan las reglas de la base de datos. La cantidad de pruebas se amplió por validaciones concretas, como duplicados, vínculos entre cursos y fechas; el objetivo es demostrar comportamiento, no acumular pruebas.

### Evidencia, coherencia y alcance

Actualicé la trazabilidad entre requisitos, métodos y pruebas. También sincronizé el UML: precisé los tipos de fechas, las operaciones de corrección y los conteos de asistencia, y retiré una consulta inversa de Subject para evitar duplicar colecciones.

La separación de responsabilidades facilita mantener y ampliar el sistema. Validar antes de guardar reduce errores de registro y reprocesos, y distingue datos pendientes de resultados académicos reales. Ese es un aporte concreto a una gestión responsable de la información.

Con esta fase queda comprobado el motor en los escenarios definidos. La interfaz WPF, la persistencia SQLite y sus pruebas de integración corresponden a fase 6. Las pruebas actuales no acreditan todavía transacciones, concurrencia ni conservación de datos entre ejecuciones.

## Versión breve — aproximadamente un minuto

En la fase 5 transformé el UML de Códice en código C# con Domain y Application. Domain protege las reglas de estudiantes, cursos, notas y asistencia; Application coordina operaciones mediante contratos de repositorio.

Demostré encapsulamiento, herencia y polimorfismo. Por ejemplo, las notas inválidas se rechazan sin alterar el registro anterior, y Grade y AttendanceRecord conservan sus comportamientos al utilizarse como AcademicRecord.

La solución compila sin errores ni advertencias. Tiene 23 métodos de prueba y 41 casos ejecutados, todos aprobados. Cubren registros válidos, duplicados, fechas, promedio, datos pendientes y correcciones, con reloj fijo y repositorios en memoria. Esto responde al criterio 2.18 con evidencia verificable y trazabilidad.

El motor está listo para integrar la interfaz y SQLite en fase 6; todavía no se ha comprobado persistencia real ni una aplicación de escritorio terminada.

## Preguntas probables y respuestas

| Pregunta | Respuesta defendible |
|---|---|
| ¿Qué cambió respecto de fase 4? | El diseño ahora tiene implementación compilable y comportamiento probado. También registré y sincronizé los ajustes del UML. |
| ¿Qué diferencia Domain de Application? | Domain decide qué estados y operaciones son válidos. Application busca el contexto, coordina la operación y solicita su guardado o prepara el informe. |
| ¿Por qué probar antes de crear pantallas? | Permite comprobar las reglas de forma independiente y detectar errores sin depender de interacción visual o de una base real. |
| ¿Dónde se demuestra encapsulamiento? | En colecciones de solo lectura y operaciones como RegisterGrade y RecordAttendance, que controlan cómo se incorporan registros. |
| ¿Dónde se demuestra polimorfismo? | Grade y AttendanceRecord redefinen GetSummary; la prueba GetSummary_WhenRecordsAreViewedPolymorphically_PreservesSpecializedBehavior verifica ambas mediante AcademicRecord. |
| ¿Por qué 23 métodos y 41 pruebas? | Fact ejecuta un caso; Theory puede ejecutar varias combinaciones de datos. El resultado cuenta casos, no solamente métodos. |
| ¿Por qué superar la meta de doce métodos? | La implementación requirió comprobar invariantes adicionales y riesgos concretos: duplicados, fechas, vínculos, estado tras rechazo y horarios locales. El ajuste está documentado. |
| ¿Qué es un assert? | Una comprobación del resultado esperado: valor de promedio, excepción concreta, cantidad de registros o conservación del estado. |
| ¿Qué significa AAA? | Preparar los datos, ejecutar la acción y comprobar el resultado. Mantiene claro qué comportamiento verifica cada prueba. |
| ¿Un test que espera una excepción es un fallo? | No. Si una entrada inválida debe rechazarse y la excepción esperada ocurre, la prueba pasa. Un fallo sería aceptar esa entrada o rechazarla de forma distinta de la esperada. |
| ¿Por qué decimal? | Permite trabajar con las notas y su precisión decimal de forma adecuada. El promedio se redondea una sola vez al producir el resultado. |
| ¿Por qué rechazar 5,45 como nota y obtenerlo en un promedio? | El ingreso acepta una décima, pero el cálculo puede generar más. 5,4 y 5,5 son entradas válidas y su media 5,45 se redondea a 5,5. |
| ¿Sin notas el promedio es cero? | No: devuelve ausencia de valor. Los pendientes no se convierten en notas inventadas. |
| ¿Sin registro de asistencia significa ausente? | No. El informe distingue ausencia registrada y sesión sin registrar. |
| ¿Cómo evitas pruebas dependientes de la fecha actual? | Inyecto un proveedor de tiempo y utilizo un reloj fijo en los tests. |
| ¿Las pruebas usan SQLite? | No. Utilizan repositorios en memoria; prueban reglas y coordinación. La integración con SQLite se verificará en fase 6. |
| ¿El sistema ya guarda datos al cerrar? | Todavía no. Los contratos de guardado están definidos, pero la persistencia real está pendiente. |
| ¿41 pruebas garantizan que no hay errores? | Demuestran que los escenarios comprobados cumplen los resultados esperados. No garantizan cobertura total ni prueban todavía interfaz, transacciones o concurrencia. |
| ¿Se obtuvo oficialmente Sobresaliente? | La revisión interna está contrastada con el descriptor de 2.18; la calificación final corresponde al docente. |
| ¿La escala se aplica por ley a todo Chile? | No. Es una regla adoptada para este prototipo con referencia institucional documentada; no se presenta como una norma universal. |

## Demostración sugerida — dos a tres minutos

1. Mostrar [informe](test-evidence/test-report.html): 41 ejecutadas y aprobadas, cero fallidas y omitidas. Identificar el TRX original como fuente.
2. Abrir `tests/Codice.UnitTests/Domain/AcademicRulesTests.cs`. Mostrar ChangeValue_WhenValueIsInvalid_PreservesValueAndTimestamp: valores inválidos, excepción y estado conservado.
3. Mostrar CalculateAverage_WhenGradesExist_ReturnsRoundedMean y explicar 5,4 + 5,5 → 5,5.
4. Abrir `tests/Codice.UnitTests/Application/AcademicServicesTests.cs`. Mostrar RecordAttendance_WhenRecordExists_RejectsDuplicateAndAllowsExplicitCorrection: rechazo del duplicado y corrección explícita.
5. Mostrar el caso de informe con corte y la [trazabilidad](requirements/implementation-traceability.md). Explicar qué requisito se verifica y qué queda para fase 6.

Si se solicita ejecución en vivo, usar `./tools/test.ps1` desde la raíz. El informe y la captura conservados describen la ejecución registrada; si se genera una nueva ejecución, actualizar el informe con `tools/build_test_report.py` usando el runtime Python disponible antes de presentar sus resultados como actuales.

## Evidencia para consultar antes de exponer

- [Compilación](test-evidence/build-output.txt).
- [Salida de ejecución](test-evidence/test-output.txt) y [TRX](test-evidence/results/unit-tests.trx).
- [Captura real del informe](test-evidence/test-results.png).
- [Cambios del UML](uml/implementation-adjustments.md) y [cierre de fase 5](phase-5-review.md).

Evitar atribuir a estas pruebas validaciones de WPF, SQLite o una aprobación docente que aún no se han realizado.
