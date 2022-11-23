using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Volume : MonoBehaviour
{
    [SerializeField] private FrustrumCulling frustrumCulling;


    public abstract bool isOnFrustrum(FrustrumCulling.Frustrum camFrustum, Transform modelTransform);

    public abstract bool isOnOrForwardPlan(FrustrumCulling.Plane plan);


    bool isOnFrustum(FrustrumCulling.Frustrum camFrustum)
    {
        return (isOnOrForwardPlan(camFrustum.leftPlane) &&
                isOnOrForwardPlan(camFrustum.rightPlane) &&
                isOnOrForwardPlan(camFrustum.topPlane) &&
                isOnOrForwardPlan(camFrustum.bottomPlane) &&
                isOnOrForwardPlan(camFrustum.nearPlane) &&
                isOnOrForwardPlan(camFrustum.farPlane));
    }

}
