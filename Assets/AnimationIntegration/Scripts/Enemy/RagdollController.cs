using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagdollController : MonoBehaviour
{    
    private List<Rigidbody> _ragdollComponents;

    private void Awake()
    {
        _ragdollComponents = GetComponentsInChildren<Rigidbody>().ToList();
    }

    public void ToggleRagdoll(bool state)
    {
        foreach (var component in _ragdollComponents)
            component.isKinematic = !state;
    }
}
