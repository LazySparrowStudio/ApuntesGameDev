# Mecanicas

## Movimiento:
- Entrada por teclado: (Input.GetAxis, Input.GetKey)
- Traslación (transform.Translate)
- Físicas (Rigidbody.velocity, Rigidbody.AddForce)
- Rotaciones : transform.Rotate();
- Metodo de Vector3,moverse hacia un punto especifico: 
    MoveTowards(a,b,c)
        a = current, position de move from
        b = target, position to move towards
        c = maxDistanceDelta, distance to move each time
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
~~~
other.gameObject.tag == "Player";
~~~


## SerializeField 
- [SerializeField] Para permitir modificarlo desde la UI de Unity

## Components
~~~
GetComponent<>().material.color = Color.black;
~~~

## Relaciones entre componentes
- Podemos declarar en los scripts asociados a objetos variables para añadir relaciones con otros objetos

## Destroy Objects
- Destroy(gameObject);

## Frefabs
- Copias de un objeto como estancias
- Cambios sobre un prefab pueder overridear el resto de instancias que se encuentren en el estado puro de ese prefab

## Order of execution
- [UnityDOC](https://docs.unity3d.com/Manual/execution-order.html)

