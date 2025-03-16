using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class PlayerMove : MonoBehaviour
{
    public float[] speedLevels = { 3f, 5f, 7f, 9f, 12f }; // Velocidades por nivel
    public int currentSpeedLevel = 0; // Nivel de velocidad actual (0 a 4)
    public float holdTime = 0f; // Tiempo de presión de W para cambio de marcha
    public float releaseTime = 0f; // Tiempo sin presionar W para bajar de marcha
    public bool isMoving = false; // Indica si el tractor está en movimiento
    private bool canShiftGear = false; // Controla si se puede presionar Q para subir de marcha
    public bool isReversing = false; // Indica si está en reversa

    public float rotationSpeed = 150f;
    public Transform pivotLlantaTrasera1;
    public Transform pivotLlantaTrasera2;
    public Transform[] frontWheels;
    public Transform[] steeringWheels;

    private Rigidbody rb;
    public float gravityForce = 60f;
    private float reverseSpeed = 5f; // Velocidad fija en reversa

    void Start()
    {
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
        float moveZ = Input.GetAxis("Vertical"); // Adelante y atrás (-1 cuando presionas S)
        float rotateY = Input.GetAxis("Horizontal");

        // Freno con Shift (reinicia la velocidad a 3)
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            isMoving = false;
            isReversing = false;
            currentSpeedLevel = 0; // Reiniciar a velocidad más baja (3)
            holdTime = 0f;
            releaseTime = 0f;
            canShiftGear = false;
            return;
        }

        // Detenerse si presiono W y S al mismo tiempo
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            isMoving = false;
            isReversing = false;
            RotateWheels(0); // Mantener ruedas girando sin moverse
            return;
        }

        // Mantener W para acumular tiempo de subida de marcha
        if (Input.GetKey(KeyCode.W))
        {
            isMoving = true;
            isReversing = false;
            holdTime += Time.deltaTime;
            releaseTime = 0f;

            if (holdTime >= 2f)
            {
                canShiftGear = true;
            }
        }
        else
        {
            if (isMoving)
            {
                releaseTime += Time.deltaTime;
                if (releaseTime >= 2f && currentSpeedLevel > 0)
                {
                    currentSpeedLevel--;
                    releaseTime = 0f;
                }
                if (currentSpeedLevel == 0)
                {
                    isMoving = false;
                }
            }
            holdTime = 0f;
            canShiftGear = false;
        }

        // Cambio manual de marcha con Q (solo si han pasado 2s con W)
        if (Input.GetKeyDown(KeyCode.Q) && currentSpeedLevel < speedLevels.Length - 1 && canShiftGear)
        {
            currentSpeedLevel++;
            canShiftGear = false;
            holdTime = 0f;
        }

        // Si presiono S mientras avanzo, el carro se detiene en lugar de retroceder
        if (Input.GetKey(KeyCode.S) && isMoving)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            isMoving = false;
            RotateWheels(0);
            return;
        }

        // Retroceder con S (y reiniciar la velocidad a 1)
        if (Input.GetKey(KeyCode.S) && !isMoving)
        {
            isReversing = true;
            currentSpeedLevel = 0; // 🚨 Aquí se reinicia la velocidad a 1 (mínima marcha)
            Vector3 move = -transform.forward * reverseSpeed;
            rb.MovePosition(rb.position + move * Time.deltaTime);
            RotateWheels(reverseSpeed);
            return;
        }

        // 🚨 SOLUCIÓN: DETENER MOVIMIENTO AL SOLTAR 'S'
        if (isReversing && !Input.GetKey(KeyCode.S))
        {
         isReversing = false;
         rb.linearVelocity = Vector3.zero; // Detener el Rigidbody
          rb.angularVelocity = Vector3.zero;
           RotateWheels(0);
        }

        // Aplicar movimiento hacia adelante si está en marcha
        if (isMoving)
        {
            Vector3 move = transform.forward * speedLevels[currentSpeedLevel];
            rb.MovePosition(rb.position + move * Time.deltaTime);
            RotateWheels(speedLevels[currentSpeedLevel]);
        }
        else
        {
            RotateWheels(0); // Si no se mueve, las ruedas no giran
        }

        // Rotación del tractor
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotateY * rotationSpeed * Time.deltaTime, 0));

        // Rotar ruedas delanteras
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
        if (speed == 0) return; // Si no hay movimiento, no rotar ruedas

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
}
