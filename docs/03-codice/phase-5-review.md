# Revisión y cierre de fase 5 — Códice

Fecha: 6 de octubre de 2026. **Completada mediante revisión interna de C.E. 2.18**, sujeta a evaluación docente.

## Resultado implementado

La solución `Codice.sln` contiene Domain, Application y UnitTests. Domain protege identidades, notas, matrículas, evaluaciones y asistencia; Application coordina registros, modificaciones e informes mediante tres contratos de repositorio. No depende de WPF ni de una base de datos concreta.

Se instaló SDK **10.0.401** en `%LOCALAPPDATA%/Codice/dotnet`, sin modificar permanentemente PATH ni reemplazar los SDK existentes. `global.json` fija esa versión. Instalación mediante el [script oficial de Microsoft](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script). La compatibilidad del IDE y el proyecto WPF se verificarán en fase 6.

## Validación y evidencia

- Compilación Release: **0 errores y 0 advertencias**.
- xUnit: **23 métodos, 41 casos ejecutados; 41 aprobados, 0 fallidos y 0 omitidos**.
- Pruebas independientes con repositorios en memoria y reloj fijo; casos positivos y negativos, nombres descriptivos y comprobaciones específicas de resultados y estado.
- Repetición: `./tools/test.ps1 -BuildOnly` y `./tools/test.ps1` desde la raíz.

Evidencia: [compilación](test-evidence/build-output.txt), [salida de pruebas](test-evidence/test-output.txt), [TRX original](test-evidence/results/unit-tests.trx), [informe legible](test-evidence/test-report.html) y [captura](test-evidence/test-results.png). El informe y la captura representan los resultados del TRX real; la captura corresponde al informe visual, no a una terminal simulada.

La meta inicial de ocho a doce métodos se amplió a 23 para cubrir reglas concretas: duplicados, fechas, vínculos entre cursos, correcciones sin cambios parciales ante errores, redondeo, informes pendientes y horarios locales inválidos. No se añadieron pruebas solo para aumentar la cantidad.

## Correspondencia y límites

Material de exposición: [defensa oral de fase 5](phase-5-oral-defense.md), con guion, respuestas y recorrido de demostración.

Consultar [trazabilidad de implementación](requirements/implementation-traceability.md) y [ajustes del modelo](uml/implementation-adjustments.md). El UML actualizado está en `output/pdf/codice-implemented-uml.pdf` y su editable en [Draw.io](uml/codice-implemented.drawio); se conserva el diagrama conceptual de fase 4.

Se verificaron reglas de dominio y coordinación relevantes de BR01–BR18. Las pruebas no acreditan índices, transacciones, concurrencia ni recuperación ante fallos de SQLite. BR19–BR20 y la persistencia efectiva corresponden a fase 6. Los contratos requieren cargar el contexto completo del curso; su eficiencia deberá revisarse al integrar persistencia.

**Cierre:** el motor y su evidencia satisfacen internamente las condiciones de fase 5. La aplicación de escritorio, persistencia, manual y entrega ejecutable continúan pendientes en fase 6.
