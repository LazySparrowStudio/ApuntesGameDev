# Apuntes Redes

## 📘 Tema 1: Conceptos Base

### 🌐 Redes
- **LAN** (Local Area Network) – Red de área local.
- **WAN** (Wide Area Network) – Red de área amplia.

### 📦 Comunicación y Paquetes
- **Paquetes** – Unidades organizadas de información que se transmiten por la red.
- **Ping** – ⏱ Tiempo que tardan los paquetes en ir y volver entre dos dispositivos.
- **TTL (Time To Live)** – ⏳ Valor que se reduce en 1 cada vez que un paquete pasa por un router. Si llega a 0, el paquete se descarta.

### 🧭 Direcciones IP
- **IP** – Dirección única que identifica a un equipo en una red.
- **IPv4** – 📏 Dirección de 32 bits (límite ~4.000 millones de combinaciones).
- **IPv6** – 📏 Dirección de 128 bits, pensada para suplir el límite de IPv4.

> 🔎 Aunque IPv6 está disponible, se sigue usando ampliamente IPv4 mediante técnicas como el uso de redes privadas con NAT (Network Address Translation).

### 🚪 Puertos
- **Puerto** – Ruta de acceso a servicios dentro del router o equipo.
  - Los puertos **por debajo del 1024** están reservados para **protocolos estándar** (ej. HTTP, FTP).
  - Los puertos **a partir del 1024** están **libres** para usos personalizados.

### 🔌 APIs y Procesamiento de Datos
- **API** (Application Programming Interface) – 🔁 Sistema que permite que dos aplicaciones se comuniquen entre sí.  
  _Ejemplo:_ Solicitar información del tiempo a través de la API de MeteoGalicia y recibir datos en formatos estructurados como CSV, JSON o XML.

### 💾 Procesamiento de Datos
- **Serializar** – 🔄 Convertir datos en un formato estándar para almacenarlos o transmitirlos fácilmente.
- **Parsear** – 📖 Verificar que una estructura de datos esté bien escrita y luego deserializarla para su procesamiento.

### 🔑 Autoridad
#### 🧠 Diferencias entre Server, Client, Owner y Host en Unity (Netcode for GameObjects)

Cuando trabajas con Netcode for GameObjects en Unity, es fundamental entender los distintos roles de red que puede tener una instancia del juego o un objeto. Aquí se explican claramente:

---

#### ✅ **Server**
- Controla el estado del juego y lo sincroniza hacia los clientes.
- Tiene autoridad sobre todos los objetos de red.
- Ejecuta todos los `ServerRpc`.
- Puede o no tener un jugador asociado.
- Solo hay uno por partida.

```csharp
if (IsServer) { /* código exclusivo del servidor */ }
```

---

#### ✅ **Client**
- Se conecta al servidor para recibir el estado del juego y enviar acciones.
- Ejecuta los `ClientRpc` recibidos desde el servidor.
- Puede ser dueño de objetos, pero no tiene autoridad total.
- Puede haber múltiples clientes.

```csharp
if (IsClient) { /* código que corre en clientes */ }
```

---

#### ✅ **Owner**
- Es el cliente **dueño de un objeto específico**.
- Solo el owner puede modificar variables con permisos restringidos y enviar ciertos RPCs.
- Cada `NetworkObject` tiene un único owner.
- Muy usado para jugadores, cámaras, inventarios, etc.

```csharp
if (IsOwner) { /* este cliente es el dueño de este objeto */ }
```

---

#### ✅ **Host**
- Es un caso especial: **actúa como servidor y cliente al mismo tiempo**.
- Ideal para pruebas o partidas locales.
- Ejecuta tanto `IsServer` como `IsClient`.
- Normalmente también es `IsOwner` de su propio objeto.

```csharp
if (IsHost) { /* código para cuando se es host */ }
```

---

#### 🎯 Ejemplo práctico

En una partida con 3 jugadores:
- Uno es el **Host** (servidor + jugador).
- Los otros dos son **Clients** conectados.
- Cada jugador es el **Owner** de su personaje.

---
#### 📊 Tabla comparativa de roles

| Rol   | ¿Ejecuta código? | ¿Tiene autoridad global? | ¿Puede ser dueño (`IsOwner`)? | ¿Usa `ServerRpc`? | ¿Usa `ClientRpc`? |
|-------|------------------|---------------------------|-------------------------------|--------------------|--------------------|
| Server | Sí               | ✅ Sí                     | ❌ No necesariamente           | ✅ Sí              | ✅ Sí              |
| Client | Sí               | ❌ No                     | ✅ Sí                          | ❌ No              | ✅ Sí              |
| Owner  | Solo sobre su objeto | ❌ No               | ✅ Sí                          | ✅ (limitado)      | ✅ (limitado)      |
| Host   | Sí (Server + Client) | ✅ Sí                 | ✅ Sí                          | ✅ Sí              | ✅ Sí              |

  
##  📘 Tema 2 Multiplayer Development -  [Unity Manual](https://docs-multiplayer.unity3d.com/netcode/current/about/)

### 1. Resumen general
#### 🔧 1. Modelo de Red (Networking Model)

Unity permite dos modelos principales:

- **Host-Client:** Un jugador actúa como servidor (host) y cliente a la vez.
- **Server-Client:** Un servidor dedicado maneja toda la lógica del juego, y los clientes solo se conectan a él.

---

#### 📦 2. Frameworks Populares para Multiplayer

- **Netcode for GameObjects (Unity Netcode):** Oficial, enfocado en objetos de Unity.
- **Mirror:** Open source, reemplazo de UNet, fácil de usar.
- **Photon (PUN, Fusion):** Solución en la nube, ideal para juegos casuales.
- **Fish-Net:** Alternativa moderna, potente y open source.

---

#### 🔁 3. Sincronización de Estado (State Sync)

Es necesario sincronizar:

- Posiciones de objetos y jugadores.
- Variables clave (vida, puntuación, etc.).
- Inputs, animaciones, etc.

**Herramientas comunes:**

- **RPCs (Remote Procedure Calls):** Ejecutan funciones remotamente entre cliente y servidor.
- **Network Variables / SyncVars:** Variables sincronizadas automáticamente.

---

#### 🔒 4. Autoridad (Authority)

Define quién controla los datos del juego:

- **Servidor autoritativo:** El servidor toma todas las decisiones. Más seguro.
- **Cliente autoritativo:** Más rápido, pero menos seguro.

---

#### 🔌 5. Gestión de Conexiones y Jugadores

- Creación de salas o lobbies.
- Unirse a partidas existentes.
- Asignar ID únicos a jugadores.
- Manejo de desconexiones/reconexiones.

---

#### 🧰 6. Flujo de Desarrollo Típico

1. Crear objetos con `NetworkObject`.
2. Añadir componentes como `NetworkTransform`, `NetworkBehaviour`.
3. Programar la lógica usando RPCs y variables sincronizadas.
4. Probar localmente en modo host/cliente o con builds separados.

---

#### ⚠️ 7. Retos Comunes

- Latencia y lag (lag compensation).
- Predicción e interpolación.
- Sincronización de físicas.
- Seguridad contra trampas o exploits.


#### 8. Conceptos en clase
- NetworkBehaviour es una clase que hereda de Monobehaviour
  
### 2. NetworkBehaviour
#### Principales cualidades de `NetworkBehaviour` en Unity

`NetworkBehaviour` es una clase base fundamental en Unity cuando estás trabajando con **Netcode for GameObjects (NGO)**, la solución oficial de Unity para juegos multijugador. Heredar de `NetworkBehaviour` permite que un objeto sincronice su estado en la red.

---

#### ✅ Principales cualidades de `NetworkBehaviour`:

1. **Sincronización de variables (`NetworkVariable`)**  
   Permite declarar variables que se sincronizan automáticamente entre el servidor y los clientes.  
   ```csharp
   public NetworkVariable<int> health = new NetworkVariable<int>(100);
   ```

2. **Detección del contexto de ejecución**  
   Puedes saber si el script se está ejecutando en el servidor, cliente o dueño del objeto:
   - `IsServer`
   - `IsClient`
   - `IsOwner`
   - `IsHost`

3. **RPCs (Remote Procedure Calls)**  
   Permite definir funciones que se ejecutan a través de la red.
   - `ServerRpc`: Llamada del cliente al servidor.
   - `ClientRpc`: Llamada del servidor a los clientes.
   ```csharp
   [ServerRpc]
   void DoSomethingServerRpc() { }

   [ClientRpc]
   void NotifyClientsClientRpc() { }
   ```

4. **Identidad de red (`NetworkObject`)**  
   Todo `NetworkBehaviour` está vinculado a un `NetworkObject`, que lo identifica en la red.

5. **Propiedad de objetos (Ownership)**  
   Puedes transferir o comprobar la propiedad de un objeto de red, útil para cosas como vehículos o personajes controlables.
   ```csharp
   NetworkObject.ChangeOwnership(ulong newOwnerClientId);
   ```

6. **Eventos de ciclo de vida de red**  
   Métodos que se ejecutan en momentos clave:
   - `OnNetworkSpawn()`: Cuando el objeto aparece en la red.
   - `OnNetworkDespawn()`: Cuando se destruye o desconecta.
   - `OnGainedOwnership()`: Cuando un cliente gana la propiedad del objeto.

7. **Seguridad y control**  
   Puedes controlar el acceso a RPCs y variables según el rol del usuario (cliente/servidor).

8. **Escalabilidad**  
   Puedes sincronizar estados simples o estructuras más complejas como listas, structs y más (con algunas limitaciones y personalizaciones).

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