# Cierre de fase 6 — Códice

Fecha: 6 de octubre de 2026. **Fase 6 completada mediante revisión interna para el alcance académico local.** Calificación sujeta al docente. Publicación portable verificada en este equipo; comprobación en otro Windows limpio no realizada.

## Resultado entregable

Aplicación C#/.NET 10 para Windows x64 con WPF/MVVM y SQLite/EF Core. Implementa los ocho requisitos funcionales expresos y cuatro operaciones de soporte del catálogo. No incorpora usuarios, roles ni vista administrativa, conforme al alcance acordado.

Siete pantallas: Inicio, Estudiantes, Asignaturas, Cursos, Calificaciones, Asistencia e Informes. Cursos incluye pestañas para matrículas, evaluaciones y sesiones. Permite registrar/editar nombres, notas y asistencia y consultar informes por curso o estudiante, con filtro opcional de curso y corte de asistencia.

`App.xaml` carga dos diccionarios de estilos y las plantillas de vistas. Tokens centraliza colores/fuente/medidas; Controls define controles, foco, selección, estados y avisos. Sin React, Tailwind, motor web ni paquetes de tema adicionales. Pantallas con scroll cuando su contenido excede el tamaño de ventana; textos largos de tablas/selecciones se abrevian visualmente.

## Correspondencia con Sobresaliente de 2.19

| Exigencia | Evidencia interna |
|---|---|
| Tres o más funciones implementadas | Doce funciones conectadas a pantallas y servicios; [trazabilidad final](requirements/implementation-traceability.md). |
| Clases POO bien estructuradas | Nueve entidades, invariantes, herencia/polimorfismo, cuatro capas y DTOs; Views/ViewModels sin DbContext ni reglas de notas. |
| Interfaz funcional y clara | Comandos reales de ViewModels probados sobre SQLite; siete vistas, dos pestañas adicionales, ambos informes y aviso de error revisados mediante doce capturas WPF. |
| Documentación completa y comprensible | Manual técnico/uso PDF de dos páginas, fuente editable, README, arquitectura y registro de revisión. Describe clases, pantallas, funciones y flujo. |

Esta revisión compara productos con el descriptor; no inventa una nota ni reemplaza evaluación docente.

## Compilación, pruebas e interfaz

- Compilación Release: **cero errores y advertencias**.
- **55 casos aprobados, cero fallidos y cero omitidos:** 41 unitarios, 8 de integración SQLite y 6 de comandos/ViewModels Desktop.
- El recorrido Desktop crea/edita estudiante y asignatura, crea curso/matrícula/evaluaciones/sesión, registra/corrige nota y asistencia, verifica rechazo sin cambiar la nota y consulta ambos informes/filtro.
- Las pruebas negativas incluyen campos obligatorios, horario mal formado y sesión futura. Las existentes verifican duplicados, fechas, redondeo, pendientes y relaciones.
- Doce renders de controles WPF reales con datos ficticios en copia aislada; **cero errores de binding**. Revisión visual a tamaño normal y ventana mínima 1100×740. No se presentan como capturas del escritorio completo ni como clics manuales.

Fuentes: [salida de verificación](test-evidence/current/verification-output.txt), [unitarias](test-evidence/current/unit-tests.trx), [SQLite](test-evidence/current/integration-tests.trx), [Desktop](test-evidence/current/desktop-tests.trx), [errores de binding](test-evidence/current/ui/binding-errors.txt) y [galería de evidencia](test-evidence/current/README.md).

## Persistencia y portabilidad

Migración inicial versionada; apertura repetible, IDs/vínculos recuperados, décimas enteras, claves foráneas, índices/checks y guardado transaccional. Workspace serializa operaciones de pantalla. Se comprobó restauración de una base ficticia cerrada con `integrity_check = ok` y conservación de correcciones.

Publicación autocontenida `win-x64`, con .NET y Windows Desktop 10.0.12 y SQLite nativo. Paquete limpio, sin SDK, tests ni base de trabajo; incluye Manual.pdf, LEEME, manifiesto y SHA-256. La aplicación crea datos/codice.db junto al ejecutable.

Se verificaron hashes, inicio con PATH/DOTNET_ROOT sin runtime compartido, carga de coreclr desde la propia carpeta, ruta con acentos/espacios, directorio de trabajo distinto y arranque después de trasladar la copia cerrada. Una segunda instancia y falta de permiso de escritura devolvieron rechazo controlado; permisos alterados únicamente en copia desechable y restaurados al terminar.

[Registro portable](test-evidence/current/portable-startup.json) y [salida](test-evidence/current/portable-verification-output.txt). Se conserva un único paquete descomprimido actual en entrega; las versiones de preparación están archivadas bajo .tools. Se incluyen ZIP separado de aplicación y ZIP de fuentes/documentos/evidencias.

## Revisión transversal y del conjunto

| Criterio | Evidencia pertinente |
|---|---|
| Innovación, emprendimiento y sostenibilidad | Solución aplicable a registro académico local, distingue pendientes de resultados y evita reprocesos por duplicados; no necesita servidor. Capas permiten ampliar; no se inventan métricas ambientales ni validación comercial. |
| Habilidades digitales | Modelado editable/exportado, desarrollo C#, EF/SQLite, xUnit, estilos compartidos, generación de PDF, hashes y publicación autocontenida aplicados a objetivos concretos. |
| Habilidades personales y sociales | Decisiones y consultas al usuario registradas, secuencia por fases, defensas, cambios fundamentados, correcciones y límites comunicados. No se afirma haber entrevistado docentes ni realizado trabajo grupal inexistente. |

Los ocho criterios académicos tienen productos/evidencia en cierres de fases 2–6. Los tres transversales tienen evidencia de proceso para revisión interna. La percepción de habilidades y el nivel final corresponden al evaluador y a la defensa del estudiante.

## Límites prácticos

No se ha probado otro equipo limpio ni todas las versiones/DPI/configuraciones de Windows. No se certifican accesibilidad completa, producción con datos reales, concurrencia multiusuario, cifrado, auditoría histórica de todos los valores ni migraciones entre versiones diferentes del esquema. Las fechas de modificación reflejan el último cambio; no constituyen un registro completo de revisiones.

Estos límites no dejan pendientes los productos académicos definidos. Antes de usar datos reales o cambiar el alcance debe revisarse protección de datos y operación institucional. Las futuras actualizaciones deben probar compatibilidad del esquema y preservar una copia previa.

## Ubicación de entrega

Ver [README principal](../../README.md): enlaces al paquete actual y ZIPs. Manual en `output/pdf/codice-technical-manual.pdf`; copia dentro de la publicación. La defensa de fase 6 puede prepararse a partir de este cierre y de los recorridos reales.
