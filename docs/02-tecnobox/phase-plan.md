# Preparación de fase 3 — Tecnobox

Fecha: 6 de octubre de 2026. Estado actualizado: ejecutada y cerrada; ver [phase-review.md](phase-review.md). Este documento conserva la preparación previa. Se prepara después de documentar la defensa de Huellitas.

## Propósito y alcance

Tecnobox vende productos tecnológicos por internet y necesita controlar entradas, salidas y stock. El proyecto está en una etapa temprana. Esta fase explica cómo el diseño y la programación OO se integran al ciclo de vida; no pide implementar el inventario ni reutilizar las clases de Huellitas.

Fuentes: [registro del enunciado](../00-planning/assignment-analysis.md), tarea 2 y subtareas 2A/2B, y [matriz de rúbrica](../00-planning/rubric-matrix.md), criterios 2.3 y 2.4, con 9 % cada uno. El límite de páginas se comprobará en los documentos finales renderizados.

## Decisión metodológica propuesta

Se utilizará un modelo incremental con iteraciones de revisión: permite precisar el diseño a medida que se aclaran los datos y validar primero partes pequeñas del inventario. No se atribuye esta decisión a Tecnobox; el enunciado permite elegir un modelo.

Los posibles incrementos son registrar productos/stock inicial, registrar entradas y luego salidas/consulta de stock. Son una secuencia explicativa, no una implementación comprometida. Antes de consolidar el diseño se deben aclarar unidades, ajustes, devoluciones y posibilidad de stock negativo; este documento no inventa sus reglas.

El ciclo general se representará como requisitos/análisis → diseño → implementación → pruebas → despliegue → mantenimiento, con retroalimentación hacia análisis y diseño. En un modelo incremental estas actividades se repiten: las flechas no representan una cascada estricta ni pruebas pospuestas hasta el final.

## Orden de trabajo y propósito

| Paso | Trabajo | Propósito y resultado |
|---|---|---|
| 1 | Delimitar necesidades expresas y preguntas pendientes. | Usar ejemplos de entradas, salidas y stock sin presentarlos como reglas confirmadas. |
| 2 | Desarrollar 2A: etapas de diseño OO. | Construir un cuadro con ubicación, objetivos, actividades y productos; acompañarlo de una explicación de máximo una página. |
| 3 | Desarrollar 2B: relación POO–ciclo de vida. | Vincular cada fase general con una práctica OO, ejemplo Tecnobox y beneficio; diseñar un mapa o infografía. |
| 4 | Redactar comentario argumentado. | Explicar calidad y mantenibilidad con relaciones causa–efecto, en máximo media página. |
| 5 | Renderizar y revisar entregables. | Comprobar extensión, legibilidad, ubicación de etapas y cobertura de 2.3/2.4. |
| 6 | Registrar evidencia y revisar cierre. | Actualizar checklist y matriz únicamente cuando los productos estén completos. |

## Subtarea 2A — estructura prevista del cuadro

Se distinguirán etapas del diseño OO de las fases generales del ciclo. La siguiente descomposición de cinco etapas es una organización metodológica propuesta; el enunciado no impone sus nombres.

| Etapa de diseño OO | Ubicación en el ciclo | Objetivo | Actividades para Tecnobox | Producto previsto |
|---|---|---|---|---|
| Identificación de responsabilidades | Transición de análisis a diseño conceptual. | Traducir necesidades en responsabilidades sin decidir aún detalles técnicos. | Examinar registrar entrada, registrar salida y consultar stock; distinguir datos, acciones y preguntas de negocio. | Lista de responsabilidades y conceptos candidatos. |
| Modelado de clases y relaciones | Diseño conceptual. | Definir qué objetos representan esas responsabilidades y cómo se relacionan. | Evaluar Product, Inventory y StockMovement como candidatos; justificar vínculos sin añadir herencia artificial. | Modelo conceptual o boceto de clases. |
| Diseño de comportamiento y colaboraciones | Diseño lógico. | Explicar cómo cooperan los objetos para realizar un caso de uso. | Recorrer una entrada/salida; ubicar cambios de stock y condiciones que deben confirmarse. | Secuencia de interacción y contratos de operaciones. |
| Diseño de interfaces y módulos | Diseño lógico y arquitectónico. | Separar responsabilidades y definir puntos de interacción. | Distinguir presentación, operaciones de inventario y almacenamiento; estudiar un contrato de persistencia si aporta valor. | Responsabilidades de módulos y especificaciones de interfaces. |
| Refinamiento y validación del diseño | Diseño detallado y revisión de cada incremento. | Comprobar coherencia antes de implementar y ajustar con retroalimentación. | Revisar trazabilidad, visibilidad, duplicación, invariantes acordadas y ejemplos de entrada/salida. | Diseño revisado y escenarios de prueba previstos. |

Los candidatos y artefactos ejemplifican la explicación; no se requiere producir aquí un sistema completo ni un UML de inventario con un mínimo inventado de clases.

## Subtarea 2B — contenido previsto del mapa

| Fase general | Práctica OO | Ejemplo contextual previsto | Beneficio que deberá argumentarse |
|---|---|---|---|
| Requisitos y análisis | Abstracción. | Distinguir producto, movimiento y necesidad de consultar existencias de detalles de pantalla. | Modelo enfocado en el problema y alcance comprensible. |
| Diseño | Encapsulamiento y modularidad. | Ubicar reglas de stock en una responsabilidad y separar almacenamiento. | Menor acoplamiento y protección de invariantes. |
| Implementación | Reutilización y polimorfismo cuando exista un contrato útil. | Consumir un contrato de almacenamiento con implementaciones intercambiables, si el diseño lo justifica. | Reutilizar casos de uso sin atarlos a detalles técnicos. |
| Pruebas | Aislamiento de objetos y contratos. | Verificar movimientos y reglas acordadas con datos controlados y sustitutos de persistencia. | Detectar errores de negocio con resultados repetibles. |
| Despliegue | Separación de módulos y configuración. | Configurar el almacenamiento sin trasladar reglas de inventario a la interfaz. | Facilitar integración; POO contribuye pero no sustituye empaquetado y configuración. |
| Mantenimiento | Encapsulamiento y extensión modular. | Incorporar un proceso de devoluciones si se solicita, revisando reglas e impactos. | Reducir propagación de cambios y ejecutar pruebas de regresión. |

El mapa incluirá flechas de retroalimentación y conexiones explícitas entre modularidad, encapsulamiento, reutilización y las etapas pertinentes. No se usará una lista de palabras aisladas como sustituto de relaciones explicadas.

## Entregables y evidencia previstos

| Ubicación prevista | Entregable | Verificación de Sobresaliente |
|---|---|---|
| `oop-design/` | Cuadro comparativo de etapas OO y explicación escrita. | C.E. 2.3: todas las etapas clave, al menos cuatro, bien ubicadas, con objetivos, actividades y productos; descripción de máximo una página. |
| `lifecycle/` | Esquema contextual del ciclo y relaciones con diseño OO. | Ubicación consistente con el modelo incremental y con el cuadro; apoyo de trazabilidad. |
| `infographic/` | Mapa conceptual o infografía editable y exportación legible. | C.E. 2.4: ciclo completo, vínculos directos con modularidad, encapsulamiento y reutilización. |
| `technical-commentary/` | Comentario de máximo media página. | Argumentación precisa de calidad y mantenibilidad mediante ejemplos de inventario. |
| `phase-review.md` | Revisión final y enlaces a evidencia. | Ambos descriptores verificados antes de cerrar la fase. |

No se crean productos vacíos ni se marcan criterios como cumplidos por disponer de esta planificación. Los nombres finales de archivos se fijarán al producirlos.

## Criterios transversales y límites

Relacionar la propuesta con una gestión ordenada de existencias y uso razonable de herramientas. El control de inventario puede ayudar a gestionar recursos, pero no se afirmarán reducciones de desperdicio o beneficios económicos medidos. Registrar decisiones, revisión y correcciones; no atribuir colaboración que no haya ocurrido.

**Próximo paso de ejecución:** elaborar el cuadro 2A y su explicación; después diseñar el mapa 2B y el comentario. Esta secuencia se ejecutó; los entregables finales y su revisión están en `phase-review.md`.
