using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BloodRandomizer : MonoBehaviour
{
    [SerializeField] private List<Material> materials = new List<Material>();
    [SerializeField] private Renderer rend;

    private void OnEnable()
    {
        rend.material = materials[Random.Range(0, materials.Count)];
    }
}