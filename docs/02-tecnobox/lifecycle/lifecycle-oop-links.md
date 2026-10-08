# Ciclo de vida y prácticas POO

## 1. REQUISITOS Y ANÁLISIS

ABSTRACCIÓN — Práctica: Separar necesidades del inventario de detalles de pantalla. Ejemplo: Identificar productos, movimientos y consulta de existencias. Aporte: Enfocar el modelo; aclarar unidades y reglas pendientes.

## 2. DISEÑO

ENCAPSULAMIENTO + MODULARIDAD — Práctica: Ubicar reglas en objetos y separar responsabilidades. Ejemplo: Diseñar operaciones de Inventory y contrato de almacenamiento. Aporte: Proteger invariantes y reducir acoplamiento entre módulos.

## 3. IMPLEMENTACIÓN

REUTILIZACIÓN + CONTRATOS — Práctica: Implementar casos de uso comunes y contratos coherentes. Ejemplo: Reutilizar una operación de movimiento en distintas pantallas. Aporte: Evitar lógica duplicada; polimorfismo si el contrato lo requiere.

## 4. PRUEBAS

AISLAMIENTO + ENCAPSULAMIENTO — Práctica: Probar objetos y casos de uso con datos controlados. Ejemplo: Validar entrada/salida según reglas acordadas; sustituir persistencia. Aporte: Detectar inconsistencias y ejecutar regresión por incremento.

## 5. DESPLIEGUE

MODULARIDAD + CONFIGURACIÓN — Práctica: Integrar módulos manteniendo sus responsabilidades. Ejemplo: Configurar almacenamiento sin mover reglas a la interfaz. Aporte: Facilitar integración; POO no sustituye empaquetado ni configuración.

## 6. MANTENIMIENTO

EXTENSIÓN + REUTILIZACIÓN — Práctica: Revisar impacto y conservar contratos cuando sea posible. Ejemplo: Analizar devoluciones si se solicitan y ajustar operaciones afectadas. Aporte: Limitar propagación de cambios; reutilizar pruebas y operaciones.
