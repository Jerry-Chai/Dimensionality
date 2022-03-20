using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class CameraChange : MonoBehaviour
{
    [Header("改变的模型位置")]
    public GameObject Node;
	[Header("旋转动画")]
	public ActionContainer[] RotationAction;
	[Header("移动动画")]
	public ActionContainer[] PositionAction;
	[Header("大小动画")]
	public ActionContainer[] ScaleAction;

	[Header("（默认旋转角度）以下信息可以不填写，则获取当前数据")]
	public Vector3 RotationValue;
	[Header("（默认位置）")]
	public Vector3 PositionValue;
	[Header("（默认大小）")]
	public Vector3 ScaleValue;

	[Header("（当物体被显示时是否执行动画）")]
	public bool OnEnableReloadUse = true;
	[Header("（是否循环执行动画）")]
	public bool CycleActionUse = true;

	public GameObject Cube1;
	public GameObject Cube2;
	public GameObject Cube3;
	public GameObject Cube4;
	public GameObject Cube5;
	public GameObject Cube6;
	public GameObject Cube7;
	public GameObject Cube8;
	public GameObject Cube9;
	public GameObject Cube10;

    /// 以下为临时变量
    private float RotationNowTime = 0;
    private float PositionNowTime = 0;
    private float ScaleNowTime = 0;
    private int RotationIndex = 0;
    private int PositionIndex = 0;
    private int ScaleIndex = 0;
    private bool RotationStartEventUse = true;
    private bool PositionStartEventUse = true;
    private bool ScaleStartEventUse = true;

    private void Awake()
    {
        PositionValue = Node.transform.localPosition;
        RotationValue = Node.transform.rotation.eulerAngles;
        ScaleValue = Node.transform.localScale;
    }
    private void OnEnable()
    {
        RotationNowTime = 0;
        PositionNowTime = 0;
        ScaleNowTime = 0;

        Node.transform.localPosition = PositionValue;
        Node.transform.rotation = Quaternion.Euler(RotationValue);
        Node.transform.localScale = ScaleValue;
    }
    private void Update()
    {
        if (!gameObject.activeSelf || !OnEnableReloadUse) return;
        RotationNowTime += Time.deltaTime;
        PositionNowTime += Time.deltaTime;
        ScaleNowTime += Time.deltaTime;

        if ((RotationAction.Length > 0) && (CycleActionUse || !RotationAction[RotationIndex].GetOverUse()))
        {
            if (RotationNowTime > RotationAction[RotationIndex].WaitTimer)
            {
                if (RotationStartEventUse) { RotationAction[RotationIndex].StartActionEvent.Invoke(); RotationStartEventUse = false; }
                float TimeValue = (RotationNowTime - RotationAction[RotationIndex].WaitTimer) / RotationAction[RotationIndex].ExecuteTimer;
                Vector3 PoorValueTemp = RotationIndex == 0 ? RotationValue : RotationAction[RotationIndex - 1].EndValue;
                Node.transform.rotation = Quaternion.Euler(Vector3.Lerp(PoorValueTemp - new Vector3(0, 360, 0), RotationAction[RotationIndex].EndValue, TimeValue));
            }
            if (RotationNowTime > (RotationAction[RotationIndex].WaitTimer + RotationAction[RotationIndex].ExecuteTimer))
            {
                RotationAction[RotationIndex].EndActionEvent.Invoke();
                RotationAction[RotationIndex].SetOverUse(true);
                RotationIndex++;
                RotationStartEventUse = true;
                if (RotationIndex >= RotationAction.Length) RotationIndex = 0;
                RotationNowTime = 0;
            }
        }

        if ((PositionAction.Length > 0) && (CycleActionUse || !PositionAction[PositionIndex].GetOverUse()))
        {
            if (PositionNowTime > PositionAction[PositionIndex].WaitTimer)
            {
                if (PositionStartEventUse) { PositionAction[PositionIndex].StartActionEvent.Invoke(); PositionStartEventUse = false; }
                float TimeValue = (PositionNowTime - PositionAction[PositionIndex].WaitTimer) / PositionAction[PositionIndex].ExecuteTimer;
                Vector3 PoorValueTemp = PositionIndex == 0 ? PositionValue : PositionAction[PositionIndex - 1].EndValue;
                Node.transform.localPosition = Vector3.Lerp(PoorValueTemp, PositionAction[PositionIndex].EndValue, TimeValue);
            }
            if (PositionNowTime > (PositionAction[PositionIndex].WaitTimer + PositionAction[PositionIndex].ExecuteTimer))
            {
                PositionAction[PositionIndex].EndActionEvent.Invoke();
                PositionAction[PositionIndex].SetOverUse(true);
                PositionStartEventUse = true;
                PositionIndex++;
                if (PositionIndex >= PositionAction.Length) PositionIndex = 0;
                PositionNowTime = 0;
            }
        }

        if ((ScaleAction.Length > 0) && (CycleActionUse || !ScaleAction[ScaleIndex].GetOverUse()))
        {
            if (ScaleNowTime > ScaleAction[ScaleIndex].WaitTimer)
            {
                if (ScaleStartEventUse) { ScaleAction[ScaleIndex].StartActionEvent.Invoke(); ScaleStartEventUse = false; }
                float TimeValue = (ScaleNowTime - ScaleAction[ScaleIndex].WaitTimer) / ScaleAction[ScaleIndex].ExecuteTimer;
                Vector3 PoorValueTemp = ScaleIndex == 0 ? ScaleValue : ScaleAction[ScaleIndex - 1].EndValue;
                Node.transform.localScale = Vector3.Lerp(PoorValueTemp, ScaleAction[ScaleIndex].EndValue, TimeValue);
            }
            if (ScaleNowTime > (ScaleAction[ScaleIndex].WaitTimer + ScaleAction[ScaleIndex].ExecuteTimer))
            {
                ScaleAction[ScaleIndex].EndActionEvent.Invoke();
                ScaleAction[ScaleIndex].SetOverUse(true);
                ScaleStartEventUse = true;
                ScaleIndex++;
                if (ScaleIndex >= ScaleAction.Length) ScaleIndex = 0;
                ScaleNowTime = 0;
            }
        }
    }

    public void Onclick()
	{
		Cube1.SetActive(false);
		Cube2.SetActive(false);
		Cube3.SetActive(false);
		Cube4.SetActive(false);
		Cube5.SetActive(false);
		Cube6.SetActive(true);
		Cube7.SetActive(true);
		Cube8.SetActive(true);
		Cube9.SetActive(true);
		Cube10.SetActive(true);
	}

	public void StartAction()
	{
		OnEnableReloadUse = true;
	}

    public void CamSet()
    {
        Node.GetComponent<Camera>().orthographic = true;
    }

    /// 开始播放动画 ， 如果已播放，则不会重复播放
    public void PlayAction() { OnEnableReloadUse = true; }

	[System.Serializable]
	public class ActionContainer
	{
		[Header("等待几秒后开始播放动画")]
		public float WaitTimer;
		[Header("动画持续时间")]
		public float ExecuteTimer;
		[Header("结束值")]
		public Vector3 EndValue;
        [Header("当动画开始执行的事件")]
		public UnityEvent StartActionEvent = new UnityEvent();
		[Header("当动画执行完毕后执行的事件")]
		public UnityEvent EndActionEvent = new UnityEvent();

		private bool OverUse;

		public bool GetOverUse() { return this.OverUse; }
		public void SetOverUse(bool Use) { this.OverUse = Use; }
	}
}
