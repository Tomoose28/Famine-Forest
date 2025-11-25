using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Lighting : MonoBehaviour
{
    public Color lightColour0;
    public Color lightColour1;
    public Color lightColour2;
    public Color lightColour3;
    public Tilemap tileMap;
    public Tilemap groundMap;
    public Tilemap markerMap;
    public Tile light1;
    public Tile light2;
    public List<Tile> light3;
    public Tile groundTile;
  
    public List<Tile> lightTiles;
    public List<Tile> midTiles;
    public List<Tile> darkTiles;
    public List<Tile> emptyTiles;
    public Vector3Int location;
    public LayerMask obstical;
    public int lightRadius;
    public int emptyRadius;
    public int midRadius;
    public int darkRadius;
    public int penetration;

    public Tile berry;
    public Tile mushroom;
    public Tile emptyBerry;
    public Tile emptyMushroom;

    void Start()
    {
        
    }

    public void SetLight(Vector3 playerPos)
    {
            location = tileMap.WorldToCell(playerPos);

            for (int x = -emptyRadius; x <= emptyRadius; ++x)
            {
                for (int y = -emptyRadius; y <= emptyRadius; ++y)
                {
                    Vector3Int newLocation = new Vector3Int(location.x + x, location.y + y, location.z);
                    Tile currentTile = tileMap.GetTile<Tile>(newLocation);
                    Vector3 cellWorldPos = tileMap.GetCellCenterWorld(newLocation);
                    float distance = Vector3.Distance(playerPos, cellWorldPos);

                    Tile newTile = null;
                    int index = -1;

                    if (lightTiles.Contains(currentTile))
                        index = GetIndex(lightTiles, currentTile);
                    else if (midTiles.Contains(currentTile))
                        index = GetIndex(midTiles, currentTile);
                    else if (darkTiles.Contains(currentTile))
                        index = GetIndex(darkTiles, currentTile);
                    else if (emptyTiles.Contains(currentTile))
                        index = GetIndex(emptyTiles, currentTile);
            
                    if (index < 0) continue;

                    if (distance <= lightRadius)
                    {
                        newTile = lightTiles[index];
                    }
                    else if (distance <= midRadius)
                    {
                        newTile = midTiles[index];
                    }
                    else if (distance <= darkRadius)
                    {
                        newTile = darkTiles[index];
                    }
                    else if (distance <= emptyRadius)
                    {
                        newTile = emptyTiles[index];
                    }
                    else
                    {
                        newTile = emptyTiles[index];
                    }

                    if (newTile != null && newTile != currentTile)
                    {
                        tileMap.SetTile(newLocation, newTile);
                    }

                }
            }
    }       


    public void SetGroundLight(Vector3 playerPos)
    {
        location = groundMap.WorldToCell(playerPos);
        for (int x = -4; x <= 4; ++x)
        {
            for (int y = -4; y <= 4; ++y)
            {
                Vector3Int newLocation = new Vector3Int(location.x + x, location.y + y, location.z);
                Tile currentTile = groundMap.GetTile<Tile>(newLocation);
                
                if (currentTile == emptyBerry || currentTile == emptyMushroom)
                {
                    continue; 
                }
                
                Vector3 cellWorldPos = groundMap.GetCellCenterWorld(newLocation);
                float distance = Vector3.Distance(playerPos, cellWorldPos);

                Tile newTile = null;
                Color tileColour = Color.white;

                if (distance <= 1)
                {
                    newTile = light1;
                    tileColour = RandomColour(lightColour0, lightColour1);
                }
                else if (distance <= 2)
                {
                    newTile = light2;
                    tileColour = RandomColour(lightColour1, lightColour2);
                }
                else if (distance <= 3)
                {
                    newTile = RandomTile(light3);
                    tileColour = RandomColour(lightColour2, lightColour3);
                }
                else
                {
                    newTile = groundTile;
                    tileColour = Color.white;
                }
                if (newTile != null)
                {
                    
                    groundMap.SetTile(newLocation, newTile);
                    groundMap.SetTileFlags(newLocation, TileFlags.None);
                    groundMap.SetColor(newLocation, tileColour);
                }
            }
        }
    }

    public int GetIndex(List<Tile> tileList, Tile currentTile)
    {
        for (int i = 0; i < tileList.Count; i++)
        {
            if (currentTile == tileList[i])
            {
                return i;
            }
        }

        return 0;
    }

    public void ClearScreen()
    {

        BoundsInt markerBounds = markerMap.cellBounds;
        foreach (Vector3Int pos in markerBounds.allPositionsWithin)
        {
            Tile currentTile = markerMap.GetTile<Tile>(pos);

            if (currentTile == emptyBerry)
            {
                tileMap.SetTile(pos, berry);
                markerMap.SetTile(pos, null);
            }
            else if (currentTile == emptyMushroom)
            {
                tileMap.SetTile(pos, mushroom);
                markerMap.SetTile(pos, null);
            }
        }

        BoundsInt groundBounds = groundMap.cellBounds;
        foreach (Vector3Int pos in groundBounds.allPositionsWithin)
        {

            Tile currentTile = groundMap.GetTile<Tile>(pos);
            
            
            if (currentTile != groundTile)
            {
                groundMap.SetTile(pos, groundTile);
            } 
        }

        BoundsInt bounds = tileMap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            
            Tile currentTile = tileMap.GetTile<Tile>(pos);
             
            if (lightTiles.Contains(currentTile))
                {
                    tileMap.SetTile(pos, emptyTiles[GetIndex(lightTiles, currentTile)]);
                }
            else if (darkTiles.Contains(currentTile))
                {
                    tileMap.SetTile(pos, emptyTiles[GetIndex(darkTiles, currentTile)]);
                }
            else if (midTiles.Contains(currentTile))
                {
                    tileMap.SetTile(pos, emptyTiles[GetIndex(midTiles, currentTile)]);
                }
        }
    }

    public string CheckTile(Vector3 playerPos)
    {
        location = tileMap.WorldToCell(playerPos);
        Tile currentTile = tileMap.GetTile<Tile>(location);
        if (currentTile == berry)
        {
            tileMap.SetTile(location, null);
            markerMap.SetTile(location, emptyBerry);
            return "berry";
        }
        else if (currentTile == mushroom)
        {
            tileMap.SetTile(location, null);
            markerMap.SetTile(location, emptyMushroom);
            return "mushroom";
        }

        return null;

    }

    public Color RandomColour(Color c1, Color c2)
    {
        float t = UnityEngine.Random.Range(0f,1f);
        Color randomColor = Color.Lerp(c1, c2, t);
        return randomColor;
    }

    public Tile RandomTile(List<Tile> lightTiles)
    {
        int n = UnityEngine.Random.Range(0, 3);
        Tile lightTile = lightTiles[n];    
        return lightTile;
    }



}
