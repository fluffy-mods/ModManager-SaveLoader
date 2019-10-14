// SaveLoader.cs
// Copyright Karel Kroeze, 2019-2019

using System;
using System.ComponentModel;
using System.IO;
using Verse;

namespace SaveLoader
{
    public class SaveLoader : Mod
    {
        public SaveLoader( ModContentPack mod ) : base( mod )
        {
            if ( GenCommandLine.TryGetCommandLineArg( "save", out var saveName ) )
                InitiateSaveLoading( saveName );
        }

        // lifted wholesale from HugsLib, with minor alterations
        public static void InitiateSaveLoading( string saveName )
        {
            saveName = saveName.Trim( '"', '\'' );

            Log.Message( "Starting saved game: " + saveName );

            if ( saveName == null )
            {
                throw new WarningException( "save filename not set" );
            }

            var filePath = GenFilePaths.FilePathForSavedGame( saveName );
            if ( !File.Exists( filePath ) )
            {
                throw new WarningException( "save file not found: " + filePath );
            }

            // skip mod/version checks, we're force loading here
            // (this also neatly hides the fact that we got weird bugs when nesting queued events)
            LongEventHandler.QueueLongEvent( delegate
            {
                Current.Game = new Game {InitData = new GameInitData {gameToLoad = saveName}};
            }, "Play", "LoadingLongEvent", true, (err) => Log.Error( err.ToString() ) );
        }
    }
}