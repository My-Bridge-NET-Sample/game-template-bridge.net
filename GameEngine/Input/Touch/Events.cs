﻿using Bridge.Html5;
using Raspware.GameEngine.Rendering;

namespace Raspware.GameEngine.Input.Touch
{
	public sealed class Events : IEvents, IEventsFullscreen
	{
		private IActionConfigurationTouch _actionConfiguration { get; }
		private HTMLDivElement _wrapper { get; }
		private Resolution _resolution { get; }
		private Layer _layer { get; }

		private bool _isButtonDown = false;
		private bool _isButtonUp = false;
		private bool _isInputDown = false;
		private bool _onceOnButtonDownLock = false;
		private bool _applyFullscreen = false;

		public Events(Resolution resolution, IActionConfigurationTouch actionConfiguration, HTMLCanvasElement controls, HTMLDivElement wrapper)
		{
			_wrapper = wrapper;
			_resolution = resolution;
			_actionConfiguration = actionConfiguration;

		}

		public void InputDown(Bridge.Html5.Touch touch)
		{
			_isInputDown = true;

			if (!_actionConfiguration.Point.Collision(GetCurrentTouchPosition(touch)))
				return;

			_isButtonDown = true;
			_isButtonUp = false;
		}

		public void InputUp(Bridge.Html5.Touch touch)
		{
			_isInputDown = false;

			if (!_actionConfiguration.Point.Collision(GetCurrentTouchPosition(touch)))
				return;

			_isButtonDown = false;
			_isButtonUp = true;
			_onceOnButtonDownLock = false;

			if (_applyFullscreen)
			{
				_applyFullscreen = false;
				EventsHelper.ApplyFullscreen(_wrapper);
			}
		}

		public void InputMove(Bridge.Html5.Touch touch)
		{
			if (_onceOnButtonDownLock && (!_isInputDown || !_actionConfiguration.Point.Collision(GetCurrentTouchPosition(touch))))
				_onceOnButtonDownLock = false;

			if (_isInputDown && _actionConfiguration.Point.Collision(GetCurrentTouchPosition(touch)))
			{
				_isButtonDown = true;
				_isButtonUp = false;
			}
			else
			{
				_isButtonDown = false;
				_isButtonUp = true;
			}
		}
		public bool PressedDown()
		{
			return _isButtonDown;
		}

		public bool PostPressedDown()
		{
			if (_isButtonUp)
			{
				_isButtonUp = false;
				return true;
			}
			return false;
		}

		public bool OnceOnPressDown()
		{
			if (_isInputDown && _isButtonDown && !_onceOnButtonDownLock)
			{
				_onceOnButtonDownLock = true;
				return true;
			}
			return false;
		}

		public void ApplyFullscreenOnPressUp() => _applyFullscreen = true;

		private Point GetCurrentTouchPosition(Bridge.Html5.Touch touch)
		{
			return new Point(
				_resolution.GetEventX(_wrapper, touch),
				_resolution.GetEventY(_wrapper, touch),
				_resolution.RenderAmount(1)
			);
		}
	}
}