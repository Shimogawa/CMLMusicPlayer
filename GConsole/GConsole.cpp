// GConsole.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "GConsole.h"

CHAR_INFO * buffer;
int width;
int height; 
HANDLE frontBuffer;
HANDLE backBuffer;

GCONSOLE_API void  InitializeBuffer(int w, int h){
	width = w;
	height = h;
	buffer = new CHAR_INFO[w * h];
	frontBuffer = CreateConsoleScreenBuffer(
		GENERIC_READ | GENERIC_WRITE, 0, NULL, CONSOLE_TEXTMODE_BUFFER, NULL);
	backBuffer = CreateConsoleScreenBuffer(
		GENERIC_READ | GENERIC_WRITE, 0, NULL, CONSOLE_TEXTMODE_BUFFER, NULL);
	SetConsoleActiveScreenBuffer(frontBuffer);
}

GCONSOLE_API void  Present() {
	COORD bufferSize = { width, height };
	COORD originCoord = { 0, 0 };
	SMALL_RECT updateRegion = { 0, 0, width - 1, height - 1 };
	WriteConsoleOutput(
		backBuffer, buffer, bufferSize, originCoord, &updateRegion);
}

GCONSOLE_API void  SwapBuffer() {
	HANDLE tmp = frontBuffer;
	frontBuffer = backBuffer;
	backBuffer = tmp;
	SetConsoleActiveScreenBuffer(frontBuffer);
}

GCONSOLE_API void  SetChar(int r, int c, short ch) {
	buffer[r * width + c].Char.UnicodeChar = ch;
	buffer[r * width + c].Attributes =
		FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE;
}


GCONSOLE_API void  SetCursorVisibility(bool visible)
{
	HANDLE out = GetStdHandle(STD_OUTPUT_HANDLE);
	CONSOLE_CURSOR_INFO     cursorInfo;
	GetConsoleCursorInfo(out, &cursorInfo);
	cursorInfo.bVisible = visible;
	SetConsoleCursorInfo(out, &cursorInfo);
}

GCONSOLE_API void  ClearBuffer()
{
	for (int i = 0; i < width * height; i++) {
		buffer[i].Char.UnicodeChar = ' ';
		buffer[i].Attributes =
			FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE;
	}
}

