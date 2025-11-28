using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public int cubesPerAxis = 5;
    public float force = 300f;
    public float radius = 2f;
    public bool startExplosion;

    void Update()
    {
        if (startExplosion)
        {
            // make an 5 x 5 x 5 cube made out of smaller cubes
            for (int x = 0; x < cubesPerAxis; x++)
            {
                for (int y = 0; y < cubesPerAxis; y++)
                {
                    for (int z = 0; z < cubesPerAxis; z++)
                    {
                        CreateCube(new Vector3(x, y, z));
                    }
                }
            }

            Destroy(gameObject);
        }
    }

    void CreateCube(Vector3 coordinates)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        Renderer rd = cube.GetComponent<Renderer>();
        rd.material = GetComponent<Renderer>().material;

        cube.transform.localScale = transform.localScale / cubesPerAxis;
        
        Vector3 firstCube = transform.position - transform.localScale / 2 + cube.transform.localScale / 2;
        cube.transform.position = firstCube + Vector3.Scale(coordinates, cube.transform.localScale);

        Rigidbody rb = cube.AddComponent<Rigidbody>();
        rb.AddExplosionForce(force, transform.position, radius);

        // set tag as hazard, not pickup
        cube.tag = "Hazard";

        // coroutine to delete cubes after 3 seconds
        cube.AddComponent<CubeLifetime>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            startExplosion = true;
        }
    }
}
