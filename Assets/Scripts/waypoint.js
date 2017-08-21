#pragma strict

 var waypoint1 : Transform;
 //extend this if you want
 var waypoint = 1;
 
 function Start () {
 transform.position = waypoint1.position;
 }
 
 function Update () {
 // extend this part too if you extended the top
 
 }
 
 //im assuming your car has a collider
 function OnTriggerEnter () {
 waypoint += 1;
 }
