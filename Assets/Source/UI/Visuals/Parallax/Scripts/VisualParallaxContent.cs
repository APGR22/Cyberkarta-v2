using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public enum VisualParallaxDirection
{
    Up, Down, Left, Right
};
public enum VisualParallaxDirectionOneLine
{
    Horizontal, Vertical
};

public class VisualParallaxContent : MonoBehaviour
{
    public GameObject maskArea; //for example, Canva. Recomended to use parent (this.gameObject)
    public GameObject prefabSpawnObject;

    public float zDistance = 1;
    public VisualParallaxDirection direction = VisualParallaxDirection.Left;

    public bool autoScroll = false;
    /// <summary>
    /// Left, Top, Right, Bottom
    /// </summary>
    private List<float> maskAreaEdges = null;

    /// <summary>
    /// Horizontal: [Left -> Right] <br/>
    /// Vertical: [Top -> Bottom]
    /// </summary>
    private List<GameObject> spawnObjects = new();

    VisualParallaxDirectionOneLine GetDirectionOneLine(VisualParallaxDirection direction)
    {
        switch (direction)
        {
            case VisualParallaxDirection.Left or VisualParallaxDirection.Right:
                return VisualParallaxDirectionOneLine.Horizontal;
            case VisualParallaxDirection.Up or VisualParallaxDirection.Down:
                return VisualParallaxDirectionOneLine.Vertical;
        }

        return VisualParallaxDirectionOneLine.Horizontal;
    }

    void MoveSpawnObjectToOtherSide(GameObject obj, GameObject objSide, VisualParallaxDirection side)
    {
        VisualTransformData objSideVTD = new(objSide);
        VisualTransformData objVTD = new(obj);

        List<float> objSideEdges = objSideVTD.GetGlobalEdges();

        float centerToX = objVTD.Width / 2f;
        float centerToY = objVTD.Height / 2f;

        switch (side)
        {
            case VisualParallaxDirection.Left:
                objVTD.GlobalPosition = new Vector2(objSideEdges[0] - centerToX, objSideVTD.GlobalPosition.y);
                break;
            case VisualParallaxDirection.Up:
                objVTD.GlobalPosition = new Vector2(objSideVTD.GlobalPosition.x, objSideEdges[1] + centerToY);
                break;
            case VisualParallaxDirection.Right:
                objVTD.GlobalPosition = new Vector2(objSideEdges[2] + centerToX, objSideVTD.GlobalPosition.y);
                break;
            case VisualParallaxDirection.Down:
                objVTD.GlobalPosition = new Vector2(objSideVTD.GlobalPosition.x, objSideEdges[3] - centerToY);
                break;
        }
    }

    /// <summary>
    /// Dead zone is inside
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="side"></param>
    /// <returns></returns>
    bool IsSpawnObjectReachDeadZone(GameObject obj, VisualParallaxDirection side)
    {
        VisualTransformData objVTD = new(obj);

        List<float> objEdges = objVTD.GetGlobalEdges();

        switch (side)
        {
            case VisualParallaxDirection.Left:
                return this.maskAreaEdges[0] <= objEdges[0];
            case VisualParallaxDirection.Up:
                return this.maskAreaEdges[1] >= objEdges[1];
            case VisualParallaxDirection.Right:
                return objEdges[2] <= this.maskAreaEdges[2];
            case VisualParallaxDirection.Down:
                return objEdges[3] >= this.maskAreaEdges[3];
        }

        return false;
    }

    bool IsSpawnObjectReachOutsideDeadZone(GameObject obj, VisualParallaxDirection side)
    {
        VisualTransformData objVTD = new(obj);

        List<float> objEdges = objVTD.GetGlobalEdges();

        switch (side)
        {
            case VisualParallaxDirection.Left:
                return objEdges[0] <= this.maskAreaEdges[0];
            case VisualParallaxDirection.Up:
                return objEdges[1] >= this.maskAreaEdges[1];
            case VisualParallaxDirection.Right:
                return this.maskAreaEdges[2] <= objEdges[2];
            case VisualParallaxDirection.Down:
                return this.maskAreaEdges[3] >= objEdges[3];
        }

        return false;
    }

    float DistanceSpawnObjectRelativeToInsideDeadZone(GameObject obj, VisualParallaxDirection side)
    {
        VisualTransformData objVTD = new(obj);

        List<float> objEdges = objVTD.GetGlobalEdges();

        switch (side)
        {
            case VisualParallaxDirection.Left:
                return objEdges[2] - this.maskAreaEdges[0];
            case VisualParallaxDirection.Up:
                return this.maskAreaEdges[1] - objEdges[3];
            case VisualParallaxDirection.Right:
                return this.maskAreaEdges[2] - objEdges[0];
            case VisualParallaxDirection.Down:
                return objEdges[1] - this.maskAreaEdges[3];
        }

        return 0;
    }

    GameObject AddSpawnObjectBeside(GameObject objSide, VisualParallaxDirection side)
    {
        GameObject obj = Instantiate(this.prefabSpawnObject, transform);

        this.MoveSpawnObjectToOtherSide(obj, objSide, side);

        return obj;
    }

    void Spawn()
    {
        //membuat objek pertama dulu, sebagai objek tengah
        GameObject firstObj = Instantiate(this.prefabSpawnObject, transform);
        VisualTransformData firstObjVTD = new(firstObj);

        float centerToX = firstObjVTD.Width / 2f;
        float centerToY = firstObjVTD.Height / 2f;

        //berdasarkan arahnya, dipojokkan sampai mentok ke batas sesuai arah
        switch (this.direction)
        {
            case VisualParallaxDirection.Left:
                firstObjVTD.GlobalPosition = new Vector2(this.maskAreaEdges[0] + centerToX, firstObjVTD.GlobalPosition.y);
                break;
            case VisualParallaxDirection.Up:
                firstObjVTD.GlobalPosition = new Vector2(firstObjVTD.GlobalPosition.x, this.maskAreaEdges[1] - centerToY);
                break;
            case VisualParallaxDirection.Right:
                firstObjVTD.GlobalPosition = new Vector2(this.maskAreaEdges[2] - centerToX, firstObjVTD.GlobalPosition.y);
                break;
            case VisualParallaxDirection.Down:
                firstObjVTD.GlobalPosition = new Vector2(firstObjVTD.GlobalPosition.x, this.maskAreaEdges[3] + centerToY);
                break;
        }

        //menambahkan ke daftar
        this.spawnObjects.Add(firstObj);

        //batas aman loopSFX agar tidak selamanya
        int limitLoop = 20;
        int loopCount = 0;

        ///jika horizontal, maka buat kiri dan kanan sampai melewati kedua batas area
        ///jika vertikal, maka buat atas dan bawah sampai melewati kedua batas area
        switch (this.GetDirectionOneLine(this.direction))
        {
            case VisualParallaxDirectionOneLine.Horizontal:

                //terus buat ke kanan sampai keluar dari deadzone (minimal harus buat 1 dulu)
                do
                {
                    this.spawnObjects.Add(this.AddSpawnObjectBeside(this.spawnObjects.Last(), VisualParallaxDirection.Right));
                    loopCount++;
                }
                while (!this.IsSpawnObjectReachOutsideDeadZone(this.spawnObjects.Last(), VisualParallaxDirection.Right) && loopCount < limitLoop);
                
                loopCount = 0;

                //terus buat ke kiri sampai keluar dari deadzone (minimal harus buat 1 dulu)
                do
                {
                    this.spawnObjects.Insert(0, this.AddSpawnObjectBeside(this.spawnObjects.First(), VisualParallaxDirection.Left));
                    loopCount++;
                }
                while (!this.IsSpawnObjectReachOutsideDeadZone(this.spawnObjects.First(), VisualParallaxDirection.Left) && loopCount < limitLoop);

                break;

            case VisualParallaxDirectionOneLine.Vertical:

                //terus buat ke bawah sampai keluar dari deadzone (minimal harus buat 1 dulu)
                do
                {
                    this.spawnObjects.Add(this.AddSpawnObjectBeside(this.spawnObjects.Last(), VisualParallaxDirection.Down));
                    loopCount++;
                }
                while (!this.IsSpawnObjectReachOutsideDeadZone(this.spawnObjects.Last(), VisualParallaxDirection.Down) && loopCount < limitLoop);

                loopCount = 0;

                //terus buat ke atas sampai keluar dari deadzone (minimal harus buat 1 dulu)
                do
                {
                    this.spawnObjects.Insert(0, this.AddSpawnObjectBeside(this.spawnObjects.First(), VisualParallaxDirection.Up));
                    loopCount++;
                }
                while (!this.IsSpawnObjectReachOutsideDeadZone(this.spawnObjects.First(), VisualParallaxDirection.Up) && loopCount < limitLoop);

                break;

        }
    }

    void Init()
    {
        RectTransform rtTemp;
        SpriteRenderer srTemp;
        if (!this.prefabSpawnObject.TryGetComponent(out srTemp) && !this.prefabSpawnObject.TryGetComponent(out rtTemp))
        {
            Debug.LogError("Tidak memiliki Sprite Renderer maupun Rect Transform, tidak bisa dilakukan");
            return;
        }
        if (!this.maskArea.TryGetComponent(out srTemp) && !this.maskArea.TryGetComponent(out rtTemp))
        {
            Debug.LogError("Tidak memiliki Sprite Renderer maupun Rect Transform, tidak bisa dilakukan");
            return;
        }

        VisualTransformData maskAreaVTD = new(this.maskArea);
        this.maskAreaEdges = maskAreaVTD.GetGlobalEdges();

        this.Spawn();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        //update
        VisualTransformData maskAreaVTD = new(this.maskArea);
        this.maskAreaEdges = maskAreaVTD.GetGlobalEdges();

        /*
         * jika objek pertama berada di luar dead zone (inside)
         *  dan mencapai maksimal jarak ke dead zone (inside)
         * maka pindahkan objek pertama ke paling ujung (kanan, bawah) secara transform dan index arrayss
         */

        VisualParallaxDirectionOneLine VPDOneLine = this.GetDirectionOneLine(this.direction);

        /// <summary>
        /// kiri atau atas
        /// </summary>
        VisualParallaxDirection VPDMin = VPDOneLine == VisualParallaxDirectionOneLine.Horizontal ? VisualParallaxDirection.Left : VisualParallaxDirection.Up;
        /// <summary>
        /// kanan atau bawah
        /// </summary>
        VisualParallaxDirection VPDMax = VPDOneLine == VisualParallaxDirectionOneLine.Horizontal ? VisualParallaxDirection.Right : VisualParallaxDirection.Down;

        if (this.IsSpawnObjectReachOutsideDeadZone(this.spawnObjects.First(), VPDMin) && this.DistanceSpawnObjectRelativeToInsideDeadZone(this.spawnObjects.First(), VPDMin) <= -5f)
        {
            GameObject first = this.spawnObjects.First();
            GameObject last = this.spawnObjects.Last();

            this.MoveSpawnObjectToOtherSide(first, last, VPDMax);

            this.spawnObjects.Remove(first);
            this.spawnObjects.Add(first);
        }

        if (this.IsSpawnObjectReachOutsideDeadZone(this.spawnObjects.Last(), VPDMax) && this.DistanceSpawnObjectRelativeToInsideDeadZone(this.spawnObjects.Last(), VPDMax) <= -5f)
        {
            GameObject last = this.spawnObjects.Last();
            GameObject first = this.spawnObjects.First();

            this.MoveSpawnObjectToOtherSide(last, first, VPDMin);

            this.spawnObjects.Remove(last);
            this.spawnObjects.Insert(0, last);
        }
    }
}
