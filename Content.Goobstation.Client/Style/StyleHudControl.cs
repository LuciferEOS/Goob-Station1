using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.Graphics;
using Robust.Shared.Maths;
using System.Collections.Generic;
using System.Numerics;
using Content.Goobstation.Common.Style;

namespace Content.Goobstation.Client.Style
{
    public sealed class StyleHudControl : Control
    {
        private readonly Label _rankLabel;
        private readonly List<Label> _eventLabels = new();

        public StyleHudControl()
        {
            MinWidth = 200;

            // Основной контейнер
            var container = new PanelContainer
            {
                PanelOverride = new StyleBoxFlat
                {
                    BackgroundColor = new Color(30, 30, 34, 200),
                    BorderColor = Color.Gold,
                    BorderThickness = new Thickness(1)
                },
                Margin = new Thickness(10, 10, 10, 10),
                HorizontalAlignment = HAlignment.Left,
                VerticalAlignment = VAlignment.Bottom
            };

            var content = new BoxContainer
            {
                Orientation = BoxContainer.LayoutOrientation.Vertical,
                Margin = new Thickness(5)
            };

            // Заголовок
            var header = new BoxContainer
            {
                Orientation = BoxContainer.LayoutOrientation.Horizontal,
                HorizontalExpand = true
            };

            _rankLabel = new Label
            {
                Text = "F",
                FontColorOverride = Color.Gold,
                HorizontalAlignment = HAlignment.Left,
                VerticalAlignment = VAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0)
            };

            var title = new Label
            {
                Text = "СТИЛЬ",
                FontColorOverride = Color.White,
                VerticalAlignment = VAlignment.Center
            };

            header.AddChild(_rankLabel);
            header.AddChild(title);
            content.AddChild(header);

            // Разделитель
            content.AddChild(new PanelContainer
            {
                PanelOverride = new StyleBoxFlat { BackgroundColor = Color.Gray },
                MinSize = new Vector2(0, 1),
                Margin = new Thickness(0, 5)
            });

            // Создаем 5 пустых лейблов для событий
            for (var i = 0; i < 5; i++)
            {
                var eventLabel = new Label
                {
                    Text = "",
                    FontColorOverride = Color.LightGray,
                    Margin = new Thickness(0, 2)
                };
                _eventLabels.Add(eventLabel);
            }

            container.AddChild(content);
            AddChild(container);
        }

        public void UpdateStyleHud(StyleRank rank, float multiplier, List<string> events)
        {
            // Обновляем ранг
            _rankLabel.Text = rank.ToString();
            _rankLabel.FontColorOverride = GetRankColor(rank);

            // Обновляем события
            for (var i = 0; i < 5; i++)
            {
                var label = _eventLabels[i];
                if (i < events.Count)
                {
                    label.Text = events[i];
                    label.FontColorOverride = events[i].StartsWith("+") ?
                        Color.LightGreen : Color.LightPink;
                }
                else
                {
                    label.Text = "";
                }
            }
        }

        private Color GetRankColor(StyleRank rank)
        {
            return rank switch
            {
                StyleRank.R => Color.Red,
                StyleRank.SSS => Color.Gold,
                StyleRank.SS => Color.Orange,
                StyleRank.S => Color.Yellow,
                StyleRank.A => Color.LimeGreen,
                StyleRank.B => Color.LightGreen,
                StyleRank.C => Color.Cyan,
                StyleRank.D => Color.Blue,
                StyleRank.F => Color.Gray,
                _ => Color.White
            };
        }
    }
}
