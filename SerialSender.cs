using UnityEngine;
using System.IO.Ports;

public class SerialSender : MonoBehaviour
{
    public static SerialSender instance;
    private SerialPort serialPort;
    public string portName = "COM4";
    public int baudRate = 115200;

    public SerialPort GetSerialPort()
    {
        return serialPort;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("🔧 SerialSender Start() ejecutado.");

        try
        {
            if (serialPort == null)
            {
                Debug.Log($"📡 Intentando abrir puerto {portName} a {baudRate}...");
                serialPort = new SerialPort(portName, baudRate);
                serialPort.Open();

                Debug.Log("✅ Puerto UART abierto correctamente.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("❌ Error al abrir el puerto UART: " + ex.Message);
        }
    }

    public void EnviarValor(int valor)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            byte data = (byte)(valor & 0xFF); // Solo 8 bits
            serialPort.Write(new byte[] { data }, 0, 1);
            Debug.Log("📤 Enviado al FPGA: " + data.ToString("X"));
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }

    public void CerrarPuerto()
    {
        if (serialPort != null)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                Debug.Log("🔌 Puerto cerrado correctamente.");
            }
            serialPort.Dispose();
            serialPort = null;
        }
    }
}

