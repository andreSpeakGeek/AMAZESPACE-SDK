using Jacovone.AssetBundleMagic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Transform player;
    public Transform pivot;

    public float zMultiplier = 2f;
    public float xMultiplier = 2f;
    public float jumpPower = 5f;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = player.GetComponent<Rigidbody>();	
	}

    private bool jump;

    private void Update()
    {
        if(!jump)
            jump = Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
        float acceleration = 1.0f;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            acceleration = 2.0f;
        }

        float z = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");

        Vector3 v = player.transform.InverseTransformVector(rb.velocity);

        v = player.transform.TransformVector(new Vector3(0, v.y, z * zMultiplier * acceleration * Time.fixedDeltaTime));

        rb.transform.Rotate(0, x * xMultiplier * Time.fixedDeltaTime, 0);
        rb.angularVelocity = Vector3.zero;

        rb.velocity = v;

        if (jump)
        {
            rb.AddForce(jumpPower * Vector3.up, ForceMode.Impulse);
            jump = false;
        }

        Camera cam = Camera.main;
        Vector3 camPos = pivot.transform.position;

        RaycastHit hit;
        bool obstacle = Physics.Raycast(player.transform.position, (pivot.transform.position - player.transform.position).normalized, out hit, Vector3.Distance(pivot.transform.position, player.transform.position));

        if (obstacle)
        {
            camPos = hit.point;
        }

        cam.transform.position = Vector3.Lerp(cam.transform.position, camPos, 0.02f);
        cam.transform.LookAt(player.transform);

    }
}
