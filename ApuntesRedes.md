# Apuntes Redes
## Conceptos base
- LAN - Local Area Network
- WAN - Wide Area Networkç
- Paquetes - Unidades organizadas de informacion
- Ping - Tiempo en el los paquetes tardan en viajar
- TTL - Time To Leave : Cada vez que un paquete pasa por un router se le resta 1.
- IP - Direccion del equipo
- IPv4 - Direccion 32 bits usada actualmente, aunque con un limite de 4.000.000.000 combinaciones. 
- IPv6 - Direccion 128 bits.

En la actualidad se sigue usando la v4 ya que hay muchas formas de replicar estas combinaciones (por ejemplo usar redes privadas de menor tamaño en la que el router tiene 2 conexiones, publica y privada)

- Puerto - Ruta de acceso del router. Clasificacion de tipologias de acceso.
 A partir del Nº 1024 son libres, hacia abajo son protocolos establecidos.

- API: Sistema que permite a 2 aplicaciones comunicarse entre si. Ejemplo: Acceder a la API de una WEB (meteogalicia) y solicitar toda la informacion relacionada con el tiempo en galicia. Extrae un archivo de informacion estructurada (CSV,JSON, XML) que puede ser posteriormente procesado.

## Tipos de archivos extraidos de API
### CSV 
Comma Separated Values

### JSON
   JavaScript Object Notation. 
    Podemos pasar clases en C# a json para aligerar los archivos. JSON presenta identificacion de tipologia de datos integrada, y es una de las ventajas por las que este formato se mantiene más presente que el resto (?).
### XML
Informacion estructurada en árbol con etiquetas. Malditos lenguajes de marcas. 

### HTTP 
Es un protocolo que usa la estructura de XML para trabajar pero con un sistema común de etiquetas. 
- [Documentación de métodos HTTP](https://developer.mozilla.org/es/docs/Web/HTTP/Reference/Methods)
- [Documentación de errores HTTP (el enlace esta mal porque no me dio tiempo a copiarlo)](https://developer.mozilla.org/es/docs/Web/HTTP/Reference/Methods)
# Practicas

## Enlaces de interes
- [UnityWebRequest](https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Networking.UnityWebRequest.html)
-  [Unity Corroutine](https://docs.unity3d.com/ScriptReference/Coroutine.html)

## P1.0 Chamada a API e serialización JSON

### Requisitos do completado
- **Abertas:** luns, 4 de marzo de 2024, 12:00 AM  
- **Pendente:** domingo, 17 de marzo de 2024, 11:59 PM

Neste exercicio utilizando `UnityWebRequest`, `JSONUtility` e corutinas deberás chamar á API:  
👉 [https://opentdb.com/](https://opentdb.com/)

Solicita 10 preguntas.

Esta API devolve un JSON que deberás serializar, creando as clases que coincidan coa estrutura do JSON.

- Crea a variable cos datos como un atributo **público** do script principal.  
- Imprime na consola:
  - O número de preguntas solicitadas
  - 4 datos da primeira pregunta

#### Entregables
- Enlace ao **repositorio GitHub** co proxecto
- Avaliarase o código que esté no último commit da rama `main`
- O fluxo de traballo será na rama `develop` e comitearás a `main` ao final mediante unha **Pull Request**
- Os 3 ficheiros `.cs` do proxecto:
  - Script principal
  - Clase da resposta da API
  - Clase da pregunta

---
## P1.1 Xogando con APIs

### Requisitos do completado
- **Abertas:** venres, 8 de marzo de 2024, 12:00 AM  
- **Pendente:** domingo, 17 de marzo de 2024, 12:00 AM

Escolle a API que ti queiras e utilízaa nun **minixogo**.

O propósito da práctica é que probes as capacidades das APIs e das corutinas en diferentes sentidos. Por exemplo, un minixogo de pregunta-resposta.

#### Ideas de qué facer con APIs (non é necesario facelas todas):
- Fai máis dunha chamada á API e que non sexa só no `Start`
- Utiliza contido multimedia como **imaxes ou vídeos**
- Integra un **bot conversacional**
- ...

#### Entregables
- Enlace ao **repositorio GitHub** co proxecto
- Avaliarase o código que esté no último commit da rama `main` ou nun tag chamado `final`
- É necesario utilizar **git ao longo do tempo**:
  - Non se admitirá un único commit á última hora
  - Nin todos os commits no último día ás presas  
  - Mellor ir equivocándose e cambiando cousas que subir todo á última hora