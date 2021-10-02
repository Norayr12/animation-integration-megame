using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [Header("Ragdoll settings")]
    [SerializeField] private List<Rigidbody> _ragdollComponents;

    public void SetRagdoll(bool state)
    {
        foreach (var component in _ragdollComponents)
            component.isKinematic = !state;
    }
}
