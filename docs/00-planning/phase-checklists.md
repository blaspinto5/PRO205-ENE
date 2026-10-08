# Checklists de fases — PRO205 ENE

Marcar un punto solo con evidencia verificable. Si falta una exigencia de cierre, corregir antes de avanzar. Los mínimos siguientes corresponden al descriptor de Sobresaliente, no al simple nivel de aprobación.

## Fase 1 — Planificación y arquitectura

Revisión de cierre: **5 de octubre de 2026**.

- [x] Stack definido y justificado: `architecture-decisions.md`, ADR-01.
- [x] Arquitectura y dependencias definidas: ADR-02 y ADR-03.
- [x] Estructura definida y preparada: `project-plan.md`, sección Organización, y carpetas `docs/`, `src/`, `tests/`.
- [x] Idioma y comentarios definidos: `project-plan.md`, sección Idioma, nombres y comentarios.
- [x] Convenciones definidas y `.editorconfig` preparado.
- [x] Estrategia de pruebas definida: plan y ADR-04.
- [x] Estrategia Git definida y repositorio local inicializado; `.gitignore` preparado.
- [x] Matriz rúbrica → entregable → evidencia definida para once criterios y 100 % del peso: `rubric-matrix.md`.
- [x] Los cuatro documentos de planificación existen.
- [x] Reglas de negocio independientes de WPF por diseño; aún no hay código productivo que auditar.
- [x] Insumos ausentes y validaciones del entorno registrados; no se inventó el contenido de los casos.

**Resultado:** fase 1 cerrada como planificación. No equivale a haber obtenido Sobresaliente en los once criterios. No se ha construido ni probado una aplicación.

**Entradas posteriores:** los enunciados fueron incorporados el 6 de octubre de 2026 y se registran en `assignment-analysis.md`; analizar cada caso en su fase; comprobar entorno antes de crear la solución C#; determinar destino GitHub cuando corresponda publicar. No son criterios adicionales inventados de la rúbrica.

## Fase 2 — Huellitas · 1.8 y 1.9

- [x] Enunciado disponible y analizado; requerimientos del caso identificados.
- [x] Análisis, esquema preliminar y justificación técnica completos; justificación de máximo una página.
- [x] Cinco o más clases correctamente estructuradas.
- [x] Asociación, agregación y composición fundamentadas, más jerarquía clara de generalización/especialización.
- [x] UML final con atributos y métodos por clase, relaciones correctas, visibilidad, multiplicidades y notación estándar.
- [x] Lista completa y coherente de requerimientos cubiertos, de máximo media página.
- [x] UML formal generado digitalmente y exportado como imagen o PDF; editable y justificación guardados; matriz actualizada con evidencia.

**Cierre:** completada el 6 de octubre de 2026. Evidencia y revisión en `docs/01-huellitas/phase-review.md`; PDFs en `output/pdf/`. Revisión interna contra Sobresaliente, sujeta a evaluación docente.

## Fase 3 — Tecnobox · 2.3 y 2.4

- [x] Enunciado disponible y analizado.
- [x] Todas las etapas clave de diseño OO, al menos cuatro, correctamente ubicadas.
- [x] Cada etapa explica objetivo, actividades y productos con lenguaje técnico; descripción escrita de máximo una página. Diferenciar diseño OO y ciclo de vida.
- [x] Esquema claro, preciso y técnicamente correcto.
- [x] Mapa o infografía representa el ciclo de vida completo pertinente al caso.
- [x] Vínculos directos con modularidad, encapsulamiento y reutilización.
- [x] Comentario de máximo media página argumenta con precisión el aporte de POO a la calidad y mantenibilidad.
- [x] Entregables y evidencia guardados; matriz actualizada.

**Cierre:** completada el 6 de octubre de 2026. Evidencia en `docs/02-tecnobox/phase-review.md` y cuatro PDF en `output/pdf/`; revisión interna contra Sobresaliente de 2.3 y 2.4.

## Fase 4 — Códice: requisitos y UML · 2.16 y 2.17

- [x] Enunciado disponible; reglas y alcance confirmados, incluyendo estudiantes y asignaturas, asistencia por fecha, calificaciones e informes por estudiante y curso.
- [x] Seis o más requisitos, mínimo cuatro funcionales, correctamente clasificados.
- [x] Nombre, tipo, descripción y prioridad completos; IDs para seguimiento.
- [x] Redacción clara y técnica.
- [x] Meta interna de ocho funcionales y cuatro no funcionales revisada, sin inventar alcance.
- [x] Cinco o más clases con atributos, métodos y relaciones correctas de asociación, agregación y herencia, fundamentadas en el caso.
- [x] Notación UML estándar y correspondencia con requisitos; diagrama digital exportado como imagen o PDF.
- [x] Matriz de trazabilidad iniciada; pruebas y pantallas futuras identificadas como previstas.
- [x] Entregables revisados y matriz de rúbrica actualizada.

**Cierre:** fase 4 completada; evidencia en `docs/03-codice/phase-4-review.md`. Revisión interna contra 2.16/2.17; pruebas e implementación pendientes.

## Fase 5 — Domain, Application y pruebas · 2.18

- [x] SDK .NET 10 comprobado para Domain/Application; WPF se verificará en fase 6.
- [x] Domain y Application implementados conforme al modelo, sin dependencia de UI ni persistencia concreta.
- [x] Tres o más pruebas unitarias funcionales que validan con precisión requisitos clave.
- [x] Meta interna de ocho a doce pruebas útiles cumplida o ajuste justificado.
- [x] xUnit, nombres descriptivos, AAA y asserts específicos.
- [x] Casos positivos y negativos de reglas confirmadas; pruebas independientes y sin base real.
- [x] `dotnet test` sin errores; captura de resultados de ejecución guardada junto al código de pruebas.
- [x] Trazabilidad y matriz de rúbrica actualizadas.

**Cierre:** completada el 6 de octubre de 2026. Compilación sin errores/advertencias y 41 casos aprobados. Ampliación a 23 métodos justificada en docs/03-codice/phase-5-review.md.

## Fase 6 — Persistencia, WPF y entrega · 2.19

- [x] Persistencia SQLite/EF Core y repositorios integrados; pruebas reales en phase-6-foundation.md.
- [x] Aplicación C# ejecutable con al menos tres requisitos funcionales implementados.
- [x] Clases POO bien estructuradas; reglas fuera de WPF; ViewModel consume CatalogService.
- [x] Interfaz funcional y clara; flujos comprobados con datos representativos.
- [x] Manual técnico de máximo dos páginas con clases principales, pantallas, funcionalidades y flujo de uso; documentación adicional separada si hace falta.
- [x] UML, requisitos, pruebas y código consistentes; trazabilidad completa.
- [x] `dotnet build` correcto y `dotnet test` sin fallos.
- [x] Ejecutable, manual, capturas y evidencia disponibles; README actualizado.
- [x] Once criterios contrastados con la evidencia disponible; brechas registradas en la auditoría.
- [ ] Evaluación docente y demostración personal realizadas; no se garantiza Sobresaliente por la existencia de documentos.

**Cierre:** fase 6 completada internamente el 6 de octubre de 2026. Doce funciones, 55 pruebas aprobadas, renders WPF, manual de dos páginas y paquete portable verificados. Otro equipo limpio no verificado; límites en phase-6-review.md.

## Revisión transversal en cada fase

- [x] Justificar valor, innovación, emprendimiento y sostenibilidad mediante características y decisiones pertinentes al caso.
- [x] Evidenciar selección y uso correcto de herramientas digitales.
- [x] Registrar orden, autonomía, responsabilidad, revisiones y comunicación/colaboración cuando corresponda.
- [x] Registrar brechas y corregirlas; evitar afirmar cumplimiento solo por disponer de documentos.

## Registro de revisiones posteriores

Actualización del 8 de octubre de 2026: historial Git real iniciado y destino público autorizado (`blaspinto5/PRO205-ENE`); 55 casos revalidados. Corregida la afirmación incondicional de revisión visual. Estado de aceptación vigente en `docs/03-codice/portable/portable-architecture.md`; otro Windows limpio, restauración manual y evaluación docente pendientes. La plantilla docente será incorporada cuando el estudiante la consiga.

| Fecha | Fase/criterio | Entregable y ubicación de evidencia | Brecha o corrección | Resultado |
|---|---|---|---|---|
| 2026-10-05 | Fase 1 | Cuatro documentos en `docs/00-planning/`, configuración y carpetas del repositorio | Enunciados pendientes como entradas de fases 2–4; entorno pendiente antes de implementación; remoto sin configurar. | Planificación cerrada; criterios académicos planificados. |

| 2026-10-06 | Entrada documental de fases 2–6 | HTML preservado, extracción y assignment-analysis.md | Resuelta la ausencia de enunciados; incorporados límites de extensión, exportaciones y alcance de Códice. | Fuente disponible; fases académicas aún sin iniciar. |
| 2026-10-06 | Fase 2, C.E. 1.8 y 1.9 | docs/01-huellitas/phase-review.md y cuatro PDF en output/pdf/ | Nueve clases, relaciones y cobertura revisadas; justificación de una página y lista dentro de media página. | Fase 2 completada; revisión interna contra Sobresaliente. |
| 2026-10-06 | Defensa de fase 2 y preparación de fase 3 | docs/01-huellitas/technical-justification/oral-defense.md y docs/02-tecnobox/phase-plan.md | Guion y preguntas de defensa; modelo incremental propuesto, etapas OO, matriz de contenido y orden de entregables. | Defensa preparada; fase 3 preparada, sin cierre de 2.3/2.4. |
| 2026-10-06 | Fase 3, C.E. 2.3 y 2.4 | docs/02-tecnobox/phase-review.md, fuentes editables y cuatro PDF | Cuadro de cinco etapas OO, ciclo de seis fases, explicación de una página y comentario dentro de media página revisados. | Fase 3 completada; revisión interna contra Sobresaliente. |
| 2026-10-06 | Defensa de fase 3 | docs/02-tecnobox/technical-commentary/oral-defense.md | Guion, versión breve y preguntas contrastadas con los entregables. | Material de exposición preparado; estado de fase 3 sin cambios. |
| 2026-10-06 | Inicio de fase 4 | docs/03-codice/phase-4-plan.md y requirements/ | Catálogo borrador: ocho funcionales, cuatro no funcionales, decisiones pendientes y trazabilidad preliminar. | Fase 4 iniciada; UML y validación pendientes. |
| 2026-10-06 | Preservación de fuente adicional | docs/00-planning/sources/ene-student-pasted-2026-10-06.txt y assignment-analysis.md | Texto completo guardado y copia verificada por SHA-256; sin cambios de instrucciones identificados. | Contexto conservado; decisiones académicas pendientes sin cambios. |
| 2026-10-06 | Reglas de Códice | docs/03-codice/requirements/business-rules.md y catálogo v0.2 | D01–D06 adoptadas; doce funciones con cuatro operaciones de soporte y cuatro no funcionales. | Base de reglas definida; UML y cierre de fase 4 pendientes. |
| 2026-10-06 | Fase 4, 2.16/2.17 | docs/03-codice/phase-4-review.md | Nueve clases UML, catálogo formal, reglas y trazabilidad revisados. | Fase 4 cerrada internamente. |
| 2026-10-06 | Defensa de fase 4 y preparación de fase 5 | docs/03-codice/uml/oral-defense.md y phase-5-plan.md | Defensa, orden Domain/Application/xUnit y doce métodos de prueba previstos; SDK disponible 9.0.318, falta .NET 10. | Fase 5 preparada, sin implementación ni ejecución de pruebas. |

| 2026-10-06 | Fase 5, 2.18 | phase-5-review.md, código, TRX y captura | SDK 10.0.401 local; UML sincronizado; 23 métodos justificados. | 41 casos aprobados; cierre interno. |

| 2026-10-06 | Defensa de fase 5 | docs/03-codice/phase-5-oral-defense.md | Guion, versión breve, preguntas y demostración vinculados al código y evidencia. | Defensa preparada; fase 6 pendiente. |
| 2026-10-06 | Arquitectura de fase 6 | docs/03-codice/portable/portable-architecture.md y perfil PortableWinX64 | Publicación autocontenida, rutas relativas al ejecutable y protección de datos definidos. | Preparación completa; implementación y aceptación portable pendientes. |

| 2026-10-06 | Base de fase 6 | docs/03-codice/phase-6-foundation.md, código, 49 resultados y publicación portable | SQLite, migración, repositorios y arranque implementados; carpetas ordenadas. | Base verificada; formularios/manual y cierre de fase 6 pendientes. |

| 2026-10-06 | Cierre de fase 6 / 2.19 y revisión transversal | phase-6-review.md, 55 pruebas, doce renders, manual y ZIPs | UI completa, estilos compartidos, referencia ≥4 y filtro individual, portable con permisos/bloqueo/traslado comprobados. | Cierre interno; otro equipo limpio y evaluación docente pendientes. |
