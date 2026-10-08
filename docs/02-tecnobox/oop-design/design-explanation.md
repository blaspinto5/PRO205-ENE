# Explicación de etapas del diseño OO

## Modelo de desarrollo y contexto

Tecnobox necesita controlar entradas, salidas y stock de productos tecnológicos. Se propone un modelo incremental porque el proyecto aún no tiene un diseño detallado: cada incremento permite aclarar necesidades, revisar el modelo y validar una porción útil. Registrar productos y stock inicial, después entradas y luego salidas/consulta son una secuencia ilustrativa, no entregas ya implementadas.

## 1. Identificar responsabilidades

En la transición de análisis a diseño se identifican acciones y conceptos del inventario, sin mezclar reglas con detalles de pantalla. El producto es una lista de responsabilidades y preguntas: unidades, devoluciones y posibilidad de stock negativo deben acordarse, no suponerse.

## 2. Modelar clases y relaciones

En diseño conceptual se distribuyen las responsabilidades entre candidatos como Product, Inventory y StockMovement, y se justifican sus vínculos. El resultado es un modelo conceptual; no se añade herencia si no existe una especialización necesaria.

## 3. Diseñar comportamiento y colaboraciones

En diseño lógico se recorre cómo los objetos colaboran para registrar una entrada o salida. Se definen contratos y condiciones de las operaciones. Se producen secuencias de interacción; las reglas de validación dependerán de los acuerdos del análisis.

## 4. Diseñar interfaces y módulos

En diseño lógico y arquitectónico se separan presentación, negocio y almacenamiento. Se especifican contratos entre módulos, evaluando una interfaz de persistencia solo si resuelve una necesidad. El resultado delimita responsabilidades y dependencias.

## 5. Refinar y validar el diseño

Antes de implementar y en cada revisión incremental se comprueba que el diseño cubre las necesidades, protege invariantes acordadas y evita duplicación. Se entrega el diseño revisado con escenarios de prueba previstos. Esta validación conceptual no equivale a ejecutar pruebas unitarias.

## Ubicación y retroalimentación

Estas cinco etapas son una descomposición metodológica del diseño OO, no cinco fases generales del ciclo de vida. Se insertan entre análisis y diseño y se revisitan al implementar, probar o mantener. El ciclo incluye además despliegue; sus actividades se repiten por incremento. Las pruebas se planifican desde el diseño y acompañan la implementación.
