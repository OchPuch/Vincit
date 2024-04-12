void GetLight_float(float3 worldPos, out float3 direction, out float3 color, out float distanceAttenuation, out float shadowAttenuation)
{
    #if defined(SHADERGRAPH_PREVIEW)
        direction = half3(0,1, 0);
        color = 1;
        distanceAttenuation = 0.5;
        shadowAttenuation = 0.5;
    #else
    float4 sCoord = TransformWorldToShadowCoord(worldPos);
    
    Light mainLight = GetMainLight(sCoord);
    direction = mainLight.direction;
    color = mainLight.color;
    distanceAttenuation = mainLight.distanceAttenuation;
    shadowAttenuation = mainLight.shadowAttenuation;

    
    
    #endif
}
