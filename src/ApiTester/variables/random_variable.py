from utils import line_to_dictionary
import random

class RandomVariable:
    def __init__(self,args):
        params = line_to_dictionary(args)
        print(params)
        min = params.get("min", None)
        if min != None:
            self.min = int(min)
        else:
            self.min = 0
        max = params.get("max", None)
        if max != None:
            self.max = int(max)
        else:
            self.max = 1000

    def eval(self):
        return random.randint(self.min, self.max)
