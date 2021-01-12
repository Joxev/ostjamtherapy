using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;
    public void OnEventRaised()
    {
        Response.Invoke();
    }
    void OnEnable()
    {
        Event.Register(this);
    }
    void OnDisable()
    {
        Event.DeRegister(this);
    }
}