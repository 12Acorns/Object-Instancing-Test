
#ifndef PROCEDURL_INSTANCE_INCLUDED
#define PROCEDURL_INSTANCE_INCLUDED

struct InstanceData
{
    float3 Position;
    float4 Color;
};

StructuredBuffer<InstanceData> _PerInstanceData;


// use #pragma instancing_options procedural:vertInstancingSetup to setup unity_InstanceID & related macro
#if UNITY_ANY_INSTANCING_ENABLED
    void vertInstancingSetup()
    {

    }
#endif

// Shader Graph Functions
void GetInstancingPos_float(in float3 PositionWS, out float3 Out)
{
    Out = 0;
    #if UNITY_ANY_INSTANCING_ENABLED
        Out = PositionWS + _PerInstanceData[unity_InstanceID].Position;
    #endif
}

// Shader Graph Functions
void GetInstancingColor_float(out float4 Out)
{
    Out = 0;
    #if UNITY_ANY_INSTANCING_ENABLED
        Out = _PerInstanceData[unity_InstanceID].Color;
#endif
}

void GetInstancedID_float(out uint Out)
{
    Out = 0;
    #if UNITY_ANY_INSTANCING_ENABLED
        Out = unity_InstanceID;
    #endif
}

#endif