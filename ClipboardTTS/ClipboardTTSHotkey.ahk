; I launch this program with an autohotkey.com script.

; Globals
global NoRapid = false

; Feel free to set these hotkeys to whatever you want.
; Kill Text-to-Speech with Shift+` or Shift+Pause
+`::
+Pause::
  KillTTS()
return

; Activate Text-to-Speech with ` or Pause
Pause::
`::
if (!NoRapid)
{
  global NoRapid = true
  SetTimer, NoRapidOff, 2000 ; Can't launch this more then once every 2 seconds.
  
  ; Check to see if process is running
  Process, Exist, ClipboardTTS.exe
  IF !errorlevel=0
  {
    KillTTS()
	sleep 200 ; Wait for Process to kill before continuing.
  }
  
  Run "ClipboardTTS.exe".
}
return

NoRapidOff:
  global NoRapid = false
  SetTimer, NoRapidOff, Off
return

KillTTS()
{
  run taskkill /IM ClipboardTTS.exe /F,, Hide
}