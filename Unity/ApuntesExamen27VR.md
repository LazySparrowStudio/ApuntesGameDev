# Movimiento y salto del personaje ✔️

1. Añadimos previamente al Jugador un Character Controller y un Rigidbody.
~~~C#
public CharacterController controller;

void Update(){
    controller.Move(playerVelocity * Time.deltaTime);
}

if(groundedPlayer) {
            //"Vertical" hace referencia a las Input Actions predefinidas por Unity.
            Input.GetAxis("Vertical"));
            //Obtiene la DIRECCION NORMALIZADA del movimiento. Esta determinado para que sea en referencia con la camara que tiene puesta. Si sustituimos el cameraTransform.forward por transform.position.forward se mueve hacia delante, pero el movimiento de la camara no define el del personaje.
            Vector3 move = cameraTransform.forward *  Input.GetAxis("Vertical") + cameraTransform.right * Input.GetAxis("Horizontal");
            move.y = 0;
            move = move.normalized;
            
            //Ahora si, aplicamos velocidad al movimiento
            playerVelocity.x = move.x * playerSpeed;
            playerVelocity.z = move.z * playerSpeed;            
        
            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
                if(currentState != State.Push) {
                    SetState(State.Run);
                }
            } else {
                //Las animaciones en este ejemplo estan definidas con un enum. Aqui esta poniendo la animacion a Idle
                SetState(State.Idle);            
            }

            // Makes the player jump
            if (Input.GetButtonDown("Jump"))
            {
                //Usa la propia "gravedad" para gestionar la fuerza del salto
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * -9.81);
                
                //Lo explicado de antes pero ahora con Saltar.
                SetState(State.Jump);
            }
~~~


# Animación del personaje ✔️
Para controlar las animaciones usamos un enum y una variable que vaya actualizandose:
~~~C#
    private enum State { Idle, Run, Jump, Push };

    private State currentState;
~~~

De esta forma definimos los states en un método:
~~~C#

    private void SetState(State newState) {
        if(newState == currentState) {
            return;
        }

        animator.ResetTrigger("run");
        animator.ResetTrigger("idle");
        animator.ResetTrigger("jump");
        animator.ResetTrigger("push");
        switch(newState) {
            case State.Idle:
               animator.SetTrigger("idle");
               break;
            case State.Run:
               animator.SetTrigger("run");
               break;
            case State.Jump:
               animator.SetTrigger("jump");
               break;
            case State.Push:
               animator.SetTrigger("push");
               break;
        }
        currentState = newState;
    }
~~~

Y luego simplemente si nos movemos, en la parte del código donde se determina el movimiento, actualizamos el State:
~~~C#
if(meMuevo){
SetState(State.Run);
}
~~~

Hay que acordarse de que los SetTrigger("") lo que hacen es activar una variable del Animator, por lo que hay que configurar previamente en el Animator del objeto esas variables con las condiciones de entrada y salida
# MO Fuerza💿 (movimiento del menhir) ✔️
~~~C#

    void FixedUpdate()  {
        if(groundedPlayer) {
            Pushable pushableStone = SearchForPushable();
            if(pushableStone != null) {
            //si encuentra el pushable llama al metodo push del menhir y le da la                 fuerza
                SetState(State.Push);
                pushableStone.Push(playerVelocity * Time.deltaTime);
            }
        }
    }
    //mira si tiene un menhir delante
    private Pushable SearchForPushable() {
        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 0.5f, ~0, QueryTriggerInteraction.Ignore)) {
        //si lo es recoge su script
            if(hit.collider.gameObject.CompareTag("Pushable")) {
                return hit.collider.GetComponent<Pushable>();
            }
        }

        return null;
    }
    //esto en el Script del menhir
    public List<Vector3>[] checkpointsDirections;
    public Transform[] checkpoints;
    
        public void Push(Vector3 displacement) {
        //Por si acaso ponemos la componente vertical a 0
        if( ! CheckForObstacles(displacement)) {
            displacement.y = 0;
            transform.position += displacement;    
        }
    }
~~~
# Espaneo de obstáculo (aclarado en NOTAS) ✔️
# Choque con barrera y muerte del personaje ✔️
Fasilito. Exactamente igual que en 2D pero más facil porque no hay que añadirle el "2D" al método
~~~C#
    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("NextLevelDoor")) {
            autopilot = true;
            target = other.transform;
            SetState(State.Run);
            return;
        }
~~~


# Cámara de entorno ✔️
1. Añadimos una cámara Cinemachine. Si no tenemos la opción disponible hay que installar el paquete correspondiente en el Package Manager
2. Click Derecho sobre la Jerarquía, añadimos la Cinemachine que nos toque.
3. En líneas generales, lo único que queremos añadir siempre en el componente de la cámara es el Follow, y el Look At. Son los Transform que dirigen el posicionamiento y movimiento de la cámara. Dependiendo del tipo que sea no tiene por qué tener los 2
# Interacción con objetos (recoger caja) ✔️
En el script del personaje (o lo que vaya a interactuar) declaramos una variable en la que vamos a recoger el recogible:
~~~C#
private GameObject pickableObject;
~~~

Luego configuramos que cuando se pulse el botón correspondiente, pase algo (en el update):
~~~C#
private void Update()
{             if(Input.GetButtonDown("Interaction")) {
                if(pickableObject == null)                 {
                    PickObject();
                } else {
                    ReleaseObject();
                }
            }
}
~~~
Como aclaración, "Interaction" es una acción del mapa de InputActions default de Unity.

Y aqui tenemos el PickObject();
~~~C#
    private void PickObject(){
        
        pickableObject = SearchForPickable(new Vector3(0f, 0.7f, 0.2f));
        if(pickableObject == null) {
            pickableObject = SearchForPickable(new Vector3(0f, 0.35f, 0.2f));
        }

        if(pickableObject != null) {
            Debug.Log("Interaction pulsado, hai un objeto pickable");
            pickableObject.transform.parent = transform;
            pickableObject.transform.localPosition = new Vector3(0f, 0.8f, 0.4f);
            pickableObject.transform.localRotation = Quaternion.identity;
            pickableObject.GetComponent<Rigidbody>().isKinematic = true;
        }

    }
~~~
Y el ReleaseObject():
~~~C#
    private void ReleaseObject() {
        pickableObject.transform.parent = null;
        pickableObject.GetComponent<Rigidbody>().isKinematic = false;
        pickableObject = null;
    }
~~~

# NOTAS (Y apartados de ejercicios) ✔️
## Spawneo de Obstaculos(GranjeroSalteado):
- Se generan las Barreras y dentro de las mismas tienen el siguiente Script:
~~~C#

    void Update()
    {
        // Cando lleguen a la posicion horizontal. Se desactivan
        if(transform.position.z < -40f) {
            Deactivate();
        }   
    }
    //
    private void Deactivate() {
        GameManager.instance.BarrierDeactivate(gameObject);
        gameObject.SetActive(false);
    }
~~~

- Además, en el GameManager se gestiona el spawneo:
~~~C#

    // Al empezar llamamos a la Corrutina e iniciamos una lista previamente declarada:
    void Start() {
        barrierPool = new List<GameObject>();
        StartCoroutine(SpawnBarriers());
        
    }
    //Método que corre dentro de Barrier y añade la barrera a la lista
    public void BarrierDeactivate(GameObject barrier) {
        barrierPool.Add(barrier);
    }
    //Numerator para la Corrutina que desactiva un objeto cuando se sale del limite y lo reposiciona al principio cuando vuelve a spawnear
    private IEnumerator SpawnBarriers() {
        while(true) {
            yield return new WaitForSeconds(Random.Range(1f, 4f));

            if(barrierPool.Count > 0) {
                GameObject barrier = barrierPool[0];
                barrierPool.RemoveAt(0);
                barrier.transform.position = barrierSpawnPoint;
                barrier.SetActive(true);
            } else {
                Instantiate(barrierPrefab, barrierSpawnPoint, Quaternion.identity);
            }
        }
    }
~~~

## Movimiento de puerta horizontal y rotacional:
### Horizontal ✔️
~~~C#
public class PlayerDetectorHorizontal : MonoBehaviour
{
   
    public GameObject verticalDoor;  //Referencia al objeto que se va a desplazar
    public Vector3 startPos; //Posicion del que parte y a la que tiene que volver
    public Vector3 endPos = new Vector3(0.25f, 4f, 0f); // Posicion a la que tiene que viajar
    public float speed = 0.25f; // Adjust speed as needed
    public float t = 0f; // Time tracker
    public bool entrasteEnLaPuerta = false; //Bandera => El personaje ha entrado en la zona
    public bool cerrandoPuerta = false; //Bandera => La puerta se esta cerrando

    private void Start()
    {
        startPos = verticalDoor.transform.position;
        endPos = startPos + new Vector3(0.25f, 0f, 4f); // Ahora es relativa a la posición inicial
    }
    
    void Update()
    {
        if (entrasteEnLaPuerta)
        {
            OpenDoor();
        }
        if (cerrandoPuerta)
        {
            CloseDoor();
        }

    }
    private void OpenDoor()
    {
        if (t < 1f)
        {
            t += Time.deltaTime * speed;
            verticalDoor.transform.position = Vector3.Lerp(startPos, endPos, t);
        }
        else
        {
            t = 0f;
            CloseDoor();
        }

    }
    private void CloseDoor()
    {
        
        t += Time.deltaTime * speed;
        Vector3 localPos = verticalDoor.transform.position;
        verticalDoor.transform.position = Vector3.Lerp(localPos, startPos, t);
    }

    void OnTriggerEnter(Collider other)
    {
        entrasteEnLaPuerta = true;
        cerrandoPuerta = false;
        t = 0f;
    }

    void OnTriggerExit(Collider other)
    {
        entrasteEnLaPuerta = false;
        cerrandoPuerta = true;
        t = 0f;
    }
}
~~~

### Vertical ✔️

~~~C#
//Exactamente igual que el script anterior
public class PlayerDetectorRotationL : MonoBehaviour
{

    public Vector3 startPos;
    public Vector3 endPos = new Vector3(0.25f, 4f, 0f);
    public float speed = 0.25f; // Adjust speed as needed
    public float t = 0f; // Time tracker
    public bool entrasteEnLaPuerta = false;
    public bool cerrandoPuerta = false;

    private void Start()
    {
        startPos = verticalDoor.transform.position;
        endPos = startPos + new Vector3(0.25f, 4f, 0f); // Ahora es relativa a la posición inicial

        Debug.Log($"startPos: {startPos}, endPos: {endPos}");
    }
    void Update()
    {
        if (entrasteEnLaPuerta)
        {
            OpenDoor();
        }
        if (cerrandoPuerta)
        {
            CloseDoor();
        }

    }
    private void OpenDoor()
    {
        if (t < 1f)
        {
            t += Time.deltaTime * speed;
            verticalDoor.transform.position = Vector3.Lerp(startPos, endPos, t);
        }
        else
        {
            t = 0f;
            CloseDoor();
        }

    }
    private void CloseDoor()
    {
        
        t += Time.deltaTime * speed;
        Vector3 localPos = verticalDoor.transform.position;
        verticalDoor.transform.position = Vector3.Lerp(localPos, startPos, t);
    }

    void OnTriggerEnter(Collider other)
    {
        entrasteEnLaPuerta = true;
        cerrandoPuerta = false;
        t = 0f;
    }

    void OnTriggerExit(Collider other)
    {
        entrasteEnLaPuerta = false;
        cerrandoPuerta = true;
        t = 0f;
    }
}

~~~