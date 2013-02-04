using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace WindowsGame
{
    [XmlInclude(typeof(Soldier)), XmlInclude(typeof(Grenade))]
    public class BaseGameObject
    {
        public Vector3 Position { get; set; }
    }

    public class Soldier : BaseGameObject
    {
        public float Health { get; set; }
    }

    public class Grenade : BaseGameObject
    {
        public float TimeToDetonate { get; set; }
    }

    public struct SaveGameData
    {
        public string PlayerName;
        public Vector2 AvatarPosition;
        public int Level;
        public int Score;
        public List<BaseGameObject> GameObjects;
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        enum SavingState
        {
            NotSaving,
            ReadyToSelectStorageDevice,
            SelectingStorageDevice,

            ReadyToOpenStorageContainer,    // once we have a storage device start here
            OpeningStorageContainer,
            ReadyToSave
        }

        GraphicsDeviceManager graphics;
        KeyboardState oldKeyboardState;
        KeyboardState currentKeyboardState;
        StorageDevice storageDevice;
        SavingState savingState = SavingState.NotSaving;
        IAsyncResult asyncResult;
        PlayerIndex playerIndex = PlayerIndex.One;
        StorageContainer storageContainer;
        string filename = "savegame.sav";

        SaveGameData saveGameData = new SaveGameData()
        {
            PlayerName = "Grunt",
            AvatarPosition = new Vector2(10, 15),
            Level = 3,
            Score = 99424,
            GameObjects = new List<BaseGameObject>() 
            { 
                new Soldier { Health = 10.0f, Position = new Vector3(0.0f, 10.0f, 0.0f) },
                new Grenade { TimeToDetonate = 3.0f, Position = new Vector3(4.0f, 3.0f, 0.0f) }
            }
        };

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

#if XBOX
            Components.Add(new GamerServicesComponent(this));
#endif

            currentKeyboardState = Keyboard.GetState();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            oldKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            UpdateSaveKey(Keys.F1);
            UpdateSaving();

            base.Update(gameTime);
        }

        private void UpdateSaveKey(Keys saveKey)
        {
            if (!oldKeyboardState.IsKeyDown(saveKey) && currentKeyboardState.IsKeyDown(saveKey))
            {
                if (savingState == SavingState.NotSaving)
                {
                    savingState = SavingState.ReadyToOpenStorageContainer;
                }
            }
        }

        private void UpdateSaving()
        {
            switch (savingState)
            {
                case SavingState.ReadyToSelectStorageDevice:
#if XBOX
                    if (!Guide.IsVisible)
#endif
                    {
                        asyncResult = StorageDevice.BeginShowSelector(playerIndex, null, null);
                        savingState = SavingState.SelectingStorageDevice;
                    }
                    break;

                case SavingState.SelectingStorageDevice:
                    if (asyncResult.IsCompleted)
                    {
                        storageDevice = StorageDevice.EndShowSelector(asyncResult);
                        savingState = SavingState.ReadyToOpenStorageContainer;
                    }
                    break;

                case SavingState.ReadyToOpenStorageContainer:
                    if (storageDevice == null || !storageDevice.IsConnected)
                    {
                        savingState = SavingState.ReadyToSelectStorageDevice;
                    }
                    else
                    {
                        asyncResult = storageDevice.BeginOpenContainer("Game1StorageContainer", null, null);
                        savingState = SavingState.OpeningStorageContainer;
                    }
                    break;

                case SavingState.OpeningStorageContainer:
                    if (asyncResult.IsCompleted)
                    {
                        storageContainer = storageDevice.EndOpenContainer(asyncResult);
                        savingState = SavingState.ReadyToSave;
                    }
                    break;

                case SavingState.ReadyToSave:
                    if (storageContainer == null)
                    {
                        savingState = SavingState.ReadyToOpenStorageContainer;
                    }
                    else
                    {
                        try
                        {
                            DeleteExisting();
                            Save();
                        }
                        catch (IOException e)
                        {
                            // Replace with in game dialog notifying user of error
                            Debug.WriteLine(e.Message);
                        }
                        finally
                        {
                            storageContainer.Dispose();
                            storageContainer = null;
                            savingState = SavingState.NotSaving;
                        }
                    }
                    break;
            }
        }

        private void DeleteExisting()
        {
            if (storageContainer.FileExists(filename))
            {
                storageContainer.DeleteFile(filename);
            }
        }

        private void Save()
        {
            using (Stream stream = storageContainer.CreateFile(filename))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
                serializer.Serialize(stream, saveGameData);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}