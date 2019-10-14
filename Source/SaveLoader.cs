// SaveLoader.cs
// Copyright Karel Kroeze, 2019-2019

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using RimWorld;
using UnityEngine;
using Verse;

namespace SaveLoader
{
    public class SaveLoader : Verse.Mod
    {
        public SaveLoader( ModContentPack mod ) : base( mod )
        {
            if ( GenCommandLine.TryGetCommandLineArg( "save", out string saveName ) )
                InitiateSaveLoading( saveName );
        }

        // lifted wholesale from HugsLib, with minor alterations
        public static void InitiateSaveLoading( string saveName )
        {
            if ( saveName == null )
            {
                throw new WarningException( "save filename not set" );
            }

            var filePath = GenFilePaths.FilePathForSavedGame( saveName );
            if ( !File.Exists( filePath ) )
            {
                throw new WarningException( "save file not found: " + filePath );
            }

            Log.Message( "Starting saved game: " + saveName );
            Action loadAction = () =>
            {
                LongEventHandler.QueueLongEvent(
                    delegate { Current.Game = new Game {InitData = new GameInitData {gameToLoad = saveName}}; },
                    "Play", "LoadingLongEvent", true, null );
            };

            PreLoadUtility.CheckVersionAndLoad( filePath, ScribeMetaHeaderUtility.ScribeHeaderMode.Map, loadAction );
        }

    }
}