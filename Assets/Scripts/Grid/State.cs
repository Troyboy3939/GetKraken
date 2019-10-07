using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    Vector3 m_v3Position;
    public abstract void OnEnter();

    public abstract void OnExit();


    public abstract void Update();


}
