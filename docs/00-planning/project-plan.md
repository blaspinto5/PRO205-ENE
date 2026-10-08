# Plan de proyecto — PRO205 ENE

## Propósito y fuentes

Desarrollar una entrega académica clara, modular, verificable y alineada con el nivel Sobresaliente (86–100 %) de la rúbrica. Este documento concreta la fase 1 del plan maestro; no reemplaza la rúbrica ni modifica sus exigencias.

Fuentes de referencia conservadas en docs/00-planning/sources: `PRO205_ENE_MASTER_PLAN.md` y `PRO205_RUBRICA_COMPLETA.md`. La segunda es una transcripción con algunos descriptores inferiores incompletos; incluye los descriptores de Sobresaliente utilizados en la planificación.

La cobertura de los once criterios, sus pesos y evidencias está en [rubric-matrix.md](rubric-matrix.md). Las decisiones técnicas están en [architecture-decisions.md](architecture-decisions.md) y las revisiones en [phase-checklists.md](phase-checklists.md).

## Alcance y secuencia

| Fase | Propósito | Resultado y condición de avance |
|---|---|---|
| 1 — Planificación y arquitectura | Preparar reglas, herramientas, organización y cobertura de evaluación. | Cuatro documentos de planificación, carpetas y repositorio local; checklist de fase revisado. |
| 2 — Huellitas | Analizar el dominio y justificar relaciones POO. | Análisis, esquema preliminar, justificación, UML final y requerimientos cubiertos; revisión de 1.8 y 1.9. |
| 3 — Tecnobox | Relacionar diseño OO con el ciclo de vida. | Esquema y tabla de etapas, explicación, infografía y comentario técnico; revisión de 2.3 y 2.4. |
| 4 — Códice: requisitos y UML | Convertir el caso en requisitos y modelo trazables. | Catálogo formal, UML y trazabilidad inicial; revisión de 2.16 y 2.17 antes de implementar. |
| 5 — Lógica y pruebas | Construir Domain y Application y verificar reglas. | Código modular y pruebas unitarias sin errores; revisión de 2.18 antes de integrar interfaz. |
| 6 — Integración y entrega | Añadir persistencia, interfaz y documentación final. | Aplicación ejecutable, manual, evidencia y trazabilidad completa; revisión de 2.19 y criterios transversales. |

No se programan fechas ni duraciones sin conocer el plazo académico. La fase siguiente comienza después de revisar la anterior.

## Insumos y límites

Actualización del 6 de octubre de 2026: los tres enunciados están disponibles y preservados en `sources/`. Su alcance y condiciones se registran en [assignment-analysis.md](assignment-analysis.md). Realizar el análisis detallado del caso al iniciar cada fase. El alcance expreso de Códice incluye estudiantes y asignaturas, asistencia por fecha, calificaciones e informes por estudiante y curso.

Los modelos y funciones citados en el plan maestro son propuestas hasta contrastarlos con sus enunciados. No se inventarán reglas de adopción, notas, asistencia, usuarios o reportes. Las escalas de notas, redondeo y condiciones de duplicidad se determinarán al analizar Códice.

La fase 1 no produce lógica de negocio, diagramas académicos finales ni una aplicación ejecutable. La solución C# y sus proyectos se crearán cuando corresponda desarrollar el código. El repositorio local no implica publicación en GitHub; aún no existe remoto configurado.

## Herramientas y arquitectura

Se mantiene el stack del plan maestro: C#, .NET 10 LTS, WPF con MVVM, SQLite, Entity Framework Core, xUnit, Git, GitHub como destino previsto, Draw.io y Visual Studio. La disponibilidad y compatibilidad del entorno de desarrollo se comprobarán antes de crear la solución C#.

Responsabilidades: Domain contiene reglas y entidades; Application contiene casos de uso y contratos; Infrastructure implementa persistencia; Desktop contiene interfaz y coordinación MVVM. Las reglas de negocio no dependen de WPF ni del motor de datos.

## Organización

- `docs/00-planning/`: planificación y revisiones.
- `docs/01-huellitas/`: análisis, UML preliminar/final, justificación y requisitos cubiertos.
- `docs/02-tecnobox/`: ciclo de vida, diseño OO, infografía y comentario.
- `docs/03-codice/`: requisitos, UML, capturas, evidencia de pruebas y manual.
- `src/Codice.Domain/`, `src/Codice.Application/`, `src/Codice.Infrastructure/`, `src/Codice.Desktop/`: ubicaciones reservadas para código.
- `tests/Codice.UnitTests/`: pruebas unitarias; `tests/Codice.IntegrationTests/`: integración cuando aporte valor.

Los archivos `.gitkeep` conservan las carpetas vacías en Git; no son entregables académicos ni proyectos compilables.

## Idioma, nombres y comentarios

Identificadores, archivos C#, carpetas técnicas y nombres de pruebas en inglés. Documentación académica e interfaz pueden estar en español.

Clases y métodos en PascalCase; variables y parámetros en camelCase; interfaces con prefijo `I`; booleanos con condición clara como `isActive`. Métodos con verbo y responsabilidad concreta, como `RecordAttendance`. Evitar `Helper`, `Utils` o `Manager` sin propósito específico.

Comentarios de producción mínimos: explicar razones técnicas o restricciones difíciles de inferir, nunca narrar instrucciones evidentes. En pruebas se permiten `// Arrange`, `// Act`, `// Assert`. No ocultar errores con bloques `catch` vacíos.

## Pruebas y trazabilidad

Se utilizará xUnit con nombres `MethodName_Scenario_ExpectedResult`, estructura Arrange / Act / Assert y asserts específicos. Las pruebas serán independientes, rápidas y sin WPF, SQLite real ni archivos reales; probarán reglas confirmadas y casos positivos y negativos. La rúbrica exige tres o más pruebas útiles para Sobresaliente; la meta interna es ocho a doce, con justificación si se ajusta.

Los requisitos tendrán ID, nombre, tipo, descripción y prioridad. La matriz de Códice seguirá `Requisito → Clase/módulo → Caso de uso/método → Prueba → Pantalla`, señalando justificadamente cuando una columna no aplique. Se inicia en fase 4 y se completa en fase 6. No confundirla con la matriz de rúbrica de fase 1.

## Estrategia Git y evidencias

Repositorio local con rama inicial `main`. Registrar cambios pequeños y coherentes mediante commits descriptivos: `docs: add Huellitas analysis`, `feat: add student registration`, `test: add grade validation tests`. Revisar el contenido antes de cada commit. Evitar binarios de compilación, bases locales y datos personales o credenciales.

Cada revisión registrará criterio, entregable, ubicación de evidencia y brecha pendiente. Las capturas complementan el código y los documentos; no los sustituyen. Mantener UML, requisitos y código consistentes cuando se implementen. La publicación o configuración de un remoto se hará cuando se determine el repositorio de destino.

## Estado

Fases 1–6 completadas mediante revisión interna. Aplicación, 55 casos aprobados, manual de dos páginas, capturas y paquetes disponibles. Cierre y límites en docs/03-codice/phase-6-review.md; calificación docente y comprobación en otro equipo limpio pendientes.
