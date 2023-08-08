﻿using GoOS.GUI.Models;
using PrismAPI.Graphics;
using IL2CPU.API.Attribs;

namespace GoOS.GUI
{
    public class Button : Control
    {
        [ManifestResourceStream(ResourceName = "GoOS.Resources.GUI.mouse_click.bmp")] private static byte[] mouseClickRaw;
        private static Canvas mouseClick = PrismAPI.Graphics.Image.FromBitmap(mouseClickRaw);

        public string Title;

        /// <summary>
        /// Optional image.
        /// </summary>
        public Canvas Image;

        public bool UseSystemStyle = true;

        public Color BackgroundColour = new Color(12, 12, 12);

        public Color TextColour = Color.White;

        public bool pressed = false;

        public bool AppearPressed = false;

        private const int PUSH_DEPTH = 1;

        public Button(Window parent, ushort x, ushort y, ushort width, ushort height, string title)
            : base(parent, x, y, width, height)
        {
            Title = title;
        }

        public override void Render()
        {
            if (UseSystemStyle)
            {
                if (AppearPressed || pressed)
                {
                    // Background.
                    Contents.DrawFilledRectangle(0, 0, Contents.Width, Contents.Height, 0, new Color(71, 71, 71));

                    // Dark shadow.
                    Contents.DrawLine(0, 0, Contents.Width - 1, 0, Color.Black);
                    Contents.DrawLine(0, 0, 0, Contents.Height - 1, Color.Black);

                    // Highlight.
                    Contents.DrawLine(1, Contents.Height - 2, Contents.Width - 2, Contents.Height - 2, new Color(80, 80, 80));
                    Contents.DrawLine(Contents.Width - 2, 1, Contents.Width - 2, Contents.Height - 1, new Color(80, 80, 80));

                    // Light highlight.
                    Contents.DrawLine(0, Contents.Height - 1, Contents.Width, Contents.Height - 1, new Color(89, 89, 89));
                    Contents.DrawLine(Contents.Width - 1, 0, Contents.Width - 1, Contents.Height - 1, new Color(89, 89, 89));
                }
                else
                {
                    // Background.
                    Contents.DrawFilledRectangle(0, 0, Contents.Width, Contents.Height, 0, new Color(71, 71, 71));

                    // Highlight.
                    Contents.DrawLine(0, 0, Contents.Width - 1, 0, new Color(80, 80, 80));
                    Contents.DrawLine(0, 0, 0, Contents.Height - 1, new Color(80, 80, 80));

                    // Light shadow.
                    Contents.DrawLine(1, Contents.Height - 2, Contents.Width - 2, Contents.Height - 2, new Color(89, 89, 89));
                    Contents.DrawLine(Contents.Width - 2, 1, Contents.Width - 2, Contents.Height - 1, new Color(89, 89, 89));

                    // Dark shadow.
                    Contents.DrawLine(0, Contents.Height - 1, Contents.Width, Contents.Height - 1, Color.Black);
                    Contents.DrawLine(Contents.Width - 1, 0, Contents.Width - 1, Contents.Height - 1, Color.Black);
                }
            }
            else
            {
                // Background.
                Contents.DrawFilledRectangle(0, 0, Contents.Width, Contents.Height, 0, BackgroundColour);
            }

            if (Image != null)
            {
                int imageX = (Contents.Width - Image.Width) / 2;
                int imageY = 0;

                if ((AppearPressed || pressed) && UseSystemStyle)
                {
                    imageX += PUSH_DEPTH;
                    imageY += PUSH_DEPTH;
                }

                Contents.DrawImage(imageX, imageY, Image, true);
            }

            // Title.
            int textX = Contents.Width / 2;
            int textY = Image != null ? Image.Height + 12 : Contents.Height / 2;

            if ((AppearPressed || pressed) && UseSystemStyle)
            {
                textX += PUSH_DEPTH;
                textY += PUSH_DEPTH;
            }

            Color textColour = UseSystemStyle ? Color.White : TextColour;

            Contents.DrawString(textX, textY, Title, BetterConsole.font, textColour, true);

            Parent.RenderControls();
        }

        public override void Update()
        {
            if (IsMouseOver)
            {
                WindowManager.MouseToDraw = mouseClick;
            }
        }

        internal override void HandleDown(MouseEventArgs _args)
        {
            pressed = true;
            Render();
            Parent.RenderControls();
        }

        internal override void HandleRelease()
        {
            pressed = false;
            Render();
            Parent.RenderControls();
        }
    }
}
