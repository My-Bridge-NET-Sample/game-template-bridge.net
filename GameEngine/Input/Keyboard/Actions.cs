﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bridge.Html5;
using Raspware.GameEngine.Rendering;

namespace Raspware.GameEngine.Input.Keyboard
{
	public sealed class Actions
	{
		public Dictionary<int,IEvents> ActionsEvents { get; private set; }

		public Actions(Layer controls, Dictionary<int, Events.Keys> buttons)
		{
			if (controls == null) 
				throw new ArgumentNullException(nameof(controls));
			if (buttons == null)
				throw new ArgumentNullException(nameof(buttons));

			var a = buttons.Select(_ => new KeyValuePair<int, IEvents>(_.Key, new Events(_.Value)));

			controls.CanvasElement.AddEventListener(EventType.KeyDown, (e) => InputKeyDown((KeyboardEvent)e));
			controls.CanvasElement.AddEventListener(EventType.KeyUp, (e) => InputKeyUp((KeyboardEvent)e));
		}

		private void InputKeyDown(KeyboardEvent e)
		{
			Up.As<Events>().InputDown(e);
			Down.As<Events>().InputDown(e);
			Left.As<Events>().InputDown(e);
			Right.As<Events>().InputDown(e);
			Cancel.As<Events>().InputDown(e);
			Button1.As<Events>().InputDown(e);
		}

		private void InputKeyUp(KeyboardEvent e)
		{
			Up.As<Events>().InputUp(e);
			Down.As<Events>().InputUp(e);
			Left.As<Events>().InputUp(e);
			Right.As<Events>().InputUp(e);
			Cancel.As<Events>().InputUp(e);
			Button1.As<Events>().InputUp(e);
		}

		public IEvents Up { get; } = new Events(Events.Keys._upArrow);
		public IEvents Down { get; } = new Events(Events.Keys._downArrow);
		public IEvents Left { get; } = new Events(Events.Keys._leftArrow);
		public IEvents Right { get; } = new Events(Events.Keys._rightArrow);
		public IEvents Cancel { get; } = new Events(Events.Keys._escape);
		public IEvents Button1 { get; } = new Events(Events.Keys._space);
	}
}