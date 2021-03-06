# 馃 C# ASP.NET CORE 5 MULTITENANCY

[![Github][github-shield]][github-url]
[![Kofi][kofi-shield]][kofi-url]
[![LinkedIn][linkedin-shield]][linkedin-url]
[![Khanakat][khanakat-shield]][khanakat-url]

## TABLA DE CONTENIDO

* [Acerca del proyecto](#acerca-del-proyecto)
* [Instalaci贸n](#instalaci贸n)
* [Resumen te贸rico](#resumen-te贸rico)
* [Dependencias](#dependencias)
* [Licencia](#licencia)

## 馃敟 ACERCA DEL PROYECTO

Este proyecto es una muestra de una aplicaci贸n de multiple tenencia. Se utilizo ``ASP.NET Core 5`` con C#.

## 鈿欙笍 INSTALACI脫N

Clonar el repositorio.

```bash
gh repo clone FernandoCalmet/dotnet-5-aspnet-core-multitenancy
```

Crear la migraci贸n de base de datos

```bash
update-database
```

Ejecutar aplicaci贸n.

```bash
dotnet run
```

## 馃摀 RESUMEN TE脫RICO

## 驴Qu茅 Es MultiTenancy?

La tenencia m煤ltiple es un patr贸n arquitect贸nico en el que un sistema de software puede servir a m煤ltiples grupos de usuarios u organizaciones. Los productos SAAS (Software As A Service) son un excelente ejemplo de arquitectura multiusuario.

Por lo general familiarizados con la tenencia 煤nica, donde una API/Servicio que es utilizada por un solo grupo de usuarios o una sola organizaci贸n. En este escenario, se necesita la implementaci贸n de una sola aplicaci贸n para cada nuevo conjunto de usuarios u organizaciones.

Imaginemos que tenemos una aplicaci贸n que necesitan varias organizaciones o grupos de usuarios. Se sabe que la aplicaci贸n tendr谩 la misma base de c贸digo para todos los usuarios. En tal escenario, 驴es siempre inteligente implementar la misma aplicaci贸n una y otra vez para cada uno de los inquilinos? 驴No aumenta el costo y el mantenimiento de la infraestructura? 驴No es m谩s inteligente dise帽ar una aplicaci贸n de tal manera que pueda acomodar a varios grupos de usuarios en una sola implementaci贸n? Esto puede reducir dr谩sticamente el n煤mero de implementaciones y los costos del servidor.

![Diagrama](./multi-tenance.png)

Aqu铆, la primera secci贸n demuestra la aplicaci贸n de un inquilino 煤nico habitual donde se necesitan implementaciones separadas por inquilino. Inquilino se refiere al grupo de usuarios o la Organizaci贸n/Compa帽铆a que desea utilizar nuestra aplicaci贸n. Se puede ver que cada uno de los inquilinos necesitar谩 una implementaci贸n completa de la infraestructura.
Mientras que, en la segunda secci贸n, varios inquilinos comparten la implementaci贸n de una sola aplicaci贸n. Los inquilinos tambi茅n pueden optar por compartir la base de datos o tener una base de datos completamente aislada para ellos mismos.

## Estrategias De Acceso A La Base De Datos

Para que la aplicaci贸n Multitenant se comunique con la base de datos, existen esencialmente las siguientes estrategias.

### Separaci贸n de esquemas

Esta es una de las estrategias menos utilizadas en la que cada inquilino obtendr谩 su propia tabla de base de datos, separada por el nombre del esquema. Digamos que una aplicaci贸n tiene 3 inquilinos, alfa, beta y gamma. Sus tablas de productos se ver谩n as铆:

- [alpha].[Productos]
- [beta].[Productos]
- [gamma].[Productos]

De esta forma, existe una clara separaci贸n de los datos del inquilino.

### Base de datos 煤nica (separaci贸n de columnas de inquilinos)

Este es un enfoque bastante popular en el que la aplicaci贸n tiene solo una base de datos y todos los inquilinos comparten la aplicaci贸n y la base de datos. Este es un enfoque m谩s econ贸mico cuando no hay muchas operaciones de datos en la aplicaci贸n.

Cada una de las tablas tendr谩 una columna adicional `'TenantId'`, que se encargar谩 de filtrar los datos por inquilinos.

TenantId | UserName | NormalizedUserName
--- | --- | ---
dotnet | dotnet.admin | DOTNET.ADMIN
java | java.admin | JAVA.ADMIN

Cuando sus inquilinos est谩n preocupados por la seguridad de los datos y tienen mucho flujo de datos, esta puede no ser una soluci贸n ideal. Hay muchas posibilidades de que el desarrollador se olvide de filtrar el tenantId de un servicio que potencialmente tambi茅n puede exponer los datos de otros inquilinos. Adem谩s, cuando un inquilino necesita mucho espacio de datos, este enfoque no ser谩 muy ideal.

### Base de datos m煤ltiple: aislamiento completo de datos

En este enfoque, cada uno de los inquilinos puede disfrutar de una base de datos separada y, por lo tanto, completar el aislamiento y la seguridad de los datos. Esta es la soluci贸n m谩s preferida cuando se trata de multipropiedad.

```text
SQLServer  
    |  
    --- Databases  
            |  
            --- [dbo].[alpha]  
            |  
            --- [dbo].[beta]  
            |  
            --- [dbo].[gamma]  
```

### Enfoque h铆brido

Aqu铆 hay un enfoque interesante, donde dise帽amos una aplicaci贸n que puede permitir a los inquilinos elegir si necesitan una base de datos separada o compartida. Esto puede ser importante cuando sabe que no todos los inquilinos van a tener un uso elevado de la base de datos. A los inquilinos que utilizan la base de datos de forma m铆nima se les puede asignar el uso de la base de datos compartida, mientras que los inquilinos con mayor uso intensivo de datos pueden optar por tener una base de datos separada. Esto tambi茅n tiene un impacto econ贸mico positivo.

## Identificaci贸n De Inquilinos

### Cadena de consulta

Este es un mecanismo simple, en el que el inquilino puede identificarse mediante una cadena de consulta. Es tan simple como pasar ``'? TenantId = alpha'`` en la URL de la solicitud y la aplicaci贸n llega a saber que esta solicitud es para inquilinos alfa. Como esta estrategia tiene varias vulnerabilidades, se recomienda utilizar este enfoque solo para fines de prueba y desarrollo.

### Solicitar direcci贸n IP

Aqu铆, asumimos que cada solicitud de inquilino se generar谩 a partir de un rango de IP particular. Una vez que se establece un rango de IP para un inquilino, la aplicaci贸n puede detectar a qu茅 inquilino pertenece. Aunque este es un enfoque seguro, no siempre es conveniente utilizarlo.

### Encabezado de solicitud

Esta es una estrategia m谩s s贸lida para identificar a los inquilinos. Cada una de las solicitudes tendr谩 un encabezado con TenantID. Luego, la aplicaci贸n atiende la solicitud para ese inquilino en particular. Se recomienda utilizar este enfoque al solicitar tokens de autenticaci贸n 煤nicamente.

### Reclamaci贸n

Una forma m谩s segura de detectar inquilinos. En los sistemas donde los tokens JWT est谩n involucrados para la autenticaci贸n, el tenantId del usuario se puede codificar en los reclamos del token. Este enfoque garantiza que la solicitud est茅 autenticada y pertenezca a un usuario del inquilino mencionado.

## 馃摜 DEPENDENCIAS

- [Microsoft.VisualStudio.Web.CodeGeneration.Design](https://www.nuget.org/packages/Microsoft.VisualStudio.Web.CodeGeneration.Design/) : Herramienta de generaci贸n de c贸digo para ASP.NET Core. Contiene el comando dotnet-aspnet-codegenerator que se usa para generar controladores y vistas.
- [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/) : Herramientas Swagger para documentar API creadas en ASP.NET Core.

## 馃搫 LICENCIA

Este proyecto est谩 bajo la Licencia (Licencia MIT) - mire el archivo [LICENSE](LICENSE) para m谩s detalles.

## 猸愶笍 DAME UNA ESTRELLA

Si esta Implementaci贸n le result贸 煤til o la utiliz贸 en sus Proyectos, d茅le una estrella. 隆Gracias! O, si te sientes realmente generoso, [隆Apoye el proyecto con una peque帽a contribuci贸n!](https://ko-fi.com/fernandocalmet).

<!--- reference style links --->
[github-shield]: https://img.shields.io/badge/-@fernandocalmet-%23181717?style=flat-square&logo=github
[github-url]: https://github.com/fernandocalmet
[kofi-shield]: https://img.shields.io/badge/-@fernandocalmet-%231DA1F2?style=flat-square&logo=kofi&logoColor=ff5f5f
[kofi-url]: https://ko-fi.com/fernandocalmet
[linkedin-shield]: https://img.shields.io/badge/-fernandocalmet-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/fernandocalmet
[linkedin-url]: https://www.linkedin.com/in/fernandocalmet
[khanakat-shield]: https://img.shields.io/badge/khanakat.com-brightgreen?style=flat-square
[khanakat-url]: https://khanakat.com
