using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


	public GameObject player;

	public PolygonCollider2D cameraBounds;

	public float followSpeed = 2;
	public float damp = 0.5f;
	public float verticalOffset = 1.5f;

	public float lookOffset = 2;
	
	private float xMin, xMax , yMin , yMax;
	private float zAxis = -10;

	private float lastY,lastX;
	
	
	private bool direction = true;

	private bool b_lookUp,b_lookDown;

	// Use this for initialization
	void Start () {
		gameObject.transform.position = player.transform.position;
		SetMinMax(cameraBounds.points);
		lastX = player.transform.position.x;
		lastY = player.transform.position.y;
		b_lookUp=false;
		b_lookDown =false;
	}
	
	

	// Update is called once per frame
	void LateUpdate () {
		float x = lastX;
		float y = lastY;
		if(Mathf.Abs(lastX - player.transform.position.x) > damp){
			x = Mathf.Clamp(player.transform.position.x,xMin,xMax);
			lastX = x;
		}
		if(Mathf.Abs(lastY - player.transform.position.y) > damp){
			y = Mathf.Clamp(player.transform.position.y,yMin,yMax);
			lastY = y;
		}
		
		y += verticalOffset;
		
		if(b_lookUp){
			y += lookOffset;
		}
		if(b_lookDown){
			y -= lookOffset;
		}
		
		gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, new Vector3(x,y,-10), followSpeed * Time.deltaTime);
	}

	private void SetMinMax(Vector2[] points){
		for (int i = 0; i < points.Length; i++)
		{
			if(xMax < points[i].x){
				xMax = points[i].x;
			}else if(xMin > points[i].x){
				xMin = points[i].x;
			}

			if(yMax < points[i].y){
				yMax = points[i].y;
			}else if(yMin > points[i].y){
				yMin = points[i].y;
			}
		}
	}

	public void lookUp(){
		b_lookUp = true;
	}

	public void lookDown(){
		b_lookDown = true;
	}

	public void resetLook(){
		b_lookDown = false;
		b_lookUp = false;
	}
}
