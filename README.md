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
```

### Json
```json
{
    "default_ct_weapons": [ "knife", "deagle", "awp" ],
    "default_t_weapons": [ "knife", "deagle", "awp" ],
    "round_end_cmd": "sv_gravity 800",
    "rounds": {
        "1": {
            "Name": "[Hs] Deagle Round",
            "Weapons": [
                "deagle"
            ],
            "Shortcut": "dr",
            "CenterMsg": "lang_specialround",
            "OnlyHeadshot": true
        },
        "2": {
            "Name": "[No Scope] Awp Round",
            "Weapons": [
                "awp"
            ],
            "Shortcut": "nr",
            "CenterMsg": "lang_specialround",
            "NoScope": true
        },
        "3": {
            "Name": "[Hs] AK47 Round",
            "Weapons": [
                "ak47"
            ],
            "Shortcut": "ar",
            "CenterMsg": "lang_specialround",
            "OnlyHeadshot": true
        },
        "4": {
            "Name": "Knife Round",
            "Weapons": [
                "knife"
            ],
            "Shortcut": "kr",
            "CenterMsg": "lang_specialround",
            "KnifeDamage": true,
            "Speed": 2
        },
        "5": {
            "Name": "Zeus",
            "Weapons": [
                "zeus"
            ],
            "Shortcut": "zr",
            "CenterMsg": "lang_specialround",
            "Speed": 2
        },
        "6": {
            "Name": "Glock Round",
            "Weapons": [
                "glock"
            ],
            "Shortcut": "gr",
            "CenterMsg": "lang_specialround"
        },
        "7": {
            "Name": "[Hs] M4A4 Round",
            "Weapons": [
                "m4a4"
            ],
            "Shortcut": "mr",
            "CenterMsg": "lang_specialround",
            "OnlyHeadshot": true
        },
        "8": {
            "Name": "[Hs] USP Round",
            "Weapons": [
                "usp"
            ],
            "Shortcut": "ur",
            "CenterMsg": "lang_specialround",
            "OnlyHeadshot": true
        },
        "9": {
            "Name": "SSG Round",
            "Weapons": [
                "ssg08"
            ],
            "Shortcut": "sr",
            "CenterMsg": "lang_specialround"
        }
    },
    "ConfigVersion": 1
}
```



