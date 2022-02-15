using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Enes: Requires some heavy refactoring
public static class MatService
{
    private const string COLLECTION_PATH = "Configurations/MatCollection";

    // MatGroupType (Ex. Red, Green, etc..) -> ([Normal or Paint] -> GMat)
    private static Dictionary<MatColorGroup, Dictionary<MatType, GMat>> matsOrganized = new Dictionary<MatColorGroup, Dictionary<MatType, GMat>>();

    static MatService()
    {
        if (!Application.isPlaying) return;

        var matCollection = Resources.Load<MatCollection>(COLLECTION_PATH);

        foreach (var mat in matCollection.Mats)
        {
            if (!matsOrganized.ContainsKey(mat.MatColorGroup))
                matsOrganized.Add(mat.MatColorGroup, new Dictionary<MatType, GMat>());

            if (!matsOrganized[mat.MatColorGroup].ContainsKey(mat.MatType))
                matsOrganized[mat.MatColorGroup].Add(mat.MatType, mat);
            else
                Debug.Log("Duplicate Mat, Mat Color Group: " + mat.MatColorGroup + ", Mat Type: " + mat.MatType);
        }

    }

    //Deprecate these since now we store MatColorGroup and MatType
    public static GMat GetMat(MatColorGroup matColorGroup)
    {
        return matsOrganized[matColorGroup][MatType.Normal];
    }
    public static GMat GetMat(MatColorGroup matColorGroup, MatType matType)
    {
        return matsOrganized[matColorGroup][matType];
    }


    //TODO Enes: Move to its own extension class?
    //Deprecate the next three methods...
    public static void SetMaterialPropertiesWithGMat(this GameObject gameObject, GMat gMat)
    {
        if (gMat == null)
        {
            Debug.Log("GMat is null, UpdateObjectWithGMat()");
            return;
        }

        Renderer renderer = gameObject.GetComponentInChildren<Renderer>();
        if (renderer == null)
        {
            Debug.Log(gameObject.name + " Has no Renderer, can't update GMat");
            return;
        }

        renderer.materials[0].SetColor("_BaseColor", gMat.Color);
    }
    public static void UpdateObjectWithGMat(GameObject gameObject, GMat gMat)
    {
        if (gMat == null)
        {
            Debug.Log("GMat is null, UpdateObjectWithGMat()");
            return;
        }

        Renderer renderer = gameObject.GetComponentInChildren<Renderer>();
        if (renderer == null)
        {
            Debug.Log(gameObject.name + " Has no Renderer, can't update GMat");
            return;
        }

        if (gMat.Material != null)
        {
            renderer.material = gMat.Material;
            //	renderer.materials[0].SetColor("_BaseColor", gMat.Color);
        }
        else
        {
            renderer.materials[0].SetColor("_BaseColor", gMat.Color);
        }
    }
    public static void UpdateObjectWithGMat(GameObject gameObject, MatColorGroup matGroup, MatType matType = MatType.Normal)
    {
        if (matGroup == MatColorGroup.UNDEFINED)
            return;

        Renderer renderer = gameObject.GetComponentInChildren<Renderer>();
        if (renderer == null)
        {
            Debug.Log(gameObject.name + " Has no Renderer, can't update GMat");
            return;
        }
        GMat gMat = GetMat(matGroup, matType);
        if (gMat.Material != null)
        {
            renderer.materials[0] = gMat.Material;
            renderer.materials[0].SetColor("_BaseColor", gMat.Color);
        }
        else
        {
            renderer.materials[0].SetColor("_BaseColor", gMat.Color);
        }
    }

    public static void SetMaterial(GameObject gameObject, MatColorGroup matGroup, MatType matType = MatType.Normal)
    {
        if (matGroup == MatColorGroup.UNDEFINED) return;

        Renderer renderer = GetRenderer(gameObject);
        if (renderer == null) return;

        GMat gMat = GetGMat(matGroup, matType);
        if (gMat == null) return;

        SetMaterialInternal(renderer, gMat);
    }
    public static void SetMaterialColor(GameObject gameObject, MatColorGroup matGroup, MatType matType = MatType.Normal)
    {
        if (matGroup == MatColorGroup.UNDEFINED) return;

        Renderer renderer = GetRenderer(gameObject);
        if (renderer == null) return;

        GMat gMat = GetGMat(matGroup, matType);
        if (gMat == null) return;

        SetMaterialColorInternal(renderer, gMat);
    }
    public static void SetMaterialAndColor(GameObject gameObject, MatColorGroup matGroup, MatType matType = MatType.Normal)
    {
        if (matGroup == MatColorGroup.UNDEFINED) return;

        Renderer renderer = GetRenderer(gameObject);
        if (renderer == null) return;

        GMat gMat = GetGMat(matGroup, matType);
        if (gMat == null) return;

        if (gMat.Material != null) SetMaterialInternal(renderer, gMat);
        SetMaterialColorInternal(renderer, gMat);
    }

    private static GMat GetGMat(MatColorGroup matGroup, MatType matType)
    {
        GMat gMat = matsOrganized[matGroup][matType];
        if (gMat == null)
        {
            Debug.Log($"{matGroup.ToString()} {matType.ToString()} Don't exist");
            return null;
        };
        return gMat;
    }
    private static Renderer GetRenderer(GameObject gameObject)
    {
        Renderer renderer = gameObject.GetComponentInChildren<Renderer>();
        if (renderer == null)
        {
            Debug.Log(gameObject.name + " Has no Renderer", gameObject);
            return null;
        }
        return renderer;
    }

    private static void SetMaterialInternal(Renderer renderer, GMat gMat)
    {
        renderer.sharedMaterial = gMat.Material;
    }
    private static void SetMaterialColorInternal(Renderer renderer, GMat gMat)
    {
        renderer.sharedMaterial.SetColor(ShaderConsts.BASE_COLOR, gMat.Color);
    }

}