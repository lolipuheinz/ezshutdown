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

# Wenn eine spanne fehlt zb es gibt nur y und m dann wird automatisch addiert vom programm also eig sehr simpel

# Es wird aber relativ schwer mit diesem 1h 30m einf lose. Es geht schon, oder lazy fix wäre einfach "1h 30m" in quotes

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

def convert_args_to_time(usr_input: Any) -> int:
    # takes sum like 1h 30m and converts into seconds
    # regex??

    alert = 0
    # if all regex inputs fail to find anything 
    # (user likely did not understand the syntax)
    # the program will terminate

    years_search = re.search(r"^(\d+)y", usr_input)
    if not years_search:
        alert += 1
        years_time = 0
    else:
        years_time = int(years_search.group(1))
        years_time *= 365 * 24 * 60 * 60

    months_search = re.search(r"^(\d+)mo", usr_input)
    if not months_search:
        months_time = 0
    else:
        months_time = int(months_search.group(1))
        months_time *= 30 * 24 * 60 * 60

    weeks_search = re.search(r"^(\d+)w", usr_input)
    if not weeks_search:
        weeks_time = 0
    else:
        weeks_time = int(weeks_search.group(1))
        weeks_time *= 7 * 24 * 60 * 60

    days_search = re.search(r"^(\d+)d", usr_input)
    if not days_search:
        days_time = 0
    else:
        days_time = int(days_search.group(1))
        days_time *= 24 * 60 * 60

    hours_search = re.search(r"^(\d+)h", usr_input)
    if not hours_search:
        hours_time = 0
    else:
        hours_time = int(hours_search.group(1))
        hours_time *= 60 * 60

    minutes_search = re.search(r"^(\d+)m$", usr_input)
    # $ needed to not overlap with the month syntax
    if not minutes_search:
        minutes_time = 0
    else:
        minutes_time = int(minutes_search.group(1))
        minutes_time *= 60

    seconds_search = re.search(r"^(\d+)s", usr_input)
    if not seconds_search:
        seconds_time = 0
    else:
        seconds_time = int(seconds_search.group(1))

    times = [years_time, months_time, weeks_time, days_time, 
             hours_time, minutes_time, seconds_time]

    final_time_seconds = 0
    for i in times:
        final_time_seconds += i

    if final_time_seconds > 315360000:
        raise ValueError("The entered timespan exceeds the " \
        "limit of 10y / 315360000s and can not be used.")
        
    return final_time_seconds

    

































