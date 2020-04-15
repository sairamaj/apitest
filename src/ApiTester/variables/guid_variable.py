from utils import line_to_dictionary
import uuid

class GuidVariable:
    def __init__(self,args):
        pass
    def eval(self):
        return str(uuid.uuid4())
