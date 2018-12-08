using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetExtra {

    /// <summary>
    /// Semantic Versioning 2.0.0 を表現するクラス。
    /// https://semver.org/
    /// </summary>
    public class SemanticVersion : IComparable<SemanticVersion>, IEquatable<SemanticVersion> {
        public const char VersionSeparator = '.';
        public const char PreReleaseIdSeparator = '-';
        public const char BuildMetadataSeparator = '+';

        public static readonly SemanticVersion Empty = new SemanticVersion(0, 0, 0, null, null);

        public SemanticVersion(byte major, byte minor, byte patch) : this(major, minor, patch, null, null) {
        }

        public SemanticVersion(byte major, byte minor, byte patch, string preReleaseId, string buildMetadata) {
            Major = major;
            Minor = minor;
            Patch = patch;
            PreReleaseId = (preReleaseId == "") ? null : preReleaseId;
            BuildMetadata = (buildMetadata == "") ? null : buildMetadata;
        }

        /// <summary>
        /// メジャーバージョン。
        /// </summary>
        public byte Major { get; }

        /// <summary>
        /// マイナーバージョン。
        /// </summary>
        public byte Minor { get; }

        /// <summary>
        /// パッチバージョン。
        /// </summary>
        public byte Patch { get; }

        /// <summary>
        /// プレリリース識別子。
        /// </summary>
        public string PreReleaseId { get; }

        /// <summary>
        /// ビルドメタデータ。
        /// </summary>
        public string BuildMetadata { get; }

        public override string ToString() {
            var bldr = new StringBuilder($"{Major}.{Minor}.{Patch}");
            if (string.IsNullOrEmpty(PreReleaseId) == false) {
                bldr.Append(PreReleaseIdSeparator);
                bldr.Append(PreReleaseId);
            }
            if (string.IsNullOrEmpty(BuildMetadata) == false) {
                bldr.Append(BuildMetadataSeparator);
                bldr.Append(BuildMetadata);
            }
            return bldr.ToString();
        }

        public int CompareTo(SemanticVersion other) {
            if (other == null) return 1;

            int diff;
            if ((diff = Major.CompareTo(other.Major)) != 0) return diff;
            if ((diff = Minor.CompareTo(other.Minor)) != 0) return diff;
            if ((diff = Patch.CompareTo(other.Patch)) != 0) return diff;
            return -string.CompareOrdinal(PreReleaseId, other.PreReleaseId);
        }

        public override bool Equals(object obj) {
            return Equals(obj as SemanticVersion);
        }

        public bool Equals(SemanticVersion other) {
            return other != null
                && Major == other.Major
                && Minor == other.Minor
                && Patch == other.Patch
                && PreReleaseId == other.PreReleaseId
                && BuildMetadata == other.BuildMetadata
                ;
        }

        public override int GetHashCode() {
            var hashCode = 1697728969;
            hashCode = hashCode * -1521134295 + Major.GetHashCode();
            hashCode = hashCode * -1521134295 + Minor.GetHashCode();
            hashCode = hashCode * -1521134295 + Patch.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PreReleaseId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BuildMetadata);
            return hashCode;
        }
    }
}