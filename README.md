# StreamLapse

Command line tools to extract one frame from rtsp stream each given interval using FFMPEG.

_Note: This tool was designed to capture the images and join them later to create an MP4 video. The join feature is not implemented yet._

## 💾 Download
You can download the latest release right here 👉🏻 [StreamLapse](https://github.com/fellypsantos/StreamLapse/releases)

## ℹ️ About

Project created when I decided to use my security cameras to create a timelapse video, this code uses [FFMPEG](https://github.com/FFmpeg/FFmpeg) behind the scenes to connect to an IP camera over rtsp (Real Time Streaming Protocol) and save the current frame as a JPG image file.

## ⌨️ Using the command line tool

Run the following command in your terminal prompt:

```sh
StreamLapse.exe --stream rtsp://10.0.0.5/stream1 --output FRONT_CAM --interval 10000
```

### ⚙️ Options:

These CLI can deal with some arguments listed below.

| Argument | Required? | Default | Description  |
|---|---|---|---|
| --stream   | YES |  | RTSP url to your IP camera. |
| --interval | NO  | 5000  | Time in milliseconds to wait before save another frame. |
| --output   | NO  |  current path | Path to store the frames, will be created if not exists. |
| --prefix   | NO  |  IMG_ | Prefix to use with filename, default results in IMG_1, IMG_2. |
| --help   | NO  |  | Display this arguments with descriptions. |
| --version   | NO  |  | Display version information. |


## 📜 License

Licensed under [GNU General Public License ](./LICENSE.txt).
