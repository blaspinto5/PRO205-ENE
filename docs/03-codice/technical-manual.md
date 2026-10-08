# Manual técnico y de uso — Códice

Edición académica local para Windows x64. 6 de octubre de 2026. El PDF asociado ocupa dos páginas.

## Entorno y ejecución

Extraer la carpeta completa y abrir Codice.Desktop.exe en Windows de 64 bits compatible con .NET 10. La publicación incluye .NET y Windows Desktop 10.0.12 y las bibliotecas de SQLite; no requiere instalar SDK ni .NET aparte ni internet para el uso normal. La ubicación debe permitir escritura. Utilizar datos ficticios bajo una cuenta de Windows controlada.

Los registros se crean en datos/codice.db junto al ejecutable. No mover bibliotecas por separado. Una sola instancia puede utilizar esa carpeta de datos.

## Arquitectura y clases principales

| Capa | Responsabilidad |
|---|---|
| Domain | Reglas, identidades y registros; independiente de WPF/EF. |
| Application | Servicios de estudiantes, asignaturas, cursos, notas, asistencia e informes; consultas de catálogo y pantalla. |
| Infrastructure | EF Core/SQLite, repositorios, migración inicial y rutas portables. |
| Desktop | Vistas XAML, ViewModels, comandos y composición de la aplicación. |

Student y Subject conservan códigos estables. Course representa una sección de asignatura durante un período; Enrollment vincula al estudiante y curso. Assessment y ClassSession representan evaluación y sesión. Grade y AttendanceRecord contienen resultados individuales y heredan identidad/fechas de AcademicRecord, con resúmenes especializados.

Las colecciones se consultan sin modificar directamente; el dominio valida cambios. El guardado del curso es transaccional. Los índices impiden duplicados y las claves foráneas conservan vínculos. Las notas se almacenan en décimas enteras para conservar precisión. Workspace serializa operaciones y prepara selecciones mediante DTOs; los ViewModels no usan DbContext.

## Pantallas y recorrido

1. **Inicio:** revisar conteos y cursos; Actualizar vuelve a consultar datos.
2. **Estudiantes / Asignaturas:** completar campos con *, Guardar; seleccionar una fila para editar nombres, o Nuevo para otra ficha. El código permanece estable.
3. **Cursos:** crear asignatura/sección/período e inicio/término. Seleccionar el curso y usar las pestañas Matrículas, Evaluaciones y Sesiones. Las fechas deben estar dentro del curso; las horas se ingresan como HH:mm, en Chile continental.
4. **Calificaciones:** seleccionar curso, evaluación y estudiante; ingresar nota 1,0–7,0 con máximo una décima. Registrar nota crea; Corregir nota modifica un registro existente conservando identidad y fecha de registro, actualizando su última modificación.
5. **Asistencia:** seleccionar curso, sesión y estudiante; elegir Presente/Ausente. Registrar asistencia crea; Corregir asistencia cambia el estado existente. No registrar sesiones futuras o anteriores a matrícula.
6. **Informes:** elegir Por curso o Por estudiante, selección y corte; Generar informe. En informe individual, curso opcional o Todos los cursos. Seleccionar una fila para notas y referencia ≥4,0; cambiar filtros requiere generar de nuevo.

## Interpretación, errores y respaldo

Una nota pendiente no vale cero ni uno; sin notas no hay promedio. La media simple se redondea al final a una décima y se marca provisional si hay evaluaciones pendientes. No determina aprobación institucional. Sin registrar es distinto de Ausente. El corte limita asistencia al fin de ese día o al momento actual; las notas reflejan su estado actual.

Los avisos superiores indican el resultado o motivo del rechazo. Una nota inválida conserva el valor anterior. Si falta escritura, mover la carpeta completa; si hay otra instancia, cerrarla. Conservar los datos si falla inicialización.

Para trasladar o respaldar: cerrar Códice y copiar toda la carpeta datos, incluyendo auxiliares SQLite si existen. Restaurar en una copia cerrada y comprobar informes. Para actualizar, respaldar y conservar la versión anterior; nunca publicar encima de datos de trabajo. No abrir un esquema nuevo con una edición antigua sin comprobar compatibilidad.

## Estilos, verificación y límites

App.xaml carga Styles/Tokens.xaml (colores, fuente, medidas), Styles/Controls.xaml (controles/estados) y Views/Templates.xaml (vistas). Cambiar el tema en esos recursos, no duplicar colores por pantalla.

Desde el código: tools/test.ps1 compila y ejecuta 55 casos (41 unitarios, 8 SQLite y 6 de flujos Desktop), todos aprobados. tools/publish-portable.ps1 -IncludeManual genera otra publicación. Las capturas se obtienen del render WPF con datos ficticios en una copia aislada.

Alcance: aplicación local sin login, roles ni cifrado; permisos de Windows controlan el acceso a la carpeta. No es un sistema multiusuario ni una certificación de protección de datos reales. La ejecución autocontenida se comprobó en el equipo de desarrollo; validación en otro equipo limpio pendiente.
