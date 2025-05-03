using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float[] speedLevels = { 3f, 5f, 7f, 9f, 12f, 15f };
    public int currentSpeedLevel = 0;
    public float holdTime = 0f;
    public float releaseTime = 0f;
    public bool isMoving = false;
    private bool canShiftGear = false;
    public bool isReversing = false;

    public float rotationSpeed = 150f;
    public Transform pivotLlantaTrasera1;
    public Transform pivotLlantaTrasera2;
    public Transform[] frontWheels;
    public Transform[] steeringWheels;

    private Rigidbody rb;
    public float gravityForce = 60f;
    private float reverseSpeed = 5f;
    private bool uartAvanceActivo = false;
    private bool uartReversaActiva = false;
    private bool uartIzquierdaActiva = false;
    private bool uartDerechaActiva = false;
    private int ultimoComandoValido = 0x00;
    private bool frenoActivo = false;
    public bool CanShiftGear => canShiftGear;

    void Start()
    {
        speedLevels = new float[] { 3f, 5f, 7f, 9f, 12f, 15f }; 
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = true;
        rb.mass = 100f;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 0.3f;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        float moveZ = Input.GetAxis("Vertical");
        float rotateY = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            isMoving = false;
            isReversing = false;
            currentSpeedLevel = 0;
            holdTime = 0f;
            releaseTime = 0f;
            canShiftGear = false;
            return;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            isMoving = false;
            isReversing = false;
            RotateWheels(0);
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            isMoving = true;
            isReversing = false;
            holdTime += Time.deltaTime;
            releaseTime = 0f;

            if (holdTime >= 2f)
                canShiftGear = true;
        }
        else
        {
            if (isMoving && !uartAvanceActivo)
            {
                releaseTime += Time.deltaTime;
                if (releaseTime >= 2f && currentSpeedLevel > 0)
                {
                    currentSpeedLevel--;
                    releaseTime = 0f;
                }
                if (currentSpeedLevel == 0)
                    isMoving = false;
            }
            holdTime = 0f;
            canShiftGear = false;
        }

        if (Input.GetKeyDown(KeyCode.Q) && currentSpeedLevel < speedLevels.Length - 1 && canShiftGear)
        {
            currentSpeedLevel++;
            canShiftGear = false;
            holdTime = 0f;
        }

        if (Input.GetKey(KeyCode.S) && isMoving)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            isMoving = false;
            RotateWheels(0);
            return;
        }

        if (Input.GetKey(KeyCode.S) && !isMoving)
        {
            isReversing = true;
            currentSpeedLevel = 0;
            Vector3 move = -transform.forward * reverseSpeed;
            rb.MovePosition(rb.position + move * Time.deltaTime);
            RotateWheels(reverseSpeed);
            return;
        }

        if (isReversing && !Input.GetKey(KeyCode.S))
        {
            isReversing = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            RotateWheels(0);
        }

        if ((uartAvanceActivo && !uartReversaActiva) || (isMoving && !isReversing))
        {
            Vector3 move = transform.forward * speedLevels[currentSpeedLevel];
            rb.MovePosition(rb.position + move * Time.deltaTime);
            RotateWheels(speedLevels[currentSpeedLevel]);
        }
        else if (uartReversaActiva && !uartAvanceActivo && !isMoving)
        {
            isReversing = true;
            Vector3 move = -transform.forward * reverseSpeed;
            rb.MovePosition(rb.position + move * Time.deltaTime);
            RotateWheels(reverseSpeed);
        }
        else
        {
            RotateWheels(0);
        }

        if (uartIzquierdaActiva)
        {
            transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
            SteerWheels(-1f);
        }
        else if (uartDerechaActiva)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            SteerWheels(1f);
        }

        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotateY * rotationSpeed * Time.deltaTime, 0));
        SteerWheels(rotateY);
    }

    void FixedUpdate()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, 1.5f))
        {
            rb.AddForce(Vector3.down * gravityForce, ForceMode.Acceleration);
        }
    }

    void RotateWheels(float speed)
    {
        if (speed == 0) return;

        float rotationAmount = speed * 300 * Time.deltaTime;
        Vector3 rearWheelAxis = Vector3.forward;
        Vector3 frontWheelAxis = Vector3.right;

        if (pivotLlantaTrasera1 != null)
        {
            pivotLlantaTrasera1.Rotate(rearWheelAxis * rotationAmount, Space.Self);
        }

        foreach (Transform wheel in frontWheels)
        {
            wheel.Rotate(frontWheelAxis * rotationAmount, Space.Self);
        }
    }

    void SteerWheels(float rotateY)
    {
        float turnAngle = rotateY * 25f;

        foreach (Transform wheel in steeringWheels)
        {
            Quaternion targetRotation = Quaternion.Euler(0, turnAngle, 0);
            wheel.localRotation = Quaternion.Lerp(wheel.localRotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    // UART control
    public void AvanzarDesdeUART()
    {
        uartAvanceActivo = true;
        uartReversaActiva = false;
        isReversing = false;
        isMoving = true;

        // Solo cambia si estaba en 0
        if (currentSpeedLevel == 0)
        {
            currentSpeedLevel = 1;
        }

        Debug.Log($"🚜 Avanzando UART. Velocidad: {currentSpeedLevel}");
    }

    public void DetenerDesdeUART()
    {
        uartAvanceActivo = false;
        uartReversaActiva = false;
        isMoving = false;
        isReversing = false;

        currentSpeedLevel = 0; // ← Reinicia velocidad al frenar

        Debug.Log("🛑 Avance y reversa UART detenidos");
    }

    public void RetrocederDesdeUART()
    {
        uartReversaActiva = true;
        uartAvanceActivo = false;
        isMoving = false;
        
        currentSpeedLevel = 1; // <- Siempre que reverses, mostrar velocidad 1
        
        Debug.Log("🔄 Reversa UART activada");
    }

    public void DetenerReversaDesdeUART()
    {
        uartReversaActiva = false;
        isReversing = false;
        Debug.Log("⛔ Reversa UART detenida");
    }

    public void GirarIzquierdaUART()
    {
        uartIzquierdaActiva = true;
        uartDerechaActiva = false;
    }

    public void GirarDerechaUART()
    {
        uartDerechaActiva = true;
        uartIzquierdaActiva = false;
    }

    public void DetenerGiroUART()
    {
        uartIzquierdaActiva = false;
        uartDerechaActiva = false;
    }

   public void AumentarVelocidadDesdeUART()
    {
        if (uartAvanceActivo && currentSpeedLevel < speedLevels.Length - 1)
        {
            currentSpeedLevel++;
            Debug.Log("✅ Velocidad aumentada desde UART: " + currentSpeedLevel);
        }
        else
{
    if (!uartAvanceActivo)
        Debug.Log("⛔ No se puede subir: uartAvanceActivo está en FALSE");
    else if (currentSpeedLevel >= speedLevels.Length - 1)
        Debug.Log("⛔ No se puede subir: ya estás en la velocidad máxima (" + currentSpeedLevel + ")");
}
    }

    public void ActivarCambioDeMarchaUART()
    {
        canShiftGear = true;
        Debug.Log("🕒 Cambio de marcha habilitado por UART");
    }



    public int MarchaVisual
{
    get
    {
        Debug.Log($"Marcha visual: {currentSpeedLevel} (modo visual)");

        if (!uartAvanceActivo && !uartReversaActiva && !isMoving && !isReversing)
            return 0;

        if (uartReversaActiva)
            return 1;

        if (uartAvanceActivo)
            return Mathf.Max(1, currentSpeedLevel);

        return 0;
    }
}



    public void DetenerTodoDesdeUART()
    {
        uartAvanceActivo = false;
        uartReversaActiva = false;
        isMoving = false;
        isReversing = false;

        Debug.Log("🛑 Todo detenido por comando 0x00");
    }

    public void DesactivarCambioDeMarchaUART()
    {
        canShiftGear = false;
    }


}




