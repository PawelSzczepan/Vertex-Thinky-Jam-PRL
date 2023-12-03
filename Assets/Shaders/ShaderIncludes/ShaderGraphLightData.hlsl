

#ifndef ShaderGraphLightingData
#define ShaderGraphLightingData
#endif
#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
#pragma multi_compile_fragment _ _SHADOWS_SOFT
#pragma multi_compile_fragment _ _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN

half Specular(float3 normalWS, float3 lightDirectionWS, float3 viewDirectionWS, float smoothness)
{
    #ifdef UNIVERSAL_LIGHTING_INCLUDED
    float roughness = PerceptualSmoothnessToRoughness(smoothness);
    float perceptualRoughness = RoughnessToPerceptualRoughness(roughness);
    float roughness2 = max(perceptualRoughness*perceptualRoughness,HALF_MIN);
    float roughness2MinusOne = roughness-1;
    float normalizationTerm = roughness * 4.0h + 2.0h;
    float3 lightDirectionWSFloat3 = float3(lightDirectionWS);
    float3 halfDir = SafeNormalize(lightDirectionWSFloat3 + float3(viewDirectionWS));

    float NoH = saturate(dot(float3(normalWS), halfDir));
    half LoH = half(saturate(dot(lightDirectionWSFloat3, halfDir)));

    // GGX Distribution multiplied by combined approximation of Visibility and Fresnel
    // BRDFspec = (D * V * F) / 4.0
    // D = roughness^2 / ( NoH^2 * (roughness^2 - 1) + 1 )^2
    // V * F = 1.0 / ( LoH^2 * (roughness + 0.5) )
    // See "Optimizing PBR for Mobile" from Siggraph 2015 moving mobile graphics course
    // https://community.arm.com/events/1155

    // Final BRDFspec = roughness^2 / ( NoH^2 * (roughness^2 - 1) + 1 )^2 * (LoH^2 * (roughness + 0.5) * 4.0)
    // We further optimize a few light invariant terms
    // brdfData.normalizationTerm = (roughness + 0.5) * 4.0 rewritten as roughness * 4.0 + 2.0 to a fit a MAD.
    float d = NoH * NoH * roughness2MinusOne + 1.00001f;

    half LoH2 = LoH * LoH;
    half specularTerm = roughness2 / ((d * d) * max(0.1h, LoH2) * normalizationTerm);
    return specularTerm;
    #endif
    return 0;
}

void GetAdditionalLightData(float3 positionWS,float3 normalWS, float3 viewDirection,float smoothness, out half3 diffuse, out half3 specular,out float3 avgLightDir,out half3 avgColor)
{
    half3 vertexLightColor = half3(0, 0, 0);
    half3 diffuseComponent = 0;
    half3 specularComponent = 0;
    float3 lightDir=0;
    half3 averageColor = 0;
    #ifdef UNIVERSAL_LIGHTING_INCLUDED
    Light mainLight = GetMainLight(TransformWorldToShadowCoord(positionWS));
    // half mainLightShadowAtten = MainLightRealtimeShadow(TransformWorldToShadowCoord(positionWS));
    // mainLight.shadowAttenuation = mainLightShadowAtten;
    half ndotL = saturate(dot(normalWS,mainLight.direction));
    averageColor+= mainLight.distanceAttenuation * mainLight.color;
    half3 attenuatedLightColor =  (mainLight.distanceAttenuation * mainLight.shadowAttenuation)* ndotL* mainLight.color;
    diffuseComponent+= attenuatedLightColor;
    half3 spec = Specular(normalWS,mainLight.direction,viewDirection,smoothness)*attenuatedLightColor * mainLight.color;
    specularComponent+= spec;
    lightDir += mainLight.direction;
    
    uint lightsCount = GetAdditionalLightsCount();

    LIGHT_LOOP_BEGIN(lightsCount)
        Light light = GetAdditionalLight(lightIndex, positionWS,0);
    
       // light.shadowAttenuation = shadowAttenuation;
        averageColor+= light.distanceAttenuation * light.color;

        half ndotL = saturate(dot(normalWS,light.direction));
        attenuatedLightColor = saturate((light.distanceAttenuation * light.shadowAttenuation)* ndotL)* light.color;
        diffuseComponent += attenuatedLightColor;
        half3 spec = Specular(normalWS,light.direction,viewDirection,smoothness)*attenuatedLightColor*light.color;
        specularComponent+= spec;
        lightDir += light.direction;
    LIGHT_LOOP_END
    
    avgLightDir = normalize(lightDir/(lightsCount+1));
    //averageColor/=(lightsCount+1);
    avgColor = averageColor;
    diffuse = diffuseComponent;
    specular = specularComponent;
    //return vertexLightColor;
    #endif
}

void GetAdditionalLightData_float(float3 positionWS,float3 normalWS, float3 viewDirection,float smoothness, out half3 diffuse, out half3 specular,out float3 avgLightDir,out half3 avgColor)
{
    diffuse=0;
    specular=0;
    avgLightDir=0;
    avgColor = 0;
    #ifndef UNIVERSAL_LIGHTING_INCLUDED
    return;
    #endif
    GetAdditionalLightData(positionWS,normalWS,viewDirection,smoothness,diffuse,specular,avgLightDir,avgColor);
}