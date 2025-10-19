using UnityEngine;

/// <summary>
/// Jika enable, maka akan mengaktifkan second environment dan mengubah environment sesuai dengan yang dipilih
/// </summary>
/// <remarks>This controller is responsible for enabling or disabling the secondary environment functionality. It
/// interacts with the global objects defined in <see cref="SceneDirector"/> to manage environment
/// settings.</remarks>
public class SecondEnvironmentsController : MonoBehaviour
{
    public cbkta_GlobalUI cbkta_globalui;
    public cbkta_GlobalObjects cbkta_globalobjects;
    public cbkta_GlobalLogic cbkta_globallogic;
    public cbkta_GlobalLogicHelper cbkta_globallogichelper;
    public GameObject brainDRealmEnvironment;
    public GameObject lastFightEnvironment;

    /// <summary>
    /// shared with <see cref="SceneDirector" />
    /// </summary>
    [HideInInspector] public SecondEnvironmentsData environmentData;

    private Canvas canvas;

    private Vector3 previousCameraPosition = Vector3.zero;
    private int previousCanvasSortingOrder = 0;

    private bool hasInit = false;

    void Init()
    {
        this.canvas = this.GetComponent<Canvas>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        if (!this.hasInit)
        {
            this.Init();
            this.hasInit = true;
        }

        //setup

        Canvas globalCanvas = this.cbkta_globalui.canvas.GetComponent<Canvas>();

        //cache

        this.previousCanvasSortingOrder = globalCanvas.sortingOrder;
        this.previousCameraPosition = this.cbkta_globalui.cam.transform.position;

        //setting

        globalCanvas.sortingOrder = 1050;
        this.cbkta_globalui.cam.transform.Translate(new(1000, 1000, this.cbkta_globalui.cam.transform.position.z));

        this.cbkta_globallogic.ExecuteFuncOnNextFrame(() => {
            Vector3 canvasPositionFollowCamera = this.canvas.transform.position;
            Vector3 canvasLocalScaleFollowCamera = this.canvas.transform.localScale;

            this.cbkta_globallogichelper.CanvasDisableFollowCamera(this.canvas);

            this.canvas.renderMode = RenderMode.WorldSpace;

            this.canvas.transform.position = canvasPositionFollowCamera;
            this.canvas.transform.localScale = canvasLocalScaleFollowCamera;
        });

        //aktifkan environment sesuai dengan yang dipilih
        switch (this.environmentData)
        {
            case SecondEnvironmentsData.BrainDRealm:
                this.brainDRealmEnvironment.SetActive(true);
                break;
            case SecondEnvironmentsData.LastFight:
                this.lastFightEnvironment.SetActive(true);
                break;
        }
    }

    void OnDisable()
    {
        //setup
        Canvas globalCanvas = this.cbkta_globalui.canvas.GetComponent<Canvas>();
        GameObject camera = this.cbkta_globalui.cam;

        //restore
        this.canvas.renderMode = RenderMode.ScreenSpaceCamera;
        this.cbkta_globallogichelper.CanvasEnableFollowCamera(this.canvas, camera.GetComponent<Camera>());

        globalCanvas.sortingOrder = this.previousCanvasSortingOrder;
        camera.transform.position = this.previousCameraPosition;

        //nonaktifkan semua environment
        this.brainDRealmEnvironment.SetActive(false);
        this.lastFightEnvironment.SetActive(false);
    }
}
