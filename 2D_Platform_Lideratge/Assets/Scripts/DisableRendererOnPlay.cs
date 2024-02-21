using UnityEngine;

public class DisableRendererOnPlay : MonoBehaviour
{

    private void Awake()
    {
        if(TryGetComponent(out SpriteRenderer renderer))
        {
            renderer.enabled = false;
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
