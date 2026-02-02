# LudoWar - WebGL Browser Version 🎮

A 3D animated Ludo game built with Unity that runs in web browsers!

## 🎯 Features

- **3D Animated Warriors** - Play with medieval warriors instead of boring tokens
- **Multiple Game Modes**:
  - ⚔️ Play vs Computer - Challenge the AI
  - 👥 Pass & Play - Play with friends on the same device
- **12 Unique Characters** - Hero, Anika, Bjorn, Cassian, and more!
- **4 Player Colors** - Green, Yellow, Blue, Red
- **Beautiful Animations** - Walk, Run, Jump, Attack, Victory celebrations

## 🚀 How to Build for WebGL

### Prerequisites
- Unity 2021.3 LTS or newer
- WebGL Build Support module installed

### Build Steps

1. **Open the project in Unity**
   ```
   File > Open Project > Select LudoWar folder
   ```

2. **Switch to WebGL Platform**
   ```
   File > Build Settings > WebGL > Switch Platform
   ```

3. **Configure WebGL Settings**
   ```
   Edit > Project Settings > Player > WebGL tab
   - Memory Size: 512 MB (recommended)
   - Compression Format: Gzip
   ```

4. **Build**
   ```
   File > Build Settings > Build
   Select output folder: WebGLBuild/
   ```

5. **Test locally**
   ```bash
   cd WebGLBuild
   python -m http.server 8080
   # Open http://localhost:8080 in browser
   ```

## 🌐 Hosting

You can host the WebGL build on:
- **itch.io** (free)
- **GitHub Pages** (free)
- **Netlify** (free)
- **Your own server**

### Deploy to GitHub Pages

1. Create a new repository for the build
2. Copy contents of `WebGLBuild/` to the repository
3. Enable GitHub Pages in repository settings
4. Access at `https://yourusername.github.io/repo-name`

## 📝 WebGL Limitations

| Feature | Status | Notes |
|---------|--------|-------|
| Play vs Computer | ✅ Works | Full gameplay |
| Pass & Play | ✅ Works | Local multiplayer |
| Play Online | ❌ Disabled | Not supported in browser |
| Play with Friends | ❌ Disabled | Not supported in browser |
| Ads | ❌ Disabled | IronSource not available |
| Rewarded Videos | 🔄 Modified | Gives free coins instead |

## 🎮 Game Controls

- **Click/Tap** on dice to roll
- **Click/Tap** on your warrior to move

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── Managers/      # Game managers
│   ├── Network/       # Multiplayer (disabled in WebGL)
│   ├── Normal/        # Offline game logic
│   ├── Lobby/         # Lobby screens
│   └── Ads/           # Ad handlers (stubbed for WebGL)
├── Scenes/
│   ├── GameMenu.unity # Main menu
│   ├── Lobby.unity    # Player setup
│   └── LudoCastle.unity # Game board
└── Resources/
    └── Prefabs2/      # Character prefabs
```

## 🔧 Development

### Adding WebGL-specific code

Use preprocessor directives:
```csharp
#if UNITY_WEBGL
    // WebGL-specific code
#else
    // Mobile/Desktop code
#endif
```

## 📄 License

This project is for personal use.

## 🤝 Contributing

Feel free to submit issues and pull requests!
