# cs2-customrounds
A custom round plugin

## Commands
```csharp
# css_voteround - Start voting for the next custom round

# css_{ShortCut} - Start custom round like "css_dr". You can set it in config.
```

## Images

![cs2-customrounds](https://media.discordapp.net/attachments/1210765210982424596/1211110019907584020/Adsz.png?ex=65ed011a&is=65da8c1a&hm=1e5e04741aa94a1e4d70bf9a628e7f1fc3ea23e440db629307dcfa3a974070da&=&format=webp&quality=lossless))

![cs2-customrounds](https://media.discordapp.net/attachments/1210765210982424596/1211110020427817030/awp.png?ex=65ed011a&is=65da8c1a&hm=22cf44d3ec7d46d164d80bc8c8913ae8a8b991508721b23910b549e2164fcf33&=&format=webp&quality=lossless)

![cs2-customrounds](https://media.discordapp.net/attachments/1210765210982424596/1211110020914090024/deagle.png?ex=65ed011a&is=65da8c1a&hm=a9de07321bd3e9143978cc85c48f103adff8568821f9b9d3920666fd4eaecfcb&=&format=webp&quality=lossless)

## Setting rounds
You can set special rounds in the config file. You can get help from the config sample file I have given below.
CenterMsg text must be integrated into the lang file for example "lang_specialround"

### Adjustable features
```
Name
Weapons
Shortcut
OnlyHeadshot
KnifeDamage
NoScope
Health
Speed
Cmd
CenterMsg
NoBuy
```

### Json
```json
// This configuration was automatically generated by CounterStrikeSharp for plugin 'cs2-customrounds', at 2024.02.25 12:48:24
{
  "default_ct_weapons": [
    "knife",
    "deagle",
    "awp"
  ],
  "default_t_weapons": [
    "knife",
    "deagle",
    "awp"
  ],
  "round_end_cmd": "sv_gravity 800",
  "admin_flag": "@css/generic",
  "rounds": {
    "1": {
      "Name": "[Hs] Deagle Round",
      "Weapons": [
        "knife",
        "deagle"
      ],
      "Shortcut": "dr",
      "OnlyHeadshot": true,
      "KnifeDamage": false,
      "NoScope": false,
      "NoBuy": true,
      "Health": 100,
      "Speed": 1,
      "Cmd": "",
      "CenterMsg": "html_customround"
    },
    "2": {
      "Name": "[No Scope] Awp Round",
      "Weapons": [
        "knife",
        "awp"
      ],
      "Shortcut": "nr",
      "OnlyHeadshot": false,
      "KnifeDamage": false,
      "NoScope": true,
      "NoBuy": true,
      "Health": 100,
      "Speed": 1,
      "Cmd": "",
      "CenterMsg": "html_customround"
    },
    "3": {
      "Name": "[Hs] AK47 Round",
      "Weapons": [
        "knife",
        "ak47"
      ],
      "Shortcut": "ar",
      "OnlyHeadshot": true,
      "KnifeDamage": false,
      "NoScope": false,
      "NoBuy": true,
      "Health": 100,
      "Speed": 1,
      "Cmd": "",
      "CenterMsg": "html_customround"
    },
    "4": {
      "Name": "Knife Round",
      "Weapons": [
        "knife"
      ],
      "Shortcut": "kr",
      "OnlyHeadshot": false,
      "KnifeDamage": true,
      "NoScope": false,
      "NoBuy": true,
      "Health": 30,
      "Speed": 2,
      "Cmd": "",
      "CenterMsg": "html_customround"
    },
    "5": {
      "Name": "Zeus Round",
      "Weapons": [
        "zeus"
      ],
      "Shortcut": "zr",
      "OnlyHeadshot": false,
      "KnifeDamage": false,
      "NoScope": false,
      "NoBuy": true,
      "Health": 100,
      "Speed": 2,
      "Cmd": "mp_taser_recharge_time 0.1",
      "CenterMsg": "html_customround"
    },
    "6": {
      "Name": "Glock Round",
      "Weapons": [
        "knife",
        "glock"
      ],
      "Shortcut": "gr",
      "OnlyHeadshot": false,
      "KnifeDamage": false,
      "NoScope": false,
      "NoBuy": true,
      "Health": 100,
      "Speed": 1,
      "Cmd": "",
      "CenterMsg": "html_customround"
    },
    "7": {
      "Name": "[Hs] M4A4 Round",
      "Weapons": [
        "knife",
        "m4a4"
      ],
      "Shortcut": "mr",
      "OnlyHeadshot": true,
      "KnifeDamage": false,
      "NoScope": false,
      "NoBuy": true,
      "Health": 100,
      "Speed": 1,
      "Cmd": "",
      "CenterMsg": "html_customround"
    },
    "8": {
      "Name": "[Hs] USP Round",
      "Weapons": [
        "knife",
        "usp"
      ],
      "Shortcut": "ur",
      "OnlyHeadshot": true,
      "KnifeDamage": false,
      "NoScope": false,
      "NoBuy": true,
      "Health": 100,
      "Speed": 1,
      "Cmd": "",
      "CenterMsg": "html_customround"
    },
    "9": {
      "Name": "SSG Round",
      "Weapons": [
        "knife",
        "ssg08"
      ],
      "Shortcut": "sr",
      "OnlyHeadshot": false,
      "KnifeDamage": false,
      "NoScope": false,
      "NoBuy": true,
      "Health": 100,
      "Speed": 1,
      "Cmd": "sv_gravity 200",
      "CenterMsg": "html_customround"
    }
  },
  "ConfigVersion": 1
}
```



