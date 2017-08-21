using UnityEngine;
using System.Collections;

public class snaptester : MonoBehaviour
{
    public string partnerName = "Hub";
    public float closeVPDist = 0.05f;
    public float moveSpeed = 40.0f;
    public float rotateSpeed = 90.0f;

    public  Color closeColor = new Color(0.2F, 0.3F, 0.4F, 0.5F);

    private float dist = 0;
    private Color normalColor;
    private GameObject partnerGO;

    private Vector3 PartnerSize;
    private Vector3 thisSize;

    void Start()
    {
        normalColor = GetComponent<Renderer>().material.color;
        partnerGO = GameObject.Find(partnerName);
        //get current item and partnerGO size
        thisSize = this.GetComponent<Collider>().bounds.size;
        PartnerSize = partnerGO.GetComponent<Collider>().bounds.size;
        Debug.Log(thisSize + "" +PartnerSize);



    }

    void OnMouseDrag()
    {
        Vector3 partnerPos = Camera.main.WorldToViewportPoint(partnerGO.transform.position);
        Vector3 myPos = Camera.main.WorldToViewportPoint(transform.position);
        dist = Vector2.Distance(partnerPos, myPos);
        GetComponent<Renderer>().material.color = (dist < closeVPDist) ? closeColor : normalColor;
        Debug.Log(dist + "Mouse Drag");
    }

    void OnMouseUp()
    {
        Debug.Log("Mouse Up");
        if (dist < closeVPDist)
        {
            transform.parent = partnerGO.transform;
            StartCoroutine(InstallPart());
        }
        else
        {
            GetComponent<Renderer>().material.color = normalColor;
        }
    }

    IEnumerator InstallPart()
    {
        //While position != vector3.zero(origin) need to change to while position != Vector3.zero + new Vector3
        while (transform.localPosition != Vector3.zero + PartnerSize || transform.localRotation != Quaternion.identity)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero + new Vector3(1.0f, 0, 0f), Time.deltaTime * moveSpeed);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.identity, Time.deltaTime * rotateSpeed);
            yield return null;
        }
    }
}
//transform.localPosition != Vector3.zero