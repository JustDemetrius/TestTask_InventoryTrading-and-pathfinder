using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    public virtual void Awake()
    {
        AddToContainer();
    }

    protected virtual void AddToContainer()
    {

    }

    protected virtual void RemoveFromContainer()
    {

    }

    public virtual void Init()
    {

    }

    public virtual void OnDestroy()
    {
        RemoveFromContainer();
    }
}
