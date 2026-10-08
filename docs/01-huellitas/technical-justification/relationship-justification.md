# Justificación de relaciones

## Agregación: Foundation - Animal

La fundación agrupa animales en su catálogo. Un animal conserva identidad e historial si todavía no pertenece a la fundación o si es trasladado. Se usa rombo vacío en Foundation: una fundación reúne 0..* animales y cada animal pertenece a 0..1 fundaciones en este modelo. Retirarlo de la agrupación no lo destruye.

## Composición: Animal - HealthRecord y Adoption - FollowUp

Cada entrada de salud pertenece exclusivamente a un animal, y cada seguimiento a una adopción. Los rombos llenos se ubican en Animal y Adoption. Cada todo contiene 0..* partes; cada parte tiene exactamente un propietario. La composición expresa dependencia de identidad y contexto, no una orden de eliminar datos: el historial se conserva y las partes no se transfieren entre propietarios.

## Asociación: Interview y Adoption con Animal y Adopter

Una entrevista referencia un animal y un adoptante; ambos pueden participar en 0..* entrevistas. Una adopción también vincula exactamente un animal y un adoptante, sin convertirlos en partes poseídas. Un adoptante puede registrar varias adopciones. El animal conserva 0..* adopciones históricas, con la restricción propuesta de una sola activa a la vez. Estas asociaciones permiten agendar entrevistas y vincular el seguimiento con el proceso correcto.

## Generalización y especialización: Animal, Dog y Cat

Animal es abstracta y concentra identificación, fecha de rescate e historial. Dog y Cat son tipos de Animal: heredan ese comportamiento y redefinen GetCareGuidelines(), según tamaño o condición de cuidado interior. El triángulo vacío apunta a Animal. La operación protegida FormatCareGuidelines() facilita reutilización; los atributos privados y métodos públicos mantienen encapsulación.

## Fundamento y supuestos

Las nueve clases cubren los cinco procesos del caso sin añadir un sistema de voluntariado no solicitado. Propiedad exclusiva de registros, pertenencia a una fundación y adopción activa única son decisiones explícitas del diseño, no reglas textuales del enunciado. Se documentan en el análisis para permitir revisión y extensión sin perder coherencia.
