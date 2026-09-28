#!/usr/bin/env python3
"""Import .unitypackage files into a Unity project by unpacking them.

A .unitypackage is a gzipped tar of <guid>/{asset,asset.meta,pathname} entries.
Writing `asset` to the path stored in `pathname` and `asset.meta` to
`pathname.meta` reproduces exactly what the Unity Editor's package importer
does, including GUID preservation.

Usage:
  python upkg_import.py --project "D:\\path\\to\\project" [--apply] pkg1.unitypackage ...
Without --apply it only reports (dry run).
"""
import argparse
import os
import sys
import tarfile

ASSET_KEYS = ("asset", "asset.meta")


def norm_path(raw):
    p = raw.decode("utf-8", "replace").replace("\ufeff", "")
    p = p.splitlines()[0].strip() if p.strip() else ""
    p = p.replace("\\", "/")
    while "//" in p:
        p = p.replace("//", "/")
    return p.lstrip("/")


def scan(pkg, project):
    """Stream the package once, returning per-package stats + planned writes."""
    stats = {
        "package": os.path.basename(pkg),
        "assets": 0,
        "folders": 0,
        "missing_meta": 0,
        "missing_meta_list": [],
        "outside_assets": [],
        "conflicts": [],
        "writes": [],  # (kind, src_bytes, dst_path)
        "toplevel": {},
    }
    buffered = {}
    current = None

    def flush(path, buf):
        if not path:
            return
        parts = path.split("/")
        is_assets = path == "Assets" or path.startswith("Assets/")
        # Embedded UPM package content lives under Packages/<name>/ and is part
        # of the project; Packages/manifest.json and packages-lock.json must be
        # left alone (Unity manages them).
        is_embedded = (len(parts) >= 2 and parts[0] == "Packages"
                       and parts[1] not in ("manifest.json", "packages-lock.json"))
        if not (is_assets or is_embedded):
            stats["outside_assets"].append(path)
            return
        is_folder = "asset" not in buf or len(buf.get("asset", b"")) == 0
        # strip trailing slash that folder entries sometimes carry
        clean = path.rstrip("/")
        if not clean:
            return
        dst = os.path.join(project, *clean.split("/"))
        if is_folder:
            stats["folders"] += 1
            if "asset.meta" in buf:
                stats["writes"].append(("meta", buf["asset.meta"], dst + ".meta"))
            else:
                stats["missing_meta"] += 1
                stats["missing_meta_list"].append(clean)
        else:
            stats["assets"] += 1
            existing = None
            if os.path.isfile(dst):
                with open(dst, "rb") as fh:
                    existing = fh.read()
            if existing is not None and existing != buf["asset"]:
                stats["conflicts"].append(clean)
            stats["writes"].append(("file", buf["asset"], dst))
            if "asset.meta" in buf:
                stats["writes"].append(("meta", buf["asset.meta"], dst + ".meta"))
            else:
                stats["missing_meta"] += 1
                stats["missing_meta_list"].append(clean)
        parts = clean.split("/")
        key = "/".join(parts[:3])
        stats["toplevel"][key] = stats["toplevel"].get(key, 0) + 1

    with tarfile.open(pkg, "r:gz") as tf:
        for member in tf:
            name = member.name
            if "/" not in name:
                continue
            guid, base = name.split("/", 1)
            if base == "pathname":
                raw = tf.extractfile(member).read() if member.isfile() else b""
                flush(norm_path(raw), buffered.get(guid, {}))
                buffered.pop(guid, None)
            elif base in ASSET_KEYS and member.isfile():
                buffered.setdefault(guid, {})[base] = tf.extractfile(member).read()
    return stats


def apply_writes(stats):
    written = 0
    for kind, data, dst in stats["writes"]:
        parent = os.path.dirname(dst)
        if parent and not os.path.isdir(parent):
            os.makedirs(parent)
        if kind == "meta" and os.path.isfile(dst):
            with open(dst, "rb") as fh:
                if fh.read() == data:
                    continue
        if kind == "file" and os.path.isfile(dst):
            with open(dst, "rb") as fh:
                if fh.read() == data:
                    continue
        with open(dst, "wb") as fh:
            fh.write(data)
        written += 1
    return written


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--project", required=True)
    ap.add_argument("--apply", action="store_true")
    ap.add_argument("packages", nargs="+")
    args = ap.parse_args()
    project = os.path.abspath(args.project)
    if not os.path.isdir(os.path.join(project, "Assets")):
        print("ERROR: %s has no Assets/ folder" % project)
        return 1

    print("PROJECT : %s" % project)
    print("MODE    : %s\n" % ("APPLY" if args.apply else "DRY RUN"))

    grand = {"assets": 0, "folders": 0, "outside": 0, "conflicts": 0, "missing_meta": 0, "written": 0}
    all_stats = []
    for pkg in args.packages:
        if not os.path.isfile(pkg):
            print("!! missing package: %s" % pkg)
            continue
        st = scan(pkg, project)
        all_stats.append(st)
        grand["assets"] += st["assets"]
        grand["folders"] += st["folders"]
        grand["outside"] += len(st["outside_assets"])
        grand["conflicts"] += len(st["conflicts"])
        grand["missing_meta"] += st["missing_meta"]
        print("=" * 78)
        print("%s" % st["package"])
        print("  assets=%d folders=%d missing_meta=%d outside_Assets=%d conflicts=%d"
              % (st["assets"], st["folders"], st["missing_meta"],
                 len(st["outside_assets"]), len(st["conflicts"])))
        tops = sorted(st["toplevel"].items(), key=lambda kv: -kv[1])[:6]
        print("  top targets: " + ", ".join("%s(%d)" % (k, v) for k, v in tops))
        for p in st["outside_assets"][:5]:
            print("    [outside Assets] %s" % p)
        for p in st["conflicts"][:10]:
            print("    [conflict] %s" % p)
        for p in st["missing_meta_list"][:5]:
            print("    [auto-meta] %s" % p)
        if args.apply:
            st["written"] = apply_writes(st)
            grand["written"] += st["written"]
            print("  -> written: %d files" % st["written"])

    print("\n" + "=" * 78)
    print("TOTAL assets=%d folders=%d outside_Assets=%d conflicts=%d missing_meta=%d%s"
          % (grand["assets"], grand["folders"], grand["outside"],
             grand["conflicts"], grand["missing_meta"],
             (" written=%d" % grand["written"]) if args.apply else ""))
    return 0


if __name__ == "__main__":
    sys.exit(main())
