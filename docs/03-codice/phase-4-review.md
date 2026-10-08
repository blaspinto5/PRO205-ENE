# Cierre de fase 4 — Códice

Fecha: 6 de octubre de 2026. Resultado: requisitos y diseño revisados internamente contra Sobresaliente de 2.16 y 2.17. No sustituye evaluación docente.

## Evidencia

- [Catálogo formal](requirements/requirements-catalog.md): doce funcionales, incluyendo cuatro operaciones de soporte necesarias, y cuatro no funcionales. Nombre, tipo, descripción y prioridad completos.
- [Reglas adoptadas](requirements/business-rules.md) y [registro de decisiones](requirements/open-decisions.md): D01–D06 resueltas por autorización del usuario, con supuestos separados de reglas institucionales.
- [UML PDF](../../output/pdf/codice-final-uml.pdf) y [fuente Draw.io](uml/codice-final.drawio).
- [Fundamento del modelo](uml/model-justification.md): responsabilidades, relaciones, multiplicidades y colaboración entre capas.
- [Trazabilidad consolidada](requirements/design-traceability.md): requisitos, clases, casos de uso, pruebas y pantallas previstas.

## Revisión 2.16

Se supera el mínimo de seis requisitos, cuatro funcionales. Se mantiene clasificación y plantilla completa. Cuatro funciones de soporte permiten definir cursos, matrícula, sesiones y evaluaciones sin introducir admisión, roles o regulación académica completa. Las reglas aclaran duplicidad, rango, pendientes, informes y preservación de relaciones. No se afirma entrevista ni validación docente.

## Revisión 2.17

Nueve clases, cada una con atributos y métodos. AcademicRecord abstracta concentra identidad, matrícula y modificación de registros; Grade y AttendanceRecord especializan comportamiento. Asociación, agregación y composición tienen fundamento escrito, extremos correctos y multiplicidades; generalización apunta al tipo base. Nombres en inglés; visibilidad privada/pública/protegida y tipos consistentes. El modelo cubre los doce requisitos funcionales mediante entidades y casos de uso trazados.

Se renderizó el PDF y revisó visualmente: texto legible, compartimentos completos y conexiones fuera de las clases. El editable es XML nativo Draw.io; el PDF se generó digitalmente desde la misma definición mediante ReportLab, sin afirmar exportación desde una sesión de Draw.io.

## Límites y siguiente fase

No hay aplicación, pruebas ejecutadas ni persistencia implementada. Los criterios 2.18/2.19 permanecen pendientes. Las pruebas/pantallas en trazabilidad son planes. Las garantías de integridad y calidad se demostrarán al implementar y probar.

**Fase 4 cerrada:** no se identifican brechas de diseño en esta revisión. Siguiente fase: comprobar entorno y construir Domain/Application con xUnit, manteniendo las reglas fuera de WPF.

## Apoyo posterior

[Defensa oral de fase 4](uml/oral-defense.md) preparada. [Plan de fase 5](phase-5-plan.md) con orden de desarrollo, cobertura de pruebas y entorno comprobado; falta SDK .NET 10 antes de ejecutarla.
