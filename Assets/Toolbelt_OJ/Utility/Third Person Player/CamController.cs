using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolbelt_OJ
{
    public class CamController : MonoBehaviour
    {
        public Transform target;

        public Vector3 offset;
        public bool useOffsetValue;
        public float rotateSpeed;
        public Transform pivot;

        public float minViewAngle;
        public float maxViewAngle;

        public bool invertY;

        // Start is called before the first frame update
        void Start()
        {
            if (!useOffsetValue)
            {
                offset = target.position - transform.position;
            }

            pivot.transform.position = target.transform.position;
            pivot.transform.parent = target.transform;

            Cursor.lockState = CursorLockMode.Confined;
        }

        // Update is called once per frame
        void LateUpdate()
        {

            // get x position of the mouse & rotate the target
            float horizontal = Input.GetAxisRaw("Mouse X") * rotateSpeed;
            target.Rotate(0, horizontal, 0);

            // get y position of the mouse and rotate the pivot
            float vertical = Input.GetAxisRaw("Mouse Y") * rotateSpeed;

            //pivot.Rotate(vertical, 0, 0);
            if (invertY)
            {
                pivot.Rotate(vertical, 0, 0);
            }
            else pivot.Rotate(-vertical, 0, 0);

            //Limit up/down camera rotation
            if (pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180f)
            {
                pivot.rotation = Quaternion.Euler(maxViewAngle, pivot.rotation.eulerAngles.y, pivot.rotation.eulerAngles.z);
            }

            if (pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 360f + minViewAngle)
            {
                pivot.rotation = Quaternion.Euler(360f + minViewAngle, pivot.rotation.eulerAngles.y, pivot.rotation.eulerAngles.z);
            }

            //move the cam based on the current rotation of the target and the original offset
            float desiredYAngle = target.eulerAngles.y;
            float desiredXAngle = pivot.eulerAngles.x;

            Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
            transform.position = target.position - (rotation * offset);

            // transform.position = target.position - offset;

            if (transform.position.y < target.position.y)
            {
                transform.position = new Vector3(transform.position.x, target.position.y - 0.5f, transform.position.z);
            }

            transform.LookAt(target);

        }
    }
}
