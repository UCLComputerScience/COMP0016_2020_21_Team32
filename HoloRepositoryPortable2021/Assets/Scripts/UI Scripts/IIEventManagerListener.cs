using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Interface implemented by all classes that subscribe to any number of the events that the EventManager fires.</summary>
public interface IEventManagerListener
{
    void subscribeToEvents();
}
