using System;
using System.Collections;

public interface IStateMachine : IDisposable
{
    public ArrayList Save();
    public void Load(ArrayList data);
}
