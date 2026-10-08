# Inicio de fase 4 — Códice: requisitos y UML

**Estado:** iniciada, sin cierre. **Fecha:** 6 de octubre de 2026.

## Propósito y fuentes

Transformar el caso del Instituto Códice en requisitos formales y, después, un modelo UML trazable. Fuentes: tarea 3, subtareas 3A/3B del [enunciado registrado](../00-planning/assignment-analysis.md) y criterios 2.16/2.17 de la [matriz de rúbrica](../00-planning/rubric-matrix.md).

El contexto exige una aplicación Windows C#/.NET para docentes, con registro/edición de estudiantes y asignaturas, asistencia por fecha, calificaciones e informes por estudiante y curso. Mantener el stack y las capas definidos; no producir código en esta fase.

## Orden de trabajo

1. **Delimitar alcance:** registrar las funciones expresas y separar decisiones propuestas.
2. **Especificar requisitos:** completar ID, nombre, tipo, descripción y prioridad; añadir origen y criterio de aceptación para que puedan revisarse.
3. **Resolver reglas que afectan al modelo:** notas, asistencia, matrícula, cursos e informes; documentar respuestas o supuestos adoptados explícitamente.
4. **Diseñar UML:** definir responsabilidades, atributos, operaciones y relaciones a partir de los requisitos. No introducir herencia sin fundamento.
5. **Completar trazabilidad de diseño:** vincular requisitos con clases y casos de uso; mantener pruebas/pantallas como previstas hasta implementarlas.
6. **Revisar 2.16 y 2.17:** guardar exportación UML como imagen/PDF y evidencia antes de cerrar.

## Base preparada

- [Catálogo inicial de requisitos](requirements/requirements-catalog.md): ocho funcionales derivados de las funciones expresas y cuatro no funcionales propuestos.
- [Registro de decisiones pendientes](requirements/open-decisions.md): reglas no especificadas y sus impactos.
- [Trazabilidad preliminar](requirements/traceability-matrix.md): candidatos y escenarios, todavía no diseño aprobado ni pruebas ejecutadas.

## Cierre esperado

**2.16:** seis o más requisitos, mínimo cuatro funcionales, clasificación correcta y campos completos; redacción clara y técnica. La meta interna es ocho funcionales y cuatro no funcionales, sin inventar funcionalidades para llegar a una cantidad.

**2.17:** cinco o más clases con atributos y métodos; asociación, agregación y herencia correctamente representadas y justificadas; UML estándar que refleje requisitos. La jerarquía se definirá después del análisis: no se añade una clase docente o una clase base genérica solo por cumplir notación.

La fase no se cierra con este documento: quedan decisiones de negocio, UML final y revisión de cobertura. Antes de implementar, verificar que las reglas dependientes del negocio estén definidas y trazadas.

## Límites de alcance inicial

No se incorporan automáticamente autenticación, roles, nube, exportación de informes, eliminación de datos, ponderaciones, cálculo de promedio ni respaldo automático. El promedio figura como ejemplo de prueba en el enunciado, por lo que su inclusión requiere una decisión expresa. Persistencia local, validación y estructura modular son propuestas del proyecto que deben vincularse a necesidades concretas.

**Siguiente trabajo:** revisar las decisiones pendientes y consolidar la especificación; después elaborar el UML de Códice.

## Actualización de decisiones

El usuario autorizó escoger reglas. D01–D06 resueltas en [business-rules.md](requirements/business-rules.md); catálogo actualizado con cuatro operaciones de soporte necesarias para los vínculos académicos. Siguiente paso: UML y trazabilidad final. Fase 4 abierta.

## Cierre posterior

Se ejecutó la planificación. UML y trazabilidad completos; ver [phase-4-review.md](phase-4-review.md). La fase 4 está cerrada internamente; fases 5/6 pendientes.
