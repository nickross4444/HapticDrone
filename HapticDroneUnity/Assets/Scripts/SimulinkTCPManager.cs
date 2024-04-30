using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SimulinkTCPManager : MonoBehaviour
{
    public Telemetry telem;
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    int port;// = 18000;
    public TMPro.TMP_InputField portInputField;
    public Button networkModeBtn;
    bool recievedData = false;
    void Start()
    {
        //ConnectToTcpServer();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (clientReceiveThread != null && clientReceiveThread.IsAlive)
        {
            SendOutput();
        }
        if (recievedData)
            networkModeBtn.interactable = true;
        else
            networkModeBtn.interactable = false;
    }
    /// <summary> 	
    /// Setup socket connection. 	
    /// </summary> 	
    public void ConnectToTcpServer()
    {
        port = int.Parse(portInputField.text);
        try
        {
            if (clientReceiveThread != null && clientReceiveThread.IsAlive) clientReceiveThread.Abort();     //stop if started
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
            Debug.Log("Connected to Port: " + port);
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
        
    }
    /// <summary> 	
    /// Runs in background clientReceiveThread; Listens for incomming data. 	
    /// </summary>     
    private void ListenForData()
    {
        try
        {
            socketConnection = new TcpClient("localhost", port);
            //networkModeBtn.interactable = true;
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        try
                        {
                            var incommingData = new byte[length];
                            Array.Copy(bytes, 0, incommingData, 0, length);
                            //each single is 4 bytes. Start at index 0 for first byte, 4 for second, etc.
                            for (int i = 0; i < telem.inputs.Length; i++)
                            {
                                telem.inputs[i] = BitConverter.ToSingle(incommingData, 4 * i);
                            }
                            recievedData = true;
                        }
                        catch
                        {
                            Debug.LogError("failed to read data, not enough bytes. Byte count = " + length + ". Looking for 108");
                        }
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
            //networkModeBtn.interactable = false;
        }
    }
    private void SendOutput()
    {
        // SendFloat send outputs based on telemetry class
        foreach (float output in telem.outputs)
        {
            SendFloat(output);
        }
    }
    /// <summary> 	
    /// Send message to server using socket connection. 	
    /// </summary> 	
    private void SendFloat(float f)
    {
        if (socketConnection == null)
        {
            return;
        }
        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                byte[] byteArr = BitConverter.GetBytes(f);
                stream.Write(byteArr, 0, byteArr.Length);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}
