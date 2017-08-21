#pragma strict
 
var partnerName = "Hub";
var closeVPDist = 0.05;
var moveSpeed = 40.0;
var rotateSpeed = 90.0;

var closeColor = Color(1, 0, 0);

private var dist = Mathf.Infinity;
private var normalColor: Color;
private var partnerGO: GameObject;

function Start() {
    normalColor = GetComponent.<Renderer>().material.color;
    partnerGO = GameObject.Find(partnerName);
}

function OnMouseDrag() {
    var partnerPos = Camera.main.WorldToViewportPoint(partnerGO.transform.position);
    var myPos = Camera.main.WorldToViewportPoint(transform.position);
    dist = Vector2.Distance(partnerPos, myPos);
    GetComponent.<Renderer>().material.color = (dist < closeVPDist) ? closeColor : normalColor;
    Debug.Log(dist + "Mouse Drag");
}

function OnMouseUp() {
    Debug.Log("Mouse Up");
    if (dist < closeVPDist) {
        transform.parent = partnerGO.transform;
        InstallPart();
    }
    else {
        GetComponent.<Renderer>().material.color = normalColor;
    }
}

function InstallPart() {
    while (transform.localPosition != Vector3.zero || transform.localRotation != Quaternion.identity) {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, Time.deltaTime * moveSpeed);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.identity, Time.deltaTime * rotateSpeed);
        yield;
    }
}