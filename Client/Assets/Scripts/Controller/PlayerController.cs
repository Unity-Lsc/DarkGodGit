/****************************************************
    文件：PlayerController.cs
	作者：LSC
    邮箱: 314623971@qq.com
    日期：2020/12/30 22:24:35
	功能：角色控制器
*****************************************************/

using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Animator ani;//角色动画控制器
    private CharacterController ctrl;//角色控制器
    private Transform tranCamera;//相机
    private Vector3 cameraOffset;
    private bool isMove = false;//角色是否移动
    private float currentBlend;
    private float targetBlend;
    private Vector2 dir = Vector2.zero;//角色方向
    public Vector2 Dir {
        get {
            return dir;
        }
        set {
            isMove = value != Vector2.zero;
            dir = value;
        }
    }

    private void Awake() {
        ani = GetComponent<Animator>();
        ctrl = GetComponent<CharacterController>();
    }

    public void Init() {
        tranCamera = Camera.main.transform;
        cameraOffset = transform.position - tranCamera.position;
    }

    private void Update() {
        //测试角色移动
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        //Vector2 normalInputDir = new Vector2(h, v).normalized;
        //if(normalInputDir != Vector2.zero) {
        //    Dir = normalInputDir;
        //    SetBlend(1);
        //}else {
        //    Dir = Vector2.zero;
        //    SetBlend(0);
        //}

        if(isMove) {
            //设置方向
            SetDir();
            //产生移动
            SetMove();
            //相机跟随
            SetCameraFollow();
        }

        if(currentBlend != targetBlend) {
            UpdateMixBlend();
        }

    }

    private void SetDir() {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + tranCamera.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.eulerAngles = eulerAngles;
    }

    private void SetMove() {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
    }

    public void SetCameraFollow() {
        if(tranCamera != null) {
            tranCamera.position = transform.position - cameraOffset;
        }
    }

    public void SetBlend(float blend) {
        targetBlend = blend;
    }

    private void UpdateMixBlend() {
        if(Mathf.Abs(currentBlend - targetBlend) < Constants.AccelerSpeed * Time.deltaTime) {
            currentBlend = targetBlend;
        }else if(currentBlend > targetBlend) {
            currentBlend -= Constants.AccelerSpeed * Time.deltaTime;
        }else {
            currentBlend += Constants.AccelerSpeed * Time.deltaTime;
        }
        ani.SetFloat("Blend", currentBlend);
    }

}