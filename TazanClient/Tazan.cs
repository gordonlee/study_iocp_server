﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace TazanClient
{
    class Tazan
    {
        public static Tazan inst = new Tazan();

        private Device directxDevice = null;
        private Sprite mainSprite = null;

        private Texture textureTile = null;
        private Texture textureOveredTile = null;
        private Texture textureCharacter = null;

        //private AsyncTcpClient client = new AsyncTcpClient();
        public const int clients_count = 30;
        private AsyncTcpClient[] clients = new AsyncTcpClient[clients_count];

        private Tazan()
        {
        }

        public void InitializeTazan(DXPanel panel)
        {
            // Setting DirectX Device
            PresentParameters pp = new PresentParameters();
            pp.Windowed = true;
            pp.SwapEffect = SwapEffect.Discard;

            directxDevice = new Device(0,
                                DeviceType.Hardware,
                                panel,
                                CreateFlags.SoftwareVertexProcessing,
                                pp);

            mainSprite = new Sprite(directxDevice);

            // Texture Load
            textureTile = new Texture(directxDevice, new Bitmap("Resource/tile.png"), Usage.None, Pool.Managed);
            textureOveredTile = new Texture(directxDevice, new Bitmap("Resource/overed_tile.png"), Usage.None, Pool.Managed);
            textureCharacter = new Texture(directxDevice, new Bitmap("Resource/character.png"), Usage.None, Pool.Managed);

            // Init client
           // client.Start();
        }

        public void InitializeClients()
        {
            if (clients[index] == null)
            {
                clients[index] = new AsyncTcpClient();
                clients[index].Start();
            }
            //for (int i = 0; i < clients_count; ++i)
            //{
            //    clients[i] = new AsyncTcpClient();
            //    clients[i].Start();
            //}
        }

        public int index = -1;
        public void Update(Point selectedTile)
        {
            directxDevice.Clear(ClearFlags.Target, Color.White, 1.0f, 0);
            directxDevice.BeginScene();

            // Render Start
            mainSprite.Begin(SpriteFlags.AlphaBlend);

            // Draw Tiles
            for (int i = 0; i < 30; ++i)
            {
                for (int j = 0; j < 30; ++j)
                {
                    if (selectedTile.X == i && selectedTile.Y == j)
                    {
                        mainSprite.Draw2D(textureOveredTile, Point.Empty, 0f, new Point(i * 20, j * 20), Color.White);
                    }
                    else
                    {
                        mainSprite.Draw2D(textureTile, Point.Empty, 0f, new Point(i * 20, j * 20), Color.White);
                    }
                }
            }

            // Draw Character
            //foreach(var user in client.otherUserInfo)
            //{
            //    mainSprite.Draw2D(textureCharacter, new Rectangle(0, 0 * user.Direction, 20, 20), new SizeF(20.0f, 20.0f), new Point(user.X * 20, user.Y * 20), Color.White);
            //}
            //mainSprite.Draw2D(textureCharacter, new Rectangle(0, 0 * client.userInfo.Direction, 20, 20), new SizeF(20.0f, 20.0f), new Point(client.userInfo.X * 20, client.userInfo.Y * 20), Color.White);
            if (index >= 0) for (int i = index; i < index+1; ++i)
            {
                if (clients[i] != null)
                {
                    //mainSprite.Draw2D(textureCharacter, new Rectangle(0, 0 * clients[i].userInfo.Direction, 20, 20), new SizeF(20.0f, 20.0f), new Point(clients[i].userInfo.X * 20, clients[i].userInfo.Y * 20), Color.White);

                    if (clients[i].otherUserInfo != null)
                    {
                        int size = 0;
                        lock (clients[i].otherUserInfo)
                        {
                            size = clients[i].otherUserInfo.Count;
                        }
                        for(int j = 0; j < size; ++j)
                        {
                            mainSprite.Draw2D(textureCharacter, new Rectangle(0, 0 * clients[i].otherUserInfo[j].Direction, 20, 20), new SizeF(20.0f, 20.0f), new Point(clients[i].otherUserInfo[j].X * 20, clients[i].otherUserInfo[j].Y * 20), Color.White);
                        }
                    }
                }
            }
            
            mainSprite.End();
            // Render End

            directxDevice.EndScene();
            directxDevice.Present();
        }
    }
}