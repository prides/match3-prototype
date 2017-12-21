using UnityEngine;

namespace Utils
{
    public class DestroyComponent : MonoBehaviour
    {
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}