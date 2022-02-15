using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShaderConsts
{
    public static int LIQUIDITY_ID = Shader.PropertyToID("_Liquidity");
    public static int OVERRIDE_HEGIHT_MAP = Shader.PropertyToID("_OverrideHeightMap");
    public static int BASE_COLOR = Shader.PropertyToID("_BaseColor");
    public static int BASE_MAP = Shader.PropertyToID("_BaseMap");
}
