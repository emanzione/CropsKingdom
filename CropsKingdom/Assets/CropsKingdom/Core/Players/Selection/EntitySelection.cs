using UnityEngine;
using CropsKingdom.Core.Entities;
using CropsKingdom.Core.Networking;

namespace CropsKingdom.Core.Players.Selection
{
    public class EntitySelection : MonoBehaviour
    {
        public GameObject SelectedPointer;
        public NetworkRemoteEntity SelectedEntity;

        protected void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hitInfo))
                {
                    if (hitInfo.transform != null)
                    {
                        SelectedEntity = hitInfo.transform.gameObject.GetComponent<NetworkRemoteEntity>();
                    }
                    else
                    {
                        SelectedEntity = null;
                    }
                }
                else
                {
                    SelectedEntity = null;
                }
                
                if (SelectedEntity != null)
                {
                    Debug.Log("Entity hit! " + SelectedEntity.name);
                    var selectorPosition = SelectedPointer.transform.position;
                    var entityTransform = SelectedEntity.transform;
                    var entityPosition = entityTransform.position;
                    selectorPosition.x = entityPosition.x;
                    selectorPosition.z = entityPosition.z;
                    SelectedPointer.transform.position = selectorPosition;
                    SelectedPointer.transform.localScale = entityTransform.localScale;
                    
                    SelectedPointer.gameObject.SetActive(true);
                }
                else
                {
                    Debug.Log("Nothing hit.");
                    SelectedPointer.gameObject.SetActive(false);
                    SelectedPointer.transform.localScale = Vector3.one;
                }
            }
        }
    }
}