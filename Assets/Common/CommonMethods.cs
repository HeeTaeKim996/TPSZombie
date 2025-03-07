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
            returnLayer |= LayerMask.GetMask(st); // GetMask�� ���̾ ������, 0 ����
        }

        return returnLayer;
    }
}
