using System;
using System.Linq;

namespace DotNetExtra {

    /// <summary>
    /// 文字列を Semantic Version 2.0.0 として解釈するパーサー。
    /// </summary>
    public interface ISemanticVersionParser {

        /// <summary>
        /// バージョン文字列を <see cref="SemanticVersion"/> に変換します。
        /// </summary>
        /// <param name="value">バージョン文字列。</param>
        /// <param name="result"><see cref="SemanticVersion"/> のインスタンス。</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> が <c>null</c> の場合。</exception>
        /// <exception cref="FormatException"><paramref name="value"/> の解釈に失敗した場合。</exception>
        SemanticVersion Parse(string value);

        /// <summary>
        /// バージョン文字列を <see cref="SemanticVersion"/> に変換します。
        /// </summary>
        /// <param name="value">バージョン文字列。</param>
        /// <param name="result"><see cref="SemanticVersion"/> のインスタンス。変換に失敗した場合は <c>null</c>。</param>
        /// <returns>変換に成功すれば <c>true</c>、それ以外なら <c>false</c>。</returns>
        bool TryParse(string value, out SemanticVersion result);
    }

    public abstract class SemanticVersionParserBase : ISemanticVersionParser {

        public SemanticVersion Parse(string value) {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }

            return TryParse(value, out var result) ? result : throw new FormatException();
        }

        public abstract bool TryParse(string value, out SemanticVersion result);
    }

    /// <summary>
    /// <see cref="ISemanticVersionParser"/> のリファレンス実装。
    /// </summary>
    public class SemanticVersionParser : SemanticVersionParserBase {
        public static readonly SemanticVersionParser Default = new SemanticVersionParser();

        private static readonly char[] s_versionSeparators = new[] { SemanticVersion.VersionSeparator };
        private static readonly char[] s_preReleaseIdSeparators = new[] { SemanticVersion.PreReleaseIdSeparator };
        private static readonly char[] s_buildMetadataSeparators = new[] { SemanticVersion.BuildMetadataSeparator };

        public override bool TryParse(string value, out SemanticVersion result) {
            result = InternalTryParse();
            return (result != null);

            SemanticVersion InternalTryParse() {
                value = value?.Trim();
                if (string.IsNullOrEmpty(value)) return null;

                var elems = value.Split(s_buildMetadataSeparators, 2);
                var buildMetadata = elems.ElementAtOrDefault(1);
                if (buildMetadata == "") return null;

                elems = elems[0].Split(s_preReleaseIdSeparators, 2);
                var preReleaseId = elems.ElementAtOrDefault(1);
                if (preReleaseId == "") return null;

                var versions = elems[0].Split(s_versionSeparators).ToArray();
                if (versions.Length != 3) return null;

                if (byte.TryParse(versions[0], out var major) == false) return null;
                if (byte.TryParse(versions[1], out var minor) == false) return null;
                if (byte.TryParse(versions[2], out var patch) == false) return null;

                return new SemanticVersion(major, minor, patch, preReleaseId, buildMetadata);
            }
        }
    }
}