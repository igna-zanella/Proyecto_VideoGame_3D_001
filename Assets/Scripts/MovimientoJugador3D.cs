using UnityEngine;

public class MovimientoJugador3D : MonoBehaviour
{
    private Rigidbody rbJugador;
    private bool estaSaltando = false;
    private Transform transformPOVCamera;
    private Animator animacionJugador;

    [SerializeField] private float fuerzaSalto = 3f;
    [SerializeField] private float velocidad = 2f;

    private PickableObject objetoEnMano = null;
    [SerializeField] private float distanciaInteraccion = 3f;

    void Start()
    {
        rbJugador = GetComponent<Rigidbody>();
        transformPOVCamera = GameObject.FindGameObjectWithTag("POVCamera").transform; 
        animacionJugador = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        DetectarMovimiento();
        DetectarSalto();
        ActualizarRotacion();
        DetectarInteraccion();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terreno"))
        {
            estaSaltando = false;
        }
    }

    private void DetectarMovimiento()
    {
        float movimientoX = Input.GetAxis("Horizontal");
        float movimientoZ = Input.GetAxis("Vertical");
        Vector3 direccion = (transform.right * movimientoX + transform.forward * movimientoZ).normalized;
        //Debug.Log(Input.GetAxis("Horizontal"));
        //Debug.Log(Input.GetAxis("Vertical"));

        if (movimientoX != 0 || movimientoZ != 0)
        {
            rbJugador.linearVelocity = direccion * velocidad + Vector3.up * rbJugador.linearVelocity.y;
        }
        animacionJugador.SetFloat("Vertical", movimientoZ);
    }
    private void DetectarSalto()
    {
        //Debug.Log(Input.GetKey(KeyCode.Space));
        if (Input.GetKey(KeyCode.Space) && !estaSaltando)
        {
            rbJugador.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            estaSaltando = true;
        }
    }

    private void ActualizarRotacion()
    {
        transform.eulerAngles = Vector3.up * transformPOVCamera.eulerAngles.y;
    }

    private void DetectarInteraccion() // Métodos agregados para tomar y soltar objetos
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objetoEnMano == null)
            {
                // Intentar tomar un objeto
                IntentarTomarObjeto();
            }
            else
            {
                // Soltar objeto
                SoltarObjeto();
            }
        }
    }

    private void IntentarTomarObjeto() // Métodos agregados para tomar y soltar objetos
    {
        Ray ray = new Ray(transformPOVCamera.position, transformPOVCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanciaInteraccion))
        {
            PickableObject pickable = hit.collider.GetComponent<PickableObject>();

            if (pickable != null && !pickable.isPicked)
            {
                objetoEnMano = pickable;
                pickable.Pick();
            }
        }
    }

    private void SoltarObjeto() // Métodos agregados para tomar y soltar objetos
    {
        Vector3 posicionSueloFrente = transform.position + transform.forward * 1f;
        posicionSueloFrente.y = transform.position.y;

        objetoEnMano.Drop(posicionSueloFrente);
        objetoEnMano = null;
    }


}
