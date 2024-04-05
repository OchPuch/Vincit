void getUV_float(float3 pos, float3 normal, out float2 uv)
{
    #if defined(SHADERGRAPH_PREVIEW)
        uv = float2(1, 0);
    #else
    
    float temp;
    float3 n;
    
    if(normal.x < 0)
        n.x = -normal.x;
    else
        n.x = normal.x;
    
    if(normal.y < 0)
        n.y = -normal.y;
    else
        n.y = normal.y;
    
    if(normal.z < 0)
        n.z = -normal.z;
    else
        n.z = normal.z;
    
    if(normal.y < 0)
        n.y = -normal.y;
    else
        n.y = normal.y;

    if(n.x > n.z)
        temp = pos.z;
    else
        temp = pos.x;


    if(n.y > n.x + n.z)
        uv = float2(pos.x, pos.z);
    else
        uv = float2(temp, pos.y);
    
    #endif
    
}