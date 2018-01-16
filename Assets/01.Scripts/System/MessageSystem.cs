using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSystem
{
    //Singleton
    static MessageSystem _instance;
    public static MessageSystem Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new MessageSystem();
            }
            return _instance;
        }
    }

    Queue<MessageParam> _messageQueue = new Queue<MessageParam>();
    //Message
    public void Send(MessageParam msgParam)
    {
        _messageQueue.Enqueue(msgParam);
    }
    public void ProcessMessage()
    {
        while (0 != _messageQueue.Count)
        {
            // ReceiveObjectMessage
            MessageParam msgParam = _messageQueue.Dequeue();
            msgParam.receiver.ReceiveObjectMessage(msgParam);
        }
    }
}
