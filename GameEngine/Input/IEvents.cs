﻿namespace Raspware.GameEngine.Input
{
	public interface IEvents
	{
		bool PressedDown();
		bool PostPressedDown();
		bool OnceOnPressDown();
		void ApplyFullscreenOnPressUp(bool applyFullscreen = false);
	}
}
