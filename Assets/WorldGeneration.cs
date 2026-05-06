using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    public int scale = 10;
    public float noiseScale = 10f;

    public int octaves = 5;

    public float _persistence = 0.5f;
    public float _lacunarity = 1.5f;

    public float debugHeight = 10f;

    private void OnDrawGizmos() {

        for (int x = 0; x < scale; x++) {
            for (int y = 0; y < scale; y++) {

                float height = 0f;
                float amplitude = 1f;
                float frequency = 1f;

                float maxValue = 0f;

                for (int i = 0; i < octaves; i++) {

                    float sampleX = x / noiseScale * frequency;
                    float sampleY = y / noiseScale * frequency;

                    height += Mathf.PerlinNoise(sampleX, sampleY);

                    maxValue += amplitude;

                    height *= amplitude;

                    frequency *= _lacunarity;
                    amplitude *= _persistence;
                }

                float value = height / maxValue;

                Gizmos.color = Color.Lerp(Color.black, Color.white, value);

                Vector3 position = new Vector3 (x, debugHeight * value, y);
                Gizmos.DrawCube(position, Vector3.one);

            }
        }
    }
}
