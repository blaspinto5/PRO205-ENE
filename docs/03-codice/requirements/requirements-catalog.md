# Códice — catálogo inicial de requisitos

**Versión:** 0.3, actualización del 6 de octubre de 2026. **Estado:** requisitos implementados y contrastados internamente en fase 6. El catálogo conserva la técnica/origen y criterios de aceptación definidos; evidencia actual en implementation-traceability.md. Consultar [business-rules.md](business-rules.md).

Técnica aplicada: análisis documental de la tarea 3 y descomposición de sus funciones en operaciones verificables. Fuente: [enunciado registrado](../../00-planning/assignment-analysis.md). No se afirma haber realizado entrevistas; se abrió una consulta sobre reglas de notas/asistencia.

Prioridades propuestas: **Alta**, necesaria para el núcleo descrito; **Media**, calidad de uso o soporte que se debe concretar. La prioridad no representa ponderación de la rúbrica. Los campos técnicos se mantendrán en inglés al diseñar y programar.

## Funcionales — funciones expresas del caso

| ID | Nombre | Tipo | Descripción | Prioridad | Origen | Criterio de aceptación previsto |
|---|---|---|---|---|---|---|
| RF01 | Registrar estudiante | Funcional | Permitir al docente crear una ficha de estudiante con los datos de identificación definidos para el sistema. | Alta | Registro de estudiantes. | Con datos válidos se crea una ficha consultable; nombres/apellidos obligatorios, ID interno y código único estable según BR10. |
| RF02 | Editar estudiante | Funcional | Permitir actualizar los datos modificables de una ficha existente, conservando sus vínculos académicos. | Alta | Edición de estudiantes. | La ficha presenta los datos actualizados y sus notas/asistencias mantienen la relación con el mismo estudiante. |
| RF03 | Registrar asignatura | Funcional | Permitir crear una asignatura con identificación y nombre, conforme al esquema de cursos acordado. | Alta | Registro de asignaturas. | La asignatura creada puede seleccionarse para la operación académica correspondiente; ID interno, código único estable y nombre; se relaciona con cursos según BR11–BR12. |
| RF04 | Editar asignatura | Funcional | Permitir actualizar los datos modificables de una asignatura existente conservando sus registros relacionados. | Alta | Edición de asignaturas. | El nombre o dato modificable cambia sin desvincular registros académicos existentes. |
| RF05 | Gestionar asistencia por fecha | Funcional | Registrar y consultar la asistencia de estudiantes para una fecha en el contexto académico definido; gestionar correcciones según la regla acordada. | Alta | Gestión de asistencia por fecha; registro diario por docentes. | Se recupera el registro para estudiante, fecha y contexto correctos. Presente/Ausente por sesión; ausencia de registro diferenciada; unicidad y corrección según BR06–BR09. |
| RF06 | Registrar y modificar calificaciones | Funcional | Permitir ingresar y actualizar calificaciones vinculadas a un estudiante y una asignatura en su contexto académico. | Alta | Ingreso y modificación de calificaciones. | La nota registrada o modificada aparece en el contexto correcto; escala 1,0–7,0 con una décima; una nota por matrícula/evaluación, conforme a BR01–BR03. |
| RF07 | Visualizar informe por estudiante | Funcional | Mostrar la información de asistencia y calificaciones de un estudiante, con alcance temporal y contexto acordados. | Alta | Informes por estudiante; gestión de asistencia y notas. | El informe corresponde al estudiante seleccionado y refleja sus registros; detalle y pendientes, promedio simple y conteos de asistencia definidos en BR03–BR05 y BR16. |
| RF08 | Visualizar informe por curso | Funcional | Mostrar la información académica de los estudiantes de un curso, según asignaturas y alcance acordados. | Alta | Informes por curso. | El informe incluye los estudiantes del curso seleccionado sin mezclar los de otros cursos; matrícula por sección de asignatura/período y contenido definidos en BR12–BR17. |

Registrar/modificar notas se conserva como una función de gestión relacionada, mientras estudiantes/asignaturas se descomponen para comprobar explícitamente ambos procesos. El número de filas no justifica añadir alcance nuevo. Las ocho funciones se concretan mediante BR01–BR20; su trazabilidad final está en implementation-traceability.md.

## No funcionales — propuestas contextualizadas

Son decisiones propuestas para concretar organización, seguridad, eficiencia y respaldo del contexto; no son frases literales de la rúbrica ni requisitos de infraestructura impuestos por el docente.

| ID | Nombre | Tipo | Descripción | Prioridad | Fundamento | Criterio de aceptación previsto |
|---|---|---|---|---|---|---|
| RNF01 | Persistencia local | No funcional | Conservar los registros confirmados entre sesiones mediante almacenamiento local, según SQLite definido en el plan. | Alta | Superar limitaciones de registro/trazabilidad; elección interna de persistencia. | Tras guardar, cerrar y reabrir la aplicación, los registros y sus vínculos se recuperan. No equivale a copia de seguridad externa. |
| RNF02 | Integridad de información | No funcional | Evitar guardar registros incompletos o que incumplan reglas acordadas; conservar relaciones válidas al editar y ante operaciones rechazadas. | Alta | Gestión organizada y segura. | Una operación inválida no deja datos parciales ni vínculos huérfanos; informa el motivo. Reglas concretas definidas en BR01–BR18. |
| RNF03 | Claridad de interacción | No funcional | Presentar formularios y mensajes comprensibles en español, con identificación de datos obligatorios y resultado de operaciones. | Media | Uso por docentes; interfaz funcional y clara evaluada en 2.19. | En un recorrido de registro, edición y consulta se identifica la acción, el dato inválido si existe y el resultado, sin mostrar excepciones técnicas al usuario. |
| RNF04 | Separación y mantenibilidad | No funcional | Mantener reglas de negocio independientes de WPF y del almacenamiento concreto, conforme a las capas del proyecto. | Alta | Buenas prácticas y pruebas; decisión arquitectónica. | Domain/Application se compilan y prueban sin Desktop ni una base SQLite real; ViewModels no implementan reglas de notas/asistencia. |

La integridad no sustituye autenticación o control de acceso, y el almacenamiento local no garantiza respaldo ante pérdida del equipo. BR19–BR20 delimitan uso académico con datos ficticios y respaldo manual fuera de la aplicación; una aplicación real requiere otra revisión. La eficiencia se menciona como necesidad contextual, pero no se inventa un límite de tiempo o volumen sin escenario acordado.

## Revisión inicial contra 2.16

- Ocho requisitos funcionales y cuatro no funcionales, con nombre, tipo, descripción y prioridad completos.
- Origen y aceptación previstos documentados para facilitar la revisión.
- Requisitos explícitos separados de propuestas y decisiones pendientes.
- Cantidad y estructura preparadas para Sobresaliente; reglas adoptadas registradas; falta revisar modelo y trazabilidad final. No se declara criterio cerrado.

## Funciones de soporte derivadas del contexto adoptado

No se añade un sistema de admisión. Estas operaciones permiten alimentar los vínculos requeridos por asistencia, calificaciones e informes; elevan el catálogo a doce funcionales y cuatro no funcionales, superando justificadamente la meta interna de ocho.

| ID | Nombre | Tipo | Descripción | Prioridad | Criterio de aceptación |
|---|---|---|---|---|---|
| RF09 | Crear curso | Funcional | Crear una sección de una asignatura en un período con fechas de inicio/término. | Alta | Combinación asignatura/período/sección única; fechas coherentes, según BR12. |
| RF10 | Matricular estudiante | Funcional | Vincular estudiante a curso con fecha de matrícula. | Alta | Un vínculo por estudiante/curso; fecha dentro del curso; conservar historial, BR13. |
| RF11 | Crear sesión | Funcional | Crear una sesión fechada de un curso para registrar asistencia. | Alta | Curso/fecha-hora únicos y fecha dentro del curso, BR06/BR14. |
| RF12 | Crear evaluación | Funcional | Definir una evaluación de un curso con nombre y fecha para registrar notas. | Alta | Nombre normalizado único por curso y fecha válida; pendientes sin nota artificial, BR02/BR14. |

RF07/RF08 incluyen promedio simple informativo y conteos, sin estado académico final. RNF01 se complementa con respaldo manual documentado y comprobación de restauración en fase 6, sin una función de backup automático.
