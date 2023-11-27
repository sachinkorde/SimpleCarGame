using System.Collections;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    [SerializeField] private float scroll_speed = 0.1f;
    [SerializeField] private MeshRenderer bgMesh;
    float x_scroll;
    Vector2 offset;

    private void Awake()
    {
        x_scroll = 0;
        bgMesh = GetComponent<MeshRenderer>();   
    }

    private void Start()
    {
        StartCoroutine(Scroll());
    }

    IEnumerator Scroll()
    {
        if(x_scroll >  10)
        {
            x_scroll = 0;
        }
        x_scroll += Time.deltaTime * scroll_speed;
        offset = new Vector2(0f, x_scroll);
        bgMesh.sharedMaterial.SetTextureOffset("_MainTex", offset);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Scroll());
    }
}
