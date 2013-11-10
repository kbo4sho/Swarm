using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ScreenSystem.Debug;
using SwarmEngine;
using XNASwarms.Screens;
using System.Threading.Tasks;
using SwarmAudio;
using XNASwarms.Screens.SwarmScreen;
using XNASwarms.Saving;
using XNASwarms.Util;

namespace XNASwarms.Screens.UI
{
    public sealed class ButtonSection
    {
        private SwarmScreenBase screen;
        private Vector2 position;
        private Texture2D bgSprite;
        private Rectangle rect, innerRect;
        private string description;
        private int selectedEntry;
        private SpriteFont LabelFont, BigFont;
        SpriteBatch spriteBatch;

        private readonly int maxLikedItems = 6;

        private List<MenuEntry> menuEntries = new List<MenuEntry>();

        private readonly Vector2 _containerMargin = new Vector2(10, 70);
        private readonly Vector2 _containerPadding = new Vector2(12, 12);

        private readonly Color _containerBGColor = new Color(30, 30, 30, 100);
        private readonly Color _BorderColor = new Color(30, 30, 30, 100);

        private readonly int BorderThickness = 4;
        private IDebugScreen debugScreen;
        private IControlClient controlClient;

        SaveAllSpecies allLikedItems;


        public ButtonSection(bool flip, SwarmScreenBase swarmscreen, string desc)
        {
            rect.Width = 100;
            rect.Height = 360;
            screen = swarmscreen;
            innerRect.Width = rect.Width - BorderThickness;
            innerRect.Height = rect.Height - BorderThickness;
            description = desc;
            allLikedItems = new SaveAllSpecies();

            //AddMenuItem("+ ZOOM", EntryType.ZoomIn, _screen);
            //AddMenuItem("- ZOOM", EntryType.ZoomOut, _screen);
            //AddMenuItem("Mutation", EntryType.Game, _screen);

            //AddMenuItem("Stable", EntryType.Stable, _screen);
            //AddMenuItem("Swinger", EntryType.Swinger, _screen);
            //AddMenuItem("Console", EntryType.Debugger, _screen);
            //AddMenuItem("Import", EntryType.ImportLikes, _screen);
            //AddMenuItem("Export", EntryType.ExportLikes, _screen);
            //AddMenuItem("Like", EntryType.Save, _screen);
#if WINDOWS
            AddMenuItem("Mutation", EntryType.Mutation, screen);
            AddMenuItem("Start Cluster", EntryType.AudioPlay, screen);
            AddMenuItem("Stop Cluster", EntryType.AudioPause, screen);
            AddMenuItem("Analyze", EntryType.Analyze, screen);
            AddMenuItem("Console", EntryType.Debugger, screen);
            AddMenuItem("Like", EntryType.Save, screen);
#else
            AddMenuItem("Stable", EntryType.Stable, swarmscreen);
            AddMenuItem("Mutation", EntryType.Mutation, swarmscreen);
            AddMenuItem("Console", EntryType.Debugger, swarmscreen);
            AddMenuItem("Import", EntryType.ImportLikes, swarmscreen);
            AddMenuItem("Export", EntryType.ExportLikes, swarmscreen);
            AddMenuItem("Like", EntryType.Save, swarmscreen);
            
#endif


        }

        public void Load()
        {
            if (allLikedItems == null)
            {
#if NETFX_CORE
                GetLocalSaveSwarmData();
#endif
            }
            spriteBatch = new SpriteBatch(screen.ScreenManager.GraphicsDevice);
            debugScreen = screen.ScreenManager.Game.Services.GetService(typeof(IDebugScreen)) as IDebugScreen;
            controlClient = screen.ScreenManager.Game.Services.GetService(typeof(IControlClient)) as IControlClient;
            Viewport viewport = screen.ScreenManager.GraphicsDevice.Viewport;
            position = new Vector2(screen.ScreenManager.GraphicsDevice.Viewport.Width - rect.Width, 0);// + _containerMargin;
            bgSprite = screen.ScreenManager.Content.Load<Texture2D>("Backgrounds/gray");
            LabelFont = screen.ScreenManager.Fonts.DetailsFont;
            BigFont = screen.ScreenManager.Fonts.FrameRateCounterFont;

            for (int i = 0; i < menuEntries.Count; ++i)
            {
                menuEntries[i].Initialize();
            }
#if WINDOWS
            UpdateLikedItemsUI();
#endif
            allLikedItems = SaveHelper.Load("SwarmsSaves");
        }

        public void UpdateMenuEntryLocations()
        {
            Vector2 position = Vector2.Zero;
            for (int i = 0; i < menuEntries.Count; ++i)
            {
                menuEntries[i].Position = new Vector2(position.X, position.Y + i * menuEntries[i].GetHeight() + i * _containerPadding.Y);
            }
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            UpdateMenuEntryLocations();
            SpriteFont font = screen.ScreenManager.Fonts.MenuSpriteFont;
            var pos = position;
            for (int i = 0; i < menuEntries.Count; ++i)
            {
                bool isSelected = screen.IsActive && (i == selectedEntry);
                menuEntries[i].Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        private void UpdateLikedItemsUI()
        {
#if WINDOWS
            SaveAllSpecies allLikedItems = SaveHelper.Load("SwarmsSaves");
#endif
            menuEntries.RemoveAll(s => s.GetType() == typeof(SavedSwarmButton));
            if (allLikedItems != null)
            {
                if (allLikedItems.Count == 1)
                {
                    AddSavedSwarm(allLikedItems[0].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall1, allLikedItems[0].GetMostUsedColors(), screen);
                }
                else if (allLikedItems.Count == 2)
                {
                    AddSavedSwarm(allLikedItems[0].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall1, allLikedItems[0].GetMostUsedColors(), screen);
                    AddSavedSwarm(allLikedItems[1].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall2, allLikedItems[1].GetMostUsedColors(), screen);
                }
                else if (allLikedItems.Count == 3)
                {
                    AddSavedSwarm(allLikedItems[0].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall1, allLikedItems[0].GetMostUsedColors(), screen);
                    AddSavedSwarm(allLikedItems[1].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall2, allLikedItems[1].GetMostUsedColors(), screen);
                    AddSavedSwarm(allLikedItems[2].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall3, allLikedItems[2].GetMostUsedColors(), screen);
                }
                else if (allLikedItems.Count == 4)
                {
                    AddSavedSwarm(allLikedItems[0].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall1, allLikedItems[0].GetMostUsedColors(), screen);
                    AddSavedSwarm(allLikedItems[1].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall2, allLikedItems[1].GetMostUsedColors(), screen);
                    AddSavedSwarm(allLikedItems[2].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall3, allLikedItems[2].GetMostUsedColors(), screen);
                    AddSavedSwarm(allLikedItems[3].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall4, allLikedItems[3].GetMostUsedColors(), screen);
                }
                else if (allLikedItems.Count == 5)
                {
                    AddSavedSwarm(allLikedItems[0].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall1, allLikedItems[0].GetMostUsedColors(), screen);
                    AddSavedSwarm(allLikedItems[1].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall2, allLikedItems[1].GetMostUsedColors(), screen);
                    AddSavedSwarm(allLikedItems[2].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall3, allLikedItems[2].GetMostUsedColors(), screen);
                    AddSavedSwarm(allLikedItems[3].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall4, allLikedItems[3].GetMostUsedColors(), screen);
                    AddSavedSwarm(allLikedItems[4].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall5, allLikedItems[4].GetMostUsedColors(), screen);
                }
                else if (allLikedItems.Count == 6)
                {
                    AddSavedSwarm(allLikedItems[0].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall1, allLikedItems[0].GetMostUsedColors(), screen);
                    AddSavedSwarm(allLikedItems[1].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall2, allLikedItems[1].GetMostUsedColors(), screen);
                    AddSavedSwarm(allLikedItems[2].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall3, allLikedItems[2].GetMostUsedColors(), screen);
                    AddSavedSwarm(allLikedItems[3].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall4, allLikedItems[3].GetMostUsedColors(), screen);
                    AddSavedSwarm(allLikedItems[4].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall5, allLikedItems[4].GetMostUsedColors(), screen);
                    AddSavedSwarm(allLikedItems[5].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall6, allLikedItems[5].GetMostUsedColors(), screen);
                }

            }
        }

#if NETFX_CORE
        private async void GetLocalSaveSwarmData()
        {
            allLikedItems = await SaveHelper.LoadGameFile("AllSaved");
            UpdateLikedItemsUI();
        }

        private async void ImportSwarmSaveData()
        {
#if WINDOWS
            
#else
            allLikedItems = await controlClient.Import();
#endif
            UpdateLikedItemsUI();
        }

        private async void ExportSwarmSaveData()
        {
            allLikedItems = await controlClient.Export();
            UpdateLikedItemsUI();
        }
#endif

        private void SaveSwarm(SaveWorldParameters saveWorldParameters)
        {
#if WINDOWS
            SaveAllSpecies allLikedItems = SaveHelper.Load("AllSaved");
#else

#endif
            if (allLikedItems != null && allLikedItems.Count() >= maxLikedItems)
            {
                //Replacing
                SaveSpecies oldestSpecies = allLikedItems.OrderBy(s => s.CreadtedDt).First();
                allLikedItems.Remove(oldestSpecies);
                Save(saveWorldParameters);
            }

            if (allLikedItems != null && allLikedItems.Count > 0)
            {
                if (allLikedItems.Count == 1)
                {
                    Save(saveWorldParameters);
                }
                else if (allLikedItems.Count == 2)
                {
                    Save(saveWorldParameters);
                }
                else if (allLikedItems.Count == 3)
                {
                    Save(saveWorldParameters);
                }
                else if (allLikedItems.Count == 4)
                {
                    Save(saveWorldParameters);
                }
                else if (allLikedItems.Count == 5)
                {
                    Save(saveWorldParameters);
                }
            }
            else
            {
                Save(saveWorldParameters);
            }
        }

        private void Save(SaveWorldParameters saveWorldParameters)
        {
            SaveSpecies savespecies = screen.GetPopulationAsSaveSpecies();
            savespecies.SaveWorldParameters = saveWorldParameters;
            allLikedItems.Add(savespecies);
            SaveHelper.Save("AllSaved", allLikedItems);

            foreach (var species in savespecies.SavedSpecies)
            {
                foreach (var indvd in species)
                {
                    //debugScreen.AddDebugItem("INDVD X", indvd.x.ToString(), ScreenSystem.Debug.DebugFlagType.Important);
                }
            }
        }

        public void AddMenuItem(string name, EntryType type, ControlScreen screen)
        {
            MenuEntry entry = new MenuEntry(screen, name, type, screen, bgSprite);
            menuEntries.Add(entry);
        }

        public void AddSavedSwarm(string name, EntryType type, List<Color> colors, ControlScreen screen)
        {
            SavedSwarmButton entry = new SavedSwarmButton(screen, name, type, colors, screen, bgSprite);
            entry.Initialize();
            menuEntries.Add(entry);
        }

        public void ClearMenuEntries()
        {
            menuEntries.Clear();
        }

        private int GetMenuEntryAt(Vector2 position)
        {
            int index = 0;
            foreach (MenuEntry entry in menuEntries)
            {
                float width = entry.GetWidth();
                float height = entry.GetHeight();
                Rectangle rect = new Rectangle((int)(entry.Position.X),
                                               (int)(entry.Position.Y),
                                               (int)width, (int)height);
                if (rect.Contains((int)position.X, (int)position.Y) && entry.Alpha > 0.1f)
                {
                    return index;
                }
                //debugScreen.AddDebugItem("CURSOR", (int)position.X + " " + (int)position.Y, XnaSwarmsData.Debug.DebugFlagType.Important);

                ++index;
            }
            return -1;
        }

        public void HandleInput(ScreenSystem.ScreenSystem.InputHelper input, GameTime gameTime)
        {
            // Mouse or touch on a menu item
            int hoverIndex = GetMenuEntryAt(input.Cursor);
            if (hoverIndex >= 0)
            {
                selectedEntry = hoverIndex;
                //debugScreen.AddDebugItem("BUTTON HOVER", "Index " + hoverIndex, XnaSwarmsData.Debug.DebugFlagType.Important);
            }
            else
            {
                selectedEntry = -1;
            }

            // Accept or cancel the menu? 
            if (input.IsMenuSelect() && selectedEntry != -1)
            {
                if (menuEntries[selectedEntry].IsStable())
                {
                    controlClient.CreateStableSwarm();
                }
                else if (menuEntries[selectedEntry].IsMutationGame())
                {
                    controlClient.CreateMutationSwarm();
                }
                else if (menuEntries[selectedEntry].IsZoomIn())
                {
                    controlClient.ZoomIn();
                }
                else if (menuEntries[selectedEntry].IsZoomOut())
                {
                    controlClient.ZoomOut();
                }
                else if (menuEntries[selectedEntry].IsDebugger())
                {
                    debugScreen.SetVisiblity();
                }
                else if (menuEntries[selectedEntry].IsSave())
                {
                    SaveSwarm(controlClient.SaveWorld());
                    UpdateLikedItemsUI();
                }
                else if (menuEntries[selectedEntry].IsRecall1())
                {
                    allLikedItems[0].SaveWorldParameters = controlClient.SaveWorld();
                    controlClient.UpdatePopulation(allLikedItems[0], false);
                }
                else if (menuEntries[selectedEntry].IsRecall2())
                {
                    allLikedItems[1].SaveWorldParameters = controlClient.SaveWorld();
                    controlClient.UpdatePopulation(allLikedItems[1], false);
                }
                else if (menuEntries[selectedEntry].IsRecall3())
                {
                    allLikedItems[2].SaveWorldParameters = controlClient.SaveWorld();
                    controlClient.UpdatePopulation(allLikedItems[2], false);
                }
                else if (menuEntries[selectedEntry].IsRecall4())
                {
                    allLikedItems[3].SaveWorldParameters = controlClient.SaveWorld();
                    controlClient.UpdatePopulation(allLikedItems[3], false);
                }
                else if (menuEntries[selectedEntry].IsRecall5())
                {
                    allLikedItems[4].SaveWorldParameters = controlClient.SaveWorld();
                    controlClient.UpdatePopulation(allLikedItems[4], false);
                }
                else if (menuEntries[selectedEntry].IsRecall6())
                {
                    allLikedItems[5].SaveWorldParameters = controlClient.SaveWorld();
                    controlClient.UpdatePopulation(allLikedItems[5], false);
                }
                else if (menuEntries[selectedEntry].IsAudioPlay())
                {
                    controlClient.StartSoundEngine();
                }
                else if (menuEntries[selectedEntry].IsAudioPause())
                {
                    controlClient.StopSoundEngine();
                }
                else if (menuEntries[selectedEntry].IsAnalyze())
                {
                    controlClient.ToggleAnalyze();
                }

#if NETFX_CORE
                else if (menuEntries[selectedEntry].IsImportLikes())
                {
                    ImportSwarmSaveData();
                }
                else if (menuEntries[selectedEntry].IsExportLikes())
                {
                    ExportSwarmSaveData();
                }
#endif

            }
        }
    }
}

