using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Toolbelt_OJ
{
    public class ObjectSelection : MonoBehaviour
    {
        public KeyCode selectInput = KeyCode.E;

        public float reachDistance = 5f;

        public Transform rayOrigin, carryPos, highlightedTarget, selectedTarget;

        public List<int> keyLayerIndexList;
        int layerMask;

        UnityEvent selectItemEvent, selectNPCEvent;

        [SerializeField] private StartDialogue initiateDialogue;
        void Start()
        {
            selectItemEvent.AddListener(SelectItem);
            //selectNPCEvent.AddListener(SelectNPC);
        }


        void Update()
        {
            foreach (int layerIndex in keyLayerIndexList)
            {
                layerMask = 1 << layerIndex;
            }

            RaycastHit hit;

            if (Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, reachDistance, layerMask))
            {
                highlightedTarget = hit.transform;



                if (Input.GetKeyDown(selectInput))
                {

                    selectItemEvent.Invoke();

                    //if (selectedTarget.GetComponent<NPCBrain>())
                    //{
                    //    selectNPCEvent.Invoke();
                    //}
                    ////else if (selectedTarget.GetComponent<ItemInWorld>())
                    ////{
                    ////    selectItemEvent.Invoke();
                    ////}
                }
            }
        }

        void SelectItem()
        {
            if (selectedTarget != null && selectedTarget.GetComponent<ItemInWorld>())
            {
                //selectedTarget.GetComponent<Rigidbody>().isKinematic = false;
                selectedTarget.GetComponent<Rigidbody>().useGravity = true;

                selectedTarget.parent = null;

                selectedTarget = null;
            }

            highlightedTarget = selectedTarget;

            //selectedTarget.GetComponent<Rigidbody>().isKinematic = true;
            selectedTarget.GetComponent<Rigidbody>().useGravity = false;
            selectedTarget.parent = carryPos;
            selectedTarget.transform.position = carryPos.position;
        }

        //void SelectNPC()
        //{
        //    if (selectedTarget != null && selectedTarget.GetComponent<NPCBrain>())
        //    {
        //        if (selectedTarget.GetComponent<NPCBrain>().npcInfo != null)
        //        {
        //            initiateDialogue.EnterDialogue(selectedTarget.GetComponent<NPCBrain>().npcInfo);
        //        }
        //    }
        //}

    }
}
