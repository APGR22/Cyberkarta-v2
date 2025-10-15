using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Only use it for 2 cases (RectTransform and Transform) at same time
/// </summary>
public class VisualTransformData
{
    public Transform transform { get; private set; }
    public RectTransform rectTransform { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }

    private bool useRT = false;

    public VisualTransformData(GameObject obj)
    {
        RectTransform rtTemp;
        SpriteRenderer srTemp;

        if (!(this.useRT = obj.TryGetComponent(out rtTemp)))
        {
            this.transform = obj.transform;
        }
        else
        {
            this.rectTransform = rtTemp;
        }

        if (obj.TryGetComponent(out srTemp))
        {
            this.spriteRenderer = srTemp;
        }
        else
        {
            this.spriteRenderer = null;
        }
    }

    public Vector2 LocalAnchoredPosition
    {
        get
        {
            return this.useRT ? this.rectTransform.anchoredPosition : this.transform.localPosition;
        }

        set
        {
            if (this.useRT)
            {
                this.rectTransform.anchoredPosition = value;
            }
            else
            {
                this.transform.localPosition = new Vector3(value.x, value.y, this.transform.localPosition.z);
            }
        }
    }

    public Vector2 GlobalPosition
    {
        get
        {
            return this.useRT ? this.rectTransform.position : this.transform.position;
        }

        set
        {
            Vector3 newValue = new Vector3(value.x, value.y, this.useRT ? this.rectTransform.position.z : this.transform.position.z);

            if (this.useRT)
            {
                this.rectTransform.position = newValue;
            }
            else
            {
                this.transform.position = newValue;
            }
        }
    }

    public float Width
    {
        get
        {
            if (this.spriteRenderer)
            {
                return this.spriteRenderer.bounds.size.x;
            }

            if (this.rectTransform)
            {
                return this.rectTransform.rect.width * this.rectTransform.lossyScale.x;
            }

            Debug.LogError("Tidak ada Sprite Renderer maupun Rect Transform, jadi 0");
            return 0;
        }
    }

    public float Height
    {
        get
        {
            if (this.spriteRenderer)
            {
                return this.spriteRenderer.bounds.size.y;
            }

            if (this.rectTransform)
            {
                return this.rectTransform.rect.height * this.rectTransform.lossyScale.y;
            }

            Debug.LogError("Tidak ada Sprite Renderer maupun Rect Transform, jadi 0");
            return 0;
        }
    }

    /// <summary>
    /// Baru berfungsi pakai RectTransform
    /// </summary>
    /// <returns>Left, Top, Right, Bottom</returns>
    public List<float> GetGlobalEdges()
    {
        if (!this.useRT && !this.spriteRenderer)
        {
            Debug.LogError("Hanya bisa dengan Sprite Renderer atau Rect Transform. Jadi akan dikembalikan null");
            return null;
        }

        float width = this.Width;
        float height = this.Height;

        Vector2 position = this.GlobalPosition;

        List<float> edges = new()
        {
            position.x - (width / 2f), //Left
            position.y + (height / 2f), //Top
            position.x + (width / 2f), //Right
            position.y - (height / 2f) //Bottom
        };

        return edges;
    }
}
