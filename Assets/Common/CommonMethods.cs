using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommonMethods
{
    public static LayerMask StringsToLayerMask(List<string> strings)
    {
        LayerMask returnLayer = 0;

        foreach (string st in strings)
        {
            returnLayer |= LayerMask.GetMask(st); // GetMask로 레이어가 없을시, 0 리턴
        }

        return returnLayer;
    }
}
