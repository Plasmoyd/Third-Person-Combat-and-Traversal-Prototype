using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Start();

    public abstract void Tick(float deltaTime);

    public abstract void Exit();
}
