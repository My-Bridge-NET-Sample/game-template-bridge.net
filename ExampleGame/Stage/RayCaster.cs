﻿using System;
using ProductiveRage.Immutable;
using Raspware.ExampleGame.Stage.RayCasterAdditional;
using Raspware.GameEngine;
using Raspware.GameEngine.Input;

namespace Raspware.ExampleGame.Stage
{
	public sealed class RayCaster : IStage
	{
		private string _message;
		private bool _renderedControls;
		public static readonly float Circle = (float)(Math.PI * 2);
		private Player _player;
		private Map _map;
		private ICore _core { get; }

		public int Id => Stage.Id.RayCaster;

		public RayCaster(ICore core)
		{
			if (core == null)
				throw new ArgumentNullException(nameof(core));

			_core = core;
			_core.Layers.Reset(NonNullList.Of(0));
			_core.ActivateActions();

			// taken from https://github.com/hunterloftis/playfuljs-demos/blob/gh-pages/raycaster/index.html
		}

		public void Draw()
		{
			if (_message == "")
				return;

			var levelContext = _core.Layers.GetStageLayer(0).GetContext();
			var resolution = _core.Resolution;
			levelContext.ClearRect(0, 0, resolution.Width, resolution.Height);

			if (!_renderedControls)
			{
				_core.RenderActions();
				_renderedControls = true;
			}
		}

		public int Update(int ms)
		{
			var data = Data.Instance;

			var up = _core.ActionEvents[DefaultActions.Up];
			var down = _core.ActionEvents[DefaultActions.Down];
			var left = _core.ActionEvents[DefaultActions.Left];
			var right = _core.ActionEvents[DefaultActions.Right];
			var button1 = _core.ActionEvents[DefaultActions.Button1];
			var menu = _core.ActionEvents[DefaultActions.Menu];

			if (left.PressedDown())
				_player.Rotate(-(float)Math.PI * ms);

			if (right.PressedDown())
				_player.Rotate((float)Math.PI * ms);

			if (up.PressedDown())
				_player.Walk(3 * ms, _map);

			if (down.PressedDown())
				_player.Walk(-3 * ms, _map);


			data.TimePassed += ms;
			_message = data.TimePassed.ToString();

			return Id;
		}
	}
}