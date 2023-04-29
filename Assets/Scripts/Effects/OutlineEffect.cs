using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineEffect : MonoBehaviour {
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private float outlineScaleFactor;
    [SerializeField] private Color outlineColor;
    private Renderer outlineRenderer;

    void Start() {
        outlineRenderer = CreateOutline(outlineMaterial, outlineScaleFactor, outlineColor);
    }

    Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color) {
        GameObject outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation, transform);
        
        Renderer rend = outlineObject.GetComponent<Renderer>();
        rend.material = outlineMat;
        rend.material.SetColor("_OutlineColor", color);
        rend.material.SetFloat("_ScaleFactor", scaleFactor);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        
        outlineObject.GetComponent<OutlineEffect>().enabled = false;
        outlineObject.GetComponent<Collider>().enabled = false;
        
        rend.enabled = false;
        return rend;
    }

    public void OnMouseEnter() {
        outlineRenderer.enabled = true;
    }

    public void OnMouseOver() {
        transform.Rotate(Vector3.up, 1f, Space.World);
    }

    public void OnMouseExit() {
        outlineRenderer.enabled = false;
    }
}