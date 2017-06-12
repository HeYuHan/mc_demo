using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PerlinNoise:SerializableObject
{
    public static float Noise(int x,int random)
    {
        x *= random;
        x = (x << 13) ^ x;
        return (1.0f - ((x * (x * x * 15731 + 789221) + 1376312589) & 0x7fffffff) / (1073741824.0f));
    }
    public static float Noise(int x, int y,int random)
    {
        x += y * 32;
        return Noise(x, random);
    }
    public static float LinearInterpolate(float from,float to,float d)
    {
        return from * (1 - d) + to * d;
    }
    public static float CosineInterpolate(float from,float to,float d)
    {
        float ft = d * Mathf.PI;
        float f = (1 - Mathf.Cos(ft)) * 0.5f;
        return from * (1 - f) + to * f;
    }
    public static float CubeInterpolate(float from1,float from2,float to1,float to2,float d)
    {
        float P = (to2 - to1) - (from2 - from1);
        float Q = (from1 - from2) - P;
        float R = to1 - from1;
        float S = from2;
        return P * Mathf.Pow(d, 3) + Q * Mathf.Pow(d, 2) + R * d + S;
    }
    [SerializeField]
    int m_RandomValue;
    [SerializeField][Range(0,1)]
    float m_Persistence=0.5f;
    [SerializeField]
    int m_Octaves=2;
    [SerializeField]
    [Range(0, 1)]
    float m_Delta = 0.2f;
    public PerlinNoise(float per, int oct)
    {
        m_Persistence = per;
        m_Octaves = oct;
        m_RandomValue = new System.Random().Next(-100, 100);
    }
    public PerlinNoise()
    {
        m_RandomValue = new System.Random().Next(-100, 100);
    }
    public void SetPersistence(float per)
    {
        m_Persistence = per;
    }
    public void SetOctaves(int oct)
    {
        m_Octaves = oct;
    }
    public float PerlinNoise1D(float x)
    {
        x *= m_Delta;
        float result = 0;
        float p = m_Persistence;
        float n = m_Octaves;
        for(int i=0;i<n;i++)
        {
            float frequency = Mathf.Pow(2, i);
            float amplitude = Mathf.Pow(p, i);
            result += InterpolatedNoise1D(x * frequency) * amplitude;
        }
        return result;
    }
    public float PerlinNoise2D(float x,float y)
    {
        x *= m_Delta;
        y *= m_Delta;
        float result = 0;
        float p = m_Persistence;
        float n = m_Octaves;
        for (int i = 0; i < n; i++)
        {
            float frequency = Mathf.Pow(2, i);
            float amplitude = Mathf.Pow(p, i);
            result += InterpolatedNoise2D(x * frequency,y*frequency) * amplitude;
        }
        return result;
    }
    float SmoothedNoise1D(int x)
    {
        return Noise(x, m_RandomValue) / 2 + Noise(x-1, m_RandomValue) / 4 + Noise(x+1, m_RandomValue) / 4;
    }
    float InterpolatedNoise1D(float x)
    {
        int int_x = (int)x;
        float d_x = x - int_x;
        float v1 = SmoothedNoise1D(int_x);
        float v2 = SmoothedNoise1D(int_x + 1);
        return CosineInterpolate(v1, v2, d_x);
    }

    float SmoothedNoise2D(int x,int y)
    {
        float corners = (Noise(x - 1, y - 1, m_RandomValue) + Noise(x + 1, y - 1, m_RandomValue) + Noise(x - 1, y + 1, m_RandomValue) + Noise(x + 1, y + 1, m_RandomValue)) / 16;
        float sides = (Noise(x - 1, y, m_RandomValue) + Noise(x + 1, y, m_RandomValue) + Noise(x, y - 1, m_RandomValue) + Noise(x, y + 1, m_RandomValue)) / 8;
        float center = Noise(x, y, m_RandomValue) / 4;
        return corners + sides + center;
    }
    float InterpolatedNoise2D(float x,float y)
    {
        int int_x = (int)x;
        float d_x = x - int_x;
        int int_y = (int)y;
        float d_y = y - int_y;
        float v1 = SmoothedNoise2D(int_x, int_y);
        float v2 = SmoothedNoise2D(int_x + 1, int_y);
        float v3 = SmoothedNoise2D(int_x, int_y + 1);
        float v4 = SmoothedNoise2D(int_x + 1, int_y + 1);
        float i1 = CosineInterpolate(v1, v2, d_x);
        float i2 = CosineInterpolate(v3, v4, d_x);
        return CosineInterpolate(i1, i2, d_y);
    }
}
