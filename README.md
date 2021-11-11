# CS ASPNET Core Multitenancy

Proyecto para ejemplificar la multiple tenencia en una aplicación.

## Configuración:

Migrar base de datos

```bash
update-database
```

## ¿Qué Es MultiTenancy?

La tenencia múltiple es un patrón arquitectónico en el que un sistema de software puede servir a múltiples grupos de usuarios u organizaciones. Los productos SAAS (Software As A Service) son un excelente ejemplo de arquitectura multiusuario.

Por lo general familiarizados con la tenencia única, donde una API/Servicio que es utilizada por un solo grupo de usuarios o una sola organización. En este escenario, se necesita la implementación de una sola aplicación para cada nuevo conjunto de usuarios u organizaciones.

Imaginemos que tenemos una aplicación que necesitan varias organizaciones o grupos de usuarios. Se sabe que la aplicación tendrá la misma base de código para todos los usuarios. En tal escenario, ¿es siempre inteligente implementar la misma aplicación una y otra vez para cada uno de los inquilinos? ¿No aumenta el costo y el mantenimiento de la infraestructura? ¿No es más inteligente diseñar una aplicación de tal manera que pueda acomodar a varios grupos de usuarios en una sola implementación? Esto puede reducir drásticamente el número de implementaciones y los costos del servidor.

![Diagrama](./multi-tenance.png)

Aquí, la primera sección demuestra la aplicación de un inquilino único habitual donde se necesitan implementaciones separadas por inquilino. Inquilino se refiere al grupo de usuarios o la Organización/Compañía que desea utilizar nuestra aplicación. Se puede ver que cada uno de los inquilinos necesitará una implementación completa de la infraestructura.
Mientras que, en la segunda sección, varios inquilinos comparten la implementación de una sola aplicación. Los inquilinos también pueden optar por compartir la base de datos o tener una base de datos completamente aislada para ellos mismos.

---

:octocat: [Follow me](https://github.com/FernandoCalmet)

[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/T6T41JKMI)