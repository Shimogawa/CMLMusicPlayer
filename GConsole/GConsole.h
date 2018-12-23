#pragma once

#ifdef GCONSOLE_EXPORTS
#define GCONSOLE_API __declspec(dllexport)
#else
#define GCONSOLE_API __declspec(dllimport)
#endif

extern "C"
{
	GCONSOLE_API void  SetCursorVisibility(bool);

	GCONSOLE_API void  InitializeBuffer(int w, int h);

	GCONSOLE_API void  Present();

	GCONSOLE_API void  SwapBuffer();

	GCONSOLE_API void  SetChar(int r, int c, short ch);

	GCONSOLE_API void  SetCursorVisibility(bool);

	GCONSOLE_API void  ClearBuffer();
}