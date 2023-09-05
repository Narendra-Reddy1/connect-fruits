using UnityEngine;

public class DestroyInProductionBuild : MonoBehaviour
{
#if !(DEBUG_DEFINE || DEVLOPMENT_BUILD)
    private void Awake()
    {
        Destroy(gameObject);
    }
#endif
}
