# TODO: Soll möglich sein mit ezshutdown [args], ezshut [args] und ezsh [args] äquivalent aufzurufen!

# TODO: ne flag die statt shutdown hibernate oder energy save macht das wäre next levellll
# ezshutdown (-s / -hib / -es / -rb) -time 18:00

# Skizze:

# eingaben sollen so möglich Sein

# ezshutdown -time 18:00

# am nächsten tag ist nicht möglich, zb

# ezshutdown -time 02:00 geht nicht mit time

# da braucht man dann date

# ezshutdown -time 18:00 -date today
# geht

# ezshutdown -time 02:00 -date tmwr
# geht

# ezshutdown -time 02:00 -date tomorrow
# geht

# also das ist einfach für easy nächsten tag

# für weitere spannen muss man angeben zb

# ezshutdown -time 02:00 -date YYYY-MM-DD und zwar genau so ISO dass es keine missverständnisse jemals geben kann
# ich will nicht in meinem mini projekt das 2000s dilemma 

# ! Windows kann max 315360000 (10 Jahre) ! muss also begrenzt werden

# -time kann immer auch ersetzt werden durch -t

# so und jetzt gibt es noch zeitspannen

# -timespan oder -ts

# ezshutdown -ts 5s geht
# ezshutdown -ts 5m geht
# ezshutdown -ts 100m geht
# ezshutdown -ts 100y geht nicht
# ezshutdown -ts -900s geht nicht
# ezshutdown -ts 1h 30m geht
# ezshutdown -ts 90m geht 
# ezshutdown -ts 0.5s geht nicht

# Es gibt keine kommazahlen, sowas wie 0.25y gibts nicht!!

# y ist groß aber erlaubt, wieso auch nicht?

# vorsicht mit 10y. off by one falle möglich.

# ts und time dürfen nicht gemixt werden!!

# Auf Kollisionen achten!! -h für hibernate geht zb nicht, denn -h ist help

# sowas wie 1h1h oder 5m 10m ist nicht erlaubt, not a bug but a feature

import re
import subprocess
from typing import Any

def main():
    ...

test_str = "1h 30m"

# TODO: Ins docstr muss rein, dass ein Monat auf 30d festgelegt ist!

# TODO: Außerdem in docstr: I am not in any way responsible for data loss or other damages caused by unexpected device shutdown.



# A single piece of the passed arguments like "30m".
# "mo" needs to be before "m", otherwise the regex would read e.g. "5mo" as "5m" and most likely break.
TOKEN = r"(\d+)(mo|y|w|d|h|m|s)"
 
# All timespan arguments must consist of these pieces plus whitespaces.
# Every invalid input ("abc", "0.5s", "-900s", "1h xyz") won't pass.
FULL_INPUT = rf"\s*(?:{TOKEN}\s*)+"

UNIT_SECONDS = {
    "y": 365 * 24 * 60 * 60,
    "mo": 30 * 24 * 60 * 60,
    "w": 7 * 24 * 60 * 60,
    "d": 24 * 60 * 60,
    "h": 60 * 60,
    "m": 60,
    "s": 1,
}

def convert_args_to_time(usr_input: str | list[str]) -> int:
    """Converts timespan arguments after "ts" to a time in seconds.
    
    If the input is invalid or a time unit is entered twice, SyntaxError will be raised. 
    If the resulting seconds would be too large, OverflowError will be raised.
    """
    # argparse returns a list like ["3h", "30m"] for "-ts 3h 30m".
    
    # "glues" together list elements for further processing
    if isinstance(usr_input, list):
        usr_input = " ".join(usr_input)
 
    # Checks whether the entire string is valid
    if not re.fullmatch(FULL_INPUT, usr_input):
        raise SyntaxError(f'Invalid syntax: "{usr_input}".')
 
    # Collects all pairs of (value, unit) in one scoop
    # "1y 5mo" turns into [("1","y"), ("5","mo")]
    pairs = re.findall(TOKEN, usr_input)
 
    # Reject double units like "1h 2h"
    units = [unit for _, unit in pairs]
    if len(units) != len(set(units)):
        raise SyntaxError(f'Same time unit found twice in "{usr_input}".')
 
    # sums up all parsed times
    total = sum(int(number) * UNIT_SECONDS[unit] for number, unit in pairs)
 
    # Checks whether the parsed total time is larger than 10 years (windows limit)
    if total > 315360000:
        raise OverflowError(f"Total time cannot be larger than 315360000 seconds.")
 
    return total
