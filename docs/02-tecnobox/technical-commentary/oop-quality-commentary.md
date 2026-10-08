# POO y calidad del software

## POO, calidad y mantenibilidad

En Tecnobox, el encapsulamiento permite concentrar la actualización de existencias en operaciones de inventario que protejan las reglas acordadas, en vez de permitir cambios dispersos desde pantallas. Esto reduce inconsistencias; su eficacia debe comprobarse con pruebas de entradas y salidas.

La modularidad separa presentación, negocio y almacenamiento. Si cambia el mecanismo de persistencia, los casos de uso pueden mantenerse cuando el contrato se conserva. Reutilizar esas operaciones para diferentes pantallas reduce duplicación y evita versiones divergentes de una misma regla. Un contrato de almacenamiento también permite sustitutos controlados para pruebas, sin exigir una base real.

En mantenimiento, una devolución solicitada se analiza como cambio de negocio, se ajustan las responsabilidades afectadas y se ejecutan pruebas de regresión. Así, POO puede limitar la propagación de cambios y facilitar diagnóstico. No garantiza calidad por sí sola: necesita requisitos claros, contratos coherentes, pruebas y revisión. No se fuerza herencia donde una responsabilidad o interfaz resulta suficiente.
