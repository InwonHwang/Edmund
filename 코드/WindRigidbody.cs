using UnityEngine;
using System.Collections;

public class WindRigidbody : MonoBehaviour {

    private float windStrength = 4;
    private float radius = 1234567;
    private int i;
    public float windStrengthMin = 0;
    public float windStrengthMax = 5;
    public Transform windTransformPosition;
    public Transform windTransformRotation;

    void Update()
    {
        if (windTransformPosition != null && windTransformRotation != null)
        {
            windStrength = Random.Range(windStrengthMin, windStrengthMax);
            windTransformRotation.rotation = transform.rotation;

            var hitColliders = Physics.OverlapSphere(windTransformPosition.transform.position, radius);
            for (i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].GetComponent<Rigidbody>() != null)
                {
                    RaycastHit hit;
                    var rayDirection = hitColliders[i].GetComponent<Rigidbody>().gameObject.transform.position - windTransformPosition.transform.position;
                    if (Physics.Raycast(windTransformPosition.transform.position, rayDirection, out hit)) //there was ',hit, 100' is from an old test.
                    {
                        if (hit.transform.GetComponent<Rigidbody>())
                        {
                            //AddExplosionForce(512, transform.position, radius, 3.0); //More garbage from old tests . . . 
                            hitColliders[i].GetComponent<Rigidbody>().AddForce(windTransformPosition.transform.forward * windStrength, ForceMode.Acceleration);
                            //there was '32' instead of windStrength //just a note for myself.
                        }
                    }
                }
            }
        }
    }

    
}
