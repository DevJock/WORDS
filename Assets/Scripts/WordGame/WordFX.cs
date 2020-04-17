using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordFX : MonoBehaviour
{
    public float explosiveForce;
    public float explosiveRadius;
    public float destroyTimePeriod;
    public GameObject explosion;
    public GameObject smoke;

    public float implodeSpeed;
    GameObject[] letters;

    private void Start()
    {
        letters = new GameObject[transform.childCount];
        for(int i=0;i<transform.childCount;i++)
        {
            letters[i] = transform.GetChild(i).gameObject;
        }
    }

    public void Implode()
    {
        Destroy(gameObject.GetComponent<Move>());
        Destroy(gameObject.GetComponent<Rigidbody>());
        foreach(GameObject letter in letters)
        {
            Abducter abduct = letter.AddComponent<Abducter>();
            abduct.endMarker = gameObject.transform;
            abduct.speed = implodeSpeed;
        }
        Destroy(gameObject, 0.1f *letters.Length);
    }

    public void Explode()
    {
        Rigidbody[] bodies = new Rigidbody[letters.Length];
        for(int i=0;i<bodies.Length;i++)
        {
            bodies[i] = transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            bodies[i].useGravity = false;
            bodies[i].mass = 10;
            bodies[i].drag = 1;
            bodies[i].angularDrag = 1;
        }
        Destroy(gameObject.GetComponent<Move>());
        Destroy(gameObject.GetComponent<Rigidbody>());
        GameObject explodeObj = Instantiate(explosion) as GameObject;
        explodeObj.name = gameObject.name + "_explodeFX";
        explodeObj.transform.parent = gameObject.transform;
        explodeObj.transform.localPosition = Vector3.zero;
        foreach(Rigidbody body in bodies)
        {
            Vector3 explosivePosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 0.05f);
            body.AddExplosionForce(explosiveForce,explosivePosition, explosiveRadius,Random.Range(0,1),ForceMode.Impulse);
           body.AddForceAtPosition(new Vector3(Random.Range(-explosiveForce, explosiveForce), Random.Range(-explosiveForce, explosiveForce)),new Vector3(body.transform.localPosition.x, body.transform.localPosition.y+0.05f, body.transform.localPosition.z),ForceMode.Impulse);
            GameObject smokeFX = Instantiate(smoke) as GameObject;
            smokeFX.name = body.name + "_smokeFX";
            smokeFX.transform.parent = body.transform;
            smokeFX.transform.localPosition = Vector3.zero;
            body.gameObject.AddComponent<Move>().speed = 0.001f;
            Destroy(body.gameObject, Random.Range(0.5f,destroyTimePeriod));
        }
        Destroy(gameObject,destroyTimePeriod);
    }

}
