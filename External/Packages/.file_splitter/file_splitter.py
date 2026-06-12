#!/usr/bin/env python3
"""
File Splitter & Merger
- Split: scans a directory, splits files > max_size into chunks, each in its own folder
- Merge: reverses the split, reassembling chunks back to original files
"""

import argparse
import os
import shutil
import sys
import json
import hashlib
from pathlib import Path

MANIFEST_NAME = ".split_manifest.json"


def checksum(filepath):
    """SHA-256 of a file, used to verify merge integrity."""
    h = hashlib.sha256()
    with open(filepath, "rb") as f:
        for chunk in iter(lambda: f.read(8192), b""):
            h.update(chunk)
    return h.hexdigest()


def split_file(filepath, max_size):
    """Split a single file into chunks of max_size bytes. Returns list of chunk paths."""
    fsize = os.path.getsize(filepath)
    if fsize <= max_size:
        return None  # no split needed

    orig = Path(filepath)
    out_dir = orig.parent / f"{orig.name}_split"
    out_dir.mkdir(exist_ok=True)

    chunks = []
    idx = 0
    with open(filepath, "rb") as f:
        while True:
            data = f.read(max_size)
            if not data:
                break
            chunk_name = f"{orig.name}.part{idx:04d}"
            chunk_path = out_dir / chunk_name
            with open(chunk_path, "wb") as cf:
                cf.write(data)
            chunks.append(str(chunk_path))
            idx += 1

    # Write manifest
    manifest = {
        "original_file": str(orig),
        "original_name": orig.name,
        "original_size": fsize,
        "original_checksum": checksum(filepath),
        "chunk_size": max_size,
        "chunk_count": idx,
        "chunks": [os.path.basename(c) for c in chunks],
    }
    with open(out_dir / MANIFEST_NAME, "w") as mf:
        json.dump(manifest, mf, indent=2)

    # Delete original
    os.remove(filepath)
    print(f"    Deleted original: {filepath}")

    return chunks


def merge_file(split_dir):
    """Merge chunks in a split directory back to the original file."""
    sp = Path(split_dir)
    manifest_path = sp / MANIFEST_NAME
    if not manifest_path.exists():
        print(f"  [SKIP] {split_dir} — no {MANIFEST_NAME} found")
        return None

    with open(manifest_path) as mf:
        manifest = json.load(mf)

    output_path = sp.parent / manifest["original_name"]
    chunks = [sp / c for c in manifest["chunks"]]

    # Verify all chunks exist
    for c in chunks:
        if not c.exists():
            print(f"  [ERROR] Missing chunk: {c}")
            return None

    print(f"  Merging {len(chunks)} chunks → {output_path}")
    with open(output_path, "wb") as of:
        for cp in chunks:
            with open(cp, "rb") as cf:
                of.write(cf.read())

    # Verify
    if checksum(output_path) == manifest["original_checksum"]:
        print(f"  [OK] Checksum verified — {output_path}")
        shutil.rmtree(sp)
        print(f"    Deleted split folder: {sp}")
    else:
        print(f"  [WARN] Checksum mismatch — {output_path} may be corrupted (split folder kept)")

    return str(output_path)


def cmd_split(args):
    target = Path(args.directory).resolve()
    if not target.is_dir():
        print(f"Error: '{target}' is not a valid directory.")
        sys.exit(1)

    print(f"\n>>> SPLIT MODE — scanning: {target}")
    print(f"    Threshold: {args.max_size / (1024*1024):.0f} MB\n")

    files = [f for f in target.rglob("*") if f.is_file() and f.name != MANIFEST_NAME]
    total = 0
    for fpath in files:
        size_mb = os.path.getsize(fpath) / (1024 * 1024)
        if os.path.getsize(fpath) > args.max_size:
            print(f"  Splitting: {fpath} ({size_mb:.1f} MB)")
            split_file(str(fpath), args.max_size)
            total += 1

    if total == 0:
        print(f"No files larger than {args.max_size / (1024*1024):.0f} MB found.")
    else:
        print(f"\nDone. {total} file(s) split.")


def cmd_merge(args):
    target = Path(args.directory).resolve()
    if not target.is_dir():
        print(f"Error: '{target}' is not a valid directory.")
        sys.exit(1)

    print(f"\n>>> MERGE MODE — scanning: {target}\n")

    split_dirs = [d for d in target.rglob("*_split") if d.is_dir() and (d / MANIFEST_NAME).exists()]
    if not split_dirs:
        print("No *_split folders found in this directory.")
        return

    for sd in sorted(split_dirs):
        print(f"  Merging: {sd}")
        merge_file(str(sd))
        # Clean up: delete split folder after successful merge
        if (sd / MANIFEST_NAME).exists():
            continue  # keep if manifest still there (merge may have been partial)
        # Cleanup handled inside merge_file now

    print("\nDone.")


def interactive():
    """Prompt-based interactive mode — no CLI args needed."""
    print("=" * 50)
    print("  File Splitter & Merger")
    print("=" * 50)
    print()
    print("1) Split  — split large files into chunks")
    print("2) Merge  — reassemble chunks back to original files")
    print()
    choice = input("Choose operation [1/2]: ").strip()

    if choice not in ("1", "2"):
        print("Invalid choice.")
        sys.exit(1)

    target = input("Directory [parent dir]: ").strip()
    if not target:
        target = ".."

    if choice == "1":
        mb = input("Max file size in MB [40]: ").strip()
        mb = float(mb) if mb else 40.0
        max_size = int(mb * 1024 * 1024)

        class Args:
            pass
        args = Args()
        args.directory = target
        args.max_size = max_size
        cmd_split(args)
    else:
        class Args:
            pass
        args = Args()
        args.directory = target
        cmd_merge(args)

    input("\nPress Enter to exit...")


def main():
    if len(sys.argv) > 1:
        # CLI mode
        parser = argparse.ArgumentParser(description="Split large files into chunks, or merge them back.")
        sub = parser.add_subparsers(dest="command")

        sp = sub.add_parser("split", help="Split files larger than max-size")
        sp.add_argument("directory", help="Target directory to scan")
        sp.add_argument("-s", "--max-size", type=int, default=40 * 1024 * 1024,
                        help="Max file size in bytes (default: 41943040 = 40 MB)")
        sp.add_argument("-m", "--megabytes", type=float,
                        help="Max file size in MB (overrides --max-size if set)")

        mp = sub.add_parser("merge", help="Merge split files back to originals")
        mp.add_argument("directory", help="Directory to scan for split folders")

        args = parser.parse_args()

        if args.command == "split":
            if args.megabytes:
                args.max_size = int(args.megabytes * 1024 * 1024)
            cmd_split(args)
        elif args.command == "merge":
            cmd_merge(args)
        else:
            parser.print_help()
    else:
        # Interactive mode
        interactive()


if __name__ == "__main__":
    main()
