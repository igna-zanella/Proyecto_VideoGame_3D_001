using UnityEngine;

public class MovimientoJugador3D : MonoBehaviour
{
    private Rigidbody rbJugador;
    private bool estaSaltando = false;
    private Transform transformPOVCamera;

    [SerializeField] private float fuerzaSalto = 3f;
    [SerializeField] private float velocidad = 2f;
    void Start()
    {
        rbJugador = GetComponent<Rigidbody>();
        transformPOVCamera = GameObject.FindGameObjectWithTag("POVCamera").transform;
    }

    void FixedUpdate()
    {
        DetectarMovimiento();
        DetectarSalto();

        transform.eulerAngles = Vector3.up * transformPOVCamera.eulerAngles.y;
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
        Debug.Log(Input.GetAxis("Horizontal"));
        Debug.Log(Input.GetAxis("Vertical"));

        if (movimientoX != 0 || movimientoZ != 0)
        {
            rbJugador.linearVelocity = direccion * velocidad + Vector3.up * rbJugador.linearVelocity.y;
        }
    }
    private void DetectarSalto()
    {
        Debug.Log(Input.GetKey(KeyCode.Space));
        if (Input.GetKey(KeyCode.Space) && !estaSaltando)
        {
            rbJugador.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            estaSaltando = true;
        }
    }

}
