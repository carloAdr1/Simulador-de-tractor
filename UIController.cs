using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO.Ports;

public class UIController : MonoBehaviour
{
    private SerialPort serialPort;
    private PlayerMove playerMove;
    private bool ignorarSiguienteCero = false;
    private bool ignorarSiguienteFreno = false;
    private bool sw8Held = false;
    private bool uartListo = false;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"📦 Escena cargada: {scene.name}");
        if (scene.name == "main simulation")  // Asegúrate que este sea el nombre exacto
        {
            StartCoroutine(EsperarYConectarUART());
        }
    }

    IEnumerator EsperarYConectarUART()
    {
        yield return new WaitUntil(() =>
            SerialSender.instance != null &&
            SerialSender.instance.GetSerialPort()?.IsOpen == true &&
            FindObjectOfType<PlayerMove>() != null);

        serialPort = SerialSender.instance.GetSerialPort();
        playerMove = FindObjectOfType<PlayerMove>();

        if (playerMove == null)
            Debug.LogError("❌ NO se encontró PlayerMove en escena.");
        else
            Debug.Log("✅ playerMove asignado correctamente.");

        uartListo = true;
        Debug.Log("✅ UART conectado correctamente desde UIController.");
    }

    public void Reiniciar()
    {
        Debug.Log("🔄 Reiniciando asignación de PlayerMove...");

        StartCoroutine(EsperarYConectarUART());
    }

    void Update()
    {
        // 🧠 Si el player fue destruido por cambio de escena, volvemos a buscarlo
        if (playerMove == null)
        {
            playerMove = FindObjectOfType<PlayerMove>();
            if (playerMove != null)
            {
                Debug.Log("✅ playerMove reasignado dinámicamente.");
                uartListo = true;
            }
            else
            {
                Debug.LogWarning("⚠️ playerMove no está asignado.");
                return;
            }
        }

        if (!uartListo || serialPort == null || !serialPort.IsOpen)
            return;

        try
        {
            if (serialPort.BytesToRead > 0)
            {
                int data = serialPort.ReadByte();

                // Solo procesar si el comando es uno de los válidos
                if (data == 0x02 || data == 0x03 || data == 0x04 || data == 0x05 ||
                    data == 0x08 || data == 0x0A || data == 0x0C || data == 0x0D ||
                    data == 0x0E || data == 0x0F || data == 0x14 || 
                    data == 0x20 || data == 0x21 || data == 0x22 || data == 0x00)
                {

                playerMove.DetenerGiroUART(); // Siempre primero

                switch(data)
                {
                    case 0x02:
                        Debug.Log("Avanzar");
                        playerMove.AvanzarDesdeUART();
                        break;
                    case 0x03:
                        Debug.Log("Retroceder");
                        playerMove.RetrocederDesdeUART();
                        break;
                    case 0x04:
                        Debug.Log("Detener avance");
                        playerMove.DetenerDesdeUART();
                        break;
                    case 0x05:
                        Debug.Log("Detener retroceso");
                        playerMove.DetenerReversaDesdeUART();
                        break;
                    case 0x08:
                        Debug.Log("Girar izquierda");
                        playerMove.GirarIzquierdaUART();
                        break;
                    case 0x0A:
                        Debug.Log("Girar derecha");
                        playerMove.GirarDerechaUART();
                        break;
                    case 0x14:
                        Debug.Log("⚠️ Conflicto + giro");
                        playerMove.DetenerDesdeUART();
                        playerMove.DetenerReversaDesdeUART();
                        break;
                    case 0x0C:
                        Debug.Log("🚜⬅️ Avanzar + izquierda");
                        playerMove.AvanzarDesdeUART();
                        playerMove.GirarIzquierdaUART();
                        break;
                    case 0x0D:
                        Debug.Log("🚜➡️ Avanzar + derecha");
                        playerMove.AvanzarDesdeUART();
                        playerMove.GirarDerechaUART();
                        break;
                    case 0x0E:
                        Debug.Log("🔄⬅️ Reversa + izquierda");
                        playerMove.RetrocederDesdeUART();
                        playerMove.GirarIzquierdaUART();
                        break;
                    case 0x0F:
                        Debug.Log("🔄➡️ Reversa + derecha");
                        playerMove.RetrocederDesdeUART();
                        playerMove.GirarDerechaUART();
                        break;
                    case 0x20:
                        if (!sw8Held)
                        {
                            Debug.Log("⚡ Cambio de velocidad UART recibido (simula Q) — NUEVO CAMBIO");
                            playerMove.AumentarVelocidadDesdeUART();
                            sw8Held = true;
                        }
                        else
                        {
                            Debug.Log("⚡ Cambio de velocidad UART recibido (simula Q) — ignorado por estar mantenido");
                        }
                        break;
                    case 0x00:
                        Debug.Log("🛑 Comando 0x00 — detener el tractor (por SW2, freno, etc)");
                        playerMove.DetenerDesdeUART();
                        playerMove.DetenerReversaDesdeUART();
                        playerMove.DetenerTodoDesdeUART();
                        break;
                    case 0x21:
                        Debug.Log("🛑 Freno total activado");
                        playerMove.DetenerDesdeUART();
                        playerMove.DetenerReversaDesdeUART();
                        playerMove.DetenerGiroUART();
                        break;
                    case 0x22:
                        sw8Held = false;
                        Debug.Log("🔄 SW8 liberado — cambio permitido nuevamente");
                        break;
                    default:
                        Debug.LogWarning("⚠️ Comando UART no reconocido: " + data.ToString("X2"));
                        break;
                    }
                }
                else
                {
                Debug.LogWarning("⚠️ UART ignorado: " + data.ToString("X2"));
             }
         }
      }
        catch (System.TimeoutException)
        {
            // Ignorar timeout
        }
    }
}



