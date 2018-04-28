﻿using System;
using System.Linq;
using Bridge.Html5;
using ProductiveRage.Immutable;

namespace Raspware.GameEngine.Rendering
{
	// TODO: Turn 'Layers' into an interface.
	// TODO: Pass in NonNullList of 'Layer' rather than presume we are going to use 'Layers'.
	public sealed class Layers
	{
		private NonNullList<Layer> _layers { get; }

		private static bool _configured;
		public static Layers Instance { get; private set; }

		//TODO: Move these into a single class
		public static HTMLDivElement Wrapper { get; } = new HTMLDivElement();
		private int _lastHeight = 0;
		private int _lastWidth = 0;

		private Layers(Resolution resolution)
		{
			if (resolution == null)
				throw new ArgumentNullException(nameof(resolution));

			_layers = NonNullList.Of(
				new Layer(resolution, Id.Background, Wrapper, 1),
				new Layer(resolution, Id.Level, Wrapper, 2),
				new Layer(resolution, Id.Controls, Wrapper, 3)
			).OrderBy(layer => layer.Order);

			_layers.ToList().ForEach(layer => Wrapper.AppendChild(layer.CanvasElement));
			Document.Body.AppendChild(Wrapper);

			Wrapper.OnTouchStart = CancelDefault;
			Wrapper.OnTouchEnd = CancelDefault;
			Wrapper.OnTouchCancel = CancelDefault;
			Wrapper.OnTouchEnter = CancelDefault;
			Wrapper.OnTouchLeave = CancelDefault;
			Wrapper.OnTouchMove = CancelDefault;
			Wrapper.OnMouseMove = CancelDefault;
			Wrapper.OnMouseLeave = CancelDefault;
			Wrapper.OnMouseDown = CancelDefault;
			Wrapper.OnMouseEnter = CancelDefault;
			Wrapper.OnMouseOut = CancelDefault;
			Wrapper.OnMouseOver = CancelDefault;
			Wrapper.OnMouseUp = CancelDefault;
			Wrapper.OnMouseWheel = CancelDefault;
			Wrapper.OnContextMenu = CancelDefault;
		}

		public static void ConfigureInstance()
		{
			if (_configured)
				throw new Exception($"'{nameof(Instance)}' has already been configured!");

			Instance = new Layers(Resolution.Instance);
			_configured = true;
		}

		public Layer GetLayer(Id id)
		{
			var layer = _layers.Where(l => l.Id == id).FirstOrDefault();
			if (layer == null)
				throw new ArgumentNullException(nameof(layer));

			return layer;
		}

		public enum Id
		{
			Background,
			Level,
			Controls
		}

		public void Resize()
		{
			if (Wrapper.ClientHeight == _lastHeight && Wrapper.ClientWidth == _lastWidth)
				return;

			_layers.ToList().ForEach(layer => layer.Resize());

			_lastHeight = Wrapper.ClientHeight;
			_lastWidth = Wrapper.ClientWidth;
		}

		private static void CancelDefault(Event e)
		{
			if (e == null)
				throw new ArgumentNullException(nameof(e));
			e.PreventDefault();
		}
	}
}
