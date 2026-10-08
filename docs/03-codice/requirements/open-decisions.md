# Códice — registro de decisiones de análisis

Fecha: 6 de octubre de 2026. Estas preguntas derivan de vacíos del enunciado; no añaden obligaciones académicas. Responder con instrucciones del docente o acordar supuestos explícitos antes de implementar reglas afectadas. Las partes independientes del análisis pueden avanzar.

| ID | Decisión | Información pendiente | Requisitos afectados | Consecuencia en el modelo |
|---|---|---|---|---|
| D01 | Calificaciones | Escala, extremos válidos, precisión, identificación de evaluación, notas pendientes y si se necesita promedio simple/ponderado o redondeo. | RF06, RF07, RF08, RNF02 | Estructura de Grade y posible Assessment; no implementar fórmulas por suposición. |
| D02 | Asistencia | Estados permitidos; por día, asignatura o sesión; clave de duplicidad y posibilidad de corregir una asistencia existente. | RF05, RF07, RF08, RNF02 | Contexto y unicidad de AttendanceRecord; reglas de actualización. |
| D03 | Identificación | Datos obligatorios de estudiante/asignatura y qué identifica una ficha sin duplicados. No imponer RUT ni validación nacional sin indicación. | RF01–RF04, RNF02 | Campos, identificadores y validaciones. |
| D04 | Cursos y matrícula | Qué representa un curso, relación curso–asignatura, inscripción de estudiantes y períodos académicos. | RF03, RF05–RF08 | Course, Subject y Enrollment como candidatos; claves académicas. |
| D05 | Informes | Columnas, filtros, fechas/períodos, detalle de asistencia/notas y agregados; promedio o porcentajes si se requieren. | RF07, RF08 | DTOs de reporte y casos de uso; sin exportación automática no solicitada. |
| D06 | Protección y respaldo | Datos esperados, uso local individual/compartido y si se exige autenticación, permisos o copia de seguridad externa. | RNF01–RNF03 | Alcance de infraestructura y funciones adicionales solo si se acuerdan. |

## Consulta abierta

Se consultó al usuario si el docente indicó escala de notas, forma de promedio y estados de asistencia. Hasta recibir respuesta no se adopta una regla académica por defecto. Las otras decisiones se conservarán para revisión progresiva del diseño.

## Registro de acuerdos

| Decisión | Acuerdo | Fuente/autoridad | Estado |
|---|---|---|---|
| D01–D06 | Reglas del prototipo definidas en [business-rules.md](business-rules.md). | Autorización del usuario para escoger reglas; referencia institucional verificada para escala/precisión. | Adoptadas como decisiones de proyecto; no validación docente. |

## Decisiones de proyecto ya definidas

C#, .NET 10, WPF/MVVM, SQLite/EF Core y xUnit; separación Domain/Application/Infrastructure/Desktop. Se mantienen desde fase 1. No confundir esas elecciones técnicas con respuestas a reglas académicas.

## Referencia recibida

Se recibió investigación sobre Chile, conservada y clasificada en [chile-research-review.md](chile-research-review.md). Aporta propuestas para D01–D06; no representa validación docente ni adopción automática. Las decisiones siguen abiertas hasta consolidar reglas del proyecto.

## Resolución posterior

Las preguntas y consultas anteriores son el registro histórico del análisis. El usuario autorizó seleccionar reglas: D01–D06 están resueltas en [business-rules.md](business-rules.md). Incluyen notas 1–7, promedio simple, asistencia por sesión, códigos internos, curso como sección de una asignatura y respaldo manual del prototipo. No se mantiene pendiente la respuesta a la consulta previa para avanzar con estas decisiones.
