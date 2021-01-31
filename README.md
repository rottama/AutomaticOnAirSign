# An automatic "On Air" sign

The room in which we are working from home is often just a repurposed room - and the people you share your living space with might still need infrequent access. But is it OK to enter right now? To eliminate any guesswork before entering your makeshift office, and to make life easier for everyone involved, here's an automated sign that lights up automatically when you're in a meeting call.

## Shopping list

- a light sign: https://www.amazon.de/gp/product/B01N77AGLI
- a WiFi-enabled power socket that has the open source firmware Tasmota pre-installed: https://www.amazon.de/gp/product/B07SNGJ8GD
- a desktop app that knows if you're on a call and toggles the sign accordingly (_IsTheMicInUse.exe_).

## Installation

1. Install your sign and WiFi socket in your preferred location.
2. Configure the WiFi socket to connect to your network as per these instructions: https://tasmota.github.io/docs/Getting-Started/#initial-configuration
3. Build the _IstheMicInuse_ app from the source code in this repository, or use the precompiled executables from the latest [IsTheMicInUse Release]().
   - Edit the _IsTheMicInUse.config_ file and set the parameter _TasmotaHostname_  to the name of your WiFi socket obtained from step 2 (e.g. _delock-2759_).
   - Right-click _IstheMicInUse.exe_ and select _Properties_ -> _Unblock_ because you downloaded the executable from the internet.
4. Try it out
   - Run _IstheMicInUse.exe_.
   - Call someone or use the Windows Voice Recorder App.
   - The Tasmota WiFi socket will switch on the light for the duration of the call.
 
## A word about the program logic

Just how do you determine if you are on a call? The diversity of communication devices and channels make this question impossible to answer. It depends on your use case. You may want to roll your own logic, deriving from presence indicators, calendar sync or other sources.

However, I found that the best indicator is the microphone: It's real-time, and most teleconferencing is done via PC. _IsTheMicInUse.exe_ only needs to check if a microphone is currently in use. This solution works across all conferencing software, both private and company-supplied. Best of all, when you mute your microphone during the call, the sign stays on. It will turn off only after you have exited the call.
