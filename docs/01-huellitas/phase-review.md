# Revisión de cierre — fase 2, Huellitas

Fecha: 6 de octubre de 2026. Resultado: entregables revisados contra los descriptores de Sobresaliente de C.E. 1.8 y 1.9. Esta revisión interna no sustituye la calificación docente.

## Entregables

| Entregable | Fuente editable | Exportación para entrega |
|---|---|---|
| Análisis del dominio | [domain-analysis.md](analysis/domain-analysis.md) | Documento de trabajo interno. |
| Esquema preliminar | [huellitas-preliminary.drawio](preliminary-uml/huellitas-preliminary.drawio) | [PDF preliminar](../../output/pdf/huellitas-preliminary.pdf) |
| Justificación, subtarea 1A | [relationship-justification.md](technical-justification/relationship-justification.md) | [PDF, una página](../../output/pdf/huellitas-relationships.pdf) |
| UML formal, subtarea 1B | [huellitas-final.drawio](final-uml/huellitas-final.drawio) | [PDF UML](../../output/pdf/huellitas-final-uml.pdf) |
| Requisitos cubiertos | [covered-requirements.md](covered-requirements/covered-requirements.md) | [PDF, contenido dentro de media página](../../output/pdf/huellitas-covered-requirements.pdf) |

Los archivos `.drawio` son XML nativo editable de Draw.io. Los PDF se generaron digitalmente desde la misma definición de clases y relaciones con ReportLab; no se afirma exportación desde una sesión de Draw.io. El enunciado permite una herramienta digital y exportación como imagen o PDF. El stack previsto de UML se conserva mediante fuentes Draw.io.

## Verificación de 1.8

- Nueve clases con responsabilidades distintas y justificadas; sin clases añadidas solo para alcanzar el mínimo.
- Asociación en entrevistas/adopciones; agregación en Foundation–Animal; composición en Animal–HealthRecord y Adoption–FollowUp.
- Generalización Dog/Cat hacia Animal abstracta; especialización de cuidados mediante redefinición de una operación.
- Justificación explica propiedad, independencia, participantes y multiplicidades; supuestos claramente separados del texto del caso.
- PDF justificativo de una página, texto completo y sin recortes, revisado visualmente.

## Verificación de 1.9

- Las nueve clases tienen atributos privados y operaciones relevantes; Animal además incorpora una operación protegida.
- Compartimentos de nombres, atributos y métodos; visibilidades +, -, #; abstracción y redefiniciones indicadas.
- Rombo vacío en el agregado, rombos llenos en los propietarios exclusivos y triángulos vacíos hacia el tipo general.
- Multiplicidades en todas las asociaciones; generalizaciones sin multiplicidades.
- Lista H-RF01 a H-RF05 cubre registro de animales, fichas de adoptantes, salud, entrevistas y seguimiento.
- PDF de cobertura de una página cuyo contenido ocupa 261 puntos de altura, dentro de la mitad superior de A4; inspección visual confirma espacio y legibilidad.
- UML final y preliminar revisados visualmente; texto y relaciones legibles, sin recortes ni conexiones atravesando clases.

## Supuestos y límites

La pertenencia a una fundación, propiedad exclusiva de entradas y seguimientos, campos concretos y límite de una adopción activa se documentan como decisiones del modelo. La composición no ordena eliminar historial ni define cascadas de base de datos. Las adopciones anteriores se conservan.

No existe código ejecutable de Huellitas: la tarea requiere modelado. Los criterios transversales reciben evidencia de orden, elección de herramientas, reutilización y valor contextual; no se declaran globalmente completados por esta fase ni se inventan ahorros medidos o colaboración.

**Cierre:** fase 2 completada con evidencia para 1.8 y 1.9. Sin brechas identificadas en la revisión interna. La siguiente fase es Tecnobox y requiere desarrollar sus entregables, aún pendientes.

## Preparación de exposición

Se añadió una [defensa oral](technical-justification/oral-defense.md) con guion, preguntas y límites del modelo. Es material de apoyo y no sustituye los entregables de extensión limitada.
