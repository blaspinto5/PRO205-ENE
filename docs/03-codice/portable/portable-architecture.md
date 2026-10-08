# Arquitectura portable — Códice, Windows x64

Fecha: 6 de octubre de 2026. **Implementada; fase 6 cerrada internamente.** Consultar [cierre actual](../phase-6-review.md). Los pasos siguientes describen el diseño adoptado; la lista de aceptación conserva pendientes finales. Decisión autorizada: entregar la aplicación con sus dependencias en una carpeta trasladable.

## Distribución y dependencias

Publicación Release para `win-x64`, explícitamente autocontenida. Incluye runtime .NET/Windows Desktop, bibliotecas de aplicación y dependencias nativas de SQLite resueltas por el proveedor. No requiere instalar SDK ni runtime en el computador receptor. Requiere Windows x64 compatible, permisos de escritura y los componentes del sistema operativo que .NET utilice.

Se entrega como carpeta con varios archivos. No se activa archivo único, trimming ni AOT. Esto facilita inspeccionar dependencias y evita introducir optimizaciones de tamaño antes de validar WPF y persistencia. Configuración: `src/Codice.Desktop/Properties/PublishProfiles/PortableWinX64.pubxml`.

```text
ENE_PROYECT/
  src/                 código y perfil de publicación
  tests/               unitarias e integración
  docs/                fuentes, decisiones y evidencia
  tools/               verificación y publicación
  output/pdf/          entregables documentales
  .tools/dotnet/       SDK opcional, no incluido actualmente
  entrega/
    Codice-win-x64-<fecha>-<id>/
      Codice.Desktop.exe
      <runtime, bibliotecas y recursos publicados>
      datos/           base creada en el primer inicio
        codice.db
      LEEME.txt
      Manual.pdf       incorporado en el paquete final
      publication.json
      SHA256SUMS.txt
```

El runtime forma parte de la entrega; el SDK y paquetes usados para construirla pertenecen al entorno de desarrollo. `tools/get-sdk.ps1` busca SDK dentro del proyecto, después el SDK local del usuario y finalmente el del sistema. Actualmente sigue instalado fuera del proyecto; no se ha copiado ni construido un entorno de compilación sin internet. Esa capacidad requeriría también conservar paquetes NuGet y packs de runtime.

## Capas y arranque que debe implementar fase 6

Domain/Application mantienen sus referencias actuales y no conocen rutas ni SQLite. Infrastructure implementará repositorios y mapeos; Desktop configurará sus dependencias al arrancar. Desktop será `WinExe`, `net10.0-windows`, WPF habilitado y nombre de ensamblado `Codice.Desktop`. No cambiar el target de todo el repositorio a Windows: el motor continúa siendo independiente.

Orden obligatorio del inicio:

1. Obtener raíz mediante `AppContext.BaseDirectory`, nunca el directorio de trabajo ni rutas del computador del autor.
2. Resolver `Path.Combine(root, "datos", "codice.db")` como ruta absoluta y crear datos si no existe. Construir conexión mediante SqliteConnectionStringBuilder, evitando interpolación de rutas con caracteres especiales.
3. Comprobar escritura sin tocar datos existentes; si falla, mostrar mensaje en español y detener inicio, sin guardar silenciosamente en otra carpeta.
4. Adquirir exclusión de instancia para esa base: identidad derivada de la ruta absoluta normalizada. Mantener el bloqueo durante toda la ejecución y liberarlo al cerrar. Verificar el comportamiento ante un segundo inicio.
5. Aplicar migraciones versionadas de EF Core antes de habilitar formularios. No borrar ni recrear bases existentes para actualizar. No mezclar EnsureCreated con migraciones para la misma base.
6. Configurar reloj/zona académica, servicios y repositorios; abrir la interfaz solo después del inicio correcto. Disponer conexiones/contextos al cerrar.

El arranque ya implementa estos pasos. Las comprobaciones de permisos y de interfaz en otro equipo continúan pendientes.

## Datos, integridad y traslado

Base local, claves foráneas e índices únicos coherentes con BR01–BR18; transacciones para operaciones relacionadas. EF/SQLite deberá comprobar carga completa y preservación de identidad al recuperar las entidades actuales. Registrar cualquier ajuste de constructores/mapeo sin debilitar invariantes.

Primera entrega sin bases de trabajo ni datos personales. La demostración utilizará datos ficticios creados explícitamente, sin duplicarlos en cada arranque. No marcar bases como contenido publicable del proyecto.

BR20 se conserva: respaldo manual con aplicación cerrada. Copiar toda la carpeta datos, incluidos auxiliares si existen. Verificar cierre de conexiones, traslado, restauración e integridad. No añadir backup automático. Evitar carpeta de red o sincronización simultánea para esta edición local.

Cada publicación crea una carpeta nueva; nunca publica encima de la carpeta usada por el usuario. Para actualizar: cerrar ambas ediciones, respaldar, copiar datos a la nueva, aplicar migración compatible y comprobarlos. Conservar la copia anterior: revertir una base migrada no se promete sin una estrategia específica.

## Herramientas y comprobación pendiente

`./tools/publish-portable.ps1` genera una edición nueva del Desktop ya implementado. `-IncludeManual` exige el manual final y lo incorpora. Comprueba salida ejecutable y ausencia de bases empaquetadas; genera manifiesto y hashes. No comprime automáticamente ni declara aprobada una aplicación sin recorrerla. Si falla publicación, su carpeta parcial no es distribuible.

Pruebas de aceptación antes del cierre:

- [x] Compilación, unitarias y pruebas reales de SQLite correctas.
- [x] Ejecutable y bibliotecas nativas de SQLite incluidos y cargados.
- [ ] Inicio en Windows x64 compatible sin .NET instalado, registrando versión del SO.
- [x] Ejecutar desde directorio de trabajo distinto; datos permanecen junto al ejecutable. Acceso directo manual pendiente.
- [ ] Ruta con espacios y acentos; registro, edición e informes correctos.
- [ ] Cerrar/reabrir conserva IDs, relaciones, notas y asistencia.
- [ ] Mover carpeta cerrada a otra ubicación conserva registros.
- [ ] Falta de escritura y segunda instancia generan mensajes claros sin corrupción.
- [ ] Migración, respaldo y restauración comprobados con base ficticia representativa.
- [ ] Paquete limpio, sin SDK, binarios de pruebas, bases usadas ni rutas personales.
- [ ] Manual de máximo dos páginas, evidencia y trazabilidad completos para 2.19.

Fuentes técnicas: [publicación .NET](https://learn.microsoft.com/en-us/dotnet/core/deploying/), [RID](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog), [autocontención explícita](https://learn.microsoft.com/en-us/dotnet/core/compatibility/sdk/8.0/runtimespecific-app-default) y [proveedor SQLite](https://learn.microsoft.com/en-us/ef/core/providers/sqlite/).

## Verificación de esta preparación
El 6 de octubre se comprobó sintaxis de las tres herramientas PowerShell, XML del perfil win-x64 autocontenido, selección del SDK 10.0.401 y rechazo claro de publicación mientras falte Desktop. La solución actual recompiló con cero errores y advertencias. No se ejecutó ni publicó una aplicación portable; las pruebas de aceptación anteriores permanecen pendientes.

Actualización de implementación: 49 casos aprobados y publicación autocontenida con arranque comprobado. Recuperación/traslado se probaron mediante integración; formularios y aceptación en otro equipo aún pendientes. Ver phase-6-foundation.md.

Cierre: aplicación completa, 55 casos aprobados, doce renders WPF, manual y publicación verificados. Permisos, bloqueo, rutas, traslado y hashes comprobados en este equipo. La prueba en otro Windows limpio permanece pendiente; no es parte de la evidencia realizada. Consultar phase-6-review.md para estado actual.
