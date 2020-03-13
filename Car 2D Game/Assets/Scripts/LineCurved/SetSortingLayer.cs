using UnityEngine;

[ExecuteInEditMode]
public class SetSortingLayer : MonoBehaviour
{
    public Renderer MyRenderer;
    public string MySortingLayer;
    public int MySortingOrderInLayer;

    private void Start()
    {
        if (MyRenderer == null)
            MyRenderer = this.GetComponent<Renderer>();
    }

    private void Update()
    {
        if (MyRenderer == null)
            MyRenderer = this.GetComponent<Renderer>();
        MyRenderer.sortingLayerName = MySortingLayer;
        MyRenderer.sortingOrder = MySortingOrderInLayer;
    }
}
