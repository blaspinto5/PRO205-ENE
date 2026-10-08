# Cuadro de etapas del diseño OO

Modelo incremental propuesto para Tecnobox.

| Etapa de diseño OO | Ubicación en el ciclo | Objetivo | Actividades en Tecnobox | Producto |
|---|---|---|---|---|
| 1. Identificar responsabilidades | Transición análisis → diseño conceptual | Traducir necesidades a responsabilidades. | Examinar entradas, salidas y consulta de stock; separar datos, acciones y preguntas de negocio. | Lista de responsabilidades y conceptos candidatos. |
| 2. Modelar clases y relaciones | Diseño conceptual | Asignar responsabilidades y vínculos entre objetos. | Evaluar Product, Inventory y StockMovement; justificar relaciones y evitar herencia sin necesidad. | Modelo conceptual o boceto de clases. |
| 3. Diseñar comportamiento y colaboraciones | Diseño lógico | Precisar cómo cooperan los objetos en un caso de uso. | Recorrer una entrada/salida; definir contratos de operaciones y condiciones de actualización por confirmar. | Secuencia de interacción y contratos de operaciones. |
| 4. Diseñar interfaces y módulos | Diseño lógico y arquitectónico | Separar responsabilidades y puntos de interacción. | Distinguir presentación, operaciones de inventario y almacenamiento; evaluar un contrato de persistencia. | Responsabilidades de módulos y especificación de interfaces. |
| 5. Refinar y validar el diseño | Diseño detallado; revisión de cada incremento | Comprobar coherencia antes de codificar y ajustar con retroalimentación. | Revisar trazabilidad, encapsulamiento, duplicación y escenarios de entrada/salida con reglas acordadas. | Diseño revisado y escenarios de prueba previstos. |

Las etapas se revisitan por incremento; no constituyen una cascada rígida. Los ejemplos son propuestas de diseño, no un inventario implementado.
