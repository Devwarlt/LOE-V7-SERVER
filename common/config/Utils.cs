using LoESoft.Core.models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LoESoft.Core.config
{
    public class GameVersion
    {
        public string Version { get; private set; }
        public bool Allowed { get; private set; }

        public GameVersion(
            string Version,
            bool Allowed
            )
        {
            this.Version = Version;
            this.Allowed = Allowed;
        }

        /// <summary>
        /// Max 5 supported versions.
        /// </summary>
        /// <returns></returns>
        public static List<GameVersion> SUPPORTED_VERSIONS() =>
            Settings.GAME_VERSIONS.Count >= 5 ?
            Settings.GAME_VERSIONS.Skip(Settings.GAME_VERSIONS.Count - 5).ToList() :
            Settings.GAME_VERSIONS;

        /// <summary>
        /// Returns only playable versions from supported versions.
        /// </summary>
        /// <returns></returns>
        public static List<GameVersion> PLAYABLE_VERSIONS() =>
            SUPPORTED_VERSIONS().Where(i => i.Allowed).ToList();

        /// <summary>
        /// Return latest supportable version to play (min version required).
        /// </summary>
        /// <returns></returns>
        public static GameVersion LatestSupportedVersion =>
            PLAYABLE_VERSIONS()[0];
    }

    public partial class Settings
    {
        public static string ProcessFile(string path) => $"_{path}_only.bat";

        public static IEnumerable<string> SplitToken(string s)
        {
            for (var i = 0; i < s.Length; i += 2)
                yield return s.Substring(i, Math.Min(2, s.Length - i));
        }

        public static byte[] ProcessToken(string token)
            => String.Join(",", SplitToken(token)).Split(',').Select(s => byte.Parse(s, NumberStyles.HexNumber)).ToArray();

        public static void DISPLAY_SUPPORTED_VERSIONS()
        {
            List<GameVersion> playableVersions = GameVersion.PLAYABLE_VERSIONS();
            List<GameVersion> supportedVersions = GameVersion.SUPPORTED_VERSIONS();

            Log.Info($"Game versions ({playableVersions.Count}/{supportedVersions.Count} available):");

            for (int i = supportedVersions.Count - 1; i >= 0; i--)
                Log.Info($"* [Access: {supportedVersions[i].Allowed}]\t{supportedVersions[i].Version}");
        }

        public static KeyValuePair<string, bool> CheckClientVersion(string build)
        {
            List<GameVersion> playableVersions = GameVersion.PLAYABLE_VERSIONS();

            foreach (GameVersion i in playableVersions)
                if (i.Version == build && i.Allowed)
                    return new KeyValuePair<string, bool>(build, true);

            return new KeyValuePair<string, bool>(GameVersion.LatestSupportedVersion.Version, false);
        }
    }
}