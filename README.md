# Pear Desktop Controller — Macro Deck 2 Plugin

Control **Pear Desktop App** from your Macro Deck 2 setup. Features playback controls, volume management, and a **playlist switcher** where each button plays a different playlist.

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
| `pear_title` | String | Current track title |
| `pear_artist` | String | Current track artist |
| `pear_album` | String | Current track album |
| `pear_is_playing` | Bool | Whether music is playing |
| `pear_volume` | Integer | Current volume (0-100) |
| `pear_repeat_mode` | String | "None", "All", or "One" |
| `pear_is_shuffled` | Bool | Whether shuffle is on |

*(Note: Variables currently use the `pear_` prefix for legacy compatibility.)*

## Requirements

- **Pear Desktop App** ([download](https://github.com/pear-devs/pear-desktop))
- **Companion/API Server** enabled in Pear Desktop (Settings → Integrations → Enable Companion Server)

## Setup

1. **Build** the plugin (see below) and copy the output DLL to `%AppData%\Macro Deck\plugins\PearMacroDeck\`
2. Open Macro Deck 2
3. Find "Pear Desktop Controller" (or "Pear Desktop Controller") in the plugin list
4. Click **Configure**:
   - Set Host (default: `localhost`) and Port (default: `9863`)
   - Click **Authorize** and follow the prompts.
5. Add buttons with your desired actions
6. For playlist buttons: right-click → Configure → Fetch Playlists → Select one

## Building

```bash
dotnet build -c Release
```

> **Note:** The `Macro Deck 2.dll` reference path is dynamically configured, so it should build on any Windows machine out of the box!

## License

MIT

