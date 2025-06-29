sampler2D input        : register(s0);
float4    OverlayColor : register(c0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 src    = tex2D(input, uv);
    float3 tinted = src.rgb * OverlayColor.rgb;
    float3 outRgb = lerp(src.rgb, tinted, OverlayColor.a);
    return float4(outRgb, src.a);
}

technique MultiplyByColor
{
    pass P0 { PixelShader = compile ps_3_0 main(); }
}