using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drawing : MonoBehaviour
{
    public RawImage map;
    public int mapWidth;
    public int mapHeight;
    private Texture2D texture;
    public Vector2 screenPos;
    public Vector2 mapPos;
    public int penSize;
    public int rubberSize;
    public Color mapColour;
    public Color penColour1;
    public Color penColour2;
    private Color currentColour;
    void Start()
    {
        texture = new Texture2D(mapWidth, mapHeight);
        texture.filterMode = FilterMode.Point;

        Color[] fillPixels = new Color[mapWidth * mapHeight];
        for (int i = 0; i < fillPixels.Length; i++)
            fillPixels[i] = mapColour;
        texture.SetPixels(fillPixels);
        texture.Apply();
        
        // Assign to UI
        map.texture = texture;
        currentColour = penColour1;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentColour = penColour1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentColour = penColour2;
        }
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject())
        {
            
            Vector2 pixelPos = GetMousePos();
            for (int x = -penSize; x <=penSize; ++x)
            {
                for (int y = -penSize; y <=penSize; ++y)
                {
                    texture.SetPixel(((int)pixelPos.x) + x, ((int)pixelPos.y) + y, currentColour);
                }
            }
            texture.Apply();
            map.texture = texture;
        }
        else if (Input.GetMouseButton(1) && EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 pixelPos = GetMousePos();
            for (int x = -rubberSize; x <=rubberSize; ++x)
            {
                for (int y = -rubberSize; y <=rubberSize; ++y)
                {
                    texture.SetPixel(((int)pixelPos.x) + x, ((int)pixelPos.y) + y, mapColour);
                }
            }
            
            texture.Apply();
            map.texture = texture;
        }
    }

    Vector2 GetMousePos()
    {
        screenPos = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(map.rectTransform, screenPos, null, out mapPos);
            
            Rect rect = map.rectTransform.rect;
            float normalizedX = (mapPos.x - rect.x) / rect.width;   
            float normalizedY = (mapPos.y - rect.y) / rect.height;  

            int pixelX = (int)(normalizedX * mapWidth);  
            int pixelY = (int)(normalizedY * mapHeight);

            return new Vector2(pixelX, pixelY);
    }

    public void ResetMap()
    {
        if (texture == null || map == null)
        {
            return;
        }
    
        Color[] fillPixels = new Color[mapWidth * mapHeight];
        for (int i = 0; i < fillPixels.Length; i++)
            fillPixels[i] = mapColour;
        texture.SetPixels(fillPixels);
        texture.Apply();
        map.texture = texture;
    }
}
