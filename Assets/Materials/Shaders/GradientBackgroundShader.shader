Shader "Custom/GradientBackgroundShader"
{
    Properties
    {
        _ColorTop ("Top", Color) = (1,1,1,1)
        _ColorBottom ("Bottom", Color) = (1,1,1,1)
		_LerpColor("Tiling lerp", Range(0,1)) = 0.5 // sliders
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard Lambert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
	fixed4 _ColorTop;
	fixed4 _ColorBottom;
	float _LerpColor;

        struct Input
        {
            float2 uv_MainTex;
			float4 screenPos;
		};

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);


			if (screenUV.x <= 0.5f)
			{
				c *= lerp(_ColorTop, lerp(_ColorBottom, _ColorTop, _LerpColor), screenUV.x * 2);
			}
			else
			{
				c *= lerp(lerp(_ColorBottom, _ColorTop, _LerpColor), _ColorTop, (screenUV.x - 0.5f) * 2);
			}
			
			//c  = tex2D(_MainTex, IN.uv_MainTex) + c;

			o.Albedo = c.rgb;
            o.Alpha = c.a;
        }



        ENDCG
    }
    FallBack "VertexLit"
}
