from colorama import init
from colorama import Fore, Back, Style

init()

def printError(str1):
    print(getattr(Fore, 'RED') + str1 + Fore.WHITE)

def printSuccess(str1):
    print(getattr(Fore, 'GREEN') + str1 + Fore.WHITE)

def printWarning(str1):
    print(getattr(Fore, 'YELLOW') + str1 + Fore.WHITE)

def printInfo(str1):
    print(getattr(Fore, 'MAGENTA') + str1 + Fore.WHITE)

def printPrompt(str1):
    print(getattr(Fore, 'CYAN') + str1 + Fore.WHITE, end='')

def printUserWaitPrompt(str1):
    print(getattr(Fore, 'CYAN') + str1 + Fore.WHITE, end='')

def printRoute(str1):
    print(getattr(Fore, 'RED') + str1 + Fore.WHITE)

def printPath(str1):
    print(getattr(Fore, 'YELLOW') + str1 + Fore.WHITE)

def getUserInput(prompt):
    printPrompt(prompt)
    return input()

def waitForUserInput(prompt):
    printUserWaitPrompt(prompt)
    return input()    