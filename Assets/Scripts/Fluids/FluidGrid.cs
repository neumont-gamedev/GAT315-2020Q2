using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FluidGrid
{
    public float[] density;
    public float[] vx;
    public float[] vy;

    public float[] prev_density;
    public float[] prev_vx;
    public float[] prev_vy;

    public FluidCell[] cells;

    public void Create(int size)
    {
        density = new float[size];
        vx = new float[size];
        vy = new float[size];

        prev_density = new float[size];
        prev_vx = new float[size];
        prev_vy = new float[size];

        cells = new FluidCell[size];
    }

    static public void Swap(ref float[] f1, ref float[] f2)
    {
        float[] temp = f1;
        f1 = f2;
        f2 = temp;
    }
}
