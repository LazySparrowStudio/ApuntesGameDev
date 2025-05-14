
# Tema 1

## Agarre y posicionamiento de objectos con las manos
- Añadir el objeto 
- Añadirle el componente XR Grab Interactable
- Añadirle un Rigidbody
    - Marcar Use Gravity (Se cae si no hay nada debajo)
    - Desmarca Is Kinematic
- Configurar el attach:
    - Crear un objeto hijo con el nombre Attach y colocarlo en el campo Attach Transform del componente XR Grab Interactable del padre

## Agarre y posicionamiento de objeto en sockets
- Creamos un objeto empty y le llamamos Socket y le añadimos un collider como trigger y el XR Socket Interactor
- Dentro de el hacemos 2 hijos. Uno para el apartado visual y otro con el nombre Attach
- Posicionamos el attach donde queramos y lo ponemos como referencia dentro del XR Socket Interactor del Padre


## Limitacion de interaccion con objectos basado en las capas del XRInteractionToolkit (interaccion con manos, sockets y cambio de interaccion dinamica durante el juego)
- Seguir los pasos de la seccion anterior. 
- En el XR Grab Interactable del objeto usamos el sistema de capas
- En el XR Socket asociamos la capa correspondiente

## Activacion de objetos al apretar el gatillo y respuesta a eventos XR Interactable (SelectEnter, SelectExit, Activate, etc)
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

## Desplazamiento
### Movimiento Continuo
- Añadirle al XR ORIGIN un componente CharacterController
- Objeto vacio llamado Locomotion System
- Añadir Locomotion System
- Arrastrar XR ORIGIN al campo XR origin del Locomotion System
- Añadir a XR ORIGIN al componente Continuous Move Provider (Action Based)
- En la opcion Move Input Action, asignar al joystick
    - Use reference
    - El preset XRI LeftHand/Move para mover con el stick izquierdo


### Snap Turn
- Añadirle al XR ORIGIN un componente CharacterController
- Objeto vacio llamado Locomotion System
- Añadir Locomotion System
- Arrastrar XR ORIGIN al campo XR origin del Locomotion System
- Añadir a XR ORIGIN wl componente Snap Turn Provider (Action Based)
- En la opcion Move Input Action, asignar al joystick
    - Use reference
    - El preset XRI LeftHand/Move para mover con el stick izquierdo
- Si quieres cambiar el angulo de giro, modifica el valor de giro (por defecto esta a 45)

### Teleport
- Añadirle al XR ORIGIN un componente CharacterController
- Objeto vacio llamado Locomotion System
- Añadir Locomotion System
- Arrastrat XR ORIGIN al campo XR origin del Locomotion System
- Añadir en Locomotion System el componente Teleportation Provider

- Hay 2 tipos de teletransporte, por plano o por puntos
- Por puntos:
- Crear Teleportation area
- Modificar el Line Bend Ratio para evitar curvatura del rayo

## Colocar un GameObjeto(A) sobre otro GameObjeto(B) en el que ya existe un Socket. Agarrar A y soltarlo en B. Solo poder retirar A MIENTRAS agarras B
- Hacer un Empty Object(B) y añadirle Rigidbody y XRGrabInteractable
- Añadirle los hijos correspondientes:
    1. Apartado visual (Opcional) - MeshRender
    2. Zona y orientacion de agarre (Attach) - Empty Object
    3. Lugar de colocación del objeto flotante - Empty Object con componente XR SocketInteractor y un Collider
- Crear el objeto flotante que va a ser colocado (A) - GameObject (cubo o el objeto que sea) con un Collider, Rigidbody y XRInteractable  
- Ojito a las curvas que se vienen:
    - Al EmptyObject(B) que hicimos al pricipio, añadimos un script para tener un registro de los distintos sockets que tenemos:
    ~~~C#
    // Script para la tabla
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;

        public class PuzzleBoard : MonoBehaviour
        {
            //Array en el que van a ir los sockets
            PuzzleSocket[] onBoardPuzzleSockets;

            void Start()
            {
                //Buscamos as referencias ós PuzzleSocket situados dentro do PuzzleBoard
                onBoardPuzzleSockets = GetComponentsInChildren<PuzzleSocket>();
            }

            //
            public void EnableHandInteraction(bool enabled)
            {
                //Recoge el Socket de cada hijo y llama al EnableHandsInteraction de PuzzleSocket y le pasa el bool
                foreach (PuzzleSocket ps in onBoardPuzzleSockets)
                {
                    ps.EnableHandInteraction(enabled);
                }
            }
        }

    ~~~
    ~~~C#
    //PuzzleSocket.cs
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;
        using UnityEngine.XR;
        using UnityEngine.XR.Interaction.Toolkit;

        public class PuzzleSocket : MonoBehaviour {
            private XRSocketInteractor xrSocketInteractor;

            //Variable que controla si esta siendo agarrado
            private bool handInteraction;

            public bool HandInteraction => handInteraction;
            
            // Start is called before the first frame updat
            void Start() {
                xrSocketInteractor = GetComponent<XRSocketInteractor>();
                handInteraction = false;
            }

            //Metodo llamado por PuzzleSocket que coge el bool, activa o desactiva el que la tabla este agarrada y si tiene algo coge el Script Puzzle Piece que este dentro, es decir, el del Objeto que tiene que colocarse
            public void EnableHandInteraction(bool enabled)  {
                handInteraction = enabled;
                PuzzlePiece pp;
                if(xrSocketInteractor.selectTarget != null) {
                    pp = xrSocketInteractor.selectTarget.GetComponent<PuzzlePiece>();
                    pp.EnableHandInteraction(enabled);
                    
                }
            }
        }

    ~~~
        ~~~C#
        //PuzzlePiece.cs
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;
        using UnityEngine.XR.Interaction.Toolkit;

        public class PuzzlePiece : MonoBehaviour {
            private XRGrabInteractable xrGrabInteractable;

            public InteractionLayerMask handsEnabledLayerMask;
            public InteractionLayerMask handsDisabledLayerMask;

            // Start is called before the first frame update
            void Start() {
                xrGrabInteractable = GetComponent<XRGrabInteractable>();
            }

            public void SetMaterial(Material material) {
                GetComponent<MeshRenderer>().material = material;
            }

            public void EnableHandInteraction(bool enabled) {
                if(enabled) {
                    xrGrabInteractable.interactionLayers = handsEnabledLayerMask;
                } else {
                    xrGrabInteractable.interactionLayers = handsDisabledLayerMask;
                }
            }

            public void EnableHandInteraction() {
                xrGrabInteractable.interactionLayers = handsEnabledLayerMask;
            }

            public void DisableHandInteraction(SelectEnterEventArgs args) {
                if(args.interactorObject is XRSocketInteractor) {
                    XRSocketInteractor socket = (XRSocketInteractor)args.interactorObject;
                    PuzzleSocket ps = socket.GetComponent<PuzzleSocket>();
                    if( ! ps.HandInteraction) {
                        xrGrabInteractable.interactionLayers = handsDisabledLayerMask;
                    }
                }
            }
        }
    ~~~

# Tema 2
    
## Comprobador de manos

    ~~~C#
    if (interactor.intereatablesSelected.Count > 0)
    {
        XRBaseInteractable interactable = (XRBaseInteractable)interactor.interactablesSelected[0];
        if(interactable != null && interactable.gameObject.CompareTag("Arrow"))
        {
                //Tenemos una flecha en la mano
                //Tenemos que saber si la flecha esta en la cuerda

                Arrow arrow = interactable.GetComponent<Arrow>();
                PullMeasurer pullMeasurer = arrow.GetSelectedString();

                if(pullMeasurer != null)
                {
                        attach.position =  pullMeasurer.stringPullPoint.position;
                        attach.position =  pullMeasurer.stringPullPoint.position;

                }else
                    {
                        attach.position = transform.position;
                        attach.rotation = transform.rotation;
                    }
            }else 
                {
                    attach.position = transform.position;
                    attach.rotation = transform.rotation;
                }
    }
    ~~~
## Interaccion con panel
- Añadir al panel con botones previamente añadidos un Tracked Device Graphic Raycaster
- En los botones se le añade (deberia de venir por defecto) un evento OnClick();
    Para ejecutar codigo en este espacio, añadimos los metodos al script. En el ejemplo tenemos la siguiente jerarquia:
        1. Marcador
            2. Panel
                3.1 Boton Reiniciar
                3.2 Boton Recuperar

El objecto marcador tiene el script de gestion, en la funcion OnClick del Boton Reiniciar le ponemos la referencia al marcador y añadimos el metodo ReiniciarOnClick
Para recuperar, lo mismo pero con su correspondiente método.


