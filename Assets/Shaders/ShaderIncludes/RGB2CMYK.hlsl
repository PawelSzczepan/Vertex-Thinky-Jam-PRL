// From the book: Graphics Shaders

float3 CMYKtoRGB (float4 cmyk) {
    return float3(1-cmyk.x,1-cmyk.y,1-cmyk.z);
}

float4 RGBtoCMYK (float3 rgb) {
    return float4(1-rgb.r,1-rgb.g,1-rgb.b,1);
   
}

void RGBtoCMYK_float(float3 rgb, out float4 cmyk)
{
    
    cmyk = RGBtoCMYK(rgb);
}

void CMYKtoRGB_float(float4 cmyk, out float3 rgb)
{
    rgb = CMYKtoRGB(cmyk);
}