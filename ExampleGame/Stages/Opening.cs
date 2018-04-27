﻿using Bridge.Html5;
using Raspware.GameEngine.Rendering;

namespace Raspware.ExampleGame.Stages
{
	public sealed class Opening : IStage
	{
		private bool _musicPlayed = false;
		private int _timePassed = 0;
		private double _alpha = 1;
		private string _message;
		private HTMLAudioElement _music;

		public Id Id => Id.Opening;

		public Opening()
		{
			_music = new HTMLAudioElement() { Src = Resources.Audio.Theme };
			_music.Play();
			_music.AddEventListener(EventType.Ended, ev => { _musicPlayed = true; });
		}

		public void Draw()
		{
			if (_message == "")
				return;

			var levelContext = Layers.Instance.GetLayer(Layers.Id.Level).GetContext();
			var resolution = Resolution.Instance;

			levelContext.FillStyle = "rgb(0,0,0)";
			levelContext.FillRect(0, 0, resolution.Width, resolution.Height); // Clear

			levelContext.FillStyle = "white";

			levelContext.Font = resolution.RenderAmount(24).ToString() + "px Consolas, monospace";
			levelContext.FillText("[RASPWARE]", resolution.RenderAmount(23), resolution.RenderAmount(50));

			levelContext.Font = resolution.RenderAmount(6).ToString() + "px Consolas, monospace";
			levelContext.FillText(_message, resolution.RenderAmount(4), resolution.RenderAmount(96));

			levelContext.FillStyle = $"rgba(0,0,0,{_alpha})";
			levelContext.FillRect(0, 0, resolution.Width, resolution.Height); // Clear
		}

		public Id Update(int ms)
		{
			_timePassed += ms;
			_message = _timePassed.ToString();

			_alpha = 1 - (_timePassed / 1000);

			if (_timePassed > 1000)
				_alpha = 0;

			if (_musicPlayed)
				return Id.Title;

			return Id;
		}
	}
}