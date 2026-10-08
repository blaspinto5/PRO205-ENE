# Defensa oral — fase 3, Tecnobox

Guía de exposición y estudio, preparada el 6 de octubre de 2026. Complementa los productos de 2A y 2B; no sustituye la explicación de una página ni el comentario de media página. Basada en la [explicación del diseño](../oop-design/design-explanation.md), el [contenido de la infografía](../lifecycle/lifecycle-oop-links.md) y la [revisión de cierre](../phase-review.md).

## Guion de exposición — aproximadamente cuatro a cinco minutos

### 1. Problema y propósito

Tecnobox es una PyME que vende productos tecnológicos por internet y necesita controlar entradas, salidas y stock. El proyecto todavía no tiene un diseño detallado. Mi trabajo en esta fase fue explicar cómo organizar su diseño orientado a objetos y cómo la POO contribuye durante el ciclo de vida del software. Los entregables son un cuadro de etapas, una explicación, una infografía y un comentario técnico.

### 2. Modelo de desarrollo elegido

Propuse un modelo incremental porque permite aclarar necesidades y revisar partes pequeñas del sistema antes de ampliar su alcance. Por ejemplo, podríamos trabajar primero productos y stock inicial, después entradas y luego salidas y consulta de existencias. Esa secuencia es una propuesta explicativa, no una implementación realizada.

Cada incremento recorre análisis, diseño, implementación y pruebas, y puede dar lugar a una entrega utilizable. La información obtenida permite revisar el siguiente incremento. No estoy suponiendo que todas las reglas estén definidas desde el comienzo: unidades, devoluciones y posibilidad de stock negativo deben aclararse con el usuario.

### 3. Cuadro de etapas de diseño OO — criterio 2.3

Distinguí cinco etapas del diseño orientado a objetos.

Primero, identificar responsabilidades: comprender qué debe hacer el inventario, como registrar un movimiento y consultar existencias. Esto produce una lista de responsabilidades y preguntas, en la transición del análisis al diseño.

Segundo, modelar clases y relaciones: distribuir esas responsabilidades entre candidatos como Product, Inventory y StockMovement. El producto es un modelo conceptual, con relaciones justificadas.

Tercero, diseñar comportamiento y colaboraciones: recorrer cómo cooperan los objetos para registrar una entrada o salida. El resultado son secuencias de interacción y contratos de operaciones.

Cuarto, diseñar interfaces y módulos: separar presentación, negocio y almacenamiento y definir cómo se comunican. Aquí una interfaz es un contrato entre componentes; no significa solamente una pantalla.

Quinto, refinar y validar el diseño: comprobar cobertura de necesidades, encapsulación y coherencia antes de codificar, y revisarlo con la retroalimentación de cada incremento. Se obtiene un diseño revisado y escenarios de prueba previstos.

Estas etapas son una descomposición metodológica propuesta, no nombres obligatorios del enunciado. El cuadro indica ubicación, objetivo, actividades y producto por etapa. Tampoco deben confundirse con las fases generales del ciclo de vida.

### 4. Infografía POO y ciclo de vida — criterio 2.4

La infografía representa requisitos y análisis, diseño, implementación, pruebas, despliegue y mantenimiento. Se lee de izquierda a derecha en la fila superior, baja a pruebas y continúa de derecha a izquierda hasta mantenimiento. La flecha de retorno representa nuevos requisitos y revisión. Las pruebas acompañan el desarrollo; su ubicación gráfica no significa que se pospongan hasta terminar todo.

En análisis, la abstracción ayuda a separar necesidades del inventario de detalles de pantalla. En diseño, el encapsulamiento concentra reglas y la modularidad separa responsabilidades. En implementación, la reutilización permite compartir operaciones entre pantallas sin duplicar la lógica. En pruebas, los contratos permiten aislar dependencias y verificar casos de uso con datos controlados.

En despliegue, la separación de módulos facilita integración y configuración, aunque no reemplaza el empaquetado. En mantenimiento, podemos revisar una devolución solicitada, ajustar las responsabilidades afectadas y ejecutar pruebas de regresión.

### 5. Calidad, mantenibilidad y límites

El aporte de POO se explica mediante causas concretas: encapsular ayuda a proteger reglas; modularizar puede limitar el impacto de cambios; reutilizar reduce duplicación. Por ejemplo, una regla de actualización de existencias debería aplicarse mediante una operación coherente, no repetirse en cada botón.

Estos beneficios dependen de requisitos claros, contratos bien diseñados y pruebas. La POO por sí sola no garantiza calidad. Tampoco afirmo haber implementado el inventario, medido ahorros ni confirmado reglas que el caso no especifica.

La revisión interna verifica cinco etapas de diseño OO, el ciclo completo y vínculos con modularidad, encapsulamiento y reutilización. La explicación ocupa una página y el comentario cabe dentro de media página, respetando los límites de la tarea.

## Preguntas posibles y respuestas

| Pregunta del profesor | Respuesta defendible |
|---|---|
| ¿Qué diferencia hay entre diseño OO y ciclo de vida? | El diseño OO define responsabilidades, objetos, colaboraciones y contratos. El ciclo de vida organiza el desarrollo completo, incluyendo análisis, implementación, pruebas, despliegue y mantenimiento. El diseño se ubica dentro de ese proceso y se revisa cuando hace falta. |
| ¿Por qué elegiste un modelo incremental? | El caso está en etapa temprana y no tiene un diseño detallado. Trabajar por partes permite obtener retroalimentación y ajustar necesidades. Es una elección propuesta y permitida por el enunciado, no una instrucción de la empresa. |
| ¿Incremental es lo mismo que iterativo o ágil? | No. Incremental añade partes de funcionalidad; iterativo revisa y mejora lo desarrollado. La propuesta combina ambos. No se definieron prácticas suficientes para afirmar que se aplica Scrum u otro marco ágil específico. |
| ¿Por qué son cinco etapas de diseño? | Para cubrir identificación de responsabilidades, estructura, comportamiento, contratos y revisión. La rúbrica exige al menos cuatro etapas clave; los nombres y la descomposición son una propuesta metodológica explicada en el cuadro. |
| ¿Cuál es la diferencia entre objetivo, actividad y producto? | El objetivo dice qué se busca; la actividad, qué se hace; el producto, qué evidencia queda. Por ejemplo: definir colaboración, recorrer una salida y producir una secuencia de interacción. |
| ¿Por qué empiezas por responsabilidades y no por clases? | Primero hay que comprender qué necesita resolver el sistema. Después se decide qué objetos asumirán esas responsabilidades; así se evita crear clases sin una función concreta. |
| ¿Product, Inventory y StockMovement son clases definitivas? | No. Son candidatos para contextualizar el diseño. Su estructura y relaciones se confirmarían mediante análisis detallado si se desarrollara el sistema. |
| ¿Encapsular es poner todos los atributos privados? | Es parte de ello, pero no suficiente. También hay que controlar cómo se modifica el estado mediante operaciones que respeten reglas acordadas. Un atributo privado con cambios arbitrarios no protege una regla por sí solo. |
| ¿Cuál sería una regla de inventario? | Un ejemplo posible es validar una cantidad o impedir una salida mayor que las existencias, pero el caso no confirma esas condiciones. Se deben acordar antes de diseñar la validación. |
| ¿Qué significa una invariante? | Una condición que debe conservarse en un estado válido del objeto. Si se acordara no permitir stock negativo, esa sería una invariante; aquí se mantiene como decisión pendiente. |
| ¿Cómo ayuda la modularidad? | Al separar presentación, negocio y almacenamiento, un cambio tiene un lugar principal donde realizarse. Esto puede reducir impactos, pero exige contratos estables y revisar dependencias. |
| ¿Reutilización exige herencia? | No. Se pueden reutilizar operaciones, objetos colaboradores e interfaces. No se introduce una jerarquía si no existe una especialización útil. |
| ¿Dónde podría aparecer polimorfismo? | En un contrato de almacenamiento con implementaciones intercambiables, si se justifica. El caso de uso consume el contrato; no necesita decidir qué implementación concreta recibe. En esta fase es un ejemplo, no código ejecutado. |
| ¿Qué es un contrato de operación? | Define lo que una operación espera y ofrece: parámetros, resultado y condiciones acordadas. Permite comprender responsabilidades y preparar escenarios de prueba. |
| ¿Por qué hay pruebas después de implementación en el dibujo? | El dibujo muestra un recorrido comprensible por incremento. Los escenarios se preparan desde el diseño y las pruebas acompañan la codificación. No se propone esperar al final del proyecto para probar. |
| ¿Validar el diseño equivale a ejecutar pruebas unitarias? | No. Validar diseño revisa responsabilidades, escenarios y coherencia del modelo; una prueba unitaria ejecuta código y comprueba resultados. Aquí se planifican, no se ejecutan pruebas de inventario. |
| ¿Qué significa prueba de regresión? | Volver a ejecutar pruebas para comprobar que un cambio no rompe comportamientos que antes funcionaban. Una futura devolución, por ejemplo, debe conservar la corrección de entradas y salidas existentes. |
| ¿POO resuelve el despliegue? | No. La modularidad puede facilitar integración y configuración, pero el despliegue requiere además empaquetado, dependencias y verificación del entorno. |
| ¿Cómo demostrarías calidad si desarrollaras el sistema? | Con trazabilidad de requisitos, pruebas de operaciones, revisión de diseño y comprobaciones con usuarios. En esta tarea se argumenta el aporte; no se presentan mediciones de una aplicación inexistente. |
| ¿Qué aporta a sostenibilidad? | Un inventario ordenado puede apoyar decisiones sobre existencias y el diseño mantenible favorece uso razonable de recursos de desarrollo. No se afirma una reducción de desperdicio o impacto ambiental medido. |
| ¿Qué herramienta utilizaste? | Los PDF se generaron digitalmente con ReportLab y se renderizaron para revisar legibilidad y extensión. La infografía conserva una fuente SVG con textos y conectores editables. |

## Ejemplo para explicar sin memorizar definiciones

Supongamos, solo para ilustrar, que Tecnobox tiene diez teclados y recibe cinco. La consulta posterior mostraría quince si se acuerda que el movimiento corresponde a cinco unidades del mismo producto.

1. **Análisis:** aclarar qué producto es, en qué unidades se cuenta y qué significa registrar una entrada.
2. **Diseño:** asignar a una responsabilidad la actualización y definir cómo se registra el movimiento.
3. **Implementación:** desarrollar la operación; las pantallas consumen ese comportamiento compartido.
4. **Pruebas:** comprobar que una entrada de cinco sobre diez produce quince bajo esas condiciones y revisar errores según reglas acordadas.
5. **Despliegue:** integrar la aplicación y configurar su almacenamiento.
6. **Mantenimiento:** analizar cambios futuros y repetir pruebas para conservar el comportamiento anterior.

Este ejemplo explica la relación entre etapas; no demuestra que ya existe un sistema de Tecnobox.

## Versión breve — aproximadamente un minuto

Tecnobox necesita gestionar entradas, salidas y stock y todavía no tiene un diseño detallado. Propuse un modelo incremental para precisar y validar partes del sistema con retroalimentación.

El cuadro organiza cinco etapas del diseño OO: responsabilidades, clases y relaciones, colaboraciones, interfaces y módulos, y refinamiento. Cada una indica dónde se ubica, qué busca, qué actividades realiza y qué producto genera.

La infografía representa el ciclo completo, desde requisitos y análisis hasta mantenimiento. Relaciona encapsulamiento con protección de reglas, modularidad con menor impacto de cambios y reutilización con menos duplicación. Las pruebas acompañan el desarrollo.

Es una propuesta de diseño contextualizada, no un inventario implementado. La explicación y el comentario respetan sus límites de extensión; la revisión interna los contrasta con 2.3 y 2.4.

## Comprobación antes de exponer

- Poder señalar en el cuadro la diferencia entre ubicación, objetivo, actividad y producto.
- Poder recorrer las seis fases de la infografía y explicar la flecha de retorno.
- Explicar encapsulamiento, modularidad y reutilización con el ejemplo de inventario.
- Distinguir necesidades expresas de reglas y clases propuestas.
- No afirmar código ejecutado, resultados medidos, marco ágil aplicado ni calificación docente que no existen.
