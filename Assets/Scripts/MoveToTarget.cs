
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveToTarget : MonoBehaviour
{
	Transform MyTrans;
	List<Vector3> endPoints;
	float speed = 15;
	float angluarSpeed = 500;

	void Start()
	{
		MyTrans = GetComponent<Transform>();
		endPoints = new List<Vector3>();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			UpdateControl();
		}
		if (endPoints.Count > 0)
		{
			Vector3 v = endPoints[0] - MyTrans.position;
			var dot = Vector3.Dot(v, MyTrans.right);
			Vector3 next = v.normalized * speed * Time.deltaTime;
			float angle = Vector3.Angle(v, MyTrans.forward);
			if (Vector3.SqrMagnitude(v) > 1f)
			{
				float minAngle = Mathf.Min(angle, angluarSpeed * Time.deltaTime);
				//点乘
				if (angle > 1f)
				{
					if (dot > 0)
					{
						MyTrans.Rotate(new Vector3(0, minAngle, 0));
					}
					else
					{
						MyTrans.Rotate(new Vector3(0, -minAngle, 0));
					}
				}
				else
				{
					MyTrans.LookAt(endPoints[0]);
					MyTrans.position += next;
				}
			}
			else
			{
				endPoints.RemoveAt(0);
			}

		}
	}
	void UpdateControl()
	{
		//获取屏幕坐标
		Vector3 mousepostion = Input.mousePosition;
		//定义从屏幕发出的射线
		Ray ray = Camera.main.ScreenPointToRay(mousepostion);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo))
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				AddEndPoint(hitInfo.point);
			}
			else
			{
				ReSetEndPoint(hitInfo.point);
			}
		}

	}
	void AddEndPoint(Vector3 endPoint)
	{
		endPoint.y = MyTrans.position.y;
		endPoints.Add(endPoint);
	}
	void ReSetEndPoint(Vector3 endPoint)
	{
		endPoint.y = MyTrans.position.y;
		endPoints.Clear();
		endPoints.Add(endPoint);
	}
}