# ClipboardTTS

Send the text in your Clipboard to Microsoft's Text-to-Speech Engine. Useful for Visual Novel Text Hookers that send foreign game text to the clipboard. Can be easily launched via an AutoHotkey script.

## Getting Started

On the first program run a "ClipboardTTS.settings" file will be created. It will list all your installed voices, you can uncomment the voice you want.

## Settings

Settings should be rather self explanitory. 

Lines that start with a semicolon are comments and are ignored. The first time you run the program, all the installed **voices** will be listed. Just uncomment the voice you want to use.

    ;Voice = "Microsoft David Desktop"
    ;Voice = "Microsoft Zira Desktop"
    Voice = "Microsoft Eva Mobile"

The **Volume** and **Rate** of Speech settings are simple numeric values.

    ;Volume 0...100
    Volume = 100
    
    ;Rate -10...10
    Rate = 0

**CharLimit** will limit the number of character's processed. Just in case you send it a book full of words and didn't actually want to listen to it all.

    ;Max Character Limit (0 = Unlimited)
    CharLimit = 500

The **Substitutions** section allows you to replace words before they are processed by the speech engine.

    [Substitutions]
    OriginalWord => ReplacementWord
