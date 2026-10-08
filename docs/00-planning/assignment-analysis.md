# Registro del enunciado ENE — PRO205 2026

**Fecha de incorporación y análisis:** 6 de octubre de 2026.

## Fuente y uso

El usuario entregó `C:/Users/blasp/Downloads/ENE_VERSIÓN ESTUDIANTE_PRO205_2026.html` para analizarlo y conservar el contexto. Se guardó una copia íntegra en [sources/ENE_VERSIÓN ESTUDIANTE_PRO205_2026.html](sources/ENE_VERSIÓN%20ESTUDIANTE_PRO205_2026.html) y una [extracción de texto](sources/ene-student-text.txt) para consulta. La copia HTML se comprobó contra el original mediante SHA-256.

SHA-256 de la copia: `B35F993B1CB1FFE57CF07CE6502B32915D900E4983B9E8DC2C344AEA91CFC793`.

El HTML es el enunciado estudiantil: define casos, tareas, productos y límites de extensión. `PRO205_RUBRICA_COMPLETA.md` define el nivel de desempeño buscado. El plan maestro organiza el trabajo y elige herramientas. Las instrucciones académicas del documento se registran como requisitos de la entrega; no constituyen una autorización independiente para publicar, enviar documentos o iniciar todas las fases.

## Contexto general

Evaluación Nacional de Especialidad de tipo Habilidades para PRO205 Taller de Programación. Evalúa competencias técnicas y sello formativo: innovación/emprendimiento, sostenibilidad, habilidades digitales y habilidades personales/sociales. Los tres casos son hipotéticos y de elaboración propia institucional.

La modalidad individual/grupal, fechas, horarios, espacios, ejecución directa/indirecta y precisiones del sello deben obtenerse de las instrucciones del docente/aula virtual: este HTML no las determina. El uso de IA aparece entre los ejemplos de herramientas digitales, sin detallar aquí una política específica de uso.

## Tarea 1 — Fundación Huellitas → fase 2

**Problema:** organización sin fines de lucro que rescata, cuida y da en adopción perros y gatos abandonados. Los registros manuales producen pérdida de datos, desorganización y dificultad para seguir adopciones.

**Procesos expresamente mencionados:** registro de animales, fichas de adoptantes, historial de salud, agendamiento de entrevistas y seguimiento post-adopción. El diseño debe ser claro, eficiente y extensible para un equipo sin experiencia tecnológica.

| Subtarea | Producto del enunciado | Contraste con Sobresaliente |
|---|---|---|
| 1A — Análisis y relaciones | Esquema preliminar digital o manual escaneado con al menos cinco clases; tres tipos de relaciones y jerarquía de generalización/especialización; justificación técnica de **máximo una página**. | C.E. 1.8: clases bien estructuradas; asociación, agregación y composición fundamentadas, además de la jerarquía clara. |
| 1B — UML formal | Al menos cinco clases con atributos y métodos relevantes; relaciones tipadas y visibilidades; herramienta digital y exportación como imagen o PDF; lista de requisitos cubiertos de **máximo media página**. | C.E. 1.9 agrega revisión explícita de multiplicidades, notación estandarizada y cobertura completa/coherente. |

Animales, adoptantes, voluntarios y seguimientos aparecen como ejemplos de entidades, no como un modelo de clases definitivo. Todavía deben justificarse las clases, atributos, métodos y ciclos de vida. No se pide implementar una aplicación Huellitas.

## Tarea 2 — Tecnobox → fase 3

**Problema:** PyME que vende productos tecnológicos por internet y necesita controlar entradas, salidas y stock. El proyecto está en etapa temprana, sin diseño detallado.

**Alcance de esta tarea:** analizar y planificar el diseño OO dentro del ciclo de vida; no desarrollar el sistema de inventario.

| Subtarea | Producto del enunciado | Contraste con Sobresaliente |
|---|---|---|
| 2A — Etapas de diseño OO | Cuadro comparativo o esquema con etapas y ubicación dentro del ciclo; objetivos, actividades y productos; descripción escrita de **máximo una página**. Se permiten modelos cascada, incremental, espiral o ágil. | C.E. 2.3: todas las etapas clave, al menos cuatro, correctamente ubicadas y descritas. Distinguir etapas de diseño OO de fases generales del ciclo, explicando su relación. |
| 2B — POO y ciclo de vida | Mapa o infografía que relacione análisis, diseño, codificación, pruebas y mantenimiento con prácticas de programación; comentario de **máximo media página** sobre calidad y mantenibilidad. | C.E. 2.4: ciclo completo y vínculos directos con modularidad, encapsulamiento y reutilización; argumentación precisa. |

El modelo de desarrollo aún no se elige; se justificará al realizar la fase 3.

## Tarea 3 — Instituto Códice → fases 4, 5 y 6

**Problema:** docentes registran asistencia y notas en Excel o papel, dificultando trazabilidad, respaldo y gestión académica. Se requiere aplicación de escritorio Windows en C#/.NET, organizada, segura y eficiente.

**Funciones expresas del contexto:**

- Registro y edición de estudiantes **y asignaturas**.
- Gestión de asistencia por fecha.
- Ingreso y modificación de calificaciones.
- Informes por estudiante **y por curso**.

| Subtarea | Producto del enunciado | Fase y criterio |
|---|---|---|
| 3A — Requisitos | Mínimo seis requisitos, al menos cuatro funcionales; funcionales y no funcionales; nombre, tipo, descripción y prioridad; formato libre o plantilla docente. | Fase 4, C.E. 2.16. |
| 3B — UML | Diagrama digital como imagen o PDF, mínimo cinco clases; atributos, métodos y relaciones; coherencia con requisitos. | Fase 4, C.E. 2.17; revisar asociación, agregación y herencia conforme al descriptor de Sobresaliente. |
| 3C — Pruebas | Al menos tres pruebas unitarias C# con xUnit, NUnit o MSTest; código funcional y **captura de resultados de ejecución**. Agregar estudiante, asistencia y promedio son ejemplos. | Fase 5, C.E. 2.18. El proyecto mantiene xUnit, AAA y meta interna de ocho a doce pruebas útiles. |
| 3D — Aplicación | Aplicación C# Windows Forms o WPF que implemente al menos tres requisitos definidos; ejecutable funcional preliminar y manual técnico de **máximo dos páginas**, describiendo clases, pantallas y funcionalidades. | Fase 6, C.E. 2.19. Mantener WPF según el plan; incluir flujo de uso para cubrir también la rúbrica. |

Se confirma que WPF y xUnit están permitidos. El enunciado no obliga SQLite, EF Core, MVVM ni una versión específica de .NET: se mantienen como decisiones internas. La persistencia sirve al contexto de respaldo/trazabilidad, pero el motor concreto es elección del proyecto.

El promedio figura como ejemplo para pruebas, no como requisito independiente expresamente enumerado. Cursos aparecen en el contexto de informes; su representación debe analizarse. No se especifican escala de notas, ponderaciones, redondeo, estados de asistencia, identificadores, autenticación ni formato de informes: definirlos con fundamento y señalar supuestos antes de implementar.

## Ajustes de planificación derivados

1. Enunciados disponibles: retirar el pendiente de obtención; mantener pendiente su análisis detallado en cada fase.
2. Incorporar límites de páginas y exportaciones a los checklists; evaluar esas extensiones en los documentos finales renderizados.
3. Incluir explícitamente asignaturas e informes por curso en el levantamiento de Códice. Las funciones sugeridas en el plan maestro no reemplazan estas indicaciones.
4. Separar las etapas de diseño OO de las etapas generales del ciclo de vida al elaborar Tecnobox.
5. Conservar las metas internas como metas, sin presentarlas como obligaciones de la rúbrica.
6. No iniciar automáticamente fase 2: este registro documenta la fuente y prepara su entrada; aún no produce su análisis, UML ni justificación.

## Próximo trabajo ordenado

Fase 2: analizar los cinco procesos de Huellitas, identificar requisitos y entidades, proponer relaciones con sus fundamentos, elaborar esquema preliminar y justificación de una página; luego producir UML formal y lista de cobertura de media página. Revisar 1.8 y 1.9 antes de cerrar.

## Actualización de ejecución — 6 de octubre de 2026

El usuario autorizó proceder con fase 2. Su análisis y entregables están completados; consultar [phase-review.md](../01-huellitas/phase-review.md). Las indicaciones previas de no inicio describen el estado al incorporar el enunciado. Tecnobox y Códice continúan pendientes.

Actualización posterior: Tecnobox completado y revisado; evidencia en [phase-review.md](../02-tecnobox/phase-review.md). Códice aún pendiente.

## Fuente adicional preservada — texto pegado, 6 de octubre de 2026

Se conservó una copia íntegra del adjunto entregado por el usuario en [ene-student-pasted-2026-10-06.txt](sources/ene-student-pasted-2026-10-06.txt). Procedencia: adjunto Texto pegado.txt de esta conversación. SHA-256 comprobado contra el original: 995AD03C70CA52417BCE201A1EE6032470D7F1B4B0786EE4D5B1B8B3ED598023.

El texto contiene las instrucciones generales y los tres casos completos con sus subtareas: Huellitas (1A/1B), Tecnobox (2A/2B) y Códice (3A–3D). La lectura y contraste con el registro previo no identifican cambios de alcance, productos o límites de extensión respecto al HTML preservado. No se exige identidad byte a byte entre ambas versiones, pues provienen de formatos diferentes.

Códice confirma Windows/C#/.NET, estudiantes y asignaturas, asistencia por fecha, calificaciones e informes por estudiante y curso; mínimo seis requisitos con cuatro funcionales, cinco clases UML, tres pruebas, tres requisitos implementados y manual de máximo dos páginas. No especifica escala de notas, fórmula de promedio ni estados de asistencia: las decisiones D01–D06 del análisis siguen abiertas.

Este registro conserva una fuente académica; no inicia nuevas fases ni modifica automáticamente el diseño. Estado vigente: fases 1–3 completadas mediante revisión interna; fase 4 iniciada con catálogo borrador y trazabilidad preliminar, pendiente de reglas, UML y cierre.

## Investigación externa aportada

El usuario compartió una investigación sobre el contexto chileno para Códice. Se conserva íntegra en [codice-chile-research-2026-10-06.txt](sources/codice-chile-research-2026-10-06.txt); su [registro de análisis](../03-codice/requirements/chile-research-review.md) separa propuestas de hechos aún sin verificar independientemente. No modifica el enunciado ni adopta todas las recomendaciones.
