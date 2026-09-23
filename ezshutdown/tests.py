import argparse


parser = argparse.ArgumentParser()
parser.add_argument("timespan", help="Use a timespan, e.g. 2h 30m")
parser.add_argument("--force", help="forces shutdown without confirmation", action="store_true")
arg = parser.parse_args()

# https://docs.python.org/3/howto/argparse.html#argparse-tutorial was used

if arg.force:
    print("no confirmation")
else:
    print("confirmation needed!") 