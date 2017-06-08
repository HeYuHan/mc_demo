using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker {

    private Material mat;
    private float m_BarHeight, m_PanelHeight;
    Color m_CurrentColor,m_SetColor;
    Rect m_Rect;
    Rect m_SelectCtrl;
    float bar_value, backup_bar_value;
    float m_Alpha = 1;
    public void Init()
    {
        if(!mat)
        {
            mat = new Material(Shader.Find("Unlit/ColorPicker"));
        }
        bar_value = 0;
        backup_bar_value = -1;
    }
    public Color GetColor()
    {
        Color c = m_CurrentColor;
        c.a = m_Alpha;
        return c;
    }
    public void SetColor(Color color)
    {
        m_Alpha = color.a;
        color.a = 1;
        m_CurrentColor = color;
        Vector3 hsv = RGBtoHSV(color);
        backup_bar_value = bar_value = hsv.x / 360;
        m_SelectCtrl.x = m_Rect.width * hsv.y + m_Rect.x;
        m_SelectCtrl.y = (1 - hsv.z) * m_PanelHeight + (m_Rect.y + m_Rect.height - m_PanelHeight);
        UpdatePanel();
    }
    private void UpdateCurrentColor()
    {
        float h = bar_value*360;
        float s = (m_SelectCtrl.x - m_Rect.x) / m_Rect.width;
        float v = (m_SelectCtrl.y - (m_Rect.y + m_Rect.height - m_PanelHeight)) / m_PanelHeight;
        Vector3 hsv = new Vector3(h, s, 1 - v);
        m_CurrentColor = HSVtoRGB(hsv);
    }
    private void SetMatColor(Color c)
    {
        m_SetColor = c;
        mat.SetColor("StartColor", c);
    }
    public void SetArg(float bar_height, float panel_height,Rect size)
    {
        m_BarHeight = bar_height;
        m_PanelHeight = panel_height;
        m_Rect = size;
        mat.SetFloat("PanelHeight", panel_height / size.height);
        mat.SetFloat("BarHeight", bar_height / size.height);
        m_CurrentColor.a = m_Alpha;
        SetColor(m_CurrentColor);
    }
    public static Vector3 RGBtoHSV(Vector4 color)
    {
        float h, s, v;
        float min = Mathf.Min(Mathf.Min(color.x, color.y), color.z);
        float max = Mathf.Max(Mathf.Max(color.x, color.y), color.z);
        float delta = max - min;

        v = max;

        if (delta == 0f || max == 0f)
        {
            h = s = 0f;
            return new Vector3(h, s, v);
        }

        s = delta / max;

        if (color.x == max)// between yellow & magenta
        {
            h = (color.y - color.z) / delta;
        }
        else if (color.y == max)// between cyan & yellow
        {
            h = 2f + (color.z - color.x) / delta;
        }
        else// between magenta & cyan
        {
            h = 4f + (color.x - color.y) / delta;
        }

        h *= 60;                    // h
        if (h < 0) h += 360;

        return new Vector3(h, s, v);
    }

    public static Vector4 HSVtoRGB(Vector3 hsv)
    {
        if (hsv.y == 0f)      // achromatic (grey)
            return new Vector4(hsv.z, hsv.z, hsv.z, 1f);

        if (hsv.x >= 360f) hsv.x = 359.999f;

        hsv.x /= 60f;           // sector 0 to 5
        int i = (int)Mathf.Floor(hsv.x);
        float f = hsv.x - i;      // factorial part of h
        float p = hsv.z * (1 - hsv.y);
        float q = hsv.z * (1 - hsv.y * f);
        float t = hsv.z * (1 - hsv.y * (1 - f));

        switch (i)
        {
            case 0: return new Vector4(hsv.z, t, p, 1f);
            case 1: return new Vector4(q, hsv.z, p, 1f);
            case 2: return new Vector4(p, hsv.z, t, 1f);
            case 3: return new Vector4(p, q, hsv.z, 1f);
            case 4: return new Vector4(t, p, hsv.z, 1f);
            default: return new Vector4(hsv.z, p, q, 1f);
        }

        return new Vector4(0f, 0f, 0f, 1f);
    }
    public void Draw()
    {
        
        Graphics.DrawTexture(m_Rect, Texture2D.whiteTexture, mat);
        bar_value = GUI.HorizontalSlider(new Rect(m_Rect.x,m_Rect.y - 5, m_Rect.width, 10), bar_value, 0, 1);

        m_Alpha = GUI.HorizontalSlider(new Rect(m_Rect.x, m_Rect.y + 10, m_Rect.width, 10), m_Alpha, 0, 1);

        Event e = Event.current;
        if(e.type==EventType.MouseDrag&&(m_SelectCtrl.x != e.mousePosition.x || m_SelectCtrl.y != e.mousePosition.y) &&m_Rect.Contains(e.mousePosition)&&e.mousePosition.y>=(m_Rect.y+m_Rect.height-m_PanelHeight))
        {

            m_SelectCtrl.x = e.mousePosition.x;
            m_SelectCtrl.y = e.mousePosition.y;
            UpdateCurrentColor();
            GUI.changed = true;
        }
        GUI.Toggle(new Rect(m_SelectCtrl.x - 8, m_SelectCtrl.y-8, 4, 4),false,"");
        
        if (bar_value != backup_bar_value)
        {
            backup_bar_value = bar_value;
            UpdatePanel();
            UpdateCurrentColor();
        }
    }
    public void UpdatePanel()
    {

        Color result = new Color(1, 1, 1);
        Color start = new Color(1, 1, 1);
        Color end = start;

        float d1 = 1.0f / 3.0f;
        float d2 = 2.0f / 3.0f;
        float d3 = 1;
        if (backup_bar_value < d1)
        {
            start = new Color(1, 0, 0);
            end = new Color(0, 1, 0);
            d3 = backup_bar_value / d1;
        }
        else if (backup_bar_value >= d1 && backup_bar_value < d2)
        {
            start = new Color(0, 1, 0);
            end = new Color(0, 0, 1);
            d3 = (backup_bar_value - d1) / d1;
        }
        else if (backup_bar_value >= d2)
        {
            start = new Color(0, 0, 1);
            end = new Color(1, 0, 0);
            d3 = (backup_bar_value - d2) / d1;
        }
        Color d_color = end - start;
        result = start + d_color * d3;
        result.a = 1;
        SetMatColor(result);
        
    }
    public void Destory()
    {
        Material.Destroy(mat);
    }
}
