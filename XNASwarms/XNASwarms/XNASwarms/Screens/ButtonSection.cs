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

namespace XNASwarms
{
    public sealed class ButtonSection : IDisposable
    {

        private SwarmScreenBase screen;
        private Vector2 position;
        private Texture2D bgSprite;
        private Rectangle rect, innerRect;
        private string description;
        private int selectedEntry;
        private SpriteFont LabelFont, BigFont;
        SpriteBatch spriteBatch;
       
        private List<MenuEntry> menuEntries = new List<MenuEntry>();

        private readonly Vector2 _containerMargin = new Vector2(10, 70);
        private readonly Vector2 _containerPadding = new Vector2(12, 12);

        private readonly Color _containerBGColor = new Color(30, 30, 30, 100);
        private readonly Color _BorderColor = new Color(30, 30, 30, 100);

        private readonly int BorderThickness = 4;
        private IDebugScreen debugScreen;
        private IControlClient controlClient;

        SaveAllSpecies allSaved;


        public ButtonSection(bool flip, SwarmScreenBase swarmscreen, string desc)
        {
            rect.Width = 100;
            rect.Height = 360;
            screen = swarmscreen;
            innerRect.Width = rect.Width - BorderThickness;
            innerRect.Height = rect.Height - BorderThickness;
            description = desc;
            allSaved = new SaveAllSpecies();

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
            if (allSaved == null)
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
            LoadSavedSwarms();
#endif
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

        private void LoadSavedSwarms()
        {
#if WINDOWS
            SaveAllSpecies allSaved = SaveHelper.Load("AllSaved");
#endif
            menuEntries.RemoveAll(s => s.GetType() == typeof(SavedSwarmButton));
            if (allSaved != null)
            {
                if (allSaved.Count == 1)
                {
                    AddSavedSwarm(allSaved[0].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall1,allSaved[0].GetMostUsedColors(), screen);
                }
                else if (allSaved.Count == 2)
                {
                    AddSavedSwarm(allSaved[0].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall1, allSaved[0].GetMostUsedColors(), screen);
                    AddSavedSwarm(allSaved[1].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall2, allSaved[1].GetMostUsedColors(), screen);
                }
                else if (allSaved.Count == 3)
                {
                    AddSavedSwarm(allSaved[0].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall1, allSaved[0].GetMostUsedColors(), screen);
                    AddSavedSwarm(allSaved[1].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall2, allSaved[1].GetMostUsedColors(), screen);
                    AddSavedSwarm(allSaved[2].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall3, allSaved[2].GetMostUsedColors(), screen);
                }
                else if (allSaved.Count == 4)
                {
                    AddSavedSwarm(allSaved[0].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall1, allSaved[0].GetMostUsedColors(), screen);
                    AddSavedSwarm(allSaved[1].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall2, allSaved[1].GetMostUsedColors(), screen);
                    AddSavedSwarm(allSaved[2].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall3, allSaved[2].GetMostUsedColors(), screen);
                    AddSavedSwarm(allSaved[3].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall4, allSaved[3].GetMostUsedColors(), screen);
                }
                else if (allSaved.Count == 5)
                {
                    AddSavedSwarm(allSaved[0].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall1, allSaved[0].GetMostUsedColors(), screen);
                    AddSavedSwarm(allSaved[1].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall2, allSaved[1].GetMostUsedColors(), screen);
                    AddSavedSwarm(allSaved[2].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall3, allSaved[2].GetMostUsedColors(), screen);
                    AddSavedSwarm(allSaved[3].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall4, allSaved[3].GetMostUsedColors(), screen);
                    AddSavedSwarm(allSaved[4].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall5, allSaved[4].GetMostUsedColors(), screen);
                }
                else if (allSaved.Count == 6)
                {
                    AddSavedSwarm(allSaved[0].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall1, allSaved[0].GetMostUsedColors(), screen);
                    AddSavedSwarm(allSaved[1].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall2, allSaved[1].GetMostUsedColors(), screen);
                    AddSavedSwarm(allSaved[2].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall3, allSaved[2].GetMostUsedColors(), screen);
                    AddSavedSwarm(allSaved[3].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall4, allSaved[3].GetMostUsedColors(), screen);
                    AddSavedSwarm(allSaved[4].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall5, allSaved[4].GetMostUsedColors(), screen);
                    AddSavedSwarm(allSaved[5].CreadtedDt.ToString("h:mm:ss"), EntryType.Recall6, allSaved[5].GetMostUsedColors(), screen);
                }
                
            }
        }

#if NETFX_CORE
        private async void GetLocalSaveSwarmData()
        {
            allSaved = await SaveHelper.LoadGameFile("AllSaved");
            LoadSavedSwarms();
        }

        private async void ImportSwarmSaveData()
        {
            var import = await ImportExportHelper.Import();
            if (import != null)
            {
                allSaved = import;
            }
            LoadSavedSwarms();
        }

        private async void ExportSwarmSaveData()
        {
            var export =  await ImportExportHelper.Export();
            if (export != null)
            {
                allSaved = export;
            }
            LoadSavedSwarms();
        }
#endif

        private void SaveSwarm()
        {
#if WINDOWS
            SaveAllSpecies allSaved = SaveHelper.Load("AllSaved");
#else

#endif
            if (allSaved != null && allSaved.Count() >= 6)
            {
                //Replacing
                SaveSpecies oldestSpecies = allSaved.OrderBy(s => s.CreadtedDt).First();
                allSaved.Remove(oldestSpecies);
                Save();
            }

            if (allSaved != null && allSaved.Count > 0)
            {
                if (allSaved.Count == 1)
                {
                    Save();
                }
                else if (allSaved.Count == 2)
                {
                    Save();
                }
                else if (allSaved.Count == 3)
                {
                    Save();
                }
                else if (allSaved.Count == 4)
                {
                    Save();
                }
                else if (allSaved.Count == 5)
                {
                    Save();
                }
            }
            else
            {
                Save();
            }
        }

        private void Save()
        {
            SaveSpecies savespecies = screen.GetPopulationAsSaveSpecies();
            savespecies.SaveWorldParameters = SaveWorld();
            allSaved.Add(savespecies);
            SaveHelper.Save("AllSaved", allSaved);

            foreach (var species in savespecies.SavedSpecies)
            {
                foreach (var indvd in species)
                {
                    debugScreen.AddDebugItem("INDVD X", indvd.x.ToString(), ScreenSystem.Debug.DebugFlagType.Important);
                }
            }
        }

        private SaveWorldParameters SaveWorld()
        {
            SaveWorldParameters world = new SaveWorldParameters();
            world.numberOfIndividualsMax = StaticWorldParameters.numberOfIndividualsMax;
            world.neighborhoodRadiusMax = StaticWorldParameters.neighborhoodRadiusMax;
            world.normalSpeedMax = StaticWorldParameters.normalSpeedMax;
            world.maxSpeedMax = StaticWorldParameters.maxSpeedMax;
            world.c1Max = StaticWorldParameters.CohesiveForceMax;
            world.c2Max = StaticWorldParameters.AligningForceMax;
            world.c3Max = StaticWorldParameters.SeperatingForceMax;
            world.c4Max = StaticWorldParameters.ChanceOfRandomSteeringMax;
            world.c5Max = StaticWorldParameters.TendencyOfPaceKeepingMax;
            return world;
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
                    SaveSwarm();
                    LoadSavedSwarms();
                }
                else if (menuEntries[selectedEntry].IsRecall1())
                {

                    allSaved[0].SaveWorldParameters = SaveWorld();
                    this.screen.UpdatePopulation(SaveSpeciesHelper.GetPopulationFromSaveSpecies(allSaved[0]), false);
                }
                else if (menuEntries[selectedEntry].IsRecall2())
                {
                    allSaved[1].SaveWorldParameters = SaveWorld();
                    this.screen.UpdatePopulation(SaveSpeciesHelper.GetPopulationFromSaveSpecies(allSaved[1]), false);
                }
                else if (menuEntries[selectedEntry].IsRecall3())
                {
                    allSaved[2].SaveWorldParameters = SaveWorld();
                    this.screen.UpdatePopulation(SaveSpeciesHelper.GetPopulationFromSaveSpecies(allSaved[2]), false);
                }
                else if (menuEntries[selectedEntry].IsRecall4())
                {
                    allSaved[3].SaveWorldParameters = SaveWorld();
                    this.screen.UpdatePopulation(SaveSpeciesHelper.GetPopulationFromSaveSpecies(allSaved[3]), false);
                }
                else if (menuEntries[selectedEntry].IsRecall5())
                {
                    allSaved[4].SaveWorldParameters = SaveWorld();
                    this.screen.UpdatePopulation(SaveSpeciesHelper.GetPopulationFromSaveSpecies(allSaved[4]), false);
                }
                else if (menuEntries[selectedEntry].IsRecall6())
                {
                    allSaved[5].SaveWorldParameters = SaveWorld();
                    this.screen.UpdatePopulation(SaveSpeciesHelper.GetPopulationFromSaveSpecies(allSaved[5]), false);
                }
                else if (menuEntries[selectedEntry].IsAudioPlay())
                {
#if WINDOWS
                        //SoundEngine.PlayPause(1);
                        SoundEngine.Play();
#endif
                }
                else if (menuEntries[selectedEntry].IsAudioPause())
                {
#if WINDOWS
                        //SoundEngine.PlayPause(0);
                        SoundEngine.Pause();
#endif
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

        public void Dispose()
        {
        }
    }
}

