using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject {

    object sender;
    static readonly List<Observer> Observers = new List<Observer>();
    public Subject(object sender) {
        this.sender = sender;
    }

    public static void AddObserver(Observer observer)
    {
        Observers.Add(observer);   
    }

    public static void RemoveObserver(Observer observer)
    {
        Observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in Observers)
        {
            observer.SubjectUpdate(sender);
        }
    }
    /**
        Todo:
            Implement methods: 
                * AddObserver - Used by observers to register to this subject
                * RemoveObserver - Used by observers to deregister from this subject
                * Notify - Used by owner of this subject to notify observers, that something has happened.   
        */  
    
}