﻿using System;
using Meadow.Foundation;
using Meadow.Foundation.Graphics;

namespace MicroGraphics.Buffers
{
    public class BufferRgb444 : IDisplayBuffer
    {
        public BufferRgb444(int width, int height)
        {
            Width = width;
            Height = height;

            Buffer = new byte[ByteCount];
        }

        public int Width { get; protected set; }

        public int Height { get; protected set; }

        public int ByteCount => Width * Height * 3 / 2;

        public GraphicsLibrary.ColorType ColorType => GraphicsLibrary.ColorType.Format12bppRgb444;

        public byte[] Buffer { get; protected set; }

        public ushort GetPixelUShort(int x, int y)
        {
            byte r, g, b;
            int index;
            if (x % 2 == 0)
            {
                index = (x + y * Width) * 3 / 2;

                r = (byte)(Buffer[index] >> 4);
                g = (byte)(Buffer[index] & 0x0F);
                b = (byte)(Buffer[index + 1] >> 4);
            }
            else
            {
                index = ((x - 1 + y * Width) * 3 / 2) + 1;
                r = (byte)(Buffer[index] & 0x0F);
                g = (byte)(Buffer[index + 1] >> 4);
                b = (byte)(Buffer[index + 1] & 0x0F);
            }

            return (ushort)(r << 8 | g << 4 | b);
        }

        public Color GetPixel(int x, int y)
        {
            byte r, g, b;
            int index;
            if (x % 2 == 0)
            {
                index = (x + y * Width) * 3 / 2;

                r = (byte)(Buffer[index] >> 4);
                g = (byte)(Buffer[index] & 0x0F);
                b = (byte)(Buffer[index + 1] >> 4);
            }
            else
            {
                index = ((x - 1 + y * Width) * 3 / 2) + 1;
                r = (byte)(Buffer[index] & 0x0F);
                g = (byte)(Buffer[index + 1] >> 4);
                b = (byte)(Buffer[index + 1] & 0x0F);
            }

            return new Color(r, g, b);

        }

        public void SetPixel(int x, int y, Color color)
        {
            SetPixel(x, y, color.Color12bppRgb444);
        }

        public void SetPixel(int x, int y, ushort color)
        {
            int index;
            //one of 2 possible write patterns 
            if (x % 2 == 0)
            {
                //1st byte RRRRGGGG
                //2nd byte BBBB
                index = ((x + y * Width) * 3 / 2);
                Buffer[index] = (byte)(color >> 4); //think this is correct - grab the r & g values
                index++;
                Buffer[index] = (byte)((Buffer[index] & 0x0F) | (color << 4));
            }
            else
            {
                //1st byte     RRRR
                //2nd byte GGGGBBBB
                index = ((x - 1 + y * Width) * 3 / 2) + 1;
                Buffer[index] = (byte)((Buffer[index] & 0xF0) | (color >> 8));
                Buffer[++index] = (byte)color; //just the lower 8 bits
            }
        }

        public void Clear()
        {
            Array.Clear(Buffer, 0, Buffer.Length);
        }
    }
}
