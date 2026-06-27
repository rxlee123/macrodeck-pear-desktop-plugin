# YTMD Controller — Macro Deck 2 Plugin

Control **YouTube Music Desktop App** from your Macro Deck 2 setup. Features playback controls, volume management, and a **playlist switcher** where each button plays a different playlist.

## Features

### Playback Controls
| Action | Description |
|--------|-------------|
| ▶️ Play / Pause | Toggle playback |
| ⏭ Next Track | Skip to the next track |
| ⏮ Previous Track | Go to the previous track |
| 🔊 Volume Up | Increase volume |
| 🔉 Volume Down | Decrease volume |
| 🔇 Mute / Unmute | Toggle mute |
| 🔀 Shuffle | Toggle shuffle mode |
| 🔁 Repeat Mode | Cycle: None → All → One |

### Playlist Switcher
| Action | Description |
|--------|-------------|
| 🎵 Play Playlist | Play a specific playlist (configurable per button) |

Add multiple "Play Playlist" buttons, each configured to a different playlist. Press a button to instantly switch to that playlist.

### Variables
The plugin exposes these Macro Deck variables, updated every 2 seconds:

| Variable | Type | Description |
|----------|------|-------------|
| `ytmd_title` | String | Current track title |
| `ytmd_artist` | String | Current track artist |
| `ytmd_album` | String | Current track album |
| `ytmd_is_playing` | Bool | Whether music is playing |
| `ytmd_volume` | Integer | Current volume (0-100) |
| `ytmd_repeat_mode` | String | "None", "All", or "One" |
| `ytmd_is_shuffled` | Bool | Whether shuffle is on |

## Requirements

- **YouTube Music Desktop App** v2.x or higher ([download](https://github.com/ytmdesktop/ytmdesktop))
- **Companion Server** enabled in YTMD (Settings → Integrations → Enable Companion Server)
- **Companion Authorization** enabled in YTMD

## Setup

1. **Build** the plugin (see below) and copy the output DLL to `%AppData%\Macro Deck\plugins\YTMDMacroDeck\`
2. Open Macro Deck 2
3. Find "YTMD Controller" in the plugin list
4. Click **Configure**:
   - Set Host (default: `localhost`) and Port (default: `9863`)
   - Click **Authorize** — a code will appear
   - In the YTMD app, approve the authorization request
5. Add buttons with your desired actions
6. For playlist buttons: right-click → Configure → Fetch Playlists → Select one

## Building

```bash
dotnet build -c Release
```

> **Note:** You must update the `Macro Deck 2.dll` reference path in `YTMDMacroDeck.csproj` to match your Macro Deck installation directory.

## License

MIT
