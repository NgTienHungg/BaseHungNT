using UnityEngine;

namespace WingsMob.HungNT
{
    public class MonoDebug : MonoBehaviour
    {
        protected void OnEnable()
        {
#if !DEBUG
            gameObject.SetActive(false);
#endif
        }
    }
}