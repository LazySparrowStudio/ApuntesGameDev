# Programacion

## Atajos
- Ctrl + F12 -- Rename all Ocurrences
- Ctrl + . -- Quick Fix (Encapsule method)
- Crear un singleton para acceder al Gamemanager directamente

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
### Input Actions
- Parte del sistema de entrada moderno que reemplaza al sistema clásico (Input.GetKey, Input.GetButton, etc.)

- Podemos asignar distintos tipos de entrada y llamarlos en codigo para, por ejemplo, implentar movimiento

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
9. Limitaciones de movimiento
- Podemos usar Mathf.Clamp() para limitar una variable entre un valor maximo y otro minimo:
~~~C#


public void OnMove(InputValue value)
{
    movement = value.Get<Vector2>():
}
private void ClampedMovement()
{
    float xOffset = movement.x * speed * Time.deltaTime;
    float rawXPost = transform.localPosition.x + xOffset;
    float clampedXPos = Mathf.Clamp(rawXpos, -xClampRange, xClampRange);

    float yOffSet = movement.y * speed * Time.deltaTime;
    float rawYPost = transform.localPosition.y + yOffset;
    float clampedYPos = Matf.Clamp(rawYPost, -yClampRange, yClampRange);

    transform.localPosition = new Vector3(clampedXPos, transform.localPosition.y + yOffset, 0f);
}
~~~
## Salto:
### Salto con comprobacion de suelo
~~~C#
// ESTO ES UN EJEMPLO EXTRAIDO SOLO CON LO RELACIONADO CON SALTAR.
public class MarioMovement : MonoBehaviour
{
    float jumpForce = 6.5f; // Fuerza a aplicar con el salto
    int movementDirection;  //Direccion en la que esta mirando 
    bool startingJump; //Comprueba si ha empezado el salto (Creo que relacionado con la animacion)
    void Start()
    {
        startingJump = false; // Esta quieto
        walking = false; // Esta quieto
    }

    void Update(){
        if( IsGrounded() ) {
            animator.SetBool("jumping", false); //Si esta en el suelo, animacion no de salto
            if(Input.GetKeyDown(KeyCode.Space)) { //Pulsamos la tecla Space
                rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse); //Le damos un impulso al rigidbody para que el calculo se realiza con las fisicas.
                startingJump = true; //Ha empezado a saltar
                animator.SetBool("jumping", true); //Animador para que salte
            }
        }
    }
    private bool IsGrounded() {
        //Si estamos iniciando el salto no se utiliza el raycast para detectar si Mario está en el suelo,
        //por definición Mario no está en el suelo
        if(startingJump) {
            return false;
        }
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y -= 0.44f;
        float raycastRange = 0.10f;
        RaycastHit2D hit;

        hit = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastRange, groundDetectionLayerMask);

        if(hit.collider != null)  {
            //Debug.Log("[Mario] IsGrounded raycast hit: "+ hit.collider.gameObject.name);
            return true;
        }
        return false;
    }
}

~~~

## Colisiones:
-  Tipos: Box, Sphere, Capsule
- OnCollisionEnter, OnTriggerEnter
- Filtros con máscaras
- IsTrigger para reacciones
![alt text](image.png)
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
GameObject bulletPrefab;

if (Input.GetKeyDown(KeyCode.Space))
{
    GameObject bullet = Instatiate 
}
~~~

## Order of execution
- [UnityDOC](https://docs.unity3d.com/Manual/execution-order.html)

## Graphic Settings
- Project Settings --> Player --> Default Icon & Default Cursor

## Particles
- Añadir componente Particle System
- Uno de los submenus permite gestionar Collisions

## Scenes
 - File - BuildProfiles -> There we have a list(index) of the diff scenes 
 - Scene max value --> SceneManager.sceneCountInBuildSettings

## Build & Share
- File - Build Profiles - "Seleccionar la plataforma en la que exportar" - Build


## Interacciones
Edit>Project Settings> Input Manager 
~~~C
if (Input.GetButtonDown("Interaction"))
{
    Interaccion();
}
~~~

# Design Tips
 - Design "moments" and then expand them into a level. 

# Unity Moduls
## Terrain
- Añadir elemento > Terrain
- Tiene una elementa de terreo con herramientas de esculpido, modificacion etc. Por ahora parece mas versatil el de blender a falta de tener en cuenta como realizar la importacion

## MasterLine
- Basicamente un timeline gigante para la escena.
- Funciona igual que animacion en 2D u otras timelines


 

