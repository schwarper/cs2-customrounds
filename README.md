# cs2-customrounds
A custom round plugin

## Commands
```csharp
# css_voteround - Start voting for the next custom round

# css_{ShortCut} - Start custom round like "css_dr". You can set it in config.
```

## Images

![cs2-customrounds](https://media.discordapp.net/attachments/1210765210982424596/1210765211607502939/Adsz.png?ex=65ebbff9&is=65d94af9&hm=9dbdbec651caf4b1fa0db601c81affd4c273b2c96623ce953ab5469dae4c9e04&=&format=webp&quality=lossless&width=994&height=559)

![cs2-customrounds](https://media.discordapp.net/attachments/1210765210982424596/1210765212324724816/Adsz2.png?ex=65ebbff9&is=65d94af9&hm=a97555e6a134a7767259c1a9ac5b1abfb95bfebabac2efc76804fc83291e0abf&=&format=webp&quality=lossless&width=994&height=559)

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



