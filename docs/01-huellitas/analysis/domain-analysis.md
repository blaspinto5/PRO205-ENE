# Huellitas — análisis de dominio

Fecha: 6 de octubre de 2026. Fuente: tarea 1, subtareas 1A/1B del [enunciado preservado](../../00-planning/assignment-analysis.md). Alcance: diseño conceptual, sin implementación ni base de datos.

## Requisitos derivados del contexto

| ID | Necesidad expresa | Responsabilidad en el modelo |
|---|---|---|
| H-RF01 | Registrar animales rescatados, perros y gatos. | Foundation agrupa Animal; Dog y Cat especializan características y cuidados. |
| H-RF02 | Mantener fichas de adoptantes. | Adopter conserva identificación y contacto y permite actualizarlos. |
| H-RF03 | Mantener historial de salud animal. | Animal contiene HealthRecord; agrega registros y devuelve su historial. |
| H-RF04 | Agendar entrevistas. | Interview vincula un adoptante y un animal, registra fecha y resultado y permite reprogramación. |
| H-RF05 | Realizar seguimiento post-adopción. | Adoption identifica animal y adoptante, registra formalización y contiene FollowUp. |

Necesidades cualitativas del contexto: claridad, eficiencia y extensibilidad. El diseño ofrece responsabilidades explícitas y una jerarquía común. No acredita rendimiento, usabilidad ni persistencia mediante implementación, pues esta tarea solo pide modelado.

## Clases y responsabilidades

| Clase | Responsabilidad y razón de existencia |
|---|---|
| Foundation | Representar la fundación y su agrupación de animales; incorporar o retirar animales del catálogo. |
| Animal (abstracta) | Concentrar identificación, nombre y fecha de rescate; administrar historial y definir contrato de cuidados. |
| Dog | Especializar Animal con tamaño y recomendaciones de cuidado de perros. |
| Cat | Especializar Animal con condición de vida interior y recomendaciones de cuidado de gatos. |
| HealthRecord | Representar una entrada fechada del historial: diagnóstico y tratamiento. |
| Adopter | Representar la ficha y contacto de una persona interesada en adoptar. |
| Interview | Representar una entrevista programada, sus participantes y resultado. |
| Adoption | Representar la formalización entre un animal y un adoptante y administrar sus seguimientos. |
| FollowUp | Representar una visita/contacto de seguimiento con fecha, observaciones y resultado. |

Todas las clases tienen atributos y comportamiento relevante en el UML final. Los tipos String, Guid, Date y DateTime son tipos conceptuales; no añaden clases de dominio. Las colecciones se expresan mediante relaciones, sin duplicarlas como atributos en el diagrama.

## Supuestos explícitos de diseño

Estas decisiones completan el modelo; no son reglas textuales del enunciado.

1. Un animal pertenece como máximo a una fundación dentro de este modelo; puede existir sin pertenecer al catálogo, por ejemplo antes de su ingreso o después de un traslado. Retirarlo del catálogo no destruye su identidad ni historial.
2. Una entrada HealthRecord pertenece exclusivamente a un animal. Composición expresa propiedad conceptual; no implica borrado físico de información ni cascada de base de datos. Se conserva el historial al salir de la fundación y no se reutilizan entradas para otro animal.
3. Una entrevista se realiza para un animal y un adoptante; cada participante puede tener cero o muchas entrevistas. No se impone límite de entrevistas ni aprobación automática.
4. Cada adopción vincula un animal y un adoptante. Un animal puede tener varios registros históricos, pero como máximo uno activo simultáneamente. End() termina una adopción sin borrar su historia; Formalize() debe respetar esa condición. No se exige entrevista aprobada para formalizar, porque el caso no especifica esa regla.
5. FollowUp pertenece a una única adopción; su identidad depende de ese proceso y no se transfiere a otra adopción. La adopción y sus seguimientos se conservan como historial.
6. Los campos concretos y operaciones son propuestas pertinentes al caso. Size y IsIndoor permiten especializar cuidados; no se supone que todos los animales de cada especie compartan idénticas necesidades.

## Relaciones y multiplicidades

| Relación | Tipo | Multiplicidad y fundamento |
|---|---|---|
| Foundation — Animal | Agregación compartida | Foundation 0..1 por Animal; Animal 0..* por Foundation. Rombo vacío en Foundation; las partes conservan existencia independiente. |
| Animal — HealthRecord | Composición | Animal 1 por HealthRecord; HealthRecord 0..* por Animal. Rombo lleno en Animal; propiedad exclusiva de entradas. |
| Interview — Animal | Asociación | Animal 1 por Interview; Interview 0..* por Animal. Referencia al animal entrevistado. |
| Interview — Adopter | Asociación | Adopter 1 por Interview; Interview 0..* por Adopter. Referencia a la persona entrevistada. |
| Adoption — Animal | Asociación | Animal 1 por Adoption; Adoption 0..* por Animal. Historial de adopciones con restricción de una activa. |
| Adoption — Adopter | Asociación | Adopter 1 por Adoption; Adoption 0..* por Adopter. Un adoptante puede participar en varias adopciones. |
| Adoption — FollowUp | Composición | Adoption 1 por FollowUp; FollowUp 0..* por Adoption. Propiedad exclusiva del seguimiento. |
| Dog / Cat → Animal | Generalización | Triángulo vacío apunta a Animal. Ambas especializaciones heredan identidad/historial y concretan GetCareGuidelines(). No lleva multiplicidades. |

## Encapsulación y extensión

Atributos privados (-); operaciones públicas (+). Animal incluye una operación protegida (#) FormatCareGuidelines(), reutilizada por sus especializaciones. Animal y su contrato GetCareGuidelines() son abstractos; Dog y Cat lo redefinen. No se introduce herencia entre personas para cumplir una cantidad: no la requiere el caso.

Voluntarios aparecen como ejemplo en 1A, pero no se les asigna un proceso obligatorio en el contexto. Se omiten para evitar responsabilidades inventadas. La solución puede añadirlos posteriormente si se define su función.

## Valor y revisión transversal

El diseño aborda pérdidas y desorganización mediante información vinculada y seguimiento explícito. La extensión por especies permite reutilizar el núcleo; conservar historia evita reconstruir registros. Es una propuesta de valor para una organización sin fines de lucro, no evidencia de innovación comercial ni de ahorro ambiental medido. Los diagramas digitales y fuentes editables evidencian uso de herramientas; el registro de supuestos y revisión evidencia orden y responsabilidad. No se afirma colaboración grupal que no haya ocurrido.
