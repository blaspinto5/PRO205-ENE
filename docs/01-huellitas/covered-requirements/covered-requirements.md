# Requerimientos cubiertos

## H-RF01 — Registro de animales

Foundation.RegisterAnimal() agrupa Animal; Dog y Cat representan perros y gatos y sus cuidados.

## H-RF02 — Fichas de adoptantes

Adopter conserva identificación y contacto; UpdateContact() actualiza la ficha.

## H-RF03 — Historial de salud

Animal.AddHealthRecord()/GetHealthHistory() y HealthRecord vinculan entradas fechadas a un animal.

## H-RF04 — Entrevistas

Interview referencia Animal y Adopter; Reschedule() y RecordOutcome() representan agenda y resultado.

## H-RF05 — Seguimiento post-adopción

Adoption vincula animal/adoptante y contiene FollowUp; AddFollowUp(), Reschedule() y Complete() representan el seguimiento.
