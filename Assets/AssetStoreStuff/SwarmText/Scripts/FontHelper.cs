// Version 1.0
// ©2011 Happymess Software. All rights reserved. Redistribution of source code without permission not allowed.
// Created By: JohnKnaack@gmail.com

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FontHelper {
	
	private const int TextureSize = 1024;
	private const int ASCII_START_OFFSET = 32;
	
	private const int FontCountX = 10;
    private const int FontCountY = 10;
	private const int FontGridCellWidth = (int)(TextureSize / FontCountX);
	private const int FontGridCellHeight = (int)(TextureSize / FontCountY);
	
	
	public static Vector3[] GetLetterPoints(char letter, Texture2D fontTexture) {
		List<Vector3> points = new List<Vector3>();
		
		Color[] letterColors = getLetterColors(letter, fontTexture);
		int pixelCount = 0;
		for (int y = 0; y < FontGridCellHeight; y++) {
			for (int x = 0; x < FontGridCellWidth; x++) {
				pixelCount = x + (y * FontGridCellWidth);
				if (letterColors[pixelCount] != Color.clear) {
					points.Add(new Vector3(x,y,0));
				}
			}
		}
		
		return points.ToArray();
	}
	
	public static float GetLetterWidth(char letter, Texture2D fontTexture) {
		if (letter == ' ') return 0.4f;
		
		Color[] letterColors = getLetterColors(letter, fontTexture);
		int pixelCount = 0;
		int maxX = 0;
		for (int y = 0; y < FontGridCellHeight; y++) {
			for (int x = 0; x < FontGridCellWidth; x++) {
				pixelCount = x + (y * FontGridCellWidth);
				if (letterColors[pixelCount] != Color.clear) {
					maxX = Mathf.Max(x, maxX);
					continue;
				}
			}
		}
		return ((float)maxX/(float)FontGridCellWidth) + 0.1f;
	}
	
	private static Color[] getLetterColors(char letter, Texture2D fontTexture) {
		Vector2 charTexturePos = getCharacterGridPosition(letter);
		charTexturePos.x *= FontGridCellWidth;
		charTexturePos.y *= FontGridCellHeight;
		return fontTexture.GetPixels((int)charTexturePos.x, fontTexture.height - (int)charTexturePos.y - FontGridCellHeight, FontGridCellWidth, FontGridCellHeight);
	}
	
	private static Vector2 getCharacterGridPosition(char c) {
		int codeOffset = c - ASCII_START_OFFSET;
		return new Vector2(codeOffset % FontCountX, (int)codeOffset / FontCountX);
	}


}
