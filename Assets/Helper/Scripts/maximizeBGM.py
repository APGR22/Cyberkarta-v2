import sys
import os
from pathlib import Path

cur_dir = os.getcwd()
dst_dir = sys.argv[1]

os.chdir(dst_dir)

output_dir = "output maximize BGM"

if not os.path.isdir(output_dir):
    os.mkdir(output_dir)

for data in os.listdir():
    if not os.path.isfile(data): continue
    if data.find(".meta") != -1: continue

    os.system(f"ffmpeg -i {data} -filter:a \"loudnorm=I=-13:TP=-0.5:LRA=8:measured_I=-16:measured_TP=-2:measured_LRA=9:measured_thresh=-30:offset=0.0\" -c:a libvorbis \"{output_dir}/{Path(data).stem}.ogg\"")

os.chdir(cur_dir)