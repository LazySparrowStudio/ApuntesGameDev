# Programacion

## Atajos
- Ctrl + F12 -- Rename all Ocurrences
- Ctrl + . -- Quick Fix (Encapsule method)

## Classes and Namespaces
- Encapsulated code. What the code can see, change or be changed by

## Methods
- Encapsulated code, member of a class and can be introduced with parametres

## Post Processing
- Volume component - Volumen en el que se va a aplicar el procesado

## Invoke 
- Call a method withing x seconds
~~~C#
Invoke("MethodName", delayInSeconds);
~~~

## Entrada

## Delegate
~~~C#
 public delegate PlayerBullet OnCannonShotDelegate(PlayerBullet bullet);
 public OnCannonShotDelegate onCannonShot;

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanShoot())
        {
            Shoot();
            GameObject bullet = Instantiate (bulletPrefab, shotPoint.position, shotPoint.rotation);
            bulletPrefab.GetComponent<Rigidbody>().AddForce(bulletPrefab.transform.position * 600f, ForceMode.Impulse);
            //Invocamos el Delegate onCannonShot
            if(onCannonShot != null)
            {
                onCannonShot(bullet.GetComponent<PlayerBullet>());
            }
        }
    }
~~~

## OnGUI
# VR

# Examen
### Agarre y posicionamiento de objectos con las manos
- Añadir el objeto 
- Añadirle el componente XR Grab Interactable
- Añadirle un Rigidbody
    - Marcar Use Gravity (Se cae si no hay nada debajo)
    - Desmarca Is Kinematic
- Configurar el attach:
    - Crear un objeto hijo con el nombre Attach y colocarlo en el campo Attach Transform del componente XR Grab Interactable del padre

### Agarre y posicionamiento de objeto en sockets
- Creamos un objeto empty y le llamamos Socket y le añadimos un collider como trigger y el XR Socket Interactor
- Dentro de el hacemos 2 hijos. Uno para el apartado visual y otro con el nombre Attach
- Posicionamos el attach donde queramos y lo ponemos como referencia dentro del XR Socket Interactor del Padre


### Limitacion de interaccion con objectos basado en las capas del XRInteractionToolkit (interaccion con manos, sockets y cambio de interaccion dinamica durante el juego)
- Seguir los pasos de la seccion anterior. 
- En el XR Grab Interactable del objeto usamos el sistema de capas
- En el XR Socket asociamos la capa correspondiente

### Activacion de objetos al apretar el gatillo y respuesta a eventos XR Interactable (SelectEnter, SelectExit, Activate, etc)
- En XR ORIGIN buscar los objetos Left Hand y Right Hand y añadirle a los 2 un XR Controller (Action based)
- En la propiedad "Select Action" comprobar que tengan asignados sus XRI/Select correspondientes
- Cogemos el objeto y le añadimos un Script
~~~ C#
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ActivateOnTrigger : MonoBehaviour
{
    public GameObject objetoActivar;
    private XRGRabInteractable grabInteractable;
    
    void Start(){
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    public void ActivarObjeto (bool enciende)
    {
        if (objetoActivar.activeSelf == false)
        {
            objetoActiar.SetActive(true);
        }else
        {
            objetoActivar.SetActive(false);
        }
    }
//Para que al soltarlo se desactive
    public void SoltarObjeto(){
        objetoActivar.SetActive(false)
    }
}
~~~

- En el XRGrabInteractable, buscamos el apartado de Interactable Events el boton pertinente (Select = gatillo, Active = prensa)
- Añadimos un evento en (Active Entered), asociamos el script y seleccionamos el metodo (ActivarObjeto) para cuando se pulse
- Si queremos que se apague cuando soltamos, en Select Exited le ponemos el SoltarObjeto(); 

### Desplazamiento
#### Movimiento Continuo
- Añadirle al XR ORIGIN un componente CharacterController
- Objeto vacio llamado Locomotion System
- Añadir Locomotion System
- Arrastrat XR ORIGIN al campo XR origin del Locomotion System
- Añadir a XR ORIGIN wl componente Continuous Move Provider (Action Based)
- En la opcion Move Input Action, asignar al joystick
    - Use reference
    - El preset XRI LeftHand/Move para mover con el stick izquierdo
En el XR Origin añadir el componente Continuos Turn Proider (Action Based)

#### Snap Turn
- Añadirle al XR ORIGIN un componente CharacterController
- Objeto vacio llamado Locomotion System
- Añadir Locomotion System
- Arrastrat XR ORIGIN al campo XR origin del Locomotion System
- Añadir a XR ORIGIN wl componente Continuous Move Provider (Action Based)
- En la opcion Move Input Action, asignar al joystick
    - Use reference
    - El preset XRI LeftHand/Move para mover con el stick izquierdo
- En el XR Origin añadir el componente Snap Turn Provider
- Si quieres cambiar el angulo de giro, modifica el valor de giro (por defecto esta a 45)

#### Teleport
- Añadirle al XR ORIGIN un componente CharacterController
- Objeto vacio llamado Locomotion System
- Añadir Locomotion System
- Arrastrat XR ORIGIN al campo XR origin del Locomotion System
- Añadir en Locomotion System el componente Teleportation Provider

- Hay 2 tipos de teletransporte, por plano o por puntos
- Por puntos:
- Crear Teleportation area
- Modificar el Line Bend Ratio para evitar curvatura del rayo

 ## Unity Quest 2 setup
 "*" For no tested
- *Build Setting to Android
- Package Manager - Install XR Plugin Management and Oculus XR plugin
- Project Settings - XR Plugin Management -> Check Oculus option 
// Hay que añadir unos objetos de los samples que no tengo muy claro
- Finalmente añadir a la escena un XR Origin

- Baseinteractable - Clase de la que derivan los interactables. Tienen los eventos del Grab sin que esten asociados directamente


## Agarrar

- XR Grab Interactable:
    - Attach Transform -> Gizmo por el que queremos agarrar el objeto. Hacemos un objeto hijo del main y lo usamos como Attach Object.
    - Interactable Events -> Lista de acciones asociadas a todos los botones de los mandos:
        
        - Select : Gatillo trasero 

private void SelectEnteredListener(SelectEnterEventArgs args)
XrBaseInteractor interactor = (XRBaseInteractor)args.interactorObject;
InteractorIdentifier ii= interactor.GetComponent<InteractorIdentifier>();

public void HoverEntered (HoverEnterEventArgs args)
{
    if(interactor is XRSocketInteractor){
        ToogleHoverMaterial(true);
    }
}

public void HoverExit (HoverExitEventArgs args)
{
    if(interactor is XRSocketInteractor){
        ToogleHoverMaterial(false);
    }
}

cosas a mirar. El obradoiro esta para mirar que interactables estan interaccionando con que y colocamiento de sockets.

Examen Tema 2 : XR Grabable. Sockets por capas
Componentes basicos de XR toolkit + un poco de codigo
# Mecanicas

## Movimiento:
1. Entrada por teclado:
    - (Input.GetAxis, Input.GetKey)
    - Definir previamente una variable como InputAction - Asignarla en el inspector y?
    ~~~C#
    [SerializeField] InputAction thrust;
    Rigidbody rb;
    
    
    private void OnEnable()
    {
        thrust.Enable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(thrust.isPressed(true))
        {
            
           rb.AddRelativeForce();
        }
    }
    ~~~
2. Traslación (transform.Translate)
3. Físicas (Rigidbody.velocity, Rigidbody.AddForce)
4. Rotaciones : transform.Rotate(); EulerRotations;
5. Metodo de Vector3,moverse hacia un punto especifico: 
    MoveTowards(a,b,c)
        a = current, position de move from
        b = target, position to move towards
        c = maxDistanceDelta, distance to move each time
6. Usar un InputAction para asociar entradas
7. Pulsar una tecla. el .xKey hace referencia a que tecla
~~~C#
Keyboard.current.lKey.isPressed
Keyboard.current.lKey.wasPressedThisFrame
~~~
La 2ª entrada solo comprueba 1 vez
8. Oscilaciones:
~~~C#
    - factorMovimiento = Mathf.PingPong(Time.time * speed, 1f); 
    - Vector3.Lerp(posicionInicial, posicionFinal, factorMovimiento); 
~~~
    - Objeto se mueve de punto A a punto B de forma progresiva 

## Salto:
- Comprobar suelo:  collider o Physics.Raycast/Collider.IsTouchingLayers

## Colisiones:
-  Tipos: Box, Sphere, Capsule
- OnCollisionEnter, OnTriggerEnter
- Filtros con máscaras
- IsTrigger para reacciones

~~~ 
private void OnCollisionEnter (Collision other)
~~~

## Cámara:
- Cinemachine: Tipo de camara inteligente preconfigurada 

## Tags
Tras registrar una colision, buscamos un componente de ese objeto (other):
~~~C#
other.gameObject.tag == "Player";
~~~

## Audio
- Necesitamos una fuente (Audio Source) y un receptor (Audio Listener)
- Reproducir audio por codigo. AudioSource es el componente de Unity y AudioClip el propio archivo a reproducir 
~~~C#
[SerializedField] audioFile;
AudioSource audiosource;
audioSource.PlayOneShot(audioFile);
~~~

## Animator
- Ventana Animation - Con FileAnimation (.anim) ya creado, seleccionamos los sprites que necesitemos y los vamos colocando en la timeline para hacer la animacion que necesitemos.
1. Creamos un Animator Controller y se lo asignamos al COMP Animator del objeto
2. Creamos un script para controlar las animaciones 
~~~C#
Animator animator;
SpriteRenderer sprite;
void Start() {
    animator = GetComponent<Animator>();
}

void Update() {
    float speed = Input.GetAxis("Vertical");
    animator.SetFloat("Speed", speed);
}

~~~

- Ventana Animator - Set as layer default state - para animacion base
- Añadimos transicciones, parametros y condiciones desde el propio entorno
- En el inspector, marcar Has Exit Time para que las animaciones tengan un fin
- Dependiendo de la animacion (2D) podemos usar la mitad de las direcciones girando los sprites - sprite.flipX = true|| sprite.flipY = true; 

~~~C#
SpriteRenderer sprite;
sprite.flipX = true;
sprite.flipY = true; 

~~~

## SerializeField 
- [SerializeField] Para permitir modificarlo desde la UI de Unity

## Components
~~~C#
GetComponent<>().material.color = Color.black;
~~~

## Relaciones entre componentes
- Podemos declarar en los scripts asociados a objetos variables para añadir relaciones con otros objetos

## Destroy Objects
- Destroy(gameObject);

## Frefabs
- Copias de un objeto como estancias
- Cambios sobre un prefab pueder overridear el resto de instancias que se encuentren en el estado puro de ese prefab

~~~C#
GameObject bylletPrefab;

if (Input.GetKeyDown(KeyCode.Space))
{
    GameObject bullet = Instatiate 
}
~~~

## Order of execution
- [UnityDOC](https://docs.unity3d.com/Manual/execution-order.html)

## Graphic Settings
- Project Settings --> Player --> Default Icon & Default Cursor

## Particules
- Añadir componente Particle System

## Scenes
 - File - BuildProfiles -> There we have a list(index) of the diff scenes 
 - Scene max value --> SceneManager.sceneCountInBuildSettings

## Build & Share
- File - Build Profiles - "Seleccionar la plataforma en la que exportar" - Build

# Design Tips
 - Design "moments" and then expand them into a level. 

 

