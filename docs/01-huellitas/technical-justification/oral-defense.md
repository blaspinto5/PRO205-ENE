# Defensa de la fase 2 — Fundación Huellitas

Guía de estudio y exposición. Complementa los entregables 1A y 1B; no sustituye la justificación de una página ni la lista de cobertura de media página. Basada en el [análisis de dominio](../analysis/domain-analysis.md) y la [revisión de fase](../phase-review.md).

## Guion de exposición — aproximadamente cuatro minutos

### 1. Problema y propósito

La Fundación Huellitas rescata, cuida y da en adopción perros y gatos. Su gestión manual produce pérdidas de información y dificultades para seguir las adopciones. Mi objetivo en esta fase fue representar cinco procesos: registro de animales, fichas de adoptantes, historial de salud, entrevistas y seguimiento post-adopción. Elaboré un diseño conceptual; todavía no es una aplicación implementada.

### 2. Organización del modelo

El modelo tiene nueve clases con responsabilidades distintas. Foundation organiza el catálogo; Animal reúne información común y administra el historial; Dog y Cat especializan los animales. HealthRecord representa una entrada de salud, Adopter una ficha de adoptante, Interview una entrevista, Adoption una adopción y FollowUp su seguimiento.

Adoption es importante porque identifica el proceso concreto al que pertenece un seguimiento. Si relacionara un seguimiento solamente con un animal, sería más difícil distinguir a qué adopción corresponde cuando existe historia previa.

### 3. Relaciones y sus motivos

Uso agregación entre Foundation y Animal: la fundación agrupa animales, pero retirarlos del catálogo no elimina su identidad ni su historial. El rombo vacío está en Foundation.

Uso composición entre Animal y HealthRecord, y entre Adoption y FollowUp. Cada registro tiene un único propietario y no se reutiliza para otro animal o adopción. El rombo lleno está en el todo. La identidad de esas partes depende del contexto al que pertenecen. Conservamos el conjunto como historial; esto no equivale a eliminar información cuando un animal sale de la fundación.

Uso asociación para vincular Interview y Adoption con Animal y Adopter: los participantes se relacionan con esos procesos, pero no son partes que les pertenezcan.

Uso generalización desde Dog y Cat hacia Animal. Ambas especies son animales y comparten identificación e historial; cada una redefine GetCareGuidelines(). El triángulo vacío apunta al tipo general, Animal.

### 4. Encapsulación y multiplicidades

Los atributos son privados, señalados con menos; las operaciones públicas usan más. Animal también tiene una operación protegida, FormatCareGuidelines(), accesible a sus especializaciones. GetCareGuidelines() es un contrato abstracto que Dog y Cat concretan.

Las multiplicidades se leen por cada extremo. Un animal puede tener cero o muchas entradas de salud, mientras que cada entrada pertenece a un animal. Una entrevista vincula exactamente un animal y un adoptante; ambos pueden participar en varias entrevistas. Un animal puede tener varias adopciones históricas, con la restricción propuesta de una sola activa simultáneamente.

### 5. Cobertura y límites

Cada uno de los cinco procesos del caso está vinculado con clases y operaciones en la lista de cobertura. El diseño se revisó contra 1.8 y 1.9: nueve clases, tres tipos de relaciones más herencia, atributos, métodos, visibilidades, multiplicidades y justificación.

Separé los requisitos del caso de los supuestos de diseño. Por ejemplo, una adopción activa por animal y la pertenencia a una sola fundación son decisiones propuestas, no frases del enunciado. Tampoco afirmo que el UML por sí solo demuestre velocidad, seguridad o ausencia de pérdidas: eso necesitaría implementación y pruebas.

## Preguntas posibles y respuestas

| Pregunta | Respuesta defendible |
|---|---|
| ¿Por qué nueve clases si pedían cinco? | Cinco es el mínimo. Cada clase representa una responsabilidad o especialización útil para los cinco procesos. No añadí clases solo para superar una cantidad. |
| ¿Por qué Foundation–Animal es agregación y no composición? | El animal existe fuera del catálogo y puede trasladarse. Foundation lo agrupa, sin poseer su ciclo de vida. Una asociación simple también podría representar el vínculo; elegí agregación para expresar explícitamente el conjunto y documenté su significado. |
| ¿La agregación significa que el animal pertenece a varias fundaciones? | No. El rombo describe el vínculo todo–parte; la cantidad la determina la multiplicidad. Aquí se propone 0..1 fundaciones por animal. |
| ¿Por qué HealthRecord es composición? | La entrada pertenece exclusivamente al historial de un animal y su identidad se interpreta dentro de él. No se comparte ni cambia de propietario. Si el requisito exigiera expedientes clínicos autónomos compartidos entre organizaciones, reconsideraría esta relación. |
| ¿Si se destruye el todo, qué ocurre con las partes? | En el modelo compuesto las partes no continúan como objetos de dominio independientes del todo. Para conservar historia se archiva el animal o la adopción junto con sus partes, en lugar de destruir el conjunto. La retención de datos y las cascadas SQL son decisiones posteriores, no resueltas por este UML. |
| ¿Una entrevista es parte del adoptante? | No en este diseño. Es un evento que referencia al adoptante y al animal. Tiene su propia fecha y resultado; por eso se usa asociación. |
| ¿Por qué un animal tiene 0..* adopciones? | La multiplicidad representa la historia, no adopciones activas simultáneas. Una restricción adicional limita las activas a una. End() termina una adopción conservando el registro. |
| ¿Por qué Animal es abstracta? | Centraliza lo común, mientras el caso identifica perros y gatos concretos. La operación abstracta obliga a especializar las recomendaciones de cuidado. |
| ¿Dónde aparece polimorfismo? | En GetCareGuidelines(): se utiliza el contrato de Animal y el comportamiento concreto depende de Dog o Cat. El UML modela ese mecanismo; aquí no hay una ejecución que demostrar. |
| ¿Para qué sirve la operación protegida? | Permite reutilizar formato de recomendaciones dentro de la jerarquía, sin exponer esa ayuda como operación pública. Es una decisión de diseño, no un requisito textual. |
| ¿Por qué no hay Volunteer? | Se menciona como ejemplo de entidad, pero no se define un proceso obligatorio que necesite esa responsabilidad. Podría añadirse si se precisara quién gestiona entrevistas o seguimientos. |
| ¿Por qué los nombres están en inglés? | Es una convención del proyecto para mantener consistencia en identificadores. La explicación académica permanece en español. |
| ¿Formalize() ya garantiza la adopción activa única? | Es la operación a la que el diseño asigna esa regla. El UML no demuestra su implementación ni su cumplimiento; eso requeriría código y pruebas en una tarea de desarrollo. |
| ¿Con qué herramienta se hicieron los entregables? | Las fuentes editables están en XML nativo Draw.io. Los PDF se generaron con ReportLab desde la misma definición de clases y relaciones y se revisaron visualmente. No afirmo haberlos exportado desde una sesión de Draw.io. |
| ¿Qué aporta a sostenibilidad y calidad? | Propone reutilización de información, historia organizada y responsabilidades que facilitan mantenimiento. No se midieron ahorro de papel, impacto ambiental ni resultados de usuarios. |

## Recorrido práctico frente al diagrama

1. Señalar Foundation y seguir su agregación hacia Animal.
2. Mostrar Dog/Cat y leer la dirección de los triángulos hacia Animal.
3. Recorrer Animal–HealthRecord y explicar la composición y sus extremos 1 y 0..*.
4. Mostrar Adopter–Interview–Animal y explicar fecha, participantes y resultado.
5. Recorrer Adopter–Adoption–Animal y después Adoption–FollowUp.
6. Relacionar ese recorrido con H-RF01 a H-RF05 y distinguir historia de estado activo.

Ejemplo ilustrativo, no dato del caso: se registra una perra llamada Luna, se añade una atención veterinaria, se crea la ficha de una adoptante y se agenda una entrevista. Cuando se formaliza una adopción, sus seguimientos se vinculan a ese proceso concreto. El modelo permite representarlo; todavía no se ha ejecutado en una aplicación.

## Qué conviene evitar al exponer

- No afirmar que toda composición equivale a una cascada de borrado en base de datos.
- No confundir generalización con asociación ni la punta del triángulo con la clase hija.
- No presentar supuestos, campos concretos o recomendaciones de cuidado como instrucciones literales del caso.
- No afirmar que el profesor ya otorgó Sobresaliente: existe una revisión interna con evidencia.
- No memorizar definiciones sin poder señalar el ejemplo correspondiente en el UML.
