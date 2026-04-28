using UnityEngine;

public class SliceableObject : MonoBehaviour
{
    public int maxSlices = 3;
    public int sliceCount = 0;

    public bool CanSlice()
    {
        return sliceCount < maxSlices;
    }

    public void RegisterSlice()
    {
        sliceCount++;
    }
}