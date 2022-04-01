using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thumbHandler : MonoBehaviour
{
    public SphereCollider col;
    public bool touching = false;

    private Rigidbody r;
    private GameObject index;
    private Material mat;

    // Start is called before the first frame update
    void Start()
    {   
        r = gameObject.AddComponent<Rigidbody>();
        r.useGravity = false;
        r.mass = 0;
        r.angularDrag = 0;

        mat = GameObject.Find("Bone").GetComponent<Material>();
    }

    void Update()
    {
        if (touching)
        {
            DrawLine(gameObject.transform.position, index.transform.position, Color.blue);
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.1f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = mat;
        lr.SetColors(color, color);
        lr.SetWidth(1f, 1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "INDEX_FINGER_TIP")
        {
            index = collision.gameObject;
            touching = true;
            Debug.Log("TOUCHED!!!");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "INDEX_FINGER_TIP")
        {
            touching = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "landmark")
        {
            Physics.IgnoreCollision(gameObject.GetComponent<SphereCollider>(), collision.gameObject.GetComponent<SphereCollider>());
        }
    }
}
