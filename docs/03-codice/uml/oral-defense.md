# Defensa oral — fase 4, Instituto Códice

Fecha: 6 de octubre de 2026. Material de exposición y estudio. Basado en el [catálogo](../requirements/requirements-catalog.md), las [reglas](../requirements/business-rules.md), el [fundamento del UML](model-justification.md) y la [trazabilidad](../requirements/design-traceability.md). No sustituye productos académicos ni acredita implementación.

## Guion — aproximadamente cuatro a cinco minutos

### Problema y alcance

Códice necesita reemplazar registros en papel o Excel por una aplicación de escritorio Windows en C# y .NET. Los docentes deben registrar y editar estudiantes y asignaturas, gestionar asistencia por fecha, ingresar y modificar calificaciones y consultar informes por estudiante y curso.

En esta fase definí requisitos formales y un UML coherente con ellos. Todavía no hay aplicación ni pruebas ejecutadas. El objetivo fue decidir qué construir y cómo organizar el dominio antes de programarlo.

### Requisitos y decisiones

El catálogo tiene doce requisitos funcionales y cuatro no funcionales. Ocho funcionales descomponen los procesos expresos del caso. Los otros cuatro permiten crear cursos, matricular estudiantes y definir sesiones y evaluaciones: son soporte necesario para saber a qué contexto corresponden notas, asistencia e informes.

Los no funcionales concretan persistencia local, integridad, claridad de interacción y separación de responsabilidades. Cada requisito tiene nombre, tipo, descripción y prioridad, además de aceptación prevista. La prioridad orienta el desarrollo; no equivale al peso del criterio de evaluación.

El enunciado no fija todas las reglas académicas. Se adoptó escala de uno a siete con una décima, promedio simple, asistencia por sesión y códigos internos. Escala y precisión toman como referencia documentación institucional de AIEP; no se presentan como una ley universal ni se adopta su reglamento completo. Promedio y estructura de cursos son decisiones del prototipo autorizadas por el usuario.

### Clases y relaciones

El modelo tiene nueve clases. Student es la persona y Subject la asignatura del catálogo. Course representa una sección de esa asignatura en un período. Enrollment vincula estudiante y curso, conservando historia sin guardar un curso permanente en Student.

Assessment representa una evaluación común del curso y Grade una nota de un matriculado. ClassSession representa una sesión común y AttendanceRecord el estado individual de asistencia. Separar el evento común del registro individual permite que varios estudiantes tengan notas o estados distintos en la misma evaluación o sesión.

AcademicRecord es abstracta y reúne identidad, matrícula, fecha de registro y modificación. Grade y AttendanceRecord heredan ese contexto, redefinen GetSummary() y conservan sus reglas particulares. MarkUpdated() es protegida para actualizar los metadatos dentro de la jerarquía.

Subject agrega cursos; un curso histórico conserva su identidad aunque deje de ofrecerse activamente. Course compone evaluaciones y sesiones, que pertenecen exclusivamente a ese curso. Las matrículas, participantes y registros individuales se vinculan mediante asociaciones. Los rombos están en los extremos que representan el todo y los triángulos apuntan a AcademicRecord.

### Integridad y datos pendientes

Una nota pertenece a una matrícula y una evaluación del mismo curso. Solo puede existir una por esa combinación. Una asistencia corresponde a matrícula y sesión del mismo curso y tampoco se duplica. La corrección utiliza una actualización explícita, conservando fecha de modificación.

Una evaluación sin nota está pendiente y no aporta cero ni uno al promedio. Sin notas se muestra “Sin calificaciones”. Una sesión sin registro tampoco convierte automáticamente al estudiante en ausente. Los informes cuentan presentes, ausentes y sin registrar y excluyen sesiones futuras o anteriores a la matrícula.

El promedio se calcula con precisión decimal y se redondea al final. El valor cuatro es una referencia informativa, no aprobación final de asignatura: el prototipo no aplica exámenes, ponderación setenta/treinta ni reprobación por asistencia.

### Trazabilidad y siguiente paso

Cada requisito se vincula con clases, casos de uso, verificaciones y pantallas previstas. Por ejemplo, registrar asistencia corresponde a AttendanceRecord, Enrollment y ClassSession, coordinados por AttendanceService; se prevé probar duplicidad y corrección y presentarlo en la pantalla Attendance.

Domain contendrá reglas; Application coordinará operaciones; Infrastructure implementará SQLite y Desktop la interfaz WPF. Los repositorios de Application serán contratos, no una base real dentro del dominio. La fase siguiente demostrará este diseño mediante código y pruebas xUnit.

## Preguntas y respuestas

| Pregunta | Respuesta defendible |
|---|---|
| ¿Qué diferencia un funcional de un no funcional? | El funcional describe una capacidad, como registrar una nota. El no funcional describe una condición de calidad, como conservar datos entre sesiones o separar negocio de la interfaz. |
| ¿Por qué doce funcionales si la meta era ocho? | Ocho cubren las funciones expresas. Cuatro permiten crear el contexto mínimo de cursos, matrículas, sesiones y evaluaciones. La ampliación está justificada y no incorpora admisión ni gestión institucional completa. |
| ¿La prioridad Alta significa que vale más en la rúbrica? | No. Es prioridad propuesta de implementación. La ponderación académica la establece la rúbrica. |
| ¿Course significa carrera? | No. En el prototipo es una sección de una asignatura durante un período. Esa definición resuelve una ambigüedad del caso y está documentada como decisión. |
| ¿Para qué sirve Enrollment? | Separa la identidad del estudiante de su participación en un curso y período. Permite varias matrículas sin perder historia y da contexto a notas y asistencia. |
| ¿Por qué Grade no está directamente en Student? | Una nota necesita matrícula y evaluación para saber a qué curso, asignatura y evento pertenece. La relación directa con Student sería insuficiente para esa trazabilidad. |
| ¿Assessment y Grade son lo mismo? | No. Assessment es la evaluación común; Grade es el resultado individual. Una evaluación puede tener muchas notas, como máximo una por matrícula. |
| ¿ClassSession y AttendanceRecord son lo mismo? | No. La sesión es el evento del curso. El registro indica la presencia o ausencia de un estudiante matriculado en ese evento. |
| ¿La herencia fue añadida solo para la rúbrica? | AcademicRecord tiene responsabilidad compartida concreta: identidad, matrícula y fechas de registro/modificación, más un contrato de resumen. Las especializaciones mantienen validaciones distintas. No se usa una base vacía ni se crea un docente sin función. |
| ¿GetSummary() es lógica de interfaz? | Representa un resumen conceptual del registro; no conoce ventanas ni XAML. La presentación localizada y los DTOs de informe se manejan fuera de las entidades. Al implementar se debe mantener esa separación. |
| ¿Dónde está el polimorfismo? | Grade y AttendanceRecord redefinen GetSummary() mediante el contrato de AcademicRecord. El UML lo representa; todavía no se ha ejecutado. |
| ¿Por qué Subject–Course es agregación? | Expresa agrupación de ofertas de una asignatura sin destruir identidad o historia de cursos al retirar una oferta activa. La referencia se conserva; no se permite eliminar una asignatura usada. Es una decisión conceptual, no una política de borrado SQL. |
| ¿Por qué sesiones/evaluaciones son composición? | Pertenecen a un curso exclusivo y no se trasladan a otro. Se conservan con su contexto; si se destruyera conceptualmente el conjunto no quedarían como partes autónomas. Archivar historia no es destruirlo. |
| ¿Cómo se leen las multiplicidades? | Cada extremo indica cuántos objetos de esa clase se relacionan con uno del extremo opuesto. Un Student tiene 0..* Enrollment; cada Enrollment tiene un Student y un Course. |
| ¿Por qué no aparecen todas las claves foráneas? | El UML expresa relaciones con líneas. Las claves concretas y mapeos corresponden a implementación/persistencia; no se duplican como atributos para contar relaciones. |
| ¿Una nota pendiente vale uno? | No. No hay Grade asignada todavía; se muestra pendiente y se excluye del promedio. Una ausencia tampoco asigna una nota automáticamente. |
| ¿Por qué se rechaza 5,45 como nota pero un promedio puede dar 5,45? | El ingreso acepta una décima. Un cálculo sobre notas válidas puede producir más decimales: ese resultado se redondea una sola vez al presentarlo. Son procesos diferentes. |
| ¿Un promedio mostrado de 4,0 aprueba la asignatura? | No. Solo indica estar sobre la referencia numérica acordada. No certifica aprobación final ni sustituye condiciones académicas que el prototipo no modela. |
| ¿Las reglas son las mismas para todo Chile? | No se hace esa afirmación. Se usa una referencia institucional fechada para escala/precisión; el resto se adopta para Códice. |
| ¿Cómo evitas duplicar asistencia? | Unicidad por matrícula y sesión. Application coordina la búsqueda y Domain valida coherencia; persistencia deberá reforzar la restricción al integrar SQLite. El UML solo planifica esa garantía. |
| ¿SQLite garantiza seguridad y respaldo? | No por sí solo. Se delimita uso académico con datos ficticios y respaldo manual consistente cuando la aplicación esté cerrada. No se afirma seguridad de producción ni cumplimiento jurídico. |
| ¿Ya funcionan los informes? | No. Hay reglas, clases y casos de uso trazados. Su funcionamiento se demostrará con implementación y pruebas posteriores. |
| ¿Por qué no calculas porcentaje de asistencia? | Para mantener alcance acotado y evitar un denominador ambiguo. Se muestran cantidades de presentes, ausentes y sin registrar, respetando matrícula y fecha de corte. |

## Ejemplo para recorrer el UML

Ejemplo ficticio: Ana se matricula en la sección A de PRO205, período 2026-2. Se crea una evaluación y una sesión para ese curso. Ana recibe una Grade de 5,4 en la evaluación y un AttendanceRecord Present en la sesión. Otra evaluación todavía no tiene nota: el informe la muestra pendiente y el promedio 5,4 como provisional.

Señalar Student → Enrollment → Course → Subject; después Course → Assessment y Grade → AcademicRecord → Enrollment; finalmente Course → ClassSession y AttendanceRecord → AcademicRecord → Enrollment. Explicar que las restricciones impiden enlazar una evaluación de otro curso.

## Versión breve — aproximadamente un minuto

Códice requiere gestionar estudiantes, asignaturas, notas, asistencia e informes. En fase 4 definí un catálogo formal y reglas explícitas para completar las ambigüedades del caso. El UML tiene nueve clases y separa persona, matrícula, curso, eventos comunes y registros individuales.

Grade y AttendanceRecord heredan identidad, matrícula y metadatos de AcademicRecord. Las relaciones y multiplicidades están justificadas. El diseño diferencia pendientes de notas reales y registros ausentes de estados Ausente.

La trazabilidad conecta requisitos con clases, operaciones, pruebas y pantallas previstas. Esta fase define el diseño; la siguiente construirá Domain y Application y comprobará las reglas con xUnit, sin depender de WPF ni SQLite real.
