# Códice — reglas adoptadas para el prototipo

Fecha: 6 de octubre de 2026. Autoridad: el usuario autorizó escoger las reglas más adecuadas al proyecto. Alcance: aplicación académica local y acotada; estas decisiones completan el enunciado y no constituyen un reglamento institucional real ni asesoría legal.

## Referencia verificada

Se consultó la [página oficial de calificaciones AIEP](https://respuestas.aiep.cl/es_CL/academico/%C2%BFcomo-funciona-el-proceso-de-calificaciones-en-aiep), que informa escala 1,0–7,0 y mínimo 4,0. Se verificó además el artículo 24 del [texto refundido AIEP de 7 de enero de 2025](https://transparencia.aiep.cl/wp-content/uploads/2025/01/2025.01.07-Decreto-Modifica-Reglamento-Academico.pdf): un decimal y aproximación de centésimas desde cinco. Este PDF se usa como referencia fechada, no como afirmación de reglamento consolidado vigente en 2026.

Se adoptan escala y precisión como inspiración institucional. Promedio simple, asistencia por sesión, estructura de cursos e identificación son decisiones de Códice. No se adopta el régimen completo de exámenes, ponderaciones o asistencia AIEP. No se emiten conclusiones jurídicas sobre normativa de datos no verificada en esta revisión.

## D01 — Notas y promedio

- **BR01:** calificaciones numéricas entre 1,0 y 7,0, extremos incluidos. Usar `decimal` en el código; admitir como máximo un decimal en ingreso/modificación. Rechazar valores fuera de rango o con precisión excesiva, sin redondearlos silenciosamente.
- **BR02:** cada evaluación pertenece a un curso; cada estudiante matriculado tiene como máximo una nota por evaluación. La evaluación tiene nombre obligatorio y fecha. El nombre es único dentro del curso, ignorando espacios extremos y diferencias de mayúsculas; una reevaluación requiere otro nombre explícito.
- **BR03:** una evaluación sin nota registrada se muestra como pendiente para el estudiante; no se crea una nota 0 ni 1 por ausencia. No participa en el promedio. Sin notas, el resultado es “Sin calificaciones”, no cero. El promedio con evaluaciones pendientes se identifica como provisional.
- **BR04:** promedio aritmético simple de las notas registradas del estudiante en un curso. Calcular con precisión decimal y redondear una sola vez al mostrar el resultado a un decimal, con empate hacia arriba para valores positivos: 5,45 → 5,5 y 5,44 → 5,4. No mezclar pesos ni aplicar 70/30.
- **BR05:** 4,0 es una referencia informativa de desempeño del promedio mostrado, no una declaración de aprobación final de asignatura. Mostrar “Promedio ≥ 4,0” o “Promedio < 4,0”; no aprobar/reprobar automáticamente ni evaluar umbrales de asistencia. La comparación usa el promedio presentado a un decimal para evitar contradicciones visibles.

## D02 — Asistencia

- **BR06:** cada sesión pertenece a un curso y tiene fecha/hora de inicio. Permitir varias sesiones del mismo curso el mismo día si difiere la hora; impedir duplicar curso + fecha/hora. La hora identifica sesiones, sin calcular asistencia ponderada por duración.
- **BR07:** estados registrados: `Present` y `Absent`, mostrados como Presente/Ausente. Una sesión sin registro para un matriculado se muestra “Sin registrar”; no equivale a Ausente.
- **BR08:** una asistencia por matrícula + sesión; la matrícula y sesión deben pertenecer al mismo curso. Registrar un duplicado se rechaza; corregir se hace mediante actualización explícita. Conservar fecha/hora de última modificación, sin añadir auditoría completa.
- **BR09:** informe de asistencia con cantidades de presentes, ausentes y sin registrar para sesiones realizadas desde la fecha de matrícula hasta la fecha de corte. Excluir sesiones futuras. No calcular porcentaje ni reprobación automática en esta versión, evitando un denominador ambiguo.

## D03 — Identificación

- **BR10:** estudiante con ID interno generado, código de estudiante único y nombres/apellidos obligatorios. Código estable y no editable; identificación por código, no por nombre. Normalizar espacios extremos y comparar código sin distinguir mayúsculas. No exigir RUN/RUT, pasaporte, domicilio, teléfono ni otros datos para este prototipo; usar datos ficticios.
- **BR11:** asignatura con ID interno generado, código único obligatorio y nombre obligatorio. Código estable y no editable; comparación normalizada como en BR10. Editar nombre no cambia identidad ni vínculos existentes.

## D04 — Cursos y matrícula

- **BR12:** Course representa una sección concreta de una asignatura durante un período. Tiene ID, código de sección, etiqueta de período y fechas de inicio/término. Se identifica por asignatura + período + código de sección; esa combinación es única, con textos normalizados. Una asignatura puede ofrecerse en varios cursos. No se modela una carrera completa ni malla curricular.
- **BR13:** Enrollment vincula un estudiante y un curso con fecha de matrícula; evitar duplicado estudiante + curso. El estudiante puede tener varias matrículas, preservando historial entre períodos. Matrícula dentro de fechas del curso; no implementar retiros o traslados en esta versión.
- **BR14:** evaluación y sesión se vinculan al curso; sus fechas están dentro de inicio/término. Una nota solo corresponde a una evaluación del mismo curso de la matrícula y no anterior a la matrícula. La asistencia tampoco puede registrarse antes de la matrícula ni para una sesión futura. No introducir matrícula global, AcademicPeriod ni SubjectOffering como clases adicionales: sus responsabilidades quedan en Course.
- **BR15:** curso y matrículas se preparan en la aplicación antes de registrar información académica. Proveer casos de uso mínimos de crear curso, matricular estudiante y crear sesión/evaluación, como soporte necesario de las funciones del enunciado. No son un módulo de admisión.

## D05 — Informes

- **BR16:** informe individual muestra estudiante, código, cursos/períodos, asignaturas, notas por evaluación, pendientes, promedio simple y resumen de asistencia. Filtro por curso; todos los cursos disponibles si no se selecciona uno. Respetar fecha de corte para asistencia.
- **BR17:** informe por curso muestra asignatura, sección, período y estudiantes matriculados; por estudiante presenta notas, pendientes, promedio y conteos de asistencia. No incluir rankings, promedio global de cursos distintos ni exportación PDF/Excel en esta versión. Consultar informes no modifica datos.
- **BR18:** las ediciones se reflejan en consultas posteriores. Informes sin datos muestran un estado vacío claro. No se representa la ausencia de notas/asistencia con resultados ficticios.

## D06 — Datos y respaldo

- **BR19:** uso local académico en equipo Windows bajo una cuenta de sistema controlada; datos ficticios. SQLite conserva registros entre sesiones; no añadir login, roles o cifrado de base sin requerimiento específico. No afirmar que SQLite o esta decisión garantizan cumplimiento legal o seguridad de producción.
- **BR20:** respaldar la base mediante copia manual cuando la aplicación está cerrada, incluyendo el conjunto consistente de archivos si el modo SQLite utilizado lo requiere. Documentar y verificar recuperación al integrar persistencia en fase 6; no implementar respaldo automático ni programar frecuencias sin necesidad. No incluir la base con datos en Git.

## Ejemplos para futuras pruebas

| Regla | Escenario | Resultado esperado |
|---|---|---|
| BR01 | 1,0; 7,0; 0,9; 7,1; 5,45 como nota ingresada | Aceptar límites; rechazar fuera de rango y precisión mayor a una décima. |
| BR03–BR04 | 5,4 y 5,5 | Promedio mostrado 5,5. |
| BR03–BR04 | Sin notas; 6,0 y evaluación pendiente | Sin calificaciones; promedio provisional 6,0 sin incluir pendiente. |
| BR05 | Promedio exacto 3,95 | Mostrar 4,0 y referencia “Promedio ≥ 4,0”; sin declarar aprobación final. |
| BR08 | Misma matrícula y sesión dos veces | Rechazar nuevo registro; permitir corrección explícita y actualizar fecha de modificación. |
| BR09 | Sesión realizada sin asistencia registrada | Contar sin registrar; no contar ausente. |
| BR13–BR14 | Matrícula repetida o evaluación de otro curso | Rechazar combinación inválida y conservar datos anteriores. |

## Estado y revisión

D01–D06 resueltas como decisiones adoptadas para el prototipo, bajo autorización del usuario. Si el docente entrega reglas distintas, registrar el cambio y su impacto antes de modificar diseño/pruebas. No son validaciones docentes.

Fases 4 y 5 completadas internamente: UML, Domain/Application y pruebas disponibles. Persistencia e interfaz corresponden a fase 6; evidencia y límites en phase-5-review.md.


Cierre de implementación: fases 4–6 completadas internamente. BR05 se muestra como referencia en detalle del informe; BR16 admite curso opcional; BR19/BR20 se verificaron en persistencia, respaldo/traslado y paquete local. Alcance y límites en phase-6-review.md.
