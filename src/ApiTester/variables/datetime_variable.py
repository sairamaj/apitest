import datetime
from datetime import date
from utils import line_to_dictionary

class DateVariable:
    def __init__(self,args):
        params = line_to_dictionary(args)
        print(params)
        self.format = params.get("format", None)
        days = params.get("days", None)
        if days != None:
            self.days = int(days)
        else:
            self.days = 0

    def eval(self):
        today = date.today()
        if self.days != None:
            today = today + datetime.timedelta(days=self.days)
        if self.format != None:
            today = today.strftime(self.format)
        else:
            today = today.strftime("%Y-%m-%d")
        return today

if __name__ == "__main__":
    print('just today date')
    print(DateVariable("").eval())
    print('just today date with different format')
    print(DateVariable("format=%Y-%m").eval())    
    print('future 10 days')
    print(DateVariable("days=10").eval())        
    print('past 10 days')
    print(DateVariable("days=-10").eval())            
    print('format and days')
    print(DateVariable("format=%Y-%Y-%m-%m-%d-%d days=50").eval())                