using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;

public enum CAMERATARGET // -----------カメラのターゲットタイプ----------------
{
    PLAYER,             // プレイヤー座標
    PLAYER_MARGIN,      // プレイヤー座標（前方視界を確保するマージン）
    PLAYER_GROUND,      // 過去にプレイヤーが世知下地面の座標
                        // (前方視界を確保するマージン）
}

public enum CAMERAHOMING
{ // ----------カメラのホーミングタイプ---------------
    DIRECT,             // ダイレクトにカメラ座標にターゲット座標を設定する
    LERP,               // カメラとターゲット座標を線形補強する
    SlERP,              // カメラとターゲット座標を曲線補強する
    STOP,               // カメラを止める
}
public class CameraFollow : MonoBehaviour
{
    // ==== 外部パラメータ(Inspector表示)===================
    [System.Serializable]
    public class Param
    {
        public CAMERATARGET targetType = CAMERATARGET.PLAYER_MARGIN;
        public CAMERAHOMING homingType = CAMERAHOMING.LERP;
        public Vector2 margin = new Vector2(2.0f, 1.0f);
        public Vector2 homing = new Vector2(0.05f, 0.1f);
        public bool borderCheck = false;
        public Transform borderLeftTop;
        public Transform borderRightBottom;
        public bool viewAreaCheck = false;
        public Vector2 viewAreaMinMargin = new Vector2(0.0f, 0.0f);
        public Vector2 viewAreaMaxMargin = new Vector2(0.0f, 2.0f);

        public bool orthographicEnabled = true;
        public float screenOGSize = 7.0f;
        public float screenOGSizeHoming = 0.1f;
        public float screenPSSize = 50.0f;
        public float screenPSSizeHoming = 0.1f;
    }
    public Param param;

    // ====== キャッシュ ===============================
    Camera cam;
    Transform playerTrfm;
    PlayerRB player;

    private float screenOGSizeAdd = 0.0f;
    private float screenPSSizeAdd = 0.0f;

    public float firstDir = 1.0f; // プレイヤーの向いている方向

    // ====== コード(Monobehaviour基本機能の実装) =========
    void Awake()
    {
        cam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").
                    GetComponent<PlayerRB>();
        playerTrfm = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Use this for initialization
    void Start()
    {
    }

    void LateUpdate()
    {
        float targetX = playerTrfm.position.x;
        float targetY = playerTrfm.position.y;
        float pX = transform.position.x;
        float pY = transform.position.y;
        float screenOGSize = cam.orthographicSize;
        float screenPSSize = cam.fieldOfView;

        // ターゲットの設定
        switch (param.targetType)
        {
            case CAMERATARGET.PLAYER:
                targetX = playerTrfm.position.x;
                targetY = playerTrfm.position.y;
                break;
            case CAMERATARGET.PLAYER_MARGIN:
                targetX = playerTrfm.position.x + param.margin.x * player.direction;
                targetY = playerTrfm.position.y + param.margin.y;
                break;
            case CAMERATARGET.PLAYER_GROUND: // 今回は使えない
                targetX = playerTrfm.position.x + param.margin.x * player.direction;
                //targetY = playerCtrl.groundY + param.margin.y;
                break;
        }

        // カメラの移動限界境界線チェック
        if (param.borderCheck) // Orthographicの時はちゃんと動作する
        {
            // 画面の高さ、幅の半分
            float height = screenOGSize;
            if (!param.orthographicEnabled)
                height = screenPSSize / 10f;
            float width = height * cam.aspect;

            targetX = Mathf.Max(targetX, param.borderLeftTop.position.x + width);
            targetY = Mathf.Min(targetY, param.borderLeftTop.position.y - height);
            targetX = Mathf.Min(targetX, param.borderRightBottom.position.x - width);
            targetY = Mathf.Max(targetY, param.borderRightBottom.position.y + height);
        }

        // プレイヤーのカメラ内チェック
        if (param.viewAreaCheck)
        {
            float z = playerTrfm.position.z - transform.position.z;
            Vector3 minMargin = param.viewAreaMinMargin;
            Vector3 maxMargin = param.viewAreaMaxMargin;
            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, z)) - minMargin;
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, z)) - maxMargin;
            if (playerTrfm.position.x < min.x || playerTrfm.position.x > max.x)
            {
                targetX = playerTrfm.position.x;
            }
            if (playerTrfm.position.y < min.y || playerTrfm.position.y > max.y)
            {
                targetY = playerTrfm.position.y + 3.0f;
                //playerCtrl.groundY = playerTrfm.position.y;
            }
        }

        // カメラ移動（ホーミング）
        switch (param.homingType)
        {
            case CAMERAHOMING.DIRECT:
                pX = targetX;
                pY = targetY;
                screenOGSize = param.screenOGSize;
                screenPSSize = param.screenPSSize;
                break;

            case CAMERAHOMING.LERP:
                pX = Mathf.Lerp(transform.position.x, targetX, param.homing.x * Time.deltaTime * 60);
                pY = Mathf.Lerp(transform.position.y, targetY, param.homing.y * Time.deltaTime * 60);
                screenOGSize = Mathf.Lerp(screenOGSize, param.screenOGSize,
                            param.screenOGSizeHoming);
                screenPSSize = Mathf.Lerp(screenPSSize, param.screenPSSize,
                            param.screenPSSize);
                break;

            case CAMERAHOMING.SlERP:
                pX = Mathf.SmoothStep(transform.position.x, targetX, param.homing.x);
                pY = Mathf.SmoothStep(transform.position.y, targetY, param.homing.y);
                screenOGSize = Mathf.SmoothStep(screenOGSize,
                            param.screenOGSize, param.screenOGSizeHoming);
                screenPSSize = Mathf.SmoothStep(screenPSSize,
                            param.screenPSSize, param.screenPSSizeHoming);
                break;

            case CAMERAHOMING.STOP:
                break;
        }

        transform.position = new Vector3(pX, pY, transform.position.z);
        cam.orthographic = param.orthographicEnabled;
        cam.orthographicSize = screenOGSize + screenOGSizeAdd;
        cam.fieldOfView = screenPSSize + screenPSSizeAdd;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 2.5f, 10.0f);
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 30.0f, 100.0f);

        // カメラの特殊ズーム効果計算
        screenOGSizeAdd *= 0.9f;
        screenPSSizeAdd *= 0.9f;
    }

    // ======= コード（その他）================================
    public void SetCamera(Param cameraPara)
    {
        param = cameraPara;
    }

    // カメラのズームに使える　ゴールしたときとかどうですか
    public void AddCameraSize(float ogAdd, float psAdd = 0f)
    {
        screenOGSizeAdd = ogAdd;
        screenPSSizeAdd = psAdd;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    private void OnDrawGizmos()
    {
        if (param.borderCheck)
        {
            Gizmos.DrawLine(param.borderLeftTop.position, param.borderLeftTop.position + new Vector3(100f, 0f));
            Gizmos.DrawLine(param.borderLeftTop.position, param.borderLeftTop.position + new Vector3(0f, -100f));
            Gizmos.DrawLine(param.borderRightBottom.position, param.borderRightBottom.position + new Vector3(-100f, 0f));
            Gizmos.DrawLine(param.borderRightBottom.position, param.borderRightBottom.position + new Vector3(0f, 100f));
            /*
            GizmosExtensions2D.DrawWireRect2D(param.borderLeftTop.position, 1.0f, 100f);
            GizmosExtensions2D.DrawWireRect2D(param.borderLeftTop.position, 100f, 0.2f);
            GizmosExtensions2D.DrawWireRect2D(param.borderRightBottom.position, 1.0f, 100f);
            GizmosExtensions2D.DrawWireRect2D(param.borderRightBottom.position, 100f, 0.4f);
            */
       /* }
    }*/
}
