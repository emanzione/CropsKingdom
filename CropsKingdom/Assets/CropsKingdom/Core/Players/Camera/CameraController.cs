using System.Runtime.CompilerServices;
using UnityEngine;

namespace CropsKingdom.Core.Players.Camera
{
    public class CameraController : MonoBehaviour
    {
        public float PanSpeed;
        public float PanBorderThreshold;
        public Vector2 PanLimit;
        public float ZoomSpeed;
        public float ZoomLimit;
        
        protected void Update()
        {
            var currentPosition = transform.position;

            if (CanMoveUp())
            {
                currentPosition.z += PanSpeed * Time.deltaTime;
            }
            else if (CanMoveDown())
            {
                currentPosition.z -= PanSpeed * Time.deltaTime;
            }
            
            if (CanMoveLeft())
            {
                currentPosition.x -= PanSpeed * Time.deltaTime;
            }
            else if (CanMoveRight())
            {
                currentPosition.x += PanSpeed * Time.deltaTime;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            currentPosition.y -= scroll * ZoomSpeed * Time.deltaTime * 100f;

            currentPosition.x = Mathf.Clamp(currentPosition.x, -PanLimit.x, PanLimit.x);
            currentPosition.y = Mathf.Clamp(currentPosition.y, 0, ZoomLimit);
            currentPosition.z = Mathf.Clamp(currentPosition.z, -PanLimit.y, PanLimit.y);

            transform.position = currentPosition;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanMoveUp()
        {
            return Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - PanBorderThreshold;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanMoveDown()
        {
            return Input.GetKey(KeyCode.S) || Input.mousePosition.y <= PanBorderThreshold;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanMoveLeft()
        {
            return Input.GetKey(KeyCode.A) || Input.mousePosition.x <= PanBorderThreshold;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CanMoveRight()
        {
            return Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - PanBorderThreshold;
        }
    }
}