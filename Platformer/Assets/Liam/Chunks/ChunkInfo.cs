using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class ChunkInfo
{
    public const char charStart = '>', charEnd = ':', charEmpty = '.';
    public const char charGround = '=', charRampUp = '/', charRampDown = '\\', charCheckpoint = '!';
    public const char charInstantDeath = 'X';
    public const char charEnemyScarySpider = 'S';
    public enum Zones { Any, Grasslands };

    public string Name;
    public Zones Zone;
    public int MinLevelStory;
    public int MinDistanceStory;
    public int MinDistanceEndless;
    public int DropChance;              //0 to 100
    public int DangerRating;            //-100 to 100
    public char[,] Map;
    public Vector2 StartIndex;
    public Vector2 EndIndex;
    public int Width;


    

    public ChunkInfo()
    {

    }

    public ChunkInfo(XmlNode node)
    {
        try
        {
           
                Name = node.SelectSingleNode("Name").InnerText;
                Zone = GetZoneFromText(node.SelectSingleNode("Zone").InnerText);
                MinLevelStory = Convert.ToInt16(node.SelectSingleNode("MinLevelStory").InnerText);
                MinDistanceStory = Convert.ToInt16(node.SelectSingleNode("MinDistanceStory").InnerText);
                MinDistanceEndless = Convert.ToInt16(node.SelectSingleNode("MinDistanceEndless").InnerText);
                DropChance = Convert.ToInt16(node.SelectSingleNode("MinLevelStory").InnerText);
                Map = GetMapFromText(node.SelectSingleNode("Map").InnerText);
                StartIndex = FindPosInMap(charStart);
                EndIndex = FindPosInMap(charEnd);
                Width = Map.GetLength(0);
            
        }
        catch (System.Exception ex)
        {
            Debug.LogError("ChunkInfoLoad: Malformed Chunk " + ex);
        }
    }

    public Vector2 FindPosInMap(char c)
    {
        for (int y = 0; y < Map.GetLength(1); y++)
        {
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                if (Map[x, y] == c)
                    return new Vector2(x, y);
            }
        }

        return new Vector2(-1, -1);
    }

    private char[,] GetMapFromText(string text)
    {
        text = text.Replace("\r\n", "");
        text = text.Trim();

        string[] lines = text.Split(new char[] { ';' });

        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].Trim();
        }

        char[,] output = new char[lines[1].Length, lines.Length];

        for (int y = 0; y < lines.Length; y++)
        {
            if (lines[y] == "")
            {
                for (int x = 0; x < lines[1].Length; x++)
                {
                    output[x, y] = charEmpty;
                }
            }
            else
            {
                for (int x = 0; x < lines[1].Length; x++)
                {
                    output[x, y] = lines[y][x];
                }
            }
        }

        return output;
    }

    private Zones GetZoneFromText(string text)
    {
        Zones output;

        switch (text)
        {
            case "Grasslands":
                output = Zones.Grasslands;
                break;
            default:
                output = Zones.Any;
                break;

        }

        return output;
    }
}
