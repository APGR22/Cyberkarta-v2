import sys
import os
from pathlib import Path

cur_dir = os.getcwd()
dst_dir = sys.argv[1]

os.chdir(dst_dir)

output_dir = "output normalize BGM"

if not os.path.isdir(output_dir):
    os.mkdir(output_dir)

for data in os.listdir():
    if not os.path.isfile(data): continue
    if data.find(".meta") != -1: continue

    os.system(f"ffmpeg -i {data} -af loudnorm=I=-14:TP=-1:LRA=11 -c:a libvorbis -q:a 5 \"{output_dir}/{Path(data).stem}.ogg\"")

os.chdir(cur_dir)