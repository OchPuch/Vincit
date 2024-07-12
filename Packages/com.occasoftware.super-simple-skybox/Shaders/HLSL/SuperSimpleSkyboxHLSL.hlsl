#ifndef SUPERSIMPLESKYBOX_INCLUDED
#define SUPERSIMPLESKYBOX_INCLUDED

#ifdef UNIVERSAL_LIGHTING_INCLUDED
float3 _WorldSpaceLightPos0;
#endif
float3 _SunDirection;
float3 _MoonDirection;


void GetSkyboxLights_float(out float3 SunDirection, out float3 MoonDirection){
	SunDirection = _SunDirection;
	MoonDirection = _MoonDirection;
}

float easeIn(float t)
{
	return t * t;
}

float easeOut(float t)
{
	return 1.0 - easeIn(1.0 - t);
}


float easeInOut(float t)
{
	float a = easeIn(t);
	float b = easeOut(t);
	return lerp(a, b, t);
}

float rand3dTo1d(float3 vec, float3 dotDir = float3(12.9898, 78.233, 37.719))
{
	float random = dot(sin(vec), dotDir);
	random = frac(sin(random) * 143758.5453);
	return random;
}

float3 rand3dTo3d(float3 vec, float3 seed = 4605)
{
	return float3
    (
        rand3dTo1d(vec + seed),
        rand3dTo1d(vec + seed, float3(39.346, 11.135, 83.155)),
        rand3dTo1d(vec + seed, float3(73.156, 52.235, 9.151))
    );
}

float map(float v, float inFrom, float inTo, float outFrom, float outTo)
{
	v = clamp(v, inFrom, inTo);
	return (v - inFrom) / (inTo - inFrom) * (outTo - outFrom) + outFrom;
}

// Targets a results range of 2400K -> 40000K
// Model based on Blackbody Color Datafile provided by Mitchell Charity
// http://www.vendian.org/mncharity/dir3/blackbody/UnstableURLs/bbr_color.html
float3 BlackbodyColor(float T)
{
	float3 col = float3(1, 1, 1);
	
	float2 VALID_RANGE = float2(2400, 40000);
	T = clamp(T, VALID_RANGE.x, VALID_RANGE.y);
	
	
	// Handle Red 
	// (R^2 = 0.943) across full range
	col.r = 74.4 * pow(T, -0.522);
	
	
	// Handle Green
	if (T <= 6600)
	{
		// R^2 = 0.987
		col.g = 0.000146 * T + 0.0327;
	}
	else if(T > 6600 && T < 10500)
	{
		// Roll off adjustment factor towards 10.5K
		col.g = 9.51 * pow(T, -0.283) + map(T, 6600, 10500, 0.2069, 0);
	}
	else
	{
		// R^2 = 0.916
		col.g = 9.51 * pow(T, -0.283);
	}
	
	// Handle Blue (R^2 = 0.998)
	// Returns 0.9966 at 6600
	if (T < 6600)
	{
		col.b = 0.000236 * T + -0.561;
	}
	
	return saturate(col);
}

// Take in a value in range (0, 1], return back color temperature
// Model based on Effective Temperature data available from VizieR
// http://vizier.u-strasbg.fr/viz-bin/VizieR-4
float GetTemperature(float vec)
{
	float EPSILON = 0.00001;
	vec = clamp(vec, EPSILON, 1.0);
	return 2400 * pow(vec, -0.378);
}


float GradientNoise3d(float3 vec)
{
	float3 f = frac(vec);
	float3 t = float3(easeInOut(f.x), easeInOut(f.y), easeInOut(f.z));
	
	float cellNoiseZ[2];
	for (int z = 0; z <= 1; z++)
	{
		float cellNoiseY[2];
		for (int y = 0; y <= 1; y++)
		{
			float cellNoiseX[2];
			for (int x = 0; x <= 1; x++)
			{
				float3 cell = floor(vec) + float3(x, y, z);
				float3 dir = rand3dTo3d(cell) * 2 - 1;
				float3 comparator = f - float3(x, y, z);
				cellNoiseX[x] = dot(dir, comparator);
			}
			cellNoiseY[y] = lerp(cellNoiseX[0], cellNoiseX[1], t.x);
		}
		cellNoiseZ[z] = lerp(cellNoiseY[0], cellNoiseY[1], t.y);
	}
	float noise = lerp(cellNoiseZ[0], cellNoiseZ[1], t.z);
	return noise;
}


float4x4 _MainLightMatrix;
void GetStars_float(float3 viewDir, float sharpness, float frequency, out float3 Out)
{
	float3 vec = normalize(viewDir);
	vec = mul(vec, _MainLightMatrix).xyz;
	
	sharpness *= 8;
	frequency *= 300;
	
	float brightness = 600;
	
	float noise = saturate(GradientNoise3d(vec * frequency));
	float star = pow(noise, sharpness);
	star *= brightness;
	
	float background = saturate(GradientNoise3d(vec * frequency * 0.015) + 0.5);
	background = lerp(0.2, 2.0, background * background);
	star *= background;
	
	float flicker = saturate(GradientNoise3d(vec * frequency * 1.2 + _Time.y * 3.0));
	flicker = lerp(0.3, 1.7, flicker);
	star *= flicker;
	
	float temperature01 = saturate(GradientNoise3d(vec * frequency * 0.7) + 0.5);
	float temperature = GetTemperature(temperature01);
	float3 color = BlackbodyColor(temperature);
	float3 backgroundColor = background * float3(0.176, 0.467, 0.823) * 0.00003 * brightness;
	float3 starColor = star * color;
	Out = backgroundColor + starColor;
}

SamplerState linear_repeat_sampler;
void GetClouds_float(float2 uv, Texture2D tex, float2 wind, int2 scale, int iterations, float gain, int lacunarity, out float value)
{
    float2 windFactor = wind * _Time.y * 0.01;
    uv = uv * scale + windFactor;
    value = 0.0;
    float amplitude = 1.0;
    float frequency = 1.0;
    float sum = 0.0;
	#define MAX_ITERATIONS 4
    for (int i = 1; i <= iterations && i <= MAX_ITERATIONS; i++)
    {
        sum += amplitude;
        value += tex.Sample(linear_repeat_sampler, uv * frequency + windFactor).r * amplitude;
        amplitude *= gain;
        frequency *= lacunarity;
    }
	
    value /= sum + 1e-10;
    saturate(value);

}

#endif