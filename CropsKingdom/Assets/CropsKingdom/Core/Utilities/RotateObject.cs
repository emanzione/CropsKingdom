using UnityEngine;

namespace CropsKingdom.Core.Utilities
{
    public class RotateObject : MonoBehaviour
    {
        public float RotationSpeed;

        protected void Update()
        {
            //var rotation = transform.rotation.eulerAngles;
            var rotation = new Vector3();
            rotation.x = 0;
            rotation.z = RotationSpeed * Time.deltaTime;
            rotation.y = 0;
            
            transform.Rotate(rotation);
        }
    }
}