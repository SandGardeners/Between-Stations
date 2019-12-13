#ifndef SPRITE_UNLIT_INCLUDED
#define SPRITE_UNLIT_INCLUDED

#include "ShaderShared.cginc"

////////////////////////////////////////
// Vertex structs
//
				
struct VertexInput
{
	float4 vertex : POSITION;
	float4 texcoord : TEXCOORD0;
	fixed4 color : COLOR;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct VertexOutput
{
	float4 pos : SV_POSITION;
	float2 texcoord : TEXCOORD0;
	float2 noiseCoord : TEXCOORD1;
	fixed4 color : COLOR;
#if defined(_FOG)
	UNITY_FOG_COORDS(1)
#endif // _FOG	

	UNITY_VERTEX_OUTPUT_STEREO
};

////////////////////////////////////////
// Vertex program
//


VertexOutput vert(VertexInput input)
{
	VertexOutput output;
	
	UNITY_SETUP_INSTANCE_ID(input);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
	
	output.pos = calculateLocalPos(input.vertex);	
	output.texcoord = calculateTextureCoord(input.texcoord);
	output.color = calculateVertexColor(input.color);
	output.noiseCoord.x = cos(_Time.z)*0.5+0.5;//calculateTextureCoord(input.texcoord);
	output.noiseCoord.y = 0.48;
#if defined(_FOG)
	UNITY_TRANSFER_FOG(output,output.pos);
#endif // _FOG
	
	return output;
}

////////////////////////////////////////
// Fragment program
//


uniform sampler2D _MaskTex;

fixed4 frag(VertexOutput input) : SV_Target
{
	float fraclines = frac((input.texcoord.y * 10) + _Time.y);//small lines
	float scanlines = step(fraclines, 0.02);// cut off based on 0.5
        // big scanline up
	float bigfracline = frac((input.texcoord.y ) - _Time.x * 4);// big gradient line
 


	fixed4 texureColor = calculateTexturePixel(input.texcoord.xy);
	ALPHA_CLIP(texureColor, input.color)

	fixed4 pixel = calculatePixel(texureColor, input.color);
	COLORISE(pixel)

	float _rr = 1;

	pixel = pixel + (bigfracline * 0.08) + (scanlines * 0.01);// * input.color);
	// pixel *= 1-(step(frac(cos(_Time.w)),0.5)*0.05);
   	pixel *= 1-(tex2D(_MaskTex, input.noiseCoord)*0.1);
    pixel.a = 0.5 * (scanlines + bigfracline);// alpha based on scanlines and rim	
   
	return pixel;
}

#endif // SPRITE_UNLIT_INCLUDED